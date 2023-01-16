using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

[System.Serializable]
public class CubeGameBoardEvent : UnityEvent<string, bool, string, int> { }  //this declaration i guess is needed to accept

public class CubeGameHandler : MonoBehaviour
//Component of CubeGame -- receives events from CubeEnteredSolutionMatrix.cs(was)/is now PlacementHandler  -- calculates row/column totals
//Here we need to figure if game is lost or won 
//How we do this is to seed the row/column with target sums that can or cannot be achieved to = 100
//So we need to add Texts(numeric values) to serve as targets 
//Some (randomly set) targets CAN be achieved while others cannot - therein lies our puzzle?
{
    public CubeGameBoardEvent cubeGameBoardEvent;  //empty class declared above - before this class // took away public see line 31 
    GameObject row1Sum, row2Sum, col1Sum, col2Sum ;

    TMP_Text row1SumText,row2SumText, col1SumText, col2SumText ;

    GameObject inputControls;
    public AudioManager audioManager;
    bool cubePlaceHolder1Taken, cubePlaceHolder2Taken, cubePlaceHolder3Taken, cubePlaceHolder4Taken;
    int place1CubeValue, place2CubeValue, place3CubeValue, place4CubeValue;
    int cubesOccupied;
    // Start is called before the first frame update
    void Start()
    {
        inputControls = GameObject.Find("Joysticks_StarterAssetsInputs_Joysticks");
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
    public void CubeEnteredOrLeft(string cubeName, bool _entered, string placeName, int cubeValue)
        //event was Invoked Sucessfully by CubeEnteredSolutionMatrix - now ? 
    {
        cubesOccupied = _entered? cubesOccupied+=1 : cubesOccupied-=1;
        //Debug.Log("WE Have " + cubesOccupied);
        //  Debug.Log("CGH event recvd: " + cubeName + " " + _entered + " " + placeName + " cubeValue = " + cubeValue);
        switch (placeName)
        {
            case "CubePlacement1":
                place1CubeValue = _entered ? cubeValue : 0;
                break;
            case "CubePlacement2":
                place2CubeValue = _entered ? cubeValue : 0;
                break;
            case "CubePlacement3":
                place3CubeValue = _entered ? cubeValue : 0;
                break;
            case "CubePlacement4":
                place4CubeValue = _entered ? cubeValue : 0;
                break;
            default: Debug.Log("CGHandler case got a default");
                break;
        }
        CalculateTheMatrix();
    }
    void CalculateTheMatrix()  //can use params here?? yes but how?
    {
        row1SumText.text = (place1CubeValue + place2CubeValue).ToString(); // + " added across";

        col1SumText.text = (place1CubeValue + place3CubeValue).ToString(); // + " added down";

        row2SumText.text = (place3CubeValue + place4CubeValue).ToString(); // + " added across" ;

        col2SumText.text = (place2CubeValue + place4CubeValue).ToString(); // + " added down"; 
        if (cubesOccupied == 4)  // the game is finished as player filled 4th placement 
        {
           // Debug.Log("WE Have 4, Should enable inputControls...");
            if (inputControls) inputControls.SetActive(true);
        }
    }
    private void OnDisable()
    {
        cubeGameBoardEvent.RemoveListener(CubeEnteredOrLeft);
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

