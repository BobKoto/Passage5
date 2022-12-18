using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

[System.Serializable]
public class CubeGameBoardEvent : UnityEvent<string, bool, string, int>   //this declaration i guess is needed to accept
{                                                                            //other scripts Invokes - unsure about params we need
}
public class CubeGameHandler : MonoBehaviour
//Component of CubeGame -- receives events from CubeEnteredSolutionMatrix.cs  -- calculates row/column totals
//Here we need to figure if game is lost or won 
//How we do this is to seed the row/column with target sums that can or cannot be achieved to = 100
//So we need to add Texts(numeric values) to serve as targets 
//Some (randomly set) targets CAN be achieved while others cannot - therein lies our puzzle?
{
    CubeGameBoardEvent cubeGameBoardEvent;  //empty class declared above - before this class // took away public see line 31 
    GameObject row1Sum, row2Sum, col1Sum, col2Sum ;

    TMP_Text row1SumText,row2SumText, col1SumText, col2SumText ;
 
//    public GameObject[] cubePlaceHolder;  //this array of gameObjects did nothng 
    public AudioManager audioManager;
    bool cubePlaceHolder1Taken, cubePlaceHolder2Taken, cubePlaceHolder3Taken, cubePlaceHolder4Taken;
    int place1CubeValue, place2CubeValue, place3CubeValue, place4CubeValue;
    // Start is called before the first frame update
    void Start()
    {
       // Debug.Log("we have " + cubePlaceHolder.Length + " placeholders ");
     if (cubeGameBoardEvent == null) cubeGameBoardEvent = new CubeGameBoardEvent();  //not sure but it stopped the null reference 
        cubeGameBoardEvent.AddListener(CubeEnteredOrLeft);
        row1SumText = GameObject.Find("Row1Sum").GetComponent<TMP_Text>();
        row2SumText = GameObject.Find("Row2Sum").GetComponent<TMP_Text>();
        col1SumText = GameObject.Find("Col1Sum").GetComponent<TMP_Text>();
        col2SumText = GameObject.Find("Col2Sum").GetComponent<TMP_Text>();

        row1SumText.text = "0";
        row2SumText.text = "0";
        col1SumText.text = "0";
        col2SumText.text = "0";

        if (!audioManager) audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
    }
    public void CubeEnteredOrLeft(string cubeName, bool _entered, string placeName, int cubeValue)   //event Invoked by CubeEnteredSolutionMatrix
    {
        // Debug.Log("event recvd: " + cubeName + " " + _entered + " " + placeName + " cubeValue = " + cubeValue);
        switch (placeName)
        {
            case "CubePlacement1":
                cubePlaceHolder1Taken = _entered;
                place1CubeValue = cubePlaceHolder1Taken ? cubeValue : 0;
                break;
            case "CubePlacement2":
                cubePlaceHolder2Taken = _entered;
                place2CubeValue = cubePlaceHolder2Taken ? cubeValue : 0;
                break;
            case "CubePlacement3":
                cubePlaceHolder3Taken = _entered;
                place3CubeValue = cubePlaceHolder3Taken ? cubeValue : 0;
                break;
            case "CubePlacement4":
                cubePlaceHolder4Taken = _entered;
                place4CubeValue = cubePlaceHolder4Taken ? cubeValue : 0;
                break;
            default: break;
        }
        CalculateTheMatrix(placeName);
    }
    void CalculateTheMatrix(string placeName)  //can use params here?? yes but how?
    {
            int topRowTotal = place1CubeValue + place2CubeValue;
            row1SumText.text = topRowTotal.ToString();

            int col1Total = place1CubeValue + place3CubeValue;
            col1SumText.text = col1Total.ToString();

            int secondRowTotal = place3CubeValue + place4CubeValue;
            row2SumText.text = secondRowTotal.ToString();

            int col2Total = place2CubeValue + place4CubeValue;
            col2SumText.text = col2Total.ToString();
    }
}  // end class 

//switch (cubeName)  //redundant code removed from CubeEnteredOrLeft(...) we had a case for each place
//{
//    case "Cube10":
//        place1CubeValue = 10;
//        break;
//    case "Cube20":
//        place1CubeValue = 20;
//        break;
//    case "Cube30":
//        place1CubeValue = 30;
//        break;
//    case "Cube40":
//        place1CubeValue = 40;
//        break;
//    default:break;
//}
//void AssignValueToPlaceHolder(string cubeName, int placeCubeValue, bool entered)  //also redundant
//{
//    if (entered)
//    {
//        switch (cubeName)
//        {
//            case "Cube10":
//                placeCubeValue = 10;
//                break;
//            case "Cube20":
//                placeCubeValue = 20;
//                break;
//            case "Cube30":
//                placeCubeValue = 30;
//                break;
//            case "Cube40":
//                placeCubeValue = 40;
//                break;
//            default: break;
//        }
//    }
//    Debug.Log("AssignValue says cubename " + cubeName + " placeCubeValue  " + placeCubeValue);
//}
// original     void CalculateTheMatrix()  quaddruple slashes are original comment-outs 
// Debug.Log("Now calculate the Matrix ....");
//// if (cubePlaceHolder1Taken && cubePlaceHolder2Taken)    //add 1st row across
//{
//    int topRowTotal = place1CubeValue + place2CubeValue;
//    //   Debug.Log("the first row is " + topRowTotal);
//    row1SumText.text = topRowTotal.ToString();
//}
//// if (cubePlaceHolder1Taken && cubePlaceHolder3Taken) //add 1st column down
//{
//    int col1Total = place1CubeValue + place3CubeValue;
//    //   Debug.Log("the first column is " + col1Total);
//    col1SumText.text = col1Total.ToString();
//}
////  if (cubePlaceHolder3Taken && cubePlaceHolder4Taken) //add 2nd row across 
//{
//    int secondRowTotal = place3CubeValue + place4CubeValue;
//    //    Debug.Log("2nd row = " + secondRowTotal + " place3 " + place3CubeValue + " place4 " + place4CubeValue);
//    row2SumText.text = secondRowTotal.ToString();
//}
//// if (cubePlaceHolder2Taken && cubePlaceHolder4Taken) //add 2nd column down 
//{
//    int col2Total = place2CubeValue + place4CubeValue;
//    //      Debug.Log("the 2nd column is " + col2Total);
//    col2SumText.text = col2Total.ToString();
//}
//       // Debug.Log("");
//    }

