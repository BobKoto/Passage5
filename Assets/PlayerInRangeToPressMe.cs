using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInRangeToPressMe : MonoBehaviour
{
    public Transform otherTransform  ;
    public float pressableDistance = 3f;
    bool tooFarToPress, closeEnoughToPress;
    Material mat;
    MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
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
        if (otherTransform)
        {
            float dist = Vector3.Distance(otherTransform.position, transform.position);
            if (dist <= pressableDistance)  //we are close enough 
            {
                if (!closeEnoughToPress && tooFarToPress)
                {
                  //  print("Distance to other just became Close : " + dist);
                    closeEnoughToPress = !closeEnoughToPress;
                    tooFarToPress = !tooFarToPress;
                    mat.color = Color.red;
                }
            }
            if (dist > pressableDistance)  //we are too far   AND always true on start 
            {
                if (closeEnoughToPress && !tooFarToPress)
                {
                  //  print("Distance to other just became too Far: " + dist);
                    tooFarToPress = !tooFarToPress;
                    closeEnoughToPress = !closeEnoughToPress;
                    mat.color = Color.blue;
                }
            }
        } // if other
    }
    private void OnCollisionEnter(Collision collision)
    {
        print("Collided with " + collision.collider);
        mat.color = Color.green;
    }
}
