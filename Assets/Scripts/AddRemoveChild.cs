using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
namespace CarouselAndMovingPlatforms 
{ 
public class AddRemoveChild : MonoBehaviour
{   //Component of Carousel AND MovingPlatforms
    public GameObject child;
    public GameObject stopButton;
    public GameObject goButton;

    //public bool platformStopped { get; set; }
    public static bool playerIsOnYellowPlatform;

    public Transform originalParent;
    public Transform newParent;

    Animator anim;
    MovingPlatform movingPlatform;

    private void Start()
    {
        // Debug.Log("AddRemoveChild reports this.name is " + this.name);
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
            Debug.Log("Something  hit " + this.name + collision.gameObject.name);
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
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Something  exited " + this.name + " trigger  " + other.gameObject.name);
            child.transform.SetParent(originalParent);
            HandleTriggerExitPerGameObject(this.name);
        }
    }
    void HandleTriggerEnterPerGameObject(string thisName)
    {
        switch (thisName)
        {
            case "MovingPlatform":
                {
                    movingPlatform.speed = 5;  //This is not an animation - its an Update() method
                    playerIsOnYellowPlatform = true;
                    stopButton.SetActive(true);
                    goButton.SetActive(false);
                    break;
                }
            case "MovingPlatformGreen":
                {
                    movingPlatform.speed = 5;  //This is not an animation - its an Update() method
                    stopButton.SetActive(true);
                    goButton.SetActive(false);
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
                    if (stopButton) stopButton.SetActive(false);
                    if (goButton) goButton.SetActive(false);
                    playerIsOnYellowPlatform = false;
                    break;
                }
            case "MovingPlatformGreen":
                {
                    movingPlatform.speed = 0;  //This is not an animation - its an Update() method
                    if (stopButton) stopButton.SetActive(false);
                    if (goButton) goButton.SetActive(false);
                    break;
                }
            case "CarouselTest":
                {
                    if (anim) anim.SetBool("runSwitch", false);
                    break;
                }
        }
    }
    public void OnPlatformStopButtonPressed()
    {
        movingPlatform.speed = 0;
        stopButton.SetActive(false);
        goButton.SetActive(true);
    }
    public void OnPlatformGoButtonPressed()
    {
        movingPlatform.speed = 5;
        stopButton.SetActive(true);
        goButton.SetActive(false);
    }
    public void OnPlatformGreenStopButtonPressed()
    {
        movingPlatform.speed = 0;
        stopButton.SetActive(false);
        goButton.SetActive(true);
    }
    public void OnPlatformGreenGoButtonPressed()
    {
        movingPlatform.speed = 5;
        stopButton.SetActive(true);
        goButton.SetActive(false);
    }
} //end class
} //end namespace CarouselAndMovingPlatforms

