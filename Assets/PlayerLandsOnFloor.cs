using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandsOnFloor : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("HELLO FROM Player hit the floor ...");
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
        Debug.Log("cube hit by " + other);
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("OnTrigger Player hit the floor ...");
        }
    }
}
