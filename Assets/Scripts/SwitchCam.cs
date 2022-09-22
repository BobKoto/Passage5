using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace SwitchCam
{
    public class SwitchCam : MonoBehaviour
    {
        public List<CinemachineVirtualCamera> virtualCameras = new List<CinemachineVirtualCamera>();
        [SerializeField]
        CinemachineVirtualCamera startPositionCamera;
        public GameObject[] directionColliders = new GameObject[9];
        readonly int camIndexNorth = 0;
        readonly int camIndexSouth = 1;
        readonly int camIndexEast = 2;
        readonly int camIndexWest = 3;
        // Start is called before the first frame update
        void Start()
        {
           // virtualCameras = List<CinemachineVirtualCamera>.FindGameObjectsWithTag("DirectionalCamera");
            // Debug.Log("direction wasMovingSouth " + DirectionTracker.wasMovingSouth);
            directionColliders = GameObject.FindGameObjectsWithTag("DirectionCollider");
            foreach (CinemachineVirtualCamera cam  in virtualCameras)
            {
               // Debug.Log(" something " + cam);
            }
            foreach (GameObject gameObject in directionColliders)
            {
              // if (gameObject== this.gameObject) Debug.Log("found " + this.gameObject.name);
            }
        }

        //// Update is called once per frame
        //void Update()
        //{

        //}
        private void OnTriggerEnter(Collider other)
        {
            string gOName = this.gameObject.name;
            switch (gOName)
            {
                case "MoveNorth":  //Debug.Log("Moving NORTH");
                    if (DirectionTracker.MovingInDirection() == "north") break;
                     Debug.Log("Moving NORTH");
                    DirectionTracker.RegisterDirection(true, false, false, false);
                    virtualCameras[camIndexNorth].MoveToTopOfPrioritySubqueue();
                    break;
                case ("MoveSouth"):   //Debug.Log("Moving SOUTH");
                    if (DirectionTracker.MovingInDirection() == "south") break;
                     Debug.Log("Moving SOUTH");
                    DirectionTracker.RegisterDirection(false, true, false, false);
                    virtualCameras[camIndexSouth].MoveToTopOfPrioritySubqueue();
                    break;
                case ("MoveEast"):   // Debug.Log("Moving EAST");
                    if (DirectionTracker.MovingInDirection() == "east") break;
                     Debug.Log("Moving EAST");
                    DirectionTracker.RegisterDirection(false, false, true, false);
                    virtualCameras[camIndexEast].MoveToTopOfPrioritySubqueue();
                    break;
                case ("MoveWest"):    //Debug.Log("Moving WEST");
                    if (DirectionTracker.MovingInDirection() == "west") break;
                     Debug.Log("Moving WEST");
                    DirectionTracker.RegisterDirection(false, false, false, true);
                    virtualCameras[camIndexWest].MoveToTopOfPrioritySubqueue();
                    break;
                    // Begin the outer angles NE NW SE SW 
                case ("NorthEast"):
                   // Debug.Log("Traversed NorthEast");
                    if (DirectionTracker.MovingInDirection() == "north")  //We were north now we switch to EAST
                    {
                        DirectionTracker.RegisterDirection(false, false, true, false);
                        virtualCameras[camIndexEast].MoveToTopOfPrioritySubqueue();
                    }
                    else   //We were East now we switch to NORTH
                    {
                        DirectionTracker.RegisterDirection(true, false, false, false);
                        virtualCameras[camIndexNorth].MoveToTopOfPrioritySubqueue();
                    }
                    break;
                case ("NorthWest"):
                 //   Debug.Log("Traversed NorthWest");
                    if (DirectionTracker.MovingInDirection() == "north")  //We were north now we switch to WEST
                    {
                        DirectionTracker.RegisterDirection(false, false, false, true);
                        virtualCameras[camIndexWest].MoveToTopOfPrioritySubqueue();
                    }
                    else   //We were West now we switch to NORTH
                    {
                        DirectionTracker.RegisterDirection(true, false, false, false);
                        virtualCameras[camIndexNorth].MoveToTopOfPrioritySubqueue();
                    }
                    break;
                case ("SouthEast"):
                //    Debug.Log("Traversed SouthEast");
                    if (DirectionTracker.MovingInDirection() == "south")  //We were south now we switch to EAST
                    {
                        DirectionTracker.RegisterDirection(false, false, true, false);
                        virtualCameras[camIndexEast].MoveToTopOfPrioritySubqueue();
                    }
                    else   //We were East now we switch to SOUTH
                    {
                        DirectionTracker.RegisterDirection(false, true, false, false);
                        virtualCameras[camIndexSouth].MoveToTopOfPrioritySubqueue();
                    }
                    break;
                case ("SouthWest"):
                //    Debug.Log("Traversed SouthWest");
                    if (DirectionTracker.MovingInDirection() == "south")  //We were south now we switch to WEST
                    {
                        DirectionTracker.RegisterDirection(false, false, false, true);
                        virtualCameras[camIndexWest].MoveToTopOfPrioritySubqueue();
                    }
                    else   //We were West now we switch to SOUTH
                    {
                        DirectionTracker.RegisterDirection(false, true, false, false);
                        virtualCameras[camIndexSouth].MoveToTopOfPrioritySubqueue();
                    }
                    break;
                default: Debug.Log("switch case default");
                    break;
            }
          //  Debug.Log(DirectionTracker.MovingInDirection() + " DirectionTracker.MovingInDirection() called by OnTriggerEnter...");
        }
    }
}