

using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System.Threading;
using System;
using UnityEngine.UI;

namespace UnityEngine.InputSystem.EnhancedTouch
{    

    public class TouchInputMgr : MonoBehaviour
    {

        GameObject[] gameObjectToDrag = new GameObject[10];
        bool[] draggingMode = new bool[10]; //cant be more than 10 fingers :)
        RaycastHit hit;
      //  public GameObject myPrefab;
        public float speed = 1;
        Vector3 moveYtoZ = Vector3.zero;
        AudioSource touchTapSound;
        public Text testText;

        public Camera refCam1; //projection: orthographic   size: 6    //best setting for touches?? think so!
        private int tid;

        public void OnEnable()
        {
            EnhancedTouchSupport.Enable();  
        }
        public void OnDisable()
        {
            EnhancedTouchSupport.Disable();  
        }
        void Start()
        {
            Debug.Log("Hello from TouchInputMgr Started ...");
            touchTapSound = GetComponent<AudioSource>();
            testText.text = ".";  //"touch empty area slowly to create a sphere";
        }
       void Update()
        {
        // Working Enhanced Touch Code 
            foreach (var touch in Touch.activeTouches)  //   var activeTouches = Touch.activeTouches;
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                            Ray ray = refCam1.ScreenPointToRay(touch.screenPosition);
                            Vector3 tempRay = new Vector2(refCam1.ScreenToWorldPoint(touch.screenPosition).x, refCam1.ScreenToWorldPoint(touch.screenPosition).y);
                            Debug.DrawLine(refCam1.transform.position, tempRay, Color.green, 5);
                            if (Physics.SphereCast(ray, 0.8f, out hit))
                            {
                                gameObjectToDrag[touch.finger.index] = hit.collider.gameObject;
                                draggingMode[touch.finger.index] = true;
                            }
                            else
                            {
                                Vector3 vp = new Vector3(touch.screenPosition.x, touch.screenPosition.y, -.1f);
                                Vector3 vps = refCam1.ScreenToWorldPoint(vp, 0);
                                //  Debug.Log( "ray:  " + ray +  "  vp :" + vp +    "   vps: " + vps);
                                //var newSphere = Instantiate(myPrefab, vps, Quaternion.identity);
                                //newSphere.GetComponent<MeshRenderer>().material.color =
                                //         new Color(Random.value, Random.value, Random.value, 1.0f);
                                draggingMode[touch.finger.index] = false;
                                tid = touch.touchId;
                            }
                            break;
                    case TouchPhase.Moved:
                            if (gameObjectToDrag[touch.finger.index] && draggingMode[touch.finger.index] && !gameObjectToDrag[touch.finger.index].isStatic)
                            {
                                moveYtoZ.x = touch.delta.x;
                                moveYtoZ.y = touch.delta.y;
                                gameObjectToDrag[touch.finger.index].transform.Translate(speed * Time.deltaTime * moveYtoZ);
                            }
                        
                        break;
                    case TouchPhase.Ended:
                        draggingMode[touch.finger.index] = false;
                        break;
                }
                if (touch.isTap)
                {
                    Ray ray = refCam1.ScreenPointToRay(touch.screenPosition);
                    if (Physics.SphereCast(ray, 0.8f, out hit))
                    {
                        gameObjectToDrag[touch.finger.index] = hit.collider.gameObject;
                        draggingMode[touch.finger.index] = true;
                    }
                    if (draggingMode[touch.finger.index] && touch.touchId != tid)
                    {
                        Destroy(gameObjectToDrag[touch.finger.index]);
                        touchTapSound.Play(0);
                    }
                }
            }  //for each
       } //end Update

    } //end class
}// Original working Enhanced Touch Code  end namespace
