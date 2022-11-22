using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MontyPlayAreaEntered : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Entered montyPlayArea...");
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Exited montyPlayArea...");
        }

    }
    //// Update is called once per frame
    //void Update()
    //{

    //}
}
