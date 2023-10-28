using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{  //Components of Elevators
    AudioManager audioManager;
    public float elevatorHeight = 11f;
    public float ascentSpeed = 1.5f;
    public float descentSpeed = 3.6f;
    public float ascentIncrement = .1f;
    public float elevatorFloorPosition = .1f;

    public bool elevatorAtTop, elevatorIsRising;  //public just so we can see in editor for testing
    bool elevatorIsGrounded = true;
    bool playerIsOnElevator;
    Vector3 originalPosition, targetPosition;
    Transform target;
    GameObject jumpButton;
    GameObject destCube;
    GameObject blueCubeChild;
    // Stuff for swapping child/parent
    GameObject thePlayer;
    public Transform originalParent;
    public Transform newParent;

    private void Awake()
    {
        Application.targetFrameRate = 30;
    }
    void Start()
    {
        // Set child/parent stuff up

        thePlayer = GameObject.Find("PlayerArmature");
        if (thePlayer) originalParent = thePlayer.GetComponent<Transform>().transform.parent;            //transform.parent.name
        newParent = transform;
        //Debug.Log("Elevator: originalparent = " + originalParent.name + "  newparent (will be) = " + newParent.name);
        destCube = GameObject.CreatePrimitive(PrimitiveType.Cube);  //so we can use vector3.MoveTowards
        destCube.GetComponent<MeshRenderer>().enabled = false; 
        destCube.GetComponent<BoxCollider>().enabled = false;
        //  Debug.Log(" Hello from Elevator ...........  ");
        jumpButton = GameObject.Find("UI_Virtual_Button_Jump");
        if (!audioManager) audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        originalPosition = transform.position;
        target = destCube.transform;
        target.position = new Vector3(transform.position.x, elevatorHeight, transform.position.z);
        blueCubeChild = this.gameObject.transform.GetChild(0).gameObject;
    }
    private void Update()
    {
    // FixedUpdate() and using fixedDeltaTime stutters pretty bad - so unless we learn different Update() it is
        
        if (playerIsOnElevator)
        {
            ascentSpeed = Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, ascentSpeed);
            if ((Vector3.Distance(transform.position,targetPosition) < 1) && audioManager.audioSource.isPlaying)
            {
                var dist = Vector3.Distance(transform.position, targetPosition);
                Debug.Log("Distance = " + dist);
                audioManager.audioSource.Stop();
            }
        }

    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (!other.gameObject.CompareTag("Player")) return;
    //    if (elevatorEmpty)  //player has entered so disable jump button (here read as "was empty")
    //    {
    //    Debug.Log(this.name + " ENTERED by " + other.gameObject + " atTop " + elevatorAtTop + " rising " +  elevatorIsRising + " grd " + elevatorIsGrounded);
    //        if (jumpButton) jumpButton.SetActive(false);
    //        elevatorEmpty = false;  //10/18/23 moved into this block from just below
    //    }
    //    if (elevatorIsGrounded)
    //    {
    //        raiseCoroutine = StartCoroutine(RaiseElevator());
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        audioManager.PlayAudio(audioManager.BkGround01);
        blueCubeChild.SetActive(false);
        //Debug.Log("? entered or Contact w/Elevator... " + other);
        if (!other.gameObject.CompareTag("Player")) return;
        elevatorIsGrounded = false;
        playerIsOnElevator = true;
        thePlayer.transform.SetParent(newParent);
    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (!other.gameObject.CompareTag("Player")) return;
    //    if (transform.position.y < elevatorHeight)
    //    {
    //        transform.Translate(Vector3.up * Time.deltaTime * ascentSpeed, Space.World);  //STILL stutters :(
    //    }
    //}
    //IEnumerator RaiseElevator()
    //{
    //    elevatorIsRising = true;
    //    elevatorIsGrounded = false;
    //    Debug.Log("Raise elevator Coroutine Started...");

    //    while (transform.position.y < elevatorHeight)
    //    {
    //        transform.Translate(Vector3.up * Time.deltaTime * ascentSpeed, Space.World); //10/18/23 changed to fixedDeltaTime  - worse stutter
    //        yield return null; //10/9/23  new WaitForEndOfFrame(); // WaitForSeconds(ascentIncrement);
    //        //yield return new WaitForFixedUpdate(); //10/18/23 make ascent smoother and maybe stop intermittant stutter/failure - got worse???

    //    }
    //    elevatorAtTop = true;
    //    elevatorIsRising = false;
    //    //elevatorIsGrounded = false;
    //    Debug.Log("Raise elevator finished...");
    //}
    private void OnTriggerExit(Collider other)
    {
        if (audioManager.audioSource.isPlaying) audioManager.audioSource.Stop();
        blueCubeChild.SetActive(true);
        //Debug.Log("? exited or LOST Contact w/Elevator... " + other);
        if (!other.gameObject.CompareTag("Player")) return;
        thePlayer.transform.SetParent(originalParent);
        audioManager.PlayAudio(audioManager.tick);
        //Debug.Log("Player exited or LOST Contact w/Elevator...");
        playerIsOnElevator = false;
        if (!elevatorIsGrounded)
        {
            LowerElevator();
        }
    }
    //private void OnTriggerExit(Collider other)
    //{
    //    if (!other.gameObject.CompareTag("Player")) return;
    //    elevatorEmpty = true;
    //    if (jumpButton) jumpButton.SetActive(true);
    //    //   Debug.Log(" Elevator EXITED...........by  " + other.gameObject);
    //    Debug.Log(this.name + " Elevator EXITED by " + other.gameObject + " atTop " + elevatorAtTop + " rising " + elevatorIsRising + " grd " + elevatorIsGrounded);

    //    if (elevatorAtTop)
    //    {
    //        LowerElevator();
    //        return;
    //    }
    //    if (elevatorIsRising)
    //    {
    //        StopCoroutine(raiseCoroutine);
    //        elevatorIsRising = false;
    //        LowerElevator();
    //    }
    //}
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
