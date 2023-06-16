using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectARandomGame: MonoBehaviour
{
    AudioManager audioManager;
    BoxCollider boxCollider;
    public GameObject selectRandomGameSign;     //The whole sign
    public TMP_Text selectRandomGameSignText; //And the text
    private void Start()
    {
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;  //6/13/23 why oh why do we need to do this? the dummy's arms & legs, dummy.
       // Debug.Log(this.name + "  " + other.name + " came thru! Let's do something now ");
        boxCollider.enabled = false;
        if (selectRandomGameSign) selectRandomGameSign.SetActive(true);
        audioManager.PlayAudio(audioManager.theetone);
        SelectARandomGameIntro();

    }
    void SelectARandomGameIntro()
    {
       // Debug.Log(this.name + "  You won " + CubeGameHandler.roundsWon + " rounds in the Cube Game");
        switch (CubeGameHandler.roundsWon)
        {
            case 0: //Debug.Log("You won zero cube rounds. YOU ARE DEAD!!!! Goodbye...");
                selectRandomGameSignText.text = "You won zero cube rounds. YOU ARE DEAD!!!! Goodbye...";
                break;
            case 1: //Debug.Log("You won 1 cube round your only choice is to enter this world...");
                selectRandomGameSignText.text = "You won 1 cube round your only choice is to enter this world...";
                break;
            case 2: //Debug.Log("You won 2 cube rounds. You can pick from these 2 worlds");
                selectRandomGameSignText.text = "You won 2 cube rounds. You can pick from these 2 worlds";
                break;
            case 3: //Debug.Log("You won 3 cube rounds. You can pick from these 3 worlds");
                selectRandomGameSignText.text = "You won 3 cube rounds. You can pick from these 3 worlds";
                break;

        }
    }
}
