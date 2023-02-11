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
{
    public MontyDoorTouchEvent montyDoorTouchEvent;
    public CinemachineVirtualCamera cVCam;
    Camera cam;
    float yPositionFixed, zPositionFixed, xPositionFixed, camCubeXDelta, yPositionTopLimit,
    zPositionRightLimit, zPositionLeftLimit, movingCubeSizeX;
    // Start is called before the first frame update
    void Start()
    {
      //  Debug.Log("Hello from ActOnMontyDoorTouch my name is " + this.name);
      //  AlignCam(); //with the on-standby vcam which will "become" cam/Camera.main when it goes Live
    }
    void AlignCam()  //moved from Start in prep to align ONLY when player enters MontyGame- else where the player starts is an issue 
    {
        cam = Camera.main;  //
        xPositionFixed = transform.position.x;  //we apparently need one of these fixed positions depending on cam/*cube orientation
        yPositionFixed = transform.position.y;  // *cube as in the thing(s) we want to drag 
        zPositionFixed = transform.position.z;
        camCubeXDelta = cVCam.transform.position.x - transform.position.x; //for ScreenToWorldPoint - use pos X of cVCam which will "become" cam when Live
        //Debug.Log("camCubeXDelta = " + camCubeXDelta);
        //yPositionTopLimit = cubeGameTopWall.transform.position.y - transform.localScale.y / 2;
        //zPositionLeftLimit = cubeGameLeftWall.transform.position.z;//
        //zPositionRightLimit = cubeGameRightWall.transform.position.z;// 
        movingCubeSizeX = transform.localScale.x;
    }
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        // audioManager.PlayAudio(audioManager.clipapert);
        if (MontyStopTrigger.montyGameActive)  //send touch event to MontyStopTrigger else ignore
        {
            Debug.Log("Object touched IPointerEnterHandler.OnPointerEnter " + eventData.pointerCurrentRaycast);
            montyDoorTouchEvent.Invoke(IntegerToSend(this.gameObject));
        }

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
