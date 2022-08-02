using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSelf : MonoBehaviour
{
    public int speed = 100;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate( 0f,     0f, Time.deltaTime * speed,   Space.World);


        //            uniSphere.transform.Rotate(00.0f, 0.0f, 0.2f, Space.Self);
    }
}
