using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioScript : MonoBehaviour
{
    public AudioClip UI_Select;
    public AudioSource traversalSound;
    public static AudioScript instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void PlaySFX()
    {
        GetComponent<AudioSource>().PlayOneShot(UI_Select);
    }
    public void PlayTraversalSound()
    {
        traversalSound.Play();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaySFX();
        }
    }
}
