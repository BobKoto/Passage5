using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Cinemachine;
using UnityEngine;

public class ActOnMontyDoorTouch : MonoBehaviour, IPointerEnterHandler
//IDragHandler, IEndDragHandler, IPointerUpHandler, IPointerExitHandler, IPointerClickHandler
//IPointerClickHandler, IInitializePotentialDragHandler, IPointerUpHandler,IPointerExitHandler, IPointerEnterHandler, IDropHandler
// Touch Testing only for now 
// Component of MontySlidingDoorN objects  when N is 1,2 or 3
// and Component of MontyPlayButton (in MontyGameIntro parent)
// and Component of MontyGameMoveOnButton
{
    public MontyDoorTouchEvent montyDoorTouchEvent;
    public MontyPlayButtonTouchEvent montyPlayButtonTouchEvent;
    //public MontyMoveOnButtonTouchEvent montyMoveOnButtonTouchEvent;   //DeImp on 5/7/23
    public MontyDoorDownEvent montyDoorDownEvent;

    public CinemachineVirtualCamera cVCam;

    // Start is called before the first frame update
    void Start()
    {
       // Debug.Log("Hello from ActOnMontyDoorTouch my name is " + this.name);
    }
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)  //a user touch(click)
    {
        switch (this.gameObject.name)
        {
            case "MontyPlayButton":
             //   Debug.Log("Object/PLAYBUTTON touched IPointerEnterHandler.OnPointerEnter " + eventData.pointerCurrentRaycast);
                montyPlayButtonTouchEvent.Invoke();
                break;
            case "MontySlidingDoor1":
            case "MontySlidingDoor2" :
            case "MontySlidingDoor3":
             //   Debug.Log("Object/DOOR touched IPointerEnterHandler.OnPointerEnter " + eventData.pointerCurrentRaycast);
                if (MontyStopTrigger.montyGameActive)  //if we're NOT waiting for the 1st animation to end 
                {
                    montyDoorTouchEvent.Invoke(IntegerToSend(this.gameObject));
                }
                break;
            //case "MontyGameMoveOnButton":     //DeImp on 5/7/23
            // //   Debug.Log("Object/MOVEonBUTTON touched IPointerEnterHandler.OnPointerEnter " + eventData.pointerCurrentRaycast);
            //    montyMoveOnButtonTouchEvent.Invoke();
            //    break;
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
        //Debug.Log(this.name + "  received Animation event received by MontyDoorEvent... " + message);
        //if (message.Equals("Door1DownFinished"))
        //{
        //    // Do other things based on an animation ending.
        //    Debug.Log(this.name + "  received Animation event received by MontyDoorEvent... " + message);
        //}
        int x = DoorNumberToSend(); //inspector STATIC parameters override x so BE SURE to select dynamic (in this case either could work) 
        montyDoorDownEvent.Invoke(x);   //we did try to use DoorNumberToSend() instead of x 
        //Debug.Log("Invoked event with door # to send... " + DoorNumberToSend());
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
    //void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    //{
    //  //  Debug.Log("Pointer ENTERED!!!! this.name = " + this.name); // + " dragging? " + eventData.dragging);
    //}
    //
    //void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    //{
    //   // audioManager.PlayAudio(audioManager.clipapert);

    //  //  Debug.Log("Object touched IPointerClickHandler.OnPointerClick " + eventData.pointerCurrentRaycast);
    //}
    //void IPointerExitHandler.OnPointerExit(PointerEventData eventData)   //player took finger off cube
    //{
    //  //  Debug.Log("Object touched IPointerExitHandler.OnPointerExit " + eventData.pointerCurrentRaycast);
    //}
} // end class 
