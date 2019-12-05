using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioScript : MonoBehaviour
{
    private AudioSource musicSource;
    private AudioSource sfxSource1;
    private AudioSource sfxSource2;

    public static AudioScript instance;

    private void Awake()
    {
        // Making sure each audio source is assigned to an instance of an object
        musicSource = this.gameObject.AddComponent<AudioSource>();
        sfxSource1 = this.gameObject.AddComponent<AudioSource>();
        sfxSource2 = this.gameObject.AddComponent<AudioSource>();

        instance = this;

        // Assigning audio mixer child to each audio source
        AudioMixer MasterMixer = Resources.Load("Master") as AudioMixer;
        string MixerGroup_1 = "Music";
        string MixerGroup_2 = "SFX1";
        string MixerGroup_3 = "SFX2";
        musicSource.outputAudioMixerGroup = MasterMixer.FindMatchingGroups(MixerGroup_1)[0];
        sfxSource1.outputAudioMixerGroup = MasterMixer.FindMatchingGroups(MixerGroup_2)[0];
        sfxSource2.outputAudioMixerGroup = MasterMixer.FindMatchingGroups(MixerGroup_3)[0];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlaySFX1(AudioClip audio, float volume, bool loop = false)
    {
        sfxSource1.volume = volume;
        sfxSource1.loop = loop;
        sfxSource1.clip = audio;
        sfxSource1.Play();
    }

    public void PlaySFX2(AudioClip audio, float volume, bool loop = false)
    {
        sfxSource2.volume = volume;
        sfxSource2.loop = loop;
        sfxSource2.clip = audio;
        sfxSource2.Play();
    }

    public void StopTraversalSound()
    {
        sfxSource1.Stop();
    }

    public void PlayMusic(AudioClip audio, float volume, bool loop = false)
    {
        musicSource.volume = volume;
        musicSource.loop = loop;
        musicSource.clip = audio;
        musicSource.Play();
    }

    /*public void SetVolumeLevel(float sliderValue)
    {
        masterMixer.SetFloat("MasterVol", Mathf.Log10(sliderValue) * 20);
    }*/
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
