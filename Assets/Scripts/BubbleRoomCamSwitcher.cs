using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BubbleRoomCamSwitcher : MonoBehaviour
{

    public CinemachineVirtualCamera thirdPersonFollowCam;
    public CinemachineClearShot bubbleRoomClearShotCam;

    public void OnTriggerEnter(Collider other)   //probably should check if the Player is doing the entering and exiting
    {
        //bubbleRoomClearShotCam.MoveToTopOfPrioritySubqueue();
        bubbleRoomClearShotCam.Priority = 12;
    }
    public void OnTriggerExit(Collider other)
    {
        //thirdPersonFollowCam.MoveToTopOfPrioritySubqueue();
        bubbleRoomClearShotCam.Priority = 7;
    }
}
