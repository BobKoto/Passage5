using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport01 : MonoBehaviour
{
    //public GameObject player, playerCameraRoot;
    public Transform playerTransform, playerCameraRootTransform;
    public Vector3 teleportPlayerToPosition;
    public Quaternion rotatePlayerToRotation;
    public Vector3 teleportCameraRootToPosition;
    public Quaternion rotateCameraRootToRotation;
    //public Vector3 rotateVector;
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

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("teleport01 sees TriggerENTER " + other.ToString());
    //    if (!teleportPerformed)
    //    {
    //        playerTransform.SetPositionAndRotation(teleportToThisPosition, rotateToThisRotation);
    //        StartCoroutine(WaitToMove());
    //      //  teleportPerformed = true;
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TriggerEnter ...");
        playerTransform.position = teleportPlayerToPosition;
        Physics.SyncTransforms();  
    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (!teleportPerformed)
    //    {
    //        Debug.Log("teleport01 sees TriggerSTAY " + other.ToString());
    //        playerTransform.SetPositionAndRotation(teleportPlayerToPosition, rotatePlayerToRotation);
    //       // playerCameraRootTransform.Rotate(rotateVector);

    //        // teleportPerformed = true;
    //    }
    //}
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("teleport01 sees TriggerEXIT " + other.ToString());
        //  playerTransform.SetPositionAndRotation(teleportPlayerToPosition, rotatePlayerToRotation);
        playerTransform.rotation = Quaternion.Euler(0, 0, 0);
        StartCoroutine(RotatePlayerCameraRoot());  //doesn't seem to work anyway
    }

    //private void OnCollisionEnter(Collision other)
    //{
    //    Debug.Log("teleport01 sees collision " + other.ToString());
    //    playerTransform.SetPositionAndRotation(teleportPlayerToPosition, rotatePlayerToRotation);
    //}
    //private void OnCollisionStay(Collision other)
    //{
    //    Debug.Log("teleport01 sees collisionSTAY " + other.ToString());
    //    playerTransform.SetPositionAndRotation(teleportPlayerToPosition, rotatePlayerToRotation);
    //}
    IEnumerator RotatePlayerCameraRoot()
    {
        Debug.Log("teleport01/RotatePlayerCameraRoot supposedly SPIN THE CAM?  BEFORE yield");

        yield return new WaitForSeconds (2f);
        Debug.Log("teleport01/RotatePlayerCameraRoot supposedly SPIN THE CAM?  AFTER yield");
       // playerCameraRootTransform.Rotate(rotateVector);
       // playerCameraRootTransform.SetPositionAndRotation(playerCameraRootTransform.position, rotateCameraRootToRotation);
    }
}
