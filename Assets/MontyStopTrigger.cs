using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject montyGameText;

    bool playerPickedDoor1, playerPickedDoor2, playerPickedDoor3, door1Down, door2Down, door3Down, montyGameEnded;
    bool awaitingFinalDoorPick;

    bool montyGamePlayed;
    int doorNumberPicked, doorNumberDown, theWinningDoor;

    Animator animDoor1, animDoor2, animDoor3;
    // Start is called before the first frame update
    void Start()
    {
        movingPlatform = GameObject.Find("MovingPlatform").GetComponent<MovingPlatform>();
        animDoor1 = montyDoor1.GetComponent<Animator>();
        animDoor2 = montyDoor2.GetComponent<Animator>();
        animDoor3 = montyDoor3.GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("MovingPlatform"))
        {
            Debug.Log(other.gameObject.name + " Entered montyPStop.. ");
            movingPlatform.speed = 0;
            if (stopButton) stopButton.SetActive(false);
            //  goButton.SetActive(true);
            if (!montyGamePlayed)    PlayTheMontyGame();
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("MovingPlatform"))
        {
            Debug.Log(other.gameObject.name + " Exited montyPlayArea... ");
        }

    }
    private void PlayTheMontyGame()
    {
        montyGamePlayed = true;
        if (door1Button) door1Button.SetActive(true);
        if (door2Button) door2Button.SetActive(true);
        if (door3Button) door3Button.SetActive(true);
    }
    public void OnDoor1ButtonPressed()
    {
        if (awaitingFinalDoorPick)
        {
            animDoor1.SetTrigger("MontyDoor1Down");
            montyGameEnded = true;
            if (theWinningDoor == 1) Debug.Log("Door 1 is a Winner");
            DisableTheDoorButtons();
            CloseTheFirstOpenedDoor();
            return;
        }
        DisableTheDoorButtons();
        playerPickedDoor1 = true;
        doorNumberPicked = 1;
        Debug.Log("player picked door 1" + playerPickedDoor1);
        ShowAlternativeDoors();
    }
    public void OnDoor2ButtonPressed()
    {
        if (awaitingFinalDoorPick)
        {
            animDoor2.SetTrigger("MontyDoor2Down");
            montyGameEnded = true;
            if (theWinningDoor == 2) Debug.Log("Door 2 is a Winner");
            CloseTheFirstOpenedDoor();
            DisableTheDoorButtons();
            return;
        }
        DisableTheDoorButtons();
        playerPickedDoor2 = true;
        doorNumberPicked = 2;
        Debug.Log("player picked door 2" + playerPickedDoor2);
        ShowAlternativeDoors();
    }
    public void OnDoor3ButtonPressed()
    {
        if (awaitingFinalDoorPick)
        {
            animDoor3.SetTrigger("MontyDoor3Down");
            montyGameEnded = true;
            if (theWinningDoor == 3) Debug.Log("Door 3 is a Winner");
            DisableTheDoorButtons();
            CloseTheFirstOpenedDoor();
            return;
        }
        DisableTheDoorButtons();
        playerPickedDoor3 = true;
        doorNumberPicked = 3;
        Debug.Log("player picked door 3" + playerPickedDoor3);
        ShowAlternativeDoors();
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
    private void CloseTheFirstOpenedDoor()
    {
        switch (doorNumberDown)
        {
            case 1:
                {
                    animDoor1.SetTrigger("MontyDoor1Up");
                    break;
                }
            case 2:
                {
                    animDoor2.SetTrigger("MontyDoor2Up");
                    break;

                }
            case 3:
                {
                    animDoor3.SetTrigger("MontyDoor3Up");
                    break;
                }
        }
    }
    private void DisableTheDoorButtons()
    {
        if (door1Button) door1Button.SetActive(false);
        if (door2Button) door2Button.SetActive(false);
        if (door3Button) door3Button.SetActive(false);
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
