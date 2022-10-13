using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlapArms : StateMachineBehaviour
{
    // WARNING!! Parameter changes in this script(and likely others)
    // WILL NOT APPEAR in EDITOR PLAYMODE unless the Animated GameObject is selected in the editor

    //  OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Scene activeScene;
        int flapArmsHash = Animator.StringToHash(animator.parameters[5].name);
        bool waveArms = animator.GetBool(flapArmsHash);
        activeScene = SceneManager.GetActiveScene();   
        {
            if (activeScene.buildIndex == 3 && waveArms)   //here we try to only start and stop waving in sceneIndex 3 //the mirror routine/scene 
            {
                int baseLayerSpeedHash = Animator.StringToHash(animator.parameters[0].name);  // get the speed name 
                float speedBaseLayer;
                speedBaseLayer = animator.GetFloat(baseLayerSpeedHash);  //get the speed 
                if (speedBaseLayer > 1f)  //is character moving? and if so we don't want to flap arms 
                {
                    animator.SetLayerWeight(1, 0);    //weight 0 stops arm flaps
                }
                else
                    animator.SetLayerWeight(1, 1);     //weight 1 lets arms flap 
            }
            else animator.SetLayerWeight(1, 0);    //weight 0 stops arm flaps
        }
        //For this to  work you gotta have an EXIT state and a TRANSITION to it in layerIndex 1 

    }
    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //int flapArmsHash = Animator.StringToHash(animator.parameters[5].name);
        //animator.SetBool(flapArmsHash, false);
        //  Debug.Log("ArmUp layer EXITED animator= " + animator.name + "  stateInfoSpeed= " + stateInfo.speed + " layerIndex =" + layerIndex);
        //  Debug.Log("EXIT params[5] = " + animator.parameters[5].name + " value = " + animator.GetBool(flapArmsHash));
    }








    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    //Called every frame 
    //    Debug.Log("ArmUp layer MOVE animator= " + animator.name + "  stateInfoSpeed= " + stateInfo.speed + " layerIndex =" + layerIndex);
    //    Debug.Log(" params sub 5  = " + animator.parameters[5].name + " value = " + animator.parameters[5].defaultBool);
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}
}
