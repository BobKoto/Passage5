using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraverseCollider : MonoBehaviour  // find on CubeFacingCameraOnGreenSphere6 in EnvironmentEAST (root level)
{
    public GameObject player;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = player.GetComponent<Animator>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (MovingPlatformAction.playerIsOnPlatform)
            {
              Debug.Log("Player entered collider and is on the platform - Start Waving");
              anim.SetBool("avatarSceneWave", true);
            }

        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (MovingPlatformAction.playerIsOnPlatform)
            {
                Debug.Log("Player exited collider and is on the platform - Stop Waving");
                anim.SetBool("avatarSceneWave", false);
            }
        }
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
