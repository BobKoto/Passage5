using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Use me like this : audioManager.PlayAudio(audioManager.clipApplause); // where clipApplause can be any clip declared below 
//AND 2 other signature flavors: 1 to loop a clip - and 2 to stop a clip after x seconds
public class AudioManager : MonoBehaviour
{
    [Header("Audio Stuff")]   //should have been an array or list -- but... it does make me think!

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
    public AudioClip strom;
    public AudioClip tick;
    public AudioClip theetone;
    public AudioClip teleport1;
    public AudioClip compVoice0;
    public AudioClip compVoice1;
    public AudioClip compVoice2;
    public AudioClip compVoice3;
    public AudioClip laser1;

    public AudioSource audioSource;

    public AudioClipFinishedEvent audioClipFinishedEvent;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
       // PlayAudio(tick, .1f); //just to see if the timeout bug (not stopping ) only affects 1st sound played - no help 
    }
    public void PlayAudio(AudioClip clip)
    {
        audioSource.clip = clip;
        //Debug.Log("audioSource.clip Length is " + audioSource.clip.length + " seconds   Clip = " + audioSource.clip.name);
        audioSource.loop = false;
        audioSource.Play();
        if (clip.name == "dramaCut2") StartCoroutine(SendEventWhenAudioFinished(audioSource));  //until we think of a better way
      //  else Debug.Log("clip.name is " + clip.name);
    }
    public void PlayAudio(AudioClip clip, bool loop)
    {
        audioSource.clip = clip;
        if (loop) audioSource.loop = true;
        audioSource.Play();
    }
    public void PlayAudio(AudioClip clip, float playTimeStop)
    {
        audioSource.clip = clip;
        Debug.Log("audioSource.clip Length is " + audioSource.clip.length + " seconds   Clip = " + audioSource.clip.name);

        audioSource.loop = false;
        audioSource.Play();
        StartCoroutine(StopAudioOnPlayTimeStop(audioSource, playTimeStop));
    }
    IEnumerator SendEventWhenAudioFinished(AudioSource thisClip)
    {
        while (thisClip.isPlaying)
        {
            yield return new WaitForSeconds(1f);
        }
        //Debug.Log("AUDIO FINISHED................" + thisClip + " now Invoke event.....");
        //Broadcast an event here like audioClipFinishedEvent.Invoke();
        audioClipFinishedEvent.Invoke();
    }
    IEnumerator StopAudioOnPlayTimeStop(AudioSource audioSource, float stopAfter)
    {
         yield return new WaitForSeconds(stopAfter);
        audioSource.Stop();
       Debug.Log("AUDIO STOPPED after ................" + stopAfter + " seconds");
    }
}
