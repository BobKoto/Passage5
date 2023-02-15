using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.Animations;
using TMPro;
using Cinemachine;
using UnityEngine.Events;
using CarouselAndMovingPlatforms;
using UnityEditor.Animations;
using System.Linq;

[System.Serializable]
public class MontyDoorTouchEvent : UnityEvent<int> { }
[System.Serializable]
public class MontyPlayButtonTouchEvent : UnityEvent { }
[System.Serializable]
public class MontyMoveOnButtonTouchEvent : UnityEvent { }
[System.Serializable]
public class MontyDoorDownEvent : UnityEvent<int> { }

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

    [Header("Text Above the Doors")]
    public TMP_Text montyGameSignText;

    [Header("Monty Game Intro")]
    public GameObject montyGameIntro;

    [Header("Monty Game Barriers")]
    public GameObject montyGameBarriers;

    [Header("Monty Game Move On Button")]
    public GameObject montyGameMoveOnButton;

    [Header("The Player")]
    public GameObject playerArmature;

    [Header("Cinemachine Cameras")]
    public CinemachineVirtualCamera thirdPersonFollowCam;
    public CinemachineFreeLook freeLookCam;
    public CinemachineVirtualCamera montyGameCam;

    [Header("Animations")]
    public Animation animDoor1Down;
    public Animation animDoor2Down;
    public Animation animDoor3Down;

    private int animDoor1FinishedHash;
    private int animDoor2FinishedHash;
    private int animDoor3FinishedHash;
    private string animDoor1FinishedName = "hello";
    int originalCamPriority;

    int thirdPersonFollowCamOriginalPriority, freeLookCamOriginalPriority;

    [Header("The Input System canvas Joystick etc.")]
    public GameObject inputControls;

    bool playerPickedDoor1, playerPickedDoor2, playerPickedDoor3, door1Down, door2Down, door3Down;
    bool awaitingFinalDoorPick, playerPickedWinner;

    public static bool montyGameEnded;
    int doorNumberDown, theWinningDoor;

    ThirdPersonController thirdPersonController;
    float originalMoveSpeed, originalSprintSpeed;

    public PlayerEnteredRelevantTrigger triggerEvent;

    public MontyDoorTouchEvent montyDoorTouchEvent;
    public MontyPlayButtonTouchEvent montyPlayButtonTouchEvent;
    public MontyMoveOnButtonTouchEvent montyMoveOnButtonTouchEvent;
    public MontyDoorDownEvent montyDoorDownEvent;

    Animator animDoor1, animDoor2, animDoor3, animPlayer;
    AudioManager audioManager;
    public static bool montyGameActive;
    void Start()
    {
        //Debug.Log(" 3pf cam priority = " + thirdPersonFollowCam.Priority + "    freeLook cam priority = " + freeLookCam.Priority);
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        movingPlatform = GameObject.Find("MovingPlatform").GetComponent<MovingPlatform>();
        animDoor1 = montyDoor1.GetComponent<Animator>();
        animDoor2 = montyDoor2.GetComponent<Animator>();
        animDoor3 = montyDoor3.GetComponent<Animator>();

        // Debug.Log("")

        animPlayer = playerArmature.GetComponent<Animator>();

        thirdPersonController = playerArmature.GetComponent<ThirdPersonController>();
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

        originalCamPriority = montyGameCam.Priority;


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
                //audioManager.PlayAudio(audioManager.clipDRUMROLL);
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
        Debug.Log("IntroPlay Button Pressed - setting Intro Panel Active to false");
        if (montyGameIntro) montyGameIntro.SetActive(false);
        if (inputControls) inputControls.SetActive(false);
        audioManager.PlayAudio(audioManager.clipDRUMROLL);
    }
    public void MoveOnButtonPressed()
    {
        Debug.Log("MoveOn Button Pressed - wiping out all");
        if (montyGameMoveOnButton) montyGameMoveOnButton.SetActive(false);
        montyGameCam.Priority = originalCamPriority;
        if (inputControls) inputControls.SetActive(true);
        // audioManager.PlayAudio(audioManager.clipDRUMROLL);
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
        if (montyGameBarriers) montyGameBarriers.SetActive(false);
        if (montyDoorsAndBoxes) montyDoorsAndBoxes.SetActive(false);
    }
    private void PlayTheMontyGame()
    {
        //if (door1Button) door1Button.SetActive(true);//if (door2Button) door2Button.SetActive(true);//if (door3Button) door3Button.SetActive(true);
        Debug.Log("PlayTheMontyGame() set  montyGameActive = true ");
        montyGameActive = true;
    }
    //Door Buttons being Deimped, so these OnPress methods commented out....

    //public void OnDoor1ButtonPressed()
    //{
    //    ProcessTheDoorButton(1);
    //}
    //public void OnDoor2ButtonPressed()
    //{
    //    ProcessTheDoorButton(2);
    //}
    //public void OnDoor3ButtonPressed()
    //{
    //    ProcessTheDoorButton(3);
    //}
    public void OnMontyDoorTouch(int _doorNumber) => ProcessTheDoorButton(_doorNumber); // its not Button anymore //Above 3 UI buttons will be deimped    
                                                                                        //getting door # from ActOnMontyDoorTouch.cs Event 
    private void ProcessTheDoorButton(int doorPressed)
    {
        // bool animationIsPlaying = AnimIsPlaying(animDoor1Down) || AnimIsPlaying(animDoor2Down) || AnimIsPlaying(animDoor3Down);
        if (awaitingFinalDoorPick)  // //////////////////// This IS the SECOND door pick!! /////////////////////////////////
        {
            ProcessFinalDoorPick(doorPressed);
        }
        DisableTheDoorButtons();  //deimped but hold because we may want to do something here like wait on the door animation
        switch (doorPressed)
        {
            case 1:
                playerPickedDoor1 = true;  //BEWARE it seems these cases are only good for a single iteration of the monty game! 
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
            StartCoroutine(WaitSeconds(2f, audioManager.clipdrama));
            //  montyGameSignText.text = "A chance to change door pick...";         
            ShowAlternativeDoors();  // HERE MAYBE is where we don't want to allow a door touch until the alt door animation is finished
            return;
        }
        // (was an else) the Monty Game IS Ended
        if (playerPickedWinner)
        {
            StartCoroutine(WaitSeconds(2f, audioManager.clipApplause));
            //   audioManager.PlayAudio(audioManager.clipApplause);
        }
        else
        {
            StartCoroutine(WaitSeconds(.5f, audioManager.clipfalling));
        }
        // montyGameCam.Priority = originalCamPriority;  //this is premature and should be done upon complete eviction of monty game
    }
    void ProcessFinalDoorPick(int doorPressed)
    {
        switch (doorPressed)     // //////////////////// This IS the SECOND door pick!! /////////////////////////////////
        {
            case 1:
                animDoor1.SetTrigger("MontyDoor1Down");  //show the Final Door player picked, & set sign to Result
                montyGameEnded = true;
                if (theWinningDoor == 1)
                {
                    Debug.Log("Door 1 is a Winner");
                    playerPickedWinner = true;
                    montyGameSignText.text = "Door 1 is a winner!";
                }
                else
                {
                    Debug.Log("Door 1 is a Loser");
                    montyGameSignText.text = "Door 1 is a loser... awww";
                }
                //    CleanUpTheMontyGameAndUnlockThePlayer();
                break;

            case 2:
                animDoor2.SetTrigger("MontyDoor2Down");
                montyGameEnded = true;
                if (theWinningDoor == 2)
                {
                    Debug.Log("Door 2 is a Winner");
                    playerPickedWinner = true;
                    montyGameSignText.text = "Door 2 is a winner!";
                }
                else
                {
                    Debug.Log("Door 2 is a Loser");
                    montyGameSignText.text = "Door 2 is a loser... awww";
                }
                //     CleanUpTheMontyGameAndUnlockThePlayer();
                break;
            case 3:
                animDoor3.SetTrigger("MontyDoor3Down");
                montyGameEnded = true;
                if (theWinningDoor == 3)
                {
                    Debug.Log("Door 3 is a Winner");
                    playerPickedWinner = true;

                    montyGameSignText.text = "Door 3 is a winner!";
                }
                else
                {
                    Debug.Log("Door 3 is a Loser");
                    montyGameSignText.text = "Door 3 is a loser... awww";
                }
                //  CleanUpTheMontyGameAndUnlockThePlayer();
                break;
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
        // StartCoroutine(WaitForAnimator(doorNumberDown));
        // audioManager.PlayAudio(audioManager.clipding);
        //switch (doorNumberDown)
        //{
        //    case 1:
        //        {
        //                                                                  // if (door1Button) door1Button.SetActive(true);
        //            if (door2Button) door2Button.SetActive(true);
        //            if (door3Button) door3Button.SetActive(true);
        //            break;
        //        }
        //    case 2:
        //        {
        //            if (door1Button) door1Button.SetActive(true);
        //                                                                      // if (door2Button) door2Button.SetActive(true);
        //            if (door3Button) door3Button.SetActive(true);
        //            break;
        //
        //        }
        //    case 3:
        //        {
        //            if (door1Button) door1Button.SetActive(true);
        //            if (door2Button) door2Button.SetActive(true);
        //                                                                       //   if (door3Button) door3Button.SetActive(true);
        //            break;
        //        }
        //}
    }
    public void OnMontyDoorDown(int doorDownReceived)
    {
        Debug.Log("MontyDoorDownEvent  says  doorDown is door " + doorDownReceived);
    }
    //public void AlertObservers(string message)  //this doesn't work - but keep as a placeholder for future logic decision(s)
    //{
    //    Debug.Log("MONTY STOP TRIGGER Animation event received... " + message);
    //    if (message.Equals("Door1DownFinished"))
    //    {
    //        //pc_attacking = false;
    //        //pc_anim.SetBool("attack", false);
    //        // Do other things based on an attack ending.
    //        Debug.Log("Animation event received... " + message);
    //    }
    //}
    IEnumerator WaitForAnimator(int doorDown)   //this doesn't work - but keep as a placeholder for future logic decision(s)
    {
        bool animIsDone = false;
        Debug.Log("WaitForAnimator Entered....... doorDown = " + doorDown);
        while (!animIsDone)
        {
            switch (doorDown)
            {
                case 1:
                    AnimatorStateInfo animState1 = animDoor1.GetCurrentAnimatorStateInfo(0);
                    animIsDone = (animState1.normalizedTime >= 1.0f && animState1.shortNameHash == animDoor1FinishedHash);
                    Debug.Log("animState1.normalizedTime = " + animState1.normalizedTime + " shortNameHash = " + animState1.shortNameHash);
                    break;
                case 2:
                    AnimatorStateInfo animState2 = animDoor2.GetCurrentAnimatorStateInfo(0);
                    animIsDone = (animState2.normalizedTime >= 1.0f && animState2.shortNameHash == animDoor2FinishedHash);
                    Debug.Log("animState2.normalizedTime = " + animState2.normalizedTime + " shortNameHash = " + animState2.shortNameHash);
                    break;
                case 3:
                    AnimatorStateInfo animState3 = animDoor3.GetCurrentAnimatorStateInfo(0);
                    animIsDone = (animState3.normalizedTime >= 1.0f && animState3.shortNameHash == animDoor3FinishedHash);
                    Debug.Log("animState3.normalizedTime = " + animState3.normalizedTime + " shortNameHash = " + animState3.shortNameHash);
                    break;
            }
            yield return new WaitForSeconds(.5f);
        }
        Debug.Log("Anim is Done Goodbye.......");
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
        if (!playerPickedWinner && !montyGameEnded) montyGameSignText.text = "A chance to change door pick...";
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