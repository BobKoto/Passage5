using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using UnityEngine.Events;

public class CubeEnteredSolutionMatrix : MonoBehaviour    // invokes Events on Exits as well 
{
    //GameObject cube10, cube20, cube30, cube40; //don't need because we obtain name IDs from OnTriggers
    //GameObject topText;c
    //GameObject bottomText;
    //TMP_Text topRowText;
    //TMP_Text bottomRowText;

    // public CubeGameBoardEvent cubeGameBoardEvent;

    public CubeGameBoardEvent cubeGameBoardEvent;
    bool alreadyOccupied;
    string placeOccupant;
    // Start is called before the first frame update
    void Start()
    {
        //cube10 = GameObject.Find("Cube10");//don't need because we obtain name IDs from OnTriggers
        //cube20 = GameObject.Find("Cube20");
        //cube30 = GameObject.Find("Cube30");
        //cube30 = GameObject.Find("Cube40");

        //topText = GameObject.Find("TopRow");don't need because
        //bottomText = GameObject.Find("BottomRow");
        //topRowText.text = "start top";
        //bottomRowText.text = "start bottom";

        //if (cubeGameBoardEvent == null) //syntaxes on other.name (duh) - triggers work so figure it out some other time :|
        //   {   
        //    cubeGameBoardEvent = new CubeGameBoardEvent().Invoke(other.name, " Entered   ", this.name, 20); 
        //    cubeGameBoardEvent => CubeGameHandler.
        //    }

        //   Debug.Log("debug.log Invoke cubeGameBoardEvent here......." );
        //  cubeGameBoardEvent.Invoke(this.name, "string2", 10, 20);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (!alreadyOccupied)  //then set the value moved into this - otherwise do nothing 
        {
            int valueToSend = CubeValue(other.name);
            switch (this.name)
            {
                case "CubePlacement1":
                    cubeGameBoardEvent.Invoke(other.name, true, this.name, valueToSend);
                    break;
                case "CubePlacement2":
                    cubeGameBoardEvent.Invoke(other.name, true, this.name, valueToSend);
                    break;
                case "CubePlacement3":
                    cubeGameBoardEvent.Invoke(other.name, true, this.name, valueToSend);
                    break;
                case "CubePlacement4":
                    cubeGameBoardEvent.Invoke(other.name, true, this.name, valueToSend);
                    break;
                default: break;
            }
            // Debug.Log(this.name + " entered by " + other);
            alreadyOccupied = true;
            placeOccupant = other.name;
        }
    }
    private void OnTriggerExit(Collider other)
    {
       // Debug.Log(this.name + " exited by " + other + " the occupant is " + placeOccupant);
        if (other.name == placeOccupant) //original cube intentionally dragged out or (physically pushed out by another?) 
        {
            alreadyOccupied = false;
            int valueToSend = CubeValue(other.name);
            switch (this.name)
            {
                case "CubePlacement1":
                    //   Debug.Log(this.name + " exited by " + other);
                    cubeGameBoardEvent.Invoke(other.name, false, this.name, valueToSend);
                    break;
                case "CubePlacement2":
                    //   Debug.Log(this.name + " exited by " + other);
                    cubeGameBoardEvent.Invoke(other.name, false, this.name, valueToSend);
                    break;
                case "CubePlacement3":
                    //   Debug.Log(this.name + " exited by " + other);
                    cubeGameBoardEvent.Invoke(other.name, false, this.name, valueToSend);
                    break;
                case "CubePlacement4":
                    //    Debug.Log(this.name + " exited by " + other);
                    cubeGameBoardEvent.Invoke(other.name, false, this.name, valueToSend);
                    break;
                default: break;
            }
        }

    }
    public int CubeValue(string cubeMovedInOrOut)
    {
        //int valueToSend = CubeValue(cubeMovedInOrOut); caused a stackOverFlow? btw it's wrong anyway
        return cubeMovedInOrOut switch         //NEW FANGLED CODE courtesy of Visual Studio
        {
            "Cube10" => 10,
            "Cube20" => 20,
            "Cube30" => 30,
            "Cube40" => 40,
            _ => 0,
        };
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
