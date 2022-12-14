using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.Events;
using CarouselAndMovingPlatforms;

public class MontyStopTrigger : MonoBehaviour
{
    MovingPlatform movingPlatform;
    [Header("UI Buttons")]
    public GameObject stopButton;
    public GameObject goButton;
    public GameObject door1Button;
    public GameObject door2Button;
    public GameObject door3Button;

    [Header("Monty Game Doors")]
    public GameObject montyDoor1;
    public GameObject montyDoor2;
    public GameObject montyDoor3;

    [Header("Text Above the Doors")]
    public TMP_Text montyGameSignText;

    [Header("The Player")]
    public GameObject playerArmature;

    [Header("Cinemachine Cameras")]
    public CinemachineVirtualCamera thirdPersonFollowCam;
    public CinemachineFreeLook freeLookCam;

    int thirdPersonFollowCamOriginalPriority, freeLookCamOriginalPriority;

    [Header("The Input System canvas")]
    public GameObject inputControls;

    bool playerPickedDoor1, playerPickedDoor2, playerPickedDoor3, door1Down, door2Down, door3Down;
    bool awaitingFinalDoorPick, playerPickedWinner;

    public static bool montyGameEnded ;
    int doorNumberDown, theWinningDoor;

    ThirdPersonController thirdPersonController;
    float originalMoveSpeed, originalSprintSpeed;

    public PlayerEnteredRelevantTrigger triggerEvent;

    Animator animDoor1, animDoor2, animDoor3, animPlayer;
    // Start is called before the first frame update
    AudioManager audioManager;
    void Start()
    {
        //Debug.Log(" 3pf cam priority = " + thirdPersonFollowCam.Priority + "    freeLook cam priority = " + freeLookCam.Priority);
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        movingPlatform = GameObject.Find("MovingPlatform").GetComponent<MovingPlatform>();
        animDoor1 = montyDoor1.GetComponent<Animator>();
        animDoor2 = montyDoor2.GetComponent<Animator>();
        animDoor3 = montyDoor3.GetComponent<Animator>();
        animPlayer = playerArmature.GetComponent<Animator>();
        thirdPersonController = playerArmature.GetComponent<ThirdPersonController>();
        originalMoveSpeed = thirdPersonController.MoveSpeed;
        originalSprintSpeed = thirdPersonController.SprintSpeed;
        if (triggerEvent == null)
        {
            triggerEvent = new PlayerEnteredRelevantTrigger();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("MovingPlatform"))
        {
         //  Debug.Log(other.gameObject.name + " Entered montyPStop.. ");
            if (!montyGameEnded)
            {
                triggerEvent.Invoke(1);
                LockPlayerInTheMontyGameTriggerArea();
                PlayTheMontyGame();
                audioManager.PlayAudio(audioManager.clipDRUMROLL);
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("MovingPlatform"))
        {
         //   Debug.Log(other.gameObject.name + " Exited montyPlay(STOP)Trigger... from MontyStopTrigger");
            DisableTheDoorButtons();
        }
    }
    private void LockPlayerInTheMontyGameTriggerArea()
    {
        movingPlatform.speed = 0;
        if (stopButton) stopButton.SetActive(false); //Part of locking the player in the game trigger area
        if (goButton) goButton.SetActive(false);
        thirdPersonController.MoveSpeed = 0f;
        thirdPersonController.SprintSpeed = 0f;
        // Here we may need to rotate player to face doors  OR the play can do it 
        // playerArmature.transform.rotation.y = //a bad start 
        //freeLookCam.MoveToTopOfPrioritySubqueue();   //moves on rotation AND Look   but the call doesn't work?
        //freeLookCam.Priority = 12;
    }
    private void UnlockPlayerFromTheGameTriggerArea()
    {
        if (AddRemoveChild.playerIsOnYellowPlatform)
        {
            if (stopButton) stopButton.SetActive(false); //Part of locking the player in the game trigger area
            if (goButton) goButton.SetActive(true);

        }
        thirdPersonController.MoveSpeed = originalMoveSpeed;
        thirdPersonController.SprintSpeed = originalSprintSpeed;
        //thirdPersonFollowCam.MoveToTopOfPrioritySubqueue();  //only moves on Look    call doesn't work?
        //freeLookCam.Priority = 10;   //spins back too soon 
    }

    private void PlayTheMontyGame()
    {
      //  montyGamePlayed = true;
        if (door1Button) door1Button.SetActive(true);
        if (door2Button) door2Button.SetActive(true);
        if (door3Button) door3Button.SetActive(true);
    }
    public void OnDoor1ButtonPressed()
    {
        ProcessTheDoorButton(1);
        //if (awaitingFinalDoorPick)
        //{
        //    animDoor1.SetTrigger("MontyDoor1Down");
        //    montyGameEnded = true;
        //    if (theWinningDoor == 1) 
        //    {
        //       Debug.Log("Door 1 is a Winner");
        //        montyGameSignText.text = "Door 1 is a winner!"; 
        //    }
        //    else
        //    {
        //        Debug.Log("Door 1 is a Loser");
        //        montyGameSignText.text = "Door 1 is a loser... awww";
        //    }
        //    CleanUpTheMontyGameAndUnlockThePlayer();
        //    return;
        //}
        //DisableTheDoorButtons();
        //playerPickedDoor1 = true;
        ////doorNumberPicked = 1;
        //Debug.Log("player picked door 1" + playerPickedDoor1);
        //ShowAlternativeDoors();
    }
    public void OnDoor2ButtonPressed()
    {
        ProcessTheDoorButton(2);
        //if (awaitingFinalDoorPick)
        //{
        //    animDoor2.SetTrigger("MontyDoor2Down");
        //    montyGameEnded = true;
        //    if (theWinningDoor == 2)
        //    {
        //        Debug.Log("Door 2 is a Winner");
        //        montyGameSignText.text = "Door 2 is a winner!";
        //    }
        //    else
        //    {
        //        Debug.Log("Door 2 is a Loser");
        //        montyGameSignText.text = "Door 2 is a loser... awww";
        //    }
        //    CleanUpTheMontyGameAndUnlockThePlayer();
        //    return;
        //}
        //DisableTheDoorButtons();
        //playerPickedDoor2 = true;
        ////doorNumberPicked = 2;
        //Debug.Log("player picked door 2" + playerPickedDoor2);
        //ShowAlternativeDoors();
    }
    public void OnDoor3ButtonPressed()
    {
        ProcessTheDoorButton(3);
        //if (awaitingFinalDoorPick)
        //{
        //    animDoor3.SetTrigger("MontyDoor3Down");
        //    montyGameEnded = true;
        //    if (theWinningDoor == 3)
        //    {
        //        Debug.Log("Door 3 is a Winner");
        //        montyGameSignText.text = "Door 3 is a winner!";
        //    }
        //    else
        //    {
        //        Debug.Log("Door 3 is a Loser");
        //        montyGameSignText.text = "Door 3 is a loser... awww";
        //    }
        //    CleanUpTheMontyGameAndUnlockThePlayer();
        //    return;
        //}
        //DisableTheDoorButtons();
        //playerPickedDoor3 = true;
        ////doorNumberPicked = 3;
        //Debug.Log("player picked door 3" + playerPickedDoor3);
        //ShowAlternativeDoors();
    }
    private void ProcessTheDoorButton(int doorPressed)
    {
        if (awaitingFinalDoorPick)   // //////////////////// This IS the SECOND door pick!! /////////////////////////////////
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

        }  // END Final/Second Door Pick
        DisableTheDoorButtons();
        switch (doorPressed)
        {
            case 1: playerPickedDoor1 = true;
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
           ShowAlternativeDoors();
        } else  // the Monty Game IS Ended
        {
            if (playerPickedWinner)
            {
                StartCoroutine(WaitSeconds(2f, audioManager.clipApplause));
             //   audioManager.PlayAudio(audioManager.clipApplause);
            } else
            {
                StartCoroutine(WaitSeconds(.5f, audioManager.clipfalling));
            }
        }


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
       // audioManager.PlayAudio(audioManager.clipding);
        switch (doorNumberDown)
        {
            case 1:
                {
                                                                              // if (door1Button) door1Button.SetActive(true);
                    if (door2Button) door2Button.SetActive(true);
                    if (door3Button) door3Button.SetActive(true);
                    break;
                }
            case 2:
                {
                    if (door1Button) door1Button.SetActive(true);
                                                                              // if (door2Button) door2Button.SetActive(true);
                    if (door3Button) door3Button.SetActive(true);
                    break;

                }
            case 3:
                {
                    if (door1Button) door1Button.SetActive(true);
                    if (door2Button) door2Button.SetActive(true);
                                                                               //   if (door3Button) door3Button.SetActive(true);
                    break;
                }
        }
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
    private void DisableTheDoorButtons()
    {
        if (door1Button) door1Button.SetActive(false);
        if (door2Button) door2Button.SetActive(false);
        if (door3Button) door3Button.SetActive(false);
    }
    private void CleanUpTheMontyGameAndUnlockThePlayer()
    {
        DisableTheDoorButtons();
        CloseTheFirstOpenedDoor();
        UnlockPlayerFromTheGameTriggerArea();
    }
    IEnumerator WaitSeconds(float timeToWait, AudioClip audioClip)
    {
        yield return new WaitForSeconds(timeToWait);
        audioManager.PlayAudio(audioClip);
        if (!playerPickedWinner && !montyGameEnded) montyGameSignText.text = "A chance to change door pick...";
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        thirdPersonController.MoveSpeed = originalMoveSpeed;   //just to be sure?
        thirdPersonController.SprintSpeed = originalSprintSpeed;
    }
    //IEnumerator ShowNextStepAfterAnimation()
    //{
    //    if (animDoor1.clip.isPlaying)
    //    yield return null;
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
