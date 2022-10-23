using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class OpenDoor : MonoBehaviour
{
     Animator anim;
    public AudioManager audioManager;
    public Transform target;
    public float maxDistance = 1.5f;
    [Range(0f, 360f)]
    public float angle = 45f;

    public float sphereRadius = 2f;
    public float transformRightXAdjust = 2f;
    public float transformSphereCheckXAdjust = 2f;
  //  bool tooFarToPress, inRangeToPress;
    Material mat;
    MeshRenderer meshRenderer;
    Vector3 transformRightAdjusted;
    Vector3 transformSphereCheck;
    //By The Cookbook
    public bool targetIsVisible { get; private set; }
    [SerializeField]
    bool visualize = true;

    // Start is called before the first frame update
    void Start()
    {
        anim = GameObject.Find("SlidingDoor1").GetComponent<Animator>();
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        mat = GetComponent<Renderer>().material;
        if (!target)
        {
         //   target = GameObject.Find("Right_Hand").GetComponent<Transform>(); //10/22 try looking at the collider - see next line 
            target = GameObject.Find("Right_Hand").GetComponent<SphereCollider>().transform;
            //tooFarToPress = true; // IF and ONLY IF we start too far from a pressable object
        }
        transformRightAdjusted = new Vector3(transform.position.x - transformRightXAdjust, transform.position.y, transform.position.z);
        transformSphereCheck = new Vector3(transform.position.x - transformSphereCheckXAdjust, transform.position.y, transform.position.z);

        //  Debug.Log("hello from " + this.name );
    }

    // Update is called once per frame
    void Update()
    {
        //targetIsVisible = CheckVisibility();    //CheckVisibilityWithSphere();
        targetIsVisible = CheckVisibilityWithSphere();
        if (visualize)
        {
            Color color = targetIsVisible ? Color.yellow : Color.white;
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
        float degreesToTarget = Vector3.Angle(transformRightAdjusted, directionToTarget);
        bool withinArc = degreesToTarget < (angle / 2);
        if (withinArc == false)
        {
            return false;
        }
        float distanceToTarget = directionToTarget.magnitude;
        float rayDistance = Mathf.Min(maxDistance, distanceToTarget);
        Ray ray = new Ray(transform.position, directionToTarget);

        var canSee = false;
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.transform == target)
            {
                canSee = true;
            }
            Debug.DrawLine(transform.position, hit.point);
        }
        else
        {
            Debug.DrawRay(transform.position, directionToTarget.normalized * rayDistance);
        }
        return canSee;
    }
    public bool CheckVisibilityWithSphere()
    {
        if (Physics.CheckSphere(transformSphereCheck, sphereRadius))
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
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(transformSphereCheck, sphereRadius);
    }
    private void OnCollisionEnter(Collision collision)
    {
        print("Collided with " + collision.collider);
        mat.color = Color.green;
        audioManager.PlayAudio(audioManager.clipapert);
        anim.SetTrigger("DoorSlideDown");
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
        var visibility = target as OpenDoor;
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