using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlapArms : StateMachineBehaviour
{
    Animator xanim;
   // readonly int flapArms = 5;
    
    
  //  OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        xanim = GameObject.Find("PlayerArmature").GetComponent<Animator>() ;
        int flapArmsHash = Animator.StringToHash(animator.parameters[5].name);
        xanim.SetBool(flapArmsHash, true);
        Debug.Log("ArmUp layer Entered animator= " + animator.name + "  stateInfoSpeed= " + stateInfo.speed + " layerIndex =" + layerIndex);
        Debug.Log("params[5] = " + animator.parameters[5].name + " value = " + xanim.GetBool(flapArmsHash));
            //+ animator.parameters[5].Equals(xanim.GetBool(animator.parameters[5].name)));//gobbledeegook that doesnt work
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        xanim = GameObject.Find("PlayerArmature").GetComponent<Animator>();
        int flapArmsHash = Animator.StringToHash(animator.parameters[5].name);
        xanim.SetBool(flapArmsHash, false);
        Debug.Log("ArmUp layer EXITED animator= " + animator.name + "  stateInfoSpeed= " + stateInfo.speed + " layerIndex =" + layerIndex);
        Debug.Log("params[5] = " + animator.parameters[5].name + " value = " + xanim.GetBool(flapArmsHash));
        //+ animator.parameters[5].Equals(xanim.GetBool(animator.parameters[5].name)));//gobbledeegook that doesnt work

    }

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
