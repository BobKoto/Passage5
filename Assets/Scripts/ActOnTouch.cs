using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ActOnTouch : MonoBehaviour, IDragHandler ,  IPointerExitHandler, IPointerEnterHandler
//  , IPointerClickHandler  , IInitializePotentialDragHandler, IPointerUpHandler, // Touch Testing only for now 
// Component of CubeNN objects 
{
    AudioManager audioManager;
    Camera cam;
    Vector3 point, newPoint;
    GameObject cubeGameLeftWall, cubeGameRightWall, cubeGameTopWall;

    float yPositionFixed, zPositionFixed, xPositionFixed, camCubeXDelta, yPositionTopLimit,
        zPositionRightLimit, zPositionLeftLimit, movingCubeSizeX ;
    public FingerPointerEvent fingerPointerEvent;
    // Start is called before the first frame update
    void Start()
    {
        cubeGameLeftWall = GameObject.Find("CubeGameLeftWall");
        cubeGameRightWall = GameObject.Find("CubeGameRightWall");
        cubeGameTopWall = GameObject.Find("CubeGameTopWall");
        cam = Camera.main;  //thankfully this finds the Cinemachine VCam
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        xPositionFixed = transform.position.x;  //we apparently need one of these fixed positions depending on cam/*cube orientation
        yPositionFixed = transform.position.y;  // *cube as in the thing(s) we want to drag 
        zPositionFixed = transform.position.z;
        camCubeXDelta = cam.transform.position.x - transform.position.x; //for ScreenToWorldPoint on drag - why? who knows
        //yPositionTopLimit = 22f;//hard coded for now - should eventually be a graphic -- DONE on next line 
        yPositionTopLimit = cubeGameTopWall.transform.position.y - transform.localScale.y /2;
        zPositionLeftLimit = cubeGameLeftWall.transform.position.z;//
        zPositionRightLimit = cubeGameRightWall.transform.position.z;// 
        movingCubeSizeX = transform.localScale.x;
      //  Debug.Log(this.name + " position is " + transform.position + " movingCubeSizeX = " + movingCubeSizeX);// yes as expected 

    }

    void IDragHandler.OnDrag(PointerEventData eventData)  //Note: movement depends on camera orientation (affects transform.positions) 
    {
        point = cam.ScreenToWorldPoint
            (new Vector3(eventData.position.x, eventData.position.y, cam.nearClipPlane +camCubeXDelta));//

        newPoint =                                       // here we remap so Y is up/down & Z is left/right (don't try this at home)
            new Vector3(xPositionFixed, Mathf.Clamp( point.y, yPositionFixed, yPositionTopLimit),
                                        Mathf.Clamp( point.z, zPositionLeftLimit + movingCubeSizeX / 2,
                                                              zPositionRightLimit - movingCubeSizeX / 2)); //movingCubeSizeX / 2?
        transform.position = newPoint;
    }
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
      //  Debug.Log("Pointer ENTERED!!!! this.name = " + this.name); // + " dragging? " + eventData.dragging);
    }
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)   //player took finger off cube
    {
        //Debug.Log
        //    ("AOTouch Pointer EXITED!! this.name = " + this.name + " Invoke event AND isPointerMoving = " + eventData.IsPointerMoving());
        // send an event to CubeGameHandler? to lock cube?
        var fingerMoving = eventData.IsPointerMoving();
        if (!fingerMoving) fingerPointerEvent.Invoke(this.name, "finger UP");
    }
    //void IPointerUpHandler.OnPointerUp(PointerEventData eventData)  //doesn't register 
    //{
    //    Debug.Log("Pointer went up!!!!");
    //}

    //void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    //{
    //    audioManager.PlayAudio(audioManager.clipapert);

    //    Debug.Log("Object touched " + eventData.pointerCurrentRaycast);
    //}
    //void IPointerMoveHandler.OnPointerMove(PointerEventData eventData)
    //{
    //    Debug.Log("eData " + eventData.dragging);
    //}
    //void IDragHandler.OnDrag(PointerEventData eventData)  //this doohickey was for movables FLAT on the floor (bad idea)
    //{
    //    point = cam.ScreenToWorldPoint
    //        (new Vector3(eventData.position.x, eventData.position.y, cam.transform.position.y));//try to maintain even if cam Y changes 
    //  //  (new Vector3(eventData.position.x, eventData.position.y, cam.nearClipPlane + 70));//works w/cam Y pos = 70 & camNCP @zero
    //    newPoint = new Vector3(point.x, yPositionFixed, point.z );
    //    transform.position = newPoint;
    //   Debug.Log("TPos = " + transform.position + "  newPoint.z = " + newPoint.z + " CamNCP = " + cam.nearClipPlane);
    //}
    //void IInitializePotentialDragHandler.OnInitializePotentialDrag(PointerEventData eventData)
    //{
    //    Debug.Log("IInitializePotentialDragHandler.OnInitializePotentialDrag dragging is " + eventData.dragging);
    //}
    // Update is called once per frame
    //void Update()
    //{

    //}
    //protected void OnEnable()
    //{
    //    Debug.Log(" EnhancedTouchSupport.Enable()");
    //    EnhancedTouchSupport.Enable();
    //}
    //protected void OnDisable()
    //{
    //    EnhancedTouchSupport.Disable();
    //}
}
        //  Debug.Log
        //     ("IDragHandler.OnDrag " + eventData.position + " XPos " + eventData.position.x + " YPos " + eventData.position.y); //showing screen position 
