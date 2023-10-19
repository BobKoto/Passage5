using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public float elevatorHeight = 11f;
    public float ascentSpeed = 1.5f;
    public float descentSpeed = 3.6f;
    public float ascentIncrement = .1f;
    public float elevatorFloorPosition = .1f;
    GameObject jumpButton;
    public bool elevatorAtTop, elevatorIsRising;  //public just so we can see in editor for testing
    bool elevatorEmpty = true;
    bool elevatorIsGrounded = true;
    Vector3 originalPosition;
    Coroutine raiseCoroutine; //, lowerCoroutine;
    // Start is called before the first frame update
    void Start()
    {
      //  Debug.Log(" Hello from Elevator ...........  ");
        jumpButton = GameObject.Find("UI_Virtual_Button_Jump");
        originalPosition = transform.position;
    }
    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        if (elevatorEmpty)  //player has entered so disable jump button (here read as "was empty")
        {
        Debug.Log(this.name + " ENTERED by " + other.gameObject + " atTop " + elevatorAtTop + " rising " +  elevatorIsRising + " grd " + elevatorIsGrounded);
            if (jumpButton) jumpButton.SetActive(false);
            elevatorEmpty = false;  //10/18/23 moved into this block from just below
        }
        if (elevatorIsGrounded)
        {
            raiseCoroutine = StartCoroutine(RaiseElevator());
        }
    }
    IEnumerator RaiseElevator()
    {
        elevatorIsRising = true;
        elevatorIsGrounded = false;
        Debug.Log("Raise elevator Coroutine Started...");

        while (transform.position.y < elevatorHeight)
        {
            transform.Translate(Vector3.up * Time.deltaTime * ascentSpeed, Space.World); //10/18/23 changed to fixedDeltaTime  - worse stutter
            yield return null; //10/9/23  new WaitForEndOfFrame(); // WaitForSeconds(ascentIncrement);
            //yield return new WaitForFixedUpdate(); //10/18/23 make ascent smoother and maybe stop intermittant stutter/failure - got worse???

        }
        elevatorAtTop = true;
        elevatorIsRising = false;
        //elevatorIsGrounded = false;
        Debug.Log("Raise elevator finished...");
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        elevatorEmpty = true;
        if (jumpButton) jumpButton.SetActive(true);
        //   Debug.Log(" Elevator EXITED...........by  " + other.gameObject);
        Debug.Log(this.name + " Elevator EXITED by " + other.gameObject + " atTop " + elevatorAtTop + " rising " + elevatorIsRising + " grd " + elevatorIsGrounded);

        if (elevatorAtTop)
        {
            LowerElevator();
            return;
        }
        if (elevatorIsRising)
        {
            StopCoroutine(raiseCoroutine);
            elevatorIsRising = false;
            LowerElevator();
        }
    }
    void LowerElevator()
    {
        //  Debug.Log("Lower elevator Started...");
        transform.position = originalPosition;  //10/10/23 down fast and mostly unseen 
        elevatorAtTop = false;
        elevatorIsGrounded = true;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}

//            if (raiseCoroutine != null && !raiseCoroutine.Equals(null))     //is coroutine running example
