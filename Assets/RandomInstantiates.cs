using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomInstantiates : MonoBehaviour
{//Component of ProceduralGenerates
    // Instantiates prefabs in a random formation  //kinda lame but it works and will do for now
    public GameObject prefab;
    public Transform centerTransform;
    public int numberOfObjects = 3;
    public float radius = 50f;
    public float yPos = 4f;
    int randomRadius, randomRadiusX, randomRadiusZ;
    void Start()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfObjects;
            float randomLow = radius - 1;
            float randomHigh = radius * 2;
            randomRadiusX = (int)Random.Range(radius - 1, radius * 2);
            randomRadiusZ = (int)Random.Range(radius - 1, radius * 2);
            float x = Mathf.Cos(angle) * randomRadiusX;
            float z = Mathf.Sin(angle) * randomRadiusZ;
            //Debug.Log("random range = " + randomLow + "  " + randomHigh  +"   randomRadiusXandZ = " + randomRadiusX + "  angle = " + angle);
            //Debug.Log("x = Mathf.Cos(angle) * randomRadiusX; " + x + "     z = Mathf.Sin(angle) * randomRadiusZ;  " + z);
            Vector3 pos = centerTransform.position + new Vector3(x, yPos, z);
            //float angleDegrees = -angle * Mathf.Rad2Deg;
            //Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);
            Instantiate(prefab, pos, Quaternion.identity);
        }
    }
}
