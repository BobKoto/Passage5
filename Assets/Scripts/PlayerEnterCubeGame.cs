using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class PlayerEnterCubeGame : MonoBehaviour      //Componenet of PlayerEnterCubeTrigger (the collider in front of the cube game  
{                                                     //DeImped as of 2/27/23 or at least 5/6/23????
    public CloudTextEvent m_CloudTextEvent;
    const string helpNeedHI = "#Need human assist!";
    public AudioManager audioManager;
    public CinemachineVirtualCamera cubeGameCam;
    int originalCamPriority;
    readonly int[] gameSums = new int[] { 30, 40, 50, 50, 60, 70 };  //cubes = 10, 20, 30, 40
    public GameObject player;
    Animator animator;
    GameObject[] cubeGameCubes;
    GameObject[] cubeGamePlacement;
    GameObject[] cubeGameTargetSum;
    GameObject inputControls;
    public GameObject menuButton, lightButton, cubeGameStartButton, cubeGameIsUnsolvableButton, cubeGameExitButton; //Buttons to toggle 
    public GameObject cubeGameResultText;
    TMP_Text cubeGameWonOrLostText;
    TMP_Text[] cubeGameTargetSumText;
    Vector3[] cubeTransformStartPosition;  // so we can put cubes back to their original positions
    Vector3[] cubePlacementPosition; // where to automatically place/start a Cube 
    void Start()
    {
        if (!audioManager) audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        cubeGameWonOrLostText = cubeGameResultText.GetComponent<TMP_Text>();
        cubeGameCubes = GameObject.FindGameObjectsWithTag("CubeGameCube");
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
    }
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
        for (int i = 0; i <= cubeGameTargetSum.Length-1; i++)
        {
            cubeGameTargetSumText[i].text = gameSums[i].ToString();  
        }
        if (GameCanBeSolved())  { }  //do nothing yet
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
         //   cubeGameStartButton.SetActive(true);  //5/23/23  and made no difference?
            audioManager.PlayAudio(audioManager.clipDRUMROLL);
            TellTextCloud(helpNeedHI);
            animator.speed = 0;
        }
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
        cubeGameCam.Priority = originalCamPriority;
        for (int i = 0; i <= cubeGameCubes.Length - 1; i++)  //Restore the cubes to home/original positions 
        {
            cubeGameCubes[i].transform.position = cubeTransformStartPosition[i];
        }
        if (menuButton) menuButton.SetActive(true);
        if (lightButton) lightButton.SetActive(true);
        if (cubeGameStartButton) cubeGameStartButton.SetActive(false);
        animator.speed = 1;
        if (inputControls) inputControls.SetActive(true);
    }
    public void OnCubeGameIsUnsolvableButtonPressed()
    {
        Debug.Log("player pressed Can't Solve ");
        if (GameCanBeSolved())
        {
            Debug.Log("Wrong --- Game CAN be solved!");
            cubeGameWonOrLostText.text = "Nope can be solved...";
            cubeGameResultText.SetActive(true);
        }
        else
        {
            Debug.Log("Right --- Game CANNOT be solved!");
            cubeGameWonOrLostText.text = "Right! You Win";
            cubeGameResultText.SetActive(true);
        }
    }
    public void OnCubeGameExitButtonPressed()
    {
        // ExitTheCubeGame();
        if (cubeGameIsUnsolvableButton) cubeGameIsUnsolvableButton.SetActive(false);
        SeedCubePuzzle();
    }
    public void OnCubeGameStartButtonPressed()
    {
        // ExitTheCubeGame();
        SeedCubePuzzle();
        if (cubeGameStartButton) cubeGameStartButton.SetActive(false);
         cubeGameIsUnsolvableButton.SetActive(true);
    }
}  //end class 