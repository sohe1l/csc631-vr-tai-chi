﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrainingVC : MonoBehaviour
{

    public Text inputName;
    public GameObject ScrollViewContent;
    public GameObject TrainingRowFab;
    Player player;

    private const int LEVEL_ROW_OFFSET = 35;

    // Start is called before the first frame update
    void Start()
    {
        string playerName = Prefs.GetPlayerName();
        inputName.text = playerName;
        player = Player.GetOrCreatePlayer(playerName);

        LoadLevels();
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void LoadLevels()
    {
        var db = DataService.Instance.GetConnection();

        var query = db.Table<Level>()
            .Where(v => v.Mode.Equals(Level.MODE_TRANING));

        int counter = 0;
        foreach (Level level in query)
        {
            GameObject LevelRow = Instantiate(TrainingRowFab);

            Text levelName = LevelRow.transform.Find("LevelName").GetComponent<Text>();
            levelName.text = level.Name;

            Toggle completion = LevelRow.transform.Find("Completion").GetComponent<Toggle>();
           var userScore = db.Table<Leaderboard>()
               .Where(v => v.Level.Equals(level.Id))
               .Where(v => v.Player_id.Equals(player.Id));

            Debug.Log(userScore);
            if ( userScore.Count() != 0)
            {

                completion.isOn = true;
            }

            Button StartBtn = LevelRow.transform.Find("StartBtn").GetComponent<Button>();
            FindObjectOfType<AudioManager>().play("menu_yes");
            StartBtn.onClick.AddListener(() => LoadLevel(level.Id));

            LevelRow.transform.SetParent(ScrollViewContent.transform, false);
            LevelRow.transform.Translate(Vector3.down * LEVEL_ROW_OFFSET * counter);
            counter++;
        }

    }

    private void LoadLevel(int id)
    {
        Prefs.SetLevelID(id);
        SceneManager.LoadScene("BeachScene");
    }



}
