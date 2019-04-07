using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoredVC : MonoBehaviour
{

    public Text inputName;

        // Start is called before the first frame update
    void Start()
    {
        string playerName = Prefs.GetPlayerName();
        inputName.text = playerName;
        Player player = Player.GetOrCreatePlayer(playerName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
