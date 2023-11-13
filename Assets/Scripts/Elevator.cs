using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{  //Components of Elevators
    AudioManager audioManager;
    [Tooltip ("Set Elevator height value here in Editor for new Elevators")]
    public float elevatorHeight = 11f;  //Set this in Editor 
    public float ascentSpeed = 1.5f;
    public float descentSpeed = 3.6f;
    public float ascentIncrement = .1f;
    public float elevatorFloorPosition = .1f;

    public bool elevatorAtTop, elevatorIsRising;  //public just so we can see in editor for testing
    bool elevatorIsGrounded = true;
    bool playerIsOnElevator;
    Vector3 originalPosition;
    Transform target;
    GameObject jumpButton;
    GameObject destCube;
    GameObject blueCubeChild;
    // Stuff for swapping child/parent
    GameObject thePlayer;
    public Transform originalParent;
    public Transform newParent;
    const string elevatorComment1 = "#Who makes these elevators?";
   // public CloudTextEvent m_CloudTextEvent;  //for TextCloud //11/9/23 replaced (editor assignment) with method assignment in this code 
    bool elevatorCommentDone;
    // Define a delegate with the same signature as the method you want to invoke
    public delegate void CloudTextDelegate(int value, int timeout, string caption);
    // Declare an instance of the delegate
    public CloudTextDelegate cloudTextDelegate;
    GameObject textCloudHandler;

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

        destCube = GameObject.CreatePrimitive(PrimitiveType.Cube);  //so we can use vector3.MoveTowards
        destCube.GetComponent<MeshRenderer>().enabled = false; 
        destCube.GetComponent<BoxCollider>().enabled = false;
        //  Debug.Log(" Hello from Elevator ...........  ");
        if (!audioManager) audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        originalPosition = transform.position;
        target = destCube.transform;
        target.position = new Vector3(transform.position.x, elevatorHeight, transform.position.z);
        blueCubeChild = gameObject.transform.GetChild(0).gameObject;

        textCloudHandler = GameObject.Find("TextCloudHandleHolder");
        cloudTextDelegate = textCloudHandler.GetComponent<TextCloudHandler>().EnableTheTextCloud;  
    }
    private void Update()
    {
    // FixedUpdate() and using fixedDeltaTime stutters pretty bad - so unless we learn different Update() it is 
        if (playerIsOnElevator)
        {
            ascentSpeed = Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, ascentSpeed);

            if ((Vector3.Distance(transform.position, target.position) < elevatorHeight /2) && !elevatorCommentDone)
            {
                cloudTextDelegate.Invoke(7, 3, elevatorComment1);
                elevatorCommentDone = true;
            }

            if (Vector3.Distance(transform.position,target.position) < 1 && audioManager.audioSource.isPlaying)
            {
                audioManager.audioSource.Stop();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("? entered or Contact w/Elevator... " + other);
        if (!other.gameObject.CompareTag("Player")) return;
        audioManager.audioSource.volume = .5f;
        audioManager.PlayAudio(audioManager.elevatorUp,true);
        blueCubeChild.SetActive(false);
        elevatorIsGrounded = false;
        playerIsOnElevator = true;
        thePlayer.transform.SetParent(newParent);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        if (audioManager.audioSource.isPlaying) audioManager.audioSource.Stop();
        blueCubeChild.SetActive(true);
        thePlayer.transform.SetParent(originalParent);
        audioManager.PlayAudio(audioManager.tick);
        playerIsOnElevator = false;
        if (!elevatorIsGrounded)
        {
            LowerElevator();
        }
    }
    void LowerElevator()
    {
        transform.position = originalPosition;  //10/10/23 down fast and mostly unseen 
        elevatorAtTop = false;
        elevatorIsGrounded = true;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
/*
 * using UnityEngine;

public class EventInvoker : MonoBehaviour
{
    // Define a delegate with the same signature as the method you want to invoke
    public delegate void CloudTextDelegate(int value, float timeout, string caption);

    // Declare an instance of the delegate
    public CloudTextDelegate cloudTextDelegate;

    void Start()
    {
        // Assign the target GameObject and method to the delegate
        GameObject targetObject = /* Set your target object here * /
cloudTextDelegate = targetObject.GetComponent<YourTargetScript>().YourTargetMethod;

// Invoke the delegate
if (cloudTextDelegate != null)
{
    cloudTextDelegate.Invoke(5, 10.0f, "Hello, World!");
}
    }
}
*/