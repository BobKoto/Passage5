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
                case ( "MoveNorth"): Debug.Log("Moving NORTH");
                    break;
                case ("MoveSouth"):
                    Debug.Log("Moving SOUTH");
                    break;
                case ("MoveEast"):
                    Debug.Log("Moving EAST");
                    break;
                case ("MoveWest"):
                    Debug.Log("Moving WEST");
                    break;
                default: Debug.Log("switch case default");
                    break;
            }
        }
    }
}