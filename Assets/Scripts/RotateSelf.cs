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

    }

    // Update is called once per frame
    void Update()
    {
        float ro = Time.deltaTime * speed;
        if (clockWise)
        {
         transform.Rotate(0f, 0f, -ro);
        }
        else
        {
            transform.Rotate(0f, 0f, ro);  //counter clockwise 
        }

        //transform.RotateAround(target.transform.position, Vector3.right, ro );


        //            uniSphere.transform.Rotate(00.0f, 0.0f, 0.2f, Space.Self);
    }
}
