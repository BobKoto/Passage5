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

[System.Serializable]
public class MontyDoorTouchEvent : UnityEvent<int> { }
[System.Serializable]
public class MontyPlayButtonTouchEvent : UnityEvent { }
[System.Serializable]
public class MontyMoveOnButtonTouchEvent : UnityEvent { }
[System.Serializable]
public class MontyDoorDownEvent : UnityEvent<int> { }
[System.Serializable]
public class AudioClipFinishedEvent : UnityEvent { }

public class MontyStopTrigger : MonoBehaviour
{  //Component of MontyStopTrigger
    MovingPlatform movingPlatform;
    [Header("UI Buttons")]
    public GameObject stopButton;
    public GameObject goButton;
    public GameObject door1Button;  // Door buttons to be deimped 
    public GameObject door2Button;
    public GameObject door3Button;

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

    [Header("Monty Game Move On Button")]
    public GameObject montyGameMoveOnButton;

    [Header("The Player")]
    public GameObject playerArmature;

    public GameObject evilTwin;
    public GameObject goodTwin;

    public float timeToPauseOnAnyTwin = 4f;

    [Header("Cinemachine Cameras")]
    public CinemachineVirtualCamera thirdPersonFollowCam;
    public CinemachineFreeLook freeLookCam;
    public CinemachineVirtualCamera montyGameCam;
    public CinemachineVirtualCamera camOnEvilTwin;
    public CinemachineVirtualCamera camOnGoodTwin;

    public float zoomAmount = 23f;
    public float zoomTime = 2.5f;

    [Header("Animations")]
    Animation animClipMontyDoorsAndBoxes;
    public Animation raiseMontyDoorsAndBoxes;
    public Animation animDoor1Down;
    public Animation animDoor2Down;
    public Animation animDoor3Down;
    public float xPos = 200;
    int originalMontyGameCamPriority, originalCamOnEvilTwinPriority, originalCamOnGoodTwinPriority;

   // int thirdPersonFollowCamOriginalPriority, freeLookCamOriginalPriority;

    [Header("The Input System canvas Joystick etc.")]
    public GameObject inputControls;

    bool playerPickedDoor1, playerPickedDoor2, playerPickedDoor3, door1Down, door2Down, door3Down;
    bool awaitingFinalDoorPick, playerPickedWinner, doorResultsShowing;

    public static bool montyGameEnded;
    int doorNumberDown, theWinningDoor;

    ThirdPersonController thirdPersonController;
    CharacterController characterController;
    float originalMoveSpeed, originalSprintSpeed;

    public PlayerEnteredRelevantTrigger triggerEvent;

    public MontyDoorTouchEvent montyDoorTouchEvent;
    public MontyPlayButtonTouchEvent montyPlayButtonTouchEvent;
    public MontyMoveOnButtonTouchEvent montyMoveOnButtonTouchEvent;
    public MontyDoorDownEvent montyDoorDownEvent;
    public AudioClipFinishedEvent audioClipFinishedEvent;
    public CloudTextEvent m_MyEvent;  //for TextCloud 

    Animator animDoor1, animDoor2, animDoor3, animMontyDoorsAndBoxes, animMontyGameIntro;
    AudioManager audioManager;
    public static bool montyGameActive;
    bool montyDoorDownEventReceived, montyDramaAudioFinishedEventReceived;

    public GameObject resetJoystickObject;  //Maybe defunct 2/27/23 and before 
    BoxCollider entryCollider;
    MeshRenderer m1, m2, m3;

    const string evilTwinSpeaks1 = "Hello, Hashnag, you mechanical jerk. Prepare yourself!";
    const string goodTwinSpeaks1 = "Hello, Hashy, I'll be looking out for you...";

    void Start()
    {
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        movingPlatform = GameObject.Find("MovingPlatform").GetComponent<MovingPlatform>();
        animDoor1 = montyDoor1.GetComponent<Animator>();
        animDoor2 = montyDoor2.GetComponent<Animator>();
        animDoor3 = montyDoor3.GetComponent<Animator>();
        animMontyDoorsAndBoxes = montyDoorsAndBoxes.GetComponent<Animator>();
        animClipMontyDoorsAndBoxes = montyDoorsAndBoxes.GetComponent<Animation>();  //3/4/23 not imp'd??
        animMontyGameIntro = montyGameIntro.GetComponent<Animator>();
        // Debug.Log("")

        //animPlayer = playerArmature.GetComponent<Animator>();

        thirdPersonController = playerArmature.GetComponent<ThirdPersonController>();
        characterController = playerArmature.GetComponent<CharacterController>();
        originalMoveSpeed = thirdPersonController.MoveSpeed;
        originalSprintSpeed = thirdPersonController.SprintSpeed;
        if (triggerEvent == null)
            triggerEvent = new PlayerEnteredRelevantTrigger();

        if (montyDoorTouchEvent == null)
            montyDoorTouchEvent = new MontyDoorTouchEvent();
        montyDoorTouchEvent.AddListener(OnMontyDoorTouch);

        if (montyPlayButtonTouchEvent == null)
            montyPlayButtonTouchEvent = new MontyPlayButtonTouchEvent();
        montyPlayButtonTouchEvent.AddListener(PlayButtonPressedOnIntro);

        if (montyMoveOnButtonTouchEvent == null)
            montyMoveOnButtonTouchEvent = new MontyMoveOnButtonTouchEvent();
        montyMoveOnButtonTouchEvent.AddListener(MoveOnButtonPressed);

        if (montyDoorDownEvent == null)
            montyDoorDownEvent = new MontyDoorDownEvent();
        montyDoorDownEvent.AddListener(OnMontyDoorDown);

        if (audioClipFinishedEvent == null)
            audioClipFinishedEvent = new AudioClipFinishedEvent();
        audioClipFinishedEvent.AddListener(OnAudioClipFinished);

        originalMontyGameCamPriority = montyGameCam.Priority; //10
        originalCamOnEvilTwinPriority = camOnEvilTwin.Priority;  //10
        entryCollider = gameObject.GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("MovingPlatform"))
        {
            //  Debug.Log(other.gameObject.name + " Entered montyPStop.. ");
            if (!montyGameEnded)
            {
                // triggerEvent.Invoke(1);   //send event to ThirdPersonController to rotate the player and camera  //2/10/23 Deimp, go straight  
                LockPlayerInTheMontyGameTriggerArea();
                //  PlayTheMontyGame();  //Only sets montyGameActive true - which causes ActOnMontyDoorTouch to accept door touches
                montyGameCam.Priority = 12;
                if (montyGameIntro) montyGameIntro.SetActive(true);  //then we need to remove/fade it out to allow play
                animMontyGameIntro.SetTrigger("RaiseCubeGameIntro");
                audioManager.PlayAudio(audioManager.clipapert);
                entryCollider.isTrigger = false; //3/1/23 just turns into a collider that blocks player/robot movement 
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("MovingPlatform"))
        {
            //   Debug.Log(other.gameObject.name + " Exited montyPlay(STOP)Trigger... from MontyStopTrigger");
            DisableTheDoorButtons();
            // montyGameCam.Priority = originalCamPriority;
        }
    }
    public void PlayButtonPressedOnIntro()
    {
        //Debug.Log("IntroPlay Button Pressed - setting Intro Panel Active to false");
       // CallResetJoystick(); //this will throw a no receiver error if inputControls are active(false)
        if (montyGameIntro) montyGameIntro.SetActive(false);
        if (inputControls) inputControls.SetActive(false);
        if (mainMontySign) mainMontySign.SetActive(true);

        StartCoroutine(ShowDoorsAndBoxesAfterDelay(1f));
        //if (montyDoorsAndBoxes) montyDoorsAndBoxes.SetActive(true);
        audioManager.PlayAudio(audioManager.clipDRUMROLL);

        if (playerArmature) playerArmature.SetActive(false);   
        if (characterController) characterController.enabled = false;
        if (animMontyDoorsAndBoxes) animMontyDoorsAndBoxes.SetTrigger("RaiseAll");

    }
    private void CallResetJoystick()
    {
        Debug.Log("MontyST doing SendMessage....");
        if (inputControls) resetJoystickObject.SendMessage("ResetJoystick");
        if (playerArmature) playerArmature.SetActive(false);
    }
    IEnumerator ShowDoorsAndBoxesAfterDelay(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        if (montyDoorsAndBoxes) montyDoorsAndBoxes.SetActive(true);
    }
    public void MoveOnButtonPressed()
    {
        Debug.Log("MoveOn Button Pressed - wiping out all");
        if (montyGameMoveOnButton) montyGameMoveOnButton.SetActive(false);
        montyGameCam.Priority = originalMontyGameCamPriority;
        //if (mainMontySign) mainMontySign.SetActive(false);
        //if (montyDoorsAndBoxes) montyDoorsAndBoxes.SetActive(false);
        //if (inputControls) inputControls.SetActive(true);
        //GameObject missed1 = GameObject.Find("Missed1(Clone)");
        //if (missed1) missed1.SetActive(false);
        //GameObject missed3 = GameObject.Find("Missed3(Clone)");
        //if (missed3) missed3.SetActive(false);
        //GameObject montyGoal = GameObject.Find("MontyGoal(Clone)");
        //if (montyGoal) montyGoal.SetActive(false);
       // CallResetJoystick();
        if (playerArmature)
        {
            Debug.Log("reactivate  player.......................");
            playerArmature.SetActive(true); // =  Instantiate(playerArmature, playerPosition, Quaternion.identity, playerParent);
        }
        characterController.enabled = true;
    }
    private void LockPlayerInTheMontyGameTriggerArea()
    {
        movingPlatform.speed = 0;
        if (stopButton) stopButton.SetActive(false); //Part of locking the player in the game trigger area
        if (goButton) goButton.SetActive(false);
        PlayTheMontyGame();  //so let's enable the door touches here - i bet no diff  //OK so we should add a start/play button
    }
    private void UnlockPlayerFromTheGameTriggerArea()
    {
        if (AddRemoveChild.playerIsOnYellowPlatform)  // another useful public static bool
        {
            if (stopButton) stopButton.SetActive(false); //Part of locking the player in the game trigger area
            if (goButton) goButton.SetActive(true);
        }
        if (montyGameBarriers) montyGameBarriers.SetActive(false);// 3/1/23 only deactivate the MontyGameStopRobot collider gO (switched in editor)
        entryCollider.enabled = false;// let the player/robot move forward
        // if (montyDoorsAndBoxes) montyDoorsAndBoxes.SetActive(false);
    }
    private void PlayTheMontyGame()
    {
        Debug.Log("PlayTheMontyGame() set  montyGameActive = true ");
        montyGameActive = true;
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
        }

        DisableTheDoorButtons();  //deimped but hold because we may want to do something here like wait on the door animation
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
        Debug.Log("player picked door " + doorPressed);

        if (!montyGameEnded)
        {
            audioManager.PlayAudio(audioManager.clipding);
            StartCoroutine(WaitSeconds(.1f, audioManager.clipdrama));
            //  montyGameSignText.text = "A chance to change door pick...";         
            montyGameActive = false;  //disallow door presses until we get animation ended event from AOMDTouch AND audio ended event from AManager
            if (mainMontySign) mainMontySign.SetActive(false);
            StartCoroutine(WaitForEventsToAllowDoorTouches());
            ShowAlternativeDoors();  // HERE MAYBE is where we don't want to allow a door touch until the alt door animation is finished
            return;
        }
        // (was an else) the Monty Game IS Ended
        if (playerPickedWinner)
        {
            StartCoroutine(WaitSeconds(2f, audioManager.clipApplause));
        }
        else
        {
            StartCoroutine(WaitSeconds(.5f, audioManager.clipfalling));
        }
        // montyGameCam.Priority = originalCamPriority;  //this is premature and should be done upon complete eviction of monty game
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
                montyGameEnded = true;
                if (theWinningDoor == 1)
                {
                    Debug.Log("Door 1 is a Winner");
                    winnerChosen = true;
                    playerPickedWinner = true;
                    montyGameSignText.text = "Door 1 is a winner!";
                }
                else
                {
                    Debug.Log("Door 1 is a Loser");
                    if (m2) m2.enabled = false;
                    if (m3) m3.enabled = false;
                    montyGameSignText.color = Color.red;
                    montyGameSignText.text = "Door 1 releases Evil Twin";

                   //Instantiate(evilTwin, new Vector3(237, 0, -228), Quaternion.Euler(0, 52, 0));
                   // StartCoroutine(WaitForEventToInstantiateEvilTwin(doorPressed));  //release the evile twin when the door is down(fully)
                }
                break;

            case 2:
                animDoor2.SetTrigger("MontyDoor2Down");
                montyGameEnded = true;
                if (theWinningDoor == 2)
                {
                    Debug.Log("Door 2 is a Winner");
                    winnerChosen = true;
                    playerPickedWinner = true;
                    montyGameSignText.text = "Door 2 is a winner!";
                }
                else
                {
                    Debug.Log("Door 2 is a Loser");
                    if (m2) m2.enabled = false;
                    if (m3) m3.enabled = false;
                    montyGameSignText.color = Color.red;
                    montyGameSignText.text = "Door 2 releases Evil Twin";
                   //Instantiate(evilTwin, new Vector3(237, 0, -221), Quaternion.Euler(0, 52, 0));
                   // StartCoroutine(WaitForEventToInstantiateEvilTwin(doorPressed));  //release the evile twin when the door is down(fully)
                }

                break;
            case 3:
                animDoor3.SetTrigger("MontyDoor3Down");
                montyGameEnded = true;
                if (theWinningDoor == 3)
                {
                    Debug.Log("Door 3 is a Winner");
                    winnerChosen = true;
                    playerPickedWinner = true;

                    montyGameSignText.text = "Door 3 is a winner!";
                }
                else
                {
                    Debug.Log("Door 3 is a Loser");
                    if (m2) m2.enabled = false;
                    if (m3) m3.enabled = false;
                    montyGameSignText.color = Color.red;
                    montyGameSignText.text = "Door 3 releases Evil Twin";
                   //  Instantiate(evilTwin, new Vector3(237, 0, -214), Quaternion.Euler(0, 52, 0));
                   // StartCoroutine(WaitForEventToInstantiateEvilTwin(doorPressed));  //release the evile twin when the door is down(fully)
                }
                break;
            default: break;
        }
        if (!winnerChosen)
        {
            StartCoroutine(WaitForEventToInstantiateEvilTwin(doorPressed));  //release the evil twin when the door is down(fully)
        }
        else
            StartCoroutine(WaitForEventToInstantiateGoodTwin(doorPressed)); //release the good twin when the door is down(fully)

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
            Debug.Log("door 1 is down so our pick is 2 or 3");
            doorNumberDown = 1;
        }
        if (door2Down)
        {
            Debug.Log("door 2 is down so our pick is 1 or 3");
            doorNumberDown = 2;
        }
        if (door3Down)
        {
            Debug.Log("door 3 is down so our pick is 1 or 2");
            doorNumberDown = 3;
        }
        awaitingFinalDoorPick = true;
    }
    public void OnMontyDoorDown(int doorDownReceived)
    {
        Debug.Log("MontyDoorDownEvent  says  doorDown is door " + doorDownReceived + "montyDoorDownEventReceived = true;");//" reset montyGameActive to TRUE");
       // montyGameActive = true;  //ActonMontyTouch will see this and allow door touches to pass thru
        montyDoorDownEventReceived = true;
    }
    public void OnAudioClipFinished()
    {
        Debug.Log("AudioClipFinishedEvent  says  audio is finished ...montyDramaAudioFinishedEventReceived = true");
        montyDramaAudioFinishedEventReceived = true;
    }
    IEnumerator WaitForEventsToAllowDoorTouches()
    {
        //
        yield return new WaitUntil(() => montyDramaAudioFinishedEventReceived && montyDoorDownEventReceived);  //every frame checked??? could be better
        Debug.Log("WE GOT BOTH EVENTS !!!!!!!!!!!!!!!!!!!!");
        audioManager.PlayAudio(audioManager.clipding);
        montyGameSignText.color = Color.green;
        montyGameSignText.text = "A chance to change door. Choose...";
        if (mainMontySign) mainMontySign.SetActive(true);
        montyGameActive = true; //re-allow door touches 
        montyDoorDownEventReceived = false;
    }
    IEnumerator WaitForEventToInstantiateGoodTwin(int outOfDoor)
    {
        // MUCH of this routine is commented UNTIL we create a GOOD TWIN
        Debug.Log("WE GOT EVENT to Instatiate/Enable the Good Twin!!!!!!! from door " + outOfDoor);
        float gTwinRot = 0f;
        switch (outOfDoor)
        {
            case 1:
                goodTwin.transform.position = new Vector3(xPos, 0, -228);
                goodTwin.transform.Rotate(0f, gTwinRot, 0f, Space.Self);  //rotation depends on the door - thank U 3D
                break;
            case 2:
                goodTwin.transform.position = new Vector3(xPos, 0, -221);
                goodTwin.transform.Rotate(0f, gTwinRot, 0f, Space.Self);  //rotation depends on the door - thank U 3D
                break;
            case 3:
                goodTwin.transform.position = new Vector3(xPos, 0, -214);
                goodTwin.transform.Rotate(0f, gTwinRot, 0f, Space.Self);  //rotation depends on the door - thank U 3D
                break;
            default:
                break;
        }

        goodTwin.SetActive(true);  //we're gonna use this and the following so KEEP

        NavMeshAgent agent1 = GameObject.Find("PlayerCloneGoodTwin").GetComponent<NavMeshAgent>();
        Animator anim1 = GameObject.Find("PlayerCloneGoodTwin").GetComponent<Animator>();
        float agent1OriginalSpeed = agent1.speed;
        float anim1OriginalSpeed = anim1.speed;
        agent1.speed = 0;
              GameObject montyGoal = GameObject.Find("MontyGoal(Clone)");
        if (montyGoal) montyGoal.SetActive(false);  anim1.speed = 0;
        camOnGoodTwin.Priority = 13; //or maybe b4
        StartCoroutine(ZoomGoodTwinCam());  //may be able to consolidate 
        yield return new WaitUntil(() => montyDoorDownEventReceived);  //every frame checked??? could be better
        yield return new WaitForSeconds(2f);
        // camOnEvilTwin.Priority = 13; //put this cam on AFTER the door is down
        TellTextCloud(goodTwinSpeaks1);
        yield return new WaitForSeconds(timeToPauseOnAnyTwin);

        // montyGameCam.Priority = originalMontyGameCamPriority;
        if (mainMontySign) mainMontySign.SetActive(false);
        if (montyDoorsAndBoxes) montyDoorsAndBoxes.SetActive(false);
        if (inputControls) inputControls.SetActive(true);
        GameObject montyGameBarriers = GameObject.Find("MontyGameBarriers");
        if (montyGameBarriers) montyGameBarriers.SetActive(false);
        GameObject missed1 = GameObject.Find("Missed1(Clone)");
        if (missed1) missed1.SetActive(false);
        GameObject missed3 = GameObject.Find("Missed3(Clone)");
        if (missed3) missed3.SetActive(false);


        agent1.speed = agent1OriginalSpeed;
        anim1.speed = anim1OriginalSpeed;
        camOnGoodTwin.Priority = originalCamOnGoodTwinPriority;  // disable the camOnGoodTwin and revert to follow cam
    }
    IEnumerator WaitForEventToInstantiateEvilTwin(int outOfDoor)
    {
        //
        Debug.Log("WE GOT EVENT to Instatiate/Enable the Evil Twin!!!!!!! from door " + outOfDoor);
        float eTwinRot = 0f;
        switch (outOfDoor)
        {
            case 1:
                evilTwin.transform.position = new Vector3(xPos, 0, -228);
                evilTwin.transform.Rotate(0f, eTwinRot, 0f, Space.Self);  //rotation depends on the door - thank U 3D
                //evilTwin.transform.Rotate(0f, eTwinRot, 0f, Space.Self);
                //evilTwin.SetActive(true);
                //NavMeshAgent agent1 = GameObject.Find("PlayerCloneEvilTwin").GetComponent<NavMeshAgent>();
                //Animator anim1 = GameObject.Find("PlayerCloneEvilTwin").GetComponent<Animator>();
                //float agent1OriginalSpeed = agent1.speed;
                //float anim1OriginalSpeed = anim1.speed;
                //agent1.speed = 0;
                //anim1.speed = 0;
                //yield return new WaitForSeconds(timeToPauseOnTheEvilTwin);
                //agent1.speed = agent1OriginalSpeed;
                //anim1.speed = anim1OriginalSpeed;
                break;
            case 2:
                evilTwin.transform.position = new Vector3(xPos, 0, -221);
                evilTwin.transform.Rotate(0f, eTwinRot, 0f, Space.Self);  //rotation depends on the door - thank U 3D
                //evilTwin.transform.Rotate(0f, eTwinRot, 0f, Space.Self);//was 52
                //evilTwin.SetActive(true);
                //NavMeshAgent agent2 = GameObject.Find("PlayerCloneEvilTwin").GetComponent<NavMeshAgent>();
                //Animator anim2 = GameObject.Find("PlayerCloneEvilTwin").GetComponent<Animator>();
                //float agent2OriginalSpeed = agent2.speed;
                //float anim2OriginalSpeed = anim2.speed;
                //agent2.speed = 0;
                //anim2.speed = 0;
                //yield return new WaitForSeconds(timeToPauseOnTheEvilTwin);
                //agent2.speed = agent2OriginalSpeed;
                //anim2.speed = anim2OriginalSpeed;
                break;
            case 3:
                evilTwin.transform.position = new Vector3(xPos, 0, -214);
                evilTwin.transform.Rotate(0f, eTwinRot, 0f, Space.Self);  //rotation depends on the door - thank U 3D
                //evilTwin.transform.Rotate(0f, eTwinRot, 0f, Space.Self);
                //evilTwin.SetActive(true);
                //NavMeshAgent agent3 = GameObject.Find("PlayerCloneEvilTwin").GetComponent<NavMeshAgent>();
                //Animator anim3 = GameObject.Find("PlayerCloneEvilTwin").GetComponent<Animator>();
                //float agent3OriginalSpeed = agent3.speed;
                //float anim3OriginalSpeed = anim3.speed;
                //agent3.speed = 0;
                //anim3.speed = 0;
                //yield return new WaitForSeconds(timeToPauseOnTheEvilTwin);
                //agent3.speed = agent3OriginalSpeed;
                //anim3.speed = anim3OriginalSpeed;
                break;
            default:
                break;
        }

        evilTwin.SetActive(true);

        NavMeshAgent agent1 = GameObject.Find("PlayerCloneEvilTwin").GetComponent<NavMeshAgent>();
        Animator anim1 = GameObject.Find("PlayerCloneEvilTwin").GetComponent<Animator>();
        float agent1OriginalSpeed = agent1.speed;
        float anim1OriginalSpeed = anim1.speed;
        agent1.speed = 0;
        anim1.speed = 0;
        camOnEvilTwin.Priority = 13; //or maybe b4
        StartCoroutine(ZoomEvilTwinCam());
        yield return new WaitUntil(() => montyDoorDownEventReceived);  //every frame checked??? could be better
        yield return new WaitForSeconds(2f);
       // camOnEvilTwin.Priority = 13; //put this cam on AFTER the door is down
        TellTextCloud(evilTwinSpeaks1);
        yield return new WaitForSeconds(timeToPauseOnAnyTwin);

       // montyGameCam.Priority = originalMontyGameCamPriority;
        if (mainMontySign) mainMontySign.SetActive(false);
        if (montyDoorsAndBoxes) montyDoorsAndBoxes.SetActive(false);
        if (inputControls) inputControls.SetActive(true);
        GameObject montyGameBarriers = GameObject.Find("MontyGameBarriers");
        if (montyGameBarriers) montyGameBarriers.SetActive(false);
        GameObject missed1 = GameObject.Find("Missed1(Clone)");
        if (missed1) missed1.SetActive(false);
        GameObject missed3 = GameObject.Find("Missed3(Clone)");
        if (missed3) missed3.SetActive(false);
        GameObject montyGoal = GameObject.Find("MontyGoal(Clone)");
        if (montyGoal) montyGoal.SetActive(false);

        agent1.speed = agent1OriginalSpeed;
        anim1.speed = anim1OriginalSpeed;
        camOnEvilTwin.Priority = originalCamOnEvilTwinPriority;  // disable the camOnEvilTwin and revert to follow cam
    }
    private IEnumerator ZoomEvilTwinCam()
    {
        var originalFOV = camOnEvilTwin.m_Lens.FieldOfView;
        var targetFOV = originalFOV - zoomAmount;  //40 - 23 = 17 

        float timer = 0f;

        while (timer < zoomTime)
        {
            timer += Time.deltaTime;
            camOnEvilTwin.m_Lens.FieldOfView = Mathf.Lerp(originalFOV, targetFOV, timer / zoomTime);
          //  Debug.Log("ori/targ  " + originalFOV + "/" + targetFOV + "  camFOV = " + camOnEvilTwin.m_Lens.FieldOfView);
            yield return null;
        }
    }
    private IEnumerator ZoomGoodTwinCam()
    {
        var originalFOV = camOnGoodTwin.m_Lens.FieldOfView;
        var targetFOV = originalFOV - zoomAmount;  //40 - 23 = 17 

        float timer = 0f;

        while (timer < zoomTime)
        {
            timer += Time.deltaTime;
            camOnGoodTwin.m_Lens.FieldOfView = Mathf.Lerp(originalFOV, targetFOV, timer / zoomTime);
            //  Debug.Log("ori/targ  " + originalFOV + "/" + targetFOV + "  camFOV = " + camOnGoodTwin.m_Lens.FieldOfView);
            yield return null;
        }
    }
    void TellTextCloud(string caption)
    {
        m_MyEvent.Invoke(5, 4, caption);
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
    private void DisableTheDoorButtons()   //DoorButtons Will be Deimped, but we keep this temporarily 
    {
        if (door1Button) door1Button.SetActive(false);
        if (door2Button) door2Button.SetActive(false);
        if (door3Button) door3Button.SetActive(false);
    }
    private void CleanUpTheMontyGameAndUnlockThePlayer()
    {
        montyGameActive = false;
        DisableTheDoorButtons();
        CloseTheFirstOpenedDoor();
        UnlockPlayerFromTheGameTriggerArea();
    }
    IEnumerator WaitSeconds(float timeToWait, AudioClip audioClip)
    {
        yield return new WaitForSeconds(timeToWait);
        audioManager.PlayAudio(audioClip);
       // if (!playerPickedWinner && !montyGameEnded) montyGameSignText.text = "A chance to change door. Choose...";  //3/7/23 moved to after drama audio
        if (montyGameEnded)
        {
            Debug.Log("WaitForSeconds sees montyGameEdnded = true");
            montyGameMoveOnButton.SetActive(true);
        }
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        thirdPersonController.MoveSpeed = originalMoveSpeed;   //just to be sure?
        thirdPersonController.SprintSpeed = originalSprintSpeed;
    }
}  //end class 