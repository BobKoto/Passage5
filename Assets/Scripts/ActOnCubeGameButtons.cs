using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Cinemachine;

public class ActOnCubeGameButtons : MonoBehaviour, IPointerEnterHandler
//IPointerClickHandler, IInitializePotentialDragHandler, IPointerUpHandler,IPointerExitHandler, IPointerEnterHandler, IDropHandler
// Touch Testing only for now 
// Component of Cube Game Play and MoveOn button gameobjects (not UI)
{

    public CubeGamePlayButtonTouchEvent cubeGamePlayButtonTouchEvent;
    public CubeGameMoveOnButtonTouchEvent cubeGameMoveOnButtonTouchEvent;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("hello from AOTOUCH");  //starts well before any issues
    }


    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)  //a user touch(click)
    {
        switch (this.gameObject.name)
        {
            case "CubeGamePlayButton":
                //   Debug.Log("Object/PLAYBUTTON touched IPointerEnterHandler.OnPointerEnter " + eventData.pointerCurrentRaycast);
                cubeGamePlayButtonTouchEvent.Invoke();
                break;
            case "CubeGameMoveOnButton":
                //   Debug.Log("Object/MOVEonBUTTON touched IPointerEnterHandler.OnPointerEnter " + eventData.pointerCurrentRaycast);
                cubeGameMoveOnButtonTouchEvent.Invoke();
                break;
            default:
                Debug.Log("ActOnCUbeGameButtons DEFAULTED!!! object = " + this.gameObject.name);
                break;
        }
    }
}