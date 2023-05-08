using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

//[System.Serializable]
//public class FingerPointerEvent : UnityEvent<GameObject, string> { }  // we receive these from ActOnTouch 
//[System.Serializable]
//public class CubeTriggerEnterExitEvent : UnityEvent<GameObject, string, GameObject, bool> { }  // we receive these from CESMatrix

public class CubePlacementHandler : MonoBehaviour
// Component of CubePlacements  // objects -- receives Cube enter Trigger events   /exit events to CubeGameHandler.cs
// GameObjects originate from the Events
{
    public AudioManager audioManager;

    public CubeGameBoardEvent cubeGameBoardEvent; //we send these to CubeGameHandler
    public FingerPointerEvent fingerPointerEvent; //12/18/22 we receive these from ActOnTouch 
    public CubeTriggerEnterExitEvent cubeTriggerEnterExitEvent;  // we receive these from CESMatrix
    public CubeGameBoardUpEvent cubeGameBoardUpEvent;

    GameObject[] cubeGameCubes;
    Vector3[] cubeTransformStartPosition;  // so we can put cubes back to their original positions
    GameObject currentCube, currentPlace;
    bool[] cubeLockStatus = new bool[4];
    GameObject[] cubeInThisPlacement = new GameObject[4];//set by SetPlacementLockStatus which may be removed 
    GameObject[] cubeLockPlacementObject = new GameObject[4];
    int cubeTransformStartPositionIndex;
    bool waitingFingerPointerExit, fingerPointerExitReceived;
    bool ignoreThisExit;
    GameObject nullGO;

    // Start is called before the first frame update
    void Start()
    {
       // Debug.Log(" Hello from CubePlacementHandler");
        if (!audioManager) audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();

        if (cubeTriggerEnterExitEvent == null) cubeTriggerEnterExitEvent = new CubeTriggerEnterExitEvent();
        cubeTriggerEnterExitEvent.AddListener(CubeEnterExitPlacement); //u change the listener (and method) name u must change Editor too!  

        if (fingerPointerEvent == null) fingerPointerEvent = new FingerPointerEvent();  //not sure but it stopped the null reference 
        fingerPointerEvent.AddListener(ReceivedFingerUpEvent);  //this line and one above needed to receive events

        if (cubeGameBoardUpEvent == null) cubeGameBoardUpEvent = new CubeGameBoardUpEvent();
        cubeGameBoardUpEvent.AddListener(OnCubeGameUpStoreCubePositions);

        cubeGameCubes = GameObject.FindGameObjectsWithTag("CubeGameCube");
        cubeTransformStartPosition = new Vector3[cubeGameCubes.Length];
        nullGO =  GameObject.CreatePrimitive(PrimitiveType.Cube);
        nullGO.name = "nullGO";
        cubeLockPlacementObject = new GameObject[cubeGameCubes.Length];
        cubeInThisPlacement = new GameObject[cubeGameCubes.Length];
        for (int i = 0; i <= cubeGameCubes.Length - 1; i++)
        {
            cubeTransformStartPosition[i] = cubeGameCubes[i].transform.position;
            cubeLockPlacementObject[i] = nullGO;
            cubeInThisPlacement[i] = nullGO;
        }
        StartCoroutine(WaitForFingerUpToLockCube());
    }
    public void OnCubeGameUpStoreCubePositions()
    {
        for (int i = 0; i <= cubeGameCubes.Length - 1; i++)
        {
            cubeTransformStartPosition[i] = cubeGameCubes[i].transform.position;
        }
    }
    public void CubeEnterExitPlacement(GameObject _place, string placementName, GameObject _cube, bool cubeEntered)  //Method name in Editor!
    {// OnTriggerEnters/Exits in CESMatrix sent here via this event 
        currentCube = _cube;
        currentPlace = _place;
        if (cubeEntered) 
        {
            waitingFingerPointerExit = true;
            //Debug.Log(" CubePlHandler: " + currentCube.name + " ENTERED " + currentPlace.name + "       waitingFingerPointerExit");
            return; //added 1/8/23 
        }
        waitingFingerPointerExit = false;
     //   Debug.Log("CubePlHandler: " + currentCube.name + " EXITED " + currentPlace.name + "  IgnoreExit = " + ignoreThisExit);
        if (cubeInThisPlacement[PlacementLockIndex()] != nullGO && !ignoreThisExit) //we have an issue here 1/12/23
        {
            //Here we need to ONLY unlock if it IS locked 
            if (currentCube == cubeInThisPlacement[PlacementLockIndex()])
            {
                SetCubeLockStatus(false);
                SetPlacementLockStatus(nullGO, false);
                int valueToSend = CubeValue(currentCube.name);
                cubeGameBoardEvent.Invoke(currentCube.name, false, currentPlace.name, valueToSend);  //Send event to CubeGameHandler
            }
        }
        ignoreThisExit = false;
    }
    public void ReceivedFingerUpEvent(GameObject _cube, string action)
    {
        // Here we get fingerUp(OnEndDrag) event from ActOnTouch -- either fingerUp or object dropped or both -- or worse, nothing
        currentCube = _cube;
        if(waitingFingerPointerExit) fingerPointerExitReceived = true;// added  if(waitingFingerPointerExit) on 1/13/23 seems ok
        if (!waitingFingerPointerExit && cubeLockStatus[CubeLockIndex()])  //cube moved WITHIN locked position (did not Exit) just need to relock
        {
            currentPlace = cubeLockPlacementObject[CubeLockIndex()];
            // Just align the Cube
            Vector3 targetPos;
            targetPos = new Vector3(currentCube.transform.position.x, currentPlace.transform.position.y, currentPlace.transform.position.z);
            currentCube.transform.position = targetPos;
        }
    }
    IEnumerator WaitForFingerUpToLockCube()
    {
        while (true)
        {
            if (waitingFingerPointerExit && fingerPointerExitReceived)
            {
                LockTheCubeDownOrSendItHome();
                fingerPointerExitReceived = false;
                waitingFingerPointerExit = false;
            }
            yield return null;
        }
    }
    void LockTheCubeDownOrSendItHome()  // Aligns cube to placement 
    {
        if (cubeInThisPlacement[PlacementLockIndex()] == nullGO)   //check currentPlace if null Align/Lock 
        {
            // Lock the cube and give audio feedback 
            Vector3 targetPos;
            targetPos = new Vector3(currentCube.transform.position.x, currentPlace.transform.position.y, currentPlace.transform.position.z);
            currentCube.transform.position = targetPos;
            if (CubeGameHandler.cubeGameIsActive) audioManager.PlayAudio(audioManager.TYPE);                       
            SetCubeLockStatus(true);
            SetPlacementLockStatus(currentCube, true);  //e.g. case "CubePlacement1": cubeInThisPlacement[0] = cubeToLock;
            int valueToSend = CubeValue(currentCube.name);
            cubeGameBoardEvent.Invoke(currentCube.name, true, currentPlace.name, valueToSend); //Send event to CubeGameHandler
            return;  //OR fall thru to SendCubeHome()
        }
        SendCubeHome();
    }
    void SendCubeHome() //(float timeInSeconds)
    {
        ignoreThisExit = true; //moved from under to above next line 1/12/23
        currentCube.transform.position = cubeTransformStartPosition[CubeTransformStartPositionIndex(currentCube.name)];
        audioManager.PlayAudio(audioManager.WHOOSH);
    }
    void SetCubeLockStatus(bool lockStatus)
    {// here currentPlace and currentCube come from global context
        switch (currentCube.name)
        {
            case "Cube10":
                cubeLockStatus[0] = lockStatus;
                cubeLockPlacementObject[0] = lockStatus? currentPlace : nullGO;  //associate/disassociate placement with cube 
                break;
            case "Cube20":
                cubeLockStatus[1] = lockStatus;
                cubeLockPlacementObject[1] = lockStatus ? currentPlace : nullGO;
                break;
            case "Cube30":
                cubeLockStatus[2] = lockStatus;
                cubeLockPlacementObject[2] = lockStatus ? currentPlace : nullGO;
                break;
            case "Cube40":
                cubeLockStatus[3] = lockStatus;
                cubeLockPlacementObject[3] = lockStatus ? currentPlace : nullGO;
                break;
            default:
                Debug.Log("SetCubeLockStatus(bool lockStatus) got ??? ");
                break;
        }
    }
    void SetPlacementLockStatus(GameObject cubeToLock, bool cubeLocked)
    {
      //  string s1;
        switch (currentPlace.name)
        {
            case "CubePlacement1":
                cubeInThisPlacement[0] = cubeLocked? cubeToLock : nullGO;
                break;
            case "CubePlacement2":
                cubeInThisPlacement[1] = cubeLocked ? cubeToLock : nullGO;
                break;
            case "CubePlacement3":
                cubeInThisPlacement[2] = cubeLocked ? cubeToLock : nullGO;
                break;
            case "CubePlacement4":
                cubeInThisPlacement[3] = cubeLocked ? cubeToLock : nullGO;
                break;
            default:
                Debug.Log("CASE DEFAULT SetPlacementLockStatus(GameObject cubeToLock) got " + cubeToLock.name + " into  " + currentPlace.name);
                break;
        }
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
    private void OnDisable()
    {
        StopAllCoroutines();
        cubeTriggerEnterExitEvent.RemoveListener(CubeEnterExitPlacement);
        fingerPointerEvent.RemoveListener(ReceivedFingerUpEvent);
    }
} //end class