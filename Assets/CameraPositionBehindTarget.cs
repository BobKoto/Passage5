using UnityEngine;
using Cinemachine;
using StarterAssets;

public class CameraPositionBehindTarget : MonoBehaviour
{//Component of PlayerFollowCamera
    public CinemachineVirtualCamera vcam, fromCam;
    public Transform followTarget;
    private Vector3 initialOffset; // Offset from the target's starting position
    private Quaternion fromCamRotation;
    public PlayerEnteredRelevantTrigger rotateCam;

    private void Awake()  //Thanks GPT but not at all what we want
    {
        //// Calculate the initial offset and rotation
        //initialOffset = vcam.transform.position - followTarget.position;
        //initialRotation = Quaternion.Inverse(followTarget.rotation) * vcam.transform.rotation;
        Debug.Log(fromCam.name + "  fromCamRotation is  " + fromCam.transform.eulerAngles);

    }

    // This method will be called when the camera becomes live
    public void PositionCameraBehindTarget(ICinemachineCamera newCamera, ICinemachineCamera previousCamera)
    {
        string newCamS = newCamera.Name;
        string prevCamS = "NULL";// newCamS = newCamera.Name;
        string vCamS = vcam.name;
        if (previousCamera != null)
        {
            prevCamS = previousCamera.Name;

            if (prevCamS == fromCam.name)  //if so here is where we want to get & apply rotation from fromCam to vcam
            {
               // float yRot = 0f;
               // fromCamRotation = Quaternion.identity; // (fromCam.rotation);
                Debug.Log(fromCam.name + "  rotation needed here -- fromCamRotation is  " + fromCam.transform.eulerAngles + "TRYING");
                var rot = fromCam.transform.eulerAngles;
                vcam.transform.eulerAngles = rot; // fromCam.transform.eulerAngles;
                Debug.Log(vcam.name + "  rotation tried here -- vcamRotation is " + vcam.transform.eulerAngles + "TRIED rot is " + rot);
                rotateCam.Invoke(fromCam.transform.eulerAngles.y);
            }
        }

        //Debug.Log("POS Called:   newS = " + newCamS + "  vCamS = " + vCamS  + "   prevS = " + prevCamS);
        //if (newCamS == vCamS)
        //{

        //    Debug.Log("newCamera == vcam -- Reposition and rotate the follow cam..." + fromCamRotation);
        //}
    }
}

            //// Calculate the desired camera position behind the target's new position
            //Vector3 desiredPosition = followTarget.position + initialOffset;

            //// Calculate the desired camera rotation based on the target's rotation
            //Quaternion desiredRotation = followTarget.rotation * initialRotation;

            //// Set the camera's position and rotation to the calculated values
            //vcam.transform.position = desiredPosition;
            //vcam.transform.rotation = desiredRotation;