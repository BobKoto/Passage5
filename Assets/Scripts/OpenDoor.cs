using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class OpenDoor : MonoBehaviour
{
     Animator anim;
    //public AudioSource audiosource;
    //public AudioClip clip;
    public AudioManager audioManager;
    public Transform otherTransform;
    public float maxDistance = 1.5f;
    public float angle = 45f;
    bool tooFarToPress, inRangeToPress;
    Material mat;
    MeshRenderer meshRenderer;
    //By The Cookbook
    

    // Start is called before the first frame update
    void Start()
    {
        anim = GameObject.Find("SlidingDoor1").GetComponent<Animator>();
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        mat = GetComponent<MeshRenderer>().material;
        if (!otherTransform)
        {
            otherTransform = GameObject.Find("Right_Hand").GetComponent<Transform>();
            // otherTransform = GameObject.Find("PlayerArmature").GetComponent<Transform>();
            tooFarToPress = true; // IF and ONLY IF we start too far from a pressable object
        }
        //  Debug.Log("hello from " + this.name );
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(otherTransform.position, transform.position);
        var directionToTarget = otherTransform.position - transform.position;
        var degreesToTarget = Vector3.Angle(transform.forward, directionToTarget);
        var withinArc = degreesToTarget < (angle / 2);

            if (dist <= maxDistance)  //we are close enough 

            {
                if (!inRangeToPress && tooFarToPress)
                {

                    //  print("Distance to other just became Close : " + dist);
                    inRangeToPress = !inRangeToPress;
                    tooFarToPress = !tooFarToPress;
                    mat.color = Color.red;
                    return;  //Added 10/20/22 seems to be ok, just to save a cycle or 2 on the CPU...
                }
            }
            if (dist > maxDistance)  //we are too far   AND always true on start 
            {
                if (inRangeToPress && !tooFarToPress)
                {
                    //  print("Distance to other just became too Far: " + dist);
                    tooFarToPress = !tooFarToPress;
                    inRangeToPress = !inRangeToPress;
                    mat.color = Color.blue;
                }
            }
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
        var visibility = target as OpenDoor;
        Handles.color = new Color(1, 1, 1, 0.1f);
        var forwardPointMinusHalfAngle = Quaternion.Euler(0, -visibility.angle / 2, 0) * visibility.transform.forward;
        Vector3 arcStart = forwardPointMinusHalfAngle * visibility.maxDistance;
        Handles.DrawSolidArc(
            visibility.transform.position,
            Vector3.up,
            arcStart,
            visibility.angle,
            visibility.maxDistance);
        Handles.color = Color.white;
        Vector3 handlePosition = visibility.transform.position +
            visibility.transform.forward * visibility.maxDistance;

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