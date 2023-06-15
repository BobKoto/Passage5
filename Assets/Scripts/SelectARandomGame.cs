using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectARandomGame: MonoBehaviour
{
    BoxCollider boxCollider;
    private void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;  //6/13/23 why oh why do we need to do this? the dummy's arms & legs, dummy.
        Debug.Log(this.name + "  " + other.name + " came thru! Let's do something now ");
        boxCollider.enabled = false;
        SelectARandomGameIntro();

    }
    void SelectARandomGameIntro()
    {
        Debug.Log(this.name + "  You won " + CubeGameHandler.roundsWon + " rounds in the Cube Game");
        switch (CubeGameHandler.roundsWon)
        {
            case 0: Debug.Log("You won zero cube rounds. YOU ARE DEAD!!!! Goodbye...");
                break;
            case 1: Debug.Log("You won 1 cube round your only choice is to enter this world...");
                break;
            case 2: Debug.Log("You won 2 cube rounds. You can pick from these 2 worlds");
                break;
            case 3: Debug.Log("You won 3 cube rounds. You can pick from these 3 worlds");
                break;

        }
    }
}
