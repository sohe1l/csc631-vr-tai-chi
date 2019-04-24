using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameVC : MonoBehaviour
{

    private int currentScore;
    private int Level;
    public GameObject Player;
    public GameObject RedBorder;
    public GameObject YellowBorder;
    bool isCreated;

    public GameObject redScreen;


    // Start is called before the first frame update
    void Start()
    {

        // Start VR
        StartCoroutine(Utils.SetVRDevice("OpenVR", true));

        Level = Prefs.GetLevelID();

        // Debug.Log(Prefs.GetLevelID());
        // Debug.Log(Prefs.GetPlayerName());

        

    }

    // Update is called once per frame
    void Update()
    {
        showMoveOffRange();
        showNirvana();

        redScreen.SetActive(true);


    }


    void loadLevel()
    {

    }

    void updatePose()
    {

    }

    void HideAllOverlays()
    {
        
    }


    void showNirvana()
    {
        if (Player.transform.position == new Vector3(1, 0, 0) & !isCreated)
        {


            Instantiate(YellowBorder, new Vector3(0, 0, 0), Quaternion.identity);
            //Destroy(YellowBorder, 0.5f);
            isCreated = true;
        }
    }

    void showMoveInRange()
    {

    }

    void showMoveOffRange()
    {
        if(Player.transform.position == new Vector3(0,0,0) & !isCreated)
        {
            Instantiate(RedBorder, new Vector3(0, 0, 0), Quaternion.identity);
            //Destroy(RedBorder, 0.5f);
            isCreated = true;
        }
    }

    // update chi meter for nirvana during the game
    void updateChiMeter(int score)
    {

    }



    // update current score in during game play
    void updateScore()
    {

    }

    // saves score to database for leaderboard
    void saveScoreToLeaderboard()
    {

    }

    void endGame()
    {

    }
}
