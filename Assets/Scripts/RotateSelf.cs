using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSelf : MonoBehaviour
{
    public int speed = 100;
    public bool clockWise = false;
    //public GameObject target;
    // Start is called before the first frame update
    void Start()
    {

        if (clockWise) Debug.Log("With clockwise set we rotate Self...");
        else
            Debug.Log("With clockwise NOT set we rotate World...");
    }

    // Update is called once per frame
    void Update()
    {
        float ro = Time.deltaTime * speed;
        if (clockWise)
        {
        //  transform.Rotate(-ro,   0f, 0f,  Space.Self);
            transform.Rotate(0f, 0f, -ro, Space.Self);
        }
        else
        {
         //   transform.Rotate(ro,   0f, 0f,  Space.World);  //counter clockwise 
            transform.Rotate(0f, 0f, ro, Space.World);  //counter clockwise 
        }

        //transform.RotateAround(target.transform.position, Vector3.right, ro );


        //            uniSphere.transform.Rotate(00.0f, 0.0f, 0.2f, Space.Self);
    }
}
