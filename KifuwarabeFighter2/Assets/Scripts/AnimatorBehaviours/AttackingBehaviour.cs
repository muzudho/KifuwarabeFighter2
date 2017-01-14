using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingBehaviour : StateMachineBehaviour {

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (
            stateInfo.IsName("LightPunch") ||
            stateInfo.IsName("MediumPunch") ||
            stateInfo.IsName("HardPunch") ||
            stateInfo.IsName("LightKick") ||
            stateInfo.IsName("MediumKick") ||
            stateInfo.IsName("HardKick")
            ) // 攻撃が始まった時
        {
            animator.SetBool("attacking", true);
        }
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (
            stateInfo.IsName("LightPunch") ||
            stateInfo.IsName("MediumPunch") ||
            stateInfo.IsName("HardPunch") ||
            stateInfo.IsName("LightKick") ||
            stateInfo.IsName("MediumKick") ||
            stateInfo.IsName("HardKick")
            ) // 攻撃が終わった時
        {
            animator.SetBool("attacking", false);
        }
    }

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMachineEnter is called when entering a statemachine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash){
    //
    //}

    // OnStateMachineExit is called when exiting a statemachine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash) {
    //
    //}
}
