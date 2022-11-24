using randomize_array;   //because RandomTest.cs is in the namespace
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MontyPlayAreaEntered : MonoBehaviour
{
    //RandomTest randomTest;
    int winningDoor;
    // Start is called before the first frame update
    void Start()
    {
        // randomTest = new randomize_array.RandomTest;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("MovingPlatform"))
        {
            winningDoor = RandomTest.winningDoor;
            Debug.Log(other.gameObject.name + " Entered montyPlayArea... the winning Door is " + winningDoor);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("MovingPlatform"))
        {
            Debug.Log(other.gameObject.name + " Exited montyPlayArea... ");
        }

    }
    //// Update is called once per frame
    //void Update()
    //{

    //}
}
