using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//using UnityEngine.InputSystem.EnhancedTouch;

public class ActOnTouch : MonoBehaviour, IDragHandler
   //  , IPointerClickHandler  , IInitializePotentialDragHandler // Touch Testing only for now 
{
    AudioManager audioManager;
    Camera cam;
    Vector3 point, newPoint;
    GameObject cubeGameLeftWall, cubeGameRightWall;

    float yPositionFixed, zPositionFixed, xPositionFixed, camCubeXDelta, yPositionTopLimit,
        zPositionRightLimit, zPositionLeftLimit, movingCubeSizeX ;
    // Start is called before the first frame update
    void Start()
    {
        cubeGameLeftWall = GameObject.Find("CubeGameLeftWall");
        cubeGameRightWall = GameObject.Find("CubeGameRightWall");
        cam = Camera.main;
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        xPositionFixed = transform.position.x;  //we apparently need one of these fixed positions depending on cam/*cube orientation
        yPositionFixed = transform.position.y;  // *cube as in the thing(s) we want to drag 
        zPositionFixed = transform.position.z;
        camCubeXDelta = cam.transform.position.x - transform.position.x; //for ScreenToWorldPoint on drag - why? who knows
        yPositionTopLimit = 16f;//hard coded for now - should eventually be a graphic 
        zPositionLeftLimit = cubeGameLeftWall.transform.position.z;//
        zPositionRightLimit = cubeGameRightWall.transform.position.z;// 
        movingCubeSizeX = transform.localScale.x;
        Debug.Log(this.name + " position is " + transform.position + " movingCubeSizeX = " + movingCubeSizeX);// yes as expected 
    }
    //void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    //{
    //    audioManager.PlayAudio(audioManager.clipapert);

    //    Debug.Log("Object touched " + eventData.pointerCurrentRaycast);
    //}
    //void IPointerMoveHandler.OnPointerMove(PointerEventData eventData)
    //{
    //    Debug.Log("eData " + eventData.dragging);
    //}
    void IDragHandler.OnDrag(PointerEventData eventData)
    {

        point = cam.ScreenToWorldPoint
            (new Vector3(eventData.position.x, eventData.position.y, cam.nearClipPlane +camCubeXDelta));//

        newPoint = 
            new Vector3(xPositionFixed, Mathf.Clamp( point.y, yPositionFixed, yPositionTopLimit),
                                        Mathf.Clamp( point.z, zPositionLeftLimit + movingCubeSizeX / 2,
                                                              zPositionRightLimit - movingCubeSizeX / 2)); //movingCubeSizeX / 2?
        transform.position = newPoint;
        //if (newPoint.y >= yPositionFixed && newPoint.y <= yPositionTopLimit)
        //{
        //   transform.position = newPoint;
        //}

        //   Debug.Log("TPos = " + transform.position + "  newPoint.z = " + newPoint.z + " CamNCP = " + cam.nearClipPlane);
    }
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
