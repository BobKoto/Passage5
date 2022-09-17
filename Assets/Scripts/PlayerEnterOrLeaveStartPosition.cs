using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerEnterOrLeaveStartPosition : MonoBehaviour
{
    public GameObject player;
    public AudioManager audioManager;
    public CinemachineVirtualCamera player3rdPersonFollowCamera;
    bool loopTheClip = true;
    bool switchTheCam = true;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("HELLO FROM Player hit the StartPosition ...");
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("cube hit by " + collision);
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit the floor ...");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("cube hit by " + other);
        if (other.gameObject.CompareTag("Player"))
        {
            audioManager.PlayAudio(audioManager.clipApplause);
            if (switchTheCam)
            {
                player3rdPersonFollowCamera.MoveToTopOfPrioritySubqueue();
                switchTheCam = false;
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            audioManager.PlayAudio(audioManager.clipkongasNoVocal, loopTheClip);
           // DirectionTracker.RegisterDirection(false, false, false, false);
        }
    }
}
