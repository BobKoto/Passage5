using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using UnityEngine.Events;

public class CubeEnteredSolutionMatrix : MonoBehaviour
{
    GameObject cube10, cube20, cube30, cube40;
    GameObject topText;
    GameObject bottomText;
    TMP_Text topRowText;
    TMP_Text bottomRowText;

    // public CubeGameBoardEvent cubeGameBoardEvent;
    public CubeGameBoardEvent cubeGameBoardEvent;

    // Start is called before the first frame update
    void Start()
    {
        cube10 = GameObject.Find("Cube10");
        cube20 = GameObject.Find("Cube20");
        cube30 = GameObject.Find("Cube30");
        cube30 = GameObject.Find("Cube40");

        topText = GameObject.Find("TopRow");
        bottomText = GameObject.Find("BottomRow");
        //topRowText.text = "start top";
        //bottomRowText.text = "start bottom";
     //   if (cubeGameBoardEvent == null) cubeGameBoardEvent = new CubeGameBoardEvent();
     //   Debug.Log("debug.log Invoke cubeGameBoardEvent here......." );
      //  cubeGameBoardEvent.Invoke(this.name, "string2", 10, 20);

    }
    private void OnTriggerEnter(Collider other)
    {
        switch (this.name)
        {
            case "CubePlacement1":
            //    Debug.Log(this.name + " entered by " + other);
                cubeGameBoardEvent.Invoke(other.name, " Entered   ", this.name, 20);
                break;
            case "CubePlacement2":
            //    Debug.Log(this.name + " entered by " + other);
                cubeGameBoardEvent.Invoke(other.name, " Entered   ", this.name, 20);
                break;
            case "CubePlacement3":
            //    Debug.Log(this.name + " entered by " + other);
                cubeGameBoardEvent.Invoke(other.name, " Entered   ", this.name, 20);
                break;
            case "CubePlacement4":
            //    Debug.Log(this.name + " entered by " + other);
                cubeGameBoardEvent.Invoke(other.name, " Entered   ", this.name, 20);
                break;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        switch (this.name)
        {
            case "CubePlacement1":
             //   Debug.Log(this.name + " exited by " + other);
                cubeGameBoardEvent.Invoke(other.name, "  Exited     ", this.name, 20);
                break;
            case "CubePlacement2":
            //   Debug.Log(this.name + " exited by " + other);
                cubeGameBoardEvent.Invoke(other.name, "  Exited     ", this.name, 20);
                break;
            case "CubePlacement3":
            //   Debug.Log(this.name + " exited by " + other);
                cubeGameBoardEvent.Invoke(other.name, "  Exited     ", this.name, 20);
                break;
            case "CubePlacement4":
            //    Debug.Log(this.name + " exited by " + other);
                cubeGameBoardEvent.Invoke(other.name, "  Exited     ", this.name, 20);
                break;

        }
    }
    //// Update is called once per frame
    //void Update()
    //{

    //}
}
