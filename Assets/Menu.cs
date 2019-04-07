using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Scene transition numbers will be finalised later. It's hard coded for convenience. 
// Can see scene numbers in File > build settings
public class Menu : MonoBehaviour
{
    public void PlayTraining()
    {
        SceneManager.LoadScene("Training");
    }
    public void PlayScored()
    {
        SceneManager.LoadScene("Scored");
    }
    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }
    public void Home()
    {
        SceneManager.LoadScene("Menu");

    }
    public void EndScreen()
    {
        SceneManager.LoadScene("End");
    }
}
