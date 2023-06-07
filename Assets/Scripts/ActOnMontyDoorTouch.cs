using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Cinemachine;
using UnityEngine;

public class ActOnMontyDoorTouch : MonoBehaviour, IPointerEnterHandler
//IDragHandler, IEndDragHandler, IPointerUpHandler, IPointerExitHandler, IPointerClickHandler
//IPointerClickHandler, IInitializePotentialDragHandler, IPointerUpHandler,IPointerExitHandler, IPointerEnterHandler, IDropHandler
// Component of MontySlidingDoorN objects  when N is 1,2 or 3
{
    public MontyDoorTouchEvent montyDoorTouchEvent;
    public MontyPlayButtonTouchEvent montyPlayButtonTouchEvent;
    public MontyDoorDownEvent montyDoorDownEvent;
    public CinemachineVirtualCamera cVCam;
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)  //a user touch(click)
    {
        switch (this.gameObject.name)
        {
            case "MontySlidingDoor1":
            case "MontySlidingDoor2" :
            case "MontySlidingDoor3":
                if (MontyStopTrigger.montyGameAllowDoorTouch)  //if we're NOT waiting for the 1st animation to end 
                {
                    montyDoorTouchEvent.Invoke(IntegerToSend(this.gameObject));
                }
                break;
            default:  
                Debug.Log("ActOnMontyDoorTouch DEFAULTED!!!");
                break;
        }
    }
    int IntegerToSend(GameObject gO)   //maybe redundant w/DoorNumberToSend()
    {
        int _doorNumber = 0;
        switch (gO.name)
        {
            case ("MontySlidingDoor1"):
                _doorNumber = 1;
                break;
            case ("MontySlidingDoor2"):
                _doorNumber = 2;
                break;
            case ("MontySlidingDoor3"):
                _doorNumber = 3;
                break;
        }
        return _doorNumber;
    }
    public void AlertObservers(string message)
    {
        int x = DoorNumberToSend(); //inspector STATIC parameters override x so BE SURE to select dynamic (in this case either could work) 
        montyDoorDownEvent.Invoke(x);   //we did try to use DoorNumberToSend() instead of x 
    }
    int DoorNumberToSend()
    {
        int _doorNumber = 88;
        switch (this.name)
        {
            case "MontySlidingDoor1":
                _doorNumber = 1;
                break;
            case "MontySlidingDoor2":
                _doorNumber = 2;
                break;
            case "MontySlidingDoor3":
                _doorNumber = 3;
                break;
        }
        return _doorNumber;
    }
} // end class 
