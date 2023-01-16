using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;
using TMPro;
using randomize_array;

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
    readonly int[] gameSums = new int[] { 30, 40, 50, 50, 60, 70 };
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
        cubeGameCubes = GameObject.FindGameObjectsWithTag("CubeGameCube");
        cubeGamePlacement = GameObject.FindGameObjectsWithTag("CubeGamePlacement");
        cubeGameTargetSum = GameObject.FindGameObjectsWithTag("TargetSum");
        inputControls = GameObject.Find("Joysticks_StarterAssetsInputs_Joysticks");


        cubePlacementPosition = new Vector3[cubeGamePlacement.Length];
        cubeTransformStartPosition = new Vector3[cubeGameCubes.Length];
        for (int i = 0; i <= cubeGameCubes.Length - 1; i++)
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

    }
    void Shuffle(int[] intArr)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < intArr.Length; t++)
        {
            int tmp = intArr[t];
            int r = Random.Range(t, intArr.Length);
            intArr[t] = intArr[r];
            intArr[r] = tmp;
        }
    }
    void SeedCubePuzzle()
    {
        Shuffle(gameSums);
        Debug.Log(gameSums[0] + ", " + gameSums[1] + ", " + gameSums[2] + ", " + gameSums[3] + ", " + gameSums[4]);
        for (int i = 0; i <= cubeGameTargetSum.Length-1; i++)
        {
            cubeGameTargetSumText[i].text = gameSums[i].ToString();  
        }
        //int Random.Range (0,10) will return a random value 0 thru "9" - beware
        /* TEMPORARILY DON'T SEED AN INITIAL CUBE - JUST SEED THE SUMS 
        int randomCubePosition = Random.Range(0, cubeGamePlacement.Length);  //higher val was  hardcoded to 3 but u never know...
        int randomCubePlacement = Random.Range(0, cubeGamePlacement.Length);  //higher val was  hardcoded to 3 but u never know...
                                                                              //Debug.Log("randCubeRAW = " + randomCubePosition +        "  randCubePlacRAW = "  + randomCubePlacement );
                                                                              //Debug.Log("randCube =      " + (randomCubePosition+1)*10 + "  randCubePlac =        " + (randomCubePlacement+1));

        Vector3 xForward = new Vector3(1.5f, 0f, 0f);//pull cube out toward cam
        cubeGameCubes[randomCubePosition].transform.position = cubePlacementPosition[randomCubePlacement] + xForward;
        */
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
        }
    }
    void TellTextCloud(string caption)
    {
        m_MyEvent.Invoke(5, 4, caption);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            cubeGameCam.Priority = originalCamPriority;
            for (int i = 0; i <= cubeGameCubes.Length - 1; i++)  //Restore the cubes to home/original positions 
            {
                cubeGameCubes[i].transform.position = cubeTransformStartPosition[i];
            }
        }
    }
}  //end class 
   //} //end namespace
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