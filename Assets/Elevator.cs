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
    public bool elevatorAtTop, elevatorIsRising, elevatorIsGrounded;  //public just so we can see in editor for testing
    bool reportOnStay = true;
    Coroutine raiseCoroutine, lowerCoroutine;
    // Start is called before the first frame update
    void Start()
    {
      //  Debug.Log(" Hello from Elevator ...........  ");
        elevatorIsGrounded = true;
        jumpButton = GameObject.Find("UI_Virtual_Button_Jump");
    }
    private void OnTriggerStay(Collider other)
    {
        if (reportOnStay)
        {
        Debug.Log(this.name + " ENTERED by " + other.gameObject + " atTop " + elevatorAtTop + " rising " +  elevatorIsRising + " grd " + elevatorIsGrounded);
            if (jumpButton) jumpButton.SetActive(false);
        }
        reportOnStay = false;
        if (!other.gameObject.CompareTag("Player")) return;
        if (elevatorIsGrounded)
        {
            raiseCoroutine = StartCoroutine(RaiseElevator());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        reportOnStay = true;
        if (jumpButton) jumpButton.SetActive(true);
        //   Debug.Log(" Elevator EXITED...........by  " + other.gameObject);
        Debug.Log(this.name + " Elevator EXITED by " + other.gameObject + " atTop " + elevatorAtTop + " rising " + elevatorIsRising + " grd " + elevatorIsGrounded);
        if (!other.gameObject.CompareTag("Player")) return;
        if (elevatorAtTop)
        {
            lowerCoroutine = StartCoroutine(LowerElevator());
            return;
        }
        if (elevatorIsRising)
        {
            StopCoroutine(raiseCoroutine);
            elevatorIsRising = false;
            lowerCoroutine = StartCoroutine(LowerElevator());
        }
    }
    IEnumerator RaiseElevator()
    {
        Debug.Log("Raise elevator Started...");
        elevatorIsRising = true;
        elevatorIsGrounded = false;
        while (transform.position.y < elevatorHeight)
        {
            transform.Translate(Vector3.up * Time.deltaTime * ascentSpeed, Space.World);
            yield return null; //10/9/23  new WaitForEndOfFrame(); // WaitForSeconds(ascentIncrement);
        }
        elevatorAtTop = true;
        elevatorIsRising = false;
        //elevatorIsGrounded = false;
     //   Debug.Log("Raise elevator finished...");
    }
    IEnumerator LowerElevator()
    {
      //  Debug.Log("Lower elevator Started...");
        elevatorAtTop = false;
        while (transform.position.y > elevatorFloorPosition)
        {
            transform.Translate(Vector3.down * Time.deltaTime * descentSpeed, Space.World);
            yield return null; //10/9/23  new WaitForEndOfFrame();  //WaitForSeconds(ascentIncrement);
        }
        elevatorIsGrounded = true;
     //   Debug.Log("Lower elevator finished...");
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}

//            if (raiseCoroutine != null && !raiseCoroutine.Equals(null))     //is coroutine running example
