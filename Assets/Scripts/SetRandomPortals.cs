using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRandomPortals : MonoBehaviour
{//Component of SelectARandomGameTrigger
    private int[] numbers = { 0, 1, 2, 3, 4, 5 }; //, 5, 6, 7, 8, 9, 10, 11, 12 };

    public int[] GetRandomNumbers(int count)
    {
        List<int> remainingNumbers = new List<int>(numbers);
        List<int> randomNumbers = new List<int>();

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, remainingNumbers.Count);
            int randomNumber = remainingNumbers[index];
            remainingNumbers.RemoveAt(index);
            randomNumbers.Add(randomNumber);
        }

        return randomNumbers.ToArray();
    }
}
//private void Start()
//{
//    Debug.Log(" Hello from RandomNumberGenerator");
//    int[] randomNumbers = GetRandomNumbers(3);

//    foreach (int number in randomNumbers)
//    {
//        Debug.Log(number);
//    }
//}
