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
        // Start is called before the first frame update
        void Start()
        {
           // Debug.Log("direction wasMovingSouth " + DirectionTracker.wasMovingSouth);
            directionColliders = GameObject.FindGameObjectsWithTag("DirectionCollider");
            foreach (GameObject gameObject in directionColliders)
            {
               if (gameObject== this.gameObject) Debug.Log("found " + this.gameObject.name);
            }
        }

        //// Update is called once per frame
        //void Update()
        //{

        //}

        private void OnTriggerEnter(Collider other)
        {
            virtualCameras[0].MoveToTopOfPrioritySubqueue();
            string gOName = this.gameObject.name;

            switch (gOName)
            {
                case ( "MoveNorth"):  Debug.Log("Moving NORTH");
                    DirectionTracker.RegisterDirection(true, false, false, false);
                    
                    break;
                case ("MoveSouth"):   Debug.Log("Moving SOUTH");
                    DirectionTracker.RegisterDirection(false, true, false, false);

                    break;
                case ("MoveEast"):    Debug.Log("Moving EAST");
                    DirectionTracker.RegisterDirection(false, false, true, false);

                    break;
                case ("MoveWest"):    Debug.Log("Moving WEST");
                    DirectionTracker.RegisterDirection(false, false, false, true);

                    break;
                default: Debug.Log("switch case default");
                    break;
            }

            Debug.Log(DirectionTracker.MovingInDirection() + " DirectionTracker.MovingInDirection() called by OnTriggerEnter...");
        }
    }
}