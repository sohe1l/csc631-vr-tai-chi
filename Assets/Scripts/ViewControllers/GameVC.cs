using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameVC : MonoBehaviour
{

    private int currentScore;
    private int Level;
    public GameObject Player;
    public GameObject RedScreen;
    public GameObject YellowScreen;
    public GameObject GreenScreen;
    public Text score;


    // Start is called before the first frame update
    void Start()
    {
        //hardcoded score
        currentScore = 4569;
        // Start VR
        StartCoroutine(Utils.SetVRDevice("OpenVR", true));

        Level = Prefs.GetLevelID();
        
       

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
