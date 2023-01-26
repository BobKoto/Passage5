using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Cinemachine;
using StarterAssets; 
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
    public CinemachineVirtualCamera cubeGameCam;
    int originalCamPriority;
    public FingerPointerEvent fingerPointerEvent; //12/18/22 we receive these from ActOnTouch 
    public CubeGameBoardEvent cubeGameBoardEvent;  //empty class declared above - before this class // took away public see line 31 
    //GameObject row1Sum, row2Sum, col1Sum, col2Sum ;

    TMP_Text row1SumText,row2SumText, col1SumText, col2SumText ;

    GameObject inputControls;
    public AudioManager audioManager;
    //bool cubePlaceHolder1Taken, cubePlaceHolder2Taken, cubePlaceHolder3Taken, cubePlaceHolder4Taken; //never used 
    bool cubeGameIsActive;
    int place1CubeValue, place2CubeValue, place3CubeValue, place4CubeValue;
    int cubesOccupied;
    int cubeGameRoundNumber = 1, roundsWon, roundsLost;
    public float cubeGameTimeLimit;
    Coroutine timeLimiter;
    // ////////////////////START MERGE OF PlayerEnterCubeGame.cs ///////////////////////////
    public MyIntEvent m_MyEvent;  //for TextCloud 

    const string helpNeedHI = "#Need human assist!";  //for textcloud
    const string okLetsGo = "#Ok, let's go";
    const string cubeGameStartRound1Text = "Start Round 1"; //for the startButton text
    const string cubeGameStartRound2Text = "Start Round 2";
    const string cubeGameStartRound3Text = "Start Round 3";
    const string cubeGameRoundsDoneText = "DONE";
    const string cubeGameRound1of3 = "Round 1 of 3";  // should be simpler than messing with strings 
    const string cubeGameRound2of3 = "Round 2 of 3 Next";
    const string cubeGameRound3of3 =     "Round 3 of 3 Next";
    const string cubeGameRoundsFinished = " So4uku2 Done    ";
    readonly int[] gameSums = new int[] { 30, 40, 50, 50, 60, 70 };  //cubes = 10, 20, 30, 40
    public GameObject player;
    Animator animator;
    ThirdPersonController thirdPersonController;
    public GameObject[] cubeGameCubes;
    GameObject[] cubeGamePlacement;
    GameObject[] cubeGameTargetSum;
    public GameObject menuButton, lightButton, cubeGameStartButton, cubeGameIsUnsolvableButton, cubeGameExitButton; //Buttons to toggle 
    public GameObject cubeGameResultText, cubeGameTimerText;
    TMP_Text cubeGameWonOrLostText, cubeGameRoundText;
    public TMP_Text cubeGameStartButtonText, cubeGameTimeLeftText;
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

        // if (!audioManager) audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();  //Duplicated in merge
        // ////////////////////START MERGE OF PlayerEnterCubeGame.cs ///////////////////////////
        if (!audioManager) audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        cubeGameWonOrLostText = cubeGameResultText.GetComponent<TMP_Text>();
        cubeGameRoundText = GameObject.Find("CubeGameRoundText").GetComponent<TMP_Text>();
        cubeGameStartButtonText.text = cubeGameStartRound1Text;
        cubeGamePlacement = GameObject.FindGameObjectsWithTag("CubeGamePlacement");
        cubeGameTargetSum = GameObject.FindGameObjectsWithTag("TargetSum");
       // inputControls = GameObject.Find("Joysticks_StarterAssetsInputs_Joysticks");  //Duplicated in merge
        cubePlacementPosition = new Vector3[cubeGamePlacement.Length];
        cubeTransformStartPosition = new Vector3[cubeGameCubes.Length];
        cubeGameTargetSumText = new TMP_Text[4];
        for (int i = 0; i <= cubeGameCubes.Length - 1; i++)  
        {
            cubeTransformStartPosition[i] = cubeGameCubes[i].transform.position;
            cubePlacementPosition[i] = cubeGamePlacement[i].transform.position;
            cubeGameTargetSumText[i] = cubeGameTargetSum[i].GetComponent<TMP_Text>();
        }
        originalCamPriority = cubeGameCam.Priority;
        animator = player.GetComponent<Animator>();
        thirdPersonController = player.GetComponent<ThirdPersonController>();
        // ////////////////////END MERGE OF PlayerEnterCubeGame.cs ///////////////////////////
    }
    public void CheckCubeMovement(GameObject go, string cubeName)  //ActOnTouch sent a fingerUp event - meaning player dragged a cube 
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
    void CalculateTheMatrix()  
    {
        row1SumText.text = (place1CubeValue + place2CubeValue).ToString(); // + " added across";
        col1SumText.text = (place1CubeValue + place3CubeValue).ToString(); // + " added down";
        row2SumText.text = (place3CubeValue + place4CubeValue).ToString(); // + " added across" ;
        col2SumText.text = (place2CubeValue + place4CubeValue).ToString(); // + " added down"; 
        if (cubesOccupied == 4)  // the game cube placements are finished as player filled 4th placement -- now check for won/lost
        {
            CheckCubePlacementResults(); //win or lose 
        }
    }
    // ///////////////////////Next 3 methods decide if round is lost or won //////////////////////////////
    void CheckCubePlacementResults()  //win or lose - player placed cubes, are they correct?
    {
        //Debug.Log("CheckCubePlacementResults() is " +gameSums[0] + ", " + gameSums[1] + ", " + gameSums[2] + ", " + gameSums[3] + ", " + gameSums[4]);
        bool row1IsGood, row2IsGood, col1IsGood, col2IsGood, roundWonPlacingCubes;
        int placeRow1 = 0, placeCol1 = 0, placeRow2 = 0, placeCol2 = 0;
        placeRow1 += place1CubeValue + place2CubeValue;
        placeCol1 += place1CubeValue + place3CubeValue;
        placeRow2 += place3CubeValue + place4CubeValue;
        placeCol2 += place2CubeValue + place4CubeValue;

        row1IsGood = placeRow1 == gameSums[0];
        col1IsGood = placeCol1 == gameSums[2];
        row2IsGood = placeRow2 == gameSums[1];
        col2IsGood = placeCol2 == gameSums[3];
        if (row1IsGood && row2IsGood && col1IsGood && col2IsGood)
        {
            cubeGameWonOrLostText.text = "Hooray you Won";
            audioManager.PlayAudio(audioManager.clipApplause);
            roundWonPlacingCubes = true;
        } else
        {
            cubeGameWonOrLostText.text = "Awwwww you lose";
            audioManager.PlayAudio(audioManager.clipfalling);
            roundWonPlacingCubes = false;
        }
        //here setting cubeGameIsActive = false; should be done - but it will?(if player drags one?) send cubes home making for an unsmooth transition
        ProcessCubeGameRoundEnd(roundWonPlacingCubes);
        //cubeGameResultText.SetActive(true); these 5 lines collapsed into ProcessCubeGameRoundEnd(bool)
        //cubeGameIsActive = false;
        //cubeGameRoundNumber += 1;
        //SetRoundNumberHeadingAndStartButtonText();
        //StopCoroutine(timeLimiter);
    }
    public void OnCubeGameIsUnsolvableButtonPressed()  //win or lose - play pressed "Can't be Solved" button - is that correct?
    {
        bool roundWonOnUnsolvablePress;
        Debug.Log("player pressed Can't Solve ");
        if (cubeGameIsUnsolvableButton) cubeGameIsUnsolvableButton.SetActive(false);
        if (GameCanBeSolved())
        {
            Debug.Log("Wrong --- Game CAN be solved!");
            cubeGameWonOrLostText.text = "Nope can be solved... Awwwwww";
            audioManager.PlayAudio(audioManager.clipfalling);
            roundWonOnUnsolvablePress = false;
           // cubeGameResultText.SetActive(true);
        }
        else
        {
            Debug.Log("Right --- Game CANNOT be solved!");
            cubeGameWonOrLostText.text = "Right! You Win! Hooray";
            audioManager.PlayAudio(audioManager.clipApplause);
            roundWonOnUnsolvablePress = true;
            // cubeGameResultText.SetActive(true);
        }
        ProcessCubeGameRoundEnd(roundWonOnUnsolvablePress);
        //cubeGameResultText.SetActive(true);
        //cubeGameIsActive = false;
        //cubeGameRoundNumber += 1;
        //SetRoundNumberHeadingAndStartButtonText();
        //StopCoroutine(timeLimiter);
    }
    // Here will be a timer and if it lapses the game is lost 
    IEnumerator CubeGameTimer(float timeLimit) //win or lose(if Time limit exceeded) -or- timer stopped by other action (above 2 methods) 
    {   //count limeLimit seconds and if exceeded we have a lost round 
        float timeLeft = timeLimit;
        bool timeLeftOnClock = false;//shouldn't ever be true can only determine a loss here when time elapses - others will stop this coroutine
        while (timeLeft >= 0)
        {
            cubeGameTimeLeftText.text = timeLeft.ToString();
            timeLeft -= 1;
            yield return new WaitForSeconds(1);
        }
        audioManager.PlayAudio(audioManager.clipfalling);
        cubeGameWonOrLostText.text = "Awwww Time ran out...";
        ProcessCubeGameRoundEnd(timeLeftOnClock);

        //cubeGameResultText.SetActive(true);
        //cubeGameIsActive = false; //causes any cubes the player drags/moves to be sent back to their home position(s)
        //cubeGameRoundNumber += 1;
        //SetRoundNumberHeadingAndStartButtonText();
        yield break;
    }
    // ///////////////////////Above 3 methods decide if round is lost or won //////////////////////////////
    void ProcessCubeGameRoundEnd(bool aWin)
    {
        Debug.Log("ProcessCubeGameRoundEnd got a win? " + aWin);
        cubeGameResultText.SetActive(true);
        cubeGameIsActive = false;
        cubeGameRoundNumber += 1;
        SetRoundNumberHeadingAndStartButtonText();
        if (timeLimiter != null) StopCoroutine(timeLimiter);
        if (!cubeGameStartButton.activeSelf) cubeGameStartButton.SetActive(true);
        // Originals here - all are commented out
        // FROM CheckCubePlacementResults()  //win or lose 

        //cubeGameResultText.SetActive(true);
        //cubeGameIsActive = false;
        //cubeGameRoundNumber += 1;
        //SetRoundNumberHeadingAndStartButtonText();
        //if (timeLimiter != null) StopCoroutine(timeLimiter);

        //// FROM OnCubeGameIsUnsolvableButtonPressed()  //win or lose 

        //cubeGameResultText.SetActive(true);
        //cubeGameIsActive = false;
        //cubeGameRoundNumber += 1;
        //SetRoundNumberHeadingAndStartButtonText();
        //if (timeLimiter != null) StopCoroutine(timeLimiter);

        //// FROM IEnumerator CubeGameTimer(float timeLimit) //win or lose 

        //cubeGameResultText.SetActive(true);
        //cubeGameIsActive = false; //causes any cubes the player drags/moves to be sent back to their home position(s)
        //cubeGameRoundNumber += 1;
        //SetRoundNumberHeadingAndStartButtonText();
        //if (timeLimiter != null) StopCoroutine(timeLimiter); //here from the coroutine itself 
    }
    void SetupNewCubeGameRound()
    {
        SendCubesToHomePositions();
        ResetTargetTextsToZero();
        ResetRowAndColumnSumsToZero();
        ResetPlaceCubeValuesToZero();
        cubeGameResultText.SetActive(false);
        cubeGameIsActive = true;  // allow cubes to be placed
        if (cubeGameStartButton) cubeGameStartButton.SetActive(false);
        if (cubeGameTimerText) cubeGameTimerText.SetActive(true);
        cubeGameIsUnsolvableButton.SetActive(true);
        EnableDisableInputControls(false);

        SeedCubePuzzle();

    }
    void SetRoundNumberHeadingAndStartButtonText()
    {
        switch (cubeGameRoundNumber)
        {
            case 1: cubeGameRoundText.text = cubeGameRound1of3;
                    cubeGameStartButtonText.text = cubeGameStartRound1Text;
                break;
            case 2: cubeGameRoundText.text = cubeGameRound2of3;
                    cubeGameStartButtonText.text = cubeGameStartRound2Text;
                break;
            case 3: cubeGameRoundText.text = cubeGameRound3of3;
                    cubeGameStartButtonText.text = cubeGameStartRound3Text;
                break;
            default:
                cubeGameRoundText.text = cubeGameRoundsFinished;
                cubeGameStartButtonText.text = cubeGameRoundsDoneText;
                break;

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
    void EnableDisableInputControls(bool _enable) //joystick etc.
    {
        if (_enable)
        {
        if (inputControls) inputControls.SetActive(true);
        }else
        {
            if (inputControls) inputControls.SetActive(false);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            cubeGameRoundNumber = 1;
            cubeGameCam.Priority = 12;
            EnableDisableInputControls(false); // disable the inputControls
            if (menuButton) menuButton.SetActive(false);
            if (lightButton) lightButton.SetActive(false);
            cubeGameStartButton.SetActive(true);
            //if (cubeGameExitButton) cubeGameExitButton.SetActive(true); //We may not need an Exit Button at all 
            audioManager.PlayAudio(audioManager.clipDRUMROLL);
            TellTextCloud(helpNeedHI);
            animator.speed = 0;
            SetRoundNumberHeadingAndStartButtonText();
            if (thirdPersonController) thirdPersonController.enabled = false;
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
        if (cubeGameIsUnsolvableButton) cubeGameIsUnsolvableButton.SetActive(false);
        if (cubeGameExitButton) cubeGameExitButton.SetActive(false);
        animator.speed = 1;
        EnableDisableInputControls(true); // if (inputControls) inputControls.SetActive(true);
    }
    void SendCubesToHomePositions()
    {
        for (int i = 0; i <= cubeGameCubes.Length - 1; i++)  //Restore the cubes to home/original positions 
        {
            cubeGameCubes[i].transform.position = cubeTransformStartPosition[i];
        }
        cubesOccupied = 0;
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
    void ResetPlaceCubeValuesToZero()
    {
        place1CubeValue = 0;
        place2CubeValue = 0;
        place3CubeValue = 0;
        place4CubeValue = 0;
    }

    public void OnCubeGameExitButtonPressed()
    {
        // ExitTheCubeGame();
        // ///////Next 8 lines moved to SetupNewCubeGameRound()
        //if (!cubeGameStartButton.activeSelf) cubeGameStartButton.SetActive(true);  
        //SendCubesToHomePositions();
        //ResetTargetTextsToZero();
        //ResetRowAndColumnSumsToZero();
        //ResetPlaceCubeValuesToZero();
        //cubeGameResultText.SetActive(false);
        //if (cubeGameTimerText) cubeGameTimerText.SetActive(false);
        //cubeGameIsActive = false;

        EnableDisableInputControls(true);

    }
    public void OnCubeGameStartButtonPressed()
    {
        if (cubeGameRoundNumber <= 3)
        {
            audioManager.PlayAudio(audioManager.clipding);
            SetupNewCubeGameRound();
            timeLimiter = StartCoroutine(CubeGameTimer(cubeGameTimeLimit));
        }
        if (cubeGameRoundNumber > 3)  //Let Player move robot out of game and cause OnTriggerExit
        {
            if (cubeGameStartButton) cubeGameStartButton.SetActive(false);
            TellTextCloud(okLetsGo);
            cubeGameRoundNumber = 0;
            animator.speed = 1;
            if (thirdPersonController) thirdPersonController.enabled = true;
            EnableDisableInputControls(true);
        }
    }
    // ////////////////////END MERGE ///////////////////////
}  // end class 
