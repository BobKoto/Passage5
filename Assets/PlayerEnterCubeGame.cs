using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerEnterCubeGame : MonoBehaviour
{
    public CinemachineVirtualCamera cubeGameCam;
    int originalCamPriority;

    GameObject[] cubeGameCubes;
    GameObject[] cubeGamePlacement;
    Vector3[] cubeTransformStartPosition;  // so we can put cubes back to their original positions
    Vector3[] cubePlacementPosition; // where to automatically place/start a Cube 
    // Start is called before the first frame update
    void Start()
    {
        cubeGameCubes = GameObject.FindGameObjectsWithTag("CubeGameCube");
        cubeGamePlacement = GameObject.FindGameObjectsWithTag("CubeGamePlacement");
        cubePlacementPosition = new Vector3[cubeGamePlacement.Length];
        cubeTransformStartPosition = new Vector3[cubeGameCubes.Length];
        for (int i =0; i <= cubeGameCubes.Length-1; i++ )
        {
            cubeTransformStartPosition[i] = cubeGameCubes[i].transform.position;
        }
        for (int x = 0; x <= cubeGamePlacement.Length-1; x++)
        {
            cubePlacementPosition[x] = cubeGamePlacement[x].transform.position;
        }
        originalCamPriority = cubeGameCam.Priority;

    }
    void SeedCubePuzzle()
    {
        //int Random.Range (0,10) will return a random value 0 thru "9" - beware
        int randomCubePosition = Random.Range(0, cubeGamePlacement.Length);  //higher val was  hardcoded to 3 but u never know...
        int randomCubePlacement = Random.Range(0, cubeGamePlacement.Length);  //higher val was  hardcoded to 3 but u never know...
        //Debug.Log("randCubeRAW = " + randomCubePosition +        "  randCubePlacRAW = "  + randomCubePlacement );
        //Debug.Log("randCube =      " + (randomCubePosition+1)*10 + "  randCubePlac =        " + (randomCubePlacement+1));

        Vector3 xForward = new Vector3(1.5f, 0f, 0f);//pull cube out toward cam
        cubeGameCubes[randomCubePosition].transform.position = cubePlacementPosition[randomCubePlacement] + xForward; 
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            cubeGameCam.Priority = 12;
            SeedCubePuzzle();
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
