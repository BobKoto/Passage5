using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public float elevatorHeight = 11f;
    public float ascentSpeed = 1.5f;
    public float ascentIncrement = .1f;
    public float elevatorFloorPosition = .1f;
    bool elevatorAtTop, elevatorIsRising, elevatorIsGrounded;
    Coroutine raiseCoroutine, lowerCoroutine;
    // Start is called before the first frame update
    void Start()
    {
      //  Debug.Log(" Hello from Elevator ...........  ");
        elevatorIsGrounded = true;
    }
    private void OnTriggerEnter(Collider other)
    {
      //  Debug.Log(" Elevator ENTERED ...........by  " + other.gameObject);
        if (!other.gameObject.CompareTag("Player")) return;
        if (elevatorIsGrounded)
        {
            raiseCoroutine = StartCoroutine(RaiseElevator());
        }
    }
    private void OnTriggerExit(Collider other)
    {
     //   Debug.Log(" Elevator EXITED...........by  " + other.gameObject);
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
      //  Debug.Log("Raise elevator Started...");
        elevatorIsRising = true;
        elevatorIsGrounded = false;
        while (transform.position.y < elevatorHeight)
        {
            transform.Translate(Vector3.up * Time.deltaTime * ascentSpeed, Space.World);
            yield return new WaitForEndOfFrame(); // WaitForSeconds(ascentIncrement);
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
            transform.Translate(Vector3.down * Time.deltaTime * ascentSpeed, Space.World);
            yield return new WaitForEndOfFrame();  //WaitForSeconds(ascentIncrement);
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
