using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport01 : MonoBehaviour
{
    public Transform playerTransform; //, playerCameraRootTransform;
    public Vector3 teleportPlayerToPosition;
    // public Quaternion rotatePlayerToRotation;
    // public Vector3 teleportCameraRootToPosition;
    // public Quaternion rotateCameraRootToRotation;
    //public Vector3 rotateVector;
    //public bool teleportPerformed;
    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;
    AudioManager audioManager;

    // IEnumerator WaitToMove;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        // Debug.Log("hello from teleport01");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("TriggerEnter ... Teleport player to " + teleportPlayerToPosition);
            playerTransform.position = teleportPlayerToPosition; //original worked but with Jank
                                                                 //var destination = Vector3.Lerp(playerTransform.position, teleportPlayerToPosition, 0.1f);
                                                                 //playerTransform.position = destination;
        }
        if (other.CompareTag("Teleportable"))
        {
            Transform otherTransform = other.transform;
            if (teleportPlayerToPosition.y < 0)  //we are teleporting to a < 0 y position (lower level) so we need to account for scale
            {
                float yScaleOfOtherOffset = otherTransform.localScale.y /2;
                Vector3 offsetForYScale = new Vector3(0, yScaleOfOtherOffset, 0);
                Vector3 offsetTeleportPlayerToPosition = teleportPlayerToPosition + offsetForYScale;
                Debug.Log("TriggerEnter ... Teleport something ELSE to Lower level" + offsetTeleportPlayerToPosition + " Other xform is " + otherTransform);
                otherTransform.position = offsetTeleportPlayerToPosition;

            }
            else
            {
                Debug.Log("TriggerEnter ... Teleport something ELSE to " + teleportPlayerToPosition + " Other xform is " + otherTransform);
                otherTransform.position = teleportPlayerToPosition;
            }

        }
        Physics.SyncTransforms();
        audioManager.PlayAudio(audioManager.teleport1);

    }
}

//    private void OnTriggerExit(Collider other)
//    {
//        Debug.Log("teleport01 sees TriggerEXIT " + other.ToString());
//        //  playerTransform.SetPositionAndRotation(teleportPlayerToPosition, rotatePlayerToRotation);
//        playerTransform.rotation = Quaternion.Euler(0, 0, 0);
//        StartCoroutine(RotatePlayerCameraRoot());  //doesn't seem to work anyway
//    }

//    IEnumerator RotatePlayerCameraRoot()
//    {
//        Debug.Log("teleport01/RotatePlayerCameraRoot supposedly SPIN THE CAM?  BEFORE yield");

//        yield return new WaitForSeconds (2f);

//        // playerCameraRootTransform.Rotate(rotateVector);
//        // playerCameraRootTransform.SetPositionAndRotation(playerCameraRootTransform.position, rotateCameraRootToRotation);
//       // CinemachineCameraTarget.transform.rotation = Quaternion.Euler(0f, -36f, 0f);
//           Debug.Log("teleport01/RotatePlayerCameraRoot supposedly SPIN THE CAM?  AFTER yield");
//    }

//    private void CameraRotation()  //Ripped right outta ThirdPersonController.cs 
//    {
//        //// if there is an input and camera position is not fixed
//        //if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
//        //{
//        //    //Don't multiply mouse input by Time.deltaTime;
//        //    float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

//        //    _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
//        //    _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
//        //}

//        //// clamp our rotations so our values are limited 360 degrees
//        //_cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
//        //_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

//        //// Cinemachine will follow this target
//        //CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
//        //    _cinemachineTargetYaw, 0.0f);
//    }
//}
