using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System;
using StarterAssets;

public class ZipAdvancePlayer : MonoBehaviour
{// Component of PlayerArmature    // written by ChatGPT with about 8 prompts over about an hour  & a week to get working 

    public float raycastDistance = 50.0f;
    public float raycastInterval = 1.0f;
    public float transformTranslateDelay = 1.0f;
    public bool noColliderInFront = false; // Set to true if no collider in front
    public bool debugDistance = false;
    public Button zipButton; // Reference to the "ZIP->" button
    public CanvasGroup buttonCanvasGroup; // Reference to the button's CanvasGroup
    public CinemachineVirtualCamera followCamera;
    private CharacterController characterController;
    public PlayerEnteredRelevantTrigger setCamAndPlayerAngle;
    bool startRan;
    float moveDistance;

    private void OnEnable()
    {
        if (startRan) Start();
        //Debug.Log("RayCasting enabled OnEnable ....");
    }

    private void Start()
    {
        startRan = true;
       // Debug.Log("RayCasting enabled in START()....");
        // Get the CharacterController component
        characterController = GetComponent<CharacterController>();

        // Get a reference to the "ZIP->" button
        zipButton = GameObject.Find("ZipButton").GetComponent<Button>();
        buttonCanvasGroup = zipButton.GetComponent<CanvasGroup>();
        buttonCanvasGroup.alpha = 0; // Hide the button by default

        // Add an onClick listener to the button
        zipButton.onClick.AddListener(OnZipButtonClick);

        // Start the coroutine to perform raycasts at intervals
        StartCoroutine(RaycastCoroutine());
        Debug.Log("ZipAdvancePlayer started coroutine");
    }

    private void OnZipButtonClick()
    {
        if (buttonCanvasGroup.alpha == 1)
        {
            StartCoroutine (ZipPlayerForward());
            // Hide the button again
            buttonCanvasGroup.alpha = 0;
        }
    }

    private IEnumerator RaycastCoroutine()
    {
        while (true)
        {
            // Calculate the ray's origin and direction from the Cinemachine camera
            Vector3 halfHeightOfCamera = new Vector3 (0f, followCamera.transform.position.y / 2, 0f);
            Vector3 rayOrigin = followCamera.transform.position - halfHeightOfCamera;
            Vector3 rayDirection = followCamera.transform.forward;
            //Debug.Log("rayDirection = " + rayDirection);

            // Perform the raycast
            if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, raycastDistance))
            {
                // A collider was hit   10 is cam 5f behind player + 5f for minimum zipable  
                if (hit.distance <= 10f) buttonCanvasGroup.alpha = 0; // Hide the button
                if (hit.distance > 10f)  // we can enable zipping
                {
                    buttonCanvasGroup.alpha = 1; // Show the button
                    moveDistance = hit.distance - 8f;  //raycastDistance less (distance from cam + 3f buffer )
                    if (debugDistance)  Debug.Log("Distance to collider: " + hit.distance + "  hit " + hit.collider);
                }

            }
            else
            {

                // No collider hit  - So set moveDistance to move player the entire raycastDistance less (distance from cam + 3f buffer )
                moveDistance = raycastDistance -8;
                buttonCanvasGroup.alpha = 1; // Show the button
               // Debug.Log("No collider hit   distance " + hit.distance);
               // Debug.DrawRay(transform.position + characterController.center, transform.TransformDirection(Vector3.forward) * (hit.distance +5), Color.yellow, 4f);
            }
            var minRay = Math.Max(hit.distance, raycastDistance);

            Debug.DrawRay(rayOrigin, followCamera.transform.TransformDirection(Vector3.forward) * minRay , Color.yellow, raycastInterval -.5f);
            yield return new WaitForSeconds(raycastInterval);

        }
    }
    private IEnumerator ZipPlayerForward()
    {
        // Debug.Log("transformForward * 4 is  = " + followCamera.transform.forward + " * " + moveDistance  + " moveDistance");
        setCamAndPlayerAngle.Invoke(followCamera.transform.eulerAngles.y);    //BK 9/4/23 if this works we can just call move once?
        yield return new WaitForSeconds(transformTranslateDelay);
        transform.Translate(Vector3.forward * moveDistance);
    }

    private void OnDisable()
    {
       // Debug.Log("RayCasting DISABLED OnDisable....");
        StopAllCoroutines();
    }
}
// 7 lines cut from OnZipButtonClick()
//var pRot = transform.rotation;
//var cRot = followCamera.transform.rotation;
//var peRot = transform.eulerAngles;
//var ceRot = followCamera.transform.eulerAngles;
//Debug.Log("pRot = " + pRot + "  cRot = " + cRot);
//Debug.Log("peRot = " + peRot + "  ceRot = " + ceRot);
//  transform.eulerAngles = followCamera.transform.eulerAngles;

/*  before delete comments on 9/5/23
 *     private IEnumerator ZipPlayerForward()
    {
        //setCamAngle.Invoke(followCamera.transform.eulerAngles.y);
        //yield return new WaitForSeconds(3f);
        //// Move the player forward by x meters

        //characterController.Move(followCamera.transform.forward * .2f);
        Debug.Log("transformForward * 4 is  = " + followCamera.transform.forward *4);
        setCamAndPlayerAngle.Invoke(followCamera.transform.eulerAngles.y);    //BK 9/4/23 if this works we can just call move once?
        yield return new WaitForSeconds(transformTranslateDelay);
        // Move the player forward by x meters
       // characterController.Move(followCamera.transform.forward * 4f);
        transform.Translate(Vector3.forward * 4);
    }
 */