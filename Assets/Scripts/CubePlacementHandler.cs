using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

[System.Serializable]
public class FingerPointerEvent : UnityEvent<GameObject, string> { }  // we receive these from ActOnTouch 
[System.Serializable]
public class CubeTriggerEnterExitEvent : UnityEvent<GameObject, string, GameObject, bool> { }  // we receive these from CESMatrix

public class CubePlacementHandler : MonoBehaviour
// Component of CubePlacements  // objects -- receives Cube enter Trigger events   /exit events to CubeGameHandler.cs
{
    public AudioManager audioManager;
    public FingerPointerEvent fingerPointerEvent; //12/18/22 we receive these from ActOnTouch 
    public CubeTriggerEnterExitEvent cubeTriggerEnterExitEvent;  // we receive these from CESMatrix

    GameObject currentCube, currentPlace;
    bool[] cubeLockStatus = new bool[4];
    bool[] placementLockCube = new bool[4];//set by SetPlacementLockStatus which may be removed 
    GameObject[] cubeLockPlacementObject = new GameObject[4];
    bool  waitingFingerPointerExit;   //fingerPointerExitReceived,

    // Start is called before the first frame update
    void Start()
    {
       // Debug.Log(" Hello from CubePlacementHandler");
        if (!audioManager) audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();

        if (cubeTriggerEnterExitEvent == null) cubeTriggerEnterExitEvent = new CubeTriggerEnterExitEvent();
        cubeTriggerEnterExitEvent.AddListener(CubeEnterExitPlacement); //u change the listener (and method) name u must change Editor too!  

        if (fingerPointerEvent == null) fingerPointerEvent = new FingerPointerEvent();  //not sure but it stopped the null reference 
        fingerPointerEvent.AddListener(ReceivedFingerUpEvent);  //this line and one above needed to receive events
    }
    public void CubeEnterExitPlacement(GameObject _place, string placementName, GameObject _cube, bool cubeEntered)  //Method name in Editor!
    {//OnTrigger enters and exits in CESMatrix sent here via this event 
     //string _exitedORentered;
     //_exitedORentered = cubeEntered ? "  ENTERED" : "  EXITED"; //only for the Debug.Log below
     //Debug.Log("CPHandler CubeEnterExitPlacement,  placeGOName= " + _place.name + ",  CubeGOName = " + _cube.name + _exitedORentered);
     //                                 //  currentCube = null;   //  currentPlace = null;
        currentCube = _cube;
        currentPlace = _place;
        waitingFingerPointerExit = false;
        if (cubeEntered)
        {
            //currentCube = _cube;
            //currentPlace = _place;
            waitingFingerPointerExit = true;
        }
        else  //the cube exited so we want to undo things 
        {
           // if (cubeLockStatus[GetCubeLockIndex()]) SetCubeLockStatus(false);
           SetCubeLockStatus(false);
           //SetPlacementLockStatus(null);  //not sure if this a good idea yet
        }
    }
    public void ReceivedFingerUpEvent(GameObject _cube, string action)
    {
        // Here we get fingerUp event from ActOnTouch -- either fingerUp or object dropped or both -- or worse, nothing
        //Debug.Log("CPHandler/ReceivedFingerUpEvent from AOTouch  ...cubeNmae = " + cubeName +
        //    ", action " + action + ", waitingFinger = " + waitingFingerPointerExit);
        currentCube = _cube; //this is a KEY
        if (waitingFingerPointerExit || cubeLockStatus[CubeLockIndex()]) //cube enetered & stayed -- or moved within & needs relocking
        {
            if (cubeLockStatus[CubeLockIndex()])  //is FingerUp on a locked cube?
            {
                currentPlace = cubeLockPlacementObject[CubeLockIndex()];  // just relock it to original Placement
            }
            LockTheCubeDown();
            waitingFingerPointerExit = false;
        }
    }
    // Move WaitBeforeLockingCube AND LockTheCubeDown methods out of CESMatrix and into here 
    //IEnumerator WaitBeforeLockingCube()
    //{
    //    yield return new WaitUntil(() => fingerPointerExitReceived);
    //    LockTheCubeDown();
    //    fingerPointerExitReceived = false;
    //    yield return null;
    //}
    void LockTheCubeDown()  // Aligns cube to placement 
    {
        Vector3 targetPos;
        targetPos = new Vector3(currentCube.transform.position.x, currentPlace.transform.position.y, currentPlace.transform.position.z);
        currentCube.transform.position = targetPos;
        SetCubeLockStatus(true);
     //   SetPlacementLockStatus(currentCube);
        audioManager.PlayAudio(audioManager.TYPE);
    }
    void SetCubeLockStatus(bool lockStatus)
    {// here currentPlace and currentCube come from global context
        string s1;
        switch (currentCube.name)
        {
            case "Cube10":
                cubeLockStatus[0] = lockStatus;
                cubeLockPlacementObject[0] = lockStatus? currentPlace : null;  //associate/disassociate placement with cube 
                break;
            case "Cube20":
                cubeLockStatus[1] = lockStatus;
                cubeLockPlacementObject[1] = lockStatus ? currentPlace : null;
                break;
            case "Cube30":
                cubeLockStatus[2] = lockStatus;
                cubeLockPlacementObject[2] = lockStatus ? currentPlace : null;
                break;
            case "Cube40":
                cubeLockStatus[3] = lockStatus;
                cubeLockPlacementObject[3] = lockStatus ? currentPlace : null;
                break;
            default:
                Debug.Log("SetCubeLockStatus(bool lockStatus) got ??? ");
                break;
        }
        s1 = lockStatus ? "locked" : "UNLOCKED";
        //Debug.Log("SCLS we just " + s1 + " " + currentCube.name + "  into/from " + currentPlace.name);
        //Debug.Log("lock array " + cubeLockStatus[0] + " " + cubeLockStatus[1] + " " + cubeLockStatus[2] + " " + cubeLockStatus[3]);
    }
    void SetPlacementLockStatus(GameObject cubeToLock)
    {
      //  string s1;
        switch (currentPlace.name)
        {
            case "CubePlacement1":
                placementLockCube[0] = cubeToLock;
                break;
            case "CubePlacement2":
                placementLockCube[1] = cubeToLock;
                break;
            case "CubePlacement3":
                placementLockCube[2] = cubeToLock;
                break;
            case "CubePlacement4":
                placementLockCube[3] = cubeToLock;
                break;
            default:
                Debug.Log("SetPlacementLockStatus(GameObject cubeToLock) got " + cubeToLock.name + " into  " + currentPlace.name);
                break;
        }
      // s1 = lockStatus ? "locked" : "UNLOCKED";
        Debug.Log("SPLS we just "  + "locked/unlocked " + currentCube.name + "  into " + currentPlace.name);
       // Debug.Log("lock array " + cubeLockStatus[0] + " " + cubeLockStatus[1] + " " + cubeLockStatus[2] + " " + cubeLockStatus[3]);
    }
    int CubeLockIndex()
    {
        return currentCube.name switch
        {
            "Cube10" => 0,
            "Cube20" => 1,
            "Cube30" => 2,
            "Cube40" => 3,
            _ => 0
        };
    }
    int PlacementLockIndex()
    {
        return currentPlace.name switch
        {
            "CubePlacement1" => 0,
            "CubePlacement2" => 1,
            "CubePlacement3" => 2,
            "CubePlacement4" => 3,
            _ => 0
        };
    }
} //end class

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

//}
//Removed from ReceivedFingerUpEvent
//switch (cubeName)
//{
//    case "Cube10":
//        if (cube10WaitingForFingerUp)
//        {
//            Debug.Log("CPHandler cube10 fingerUp  cube = " + currentCube.name);
//            cube10FingerPointerEvent.Invoke(cubeName, "fingerUp", currentCube);
//            cube10WaitingForFingerUp = false;
//        }
//        break;
//    case "Cube20":
//        if (cube20WaitingForFingerUp)
//        {
//            Debug.Log("CPHandler cube20 fingerUp  cube = " + currentCube.name);
//            cube20FingerPointerEvent.Invoke(cubeName, "fingerUp", currentCube);
//            cube20WaitingForFingerUp = false;
//        }
//        break;
//    case "Cube30":
//        if (cube30WaitingForFingerUp)
//        {
//            Debug.Log("CPHandler cube30 fingerUp  cube = " + currentCube.name);
//            cube30FingerPointerEvent.Invoke(cubeName, "fingerUp", currentCube);
//            cube30WaitingForFingerUp = false;
//        }
//        break;
//    case "Cube40":
//        if (cube40WaitingForFingerUp)
//        {
//            Debug.Log("CPHandler cube40 fingerUp  cube = " + currentCube.name);
//            cube40FingerPointerEvent.Invoke(cubeName, "fingerUp", currentCube);
//            cube40WaitingForFingerUp = false;
//        }
//        break;
//    default: Debug.Log("ReceivedFingerUpEvent sent UFO");
//        break;
//}

//Removed from CubeEnterExitPlacement
//    switch (_cube.name)
//    {
//        case "Cube10":
//            cube10WaitingForFingerUp = cubeEntered;
//            break;
//        case "Cube20":
//            cube20WaitingForFingerUp = cubeEntered;
//            break;
//        case "Cube30":
//            cube30WaitingForFingerUp = cubeEntered;
//            break;
//        case "Cube40":
//            cube40WaitingForFingerUp = cubeEntered;
//            break;
//        default:
//            break;
//    }
// EVENT Stuff not needed 
//[System.Serializable]
//public class FingerPointerEvent : UnityEvent<string, string> { }
//[System.Serializable]
//public class HandlerFingerPointerUpEvent : UnityEvent<string, string, int> { }
//CubeEnteredSolutionMatrix waits for us to send these - NOT after 12/18/22
//  HandlerFingerPointerUpEvent handlerFingerPointerEvent;  //empty class declared above - before this class -- we receive these 

//  bool cube10WaitingForFingerUp, cube20WaitingForFingerUp, cube30WaitingForFingerUp, cube40WaitingForFingerUp;

//public Cube10FingerPointerEvent cube10FingerPointerEvent;  // we send these -  NOT after 12/18/22
//public Cube20FingerPointerEvent cube20FingerPointerEvent;  //
//public Cube30FingerPointerEvent cube30FingerPointerEvent;  // 
//public Cube40FingerPointerEvent cube40FingerPointerEvent;  // 

//More unused stuff 
//GameObject cube10, cube20, cube30, cube40;
//GameObject placement1, placement2, placement3, placement4;
//Find the Cube GameObjects (Cube10, Cube20, etc.)  we could do an array - maybe later 
//cube10 = GameObject.Find("Cube10");
//cube20 = GameObject.Find("Cube20");
//cube30 = GameObject.Find("Cube30");
//cube40 = GameObject.Find("Cube40");
////Find the Placement GameObjects (CubePlacement1, CubePlacemrnt2, etc.)  we could do an array - maybe later 
//placement1 = GameObject.Find("CubePlacement1");
//placement2 = GameObject.Find("CubePlacement2");
//placement3 = GameObject.Find("CubePlacement3");
//placement4 = GameObject.Find("CubePlacement4");

//if (handlerFingerPointerEvent == null) handlerFingerPointerEvent = new HandlerFingerPointerUpEvent();  //not sure but it stopped the null reference 
//handlerFingerPointerEvent.AddListener(ReceivedFingerUpEvent);   // in paren is the method in this script that gets invoked 