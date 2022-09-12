using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Stuff")]


    public AudioClip clipApplause;
    public AudioClip clipkongas;
    public AudioClip clipkongasNoVocal;

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
