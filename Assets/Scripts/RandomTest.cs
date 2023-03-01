using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Security.Cryptography;
//using Random = UnityEngine.Random;

namespace randomize_array
{
    public class RandomTest : MonoBehaviour
    {//Component of RandomTest
        int[] doors = { 1, 2, 3 };  //just to confuse me and anyone else who looks at this 
        public GameObject[] montyGameObject;
        public static int winningDoor;

        private void Start()
        {
            SetRandomWinningDoor();

            // arr[0] actually holds a 1,2 or 3 so we need to subtract 1 for the index of montyGameObject array, and so on
            // otherwise Monty Hall would ask us to pick door 0, 1, or 2... :|

            Instantiate(montyGameObject[doors[0] - 1], new Vector3(237f, 3f, -219), Quaternion.Euler(0f, 271f, 0f));
            MeshRenderer m1 = montyGameObject[doors[0] - 1].GetComponent<MeshRenderer>();
            m1.enabled = false;

            Instantiate(montyGameObject[doors[1] - 1], new Vector3(237f, 3f, -212), Quaternion.Euler(0f, 271f, 0f));
            MeshRenderer m2 = montyGameObject[doors[1] - 1].GetComponent<MeshRenderer>();
            m2.enabled = false;

            Instantiate(montyGameObject[doors[2] - 1], new Vector3(237f, 3f, -205), Quaternion.Euler(0f, 271f, 0f));
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
