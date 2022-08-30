using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport01 : MonoBehaviour
{
    public GameObject player;
    public Transform playerTransform;
    public Vector3 teleportToThisPosition;
    public Quaternion rotateToThisRotation;

    // Start is called before the first frame update
    void Start()
    {
        //  teleportToThisPosition = new Vector3(-16f, -1f, -10f);
        // rotateToThisRotation = new Quaternion(0f, 0f, 0f, 0f);
        Debug.Log("hello from teleport01");
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("teleport01 sees Triggercollision " + other.ToString());
        playerTransform.SetPositionAndRotation(teleportToThisPosition, rotateToThisRotation);
    }
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("teleport01 sees collision " + other.ToString());
        playerTransform.SetPositionAndRotation(teleportToThisPosition, rotateToThisRotation);
    }
    private void OnCollisionStay(Collision other)
    {
        Debug.Log("teleport01 sees collisionSTAY " + other.ToString());
        playerTransform.SetPositionAndRotation(teleportToThisPosition, rotateToThisRotation);
    }
}
