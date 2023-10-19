using UnityEngine;
using Cinemachine;

public class CameraPositionBehindTarget : MonoBehaviour
{//Component of PlayerFollowCamera
    public CinemachineVirtualCamera vcam, fromCam;
    public PlayerEnteredRelevantTrigger rotateCam;

    public void PositionCameraBehindTarget(ICinemachineCamera newCamera, ICinemachineCamera previousCamera)  // Called when the camera becomes live
    {
        string newCamS = newCamera.Name;
        string prevCamS = "NULL";// newCamS = newCamera.Name;
        string vCamS = vcam.name;
        if (previousCamera != null)
        {
            prevCamS = previousCamera.Name;
            if (prevCamS == fromCam.name)  //if so here is where we want to get & apply rotation from fromCam to vcam
            {
                rotateCam.Invoke(fromCam.transform.eulerAngles.y,false);  //10/3/23 added bool NOT Used for now 
            }
        }
    }
}