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

            Debug.Log("Player entered");
            anim.SetBool("avatarSceneWave", true);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited");
            anim.SetBool("avatarSceneWave", false);

        }
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
