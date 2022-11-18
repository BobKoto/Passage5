using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnterBubbleRoom : MonoBehaviour
{

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
            audioManager.PlayAudio(audioManager.clipSplash);
        }
    }
    // Update is called once per frame
    //void Update()
    //{

    //}
}
