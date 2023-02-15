using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Cinemachine;
using UnityEngine;

public class ActOnMontyDoorTouch : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
//IDragHandler, IEndDragHandler, IPointerUpHandler, IPointerExitHandler, IPointerClickHandler
//IPointerClickHandler, IInitializePotentialDragHandler, IPointerUpHandler,IPointerExitHandler, IPointerEnterHandler, IDropHandler
// Touch Testing only for now 
// Component of MontySlidingDoorN objects  when N is 1,2 or 3
// and Component of MontyPlayButton 
{
    public MontyDoorTouchEvent montyDoorTouchEvent;
    public MontyPlayButtonTouchEvent montyPlayButtonTouchEvent;
    public MontyMoveOnButtonTouchEvent montyMoveOnButtonTouchEvent;
    public CinemachineVirtualCamera cVCam;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hello from ActOnMontyDoorTouch my name is " + this.name);
      //  AlignCam(); //with the on-standby vcam which will "become" cam/Camera.main when it goes Live
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        switch (this.gameObject.name)
        {
            case "MontyPlayButton":
                Debug.Log("Object/PLAYBUTTON touched IPointerEnterHandler.OnPointerEnter " + eventData.pointerCurrentRaycast);
                montyPlayButtonTouchEvent.Invoke();
                break;
            case "MontySlidingDoor1": case "MontySlidingDoor2" : case "MontySlidingDoor3":
                Debug.Log("Object/DOOR touched IPointerEnterHandler.OnPointerEnter " + eventData.pointerCurrentRaycast);
                if (MontyStopTrigger.montyGameActive)
                {
                    montyDoorTouchEvent.Invoke(IntegerToSend(this.gameObject));
                }
                break;
            case "MontyGameMoveOnButton":
                Debug.Log("Object/MOVEonBUTTON touched IPointerEnterHandler.OnPointerEnter " + eventData.pointerCurrentRaycast);
                montyMoveOnButtonTouchEvent.Invoke();
                break;
            default:  
                Debug.Log("ActOnMontyDoorTouch DEFAULTED!!!");
                //if (MontyStopTrigger.montyGameActive)
                //{
                //    montyDoorTouchEvent.Invoke(IntegerToSend(this.gameObject));
                //}
                break;
        }
        //if (MontyStopTrigger.montyGameActive)  //send touch event to MontyStopTrigger else ignore
        //{
        //    Debug.Log("Object/DOOR touched IPointerEnterHandler.OnPointerEnter " + eventData.pointerCurrentRaycast);
        //    montyDoorTouchEvent.Invoke(IntegerToSend(this.gameObject));
        //    return;
        //}
        //else  //only works because intro panel is blocking the doors - lazy and shitty but...
        //if (this.gameObject.name == "MontyPlayButton")
        //{
        //    Debug.Log("Object/PLAYBUTTON touched IPointerEnterHandler.OnPointerEnter " + eventData.pointerCurrentRaycast);
        //    montyPlayButtonTouchEvent.Invoke();
        //}

    }
    int IntegerToSend(GameObject gO)
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
    //public void AlertObservers(string message)  //this was for testing on 2/14/23 and I hope no longer needed 
    //{
    //    if (message.Equals("Door1DownFinished"))
    //    {
    //        //pc_attacking = false;
    //        //pc_anim.SetBool("attack", false);
    //        // Do other things based on an attack ending.
    //        Debug.Log("Animation event received by ActOnMontyTouch... " + message);
    //    }
    //}
    //void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    //{
    //  //  Debug.Log("Pointer ENTERED!!!! this.name = " + this.name); // + " dragging? " + eventData.dragging);
    //}

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
       // audioManager.PlayAudio(audioManager.clipapert);

      //  Debug.Log("Object touched IPointerClickHandler.OnPointerClick " + eventData.pointerCurrentRaycast);
    }
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)   //player took finger off cube
    {
      //  Debug.Log("Object touched IPointerExitHandler.OnPointerExit " + eventData.pointerCurrentRaycast);
    }
}
