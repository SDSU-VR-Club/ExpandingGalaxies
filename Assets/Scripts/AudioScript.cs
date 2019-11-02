using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioScript : MonoBehaviour
{
    public AudioSource planetSelect;
    public AudioSource traversalSound;
    public AudioMixer masterMixer;
    public static AudioScript instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void PlaySFX()
    {
        planetSelect.Play();
        traversalSound.Play();
    }

    public void SetVolumeLevel(float sliderValue)
    {
        masterMixer.SetFloat("MasterVol", Mathf.Log10(sliderValue) * 20);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
