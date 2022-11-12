using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AddRemoveChild : MonoBehaviour
{
    public GameObject child;
   // public GameObject newParentGameObject;

    public Transform originalParent;
    public Transform newParent;

    Animator anim;

    public CinemachineVirtualCamera thirdPersonFollowCam;
    public CinemachineVirtualCamera freeLookCam;

    MovingPlatform movingPlatform;
   // MovingPlatformGreen movingPlatformGreen;
    private void Start()
    {
        Debug.Log("AddRemoveChild reports this.name is " + this.name);
        // Transform originalParent = child.transform.parent;
       anim = GetComponent<Animator>();
        switch (this.name)
        {
            case "MovingPlatform":
                {
                    movingPlatform = GameObject.Find("MovingPlatform").GetComponent<MovingPlatform>();
                    movingPlatform.speed = 0;
                    break;
                }
            case "MovingPlatformGreen":
                {
                    movingPlatform = GameObject.Find("MovingPlatformGreen").GetComponent<MovingPlatform>();
                    movingPlatform.speed = 0;
                    break;
                }
        }
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
            HandleTriggerEnterPerGameObject(this.name);
      //    if (anim)  anim.SetBool("runSwitch", true);
            thirdPersonFollowCam.Priority=9; //Kludge to make freeLook cam ON
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Something  exited " + this.name + " trigger  " + other.gameObject.name);
            child.transform.SetParent(originalParent);
            HandleTriggerExitPerGameObject(this.name);
            //if (anim)   anim.SetBool("runSwitch", false);
            // thirdPersonFollowCam.MoveToTopOfPrioritySubqueue();
            thirdPersonFollowCam.Priority=11;  //Kludge to make followPlayer cam ON
            //  child.transform.SetParent(null);
        }

    }
    void HandleTriggerEnterPerGameObject(string thisName)
    {
       switch (thisName)
        {
            case "MovingPlatform":
                {
                    movingPlatform.speed = 5;  //This is not an animation - its an Update() method
                    break;
                }
            case "MovingPlatformGreen":
                {
                    movingPlatform.speed = 5;  //This is not an animation - its an Update() method
                    break;
                }
            case "CarouselTest":
                {
                    if (anim) anim.SetBool("runSwitch", true);
                    break;
                }

        }
    }
    void HandleTriggerExitPerGameObject(string thisName)
    {
        switch (thisName)
        {
            case "MovingPlatform":
                {
                    movingPlatform.speed = 0;  //This is not an animation - its an Update() method
                    break;
                }
            case "MovingPlatformGreen":
                {
                    movingPlatform.speed = 0;  //This is not an animation - its an Update() method
                    break;
                }
            case "CarouselTest":
                {
                    if (anim) anim.SetBool("runSwitch", false);
                    break;
                }

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
