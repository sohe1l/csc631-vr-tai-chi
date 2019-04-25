﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameVC : MonoBehaviour
{

    private int currentScore;
    private int Level;
    public GameObject Player;
    public GameObject redScreen;
    public GameObject YellowScreen;


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
        HideAllOverlays();
        showMoveOffRange();
        showNirvana();



    }


    void loadLevel()
    {

    }

    void updatePose()
    {

    }

    void HideAllOverlays()
    {
        redScreen.SetActive(false);
        YellowScreen.SetActive(false);

    }


    void showNirvana()
    {
        if (Player.transform.position == new Vector3(1, 0, 0))
        {
            YellowScreen.SetActive(true);
        }
    }

    void showMoveInRange()
    {

    }

    void showMoveOffRange()
    {
        if(Player.transform.position == new Vector3(0,0,0))
        {
            redScreen.SetActive(true);
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
