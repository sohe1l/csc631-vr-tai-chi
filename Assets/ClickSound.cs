using UnityEngine;

using System.Collections;

using UnityEngine.UI;


[RequireComponent(typeof(Button))]

public class ClickSound : MonoBehaviour

{

    public AudioClip sound;
    private Button button { get { return GetComponent<Button>(); } }
    private AudioSource source { get { return GetComponent<AudioSource>(); } }
    // Use this for initialization

    void Start()
    {

        button.onClick.AddListener(() => FindObjectOfType<AudioManager>().play("menu_no"));
    }



}
