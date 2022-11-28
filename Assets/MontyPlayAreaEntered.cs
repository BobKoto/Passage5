using Cinemachine;
using randomize_array;   //because RandomTest.cs is in the namespace
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MontyPlayAreaEntered : MonoBehaviour
{
    //RandomTest randomTest;
    int winningDoor;
    [Header("MontyGameSignage")]
    public GameObject mainSign;
    public GameObject rearSign;
    public GameObject leftSign;
    public GameObject rightSign;
    [Header("Cinemachine Cameras")]
    public CinemachineVirtualCamera thirdPersonFollowCam;
    public CinemachineFreeLook freeLookCam;

    int thirdPersonFollowCamOriginalPriority, freeLookCamOriginalPriority;
    // Start is called before the first frame update
    void Start()
    {
        // randomTest = new randomize_array.RandomTest;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("MovingPlatform"))
        {
            winningDoor = RandomTest.winningDoor;
            Debug.Log(other.gameObject.name + " Entered montyPlayArea... the winning Door is " + winningDoor);
            rearSign.SetActive(true);
            leftSign.SetActive(true);
            rightSign.SetActive(true);
            mainSign.SetActive(true);
          //  freeLookCam.Priority = 12;  // make it Live   - cam transition not good - probably something i don't know 
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("MovingPlatform"))
        {
            rearSign.SetActive(false);
            leftSign.SetActive(false);
            rightSign.SetActive(false);
            mainSign.SetActive(false);
          //  freeLookCam.Priority = 10;  // make it Standby  - cam transition not good - probably something i don't know
            Debug.Log(other.gameObject.name + " Exited montyPlayArea... from " + this.name);
        }

    }
    //// Update is called once per frame
    //void Update()
    //{

    //}
}
