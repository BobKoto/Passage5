using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AddRemoveChild : MonoBehaviour
{
    public GameObject child;
    public GameObject newParentGameObject;


    public Transform originalParent;
    public Transform newParent;

    Animator anim;

    public CinemachineVirtualCamera thirdPersonFollowCam;
    public CinemachineVirtualCamera freeLookCam;
    private void Start()
    {
        // Transform originalParent = child.transform.parent;
        anim = GetComponent<Animator>();
    }
    private void OnCollisionEnter(Collision collision)
    {
      //  if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Something  hit " + this.name +  collision.gameObject.name);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
      //  if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Something  left " + this.name + collision.gameObject.name);
            // Debug.Log("Player Left Carousel");
        }
    }

    private void OnTriggerEnter(Collider other)   //the triggers control 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Something  entered " + this.name + " trigger  " + other.gameObject.name);
            child.transform.SetParent(newParent);
          if (anim)  anim.SetBool("runSwitch", true);
            thirdPersonFollowCam.Priority=9;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Something  exited " + this.name + " trigger  " + other.gameObject.name);
            child.transform.SetParent(originalParent);
         if (anim)   anim.SetBool("runSwitch", false);
            // thirdPersonFollowCam.MoveToTopOfPrioritySubqueue();
            thirdPersonFollowCam.Priority=11;
            //  child.transform.SetParent(null);
        }

    }

    ////Invoked when a button is clicked.
    //public void Example(Transform newParent)
    //{
    //    // Sets "newParent" as the new parent of the child GameObject.
    //    child.transform.SetParent(newParent);

    //    // Same as above, except worldPositionStays set to false
    //    // makes the child keep its local orientation rather than
    //    // its global orientation.
    //    child.transform.SetParent(newParent, false);

    //    // Setting the parent to ‘null’ unparents the GameObject
    //    // and turns child into a top-level object in the hierarchy
    //    child.transform.SetParent(null);
    //}
}
