using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
[System.Serializable]
public class FingerPointerEvent : UnityEvent<string, string>
{
}


public class CubeEnteredSolutionMatrix : MonoBehaviour   
// Component of CubePlacement objects -- sends Cube enter/exit events to CubeGameHandler.cs
{
    public AudioManager audioManager;
    FingerPointerEvent fingerPointerEvent;  //empty class declared above - before this class -- we receive these 
    public CubeGameBoardEvent cubeGameBoardEvent;  //event we invoke in here 
    bool placeOccupied;
    bool fingerPointerExitReceived;
    string placeOccupant;
    // Start is called before the first frame update
    void Start()
    {
        if (!audioManager) audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        if (fingerPointerEvent == null) fingerPointerEvent = new FingerPointerEvent();  //not sure but it stopped the null reference 
        fingerPointerEvent.AddListener(FingerPointerHappened);   // in paren is the method in here that gets invoked 
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!placeOccupied)  //then set the value moved into this - otherwise do nothing 
        {
            int valueToSend = CubeValue(other.name);
            audioManager.PlayAudio(audioManager.TYPE);
            placeOccupant = other.name;
            placeOccupied = true;
            //  LockTheCubeDown(other.name,  other.gameObject, this.name);  //other.GetComponent<Transform>(),not needed cause we set the cubes to Kinematic  -- but keep for now 
            StartCoroutine(WaitBeforeLockingCube(other.name, other.gameObject, this.name));
          // NOTE: Kinematic cubes mean our Robot avatar can no longer "push" them
            switch (this.name)
            {
                case "CubePlacement1":

                    cubeGameBoardEvent.Invoke(other.name, true, this.name, valueToSend);
                    break;
                case "CubePlacement2":
                    cubeGameBoardEvent.Invoke(other.name, true, this.name, valueToSend);
                    break;
                case "CubePlacement3":
                    cubeGameBoardEvent.Invoke(other.name, true, this.name, valueToSend);
                    break;
                case "CubePlacement4":
                    cubeGameBoardEvent.Invoke(other.name, true, this.name, valueToSend);
                    break;
                default: break;
            }
            // Debug.Log(this.name + " entered by " + other);
           // placeOccupied = true;
           // placeOccupant = other.name;
        }
    }
    private void OnTriggerExit(Collider other)
    {
       // Debug.Log(this.name + " exited by " + other + " the occupant is " + placeOccupant);
        if (other.name == placeOccupant) //original cube intentionally dragged out or (physically pushed out by another?) 
        {
            placeOccupant = null;
            placeOccupied = false;
            int valueToSend = CubeValue(other.name);
            switch (this.name)
            {
                case "CubePlacement1":
                    //   Debug.Log(this.name + " exited by " + other);
                    cubeGameBoardEvent.Invoke(other.name, false, this.name, valueToSend);
                    break;
                case "CubePlacement2":
                    //   Debug.Log(this.name + " exited by " + other);
                    cubeGameBoardEvent.Invoke(other.name, false, this.name, valueToSend);
                    break;
                case "CubePlacement3":
                    //   Debug.Log(this.name + " exited by " + other);
                    cubeGameBoardEvent.Invoke(other.name, false, this.name, valueToSend);
                    break;
                case "CubePlacement4":
                    //    Debug.Log(this.name + " exited by " + other);
                    cubeGameBoardEvent.Invoke(other.name, false, this.name, valueToSend);
                    break;
                default: break;
            }
        }
    }
    IEnumerator WaitBeforeLockingCube(string othername, GameObject othergameObject, string placeName)
    {
        Debug.Log("Coroutine WaitBeforeLockingCube is Waiting on finger up event");
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
        Debug.Log("Received event from ActOnTouch!!!  cubeName = " + cubeName + "  Action = " + fingerAction);
        if (cubeName == placeOccupant)
        {
            fingerPointerExitReceived = true;
        }
        else Debug.Log("MISMATCH on PlaceOccupant  placeOccupant = " + placeOccupant);
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
