using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectARandomGame: MonoBehaviour
{//Component of SelectARandomGameTrigger
    AudioManager audioManager;
    BoxCollider boxCollider;
    [Header ("The Signage")]
    public GameObject selectRandomGameSign;     //The whole sign
    public TMP_Text selectRandomGameSignText; //And the text

    GameObject[] teleportals;

    Vector3 leftPortal =   new Vector3(203.74f, 4.82f, -231.45f);   //make these public?
    Vector3 centerPortal = new Vector3(203.74f, 4.82f, -217.2f);
    Vector3 rightPortal =  new Vector3(203.74f, 4.82f, -202.94f);

    private void Start()
    {
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
        DisableThePortals();  //6/17/23 a case for a game manager - but let's keep going because we might not go too much farther 
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
    void DisableThePortals()   //this should be temporary
    {
        teleportals = GameObject.FindGameObjectsWithTag("RandomGamePortal");
        Debug.Log("found " + teleportals.Length + " portals for the random game ...............");
        for (int i = 0; i <= teleportals.Length -1; i++)
        {
            teleportals[i].SetActive(false);
        }
    }
    void SelectARandomGameIntro()
    {
       // Debug.Log(this.name + "  You won " + CubeGameHandler.roundsWon + " rounds in the Cube Game");
        switch (CubeGameHandler.roundsWon)
        {
            case 0: //Debug.Log("You won zero cube rounds. YOU ARE DEAD!!!! Goodbye...");
                selectRandomGameSignText.text = "Zero cube rounds won. YOU ARE DEAD!!!! Goodbye...";
                break;
            case 1: //Debug.Log("You won 1 cube round your only choice is to enter this world...");
                selectRandomGameSignText.text = "1 cube round won. The only choice is to enter this portal. Walk forward to enter...";
                teleportals[0].transform.position = centerPortal;
                teleportals[0].SetActive(true);
                break;
            case 2: //Debug.Log("You won 2 cube rounds. You can pick from these 2 worlds");
                selectRandomGameSignText.text = "2 cube rounds won. Pick from these 2 portals. Walk into your choice...";
                teleportals[0].transform.position = leftPortal;
                teleportals[0].SetActive(true);
                teleportals[1].transform.position = rightPortal;
                teleportals[1].SetActive(true);
                break;
            case 3: //Debug.Log("You won 3 cube rounds. You can pick from these 3 worlds");
                selectRandomGameSignText.text = "3 cube rounds won. Pick from these 3 portals. Walk into your choice...";
                teleportals[0].transform.position = leftPortal;
                teleportals[0].SetActive(true);
                teleportals[1].transform.position = centerPortal;
                teleportals[1].SetActive(true);
                teleportals[2].transform.position = rightPortal;
                teleportals[2].SetActive(true);

                break;

        }
    }
}
