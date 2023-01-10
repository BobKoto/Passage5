using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
public class CubeEnteredSolutionMatrix : MonoBehaviour   
// Component of each CubePlacement object -NOT the Cubes- -- sends Cube enter/exit events to CubeGameHandler.cs
{
   // public AudioManager audioManager;
    public CubeTriggerEnterExitEvent cubeTriggerEnterExitEvent;
    public CubeGameBoardEvent cubeGameBoardEvent;  //event we invoke in here 
    //GameObject[] cubeGameCubes;
    //Vector3[] cubeTransformStartPosition;  // so we can put cubes back to their original positions
    //int cubeTransformStartPositionIndex;
    //GameObject thisCube;
    //Vector3 errantCube;
    //Vector3 offsetErrantCube;
    //Vector3 newCubePosition;
    //bool placeOccupied;
    string placeOccupant;
    // Start is called before the first frame update
    void Start()
    {
        //cubeGameCubes = GameObject.FindGameObjectsWithTag("CubeGameCube");
        //cubeTransformStartPosition = new Vector3[cubeGameCubes.Length];
        //for (int i = 0; i <= cubeGameCubes.Length - 1; i++)
        //{
        //    cubeTransformStartPosition[i] = cubeGameCubes[i].transform.position;
        //}
        //if (!audioManager) audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
    }
    // /////// On 1/9/23 redesign triggers to ONLY send events to CubePlacementHandler & CubeGameHandler 
    private void OnTriggerEnter(Collider cube)
    {
        // NOTE: Kinematic cubes means our Robot avatar can no longer "push" them - nor can other Cubes - so overlap can exist
        Debug.Log("CESMatrix " + this.name + " ENTERED by " + cube + " the occupant is " + placeOccupant);
        int valueToSend = CubeValue(cube.name);
        cubeTriggerEnterExitEvent.Invoke(this.gameObject, this.name, cube.gameObject, true);  //Send event to CubePlacementHandler
        cubeGameBoardEvent.Invoke(cube.name, true, this.name, valueToSend); //Send event to CubeGameHandler
    }
    private void OnTriggerExit(Collider cube)
    {
        Debug.Log("CESMatrix " + this.name + " EXITED by " + cube + " the occupant is " + placeOccupant);
        int valueToSend = CubeValue(cube.name);
        cubeTriggerEnterExitEvent.Invoke(this.gameObject, this.name, cube.gameObject, false);  //Send event to CubePlacementHandler
        cubeGameBoardEvent.Invoke(cube.name, false, this.name, valueToSend);  //Send event to CubeGameHandler
    }
    public int CubeValue(string cubeMovedInOrOut)
    {
        //int valueToSend = CubeValue(cubeMovedInOrOut); caused a stackOverFlow? btw it's wrong anyway
        return cubeMovedInOrOut switch         //NEW FANGLED CODE courtesy of Visual Studio
        {
            "Cube10" => 10,
            "Cube20" => 20,
            "Cube30" => 30,
            "Cube40" => 40,
            _ => 0,
        };
    }
    //public int CubeTransformStartPositionIndex(string thisCubeName)
    //{
    //    return thisCubeName switch         //NEW FANGLED CODE courtesy of Visual Studio
    //    {
    //        "Cube10" => 0,
    //        "Cube20" => 1,
    //        "Cube30" => 2,
    //        "Cube40" => 3,
    //        _ => 0,
    //    };
    //}
} // end class 

/*    //////// original triggers and coroutine before redesign on 1/9/23 
    private void OnTriggerEnter(Collider cube)
    {
        if (!placeOccupied)  //then set the value moved into this - otherwise do nothing? maybe we should send the Cube back home?
        {
            // NOTE: Kinematic cubes means our Robot avatar can no longer "push" them - nor can other Cubes - so overlap can exist
            int valueToSend = CubeValue(cube.name);
            cubeTriggerEnterExitEvent.Invoke(this.gameObject, this.name, cube.gameObject, true);  //Send event to CubePlacementHandler
            cubeGameBoardEvent.Invoke(cube.name, true, this.name, valueToSend); //Send event to CubeGameHandler
            placeOccupied = true;
            placeOccupant = cube.name;
        }
        else  //it IS occupied
        {
            // so here we need to handle an overlapping cube - send it home
            thisCube = GameObject.Find(cube.name);
            Debug.Log("CESM " + this.name + " TriggerEnter " + thisCube.name + " is overlapping ");
          //  StartCoroutine(SetThisCubePositionBackToHome(.5f)); //i think we need a coroutine here - after hours of trying 
        }
    }
    IEnumerator SetThisCubePositionBackToHome(float timeInSeconds)
    {
        yield return new WaitForSeconds(timeInSeconds);
        thisCube.transform.position = cubeTransformStartPosition[CubeTransformStartPositionIndex(thisCube.name)];
    }
    private void OnTriggerExit(Collider cube)
    {
        Debug.Log("CESMatrix " + this.name + " EXITED by " + cube + " the occupant is " + placeOccupant);
        if (cube.name == placeOccupant) //original cube intentionally dragged out - an overlapping cube is ignored for now
        {
            placeOccupant = null;
            placeOccupied = false;
            int valueToSend = CubeValue(cube.name);
            cubeTriggerEnterExitEvent.Invoke(this.gameObject, this.name, cube.gameObject, false);  //Send event to CubePlacementHandler
            cubeGameBoardEvent.Invoke(cube.name, false, this.name, valueToSend);  //Send event to CubeGameHandler
        }
    }
 * */
