using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnterOrLeaveObstacle : MonoBehaviour
{
    public GameObject player;
    public AudioManager audioManager;
    bool loopTheClip = true;
    bool alreadyHit;
    Material material;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("HELLO FROM Player hit Enter or Leave Obstacle ...");
        material = GetComponent<Renderer>().material;
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
            if (!alreadyHit)
            {
                material.color = Color.black;
                audioManager.PlayAudio(audioManager.clipApplause);
              //  alreadyHit = true;
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!alreadyHit)
            {
                alreadyHit = true; //Ignore alreadyHit boolean for StartPosition
                audioManager.PlayAudio(audioManager.clipkongasNoVocal,loopTheClip);
            }

        }

    }
}
