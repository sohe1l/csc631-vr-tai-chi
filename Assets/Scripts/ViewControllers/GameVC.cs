using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameVC : MonoBehaviour
{
    private int currentScore;
    private int Level;
    public GameObject Player;
    public GameObject Master;
    public GameObject RedScreen;
    public GameObject YellowScreen;
    public GameObject GreenScreen;
    public Text score;


    private Pose[] Poses;


    // Start is called before the first frame update
    void Start()
    {
        //hardcoded score
        currentScore = 4569;
        // Start VR
        StartCoroutine(Utils.SetVRDevice("OpenVR", true));

        Level = Prefs.GetLevelID();
        loadLevel();
       

        // Debug.Log(Prefs.GetLevelID());
        // Debug.Log(Prefs.GetPlayerName());



    }

    // Update is called once per frame
    void Update()
    {
        HideAllOverlays();
        showMoveOffRange();
        showMoveInRange();
        showNirvana();
        updateScore();



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

        for(int i = 0; i < poses.Length; i++)
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
            Poses[i]  = poseQuery.First();
        }
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
        if (Player.transform.position == new Vector3(0,0,0))
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
