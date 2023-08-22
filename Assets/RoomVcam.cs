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