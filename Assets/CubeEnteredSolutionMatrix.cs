using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CubeEnteredSolutionMatrix : MonoBehaviour
{
    GameObject cube10, cube20, cube30, cube40;
    GameObject topText;
    GameObject bottomText;
    TMP_Text topRowText;
    TMP_Text bottomRowText;

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

    }
    private void OnTriggerEnter(Collider other)
    {
        switch (this.name)
        {
            case "CubePlacement1":
                Debug.Log(this.name + " entered by " + other);
               break;
            case "CubePlacement2":
                Debug.Log(this.name + " entered by " + other);
                break;
            case "CubePlacement3":
                Debug.Log(this.name + " entered by " + other);
                break;
            case "CubePlacement4":
                Debug.Log(this.name + " entered by " + other);
                break;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        switch (this.name)
        {
            case "CubePlacement1":
                Debug.Log(this.name + " exited by " + other);
                break;
            case "CubePlacement2":
                Debug.Log(this.name + " exited by " + other);
                break;
            case "CubePlacement3":
                Debug.Log(this.name + " exited by " + other);
                break;
            case "CubePlacement4":
                Debug.Log(this.name + " exited by " + other);
                break;

        }
    }
    //// Update is called once per frame
    //void Update()
    //{

    //}
}
