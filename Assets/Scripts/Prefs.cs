using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Prefs 
{
    private const string KEY_PLAYER_NAME = "KEY_PLAYER_NAME";

    public static void SetPlayerName(string name)
    {
        PlayerPrefs.SetString(KEY_PLAYER_NAME, name);
    }

    public static string GetPlayerName()
    {
        return PlayerPrefs.GetString(KEY_PLAYER_NAME);
    }

}
