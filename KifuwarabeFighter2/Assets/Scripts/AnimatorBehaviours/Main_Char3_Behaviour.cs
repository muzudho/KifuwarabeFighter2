﻿using Assets.Scripts.Models.Scenes.Fight;
using DojinCircleGrayscale.Hitbox2DLorikeet;
using DojinCircleGrayscale.StellaQL.Acons.Main_Char3;
using SceneMain;
using UnityEngine;

public class Main_Char3_Behaviour : StateMachineBehaviour {

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        #region 立ち
        if (stateInfo.IsName(AControl.Instance.StateHash_to_record[Animator.StringToHash(Main_Char3_AbstractAControl.BASELAYER_SWAIT)].Name)) // 立ち待機
        {
            animator.SetInteger(ThisSceneStatus.IntegerActioning, (int)TilesetfileType.Stand);
        }
        #endregion
        #region ジャンプ
        else if (stateInfo.IsName(AControl.Instance.StateHash_to_record[Animator.StringToHash(Main_Char3_AbstractAControl.BASELAYER_JMOVE_JMOVE0)].Name)) // ジャンプに着手した。
        {
            animator.SetInteger(ThisSceneStatus.IntegerActioning, (int)TilesetfileType.Jump);
            animator.SetBool(ThisSceneStatus.BoolJMove0, true);
        }
        else if (stateInfo.IsName(AControl.Instance.StateHash_to_record[Animator.StringToHash(Main_Char3_AbstractAControl.BASELAYER_JMOVE_JMOVE1)].Name)) // 上昇
        {
            PlayerBehaviour script = animator.gameObject.GetComponent<PlayerBehaviour>();
            script.Jump1();
        }
        #endregion
        #region 屈み
        else if (stateInfo.IsName(AControl.Instance.StateHash_to_record[Animator.StringToHash(Main_Char3_AbstractAControl.BASELAYER_CWAIT)].Name)) // かがみ待機
        {
            animator.SetInteger(ThisSceneStatus.IntegerActioning, (int)TilesetfileType.Crouch);
        }
        #endregion
        #region その他
        else if (stateInfo.IsName(AControl.Instance.StateHash_to_record[Animator.StringToHash(Main_Char3_AbstractAControl.BASELAYER_OBACKSTEP)].Name)) // バックステップ
        {
            animator.SetInteger(ThisSceneStatus.IntegerActioning, (int)TilesetfileType.Stand);
        }
        else if (stateInfo.IsName(AControl.Instance.StateHash_to_record[Animator.StringToHash(Main_Char3_AbstractAControl.BASELAYER_OGIVEUP)].Name))
        {
            animator.SetBool(ThisSceneStatus.BoolGiveUping, true);
        }
        #endregion
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName(AControl.Instance.StateHash_to_record[Animator.StringToHash(Main_Char3_AbstractAControl.BASELAYER_OGIVEUP)].Name))
        {
            // 投了モーション中
            if(2.0f<=stateInfo.normalizedTime % 1 * animator.GetCurrentAnimatorClipInfo(0)[0].clip.frameRate)
            {
                PlayerBehaviour script = animator.gameObject.GetComponent<PlayerBehaviour>();
                if (!script.MainCameraScript.IsResignCalled)
                {
                    script.ResignCall();
                }
            }
        }
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName(AControl.Instance.StateHash_to_record[Animator.StringToHash(Main_Char3_AbstractAControl.BASELAYER_JMOVE_JMOVE0)].Name)) // 屈伸が終わった時
        {
            PlayerBehaviour script = animator.gameObject.GetComponent<PlayerBehaviour>();
            script.JMove0Exit();
        }
        else if (stateInfo.IsName(AControl.Instance.StateHash_to_record[Animator.StringToHash(Main_Char3_AbstractAControl.BASELAYER_OGIVEUP)].Name))
        {
            // 投了モーションが終わった時。
            animator.SetBool(ThisSceneStatus.BoolGiveUping, false);

            PlayerBehaviour script = animator.gameObject.GetComponent<PlayerBehaviour>();
            script.IsResign = true;
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
