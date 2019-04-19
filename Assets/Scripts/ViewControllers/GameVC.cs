using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameVC : MonoBehaviour
{

    private int currentScore;
    private int Level;

    // Start is called before the first frame update
    void Start()
    {
        Level = Prefs.GetLevelID();

        // Debug.Log(Prefs.GetLevelID());
        // Debug.Log(Prefs.GetPlayerName());

        

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void loadLevel()
    {

    }

    void updatePose()
    {

    }


    void showNirvana()
    {

    }

    void showMoveInRange()
    {

    }

    void showMoveOffRange()
    {

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
