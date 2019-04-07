using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Required when Using UI elements.


//Scene transition numbers will be finalised later. It's hard coded for convenience. 
// Can see scene numbers in File > build settings
public class Menu : MonoBehaviour
{

    public InputField Input_1;

    public void PlayTraining()
    {
        if (ValidateName())
        {
            SceneManager.LoadScene("Training");
        }
    }
    public void PlayScored()
    {
        if (ValidateName())
        {
            SceneManager.LoadScene("Scored");
        }
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

    private bool ValidateName()
    {
        string nameError = Player.ValidateName(Input_1.text);
        if(nameError != null)
        {
            return false;
        }
        return true;
    }
}
