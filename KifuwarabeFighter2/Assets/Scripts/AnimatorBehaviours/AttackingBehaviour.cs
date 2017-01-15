using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingBehaviour : StateMachineBehaviour {

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (
            stateInfo.IsName(CommonScript.MOTION_LIGHT_PUNCH0) ||
            stateInfo.IsName(CommonScript.MOTION_MEDIUM_PUNCH0) ||
            stateInfo.IsName(CommonScript.MOTION_HARD_PUNCH0) ||
            stateInfo.IsName(CommonScript.MOTION_LIGHT_KICK0) ||
            stateInfo.IsName(CommonScript.MOTION_MEDIUM_KICK0) ||
            stateInfo.IsName(CommonScript.MOTION_HARD_KICK0)
            ) // 攻撃が始まった時
        {
            animator.SetBool("attacking", true);

            // 連打防止のフラグ立て。
            if (stateInfo.IsName(CommonScript.MOTION_LIGHT_PUNCH0))
            {
                animator.SetBool("pushingLP", true);
            }
            else if (stateInfo.IsName(CommonScript.MOTION_MEDIUM_PUNCH0))
            {
                animator.SetBool("pushingMP", true);
            }
            else if (stateInfo.IsName(CommonScript.MOTION_HARD_PUNCH0))
            {
                animator.SetBool("pushingHP", true);
            }
            else if (stateInfo.IsName(CommonScript.MOTION_LIGHT_KICK0))
            {
                animator.SetBool("pushingLK", true);
            }
            else if (stateInfo.IsName(CommonScript.MOTION_MEDIUM_KICK0))
            {
                animator.SetBool("pushingMK", true);
            }
            else if (stateInfo.IsName(CommonScript.MOTION_HARD_KICK0))
            {
                animator.SetBool("pushingHK", true);
            }
        }
        
        if (stateInfo.IsName(CommonScript.MOTION_DOWN_DAMAGE2))
        {
            // ダウンに入る時。
            animator.SetBool(CommonScript.BOOL_DOWNING, true);
            animator.SetBool(CommonScript.BOOL_INVINCIBLE,true);// 攻撃が当たらない状態になる。
            //CharacterScript script = animator.gameObject.GetComponent<CharacterScript>();
            //script.isInvincible = true; // 攻撃が当たらない状態になる。
        }

        if (stateInfo.IsName(CommonScript.MOTION_GIVEUP0))
        {
            // 投了モーションに入る時。
            animator.SetBool(CommonScript.BOOL_GIVEUPING, true);
            animator.SetBool(CommonScript.BOOL_INVINCIBLE, true);// 攻撃が当たらない状態になる。
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
            stateInfo.IsName(CommonScript.MOTION_LIGHT_PUNCH0) ||
            stateInfo.IsName(CommonScript.MOTION_MEDIUM_PUNCH0) ||
            stateInfo.IsName(CommonScript.MOTION_HARD_PUNCH0) ||
            stateInfo.IsName(CommonScript.MOTION_LIGHT_KICK0) ||
            stateInfo.IsName(CommonScript.MOTION_MEDIUM_KICK0) ||
            stateInfo.IsName(CommonScript.MOTION_HARD_KICK0)
            ) // 攻撃が終わった時
        {
            animator.SetBool("attacking", false);
        }

        if (stateInfo.IsName(CommonScript.MOTION_STANDUP0))//起き上がりから抜けたとき。
        {
            animator.SetBool(CommonScript.BOOL_DOWNING, false);
            animator.SetBool(CommonScript.BOOL_INVINCIBLE, false);// 攻撃が当たる状態になる。
            //CharacterScript script = animator.gameObject.GetComponent<CharacterScript>();
            //script.isInvincible = false; // 攻撃が当たる状態になる。
        }

        if (stateInfo.IsName(CommonScript.MOTION_GIVEUP0))
        {
            // 投了モーションが終わった時。
            animator.SetBool(CommonScript.BOOL_GIVEUPING, false);
            CharacterScript script = animator.gameObject.GetComponent<CharacterScript>();
            script.isResign = true;
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
