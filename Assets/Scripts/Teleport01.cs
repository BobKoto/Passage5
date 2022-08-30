using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport01 : MonoBehaviour
{
    public GameObject player, playerCameraRoot;
    public Transform playerTransform, playerCameraRootTransform;
    public Vector3 teleportToThisPosition;
    public Quaternion rotateToThisRotation;
    public Vector3 rotateVector;
    public bool teleportPerformed;

   // IEnumerator WaitToMove;

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
        Debug.Log("teleport01 sees TriggerENTER " + other.ToString());
        if (!teleportPerformed)
        {
            playerTransform.SetPositionAndRotation(teleportToThisPosition, rotateToThisRotation);
            StartCoroutine(WaitToMove());
          //  teleportPerformed = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("teleport01 sees TriggerStay " + other.ToString());
        if (!teleportPerformed)
        {
            playerTransform.SetPositionAndRotation(teleportToThisPosition, rotateToThisRotation);
            StartCoroutine(WaitToMove());
         //   teleportPerformed = true;
        }

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
    IEnumerator WaitToMove()
    {
        yield return new WaitForSeconds (5f);
        Debug.Log("teleport01 supposedly SPINS THE CAM?");
        playerCameraRootTransform.Rotate(rotateVector);

    }
}
