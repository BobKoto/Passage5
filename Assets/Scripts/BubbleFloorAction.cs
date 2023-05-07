using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BubbleFloorAction : MonoBehaviour
{   //Component of WallsInMaze\BubbleRoom\FloorOfBubbles - pretty sure 

    public CloudTextEvent m_CloudTextEvent;

    const string hitFloorYuk = "#Yuck";

    AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();

        //if (m_MyEvent == null)  //not needed if we take care
        //{
        //    Debug.Log(this.name + " reports m_MyEvent is null ");
        //    m_MyEvent = new MyIntEvent();
        //}
        //else Debug.Log(this.name + " reports m_MyEvent is found ");

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioManager.PlayAudio(audioManager.clipSplash);
            TellTextCloud(hitFloorYuk);
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
