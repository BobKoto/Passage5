using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectARandomGame: MonoBehaviour
{//Component of SelectARandomGameTrigger
    AudioManager audioManager;
    BoxCollider boxCollider;
    [Header ("The Signage")]
    public GameObject selectRandomGameSign;     //The whole sign
    public TMP_Text selectRandomGameSignText; //And the text

    GameObject[] teleportals;

    Vector3 leftPortal =   new Vector3(203.74f, 4.82f, -231.45f);   //make these public?
    Vector3 centerPortal = new Vector3(203.74f, 4.82f, -217.2f);
    Vector3 rightPortal =  new Vector3(203.74f, 4.82f, -202.94f);

    private SetRandomPortals randomPortalsGenerator;
    const int numberOfPortals = 3;
    int[] randomNumbers;
    private void Start()
    {
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
        DisableThePortals();  //6/17/23 a case for a game manager - but let's keep going because we might not go too much farther 

        randomPortalsGenerator = FindObjectOfType<SetRandomPortals>();
        randomNumbers = randomPortalsGenerator.GetRandomNumbers(numberOfPortals);
        foreach (int number in randomNumbers)
        {
            Debug.Log("random portal = " + number);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;  //6/13/23 why oh why do we need to do this? the dummy's arms & legs, dummy.
       // Debug.Log(this.name + "  " + other.name + " came thru! Let's do something now ");
        boxCollider.enabled = false;
        if (selectRandomGameSign) selectRandomGameSign.SetActive(true);
        audioManager.PlayAudio(audioManager.theetone);
        SelectARandomGameIntro();

    }
    void DisableThePortals()   //this should be temporary
    {
        teleportals = GameObject.FindGameObjectsWithTag("RandomGamePortal");
        Debug.Log("found " + teleportals.Length + " portals for the random game ...............");
        for (int i = 0; i <= teleportals.Length -1; i++)
        {
            teleportals[i].SetActive(false);
        }
    }
    void SelectARandomGameIntro()
    {
       // Debug.Log(this.name + "  You won " + CubeGameHandler.roundsWon + " rounds in the Cube Game");
        switch (CubeGameHandler.roundsWon)
        {
            case 0: //Debug.Log("You won zero cube rounds. YOU ARE DEAD!!!! Goodbye...");
                selectRandomGameSignText.text = "Zero cube rounds won. YOU ARE DEAD!!!! Goodbye...";
                break;
            case 1: //Debug.Log("You won 1 cube round your only choice is to enter this world...");
                selectRandomGameSignText.text = "1 cube round won. The only choice is to enter this portal. Walk forward to enter...";
                teleportals[randomNumbers[0]].transform.position = centerPortal;
                teleportals[randomNumbers[0]].SetActive(true);
                break;
            case 2: //Debug.Log("You won 2 cube rounds. You can pick from these 2 worlds");
                selectRandomGameSignText.text = "2 cube rounds won. Pick from these 2 portals. Walk into your choice...";
                teleportals[randomNumbers[0]].transform.position = centerPortal;
                teleportals[randomNumbers[0]].SetActive(true);
                teleportals[randomNumbers[1]].transform.position = rightPortal;
                teleportals[randomNumbers[1]].SetActive(true);
                break;
            case 3: //Debug.Log("You won 3 cube rounds. You can pick from these 3 worlds");
                selectRandomGameSignText.text = "3 cube rounds won. Pick from these 3 portals. Walk into your choice...";
                teleportals[randomNumbers[0]].transform.position = leftPortal;
                teleportals[randomNumbers[0]].SetActive(true);
                teleportals[randomNumbers[1]].transform.position = centerPortal;
                teleportals[randomNumbers[1]].SetActive(true);
                teleportals[randomNumbers[2]].transform.position = rightPortal;
                teleportals[randomNumbers[2]].SetActive(true);
                break;
        }
    }
}
// following 2 classes rightoutta GPT FWIW
//public class RandomNumberGenerator : MonoBehaviour
//{
//    private int[] numbers = { 1, 2, 3, 4 }; //, 5, 6, 7, 8, 9, 10, 11, 12 };

//    private void Start()
//    {
//        Debug.Log(" Hello from RandomNumberGenerator");
//        int[] randomNumbers = GetRandomNumbers(3);

//        foreach (int number in randomNumbers)
//        {
//            Debug.Log(number);
//        }
//    }

//    public int[] GetRandomNumbers(int count)
//    {
//        List<int> remainingNumbers = new List<int>(numbers);
//        List<int> randomNumbers = new List<int>();

//        for (int i = 0; i < count; i++)
//        {
//            int index = Random.Range(0, remainingNumbers.Count);
//            int randomNumber = remainingNumbers[index];
//            remainingNumbers.RemoveAt(index);
//            randomNumbers.Add(randomNumber);
//        }

//        return randomNumbers.ToArray();
//    }
//}
//================== now call it ====================   moved into our original Start()
//using UnityEngine;

//public class RandomNumberCaller : MonoBehaviour
//{
//    private RandomNumberGenerator randomNumberGenerator;
//    const int numberOfPortals = 3;
//    private void Start()
//    {
//        randomNumberGenerator = FindObjectOfType<RandomNumberGenerator>();

//        // Call the GetRandomNumbers method from the RandomNumberGenerator script
//        int[] randomNumbers = randomNumberGenerator.GetRandomNumbers(numberOfPortals);

//        foreach (int number in randomNumbers)
//        {
//            Debug.Log("random portal = " + number);
//        }
//    }
//}

