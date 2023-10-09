using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BubbleRoomCamSwitcher : MonoBehaviour
{  // Component of ClearShotCamCollideramCollider  Assumes PlayerFollowCamera.Priority = 11

    public CinemachineClearShot bubbleRoomClearShotCam;

    public void OnTriggerEnter(Collider other)  
    {
        if (other.CompareTag("Player"))
        {
            bubbleRoomClearShotCam.Priority = 12;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bubbleRoomClearShotCam.Priority = 7;
        }
    }
}
