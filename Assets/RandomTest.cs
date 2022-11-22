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
    {
        int[] doors = { 1, 2, 3 };
        public GameObject[] montyGameObject;
        private void Start()
        {
            DoGetRandom();
            //for (int i = 0; i <= montyGameObject.Length; i++)
            //{
            //    switch (i)
            //    {
            //        case 0:
            //           // var x = arr[0] -1;
            //          //  Debug.Log("x = " + x);

            //            Instantiate(montyGameObject[arr[0] - 1], new Vector3(276f, 3f, -228), Quaternion.Euler(0f, 180f, 0f));
            //            break;
            //        case 1:

            //          //  var y = arr[1] -1;
            //          //  Debug.Log("y = " + y);
            //            Instantiate(montyGameObject[arr[1] - 1], new Vector3(269f, 3f, -228), Quaternion.Euler(0f, 180f, 0f));
            //            break;
            //        case 2:

            //          //  var z = arr[2]-1;
            //          //  Debug.Log("z = " + z);
            //            Instantiate(montyGameObject[arr[2] - 1], new Vector3(262f, 3f, -228), Quaternion.Euler(0f, 180f, 0f));
            //            break;
            //    }

            //}
            // arr[0] actually holds a 1,2 or 3 so we need to subtract 1 for the index of montyGameObject array, and so on
            // otherwise Monty Hall would ask us to pick door 0, 1, or 2... :|
            Instantiate(montyGameObject[doors[0] - 1], new Vector3(276f, 3f, -228), Quaternion.Euler(0f, 180f, 0f));
            Instantiate(montyGameObject[doors[1] - 1], new Vector3(269f, 3f, -228), Quaternion.Euler(0f, 180f, 0f));
            Instantiate(montyGameObject[doors[2] - 1], new Vector3(262f, 3f, -228), Quaternion.Euler(0f, 180f, 0f));

            for (int i = 0; i <= doors.Length; i++)

            {
                if (montyGameObject[doors[i] -1].name == "MontyGoal")
                {
                    var goalDoor = i + 1;
                    Debug.Log("GOAL is door " + goalDoor + "  " + montyGameObject[doors[i] - 1].name);
                    break;
                }
            }
        }
        //IEnumerator RandomizeEveryXSeconds(float x)
        //{
        //    for (int i = 0; i < 5; i++)
        //    {
        //        DoGetRandom();
        //        yield return new WaitForSeconds(x);
        //    }
        //    yield return null;
        //}
        public void DoGetRandom()
        {

            System.Random random = new System.Random();
            doors = doors.OrderBy(x => random.Next()).ToArray();
           // foreach (var i in arr)
            {
                Debug.Log(" The random sequence is " + doors[0] + " " + doors[1] + " " + doors[2]);
            }
        }
        //private void OnDisable()
        //{
        //    StopAllCoroutines();
        //}

    }
}
