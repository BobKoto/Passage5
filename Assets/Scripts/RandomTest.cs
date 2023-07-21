using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//using Random = UnityEngine.Random;

namespace randomize_array
{
    public class RandomTest : MonoBehaviour
    {//Component of RandomTest
        int[] doors = { 1, 2, 3 };  //just to confuse me and anyone else who looks at this 
        public GameObject[] montyGameObject;
        [Header("The position of something in the 1st opened Monty door")]
        public Vector3 M1Position = new Vector3(204.09f, 4.51f, -214.4f); //all 3 close but refined in the editor
        public Vector3 M2Position = new Vector3(195.51f, 4.51f, -215.75f);
        public Vector3 M3Position = new Vector3(186.35f, 4.51f, -214.4f);

        public static int winningDoor;

        private void Start()
        {
            SetRandomWinningDoor();

            // arr[0] actually holds a 1,2 or 3 so we need to subtract 1 for the index of montyGameObject array, and so on
            // otherwise Monty Hall would ask us to pick door 0, 1, or 2... :|

            Instantiate(montyGameObject[doors[0] - 1], M1Position, Quaternion.Euler(0f, -16.5f, 0f));  //ori V3(237f, 3f, -219)
            MeshRenderer m1 = montyGameObject[doors[0] - 1].GetComponent<MeshRenderer>();
            m1.enabled = false;

            Instantiate(montyGameObject[doors[1] - 1], M2Position, Quaternion.Euler(0f, -16.5f, 0f));  //ori V3(237f, 3f, -212)
            MeshRenderer m2 = montyGameObject[doors[1] - 1].GetComponent<MeshRenderer>();
            m2.enabled = false;

            Instantiate(montyGameObject[doors[2] - 1], M3Position, Quaternion.Euler(0f, -16.5f, 0f));
            MeshRenderer m3 = montyGameObject[doors[2] - 1].GetComponent<MeshRenderer>();
            m3.enabled = false;

            for (int i = 0; i <= doors.Length; i++)
            {
                if (montyGameObject[doors[i] -1].name == "MontyGoal")
                {
                    winningDoor = i + 1;
                    Debug.Log("Winning door is " + winningDoor + "  " + montyGameObject[doors[i] - 1].name);
                    break;
                }
            }
        }
        public void SetRandomWinningDoor()
        {
            System.Random random = new System.Random();
            doors = doors.OrderBy(x => random.Next()).ToArray();
            //Debug.Log(" The random sequence is " + doors[0] + " " + doors[1] + " " + doors[2]);
        }
    }
}
