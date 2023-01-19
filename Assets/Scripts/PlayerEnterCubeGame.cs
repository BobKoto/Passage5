using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
//using System.Linq;
using TMPro;
//using randomize_array;

//using System.Security.Cryptography;
//namespace randomize_array
//{ 
public class PlayerEnterCubeGame : MonoBehaviour
{
    //Componenet of PlayerEnterCubeTrigger (the collider in front of the cube game  
    public MyIntEvent m_MyEvent;
    const string helpNeedHI = "#Need human assist!";
   // const string offPlatform = "#Now walk";
    public AudioManager audioManager;
    public CinemachineVirtualCamera cubeGameCam;
    int originalCamPriority;
    readonly int[] gameSums = new int[] { 30, 40, 50, 50, 60, 70 };  //cubes = 10, 20, 30, 40
    public GameObject player;
    Animator animator;
    GameObject cubeGameButtons;
    GameObject[] cubeGameCubes;
    GameObject[] cubeGamePlacement;
    GameObject[] cubeGameTargetSum;
    GameObject inputControls;
    TMP_Text[] cubeGameTargetSumText;
    Vector3[] cubeTransformStartPosition;  // so we can put cubes back to their original positions
    Vector3[] cubePlacementPosition; // where to automatically place/start a Cube 
    // Start is called before the first frame update
    void Start()
    {
        if (!audioManager) audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        cubeGameButtons = GameObject.Find("CubeGameButtons");  //won't find if it's disabled 
        //if (cubeGameButtons) Debug.Log("CGButtons found...."); else Debug.Log("CGButtons NOT found...."); //guess we must deactivate 
        if (cubeGameButtons) cubeGameButtons.SetActive(false);
        cubeGameCubes = GameObject.FindGameObjectsWithTag("CubeGameCube");
        cubeGamePlacement = GameObject.FindGameObjectsWithTag("CubeGamePlacement");
        cubeGameTargetSum = GameObject.FindGameObjectsWithTag("TargetSum");
        inputControls = GameObject.Find("Joysticks_StarterAssetsInputs_Joysticks");


        cubePlacementPosition = new Vector3[cubeGamePlacement.Length];
        cubeTransformStartPosition = new Vector3[cubeGameCubes.Length];
        for (int i = 0; i <= cubeGameCubes.Length - 1; i++)   //these 3 for loops can be consolidated...
        {
            cubeTransformStartPosition[i] = cubeGameCubes[i].transform.position;
        }
        for (int x = 0; x <= cubeGamePlacement.Length - 1; x++)
        {
            cubePlacementPosition[x] = cubeGamePlacement[x].transform.position;
        }
        cubeGameTargetSumText = new TMP_Text[4];
        for (int i= 0; i <= cubeGameTargetSum.Length-1; i++)
        {
            cubeGameTargetSumText[i] = cubeGameTargetSum[i].GetComponent<TMP_Text>();
        }

        originalCamPriority = cubeGameCam.Priority;
        animator = player.GetComponent<Animator>();

    }
    void Shuffle(int[] intArr)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < intArr.Length; t++)  //30, 40, 50, 60, 70 
        {
            int tmp = intArr[t];  //3 --
                                                   //t0  t1  t2  t3  t4  
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
       // int col1, col2, colSum, row1, row2, rowSum, theSum, firstSeed, secondSeed, thirdSeed, fourthSeed, firstPlusSecond, thirdPlusFourth;
        int firstPlusSecond, thirdPlusFourth;
        //as an example we got a seeding of 50 50 60 40 // which is solvable by 40 10 //50 
        //col1 = gameSums[0] + gameSums[2]; //110                                 20 30 //50
        //col2 = gameSums[1] + gameSums[3]; //90                                  60 40  100->each way - a winner 
        //colSum = col1 + col2;

        //row1 = gameSums[0] + gameSums[1]; //100  //ok row 1 
        //row2 = gameSums[2] + gameSums[3]; //100
        //rowSum = row1 + row2;

        //theSum = colSum + rowSum;  // col1 + col2 + row1 + row2;
        firstPlusSecond = gameSums[0] + gameSums[1];
        thirdPlusFourth = gameSums[2] + gameSums[3];
       // if (theSum == 400 && colSum == 200 && rowSum == 200)//can solve is not true yet - there is more to it 
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
            SeedCubePuzzle();
            audioManager.PlayAudio(audioManager.clipDRUMROLL);
            TellTextCloud(helpNeedHI);
            cubeGameButtons.SetActive(true);
            //animator.SetFloat("Speed", 0);
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
        //{
        //    cubeGameCam.Priority = originalCamPriority;
        //    for (int i = 0; i <= cubeGameCubes.Length - 1; i++)  //Restore the cubes to home/original positions 
        //    {
        //        cubeGameCubes[i].transform.position = cubeTransformStartPosition[i];
        //    }
        //}
        //cubeGameButtons.SetActive(false);
        //animator.speed = 1;
    }
    void ExitTheCubeGame()
    {
        cubeGameCam.Priority = originalCamPriority;
        for (int i = 0; i <= cubeGameCubes.Length - 1; i++)  //Restore the cubes to home/original positions 
        {
            cubeGameCubes[i].transform.position = cubeTransformStartPosition[i];
        }
        cubeGameButtons.SetActive(false);
        animator.speed = 1;
        if (inputControls) inputControls.SetActive(true);
    }
    public void OnCannotSolvePressed()
    {
        Debug.Log("player pressed Can't Solve ");
        if (GameCanBeSolved())
        {
            Debug.Log("Wrong --- Game CAN be solved!");
        }
        else
        {
            Debug.Log("Right --- Game CANNOT be solved!");
        }
    }
    public void OnCubeGameExitPressed()
    {
        // ExitTheCubeGame();
        SeedCubePuzzle();
    }
}  //end class 
   //} //end namespace
/* TEMPORARILY DON'T SEED AN INITIAL CUBE - JUST SEED THE SUMS 
int randomCubePosition = Random.Range(0, cubeGamePlacement.Length);  //higher val was  hardcoded to 3 but u never know...
int randomCubePlacement = Random.Range(0, cubeGamePlacement.Length);  //higher val was  hardcoded to 3 but u never know...
                                                                   //Debug.Log("randCubeRAW = " + randomCubePosition +        "  randCubePlacRAW = "  + randomCubePlacement );
                                                                   //Debug.Log("randCube =      " + (randomCubePosition+1)*10 + "  randCubePlac =        " + (randomCubePlacement+1));

Vector3 xForward = new Vector3(1.5f, 0f, 0f);//pull cube out toward cam
cubeGameCubes[randomCubePosition].transform.position = cubePlacementPosition[randomCubePlacement] + xForward;
*/
/*
using System;
using System.Linq;
using System.Security.Cryptography;

namespace randomize_array
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] arr = { 1, 2, 3, 4, 5 };
            Random random = new Random();
            arr = arr.OrderBy(x => random.Next()).ToArray();
            foreach (var i in arr)
            {
              //  Console.WriteLine(i);
            }
        }
    }
}
OR Another
void reshuffle(string[] texts)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < texts.Length; t++ )
        {
            string tmp = texts[t];
            int r = Random.Range(t, texts.Length);
            texts[t] = texts[r];
            texts[r] = tmp;
        }
    }
 
*/