using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniConChar3Behaviour : StateMachineBehaviour {

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName(CommonScript.MOTION_S_WAIT)) // 立ち待機
        {
            animator.SetInteger(CommonScript.INTEGER_ACTIONING, (int)ActioningIndex.Stand);
        }
        else if (stateInfo.IsName(CommonScript.MOTION_J_MOVE0)) // ジャンプに着手した。
        {
            animator.SetInteger(CommonScript.INTEGER_ACTIONING, (int)ActioningIndex.Jump);
            animator.SetBool(CommonScript.BOOL_JMOVE0, true);
        }
        else if (stateInfo.IsName(CommonScript.MOTION_J_MOVE1)) // 上昇
        {
            CharacterScript script = animator.gameObject.GetComponent<CharacterScript>();
            script.Jump1();
        }
        else if (stateInfo.IsName(CommonScript.MOTION_O_GIVEUP))
        {
            // 投了モーションに入る時。
            //Debug.Log("投了モーション始まり☆ layerIndex = " + layerIndex + " stateInfo.fullPathHash = " + stateInfo.fullPathHash + " animator.name = " + animator.name);

            animator.SetBool(CommonScript.BOOL_GIVEUPING, true);
            animator.SetBool(CommonScript.BOOL_INVINCIBLE, true);// 攻撃が当たらない状態になる。
        }
        else if (
            stateInfo.IsName(CommonScript.MOTION_S_ATK_LP) ||
            stateInfo.IsName(CommonScript.MOTION_S_ATK_MP) ||
            stateInfo.IsName(CommonScript.MOTION_S_ATK_HP) ||
            stateInfo.IsName(CommonScript.MOTION_S_ATK_LK) ||
            stateInfo.IsName(CommonScript.MOTION_S_ATK_MK) ||
            stateInfo.IsName(CommonScript.MOTION_S_ATK_HK)
            ) // 攻撃が始まった時
        {
            animator.SetBool(CommonScript.BOOL_ATTACKING, true);

            // 連打防止のフラグ立て。
            if (stateInfo.IsName(CommonScript.MOTION_S_ATK_LP))
            {
                animator.SetBool(CommonScript.BOOL_PUSHING_LP, true);
            }
            else if (stateInfo.IsName(CommonScript.MOTION_S_ATK_MP))
            {
                animator.SetBool(CommonScript.BOOL_PUSHING_MP, true);
            }
            else if (stateInfo.IsName(CommonScript.MOTION_S_ATK_HP))
            {
                animator.SetBool(CommonScript.BOOL_PUSHING_HP, true);
            }
            else if (stateInfo.IsName(CommonScript.MOTION_S_ATK_LK))
            {
                animator.SetBool(CommonScript.BOOL_PUSHING_LK, true);
            }
            else if (stateInfo.IsName(CommonScript.MOTION_S_ATK_MK))
            {
                animator.SetBool(CommonScript.BOOL_PUSHING_MK, true);
            }
            else if (stateInfo.IsName(CommonScript.MOTION_S_ATK_HK))
            {
                animator.SetBool(CommonScript.BOOL_PUSHING_HK, true);
            }
        }
        else if (stateInfo.IsName(CommonScript.MOTION_O_DOWN_DAMAGE_H))
        {
            // ダウンに入る時。
            animator.SetBool(CommonScript.BOOL_DOWNING, true);
            animator.SetBool(CommonScript.BOOL_INVINCIBLE,true);// 攻撃が当たらない状態になる。
            //CharacterScript script = animator.gameObject.GetComponent<CharacterScript>();
            //script.isInvincible = true; // 攻撃が当たらない状態になる。
        }
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName(CommonScript.MOTION_O_GIVEUP))
        {
            // 投了モーション中
            if(2.0f<=stateInfo.normalizedTime % 1 * animator.GetCurrentAnimatorClipInfo(0)[0].clip.frameRate)
            {
                CharacterScript script = animator.gameObject.GetComponent<CharacterScript>();
                if (!script.mainCameraScript.IsResignCalled)
                {
                    script.ResignCall();
                }
            }
        }
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName(CommonScript.MOTION_J_MOVE0)) // 屈伸が終わった時
        {
            CharacterScript script = animator.gameObject.GetComponent<CharacterScript>();
            script.JMove0Exit();
        }
        else if (stateInfo.IsName(CommonScript.MOTION_O_GIVEUP))
        {
            //Debug.Log("投了モーション終わり☆ layerIndex = " + layerIndex + " stateInfo.fullPathHash = " + stateInfo.fullPathHash + " animator.name = " + animator.name);

            // 投了モーションが終わった時。
            animator.SetBool(CommonScript.BOOL_GIVEUPING, false);
            animator.SetBool(CommonScript.BOOL_INVINCIBLE, false);// 攻撃が当たらない状態を解除☆

            CharacterScript script = animator.gameObject.GetComponent<CharacterScript>();
            script.isResign = true;
        }
        else if (
            stateInfo.IsName(CommonScript.MOTION_S_ATK_LP) ||
            stateInfo.IsName(CommonScript.MOTION_S_ATK_MP) ||
            stateInfo.IsName(CommonScript.MOTION_S_ATK_HP) ||
            stateInfo.IsName(CommonScript.MOTION_S_ATK_LK) ||
            stateInfo.IsName(CommonScript.MOTION_S_ATK_MK) ||
            stateInfo.IsName(CommonScript.MOTION_S_ATK_HK)
            ) // 攻撃が終わった時
        {
            animator.SetBool(CommonScript.BOOL_ATTACKING, false);
        }
        else if (stateInfo.IsName(CommonScript.MOTION_O_STANDUP))//起き上がりから抜けたとき。
        {
            animator.SetBool(CommonScript.BOOL_DOWNING, false);
            animator.SetBool(CommonScript.BOOL_INVINCIBLE, false);// 攻撃が当たる状態になる。
            //CharacterScript script = animator.gameObject.GetComponent<CharacterScript>();
            //script.isInvincible = false; // 攻撃が当たる状態になる。
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
