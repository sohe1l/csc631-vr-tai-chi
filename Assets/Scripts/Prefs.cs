using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Prefs 
{
    private const string KEY_PLAYER_NAME = "KEY_PLAYER_NAME";
    private const string KEY_LEVEL_ID = "KEY_LEVEL_ID";

    public static void SetPlayerName(string name)
    {
        PlayerPrefs.SetString(KEY_PLAYER_NAME, name);
    }

    public static string GetPlayerName()
    {
        return PlayerPrefs.GetString(KEY_PLAYER_NAME);
    }

    public static void SetLevelID(int id)
    {
        PlayerPrefs.SetInt(KEY_LEVEL_ID, id);
    }

    public static int GetLevelID()
    {
        return PlayerPrefs.GetInt(KEY_LEVEL_ID);
    }

}
