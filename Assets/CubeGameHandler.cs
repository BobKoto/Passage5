using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

[System.Serializable]
public class CubeGameBoardEvent : UnityEvent<string, string, string, int>   //this declaration i guess is needed to accept
{                                                                            //other scripts Invokes - unsure about params we need
}
public class CubeGameHandler : MonoBehaviour
{
    public CubeGameBoardEvent cubeGameBoardEvent;  //empty class declared above - before this class 
    GameObject topText;
    GameObject bottomText;
    TMP_Text topRowText;
    TMP_Text bottomRowText;
    // Start is called before the first frame update
    void Start()
    {
     if (cubeGameBoardEvent == null) cubeGameBoardEvent = new CubeGameBoardEvent();  //not sure but it stopped the null reference 
        cubeGameBoardEvent.AddListener(CubeEnteredOrLeft);
        topRowText = GameObject.Find("TopRow").GetComponent<TMP_Text>();
        bottomText = GameObject.Find("BottomRow");
       // TMP_Text = GameObject.Find("")
        topRowText.text = "Game On!";
    }
    public void CubeEnteredOrLeft(string s1, string s2, string s3, int y)   //event Invoked by CubeEnteredSolutionMatrix
    {
        Debug.Log("event recvd: " + s1 + " " + s2 + s3 + " intY " + y);





    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
