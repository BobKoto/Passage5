using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlapArms : StateMachineBehaviour
{
    // WARNING!! Parameter changes in this script(and likely others)
    // WILL NOT APPEAR in EDITOR PLAYMODE unless the Animated GameObject is selected in the editor

    //  OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
     //   int flapArmsHash = Animator.StringToHash(animator.parameters[5].name);
        int baseLayerSpeedHash = Animator.StringToHash(animator.parameters[0].name);

      //  float flapArmsWeight;
      //  bool flapArmsBool;
        float speedBaseLayer;

        speedBaseLayer = animator.GetFloat(baseLayerSpeedHash);
     //   flapArmsBool = animator.GetBool(flapArmsHash);
        if (speedBaseLayer > 1f)  //is character moving? and if so we don't want to flap arms 
        {
            animator.SetLayerWeight(1, 0);    //weight 0 stops arm flaps
        }
        else
            animator.SetLayerWeight(1, 1);     //weight 1 lets arms flap 
        //var animClipsSize = animator.GetNextAnimatorClipInfo(1).Length;
        //if (animClipsSize > 0)
        //{

        // Debug.Log("Array size is " + animClipsSize + " Next clip WEIGHT is " + animator.GetNextAnimatorClipInfo(1)[0].weight);
        //animator.GetNextAnimatorClipInfo(1)[0].weight = 1; // can't do because ReadOnly

        //animator.SetLayerWeight(1, 1);
        //flapArmsWeight = animator.GetNextAnimatorClipInfo(1)[0].weight;
        //flapArmsWeight = animator.GetLayerWeight(1);

        // Debug.Log("Starting clip : " + animator.GetNextAnimatorClipInfo(1)[0].clip);
        //flapArmsWeight = animator.GetLayerWeight(1);
        //Debug.Log("flaparms clip  Weight = " + flapArmsWeight + " flapArmsBool is " + flapArmsBool);
        //}

        //  Debug.Log("ArmUp layer ENTERED animator= " + animator.name + "  stateInfoSpeed= " + stateInfo.speed + " layerIndex =" + layerIndex);
        //  Debug.Log("ENTER params[5] = " + animator.parameters[5].name + " value = " + animator.GetBool(flapArmsHash));
        //  Debug.Log(" flapArmsWeight = flapArms ['RobotArmsUp'].weight = " + flapArmsWeight);


    }
    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int flapArmsHash = Animator.StringToHash(animator.parameters[5].name);
        animator.SetBool(flapArmsHash, false);
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
