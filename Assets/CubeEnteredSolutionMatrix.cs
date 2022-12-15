using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
[System.Serializable]
public class FingerPointerEvent : UnityEvent<string, string> { }
[System.Serializable]
public class Cube10FingerPointerEvent : UnityEvent<string, string> { }
[System.Serializable]
public class Cube20FingerPointerEvent : UnityEvent<string, string> { }
[System.Serializable]
public class Cube30FingerPointerEvent : UnityEvent<string, string> { }
[System.Serializable]
public class Cube40FingerPointerEvent : UnityEvent<string, string> { }
public class CubeEnteredSolutionMatrix : MonoBehaviour   
// Component of CubePlacement objects -- sends Cube enter/exit events to CubeGameHandler.cs
{
    public AudioManager audioManager;
    FingerPointerEvent fingerPointerEvent;  //empty class declared above - before this class -- we receive these 
    Cube10FingerPointerEvent cube10FingerPointerEvent;  //empty class declared above - before this class -- we receive these 
    Cube20FingerPointerEvent cube20FingerPointerEvent;  //empty class declared above - before this class -- we receive these 
    Cube30FingerPointerEvent cube30FingerPointerEvent;  //empty class declared above - before this class -- we receive these 
    Cube40FingerPointerEvent cube40FingerPointerEvent;  //empty class declared above - before this class -- we receive these 

    public CubeTriggerEnterExitEvent cubeTriggerEnterExitEvent;

    public CubeGameBoardEvent cubeGameBoardEvent;  //event we invoke in here 
    bool placeOccupied;
    bool fingerPointerExitReceived;
    string placeOccupant;
    // Start is called before the first frame update
    void Start()
    {
        if (!audioManager) audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        if (fingerPointerEvent == null) fingerPointerEvent = new FingerPointerEvent();  //not sure but it stopped the null reference 
        fingerPointerEvent.AddListener(FingerPointerHappened);   // in paren is the method in this script that gets invoked 

        if (cube10FingerPointerEvent == null) cube10FingerPointerEvent = new Cube10FingerPointerEvent();  //not sure but it stopped the null reference 
        cube10FingerPointerEvent.AddListener(Cube10FingerPointerHappened);   // in paren is the method in this script that gets invoked 
        if (cube20FingerPointerEvent == null) cube20FingerPointerEvent = new Cube20FingerPointerEvent();  //not sure but it stopped the null reference 
        cube10FingerPointerEvent.AddListener(Cube20FingerPointerHappened);   // in paren is the method in this script that gets invoked 
        if (cube30FingerPointerEvent == null) cube30FingerPointerEvent = new Cube30FingerPointerEvent();  //not sure but it stopped the null reference 
        cube30FingerPointerEvent.AddListener(Cube30FingerPointerHappened);   // in paren is the method in this script that gets invoked 
        if (cube40FingerPointerEvent == null) cube40FingerPointerEvent = new Cube40FingerPointerEvent();  //not sure but it stopped the null reference 
        cube10FingerPointerEvent.AddListener(Cube10FingerPointerHappened);   // in paren is the method in this script that gets invoked 

    }
    public void Cube10FingerPointerHappened(string cubeName, string fingerAction)   //CHECK PARAMETERS!
    {
        Debug.Log("Cube 10 finger up");
    }
    public void Cube20FingerPointerHappened(string cubeName, string fingerAction)   //CHECK PARAMETERS!
    {
        Debug.Log("Cube 20 finger up");
    }
    public void Cube30FingerPointerHappened(string cubeName, string fingerAction)   //CHECK PARAMETERS!
    {
        Debug.Log("Cube 30 finger up");
    }
    public void Cube40FingerPointerHappened(string cubeName, string fingerAction)   //CHECK PARAMETERS!
    {
        Debug.Log("Cube 40 finger up");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!placeOccupied)  //then set the value moved into this - otherwise do nothing 
        {
            int valueToSend = CubeValue(other.name);
            
          // NOTE: Kinematic cubes mean our Robot avatar can no longer "push" them
            cubeTriggerEnterExitEvent.Invoke(other.name, this.name, 0, true);
            cubeGameBoardEvent.Invoke(other.name, true, this.name, valueToSend);
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
            audioManager.PlayAudio(audioManager.TYPE);
            placeOccupant = other.name;
            placeOccupied = true;
            Debug.Log(this.name + " entered by " + other + " placeOccupant = " + placeOccupant);
            StartCoroutine(WaitBeforeLockingCube(other.name, other.gameObject, this.name));

        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(this.name + " exited by " + other + " the occupant is " + placeOccupant);
        cubeTriggerEnterExitEvent.Invoke(other.name, this.name, 0, false);
        if (other.name == placeOccupant) //original cube intentionally dragged out or (physically pushed out by another?) 
        {
            placeOccupant = null;
            placeOccupied = false;
            int valueToSend = CubeValue(other.name);
            cubeGameBoardEvent.Invoke(other.name, false, this.name, valueToSend);
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
        }
    }
    IEnumerator WaitBeforeLockingCube(string othername, GameObject othergameObject, string placeName)
    {
        Debug.Log("Coroutine WaitBeforeLockingCube is Waiting on finger up event" + " This is " + this.name);
        yield return new WaitUntil(() => fingerPointerExitReceived == true);
        Debug.Log("fingerPointerExitReceived == true");
        LockTheCubeDown(othername, othergameObject, placeName);
        fingerPointerExitReceived = false;
        yield return null;
    }
    void LockTheCubeDown(string objectToLock,GameObject _other, string placeName)  //Transform xForm, unneeded as we set the cubes to Kinematic  -- but keep for now 
    {
        Debug.Log("LockTheCubeDown says Object to lock is " + objectToLock + " Game object = " + _other.name + " Place = " + placeName);
        if (placeOccupied)
        {
            Vector3 targetPos;
            targetPos = new Vector3(_other.transform.position.x, this.transform.position.y, this.transform.position.z);
            _other.transform.position = targetPos;
            Debug.Log("LockTheCubeDown says Object " + _other.name + " Lock attempted, MAYBE OK");
        }
        else Debug.Log("LockTheCubeDown says Lock failed because placeOccupied is FALSE");

    }
    public void FingerPointerHappened(string cubeName, string fingerAction)
    {
        fingerPointerExitReceived = false;  // 'cause we only want if we're ON a placement 
        Debug.Log("Recvd from ActOnTouch cubeName = " + cubeName + "  Action = " + fingerAction + " This is " + this.name);
        if (cubeName == placeOccupant)
        {
            fingerPointerExitReceived = true;
        }
        else 
            Debug.Log("MISMATCH on PlaceOccupant Occupant = " + placeOccupant + " cubeName = " + cubeName + " This is " + this.name);
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
}
// Old class declarations 
//GameObject cube10, cube20, cube30, cube40; //don't need because we obtain name IDs from OnTriggers
//GameObject topText;c
//GameObject bottomText;
//TMP_Text topRowText;
//TMP_Text bottomRowText;

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
