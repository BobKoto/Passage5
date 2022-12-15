using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
[System.Serializable]
public class HandlerFingerPointerUpEvent : UnityEvent<string, string, int> { }
[System.Serializable]
public class CubeTriggerEnterExitEvent : UnityEvent<string, string, int, bool> { }
//[System.Serializable]
//public class FingerPointerEvent : UnityEvent<string, string> { }
public class CubePlacementHandler : MonoBehaviour
// Component of CubePlacements  // objects -- receives Cube enter Trigger events   /exit events to CubeGameHandler.cs
{
    public AudioManager audioManager;
    public FingerPointerEvent fingerPointerEvent; //CubeEnteredSolutionMatrix waits for us to send these 
    HandlerFingerPointerUpEvent handlerFingerPointerEvent;  //empty class declared above - before this class -- we receive these 

    bool cube10WaitingForFingerUp, cube20WaitingForFingerUp, cube30WaitingForFingerUp, cube40WaitingForFingerUp;
    CubeTriggerEnterExitEvent cubeTriggerEnterExitEvent;
    Cube10FingerPointerEvent cube10FingerPointerEvent;  // we send these 
    Cube20FingerPointerEvent cube20FingerPointerEvent;  //
    Cube30FingerPointerEvent cube30FingerPointerEvent;  // 
    Cube40FingerPointerEvent cube40FingerPointerEvent;  // 
    // Start is called before the first frame update
    void Start()
    {
       // Debug.Log(" Hello from CubePlacementHandler");
        if (!audioManager) audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        //if (handlerFingerPointerEvent == null) handlerFingerPointerEvent = new HandlerFingerPointerUpEvent();  //not sure but it stopped the null reference 
        //handlerFingerPointerEvent.AddListener(ReceivedFingerUpEvent);   // in paren is the method in this script that gets invoked 

        if (cubeTriggerEnterExitEvent == null) cubeTriggerEnterExitEvent = new CubeTriggerEnterExitEvent();
        cubeTriggerEnterExitEvent.AddListener(CubeEneteredOrExitedPlacement);

        fingerPointerEvent.AddListener(ReceivedFingerUpEvent);
    }

    public void CubeEneteredOrExitedPlacement(string cubeName, string placementName, int extraInt, bool cubeEntered)
    {
        Debug.LogFormat("CUBEENETEREDOREXITEDPLACEMENT cubeName= " + cubeName + " cubeEntered = " + cubeEntered +
             " placement = " + placementName );

        switch (cubeName)
        {
            case "Cube10":
                cube10WaitingForFingerUp = cubeEntered;
                break;
            case "Cube20":
                cube20WaitingForFingerUp = cubeEntered; 
                break;
            case "Cube30":
                cube30WaitingForFingerUp = cubeEntered; 
                break;
            case "Cube40":
                cube40WaitingForFingerUp = cubeEntered;
                break;
            default:
                break;
        }
    }

    //  public void ReceivedFingerUpEvent(string cubeName, string extraOne, int cubeValue)
    public void ReceivedFingerUpEvent(string cubeName, string action)
    {
        // Here is where we get a fingerUp event from ActOnTouch
        //and if it is flagged as being awaited we send an Up event to CubeEnteredSolutionMatrix
        Debug.Log("ReceivedFingerUpEvent.................cubeNmae = " + cubeName + " action " + action);
        switch (cubeName)
        {
            case "Cube10":
                if (cube10WaitingForFingerUp)
                {
                    // fingerPointerEvent.Invoke(this.name, "finger UP");  //here we need to send a unique event????
                     cube10FingerPointerEvent.Invoke(cubeName, "fingerUp");
                     cube10WaitingForFingerUp = false;
                }

                break;
            case "Cube20":
                if (cube20WaitingForFingerUp)
                {
                    // fingerPointerEvent.Invoke(this.name, "finger UP");  //here we need to send a unique event????
                    cube20FingerPointerEvent.Invoke(cubeName, "fingerUp");
                    cube20WaitingForFingerUp = false;
                }

                break;
            case "Cube30":
                if (cube30WaitingForFingerUp)
                {
                    //fingerPointerEvent.Invoke(this.name, "finger UP");  //here we need to send a unique event????
                    cube30FingerPointerEvent.Invoke(cubeName, "fingerUp");
                    cube30WaitingForFingerUp = false;
                }

                break;
            case "Cube40":
                if (cube40WaitingForFingerUp)
                {
                    //fingerPointerEvent.Invoke(this.name, "finger UP");  //here we need to send a unique event????
                    cube40FingerPointerEvent.Invoke(cubeName, "fingerUp");
                    cube40WaitingForFingerUp = false;
                }
                break;
            default: Debug.Log("ReceivedFingerUpEvent sent UFO");
                break;
        }

    }


} //end class
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!placeOccupied)  //then set the value moved into this - otherwise do nothing 
    //    {
    //        int valueToSend = CubeValue(other.name);

    //        // NOTE: Kinematic cubes mean our Robot avatar can no longer "push" them
    //        switch (this.name)
    //        {
    //            case "CubePlacement1":
    //                Debug.Log(this.name + " ONTriggerEnter Entered by " + other);
    //                cubeGameBoardEvent.Invoke(other.name, true, this.name, valueToSend);
    //                break;
    //            case "CubePlacement2":
    //                Debug.Log(this.name + " ONTriggerEnter Entered by " + other);
    //                cubeGameBoardEvent.Invoke(other.name, true, this.name, valueToSend);
    //                break;
    //            case "CubePlacement3":
    //                Debug.Log(this.name + " ONTriggerEnter Entered by " + other);
    //                cubeGameBoardEvent.Invoke(other.name, true, this.name, valueToSend);
    //                break;
    //            case "CubePlacement4":
    //                Debug.Log(this.name + " ONTriggerEnter Entered by " + other);
    //                cubeGameBoardEvent.Invoke(other.name, true, this.name, valueToSend);
    //                break;
    //            default: break;
    //        }
    //        audioManager.PlayAudio(audioManager.TYPE);
    //        placeOccupant = other.name;
    //        placeOccupied = true;
    //        //  LockTheCubeDown(other.name,  other.gameObject, this.name);  //other.GetComponent<Transform>(),not needed cause we set the cubes to Kinematic  -- but keep for now 
    //        Debug.Log(this.name + " entered by " + other + " placeOccupant = " + placeOccupant);
    //        StartCoroutine(WaitBeforeLockingCube(other.name, other.gameObject, this.name));
    //        // placeOccupied = true;
    //        // placeOccupant = other.name;
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    Debug.Log(this.name + " exited by " + other + " the occupant is " + placeOccupant);
    //    if (other.name == placeOccupant) //original cube intentionally dragged out or (physically pushed out by another?) 
    //    {
    //        placeOccupant = null;
    //        placeOccupied = false;
    //        int valueToSend = CubeValue(other.name);
    //        switch (this.name)
    //        {
    //            case "CubePlacement1":
    //                //     Debug.Log(this.name + " ONTriggerExit exited by " + other);
    //                cubeGameBoardEvent.Invoke(other.name, false, this.name, valueToSend);
    //                break;
    //            case "CubePlacement2":
    //                //    Debug.Log(this.name + " ONTriggerExit exited by " + other);
    //                cubeGameBoardEvent.Invoke(other.name, false, this.name, valueToSend);
    //                break;
    //            case "CubePlacement3":
    //                //    Debug.Log(this.name + " ONTriggerExit exited by " + other);
    //                cubeGameBoardEvent.Invoke(other.name, false, this.name, valueToSend);
    //                break;
    //            case "CubePlacement4":
    //                //     Debug.Log(this.name + " ONTriggerExit exited by " + other);
    //                cubeGameBoardEvent.Invoke(other.name, false, this.name, valueToSend);
    //                break;
    //            default: break;
    //        }
    //    }
    //}
    //IEnumerator WaitBeforeLockingCube(string othername, GameObject othergameObject, string placeName)
    //{
    //    Debug.Log("Coroutine WaitBeforeLockingCube is Waiting on finger up event" + " This is " + this.name);
    //    yield return new WaitUntil(() => fingerPointerExitReceived == true);
    //    Debug.Log("fingerPointerExitReceived == true");
    //    LockTheCubeDown(othername, othergameObject, placeName);
    //    fingerPointerExitReceived = false;
    //    yield return null;
    //}
    //void LockTheCubeDown(string objectToLock, GameObject _other, string placeName)  //Transform xForm, unneeded as we set the cubes to Kinematic  -- but keep for now 
    //{
    //    Debug.Log("LockTheCubeDown says Object to lock is " + objectToLock + " Game object = " + _other.name + " Place = " + placeName);
    //    if (placeOccupied)
    //    {
    //        Vector3 targetPos;
    //        targetPos = new Vector3(_other.transform.position.x, this.transform.position.y, this.transform.position.z);
    //        _other.transform.position = targetPos;
    //        Debug.Log("LockTheCubeDown says Object " + _other.name + " Lock attempted, MAYBE OK");
    //    }
    //    else Debug.Log("LockTheCubeDown says Lock failed because placeOccupied is FALSE");

    //}
    //public void FingerPointerHappened(string cubeName, string fingerAction)
    //{
    //    fingerPointerExitReceived = false;  // 'cause we only want if we're ON a placement 
    //    Debug.Log("Recvd from ActOnTouch cubeName = " + cubeName + "  Action = " + fingerAction + " This is " + this.name);
    //    if (cubeName == placeOccupant)
    //    {
    //        fingerPointerExitReceived = true;
    //    }
    //    else
    //        Debug.Log("MISMATCH on PlaceOccupant Occupant = " + placeOccupant + " cubeName = " + cubeName + " This is " + this.name);
    //}
    //public int CubeValue(string cubeMovedInOrOut)
    //{
    //    //int valueToSend = CubeValue(cubeMovedInOrOut); caused a stackOverFlow? btw it's wrong anyway
    //    return cubeMovedInOrOut switch         //NEW FANGLED CODE courtesy of Visual Studio
    //    {
    //        "Cube10" => 10,
    //        "Cube20" => 20,
    //        "Cube30" => 30,
    //        "Cube40" => 40,
    //        _ => 0,
    //    };
    //}
//}

