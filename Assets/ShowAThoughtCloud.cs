using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ShowAThoughtCloud : MonoBehaviour
{//Component of VisibilityMarkerCollider //show a thought cloud when player enters a triggered box collider or "looks(raycast)" at subject

    bool entryCommentDone, commentFound, playerIsInsideCollider;
    // Define a delegate with the same signature as the method you want to invoke
    public delegate void CloudTextDelegate(int value, int timeout, string caption);
    // Declare an instance of the delegate
    public CloudTextDelegate cloudTextDelegate;
    GameObject textCloudHandler;
    string entryComment1 = "#The Teachers. #More silicon than human brain cells by now.";

    //stuff for Raycasting
    CinemachineBrain cinemachineBrain;
    //public CinemachineVirtualCamera followCamera;
    Vector3 rayOriginFixedHeight;
    public float raycastDistance = 100.0f;
    public int thoughtCloudOption = 9;
    public int thoughtCloudTimeout = 5;
    GameObject playerArmature;
    GameObject followCamera;
    Coroutine showThoughtOnSubject;

    private void Start()
    {
        textCloudHandler = GameObject.Find("TextCloudHandleHolder");
        //11/14/23 Now do this instead of assigning in Editor  
        if (textCloudHandler != null)
            cloudTextDelegate = textCloudHandler.GetComponent<TextCloudHandler>().EnableTheTextCloud;
        // Get the CinemachineBrain component attached to the camera
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        playerArmature = GameObject.Find("PlayerArmature");
        followCamera = GameObject.Find ("PlayerFollowCamera");   //Find("PlayerFollowCamera");
        //showThoughtOnSubject = ShowThoughtOnSubject();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInsideCollider = true;
            // Object went inside the collider, stage the comment string 
            if (!entryCommentDone)
            {
                ChooseEntryComment();  //sets the thought string based on name of collider entered 
                //if (commentFound) cloudTextDelegate.Invoke(7, 5, entryComment1);  //11/14/23 moved to - but keep for possible use
                //entryCommentDone = true;
                if (showThoughtOnSubject == null) showThoughtOnSubject = StartCoroutine(ShowThoughtOnSubject());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Object went inside the collider, stage the comment string 
            playerIsInsideCollider = false;
            if (showThoughtOnSubject != null) StopCoroutine(showThoughtOnSubject);
        }
    }
    IEnumerator ShowThoughtOnSubject()
    {
        WaitForSeconds waitTime = new WaitForSeconds(1);  //used to limit raycast frequency
       // Debug.Log(this.name + " started ShowThoughtOnSubject() ");
        while (true)
        {
            if (playerIsInsideCollider && !entryCommentDone)
            {
                // Calculate the ray's origin and direction from the Cinemachine camera
                Vector3 halfHeightOfCamera = new(0f, followCamera.transform.position.y / 2, 0f);
                Vector3 rayOrigin = followCamera.transform.position - halfHeightOfCamera;
                Vector3 rayDirection = new(followCamera.transform.forward.x, 0, followCamera.transform.forward.z);

                rayOriginFixedHeight = new Vector3(rayOrigin.x, playerArmature.transform.position.y + 2, rayOrigin.z);
                // Perform the raycast
                if (Physics.Raycast(rayOriginFixedHeight, rayDirection, out RaycastHit hit, raycastDistance))
                {
                    // A collider was hit. Is it one that should trigger a thought cloud? 
                    string hitName = hit.collider.name;
                    ShowThoughtIfSubjectHit(hitName);
                }
            }
            yield return waitTime;
        }
    }

    void ShowThoughtIfSubjectHit(string hitName)   //here we already know we're in trigger collider & which one
    {
        switch (hitName)   //
        {
            case "InfluencersForceField":
            case "HiveForceField":
            case "XorBitAntForceField": 
                if (commentFound) cloudTextDelegate.Invoke(thoughtCloudOption, thoughtCloudTimeout, entryComment1);
                entryCommentDone = true;
                return;
            default: return;
        }
    }
    void ChooseEntryComment()   //choose a comment based on our GO name  - coming soon
    {//here we add GameObjects w/colliders (the cases) that want to emit thoughts

        switch (this.name)   //
        {
            case "VisibilityMarkerColliderInfluencers":
                entryComment1 = "#The Influencers. #What they'll do for cha-ching.";
                break;
            case "VisibilityMarkerColliderHive":
                entryComment1 = "#The Teachers. #More silicon than human brain cells by now.";
                break;
            case "VisibilityMarkerColliderXorBitAnts":
                entryComment1 = "#The XorBit Ants. #Lies and Misinfo run amok!";
                break;
            default: Debug.Log(this.name + " has an Invalid Case!!! Misspelled or Missing?");
                commentFound = false;
                return;
        }
      //  Debug.Log(this.name + " has a thought to show");
        commentFound = true;
    }

}
//private void Update()    //Should this be a Coroutine? We really don't need to raycast every frame...
//{
//    if (playerIsInsideCollider  && !entryCommentDone)
//    {
//            // Calculate the ray's origin and direction from the Cinemachine camera

//            Vector3 halfHeightOfCamera = new(0f, followCamera.transform.position.y / 2, 0f);
//            Vector3 rayOrigin = followCamera.transform.position - halfHeightOfCamera;
//            Vector3 rayDirection = new(followCamera.transform.forward.x, 0, followCamera.transform.forward.z);

//            rayOriginFixedHeight = new Vector3(rayOrigin.x, playerArmature.transform.position.y + 2, rayOrigin.z);
//            // Perform the raycast
//            if (Physics.Raycast(rayOriginFixedHeight, rayDirection, out RaycastHit hit, raycastDistance))
//            {
//                // A collider was hit. Is it one that should trigger a thought cloud? 
//                string hitName = hit.collider.name;
//                ShowThoughtIfSubjectHit(hitName);

//            }
//    }
//}
//private void OnTriggerStay(Collider other)
//{
//    if (other.CompareTag("Player"))
//    {
//        // Object went inside the collider, stage the comment string 
//        playerIsInsideCollider = true;
//    }
//}