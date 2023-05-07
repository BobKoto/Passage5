using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformAction : MonoBehaviour
{

    public CloudTextEvent m_CloudTextEvent;

    const string onPlatform = "#Free ride";
    const string offPlatform = "#Now walk";

    public static bool playerIsOnPlatform { get; set; }

    AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
          //  audioManager.PlayAudio(audioManager.clipfalling);
            TellTextCloud(onPlatform);
            playerIsOnPlatform = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
          //  audioManager.PlayAudio(audioManager.clipBeamB);
            TellTextCloud(offPlatform);
            playerIsOnPlatform = false;
        }
    }
    public void TellTextCloud(string caption)
    {
        m_CloudTextEvent.Invoke(5, 4, caption);
    }

    // Update is called once per frame
    //void Update()
    //{

    //}
}
