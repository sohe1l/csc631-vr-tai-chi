using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Clicksound : MonoBehaviour
{
    public AudioClip sound;

    private Button button { get { return GetComponent<Button>(); } } 
    private AudioSource source { get { return GetComponent<AudioSource>(); } }
    void Start()
    {
        gameObject.AddComponent<AudioSource>();
        source.clip =  sound;
        source.playOnAwake = false;
        source.volume = 60;

        button.onClick.AddListener(() => PlaySound());
    }

    // Update is called once per frame
    void PlaySound()
    {
        source.PlayOneShot(sound);
    }
}
