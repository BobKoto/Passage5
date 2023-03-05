using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CubeGameBoard : MonoBehaviour
    //Component of CubeGame
{
    public CubeGameBoardUpEvent cubeGameBoardUpEvent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCubeGameBoardIsUp()
    {
        Debug.Log("CubeGameBoard.cs SAYS the cube game board is up !!!!!");
        //here we send an event to AOTouch to then call AlignCam()  and to CGHandler to then call OnCubeGameBoardUpStore...
        cubeGameBoardUpEvent.Invoke();
    }
}
