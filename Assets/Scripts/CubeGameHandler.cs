using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Cinemachine;
using StarterAssets; 

public class CubeGameHandler : MonoBehaviour
//Component of CubeGame -- receives events from CubeEnteredSolutionMatrix.cs(was)/is now PlacementHandler  -- calculates row/column totals
//Here we need to figure if game is lost or won 
//How we do this is to seed the row/column with target sums that can or cannot be achieved to = 100
//So we need to add Texts(numeric values) to serve as targets 
//Some (randomly set) targets CAN be achieved while others cannot - therein lies our puzzle?
{
    [Header("The Now Play Sign and NextPage button aka The UI stuff as GameObjects")]
    public GameObject nextPage;
    public GameObject nowPlay;

    [Header("Touch Events")]
    FingerPointerEvent fingerPointerEvent; //12/18/22 we receive these from ActOnTouch 
    CubeGameBoardEvent cubeGameBoardEvent;  //empty class declared above - before this class // took away public 
    public CubeGamePlayButtonTouchEvent cubeGamePlayButtonTouchEvent;
    //public CubeGameMoveOnButtonTouchEvent cubeGameMoveOnButtonTouchEvent;
    CubeGameBoardUpEvent cubeGameBoardUpEvent;
    public CloudTextEvent m_CloudTextEvent;  //for TextCloud 

    [Header("Other Public Items")]
    public AudioManager audioManager;
    public float cubeGameTimeLimit;
    public GameObject player;
    public GameObject[] cubeGameCubes;
    public GameObject menuButton, lightButton, cubeGameStartButton, cubeGameIsUnsolvableButton, cubeGameExitButton; //Buttons to toggle 
    public GameObject cubeGameResultText, cubeGameTimerText;
    public GameObject cubeGameTitleText, cubeGameInstructText;
    public GameObject cubeGamesWonInteger, cubeGamesLostInteger;
    public GameObject cubeGameIntro;
    public GameObject cubeGame;
    public TMP_Text cubeGameStartButtonText, cubeGameTimeLeftText;

    public static bool cubeGameIsActive, cubeGameIsResetting; //set public static 1/30/23 //added cubeGameIsResetting 2/1/23 static may not be needed
    bool gameBoardIsUp;  //added 5/24/23

    GameObject inputControls;
    int place1CubeValue, place2CubeValue, place3CubeValue, place4CubeValue;
    int cubesOccupied, cubesToBeSentHome, cubesSentHome;
    int cubeGameRoundNumber = 1, roundsWon, roundsLost, nonPluggedSeed;
    TMP_Text row1SumText,row2SumText, col1SumText, col2SumText ;
    Coroutine timeLimiter;
    // ////////////////////START MERGE OF PlayerEnterCubeGame.cs ///////////////////////////
    public CinemachineVirtualCamera cubeGameCam;
    int originalCamPriority;

    const string helpNeedHI = "#Need human assist! \n #Your fingers please";  //for textcloud
    const string okLetsGo = "#Ok, let's go \n #Slide me out!";
    const string cubeGameStartRound1Text = "Start Round 1"; //for the startButton text
    const string cubeGameStartRound2Text = "Start Round 2";
    const string cubeGameStartRound3Text = "Start Round 3";
    const string cubeGameRoundsDoneText = "DONE";
    const string cubeGameRound1of3 = "Round 1 of 3";  // should be simpler than messing with strings 
    const string cubeGameRound2of3 = "Round 2 of 3 Next";
    const string cubeGameRound3of3 =     "Round 3 of 3 Next";
    const string cubeGameRoundsFinished = " So4uku2 Done    ";
    readonly int [] startGameSums = new int [] { 30, 40, 50, 50, 60, 70 };  //cubes = 10, 20, 30, 40
    int[] gameSums = new int[] { 30, 40, 50, 50, 60, 70 };  //cubes = 10, 20, 30, 40
    readonly int[] winningGameSums = new int[]  //odds of generating winnable game just from gameSums are too low, so enable (randomized) plugged wins here
        {
         30, 70, 40, 60, //index 0
         30, 70, 50, 50, //index 4
         30, 70, 60, 40, //index 8
         40, 60, 30, 70, //index 12
         40, 60, 50, 50, //index 16
         40, 60, 70, 30, //index 20
         50, 50, 30, 70, //index 24
         50, 50, 40, 60, //index 28
         50, 50, 60, 40, //index 32
         50, 50, 70, 30, //index 36
         60, 40, 30, 70, //index 40
         60, 40, 50, 50, //index 44
         60, 40, 70, 30, //index 48
         70, 30, 40, 60, //index 52
         70, 30, 50, 50, //index 56
         70, 30, 60, 40  //index 60
         };
    readonly int[] winningGameSumsIndex = new int[] { 0, 4, 8, 12, 16, 20, 24, 28, 32, 36, 40, 44, 48, 52, 56, 60 }; // so we have 16
                                                                                                                     // int[] variableGameSums = new int[4];
    Animator animCubeGame; 

    Animator animator;
    ThirdPersonController thirdPersonController;

    GameObject[] cubeGamePlacement;
    GameObject[] cubeGameTargetSum;

    TMP_Text cubeGameWonOrLostText, cubeGameRoundText;

    TMP_Text[] cubeGameTargetSumText;
    Vector3[] cubeTransformStartPosition;  // so we can put cubes back to their original positions
    Vector3[] cubePlacementPosition; // where to automatically place/start a Cube 
    // ////////////////////END MERGE ///////////////////////
    // Start is called before the first frame update
    void Start()
    {
        inputControls = GameObject.Find("Joysticks_StarterAssetsInputs_Joysticks");
        //if (inputControls) Debug.Log(this.name + " found inputControls");
        //else  Debug.Log(this.name + "  inputControls  NOT FOUND?");

            //Events and Listeners
        if (cubeGameBoardEvent == null) cubeGameBoardEvent = new CubeGameBoardEvent();  //not sure but it stopped the null reference 
        cubeGameBoardEvent.AddListener(CubeEnteredOrLeft);
        if (fingerPointerEvent == null) fingerPointerEvent = new FingerPointerEvent();  //not sure but it stopped the null reference 
        fingerPointerEvent.AddListener(CheckCubeMovement);
        //if (cubeGamePlayButtonTouchEvent == null) cubeGamePlayButtonTouchEvent = new CubeGamePlayButtonTouchEvent(); //DeImp 0n 5/7/23
        //cubeGamePlayButtonTouchEvent.AddListener(PlayButtonPressedOnIntro);
        //if (cubeGameMoveOnButtonTouchEvent == null) cubeGameMoveOnButtonTouchEvent = new CubeGameMoveOnButtonTouchEvent(); //DeImp 0n 5/7/23
        //cubeGameMoveOnButtonTouchEvent.AddListener(MoveOnButtonPressed);
        if (cubeGameBoardUpEvent == null)
            cubeGameBoardUpEvent = new CubeGameBoardUpEvent();
        cubeGameBoardUpEvent.AddListener(OnGameBoardUpStoreCubeHomePositions);


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
        animCubeGame = cubeGame.GetComponent<Animator>();
        thirdPersonController = player.GetComponent<ThirdPersonController>();
        // ////////////////////END MERGE OF PlayerEnterCubeGame.cs ///////////////////////////
    }
    //public void PlayButtonPressedOnIntro()  //DeImp 0n 5/7/23
    //{
    //    animCubeGame.SetTrigger("RaiseCubeGame");
    //    cubeGameIntro.SetActive(false);
    //    cubeGameStartButton.SetActive(true); //2/27/23 moved here from OnTriggerEnter
    //    TellTextCloud(helpNeedHI);//2/27/23 moved here from OnTriggerEnter
    //}

    public void OnCanvasNextPagePressedOnIntro()   //5/7/23 as part of moving to nextPage Canvas button controlling things 
    {
        if (cubeGameIntro.activeSelf)
        {
            animCubeGame.SetTrigger("RaiseCubeGame");
            cubeGameIntro.SetActive(false);
          //  cubeGameStartButton.SetActive(true); //2/27/23 moved here from OnTriggerEnter  //5/23/23
            TellTextCloud(helpNeedHI);//2/27/23 moved here from OnTriggerEnter
            if (nextPage) nextPage.SetActive(false);
            if (gameBoardIsUp)  //added 5/24/23
            {
                if (cubeGameStartButton) cubeGameStartButton.SetActive(true);
            }
        }
    }
    //public void MoveOnButtonPressed()   //DeImp 0n 5/7/23
    //{
    //    Debug.Log("CubeGame MoveOn Button Pressed!!");

    //}

    public void CheckCubeMovement(GameObject go, string cubeName)  //ActOnTouch sent a fingerUp event - meaning player dragged a cube 
    {
        if (!cubeGameIsActive) //Player should not be moving cubes while no game in progress - we take the easy way out // needed since AOTouch locked?
        {
            //We may want a sound effect here 
            //aDebug.Log("CheckCubeMovement(go,str) calling SendCubesToHomePositions()");
            SendCubesToHomePositions();
        }
    }
    public void CubeEnteredOrLeft(string cubeName, bool _entered, string placeName, int cubeValue)
        //event was Invoked Sucessfully by CubeEnteredSolutionMatrix - now ? 
    {
        if (!cubeGameIsActive && !_entered)
        {
            cubesToBeSentHome -= 1;
            //aDebug.Log("CGH Ev recvd when cubeGameIsActive is FALSE,  _entered = " + _entered + " ,cubesToBeSentHome = " + cubesToBeSentHome);
        }

        if (cubeGameIsActive)  //since we now shutdown ActOnTouch, cubeGameIsActive should ALWAYS be true EXCEPT when SendCubesToHomePositions causes
        {                      //OnTriggerExit(s) via CESMatrix.cs  - first call of SCTHPositions in SetupNewCubeGameRound() causes our timing issue... 
            cubesOccupied = _entered ? cubesOccupied += 1 : cubesOccupied -= 1;  //this is happening sometimes when it shouldn't??
            if (cubesOccupied == 1 && cubeGameIsUnsolvableButton.activeSelf) cubeGameIsUnsolvableButton.SetActive(false);
            //aDebug.Log("CGH Ev recvd: " + cubeName + " " + _entered + " " + placeName + " cubeValue = " + cubeValue + ", cubeGameIsActive = " + cubeGameIsActive );
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
                    //aDebug.Log("CGHandler case got a default");
                    break;
            }
            CalculateTheMatrix();
            return;
        }
        if (cubeGameIsResetting) return;
        //else //commented because we return above - but if the game is active we fall thru
        //{
            //aDebug.Log("CubeEnteredOrLeft(str,bool,str,int) calling SendCubesToHomePositions() _entered = " + _entered + " resetting = " +cubeGameIsResetting);
            SendCubesToHomePositions();  //issue involves this call getting done 
        //}
    }
    void CalculateTheMatrix()  
    {
        row1SumText.text = (place1CubeValue + place2CubeValue).ToString(); // + " added across";
        col1SumText.text = (place1CubeValue + place3CubeValue).ToString(); // + " added down";
        row2SumText.text = (place3CubeValue + place4CubeValue).ToString(); // + " added across" ;
        col2SumText.text = (place2CubeValue + place4CubeValue).ToString(); // + " added down"; 
        //aDebug.Log("CalculateTheMatrix says cubesOccupied = " + cubesOccupied);
        if (cubesOccupied == 4)  // the game cube placements are finished as player filled 4th placement -- now check for won/lost
        {
            CheckCubePlacementResults(); //win or lose 
        }
    }
    // ///////////////////////Next 3 methods decide if round is lost or won //////////////////////////////
    void CheckCubePlacementResults()  //win or lose - player placed cubes, are they correct?
    {
        ////aDebug.Log("CheckCubePlacementResults() is " +gameSums[0] + ", " + gameSums[1] + ", " + gameSums[2] + ", " + gameSums[3] + ", " + gameSums[4]);
        bool row1IsGood, row2IsGood, col1IsGood, col2IsGood, roundWonPlacingCubes;
        int placeRow1 = 0, placeCol1 = 0, placeRow2 = 0, placeCol2 = 0;
        placeRow1 += place1CubeValue + place2CubeValue;
        placeCol1 += place1CubeValue + place3CubeValue;
        placeRow2 += place3CubeValue + place4CubeValue;
        placeCol2 += place2CubeValue + place4CubeValue;

        if (Application.platform == RuntimePlatform.Android)  //maybe need on IOS too? // kludgy but I surrender, and it works 
        {   //flip the columns/rows
            row1IsGood = placeCol1 == gameSums[0];  
            col1IsGood = placeRow1 == gameSums[2];
            row2IsGood = placeCol2 == gameSums[1];
            col2IsGood = placeRow2 == gameSums[3];
        }
        else
        {  // as originally coded and working in Unity Editor
           row1IsGood = placeRow1 == gameSums[0];  //these 4 are original working but not for HHeld
           col1IsGood = placeCol1 == gameSums[2];
           row2IsGood = placeRow2 == gameSums[1];
           col2IsGood = placeCol2 == gameSums[3];
        }
        if (row1IsGood && row2IsGood && col1IsGood && col2IsGood)
        {
            cubeGameWonOrLostText.text = "Hooray you Won";
            audioManager.PlayAudio(audioManager.clipApplause);
            roundWonPlacingCubes = true;
        } else
        {
            int x = 0;
            if (Application.platform == RuntimePlatform.Android) x = 94;
            cubeGameWonOrLostText.text = "Awwwww you lose " + gameSums[0] + " " + gameSums[1] + " " + gameSums[2] + " " + gameSums[3] + " " + x;
            //Debug.Log(s0 + placeRow1 + s1 + placeCol1 + s2 + placeRow2 + s3+ placeCol2);
            audioManager.PlayAudio(audioManager.clipfalling);
            roundWonPlacingCubes = false;
        }
        //here setting cubeGameIsActive = false; should be done - but it will?(if player drags one?) send cubes home making for an unsmooth transition
        ProcessCubeGameRoundEnd(roundWonPlacingCubes);
    }
    public void OnCubeGameIsUnsolvableButtonPressed()  //win or lose - play pressed "Can't be Solved" button - is that correct?
    {
        bool roundWonOnUnsolvablePress;
        //aDebug.Log("player pressed Can't Solve ");
        if (cubeGameIsUnsolvableButton) cubeGameIsUnsolvableButton.SetActive(false);
        if (GameCanBeSolved())
        {
            //aDebug.Log("Wrong --- Game CAN be solved!");
            cubeGameWonOrLostText.text = "Nope can be solved... Awwwwww";
            audioManager.PlayAudio(audioManager.clipfalling);
            roundWonOnUnsolvablePress = false;
        }
        else
        {
            //aDebug.Log("Right --- Game CANNOT be solved!");
            cubeGameWonOrLostText.text = "Right! You Win! Hooray";
            audioManager.PlayAudio(audioManager.clipApplause);
            roundWonOnUnsolvablePress = true;
        }
        ProcessCubeGameRoundEnd(roundWonOnUnsolvablePress);
    }
    // Here is a timer, and if it lapses the game is lost 
    IEnumerator CubeGameTimer(float timeLimit) //win or lose(if Time limit exceeded) -or- timer stopped by other action (above 2 methods) 
    {   //count limeLimit seconds and if exceeded we have a lost round 
        float timeLeft = timeLimit;
        bool timeLeftOnClock = false;//shouldn't ever be true can only determine a loss here when time elapses - others will stop this coroutine
        while (timeLeft >= -1)
        {
            cubeGameTimeLeftText.text = timeLeft.ToString();
            timeLeft -= 1;
            if (timeLeft <= 0) cubeGameTimeLeftText.text = 0.ToString();
            yield return new WaitForSeconds(1);
        }

        audioManager.PlayAudio(audioManager.clipfalling);
        cubeGameWonOrLostText.text = "Awwww Time ran out...";
        ProcessCubeGameRoundEnd(timeLeftOnClock); 
        yield break;
    }
    // ///////////////////////Above 3 methods decide if round is lost or won //////////////////////////////
    void ProcessCubeGameRoundEnd(bool aWin)
    {
        //aDebug.Log("ProcessCubeGameRoundEnd got a win? " + aWin);
        if (aWin) roundsWon += 1; else roundsLost += 1;
        cubeGameResultText.SetActive(true);
        cubeGameIsActive = false;
        cubeGameRoundNumber += 1;
        SetCubeGameRoundsWonLostText();
        SetRoundNumberHeadingAndStartButtonText();
        if (timeLimiter != null) StopCoroutine(timeLimiter);
        if (!cubeGameStartButton.activeSelf) cubeGameStartButton.SetActive(true);
    }
    void SetupNewCubeGameRound()
    {
        //cubeGameIsResetting = true;
       // if (cubesOccupied > 0) // 3/5/23 
        {
            cubesToBeSentHome = cubesOccupied;
            cubeGameIsResetting = true;
            //aDebug.Log("SetupNewCubeGameRound() calling SendCubesToHomePositions() cubeGameIsActive = " + cubeGameIsActive + ", resetting = " + cubeGameIsResetting);
            SendCubesToHomePositions(); //acts as if cubeGameIsActive is already true!  the OnExitTriggers in CESMatrix.cs? yes but what do we do about it?
        }

        ResetTargetTextsToZero();
        ResetRowAndColumnSumsToZero();
        ResetPlaceCubeValuesToZero();
        SetCubeGameRoundsWonLostText();
        cubeGameResultText.SetActive(false);
        StartCoroutine(SetCubeGameIsActiveAfterCubesSentHome()); 
        if (cubeGameStartButton) cubeGameStartButton.SetActive(false);
        if (cubeGameTimerText) cubeGameTimerText.SetActive(true);
        cubeGameIsUnsolvableButton.SetActive(true);
        //EnableDisableInputControls(false);  5/18/23 we disable on enter then reenable after round 3 so this call may be unnecessary 

        //cubeGameIsResetting = false;  //99% sure this is setting BEFORE SendCubesToHomePositions() triggers our exit events
        // nonPlugged is a random 1,2 or 3 thus ensuring 2 winnables and 1 maybe winnable 
        if  (cubeGameRoundNumber == nonPluggedSeed) SeedCubePuzzle(); else SeedCubePuzzleWithWinner();  //either one calls GameCanBeSolved()
        timeLimiter = StartCoroutine(CubeGameTimer(cubeGameTimeLimit));  //moved from OnCubeGameStartButtonPressed()
    }
    IEnumerator SetCubeGameIsActiveAfterCubesSentHome()
    {  //need to revamp this coroutine to look for something like cubesToBeSentHome == 0
        //yield return new WaitForSeconds(_delay);
        yield return new WaitUntil(() => cubesToBeSentHome == 0);
        cubeGameIsActive = true;
        cubeGameIsResetting = false;  //99% sure this is setting BEFORE SendCubesToHomePositions() triggers our exit events
        //aDebug.Log("SetCubeGameIsActiveAfterDelay set cubeGameIsActive = TRUE; ");
    }
    void SetCubeGameRoundsWonLostText()
    {
        cubeGamesLostInteger.GetComponent<TextMeshPro>().text = roundsLost.ToString();
        cubeGamesWonInteger.GetComponent<TextMeshPro>().text = roundsWon.ToString();
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
        fingerPointerEvent.RemoveListener(CheckCubeMovement);
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
        startGameSums.CopyTo(gameSums, 0);
        Shuffle(gameSums);
        ////aDebug.Log("SeedCubePuzzle() is " +gameSums[0] + ", " + gameSums[1] + ", " + gameSums[2] + ", " + gameSums[3] + ", " + gameSums[4]);
        for (int i = 0; i <= cubeGameTargetSum.Length - 1; i++)
        {
            cubeGameTargetSumText[i].text = gameSums[i].ToString();
        }
        if (GameCanBeSolved()) { }  //do nothing yet
    }
    void SeedCubePuzzleWithWinner()
    {
        //BEWARE int Random.Range (0,10) will return a random value 0 thru "9" 
        int winnableIndex = winningGameSumsIndex[Random.Range(0, 16)];  // index to a random "row" of winning combo in winningGameSums array
        for (int i = 0; i <= cubeGameTargetSum.Length - 1; i++)
        {
            cubeGameTargetSumText[i].text = winningGameSums[winnableIndex+i].ToString();
            gameSums[i] = winningGameSums[winnableIndex + i];        // Plug the gameSums Array with our winner 
        }
        if (GameCanBeSolved()) { }  //do nothing yet
    }
    bool GameCanBeSolved()
    {
        int firstPlusSecond, thirdPlusFourth;
        if (Application.platform == RuntimePlatform.Android)
        {
            firstPlusSecond = gameSums[2] + gameSums[3];
            thirdPlusFourth = gameSums[0] + gameSums[1];
        }
        else
        {
            firstPlusSecond = gameSums[0] + gameSums[1];
            thirdPlusFourth = gameSums[2] + gameSums[3];
        }

        if (firstPlusSecond == 100 && thirdPlusFourth == 100)
        {
            // //aDebug.Log("Game CAN be solved... theSum = " + theSum + " colSum = " + colSum + " rowSum = " + rowSum); 
            //aDebug.Log("Game CAN be solved... firstPlusSecond = " + firstPlusSecond + " and thirdPlusFourth = " + thirdPlusFourth);
            return true;
        }
       // else
       // {
            // //aDebug.Log("Game CANNOT be solved... theSum = " + theSum + " colSum = " + colSum + " rowSum = " + rowSum);
            //aDebug.Log("Game CANNOT be solved... firstPlusSecond = " + firstPlusSecond + " and thirdPlusFourth = " + thirdPlusFourth);
            return false;
       // }
    }
    void EnableDisableInputControls(bool _enable) //joystick etc.
    {
        if (inputControls) 
            Debug.Log(this.name + " inputControls in EnableDisable Method found cubeGameRoundNumber = " + cubeGameRoundNumber + " _enable = " + _enable);
        else Debug.Log(this.name + "  inputControls in EnableDisable Method NOT FOUND?");
        if (_enable)
        {
        if (inputControls) inputControls.SetActive(true);
            Debug.Log(this.name + "   setting jsticks TRUE");
        }
        else
        {
             if (inputControls.activeSelf)
             {
                inputControls.SetActive(false);
                Debug.Log(this.name + "  setting jsticks FALSE");
             }
        }
    }
    void EnableDisableUIButtons(bool _enable) //Menu & Light
    {
        if (_enable)
        {
            if (menuButton) menuButton.SetActive(true);
            if (lightButton) lightButton.SetActive(true);
        }
        else
        {
            if (menuButton) menuButton.SetActive(false);
            if (lightButton) lightButton.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            cubeGameRoundNumber = 1;
            cubeGameCam.Priority = 12;
            EnableDisableInputControls(false); // disable the inputControls

            EnableDisableUIButtons(false);
            audioManager.PlayAudio(audioManager.clipDRUMROLL);

            animator.speed = 0;
            ResetCubeGameOnEntry(); //2/27/23 a little maint.

            if (thirdPersonController) thirdPersonController.enabled = false;
            nonPluggedSeed = Random.Range(1, 4); //which round to call SeedCubePuzzle() - other rounds get a Winnable
            cubeGameIntro.SetActive(true);
            nextPage.SetActive(true);
        }
    }
    void ResetCubeGameOnEntry()
    {
        if (!cubeGameTitleText.activeSelf) cubeGameTitleText.SetActive(true);  //maybe move these 4 into a method too...
        if (!cubeGameInstructText.activeSelf) cubeGameInstructText.SetActive(true);
        if (cubeGameResultText.activeSelf) cubeGameResultText.SetActive(false);
        if (cubeGameTimerText) cubeGameTimerText.SetActive(false);
        SetRoundNumberHeadingAndStartButtonText();
        ResetTargetTextsToZero();
        ResetRowAndColumnSumsToZero();
        ResetPlaceCubeValuesToZero();
        SetCubeGameRoundsWonLostText();
    }
    void TellTextCloud(string caption)
    {
        m_CloudTextEvent.Invoke(5, 4, caption);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) ExitTheCubeGame();
    }
    void ExitTheCubeGame()
    {
        if (cubesOccupied > 0)  //replicated from SetupNewCubeGameRound()
        {
            cubesToBeSentHome = cubesOccupied;
            cubeGameIsResetting = true;
            //aDebug.Log("ExitTheCubeGame() calling SendCubesToHomePositions() cubeGameIsActive = " + cubeGameIsActive + ", resetting = " + cubeGameIsResetting);
            SendCubesToHomePositions(); //acts as if cubeGameIsActive is already true!  the OnExitTriggers in CESMatrix.cs? yes but what do we do about it?
        }
        //cubeGameCam.Priority = originalCamPriority;// 2/3/23 try moving to 3rd round is over to when start(Done) is pressed 
        // //aDebug.Log("ExitTheCubeGame() calling SendCubesToHomePositions()");
        // SendCubesToHomePositions();
        roundsLost = 0;
        roundsWon = 0;
        EnableDisableUIButtons(true);
        if (cubeGameStartButton) cubeGameStartButton.SetActive(false);
        if (cubeGameIsUnsolvableButton) cubeGameIsUnsolvableButton.SetActive(false);
        if (cubeGameExitButton) cubeGameExitButton.SetActive(false);
        animator.speed = 1;
        // EnableDisableInputControls(true); // 5/18/23 already done by OnCubeGameStartButtonPressed() which is now a "DONE" button
    }

    public void OnGameBoardUpStoreCubeHomePositions()
    {
        // Debug.Log("OnGameBoardUpStoreCubeHomePositions() Called..........");
        gameBoardIsUp = true;
        for (int i = 0; i <= cubeGameCubes.Length - 1; i++)
        {
            cubeTransformStartPosition[i] = cubeGameCubes[i].transform.position;
            //  Debug.Log("OnGameBoardUpStoreCubeHomePositions() Transform position = " + cubeGameCubes[i].transform.position);

        }
        if (cubeGameStartButton) cubeGameStartButton.SetActive(true);  //5/23/23 so we wait until game board is up 
    }
    void SendCubesToHomePositions()
    {
        //aDebug.Log("SendCubesToHomePositions() called, cubesOccupied = " + cubesOccupied);
      //  if (cubesOccupied != 0)  // 3/5/23 commented so we send all cubes home whether or not they are in a placement 
        {
            for (int i = 0; i <= cubeGameCubes.Length - 1; i++)  //Restore the cubes to home/original positions 
            {
                cubeGameCubes[i].transform.position = cubeTransformStartPosition[i];
            }
            //aDebug.Log("set cubesOccupied to zero");
            cubesOccupied = 0;
        }
       // allCubesAreHome = true;  //may be defunct 2/1/23
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

    public void OnCubeGameExitButtonPressed()  //not sure we're gonna ever use this 
    {
        EnableDisableInputControls(true);
    }
    public void OnCubeGameStartButtonPressed()
    {
        if (cubeGameRoundNumber <= 3)
        {
            audioManager.PlayAudio(audioManager.clipding);
            SetupNewCubeGameRound();
            if (cubeGameTitleText) cubeGameTitleText.SetActive(false);
            if (cubeGameInstructText) cubeGameInstructText.SetActive(false);
            //timeLimiter = StartCoroutine(CubeGameTimer(cubeGameTimeLimit));  //moved into SetupNewCubeGameRound()
            return;
        }
       // if (cubeGameRoundNumber > 3)  //Here Start button is "DONE" - Let Player move robot out of game to OnTriggerExit  //should just be an "else"
            if (cubeGameStartButton) cubeGameStartButton.SetActive(false);
            TellTextCloud(okLetsGo);
            cubeGameRoundNumber = 0;
            cubeGameCam.Priority = originalCamPriority;// 2/3/23 try moving to 3rd round is over to when start(Done) is pressed 
           // animator.speed = 1;
            if (thirdPersonController) thirdPersonController.enabled = true;
            EnableDisableInputControls(true);
    }
    // ////////////////////END MERGE ///////////////////////
}  // end class 
