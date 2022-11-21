using System;
using System.Linq;
using UnityEngine;
using System.Security.Cryptography;

namespace randomize_array
{
    public class RandomTest : MonoBehaviour
    {
            int[] arr = { 1, 2, 3 };
        //static void Main(string[] args)
        private void Start()
        {
            for (int i = 0; i < 5; i++)
            {
                DoGetRandom();
            }
        }
        public void DoGetRandom()
        {

            System.Random random = new System.Random();
            arr = arr.OrderBy(x => random.Next()).ToArray();
           // foreach (var i in arr)
            {
                Debug.Log(arr[0] + " " + arr[1] + " " + arr[2]);
            }
        } 
    }
}
