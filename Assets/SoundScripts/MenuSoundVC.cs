using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSoundVC : MonoBehaviour
{
    // Start is called before the first frame update
    void start()
    {
        FindObjectOfType<AudioManager>().play("Menu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
