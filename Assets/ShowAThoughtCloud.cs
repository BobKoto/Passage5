using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAThoughtCloud : MonoBehaviour
{//Component of VisibilityMarkerCollider //show a thought cloud when player enters a triggered box collider. 

    bool entryCommentDone, commentFound;
    // Define a delegate with the same signature as the method you want to invoke
    public delegate void CloudTextDelegate(int value, int timeout, string caption);
    // Declare an instance of the delegate
    public CloudTextDelegate cloudTextDelegate;
    GameObject textCloudHandler;
    string entryComment1 = "#The Teachers. #More silicon than human brain cells by now.";

    private void Start()
    {
        textCloudHandler = GameObject.Find("TextCloudHandleHolder");
        if (textCloudHandler != null)
            cloudTextDelegate = textCloudHandler.GetComponent<TextCloudHandler>().EnableTheTextCloud;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Object went inside the collider, play the particle system
            if (!entryCommentDone)
            {
                ChooseEntryComment();
                if (commentFound) cloudTextDelegate.Invoke(7, 5, entryComment1);
                entryCommentDone = true;
            }
        }
    }
    void ChooseEntryComment()   //choose a comment based on our GO name  - coming soon
    {//here we add GameObjects w/colliders that want to emit thoughts

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
