﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class GameVC : MonoBehaviour
{

    public GameObject MasterHead;
    public GameObject MasterRight;
    public GameObject MasterLeft;

    public GameObject PlayerHead;
    public GameObject PlayerRight;
    public GameObject PlayerLeft;

    // relative to head
    public Vector3 MasterLeftV3 { get { return MasterHead.transform.position - MasterLeft.transform.position; } }
    public Vector3 MasterRightV3 { get { return MasterHead.transform.position - MasterRight.transform.position; } }

    public Vector3 PlayerLeftV3 { get { return PlayerHead.transform.position - PlayerLeft.transform.position; } }
    public Vector3 PlayerRightV3 { get { return PlayerHead.transform.position - PlayerRight.transform.position; } }

    public GameObject Master;
    private MasterController masterController;

    public int currentScore = 0;
    private int LevelID;
    // public GameObject Player;
    public GameObject RedScreen;
    public GameObject YellowScreen;
    public Text score;

    public Text PoseName;

    public TextMesh centerMessage;

    private Level CurrentLevel;

    PoseLoader PL = PoseLoader.Instance;
    float delta = 0;

    float lastAudioPlayed = 0;




    private Pose[] Poses; // poses for the current level
    private int currentPoseIndex = -1; // current pose index. -1 means game not started.
    int countDown = -1; // countdown for starting game



    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().StopMenu();

        AddScore(0);

        // get controllers
        masterController = Master.GetComponent<MasterController>();

        // Start VR
        StartCoroutine(Utils.SetVRDevice("OpenVR", true));

        LevelID = Prefs.GetLevelID();
        loadLevel();



        // Debug.Log(Prefs.GetLevelID());
        // Debug.Log(Prefs.GetPlayerName());

        InvokeRepeating("RunCountDown", 0, 1);
    }


    void RunCountDown()
    {
        countDown--;

        if (countDown == 0)
        {
            centerMessage.text = "Start";
            return;
        }
        else if (countDown == -1)
        {
            CancelInvoke("RunCountDown");
            centerMessage.text = "";
            currentPoseIndex = 0;
            PL.SwitchPose(Poses[currentPoseIndex].Id);
            return;
        }
        centerMessage.text = countDown.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        


        //HideAllOverlays();
        //showMoveOffRange();
        //showMoveInRange();
        //showNirvana();
        //updateScore();

        delta += Time.deltaTime;
        if (delta > 0.1)
        {
            HideAllOverlays();

            if (currentPoseIndex == -1) return;

            if (!PL.NextFrame()) {
                PL.SetNotLoaded();
                currentPoseIndex++;
                if (currentPoseIndex == Poses.Length)
                {
                    EndGame();
                    return;
                }
                PL.SwitchPose(Poses[currentPoseIndex].Id);
                PoseName.text = "Pose: " + Poses[currentPoseIndex].Name; 
            }
            masterController.UpdateMaster();
            delta = 0;


            Debug.Log("L " + Vector3.Distance(MasterLeftV3, PlayerLeftV3));
            Debug.Log("R " + Vector3.Distance(MasterRightV3, PlayerRightV3));


            if (Vector3.Distance(MasterLeftV3, PlayerLeftV3) < 0.2)
            {
                AddScore(1);
            }

            if (Vector3.Distance(MasterRightV3, PlayerRightV3) < 0.2)
            {
                AddScore(1);
            }

            PlayMoveAudio();
        }
    }

    void PlayMoveAudio()
    {
        if(lastAudioPlayed > 5)
        {

            
            lastAudioPlayed = 0;

            float total = Vector3.Distance(MasterLeftV3, PlayerLeftV3) + Vector3.Distance(MasterRightV3, PlayerRightV3);
            if (total > 0.5)
            {
                showMoveOffRange();
                FindObjectOfType<AudioManager>().play("fart2");
            }
            else if (total > 0.4)
            {
                showMoveOffRange();
                FindObjectOfType<AudioManager>().play("fart1");
            }
            else if (total > 0.3)
            {
                showNirvana();
                FindObjectOfType<AudioManager>().play("invalid");
            }
            else
            {
                FindObjectOfType<AudioManager>().play("valid");
            }
        }

        

        lastAudioPlayed += 0.1f;
    }

    void EndGame()
    {
        currentPoseIndex = -1;
        centerMessage.text = "Well Done! Your score: " + currentScore;

        var db = DataService.Instance.GetConnection();

        db.Insert(new Leaderboard()
        {
            Player_id = Player.GetOrCreatePlayer(Prefs.GetPlayerName()).Id,
            Level = LevelID,
            Score = currentScore
        });

        Invoke("ExitScene", 10);
    }

    void ExitScene()
    {
        FindObjectOfType<AudioManager>().PlayMenu();

        if (CurrentLevel.Mode == Level.MODE_SCORED)
        {
            SceneManager.LoadScene("Scored");
        }
        else
        {
            SceneManager.LoadScene("Training");
        }
    }

    void loadLevel()
    {
        var db = DataService.Instance.GetConnection();
        var levelQuery = db.Table<Level>()
            .Where(v => v.Id.Equals(LevelID));

        if (levelQuery.Count() != 1)
        {
            Debug.Log("Invalid level ID");
            // StatusLabel.text = "Error! Invalid level ID.";
            return;
        }

        CurrentLevel = levelQuery.First();
        // show level name on screen
        // currentLevel.Name

        Debug.Log("Al poses " + CurrentLevel.Poses.Split(','));


        string[] poses = CurrentLevel.Poses.Split(',');

        Poses = new Pose[poses.Length];

        for (int i = 0; i < poses.Length; i++)
        {
            int poseId = int.Parse(poses[i]);
            var poseQuery = db.Table<Pose>()
                .Where(v => v.Id.Equals(poseId)); // convert to int

            if (poseQuery.Count() != 1)
            {
                Debug.Log("Invalid Pose ID");
                // StatusLabel.text = "Error! Invalid level ID.";
                return;
            }
            Poses[i] = poseQuery.First();
        }
        countDown = 3;
    }

    void updatePose()
    {

    }

    void HideAllOverlays()
    {
        RedScreen.SetActive(false);
        YellowScreen.SetActive(false);

    }


    //shows yellow border if player is in nirvana state 
    void showNirvana()
    {
        YellowScreen.SetActive(true);
    }

    void showMoveInRange()
    {   //shows green border if player is in range of move 
    //    if (Player.transform.position == new Vector3(0, 0, 1))
    //    {
    //        GreenScreen.SetActive(true);
    //    }
    }

    //shows red border if player is out of range 
    void showMoveOffRange()
    {
        RedScreen.SetActive(true);
    }

    // update chi meter for nirvana during the game
    void updateChiMeter(int score)
    {

    }



    // update current score in during game play
    void AddScore(int increment)
    {
        currentScore += increment;
        score.text = "Score: " + currentScore.ToString();
    }

    // saves score to database for leaderboard
    void saveScoreToLeaderboard()
    {

    }
}
