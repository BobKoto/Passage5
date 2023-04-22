using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class OpenDoor : MonoBehaviour
{
     Animator anim;
    public AudioManager audioManager;
    public Transform target;
    public Transform player;
    public GameObject doorOpener;  //the button and text prompts 
    public GameObject stepsRaiser; //the button and text prompts 
    public float maxDistance = 1.5f;
    [Range(0f, 360f)]
    public float angle = 45f;

    public float sphereRadius = 2f;
    public float transformRightXAdjust = 2f;
    public float transformXOffset = 1f;
    public float transformZOffset = 1f;
    public float yAngleMin = -104f;
    public float yAngleMax = -74f; 
    public GameObject pressToOpenButton;
    public string operateButton = "DoorSlideDown";  // we change in the prefab(s)
   
    Vector3 playerRotationY;
    //  bool tooFarToPress, inRangeToPress;
    Material mat;
    MeshRenderer meshRenderer;
    //Vector3 transformRightAdjusted;
    Vector3 transformSphereCheck;
    //By The Cookbook
    public bool targetIsVisible { get; private set; }
    [SerializeField]
    bool visualize = true;
    public CloudTextEvent m_MyEvent;
    const string firstDoor = "#Good Robot.";
    const string firstSteps = "#Take a dip!";
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("hello from " + this.name);
        string myName = this.name;
        switch (myName)
        {
            case "DoorOpener":
                {
                    anim = GameObject.Find("SlidingDoor1").GetComponent<Animator>();
                    break;
                }
            case "StepsRaise":
                {
                    anim = GameObject.Find("RaiseSteps1").GetComponent<Animator>();
                    break;
                }
        }

 

        if (!audioManager) audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        mat = GetComponent<Renderer>().material;
        if (!target)  // we didn't set a target in the editor 
        {
            target = GameObject.Find
            ("/NestedParentArmature_Unpack/PlayerArmature/Skeleton/Hips/Spine/Chest").GetComponent<SphereCollider>().transform;

          //   ("/NestedParentArmature_Unpack/PlayerArmature").GetComponent<CharacterController>().transform;  //maybe charctrllr



            //("/NestedParentArmature_Unpack/PlayerArmature/Skeleton/Hips/Spine/Chest/UpperChest/Right_Shoulder/Right_UpperArm/Right_LowerArm/Right_Hand")
            //.GetComponent<SphereCollider>().transform; //10/22 try looking at the collider - see next line 
            //  target = GameObject.Find("Right_Hand").GetComponent<SphereCollider>().transform;  //Original working line 
            //tooFarToPress = true; // IF and ONLY IF we start too far from a pressable object

        }
        //transformRightAdjusted = new Vector3(transform.position.x - transformRightXAdjust, transform.position.y, transform.position.z);
        //transformSphereCheck = new Vector3(
        //    transform.position.x + transformXOffset,
        //    transform.position.y,
        //    transform.position.z + transformZOffset);

        pressToOpenButton.SetActive(false);

        // playerRotationY = player.rotation.y;
        //Debug.Log("player y = " + playerRotationY + " transform.rotation = " + player.rotation + " and Transform.Rotation = " + player.T );
        playerRotationY = player.transform.eulerAngles;
        //  Debug.Log(" playerY rotation = " + playerRotationY);
        if (m_MyEvent == null)
            m_MyEvent = new CloudTextEvent();

      //  m_MyEvent.AddListener(EnableTheTextCloud);

    }

    // Update is called once per frame
    void Update()
    {
        targetIsVisible = CheckVisibility();    //CheckVisibilityWithSphere();
       // targetIsVisible = CheckVisibilityWithSphere();
        if (visualize)   // not sure about this in the long run but Color uses it for now 
        {
            if (targetIsVisible)
            {
                pressToOpenButton.SetActive(true);
            }
            else
            {
                pressToOpenButton.SetActive(false);
            }
            Color color = targetIsVisible ? Color.green : Color.red;
            mat.color = color;
        }

        ///Below commented and replaced with Cookbook code above
            //if (dist <= maxDistance)  //we are close enough 

            //{
            //    if (!inRangeToPress && tooFarToPress)
            //    {

            //        //  print("Distance to other just became Close : " + dist);
            //        inRangeToPress = !inRangeToPress;
            //        tooFarToPress = !tooFarToPress;
            //        mat.color = Color.red;
            //        return;  //Added 10/20/22 seems to be ok, just to save a cycle or 2 on the CPU...
            //    }
            //}
            //if (dist > maxDistance)  //we are too far   AND always true on start 
            //{
            //    if (inRangeToPress && !tooFarToPress)
            //    {
            //        //  print("Distance to other just became too Far: " + dist);
            //        tooFarToPress = !tooFarToPress;
            //        inRangeToPress = !inRangeToPress;
            //        mat.color = Color.blue;
            //    }
            //}
    }

    public bool CheckVisibility()
    {
        Vector3 directionToTarget = target.position - transform.position;
        float degreesToTarget = Vector3.Angle(transform.right, directionToTarget);
        //new Vector3(transform.position.x,transform.position.y,transform.position.z)
        bool withinArc = degreesToTarget < (angle / 2);
        if (withinArc == false)
        {
            return false;
        }
        float distanceToTarget = directionToTarget.magnitude;
        float rayDistance = Mathf.Min(maxDistance, distanceToTarget);
        Ray ray = new Ray(transform.position, directionToTarget);

        bool canSee = false;
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            float playerRotation = player.transform.eulerAngles.y - 360f;  // fuck if I know why we gotta subtract 360 - but I know

            canSee = (hit.collider.transform == target
                && (playerRotation > yAngleMin && playerRotation < yAngleMax)) ;  //10/23/22 added 2 && conditions
            //{
            //  //  Debug.Log(this.name + " canSee is True and playerRotationY = " + playerRotationY);
            //    canSee = true;
            //}
            Debug.DrawLine(transform.position, hit.point);
        }
        else
        {
           // Debug.Log(this.name + "  ray hit nothing, rayDistance  = " + rayDistance);
            Debug.DrawRay(transform.position, directionToTarget.normalized * rayDistance);
        }
        return canSee;
    }
    public bool CheckVisibilityWithSphere()
    {
        if (Physics.CheckSphere(transformSphereCheck, sphereRadius))   //Make sure the GizmoDrawSphere matches this
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void OnDrawGizmosSelected()
    {
        // Draw a black sphere at the transform's position
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transformSphereCheck, sphereRadius);
    }
    private void OnCollisionEnter(Collision collision)   //robot touched the collider so do the animation and deactivate the button and text
    {
        print("Collided with " + collision.collider);
        mat.color = Color.green;
        audioManager.PlayAudio(audioManager.clipapert);
        anim.SetTrigger(operateButton);
        string myName = this.name;
        switch (myName)
        {
            case "DoorOpener":
                {
                    doorOpener.SetActive(false);
                    pressToOpenButton.SetActive(false);
                    TellTextCloud(firstDoor);
                    break;
                }
            case "StepsRaise":
                {
                    stepsRaiser.SetActive(false);
                    pressToOpenButton.SetActive(false);
                    TellTextCloud(firstSteps);
                    break;  
                }
        }
    }
    public void TellTextCloud(string caption)
    {
        m_MyEvent.Invoke(5, 4, caption);
    }
}

#if UNITY_EDITOR
// do some custom Editor stuff 
[CustomEditor(typeof(OpenDoor))]
public class OpenDoorEditor : Editor
{
    private void OnSceneGUI()
    {
        // Debug.Log("entered editor code");  //runs every frame so we know and we commented
        OpenDoor visibility = target as OpenDoor;
        Handles.color = new Color(1, 1, 1, 0.1f);
        Vector3 forwardPointMinusHalfAngle = Quaternion.Euler(0, -visibility.angle / 2, 0) * visibility.transform.right;
        Vector3 arcStart = forwardPointMinusHalfAngle * visibility.maxDistance;
        Handles.DrawSolidArc(
            visibility.transform.position,
            Vector3.up,
            arcStart,
            visibility.angle,
            visibility.maxDistance);
        Handles.color = Color.white;
        Vector3 handlePosition = visibility.transform.position +
            visibility.transform.right * visibility.maxDistance;

        visibility.maxDistance = Handles.ScaleValueHandle(
            visibility.maxDistance,
            handlePosition,
            visibility.transform.rotation,
            1,
            Handles.ConeHandleCap,
            0.25f);
    }
}
#endif