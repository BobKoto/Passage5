using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public float elevatorHeight = 11f;
    public float ascentSpeed = 1.5f;
    public float ascentIncrement = .1f;
    public float elevatorFloorPosition = .1f;
    bool elevatorTriggered, elevatorLowered;
    Coroutine raiseCoroutine, lowerCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(" Hello from Elevator ...........  ");
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log(" Elevator ...........  " + collision.gameObject );
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        Debug.Log(" Player is on Elevator...........");
    //        if (!elevatorTriggered) StartCoroutine(RaiseElevator(ascentIncrement));
    //        elevatorTriggered = true;
    //    }

    //}
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(" Elevator ...........  " + other.gameObject);
        if (other.gameObject.CompareTag("Player"))
        {
            if (lowerCoroutine != null && !lowerCoroutine.Equals(null))
            {
                Debug.Log(" player entered Raise trigger  but lowerElevator is running.............");
                return;
            }
                Debug.Log(" Player is on Elevator...........");
            if (!elevatorTriggered)
            {
                raiseCoroutine = StartCoroutine(RaiseElevator());
            }

            elevatorTriggered = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(" Elevator EXITED...........by  " + other.gameObject);
        if (other.gameObject.CompareTag("Player"))
        {
            if (raiseCoroutine != null && !raiseCoroutine.Equals(null))
            {
                Debug.Log("RaiseElevator is running......Stopping ");
                StopCoroutine(raiseCoroutine);
                //Debug.Log(" Player is on Elevator...........");
                if (!elevatorLowered)
                {
                    lowerCoroutine =  StartCoroutine(LowerElevator());
                }
                // elevatorLowered = true;
            }
        }
    }
    IEnumerator RaiseElevator()
    {
        while (transform.position.y < elevatorHeight)
        {
            transform.Translate(Vector3.up * Time.deltaTime * ascentSpeed, Space.World);
            yield return new WaitForSeconds(ascentIncrement);
        }
    }
    IEnumerator LowerElevator()
    {
        Debug.Log("Lower elevator Started...");
        while (transform.position.y > elevatorFloorPosition)
        {
            transform.Translate(Vector3.down * Time.deltaTime * ascentSpeed, Space.World);
            yield return new WaitForSeconds(ascentIncrement);
        }
        elevatorLowered = false;
        elevatorTriggered = false;
        Debug.Log("Lower elevator finished...");
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
