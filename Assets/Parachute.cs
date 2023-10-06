using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parachute : MonoBehaviour
{
    Vector3 originalPosition;
    Quaternion originalRotation;

    public PlayerEnteredRelevantTrigger setCamAndPlayerAngle;

    // Start is called before the first frame update
    void Start()
    {

        originalPosition = transform.position;
        originalRotation =   transform.rotation;
        Debug.Log("Player position is " + originalPosition);
    }

    public void OnParachutePressed()
    {
        Debug.Log("Parachute pressed!!!");
        transform.position =  originalPosition;
        transform.rotation = originalRotation;
        setCamAndPlayerAngle.Invoke(originalRotation.eulerAngles.y, true);    //BK 9/4/23 if this works we can just call move once?

        Physics.SyncTransforms();

    }
}
