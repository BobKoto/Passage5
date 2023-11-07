using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomInstantiates : MonoBehaviour
{
    // Instantiates prefabs in a random formation  //or it will soon
    public GameObject prefab;
    public Transform centerTransform;
    public int numberOfObjects = 3;
    public float radius = 15f;
    public float yPos = 4f;
    void Start()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfObjects;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            Vector3 pos = centerTransform.position + new Vector3(x, yPos, z);
            float angleDegrees = -angle * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);
            Instantiate(prefab, pos, rot);
        }
    }
}
