using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Use me like this : audioManager.PlayAudio(audioManager.clipApplause); // where clipApplause can be any clip declared below 
public class AudioManager : MonoBehaviour
{
    [Header("Audio Stuff")]   //should have been an array or list -- but...


    public AudioClip clipApplause;
    public AudioClip clipkongas;
    public AudioClip clipkongasNoVocal;
    public AudioClip clipapert;
    public AudioClip clipSplash;
    public AudioClip clipfalling;
    public AudioClip clipBeamB;
    public AudioClip clipdrama;
    public AudioClip clipDRUMROLL;
    public AudioClip clipding;
    public AudioClip TYPE;
    public AudioClip WHOOSH;

    public AudioSource audioSource;
  //  [Header("END Audio Stuff")]
  
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
    public void PlayAudio(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.loop = false;
        audioSource.Play();
    }
    public void PlayAudio(AudioClip clip, bool loop)
    {
        audioSource.clip = clip;
        if (loop) audioSource.loop = true;
        audioSource.Play();
    }
}
