using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class GameVC : MonoBehaviour
{

    public GameObject Master; // reference to the master object
    private MasterController masterController; // refrence to master controller script on master object

    public int currentScore;
    private int Level;
    public GameObject Player;
    public GameObject RedScreen;
    public GameObject YellowScreen;
    public GameObject GreenScreen;
    public Text score;
    public TextMesh centerMessage;

    public GameObject RightHand;
    public GameObject LeftHand;

    PoseLoader PL = PoseLoader.Instance;
    float delta = 0;



    private Pose[] Poses; // poses for the current level
    private int currentPoseIndex = -1; // current pose index. -1 means game not started.
    int countDown = -1; // countdown for starting game



    // Start is called before the first frame update
    void Start()
    {

        // get controllers
        masterController = Master.GetComponent<MasterController>();

        //hardcoded score
        currentScore = 4569;
        // Start VR
        StartCoroutine(Utils.SetVRDevice("OpenVR", true));

        Level = Prefs.GetLevelID();
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

            if (currentPoseIndex != -1 && !PL.NextFrame()) {
                currentPoseIndex++;
                PL.SwitchPose(Poses[currentPoseIndex].Id);
            }
            masterController.UpdateMaster();
            delta = 0;
        }



    }


    void loadLevel()
    {
        var db = DataService.Instance.GetConnection();
        var levelQuery = db.Table<Level>()
            .Where(v => v.Id.Equals(Level));

        if (levelQuery.Count() != 1)
        {
            Debug.Log("Invalid level ID");
            // StatusLabel.text = "Error! Invalid level ID.";
            return;
        }

        Level currentLevel = levelQuery.First();
        // show level name on screen
        // currentLevel.Name

        Debug.Log("Al poses " + currentLevel.Poses.Split(','));


        string[] poses = currentLevel.Poses.Split(',');

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
        countDown = 10;
    }

    void updatePose()
    {

    }

    void HideAllOverlays()
    {
        RedScreen.SetActive(false);
        YellowScreen.SetActive(false);
        GreenScreen.SetActive(false);

    }


    void showNirvana()
    {   //shows yellow border if player is in nirvana state 
        if (Player.transform.position == new Vector3(1, 0, 0))
        {
            YellowScreen.SetActive(true);
        }
    }

    void showMoveInRange()
    {   //shows green border if player is in range of move 
        if (Player.transform.position == new Vector3(0, 0, 1))
        {
            GreenScreen.SetActive(true);
        }
    }

    void showMoveOffRange()
    {   //shows red border if player is out of range 
        if (Player.transform.position == new Vector3(0, 0, 0))
        {
            RedScreen.SetActive(true);
        }
    }

    // update chi meter for nirvana during the game
    void updateChiMeter(int score)
    {

    }



    // update current score in during game play
    void updateScore()
    {

        score.text = "Score: " + currentScore.ToString();

    }

    // saves score to database for leaderboard
    void saveScoreToLeaderboard()
    {

    }

    void endGame()
    {

    }
}
