using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;
    public Boolean isPlaying = false;
    // Start is called before the first frame update
    void Awake()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach(Sound s in sounds)
        {
           s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            //s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }   
    }

    void Start()
    {
        PlayMenu();
    }

    public void PlayMenu()
    {
        if (!isPlaying)
        {
            play("Menu");
            isPlaying = true;
        }
    }


    public void StopMenu()
    {
        stop("Menu");
        isPlaying = false;
    }


    // Update is called once per frame
    public void play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("sound not found");
            return;
        }
        s.source.Play();
    }
    public void stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("sound not found");
            return;
        }
        s.source.Stop();
        

    }
}
