using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class RoomVcam : MonoBehaviour
{  //Component of DarkPlaceInnerWithOcclusion
    public CinemachineVirtualCamera vCam;
    public float switchCooldown = 2.0f; // Adjust this value as needed
    int vCamOriginalPriority;
    private float lastSwitchTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        vCamOriginalPriority = vCam.Priority;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Time.time - lastSwitchTime >= switchCooldown)
        {
            Debug.Log(other.name + " entered  swtichCoolDown = " + switchCooldown);
            vCam.Priority = 13;
            lastSwitchTime = Time.time;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && Time.time - lastSwitchTime >= switchCooldown)
        {
            vCam.Priority = vCamOriginalPriority;
            lastSwitchTime = Time.time;
        }
    }
}
//public class CameraSwitcher : MonoBehaviour
//{
//    public CinemachineVirtualCamera camera1;
//    public CinemachineVirtualCamera camera2;
//    public float switchCooldown = 2.0f; // Adjust this value as needed

//    private float lastSwitchTime = 0.0f;

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("Player") && Time.time - lastSwitchTime >= switchCooldown)
//        {
//            // Switch to camera2 when the player enters the trigger
//            CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera.Priority = 0;
//            camera2.Priority = 10;

//            lastSwitchTime = Time.time;
//        }
//    }

//    private void OnTriggerExit(Collider other)
//    {
//        if (other.CompareTag("Player") && Time.time - lastSwitchTime >= switchCooldown)
//        {
//            // Switch back to camera1 when the player exits the trigger
//            CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera.Priority = 0;
//            camera1.Priority = 10;

//            lastSwitchTime = Time.time;
//        }
//    }
//}
/*
using UnityEngine;
using Cinemachine;

public class CameraPositionBehindTarget : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    public Transform followTarget;
    private Vector3 initialOffset; // Offset from the target's starting position
    private Quaternion initialRotation; // Rotation offset from the target's starting rotation

    private void Awake()
    {
        // Calculate the initial offset and rotation
        initialOffset = vcam.transform.position - followTarget.position;
        initialRotation = Quaternion.Inverse(followTarget.rotation) * vcam.transform.rotation;
    }

    // This method will be called when the camera becomes live
    public void PositionCameraBehindTarget(ICinemachineCamera previousCamera, ICinemachineCamera newCamera)
    {
        // Check if the newly live camera is the VCam we're handling
        if (newCamera == vcam)
        {
            // Calculate the desired camera position behind the target's new position
            Vector3 desiredPosition = followTarget.position + initialOffset;

            // Calculate the desired camera rotation based on the target's rotation
            Quaternion desiredRotation = followTarget.rotation * initialRotation;

            // Set the camera's position and rotation to the calculated values
            vcam.transform.position = desiredPosition;
            vcam.transform.rotation = desiredRotation;
        }
    }
}

 */