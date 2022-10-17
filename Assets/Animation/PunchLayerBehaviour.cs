using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PunchLayerBehaviour : StateMachineBehaviour
{
     Collider playerRightHandCollider;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Scene activeScene;
        //int punchHash = Animator.StringToHash(animator.parameters[6].name);
        // waveArms = animator.GetBool(flapArmsHash);
        activeScene = SceneManager.GetActiveScene();
        playerRightHandCollider = GameObject.Find("Right_Hand").GetComponent<SphereCollider>();
            if (activeScene.buildIndex == 2 )   //here we try to only do punches in sceneIndex 2 //the avatar scene 
            {
                int baseLayerSpeedHash = Animator.StringToHash(animator.parameters[0].name);  // get the speed name 
                float speedBaseLayer;
                speedBaseLayer = animator.GetFloat(baseLayerSpeedHash);  //get the speed 
                if (speedBaseLayer > 1f)  //is character moving? and if so we don't want to punch
                {
                    animator.SetLayerWeight(2, 0);    //weight 0 inhibits punch
                }
               else
               {
                    playerRightHandCollider.enabled = true;
                    animator.SetLayerWeight(2, 1);     //does a punch 
                    Debug.Log("Punch started collider.enabled set TRUE");
               }

            }
            else animator.SetLayerWeight(2, 0);    //weight 0 stops arm flaps

        //For this to  work you gotta have an EXIT state and a TRANSITION to it in layerIndex 2
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerRightHandCollider = GameObject.Find("Right_Hand").GetComponent<SphereCollider>();
        playerRightHandCollider.enabled = false;
        Debug.Log(" Punch Exited collider.enabled set FALSE");
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
