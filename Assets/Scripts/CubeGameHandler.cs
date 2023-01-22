using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Cinemachine;
//[System.Serializable]
//public class FingerPointerEvent : UnityEvent<GameObject, string> { }  // we receive these from ActOnTouch 
[System.Serializable]
public class CubeGameBoardEvent : UnityEvent<string, bool, string, int> { }  //this declaration i guess is needed to accept

public class CubeGameHandler : MonoBehaviour
//Component of CubeGame -- receives events from CubeEnteredSolutionMatrix.cs(was)/is now PlacementHandler  -- calculates row/column totals
//Here we need to figure if game is lost or won 
//How we do this is to seed the row/column with target sums that can or cannot be achieved to = 100
//So we need to add Texts(numeric values) to serve as targets 
//Some (randomly set) targets CAN be achieved while others cannot - therein lies our puzzle?
{
    public FingerPointerEvent fingerPointerEvent; //12/18/22 we receive these from ActOnTouch 
    public CubeGameBoardEvent cubeGameBoardEvent;  //empty class declared above - before this class // took away public see line 31 
    //GameObject row1Sum, row2Sum, col1Sum, col2Sum ;

    TMP_Text row1SumText,row2SumText, col1SumText, col2SumText ;

    GameObject inputControls;
    public AudioManager audioManager;
    //bool cubePlaceHolder1Taken, cubePlaceHolder2Taken, cubePlaceHolder3Taken, cubePlaceHolder4Taken;
    bool cubeGameIsActive;
    int place1CubeValue, place2CubeValue, place3CubeValue, place4CubeValue;
    int cubesOccupied;
    // ////////////////////START MERGE OF PlayerEnterCubeGame.cs ///////////////////////////
    public MyIntEvent m_MyEvent;
    const string helpNeedHI = "#Need human assist!";
    public CinemachineVirtualCamera cubeGameCam;
    int originalCamPriority;
    readonly int[] gameSums = new int[] { 30, 40, 50, 50, 60, 70 };  //cubes = 10, 20, 30, 40
    public GameObject player;
    Animator animator;
    public GameObject[] cubeGameCubes;
    GameObject[] cubeGamePlacement;
    GameObject[] cubeGameTargetSum;
    public GameObject menuButton, lightButton, cubeGameStartButton, cubeGameIsUnsolvableButton, cubeGameExitButton; //Buttons to toggle 
    public GameObject cubeGameResultText;
    TMP_Text cubeGameWonOrLostText;
    TMP_Text[] cubeGameTargetSumText;
    Vector3[] cubeTransformStartPosition;  // so we can put cubes back to their original positions
    Vector3[] cubePlacementPosition; // where to automatically place/start a Cube 
    // ////////////////////END MERGE ///////////////////////
    // Start is called before the first frame update
    void Start()
    {
        inputControls = GameObject.Find("Joysticks_StarterAssetsInputs_Joysticks");
        // Debug.Log("we have " + cubePlaceHolder.Length + " placeholders ");
        if (cubeGameBoardEvent == null) cubeGameBoardEvent = new CubeGameBoardEvent();  //not sure but it stopped the null reference 
        cubeGameBoardEvent.AddListener(CubeEnteredOrLeft);
        if (fingerPointerEvent == null) fingerPointerEvent = new FingerPointerEvent();  //not sure but it stopped the null reference 
        fingerPointerEvent.AddListener(CheckCubeMovement);
        row1SumText = GameObject.Find("Row1Sum").GetComponent<TMP_Text>();
        row2SumText = GameObject.Find("Row2Sum").GetComponent<TMP_Text>();
        col1SumText = GameObject.Find("Col1Sum").GetComponent<TMP_Text>();
        col2SumText = GameObject.Find("Col2Sum").GetComponent<TMP_Text>();
        ResetRowAndColumnSumsToZero();

        if (!audioManager) audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        // ////////////////////START MERGE OF PlayerEnterCubeGame.cs ///////////////////////////
        if (!audioManager) audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        cubeGameWonOrLostText = cubeGameResultText.GetComponent<TMP_Text>();
        //cubeGameCubes = GameObject.FindGameObjectsWithTag("CubeGameCube");
        cubeGamePlacement = GameObject.FindGameObjectsWithTag("CubeGamePlacement");
        cubeGameTargetSum = GameObject.FindGameObjectsWithTag("TargetSum");
        inputControls = GameObject.Find("Joysticks_StarterAssetsInputs_Joysticks");
        cubePlacementPosition = new Vector3[cubeGamePlacement.Length];
        cubeTransformStartPosition = new Vector3[cubeGameCubes.Length];
        cubeGameTargetSumText = new TMP_Text[4];
        for (int i = 0; i <= cubeGameCubes.Length - 1; i++)   //these 3 for loops can be consolidated...
        {
            cubeTransformStartPosition[i] = cubeGameCubes[i].transform.position;
            cubePlacementPosition[i] = cubeGamePlacement[i].transform.position;
            cubeGameTargetSumText[i] = cubeGameTargetSum[i].GetComponent<TMP_Text>();
        }
        originalCamPriority = cubeGameCam.Priority;
        animator = player.GetComponent<Animator>();
        // ////////////////////START MERGE OF PlayerEnterCubeGame.cs ///////////////////////////
    }
    public void CheckCubeMovement(GameObject go, string cubeName)  //ActOnTouch sent a fingerUp event
    {
        if (!cubeGameIsActive) //Player should not be moving cubes while no game in progress - we take the easy way out 
        {
            //We may want a sound effect here 
            SendCubesToHomePositions();
        }
    }
    public void CubeEnteredOrLeft(string cubeName, bool _entered, string placeName, int cubeValue)
        //event was Invoked Sucessfully by CubeEnteredSolutionMatrix - now ? 
    {
        if (cubeGameIsActive)
        {
            cubesOccupied = _entered ? cubesOccupied += 1 : cubesOccupied -= 1;
            if (cubesOccupied == 1 && cubeGameIsUnsolvableButton.activeSelf) cubeGameIsUnsolvableButton.SetActive(false);
            //  Debug.Log("CGH event recvd: " + cubeName + " " + _entered + " " + placeName + " cubeValue = " + cubeValue);
            switch (placeName)
            {
                case "CubePlacement1":
                    place1CubeValue = _entered ? cubeValue : 0;
                    break;
                case "CubePlacement2":
                    place2CubeValue = _entered ? cubeValue : 0;
                    break;
                case "CubePlacement3":
                    place3CubeValue = _entered ? cubeValue : 0;
                    break;
                case "CubePlacement4":
                    place4CubeValue = _entered ? cubeValue : 0;
                    break;
                default:
                    Debug.Log("CGHandler case got a default");
                    break;
            }
            CalculateTheMatrix();
        }
        else
            SendCubesToHomePositions();

    }
    void CalculateTheMatrix()  //can use params here?? yes but how?
    {
        row1SumText.text = (place1CubeValue + place2CubeValue).ToString(); // + " added across";

        col1SumText.text = (place1CubeValue + place3CubeValue).ToString(); // + " added down";

        row2SumText.text = (place3CubeValue + place4CubeValue).ToString(); // + " added across" ;

        col2SumText.text = (place2CubeValue + place4CubeValue).ToString(); // + " added down"; 
        if (cubesOccupied == 4)  // the game is finished as player filled 4th placement 
        {
           // Debug.Log("WE Have 4, Should enable inputControls...");
            if (inputControls) inputControls.SetActive(true);
        }
    }

    private void OnDisable()
    {
        cubeGameBoardEvent.RemoveListener(CubeEnteredOrLeft);
    }
    // ////////////////////START MERGE OF PlayerEnterCubeGame.cs ///////////////////////////
    void Shuffle(int[] intArr)           // Knuth shuffle algorithm :: courtesy of Wikipedia :)
    {
        for (int t = 0; t < intArr.Length; t++)  //30, 40, 50, 60, 70 
        {
            int tmp = intArr[t];  //3 --           //t0  t1  t2  t3  t4  
            int r = Random.Range(t, intArr.Length);//3, 4, 5, 6, 7 
            intArr[t] = intArr[r];
            intArr[r] = tmp;
        }
    }
    void SeedCubePuzzle()
    {
        Shuffle(gameSums);
        //Debug.Log("SeedCubePuzzle() is " +gameSums[0] + ", " + gameSums[1] + ", " + gameSums[2] + ", " + gameSums[3] + ", " + gameSums[4]);
        for (int i = 0; i <= cubeGameTargetSum.Length - 1; i++)
        {
            cubeGameTargetSumText[i].text = gameSums[i].ToString();
        }
        if (GameCanBeSolved()) { }  //do nothing yet
        //BEWARE int Random.Range (0,10) will return a random value 0 thru "9" 
    }
    bool GameCanBeSolved()
    {
        int firstPlusSecond, thirdPlusFourth;
        firstPlusSecond = gameSums[0] + gameSums[1];
        thirdPlusFourth = gameSums[2] + gameSums[3];
        if (firstPlusSecond == 100 && thirdPlusFourth == 100)
        {
            // Debug.Log("Game CAN be solved... theSum = " + theSum + " colSum = " + colSum + " rowSum = " + rowSum); 
            Debug.Log("Game CAN be solved... firstPlusSecond = " + firstPlusSecond + " and thirdPlusFourth = " + thirdPlusFourth);
            return true;
        }
        else
        {
            // Debug.Log("Game CANNOT be solved... theSum = " + theSum + " colSum = " + colSum + " rowSum = " + rowSum);
            Debug.Log("Game CANNOT be solved... firstPlusSecond = " + firstPlusSecond + " and thirdPlusFourth = " + thirdPlusFourth);
            return false;
        }
    }
    void DisableInputControls() //joystick etc.
    {
        if (inputControls) inputControls.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            cubeGameCam.Priority = 12;
            DisableInputControls();
            if (menuButton) menuButton.SetActive(false);
            if (lightButton) lightButton.SetActive(false);
            cubeGameStartButton.SetActive(true);
            if (cubeGameExitButton) cubeGameExitButton.SetActive(true);
            audioManager.PlayAudio(audioManager.clipDRUMROLL);
            TellTextCloud(helpNeedHI);
            animator.speed = 0;
        }
    }
    void TellTextCloud(string caption)
    {
        m_MyEvent.Invoke(5, 4, caption);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) ExitTheCubeGame();
    }
    void ExitTheCubeGame()
    {
        cubeGameCam.Priority = originalCamPriority;
        SendCubesToHomePositions();
        if (menuButton) menuButton.SetActive(true);
        if (lightButton) lightButton.SetActive(true);
        if (cubeGameStartButton) cubeGameStartButton.SetActive(false);
        animator.speed = 1;
        if (inputControls) inputControls.SetActive(true);
    }
    void SendCubesToHomePositions()
    {
        for (int i = 0; i <= cubeGameCubes.Length - 1; i++)  //Restore the cubes to home/original positions 
        {
            cubeGameCubes[i].transform.position = cubeTransformStartPosition[i];
        }
    }
    void ResetTargetTextsToZero()
    {
        for (int i = 0; i <= cubeGameTargetSum.Length - 1; i++)
        {
            cubeGameTargetSumText[i].text = 00.ToString();// or we could try "00"
        }
    }
    void ResetRowAndColumnSumsToZero()
    {
        row1SumText.text = "0";
        row2SumText.text = "0";
        col1SumText.text = "0";
        col2SumText.text = "0";
    }
    public void OnCubeGameIsUnsolvableButtonPressed()
    {
        Debug.Log("player pressed Can't Solve ");
        if (GameCanBeSolved())
        {
            Debug.Log("Wrong --- Game CAN be solved!");
            cubeGameWonOrLostText.text = "Nope can be solved... Awwwwww";
            audioManager.PlayAudio(audioManager.clipfalling);
            cubeGameResultText.SetActive(true);
        }
        else
        {
            Debug.Log("Right --- Game CANNOT be solved!");
            cubeGameWonOrLostText.text = "Right! You Win! Hooray";
            audioManager.PlayAudio(audioManager.clipApplause);
            cubeGameResultText.SetActive(true);
        }
        cubeGameIsActive = false;
    }
    public void OnCubeGameExitButtonPressed()
    {
        // ExitTheCubeGame();
        if (!cubeGameStartButton.activeSelf) cubeGameStartButton.SetActive(true);  //leave active for testing 
        SendCubesToHomePositions();
        ResetTargetTextsToZero();
        ResetRowAndColumnSumsToZero();
        cubeGameIsActive = false;
       // SeedCubePuzzle();
    }
    public void OnCubeGameStartButtonPressed()
    {
        // ExitTheCubeGame();
        SeedCubePuzzle();
        cubeGameIsActive = true;
        if (cubeGameStartButton) cubeGameStartButton.SetActive(false);
        cubeGameIsUnsolvableButton.SetActive(true);
    }
    // ////////////////////END MERGE ///////////////////////
}  // end class 
