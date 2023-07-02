using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missions : MonoBehaviour
{//component of Missions
   public static GameObject [] missions;
    [Header("Set how many missions we have here.")]
    public int numberOfMissions = 3;
    public static int randomlyPickedMission;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("hello from " + this.name);
        missions = GameObject.FindGameObjectsWithTag("Mission");
        for (int i = 0; i < missions.Length; i++)
        {
            Debug.Log(this.name + "  " + missions[i].name);
            missions[i].SetActive(false);
        }
        randomlyPickedMission = GetRandomMission(numberOfMissions);
    }
    int GetRandomMission(int missionsToChooseFrom)
    {
        int xRandom = Random.Range(0, missionsToChooseFrom);
        Debug.Log("Mission randomly picked is " + xRandom);
        return xRandom;
    }
}
