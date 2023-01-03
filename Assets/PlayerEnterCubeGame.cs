using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerEnterCubeGame : MonoBehaviour
{
    public CinemachineVirtualCamera cubeGameCam;
    int originalCamPriority;

    GameObject[] cubeGameCubes;
    Vector3[] cubeTransformStartPosition;
    // Start is called before the first frame update
    void Start()
    {
        cubeGameCubes = GameObject.FindGameObjectsWithTag("CubeGameCube");
        cubeTransformStartPosition = new Vector3[cubeGameCubes.Length];
        for (int i =0; i <= cubeGameCubes.Length-1; i++ )
        {
            cubeTransformStartPosition[i] = cubeGameCubes[i].transform.position;
        }
        originalCamPriority = cubeGameCam.Priority;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            cubeGameCam.Priority = 12;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            cubeGameCam.Priority = originalCamPriority;
            for (int i = 0; i <= cubeGameCubes.Length-1; i++)  //Restore the cubes to home/original positions 
            {
                cubeGameCubes[i].transform.position = cubeTransformStartPosition[i];
            }
        }
    }
}
