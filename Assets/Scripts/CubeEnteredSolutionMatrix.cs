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
    GameObject[] cubeGameCubes;
    Vector3[] cubeTransformStartPosition;  // so we can put cubes back to their original positions
    int cubeTransformStartPositionIndex;
    GameObject thisCube;
    Vector3 errantCube;
    Vector3 offsetErrantCube;
    Vector3 newCubePosition;
    bool placeOccupied;
    string placeOccupant;
    // Start is called before the first frame update
    void Start()
    {
        cubeGameCubes = GameObject.FindGameObjectsWithTag("CubeGameCube");
        cubeTransformStartPosition = new Vector3[cubeGameCubes.Length];
        for (int i = 0; i <= cubeGameCubes.Length - 1; i++)
        {
            cubeTransformStartPosition[i] = cubeGameCubes[i].transform.position;
        }
        //if (!audioManager) audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
    }
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
    public int CubeTransformStartPositionIndex(string thisCubeName)
    {
        return thisCubeName switch         //NEW FANGLED CODE courtesy of Visual Studio
        {
            "Cube10" => 0,
            "Cube20" => 1,
            "Cube30" => 2,
            "Cube40" => 3,
            _ => 0,
        };
    }
} // end class 
  //old Now unused Event declarations
  //[System.Serializable]
  //public class FingerPointerEvent : UnityEvent<string, string> { }
  //[System.Serializable]
  //public class Cube10FingerPointerEvent : UnityEvent<string, string, GameObject> { }
  //[System.Serializable]
  //public class Cube20FingerPointerEvent : UnityEvent<string, string, GameObject> { }
  //[System.Serializable]
  //public class Cube30FingerPointerEvent : UnityEvent<string, string, GameObject> { }
  //[System.Serializable]
  //public class Cube40FingerPointerEvent : UnityEvent<string, string, GameObject> { }
  // Old class declarations 
  //GameObject cube10, cube20, cube30, cube40; //don't need because we obtain name IDs from OnTriggers
  //GameObject topText;c
  //GameObject bottomText;
  //TMP_Text topRowText;
  //TMP_Text bottomRowText;
  // bool fingerPointerExitReceived;

// public CubeGameBoardEvent cubeGameBoardEvent;
// Old Start() stuff 
//cube10 = GameObject.Find("Cube10");//don't need because we obtain name IDs from OnTriggers
//cube20 = GameObject.Find("Cube20");
//cube30 = GameObject.Find("Cube30");
//cube30 = GameObject.Find("Cube40");

//topText = GameObject.Find("TopRow");don't need because
//bottomText = GameObject.Find("BottomRow");
//topRowText.text = "start top";
//bottomRowText.text = "start bottom";

//if (cubeGameBoardEvent == null) //syntaxes on other.name (duh) - triggers work so figure it out some other time :|
//   {   
//    cubeGameBoardEvent = new CubeGameBoardEvent().Invoke(other.name, " Entered   ", this.name, 20); 
//    cubeGameBoardEvent => CubeGameHandler.
//    }

//   Debug.Log("debug.log Invoke cubeGameBoardEvent here......." );
//  cubeGameBoardEvent.Invoke(this.name, "string2", 10, 20);
//IEnumerator WaitBeforeLockingCube(string othername, GameObject othergameObject, string placeName)
//{
//    Debug.Log("CESMatrix Coroutine WaitBeforeLockingCube is Waiting on finger up event" + " This is " + this.name);
//    yield return new WaitUntil(() => fingerPointerExitReceived == true);
//    Debug.Log("CESMatrix fingerPointerExitReceived == true");
//    LockTheCubeDown(othername, othergameObject, placeName);
//    fingerPointerExitReceived = false;
//    yield return null;
//}
//public void FingerPointerHappened(string cubeName, string fingerAction)
//{
//    fingerPointerExitReceived = false;  // 'cause we only want if we're ON a placement 
//    Debug.Log("CESMatrix Recvd from ActOnTouch cubeName = " + cubeName + "  Action = " + fingerAction + " This is " + this.name);
//    if (cubeName == placeOccupant)
//    {
//        fingerPointerExitReceived = true;
//    }
//    else
//    {
//        Debug.Log("CESMatrix MISMATCH on PlaceOccupant Occupant = " + placeOccupant + " cubeName = " + cubeName + " This is " + this.name);
//        if (placeOccupant == null) Debug.Log("CESMatrix place occupant is null!!");
//    }
//}
//void LockTheCubeDown(string objectToLock,GameObject _other, string placeName)  //Transform xForm, unneeded as we set the cubes to Kinematic  -- but keep for now 
//{
//    Debug.Log("LockTheCubeDown says Object to lock is " + objectToLock + " Game object = " + _other.name + " Place = " + placeName);
//    if (placeOccupied)
//    {
//        Vector3 targetPos;
//        targetPos = new Vector3(_other.transform.position.x, this.transform.position.y, this.transform.position.z);
//        _other.transform.position = targetPos;
//        Debug.Log("CESMatrix LockTheCubeDown says Object " + _other.name + " Lock attempted, MAYBE OK");
//    }
//    else Debug.Log("CESMatrix LockTheCubeDown says Lock failed because placeOccupied is FALSE");

//}
// Stripped out of OnTriggerExit -- redundant 
//switch (this.name)
//{
//    case "CubePlacement1":
//      //     Debug.Log(this.name + " ONTriggerExit exited by " + other);
//        cubeGameBoardEvent.Invoke(other.name, false, this.name, valueToSend);
//        break;
//    case "CubePlacement2":
//        //    Debug.Log(this.name + " ONTriggerExit exited by " + other);
//        cubeGameBoardEvent.Invoke(other.name, false, this.name, valueToSend);
//        break;
//    case "CubePlacement3":
//        //    Debug.Log(this.name + " ONTriggerExit exited by " + other);
//        cubeGameBoardEvent.Invoke(other.name, false, this.name, valueToSend);
//        break;
//    case "CubePlacement4":
//        //     Debug.Log(this.name + " ONTriggerExit exited by " + other);
//        cubeGameBoardEvent.Invoke(other.name, false, this.name, valueToSend);
//        break;
//    default: break;
//}
// Stripped out of OnTriggerEnter -- redundant 
//switch (this.name)
//{
//    case "CubePlacement1":
//        Debug.Log(this.name + " ONTriggerEnter Entered by " + other);
//        cubeGameBoardEvent.Invoke(other.name, true, this.name, valueToSend);

//        break;
//    case "CubePlacement2":
//        Debug.Log(this.name + " ONTriggerEnter Entered by " + other);
//        cubeGameBoardEvent.Invoke(other.name, true, this.name, valueToSend);
//        break;
//    case "CubePlacement3":
//        Debug.Log(this.name + " ONTriggerEnter Entered by " + other);
//        cubeGameBoardEvent.Invoke(other.name, true, this.name, valueToSend);
//        break;
//    case "CubePlacement4":
//        Debug.Log(this.name + " ONTriggerEnter Entered by " + other);
//        cubeGameBoardEvent.Invoke(other.name, true, this.name, valueToSend);
//        break;
//    default: break;
//}
//Unused stuff 
//These CubeXXFingerPointerHappened methods tells us to Lock a cube in place that is in a trigger //12/18/22 no longer used I hope 
//public void Cube10FingerPointerHappened(string cubeName, string fingerAction, GameObject _cube)   //CHECK PARAMETERS!
//{
//    Debug.Log("CESMatrix Cube10FingerPointerHappened Cube 10 finger up on " + _cube.name + " MyName =" + this.name);
//}
//public void Cube20FingerPointerHappened(string cubeName, string fingerAction, GameObject _cube)   //CHECK PARAMETERS!
//{
//    Debug.Log("CESMatrix Cube20FingerPointerHappened Cube 20 finger up on " + _cube.name + " MyName =" + this.name);
//}
//public void Cube30FingerPointerHappened(string cubeName, string fingerAction, GameObject _cube)   //CHECK PARAMETERS!
//{
//    Debug.Log("CESMatrix Cube30FingerPointerHappened Cube 30 finger up on " + _cube.name + " MyName =" + this.name);
//}
//public void Cube40FingerPointerHappened(string cubeName, string fingerAction, GameObject _cube)   //CHECK PARAMETERS!
//{
//    Debug.Log("CESMatrix Cube40FingerPointerHappened Cube 40 finger up on " + _cube.name + " MyName =" + this.name);
//}
//FingerPointerEvent fingerPointerEvent;  //empty class declared above - before this class -- we receive these 
//public Cube10FingerPointerEvent cube10FingerPointerEvent;  //empty class declared above - before this class -- we receive these 
//public Cube20FingerPointerEvent cube20FingerPointerEvent;  //empty class declared above - before this class -- we receive these 
//public Cube30FingerPointerEvent cube30FingerPointerEvent;  //empty class declared above - before this class -- we receive these 
//public Cube40FingerPointerEvent cube40FingerPointerEvent;  //empty class declared above - before this class -- we receive these 
//Set up received events BUT - as of 12/17/22 this script receives NO Events
//if (fingerPointerEvent == null) fingerPointerEvent = new FingerPointerEvent();  //not sure but it stopped the null reference 
//fingerPointerEvent.AddListener(FingerPointerHappened);   // in paren is the method in this script that gets invoked 
//ABOVE 2 Lines Commented so only CubePlacementHandler reacts to fingerUp from ActOnTouch 
//ABOVE comment-outs does NOT stop FingerPointerHappened from running! (learn) not until ActOnTouch event is deleted in Editor?

//if (cube10FingerPointerEvent == null) cube10FingerPointerEvent = new Cube10FingerPointerEvent();  //not sure but it stopped the null reference 
//if (cube20FingerPointerEvent == null) cube20FingerPointerEvent = new Cube20FingerPointerEvent();  //not sure but it stopped the null reference 
//if (cube30FingerPointerEvent == null) cube30FingerPointerEvent = new Cube30FingerPointerEvent();  //not sure but it stopped the null reference 
//if (cube40FingerPointerEvent == null) cube40FingerPointerEvent = new Cube40FingerPointerEvent();  //not sure but it stopped the null reference 

//cube10FingerPointerEvent.AddListener(Cube10FingerPointerHappened);   // in paren is the method in this script that gets invoked 
//cube20FingerPointerEvent.AddListener(Cube20FingerPointerHappened);   // in paren is the method in this script that gets invoked 
//cube30FingerPointerEvent.AddListener(Cube30FingerPointerHappened);   // in paren is the method in this script that gets invoked       
//cube40FingerPointerEvent.AddListener(Cube40FingerPointerHappened);   // in paren is the method in this script that gets invoked 
//End set up received events
//Not using Coroutines 
//void OnDisable()
//{
//    StopAllCoroutines(); // just to be safe/sure/clean whatever
//}