using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MontyStopTrigger : MonoBehaviour
{
    MovingPlatform movingPlatform;
    // Start is called before the first frame update
    void Start()
    {
        movingPlatform = GameObject.Find("MovingPlatform").GetComponent<MovingPlatform>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("MovingPlatform"))
        {
            Debug.Log(other.gameObject.name + " Entered montyPStop.. ");
            movingPlatform.speed = 0;
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
