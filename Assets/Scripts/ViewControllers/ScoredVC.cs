﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoredVC : MonoBehaviour
{

    public Text inputName;
    public GameObject ScrollViewContent;
    public GameObject LevelRowPrefab;
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
            .Where(v => v.Mode.Equals(Level.MODE_SCORED));

        int counter = 0;
        foreach (Level level in query)
        {
            GameObject LevelRow = Instantiate(LevelRowPrefab);

            Text levelName = LevelRow.transform.Find("LevelName").GetComponent<Text>();
            levelName.text = level.Name;

            Text score = LevelRow.transform.Find("Score").GetComponent<Text>();
            //query scores from the db
            //set score to score

            Button StartBtn = LevelRow.transform.Find("StartBtn").GetComponent<Button>();
            StartBtn.onClick.AddListener(() => LoadLevel(level.Id));

            LevelRow.transform.SetParent(ScrollViewContent.transform, false);
            LevelRow.transform.Translate(Vector3.down * LEVEL_ROW_OFFSET * counter);
            counter++;
        }

    }

    private void LoadLevel(int id)
    {
        Prefs.SetLevelID(id);
        SceneManager.LoadScene("Game");
    }



}
