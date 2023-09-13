using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public float elevatorHeight = 11f;
    public float ascentSpeed = 1f;
    public float ascentIncrement = .1f;
    public float elevatorFloorPosition = .1f;
    bool elevatorTriggered, elevatorLowered;
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
            Debug.Log(" Player is on Elevator...........");
            if (!elevatorTriggered) StartCoroutine(RaiseElevator(ascentIncrement));
            elevatorTriggered = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(" Elevator EXITED...........  " + other.gameObject);
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log(" Player is on Elevator...........");
            if (!elevatorLowered) StartCoroutine(LowerElevator(ascentIncrement));
            elevatorLowered = true;
        }
    }
    IEnumerator RaiseElevator(float increment)
    {
        while (transform.position.y < elevatorHeight)
        {
            transform.Translate(Vector3.up * Time.deltaTime, Space.World);
            yield return new WaitForSeconds(increment);
        }
    }
    IEnumerator LowerElevator(float increment)
    {
        while (transform.position.y > elevatorFloorPosition)
        {
            transform.Translate(Vector3.down * Time.deltaTime, Space.World);
            yield return new WaitForSeconds(increment);
        }
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
