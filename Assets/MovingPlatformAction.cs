using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformAction : MonoBehaviour
{

    public MyIntEvent m_MyEvent;

    const string onPlatform = "#Free ride";
    const string offPlatform = "#Now walk";

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
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
          //  audioManager.PlayAudio(audioManager.clipBeamB);
            TellTextCloud(offPlatform);
        }
    }
    public void TellTextCloud(string caption)
    {
        m_MyEvent.Invoke(5, 4, caption);
    }

    // Update is called once per frame
    //void Update()
    //{

    //}
}
