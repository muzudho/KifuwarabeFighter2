using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneMain;

public class AniConChar3Behaviour : StateMachineBehaviour {

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        #region 立ち
        if (stateInfo.IsName(AstateDatabase.index_to_record[AstateIndex.SWait].name)) // 立ち待機
        {
            animator.SetInteger(SceneCommon.INTEGER_ACTIONING, (int)ActioningIndex.Stand);
        }
        else if (
            stateInfo.IsName(AstateDatabase.index_to_record[AstateIndex.SAtkLP].name) ||
            stateInfo.IsName(AstateDatabase.index_to_record[AstateIndex.SAtkMP].name) ||
            stateInfo.IsName(AstateDatabase.index_to_record[AstateIndex.SAtkHP].name) ||
            stateInfo.IsName(AstateDatabase.index_to_record[AstateIndex.SAtkLK].name) ||
            stateInfo.IsName(AstateDatabase.index_to_record[AstateIndex.SAtkMK].name) ||
            stateInfo.IsName(AstateDatabase.index_to_record[AstateIndex.SAtkHK].name)
            ) // 攻撃が始まった時
        {
            // 連打防止のフラグ立て。
            if (stateInfo.IsName(AstateDatabase.index_to_record[AstateIndex.SAtkLP].name))
            {
                animator.SetBool(SceneCommon.BOOL_PUSHING_LP, true);
            }
            else if (stateInfo.IsName(AstateDatabase.index_to_record[AstateIndex.SAtkMP].name))
            {
                animator.SetBool(SceneCommon.BOOL_PUSHING_MP, true);
            }
            else if (stateInfo.IsName(AstateDatabase.index_to_record[AstateIndex.SAtkHP].name))
            {
                animator.SetBool(SceneCommon.BOOL_PUSHING_HP, true);
            }
            else if (stateInfo.IsName(AstateDatabase.index_to_record[AstateIndex.SAtkLK].name))
            {
                animator.SetBool(SceneCommon.BOOL_PUSHING_LK, true);
            }
            else if (stateInfo.IsName(AstateDatabase.index_to_record[AstateIndex.SAtkMK].name))
            {
                animator.SetBool(SceneCommon.BOOL_PUSHING_MK, true);
            }
            else if (stateInfo.IsName(AstateDatabase.index_to_record[AstateIndex.SAtkHK].name))
            {
                animator.SetBool(SceneCommon.BOOL_PUSHING_HK, true);
            }
        }
        #endregion
        #region ジャンプ
        else if (stateInfo.IsName(AstateDatabase.index_to_record[AstateIndex.JMove0].name)) // ジャンプに着手した。
        {
            animator.SetInteger(SceneCommon.INTEGER_ACTIONING, (int)ActioningIndex.Jump);
            animator.SetBool(SceneCommon.BOOL_JMOVE0, true);
        }
        else if (stateInfo.IsName(AstateDatabase.index_to_record[AstateIndex.JMove1].name)) // 上昇
        {
            Main_PlayerScript script = animator.gameObject.GetComponent<Main_PlayerScript>();
            script.Jump1();
        }
        #endregion
        #region 走り
        #endregion
        #region 屈み
        else if (stateInfo.IsName(AstateDatabase.index_to_record[AstateIndex.CWait].name)) // かがみ待機
        {
            animator.SetInteger(SceneCommon.INTEGER_ACTIONING, (int)ActioningIndex.Crouch);
        }
        #endregion
        #region その他
        else if (stateInfo.IsName(AstateDatabase.index_to_record[AstateIndex.OBackstep].name)) // バックステップ
        {
            animator.SetInteger(SceneCommon.INTEGER_ACTIONING, (int)ActioningIndex.Stand);
        }
        else if (stateInfo.IsName(AstateDatabase.index_to_record[AstateIndex.OGiveup].name))
        {
            // 投了モーションに入る時。
            //Debug.Log("投了モーション始まり☆ layerIndex = " + layerIndex + " stateInfo.fullPathHash = " + stateInfo.fullPathHash + " animator.name = " + animator.name);

            animator.SetBool(SceneCommon.BOOL_GIVEUPING, true);
        }
        //else if (stateInfo.IsName(AstateDatabase.index_to_record[AstateIndex.ODown_SDamageH].name))
        //{
        //    // ダウンに入る時。
        //}
        #endregion
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName(AstateDatabase.index_to_record[AstateIndex.OGiveup].name))
        {
            // 投了モーション中
            if(2.0f<=stateInfo.normalizedTime % 1 * animator.GetCurrentAnimatorClipInfo(0)[0].clip.frameRate)
            {
                Main_PlayerScript script = animator.gameObject.GetComponent<Main_PlayerScript>();
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
        if (stateInfo.IsName(AstateDatabase.index_to_record[AstateIndex.JMove0].name)) // 屈伸が終わった時
        {
            Main_PlayerScript script = animator.gameObject.GetComponent<Main_PlayerScript>();
            script.JMove0Exit();
        }
        else if (stateInfo.IsName(AstateDatabase.index_to_record[AstateIndex.OGiveup].name))
        {
            //Debug.Log("投了モーション終わり☆ layerIndex = " + layerIndex + " stateInfo.fullPathHash = " + stateInfo.fullPathHash + " animator.name = " + animator.name);

            // 投了モーションが終わった時。
            animator.SetBool(SceneCommon.BOOL_GIVEUPING, false);

            Main_PlayerScript script = animator.gameObject.GetComponent<Main_PlayerScript>();
            script.isResign = true;
        }
        //else if (
        //    stateInfo.IsName(MotionDatabaseScript.astate_to_record[MotionDatabaseScript.AstateIndex.SAtkLP].name) ||
        //    stateInfo.IsName(MotionDatabaseScript.astate_to_record[MotionDatabaseScript.AstateIndex.SAtkMP].name) ||
        //    stateInfo.IsName(MotionDatabaseScript.astate_to_record[MotionDatabaseScript.AstateIndex.SAtkHP].name) ||
        //    stateInfo.IsName(MotionDatabaseScript.astate_to_record[MotionDatabaseScript.AstateIndex.SAtkLK].name) ||
        //    stateInfo.IsName(MotionDatabaseScript.astate_to_record[MotionDatabaseScript.AstateIndex.SAtkMK].name) ||
        //    stateInfo.IsName(MotionDatabaseScript.astate_to_record[MotionDatabaseScript.AstateIndex.SAtkHK].name)
        //    ) // 攻撃が終わった時
        //{
        //}
        //else if (stateInfo.IsName(AstateDatabase.index_to_record[AstateIndex.OStandup].name))//起き上がりから抜けたとき。
        //{
        //}
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
