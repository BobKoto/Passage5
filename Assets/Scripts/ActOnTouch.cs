using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Cinemachine;
public class ActOnTouch : MonoBehaviour, IDragHandler,  IEndDragHandler
//IPointerClickHandler, IInitializePotentialDragHandler, IPointerUpHandler,IPointerExitHandler, IPointerEnterHandler, IDropHandler
// Component of CubeNN objects 
{
    Camera cam;
    public CinemachineVirtualCamera cVCam;
    Vector3 point, newPoint;
    GameObject cubeGameLeftWall, cubeGameRightWall, cubeGameTopWall;

    float yPositionFixed, zPositionFixed, xPositionFixed, camCubeXDelta, yPositionTopLimit,
        zPositionRightLimit, zPositionLeftLimit, movingCubeSizeX ;
    public FingerPointerEvent fingerPointerEvent;
    public CubeGameBoardUpEvent cubeGameBoardUpEvent;
    // Start is called before the first frame update
    void Start()
    {
        cubeGameLeftWall = GameObject.Find("CubeGameLeftWall");
        cubeGameRightWall = GameObject.Find("CubeGameRightWall");
        cubeGameTopWall = GameObject.Find("CubeGameTopWall");
        if (cubeGameBoardUpEvent == null )
            cubeGameBoardUpEvent = new CubeGameBoardUpEvent();
        cubeGameBoardUpEvent.AddListener(OnGameBoardUp);
    }
    public void OnGameBoardUp()
    {
        //Debug.Log("AOTouch recvd event for OnGameBoardUp  Calling AlignCam....");
        AlignCam();
    }
    void AlignCam()  //moved from Start in prep to align ONLY when player enters CubeGame - else where the player starts is an issue 
    {
        cam = Camera.main;  //
        xPositionFixed = transform.position.x;  //we apparently need one of these fixed positions depending on cam/*cube orientation
        yPositionFixed = transform.position.y;  // *cube as in the thing(s) we want to drag 
        zPositionFixed = transform.position.z;
        camCubeXDelta = cVCam.transform.position.x - transform.position.x; //for ScreenToWorldPoint - use pos X of cVCam which will "become" cam when Live

        yPositionTopLimit = cubeGameTopWall.transform.position.y - transform.localScale.y /2;
        zPositionLeftLimit = cubeGameLeftWall.transform.position.z;//
        zPositionRightLimit = cubeGameRightWall.transform.position.z;// 
        movingCubeSizeX = transform.localScale.x;
    }
    void IDragHandler.OnDrag(PointerEventData eventData)  //Note: movement depends on camera orientation (affects transform.positions) 
    {
        if (CubeGameHandler.cubeGameIsActive)  // added if clause 1/30/23 
        {
            point = cam.ScreenToWorldPoint
                (new Vector3(eventData.position.x, eventData.position.y, cam.nearClipPlane + camCubeXDelta));//

            newPoint =                                       // here we remap so Y is up/down & Z is left/right (don't try this at home)
                new Vector3(xPositionFixed, Mathf.Clamp(point.y, yPositionFixed, yPositionTopLimit),
                                            Mathf.Clamp(point.z, zPositionLeftLimit + movingCubeSizeX / 2,
                                                                  zPositionRightLimit - movingCubeSizeX / 2)); //movingCubeSizeX / 2?
            transform.position = newPoint;
        }
    }
    void IEndDragHandler.OnEndDrag(PointerEventData eventData)   //We don't consistently get this?, so try PointerUp/PointerExit
    {
        if (CubeGameHandler.cubeGameIsActive)  // added if clause 1/30/23 
        {
            //Debug.Log("AOTouch END Drag detected !!!! " + this.name);
            fingerPointerEvent.Invoke(this.gameObject, "finger UP  Drag ENDED");
        }
    }
}
