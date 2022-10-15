using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInRangeToPressMe : MonoBehaviour
{
    public Transform other  ;
    public float pressableDistance = 3f;
    bool tooFarToPress, closeEnoughToPress;
    Material mat;
    MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
       mat = GetComponent<MeshRenderer>().material;
        if (!other)
        {
            other = GameObject.Find("PlayerArmature").GetComponent<Transform>();
            tooFarToPress = true; // IF and ONLY IF we start too far from a pressable object
        }
        Debug.Log("hello from " + this.name );
    }

    // Update is called once per frame
    void Update()
    {
        if (other)
        {
            float dist = Vector3.Distance(other.position, transform.position);
            if (dist <= pressableDistance)  //we are close enough 
            {
                if (!closeEnoughToPress && tooFarToPress)
                {
                    print("Distance to other is Close : " + dist);
                    closeEnoughToPress = !closeEnoughToPress;
                    tooFarToPress = !tooFarToPress;
                    mat.color = Color.red;
                }

            }
            if (dist > pressableDistance)  //we are too far   AND always true on start 
            //else    //we are too far away    and we suck at boolean logic so far 
            {
                if (closeEnoughToPress && !tooFarToPress)
                {
                    print("Distance to other on ELSE (not <=) is too Far: " + dist);
                    tooFarToPress = !tooFarToPress;
                    closeEnoughToPress = !closeEnoughToPress;
                    mat.color = Color.blue;
                }

            }


        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        mat.color = Color.green;
    }
}
