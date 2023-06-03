using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.Animations;
using TMPro;
using Cinemachine;
using UnityEngine.Events;
using CarouselAndMovingPlatforms;
//using UnityEditor.Animations;
using System.Linq;
using UnityEngine.AI;

public class MontyStopTrigger : MonoBehaviour
{  //Component of MontyStopTrigger
    //MovingPlatform movingPlatform;  //removed 5/26/23
    [Header("UI Buttons")]
    public GameObject stopButton;
    public GameObject goButton;

    [Header("Monty Game Doors and Boxes")]
    public GameObject montyDoor1;
    public GameObject montyDoor2;
    public GameObject montyDoor3;
    public GameObject montyDoorsAndBoxes;

    public GameObject randomTest; //contains the script that instantiates the randomized text of winners and losers 

    [Header("Text Above the Doors")]
    public GameObject mainMontySign;
    public TMP_Text montyGameSignText;

    [Header("Monty Game Intro")]
    public GameObject montyGameIntro;

    [Header("Monty Game Barriers")]
    public GameObject montyGameBarriers;

    [Header("The Player")]
    public GameObject playerArmature;

    public GameObject evilTwin;
    public GameObject goodTwin;

    public float timeToPauseOnAnyTwin = 4f;

    [Header("Cinemachine Cameras")]
    public CinemachineVirtualCamera thirdPersonFollowCam;
    public CinemachineVirtualCamera camOnPlayer;
    public CinemachineVirtualCamera montyGameCam;
    public CinemachineVirtualCamera camOnTwin;

    public float zoomAmount = 23f;
    public float zoomTime = 2.5f;

    [Header("Animations")]
    Animation animClipMontyDoorsAndBoxes;
    public Animation raiseMontyDoorsAndBoxes;
    public Animation animDoor1Down;
    public Animation animDoor2Down;
    public Animation animDoor3Down;
    public float xPos = 200;
    //int originalMontyGameCamPriority, originalCamOnEvilTwinPriority, originalCamOnGoodTwinPriority, originalCamOnTwinPriority; 6/2/23
    int originalMontyGameCamPriority, originalCamOnlayerPriority, originalCamOnTwinPriority;
    [Header("The Input System canvas Joystick etc.")]
    public GameObject inputControls;

    [Header("The Now Play Sign and NextPage button aka The UI stuff as GameObjects")]
    public GameObject nextPage;
    public GameObject nowPlay;

    bool playerPickedDoor1, playerPickedDoor2, playerPickedDoor3, door1Down, door2Down, door3Down;
    bool awaitingFinalDoorPick, playerPickedWinner, doorResultsShowing;
   // bool ignoreNextPagePress; //, nextPagePressed;   //5/31/23 nextPagePressed not used //6/1/23 neither is ignoreNextPagePress
    bool waitingForTextExtinguishEvent, waitingForNextPagePressEvent;

    public static bool montyGameEnded,montyGameActive, twinActivated,  montyGameAllowDoorTouch ;     //evilTwinActivated, goodTwinActivated,
    bool montyDoorDownEventReceived, montyDramaAudioFinishedEventReceived;

    int doorNumberDown, theWinningDoor;
    float originalMoveSpeed, originalSprintSpeed, originalPlayerSpeed;

    ThirdPersonController thirdPersonController;
    CharacterController characterController;

    public PlayerEnteredRelevantTrigger triggerEvent;

    MontyDoorTouchEvent montyDoorTouchEvent;
   // public MontyPlayButtonTouchEvent montyPlayButtonTouchEvent;
    MontyDoorDownEvent montyDoorDownEvent;
    AudioClipFinishedEvent audioClipFinishedEvent;
    public CloudTextEvent m_CloudTextEvent;  //for TextCloud 
    CanvasNextPagePressedEvent m_CanvasNextPagePressedEvent;
    CloudTextExtinguishedEvent m_CloudTextExtinguishedEvent;

    Animator animDoor1, animDoor2, animDoor3, animMontyDoorsAndBoxes, animMontyGameIntro, animPlayer;
    AudioManager audioManager;

    public GameObject resetJoystickObject;  //Maybe defunct 2/27/23 and before 
    BoxCollider entryCollider;
    MeshRenderer m1, m2, m3;
    const string evilTwinSpeaks1 = "Hello, Hashnag, you mechanical jerk. " +
        "Prepare yourself! If I catch you in my sensors I'll slice your servos to bits.";
    const string goodTwinSpeaks1 = "Hello, Hashy, I'll keep my sensors out for you...";
    const string playerSpeakstoEvilTwin1 = "#Hello, Metal, thanks for your cutting remarks!";
    const string playerSpeaksToGoodTwin1 = "Hello, TikTak! Always good to see a friend!";

    enum MontyGameState :int
    {
        MontyGameInProgress,
        MontyGameInDialogue,
        MontyGameOver
    }
    MontyGameState montyGameState;
    void Start()
    {
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        animDoor1 = montyDoor1.GetComponent<Animator>();
        animDoor2 = montyDoor2.GetComponent<Animator>();
        animDoor3 = montyDoor3.GetComponent<Animator>();
        animMontyDoorsAndBoxes = montyDoorsAndBoxes.GetComponent<Animator>();
        animClipMontyDoorsAndBoxes = montyDoorsAndBoxes.GetComponent<Animation>();  //3/4/23 not imp'd??
        animMontyGameIntro = montyGameIntro.GetComponent<Animator>();
        animPlayer = playerArmature.GetComponent<Animator>();

        thirdPersonController = playerArmature.GetComponent<ThirdPersonController>();
        characterController = playerArmature.GetComponent<CharacterController>();
        originalMoveSpeed = thirdPersonController.MoveSpeed;
        originalSprintSpeed = thirdPersonController.SprintSpeed;
        originalPlayerSpeed = animPlayer.speed;

        if (triggerEvent == null)
            triggerEvent = new PlayerEnteredRelevantTrigger();

        if (montyDoorTouchEvent == null)
            montyDoorTouchEvent = new MontyDoorTouchEvent();
        montyDoorTouchEvent.AddListener(OnMontyDoorTouch);

        if (montyDoorDownEvent == null)
            montyDoorDownEvent = new MontyDoorDownEvent();
        montyDoorDownEvent.AddListener(OnMontyDoorDown);

        if (audioClipFinishedEvent == null)
            audioClipFinishedEvent = new AudioClipFinishedEvent();
        audioClipFinishedEvent.AddListener(OnAudioClipFinished);

        if (m_CanvasNextPagePressedEvent == null)
            m_CanvasNextPagePressedEvent = new CanvasNextPagePressedEvent();

        if (m_CloudTextExtinguishedEvent == null)
            m_CloudTextExtinguishedEvent = new CloudTextExtinguishedEvent();

        originalMontyGameCamPriority = montyGameCam.Priority; //10
      //  originalCamOnEvilTwinPriority = camOnEvilTwin.Priority;  //10  //removed 5/26/23
        originalCamOnTwinPriority = camOnTwin.Priority;  //10
        entryCollider = gameObject.GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)   //Player enters the MontyGame area
    {
        if (other.CompareTag("Player") || other.CompareTag("MovingPlatform"))
        {
            if (!montyGameEnded)
            {
                PlayTheMontyGame();  //so let's enable the door touches here - i bet no diff  //OK so we should add a start/play button
                montyGameCam.Priority = 12;
                if (montyGameIntro) montyGameIntro.SetActive(true);  //then we need to remove/fade it out to allow play
                animMontyGameIntro.SetTrigger("RaiseCubeGameIntro");
                audioManager.PlayAudio(audioManager.clipapert);
                entryCollider.isTrigger = false; //3/1/23 just turns into a collider that blocks player/robot movement 
            }
        }
    }
    private void PlayTheMontyGame()  //Called by LockPlayerInTheMontyGameTriggerArea() 
    {
        //Here we need to turn off PlayBox and inputcontrols - then enable NextPage

        montyGameState = MontyGameState.MontyGameInProgress;
        switch (montyGameState)
        {
           case MontyGameState.MontyGameInProgress: Debug.Log("In progress  FOUND IN ENUMS ");
           break;
        }
        thirdPersonController.MoveSpeed = 0;  
        thirdPersonController.SprintSpeed = 0;
        thirdPersonController.enabled = false;
        animPlayer.speed = 0;
        if (characterController) characterController.enabled = false;
        if (inputControls) inputControls.SetActive(false);

        if (nowPlay) nowPlay.SetActive(false);
        if (nextPage) nextPage.SetActive(true);
      //  Debug.Log("PlayTheMontyGame() set  montyGameActive = true AND Wait for user action like 'next page' ");
        // m_CanvasNextPagePressedEvent.AddListener(OnCanvasNextPagePressedEvent);   //The Listener get removed every occurence
        waitingForNextPagePressEvent = true;
        montyGameActive = true;   //Now we just wait for a user action like "nextPage"
        montyGameAllowDoorTouch = true;
    }
    public void OnCloudTextExtinguishedEvent()
    {
        if (!waitingForTextExtinguishEvent) return;
        {
            m_CloudTextExtinguishedEvent.RemoveListener(OnCloudTextExtinguishedEvent);  //5/24/23
            waitingForTextExtinguishEvent = false;
        }
    }
    public void OnCanvasNextPagePressed()   //This is the BUTTON
    {
        if (!waitingForNextPagePressEvent) return;

        OnCanvasNextPagePressedEvent();//5/27/23 so let's try this 
        waitingForNextPagePressEvent = false;

    }
    public void OnCanvasNextPagePressedEvent()   // Replaces PlayButtonPressedOnIntro()
    {
        if (montyGameActive)
        {
        if (montyGameIntro) montyGameIntro.SetActive(false);
        if (inputControls) inputControls.SetActive(false);
        if (mainMontySign) mainMontySign.SetActive(true);

        StartCoroutine(ShowDoorsAndBoxesAfterDelay(1f));
        audioManager.PlayAudio(audioManager.clipDRUMROLL);

        if (playerArmature) playerArmature.SetActive(false);
        if (characterController) characterController.enabled = false;
        if (animMontyDoorsAndBoxes) animMontyDoorsAndBoxes.SetTrigger("RaiseAll");
        montyGameActive = false; //5/27/23  //5/28/23 move to afrer  if (montyGameEnded)  //5/26/23 
            if (nextPage) nextPage.SetActive(false);
        }
        else
        if (montyGameEnded)  //5/26/23 
     //    if (montyGameState == MontyGameState.MontyGameOver)  
        {
            if (nextPage) nextPage.SetActive(false);
            montyGameCam.Priority = originalMontyGameCamPriority;
            if (playerArmature)
            {
                Debug.Log("reactivate  player....................... MontyGameState = " + montyGameState);
                if (playerArmature) playerArmature.SetActive(true);  // to enable the Gamepad?
                thirdPersonController.enabled = true;
                CallResetJoystick();
                thirdPersonController.SprintSpeed = originalSprintSpeed;
                animPlayer.speed = originalPlayerSpeed;
                if (characterController) characterController.enabled = true;
            }
            characterController.enabled = true;
            montyGameEnded = twinActivated;     // (evilTwinActivated || goodTwinActivated);  //5/27/23
            StartCoroutine(WaitSecondsThenSwitchCam(1.5f));
        }
    }

    private void CallResetJoystick()
    {
      //  Debug.Log("MontyST CallResetJoystick()    doing SendMessage....");
        if (inputControls) inputControls.SetActive(true);
        thirdPersonController.MoveSpeed = originalMoveSpeed;
        if (inputControls) resetJoystickObject.SendMessage("ResetJoystick");
    }
    IEnumerator ShowDoorsAndBoxesAfterDelay(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        if (montyDoorsAndBoxes) montyDoorsAndBoxes.SetActive(true);
    }
    private void UnlockPlayerFromTheGameTriggerArea()
    {
        if (montyGameBarriers) montyGameBarriers.SetActive(false);// 3/1/23 only deactivate the MontyGameStopRobot collider gO (switched in editor)
        entryCollider.enabled = false;// let the player/robot move forward
    }

    public void OnMontyDoorTouch(int _doorNumber) => ProcessTheDoorButton(_doorNumber); // its not Button anymore //Above 3 UI buttons will be deimped    
                                                                                        //getting door # from ActOnMontyDoorTouch.cs Event 
    private void ProcessTheDoorButton(int doorPressed)
    {
        if (!doorResultsShowing)
        {
            EnableTheDoorResultsMeshRenderers();
        }
        if (awaitingFinalDoorPick)  // //////////////////// This IS the SECOND door pick!! /////////////////////////////////
        {
            ProcessFinalDoorPick(doorPressed);
            montyGameEnded = true;  //5/10/23 moved here out of ProcessFinalDoorPick(doorPressed);
        }

        switch (doorPressed)
        {
            case 1:
                playerPickedDoor1 = true;  //BEWARE these cases are only good for ONE iteration of the monty game! need to reset for/if multiple games
                break;
            case 2:
                playerPickedDoor2 = true;
                break;
            case 3:
                playerPickedDoor3 = true;
                break;
        }

        if (!montyGameEnded)
        {
            audioManager.PlayAudio(audioManager.clipding);
            StartCoroutine(WaitSecondsThenPlayAudioClip(.1f, audioManager.clipdrama));
            montyGameAllowDoorTouch = false;  //disallow door presses until we get animation ended event from AOMDTouch AND audio ended event from AManager
            if (mainMontySign) mainMontySign.SetActive(false);
            StartCoroutine(WaitForEventsToAllowDoorTouches());
            ShowAlternativeDoors();  // HERE MAYBE is where we don't want to allow a door touch until the alt door animation is finished
            return;
        }
        if (playerPickedWinner)
        {
            StartCoroutine(WaitSecondsThenPlayAudioClip(2f, audioManager.clipApplause));
        }
        else
        {
            StartCoroutine(WaitSecondsThenPlayAudioClip(.5f, audioManager.clipfalling));
        }
    }
    void EnableTheDoorResultsMeshRenderers()
    {
        m1 = GameObject.Find("MontyGoal(Clone)").GetComponent<MeshRenderer>();
        if(m1) m1.enabled = true;
        m2 = GameObject.Find("Missed1(Clone)").GetComponent<MeshRenderer>();
        if (m2) m2.enabled = true;
        m3 = GameObject.Find("Missed3(Clone)").GetComponent<MeshRenderer>();
        if (m3) m3.enabled = true;
        doorResultsShowing = true;
    }
    void ProcessFinalDoorPick(int doorPressed)
    {
        bool winnerChosen = false;
        switch (doorPressed)     // //////////////////// This IS the SECOND door pick!! /////////////////////////////////
        {
            case 1:
                animDoor1.SetTrigger("MontyDoor1Down");  //show the Final Door player picked, & set sign to Result
                if (theWinningDoor == 1)
                {
                 //   Debug.Log("Door 1 is a Winner");
                    winnerChosen = true;
                    playerPickedWinner = true;
                    montyGameSignText.text = "Door 1 is a winner!";
                }
                else
                {
                  //  Debug.Log("Door 1 is a Loser");
                    if (m2) m2.enabled = false;
                    if (m3) m3.enabled = false;
                    montyGameSignText.color = Color.red;
                    montyGameSignText.text = "Door 1 releases Evil Twin";
                }
                break;

            case 2:
                animDoor2.SetTrigger("MontyDoor2Down");
                if (theWinningDoor == 2)
                {
                  //  Debug.Log("Door 2 is a Winner");
                    winnerChosen = true;
                    playerPickedWinner = true;
                    montyGameSignText.text = "Door 2 is a winner!";
                }
                else
                {
                 //   Debug.Log("Door 2 is a Loser");
                    if (m2) m2.enabled = false;
                    if (m3) m3.enabled = false;
                    montyGameSignText.color = Color.red;
                    montyGameSignText.text = "Door 2 releases Evil Twin";
                }

                break;
            case 3:
                animDoor3.SetTrigger("MontyDoor3Down");
             //   montyGameEnded = true;
                if (theWinningDoor == 3)
                {
                 //   Debug.Log("Door 3 is a Winner");
                    winnerChosen = true;
                    playerPickedWinner = true;

                    montyGameSignText.text = "Door 3 is a winner!";
                }
                else
                {
                 //   Debug.Log("Door 3 is a Loser");
                    if (m2) m2.enabled = false;
                    if (m3) m3.enabled = false;
                    montyGameSignText.color = Color.red;
                    montyGameSignText.text = "Door 3 releases Evil Twin";
                }
                break;
            default: break;
        }
        if (!winnerChosen)
        {
         //   StartCoroutine(WaitForEventToInstantiateEvilTwin(doorPressed));  //release the evil twin when the door is down(fully)
            StartCoroutine(WaitForEventToActivateTwin(doorPressed, evilTwin,false));  //release the evil twin when the door is down(fully)
        }
        else
        {
         //  StartCoroutine(WaitForEventToInstantiateGoodTwin(doorPressed)); //release the good twin when the door is down(fully)
            StartCoroutine(WaitForEventToActivateTwin(doorPressed, goodTwin, true)); //release the good twin when the door is down(fully)
        }
        CleanUpTheMontyGameAndUnlockThePlayer();
    }
    private void ShowAlternativeDoors()
    {
        theWinningDoor = randomize_array.RandomTest.winningDoor;
        switch (theWinningDoor)
        {
            case 1:  //AND door 1 is the winner
                if (playerPickedDoor1)   //AND door 1 is the winner so show door 2 or 3 
                {
                    //Randomize(2,3); or maybe just right/left?
                    int x = 2;
                    switch (x)
                    {
                        case 2:
                            animDoor2.SetTrigger("MontyDoor2Down");// here we need to really pick 2 or 3 //more later!
                            door2Down = true;
                            break;
                        case 3:
                            animDoor3.SetTrigger("MontyDoor3Down");// here we need to really pick 2 or 3 //more later!
                            door3Down = true;
                            break;
                    }
                    break;
                }
                if (playerPickedDoor2)
                {
                    animDoor3.SetTrigger("MontyDoor3Down"); //now player has to choose between doors 3 & 1
                    door3Down = true;
                    break;
                }
                if (playerPickedDoor3)
                {
                    animDoor2.SetTrigger("MontyDoor2Down");  //no player has to choose between doors 2 & 1 
                    door2Down = true;
                    break;
                }
                break;

            case 2:
                if (playerPickedDoor2)   //AND door 2 is the winner so show door 1 or 3 
                {
                    //Randomize(1,3); or maybe just right/left?
                    int x = 3;
                    switch (x)
                    {
                        case 1:
                            animDoor1.SetTrigger("MontyDoor1Down");// here we need to really pick 1 or 3 //more later!
                            door1Down = true;
                            break;
                        case 3:
                            animDoor3.SetTrigger("MontyDoor3Down");// here we need to really pick 2 or 3 //more later!
                            door3Down = true;
                            break;
                    }
                    break;
                }
                if (playerPickedDoor1)
                {
                    animDoor3.SetTrigger("MontyDoor3Down");
                    door3Down = true;
                    break;
                }
                if (playerPickedDoor3)
                {
                    animDoor1.SetTrigger("MontyDoor1Down");
                    door1Down = true;

                    break;
                }
                break;

            case 3:
                if (playerPickedDoor3)   //AND door 3 is the winner so show door 1 or 2 
                {
                    int x = 2;
                    switch (x)  // x should be randomized 
                    {
                        case 1:
                            animDoor1.SetTrigger("MontyDoor1Down");// here we need to really pick 1 or 2 //more later!
                            door1Down = true;
                            break;
                        case 2:
                            animDoor2.SetTrigger("MontyDoor2Down");// here we need to really pick 1 or 2 //more later!
                            door2Down = true;
                            break;
                    }
                }
                if (playerPickedDoor1)
                {
                    animDoor2.SetTrigger("MontyDoor2Down");
                    door2Down = true;
                    break;
                }
                if (playerPickedDoor2)
                {
                    animDoor1.SetTrigger("MontyDoor1Down");
                    door1Down = true;
                    break;
                }
                break;
        }

        if (door1Down)
        {
         //   Debug.Log("door 1 is down so our pick is 2 or 3");
            doorNumberDown = 1;
        }
        if (door2Down)
        {
         //   Debug.Log("door 2 is down so our pick is 1 or 3");
            doorNumberDown = 2;
        }
        if (door3Down)
        {
          //  Debug.Log("door 3 is down so our pick is 1 or 2");
            doorNumberDown = 3;
        }
        awaitingFinalDoorPick = true;
    }
    public void OnMontyDoorDown(int doorDownReceived) => montyDoorDownEventReceived = true;
    public void OnAudioClipFinished() => montyDramaAudioFinishedEventReceived = true;
    IEnumerator WaitForEventsToAllowDoorTouches()
    {
        montyGameSignText.color = Color.red;  // 5/18/23 added these 3 to show a wait between door selections
        montyGameSignText.text = "Ok, hold on...";
        if (mainMontySign) mainMontySign.SetActive(true);
        yield return new WaitUntil(() => montyDramaAudioFinishedEventReceived && montyDoorDownEventReceived);  //every frame checked??? could be better
        audioManager.PlayAudio(audioManager.clipding);
        montyGameSignText.color = Color.green;
        montyGameSignText.text = "A chance to change door and unleash your Good or Evil Twin. Choose...";
        if (mainMontySign) mainMontySign.SetActive(true);
        montyGameAllowDoorTouch = true; //re-allow door touches 
        montyDoorDownEventReceived = false;
    }

    IEnumerator WaitForEventToActivateTwin(int outOfDoor, GameObject twinNpc, bool goodTwinActivating)
    {
        float twinRotate = 0f;
        float agent1OriginalSpeed;
        float anim1OriginalSpeed;
        NavMeshAgent agent1;
        Animator anim1;
        camOnTwin.transform.SetParent(twinNpc.transform);  // NEW Cam consolidation
        camOnTwin.LookAt = twinNpc.transform;              // NEW Cam consolidation

        switch (outOfDoor)
        {
            case 1:
                twinNpc.transform.position = new Vector3(xPos, 0, -228);
                break;
            case 2:
                twinNpc.transform.position = new Vector3(xPos, 0, -221);
                break;
            case 3:
                twinNpc.transform.position = new Vector3(xPos, 0, -214);
                break;
            default:
                break;
        }
        twinNpc.transform.Rotate(0f, twinRotate, 0f, Space.Self);  //rotation depends on the door - thank U 3D  //moved out of switch case 4/25/23
        twinNpc.SetActive(true);  //we're gonna use this and the following so KEEP
        // for now just have the twin pace back and forth...
        if (goodTwinActivating)
        {
            agent1 = GameObject.Find("PlayerCloneGoodTwin").GetComponent<NavMeshAgent>();
            anim1 = GameObject.Find("PlayerCloneGoodTwin").GetComponent<Animator>();
        }
        else
        {
            agent1 = GameObject.Find("PlayerCloneEvilTwin").GetComponent<NavMeshAgent>();
            anim1 = GameObject.Find("PlayerCloneEvilTwin").GetComponent<Animator>();
        }
        agent1OriginalSpeed = agent1.speed;
        anim1OriginalSpeed = anim1.speed;
        agent1.speed = 0;
        GameObject montyGoal = GameObject.Find("MontyGoal(Clone)");
        if (montyGoal) montyGoal.SetActive(false);
        anim1.speed = 0;

        camOnTwin.Priority = 13; //or maybe b4
        StartCoroutine(ZoomTwinCam());
        yield return new WaitUntil(() => montyDoorDownEventReceived);  //every frame checked??? could be better
        yield return new WaitForSeconds(2f);

        if (goodTwinActivating)    //the true parameter causes TTC to set nextPage active
        {
            TellTextCloud(goodTwinSpeaks1, true);  //5/26/23 now wait for nextPage press which TextCloudHandler.cs will raise 
        }
        else
        {
            TellTextCloud(evilTwinSpeaks1, true);  //5/26/23 now wait for nextPage press which TextCloudHandler.cs will raise 
        }
        //waitingForNextPagePressEvent = true;   //6/2/23 moved these 3 lines to TellTextCloud (..., true)
        //waitingForTextExtinguishEvent = true;
        //m_CloudTextExtinguishedEvent.AddListener(OnCloudTextExtinguishedEvent);
        yield return new WaitUntil(() => !waitingForTextExtinguishEvent);

        //Debug.Log("WE SEE !waitingForTextExtinguishEvent, SET Twin in Motion !!! montyGameActive = " + montyGameActive +
        //    "  montyGameEnded = " + montyGameEnded);

        if (goodTwinActivating)    //the true parameter causes TTC to set nextPage active
        {
            TellTextCloud(playerSpeaksToGoodTwin1, true);  //5/26/23 now wait for nextPage press which TextCloudHandler.cs will raise 
        }
        else
        {
            TellTextCloud(playerSpeakstoEvilTwin1, true);  //5/26/23 now wait for nextPage press which TextCloudHandler.cs will raise 
        }
        camOnPlayer.Priority = 13;
        montyGameState = MontyGameState.MontyGameInDialogue;
        yield return new WaitUntil(() => !waitingForTextExtinguishEvent);
        camOnPlayer.Priority = originalCamOnlayerPriority;

        montyGameState = MontyGameState.MontyGameOver;
        
        if (mainMontySign) mainMontySign.SetActive(false);
        if (montyDoorsAndBoxes) montyDoorsAndBoxes.SetActive(false);

        GameObject montyGameBarriers = GameObject.Find("MontyGameBarriers");
        if (montyGameBarriers) montyGameBarriers.SetActive(false);
        GameObject missed1 = GameObject.Find("Missed1(Clone)");
        if (missed1) missed1.SetActive(false);
        GameObject missed3 = GameObject.Find("Missed3(Clone)");
        if (missed3) missed3.SetActive(false);

        agent1.speed = agent1OriginalSpeed;
        anim1.speed = anim1OriginalSpeed;
        //camOnTwin.Priority = originalCamOnTwinPriority;  // disable the camOnTwin and revert to follow cam
       // StartCoroutine(WaitSecondsThenSwitchCam(1.5f));  //let user see the twin start walking
        twinActivated = true;
    }
    IEnumerator WaitSecondsThenSwitchCam(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        camOnTwin.Priority = originalCamOnTwinPriority;  // disable the camOnTwin and revert to follow cam
    }
    private IEnumerator ZoomTwinCam()  //added 5/22/23 to consolidate Good and Evil Twin cam zoom ops
    {
        var originalFOV = camOnTwin.m_Lens.FieldOfView;
        var targetFOV = originalFOV - zoomAmount;  //40 - 23 = 17 //zoomTime and zoomAmount are public in editor so we can adjust
        float timer = 0f;

        while (timer < zoomTime)
        {
            timer += Time.deltaTime;
            camOnTwin.m_Lens.FieldOfView = Mathf.Lerp(originalFOV, targetFOV, timer / zoomTime);
            //  Debug.Log("ori/targ  " + originalFOV + "/" + targetFOV + "  camFOV = " + camOnGoodTwin.m_Lens.FieldOfView);
            yield return null;
        }
    }
    void TellTextCloud(string caption)
    {
        m_CloudTextEvent.Invoke(5, 4, caption);
    }
    void TellTextCloud(string caption, bool waitForNextPagePressed)
    {
        m_CloudTextEvent.Invoke(6, 4, caption);
        waitingForNextPagePressEvent = true;
        waitingForTextExtinguishEvent = true;
        m_CloudTextExtinguishedEvent.AddListener(OnCloudTextExtinguishedEvent);
    }
    private void CloseTheFirstOpenedDoor()  //do we really want to do this?
    {
        switch (doorNumberDown)
        {
            case 1:
                animDoor1.SetTrigger("MontyDoor1Up");
                break;
            case 2:
                animDoor2.SetTrigger("MontyDoor2Up");
                break;
            case 3:
                animDoor3.SetTrigger("MontyDoor3Up");
                break;
        }
    }
    private void CleanUpTheMontyGameAndUnlockThePlayer()
    {
    //    Debug.Log("MSTCleanUpTheMontyGameAndUnlockThePlayer() Called montyGameActive = " + montyGameActive);
        CloseTheFirstOpenedDoor();
        UnlockPlayerFromTheGameTriggerArea();
    }
    IEnumerator WaitSecondsThenPlayAudioClip(float timeToWait, AudioClip audioClip)
    {
        yield return new WaitForSeconds(timeToWait);
        audioManager.PlayAudio(audioClip);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        thirdPersonController.MoveSpeed = originalMoveSpeed;   //just to be sure?
        thirdPersonController.SprintSpeed = originalSprintSpeed;
    }
}  //end class 
   //IEnumerator WaitForEventToInstantiateGoodTwin(int outOfDoor)
   //{
   //   // goodTwinActivated = true;
   //    float gTwinRot = 0f;

//    camOnTwin.transform.SetParent(goodTwin.transform);  // NEW Cam consolidation
//    camOnTwin.LookAt = goodTwin.transform;              // NEW Cam consolidation

//    switch (outOfDoor)
//    {
//        case 1:
//            goodTwin.transform.position = new Vector3(xPos, 0, -228);
//         //    goodTwin.transform.Rotate(0f, gTwinRot, 0f, Space.Self);  //rotation depends on the door - thank U 3D
//            break;
//        case 2:
//            goodTwin.transform.position = new Vector3(xPos, 0, -221);
//         //   goodTwin.transform.Rotate(0f, gTwinRot, 0f, Space.Self);  //rotation depends on the door - thank U 3D
//            break;
//        case 3:
//            goodTwin.transform.position = new Vector3(xPos, 0, -214);
//          //  goodTwin.transform.Rotate(0f, gTwinRot, 0f, Space.Self);  //rotation depends on the door - thank U 3D
//            break;
//        default:
//            break;
//    }
//    goodTwin.transform.Rotate(0f, gTwinRot, 0f, Space.Self);  //rotation depends on the door - thank U 3D  //moved out of switch case 4/25/23
//    goodTwin.SetActive(true);  //we're gonna use this and the following so KEEP
//    // for now just have the twin pace back and forth...
//    NavMeshAgent agent1 = GameObject.Find("PlayerCloneGoodTwin").GetComponent<NavMeshAgent>();
//    Animator anim1 = GameObject.Find("PlayerCloneGoodTwin").GetComponent<Animator>();
//    float agent1OriginalSpeed = agent1.speed;
//    float anim1OriginalSpeed = anim1.speed;
//    agent1.speed = 0;
//    GameObject montyGoal = GameObject.Find("MontyGoal(Clone)");
//    if (montyGoal) montyGoal.SetActive(false);
//    anim1.speed = 0;

//    camOnTwin.Priority = 13; //or maybe b4
//    StartCoroutine(ZoomTwinCam());
//    yield return new WaitUntil(() => montyDoorDownEventReceived);  //every frame checked??? could be better
//    yield return new WaitForSeconds(2f);

//    TellTextCloud(goodTwinSpeaks1, true);  //5/26/23 now wait for nextPage press which TextCloudHandler.cs will raise 
//    waitingForTextExtinguishEvent = true;
//    m_CloudTextExtinguishedEvent.AddListener(OnCloudTextExtinguishedEvent);
//    yield return new WaitUntil(() => !waitingForTextExtinguishEvent);
//    Debug.Log("WE SEE !waitingForTextExtinguishEvent, SET Twin in Motion !!! montyGameActive = " + montyGameActive +
//        "  montyGameEnded = " + montyGameEnded);
//    if (mainMontySign) mainMontySign.SetActive(false);
//    if (montyDoorsAndBoxes) montyDoorsAndBoxes.SetActive(false);

//    GameObject montyGameBarriers = GameObject.Find("MontyGameBarriers");
//    if (montyGameBarriers) montyGameBarriers.SetActive(false);
//    GameObject missed1 = GameObject.Find("Missed1(Clone)");
//    if (missed1) missed1.SetActive(false);
//    GameObject missed3 = GameObject.Find("Missed3(Clone)");
//    if (missed3) missed3.SetActive(false);

//    agent1.speed = agent1OriginalSpeed;
//    anim1.speed = anim1OriginalSpeed;
//    camOnTwin.Priority = originalCamOnTwinPriority;  // disable the camOnTwin and revert to follow cam
// //   goodTwinActivated = true;
//}


//IEnumerator WaitForEventToInstantiateEvilTwin(int outOfDoor)
//{
//    // evilTwinActivated = true;
//    float eTwinRot = 0f;

//    camOnTwin.transform.SetParent(evilTwin.transform);  // NEW Cam consolidation
//    camOnTwin.LookAt = evilTwin.transform;              // NEW Cam consolidation

//    switch (outOfDoor)
//    {
//        case 1:
//            evilTwin.transform.position = new Vector3(xPos, 0, -228);
//            // evilTwin.transform.Rotate(0f, eTwinRot, 0f, Space.Self);  //rotation depends on the door - thank U 3D
//            break;
//        case 2:
//            evilTwin.transform.position = new Vector3(xPos, 0, -221);
//            // evilTwin.transform.Rotate(0f, eTwinRot, 0f, Space.Self);  //rotation depends on the door - thank U 3D
//            break;
//        case 3:
//            evilTwin.transform.position = new Vector3(xPos, 0, -214);
//            // evilTwin.transform.Rotate(0f, eTwinRot, 0f, Space.Self);  //rotation depends on the door - thank U 3D
//            break;
//        default:
//            break;
//    }
//    evilTwin.transform.Rotate(0f, eTwinRot, 0f, Space.Self);  //rotation depends on the door - thank U 3D  //moved out of switch case 4/25/23
//    evilTwin.SetActive(true);
//    // for now just have the twin pace back and forth...
//    NavMeshAgent agent1 = GameObject.Find("PlayerCloneEvilTwin").GetComponent<NavMeshAgent>();
//    Animator anim1 = GameObject.Find("PlayerCloneEvilTwin").GetComponent<Animator>();
//    float agent1OriginalSpeed = agent1.speed;
//    float anim1OriginalSpeed = anim1.speed;
//    agent1.speed = 0;
//    anim1.speed = 0;

//    camOnTwin.Priority = 13; //or maybe b4
//    StartCoroutine(ZoomTwinCam());
//    yield return new WaitUntil(() => montyDoorDownEventReceived);  //every frame checked??? could be better
//    yield return new WaitForSeconds(2f);

//    TellTextCloud(evilTwinSpeaks1, true);  //5/26/23 now wait for nextPage press which TextCloudHandler.cs will raise 
//    waitingForTextExtinguishEvent = true;
//    m_CloudTextExtinguishedEvent.AddListener(OnCloudTextExtinguishedEvent);
//    yield return new WaitUntil(() => !waitingForTextExtinguishEvent);
//    Debug.Log("WE SEE !waitingForTextExtinguishEvent, SET EVIL Twin in Motion !!! montyGameActive = " + montyGameActive +
//"  montyGameEnded = " + montyGameEnded);
//    if (mainMontySign) mainMontySign.SetActive(false);
//    if (montyDoorsAndBoxes) montyDoorsAndBoxes.SetActive(false);

//    GameObject montyGameBarriers = GameObject.Find("MontyGameBarriers");
//    if (montyGameBarriers) montyGameBarriers.SetActive(false);
//    GameObject missed1 = GameObject.Find("Missed1(Clone)");
//    if (missed1) missed1.SetActive(false);
//    GameObject missed3 = GameObject.Find("Missed3(Clone)");
//    if (missed3) missed3.SetActive(false);
//    GameObject montyGoal = GameObject.Find("MontyGoal(Clone)");
//    if (montyGoal) montyGoal.SetActive(false);

//    agent1.speed = agent1OriginalSpeed;
//    anim1.speed = anim1OriginalSpeed;
//    camOnTwin.Priority = originalCamOnTwinPriority;  // disable the camOnTwin and revert to follow cam
//                                                     //  evilTwinActivated = true;

//}