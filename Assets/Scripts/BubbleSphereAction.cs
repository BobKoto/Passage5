using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSphereAction : MonoBehaviour
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
            if (!gameObject.CompareTag("FloorOfBubbles"))  this.gameObject.SetActive(false);

            audioManager.PlayAudio(audioManager.clipSplash);
        }
    }

    // Update is called once per frame
    //void Update()
    //{

    //}
}
