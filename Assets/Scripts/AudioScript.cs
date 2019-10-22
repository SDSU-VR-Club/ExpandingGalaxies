using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioScript : MonoBehaviour
{
    public AudioClip UI_Select;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlaySFX()
    {
        GetComponent<AudioSource>().PlayOneShot(UI_Select);
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
