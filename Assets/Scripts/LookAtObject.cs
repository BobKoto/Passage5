using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObject : MonoBehaviour
{//Component of FloatingEye-color- Prefab(s)
    public Transform objectToLookAt;
    // Start is called before the first frame update
    void Start()
    {
        if (!objectToLookAt)
        {
            objectToLookAt = GameObject.Find("PlayerCameraRoot").GetComponent<Transform>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(objectToLookAt); //, Vector3.forward);
    }
}
