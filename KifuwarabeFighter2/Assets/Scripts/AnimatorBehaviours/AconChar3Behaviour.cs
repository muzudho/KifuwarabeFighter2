using SceneMain;
using StellaQL.Acons;
using UnityEngine;

public class AconChar3Behaviour : StateMachineBehaviour {

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        #region 立ち
        if (stateInfo.IsName(AControll.Instance.StateHash_to_record[Animator.StringToHash(AconChar3.BASELAYER_SWAIT)].Name)) // 立ち待機
        {
            animator.SetInteger(SceneCommon.INTEGER_ACTIONING, (int)TilesetfileTypeIndex.Stand);
        }
        #endregion
        #region ジャンプ
        else if (stateInfo.IsName(AControll.Instance.StateHash_to_record[Animator.StringToHash(AconChar3.BASELAYER_JMOVE_JMOVE0)].Name)) // ジャンプに着手した。
        {
            animator.SetInteger(SceneCommon.INTEGER_ACTIONING, (int)TilesetfileTypeIndex.Jump);
            animator.SetBool(SceneCommon.BOOL_JMOVE0, true);
        }
        else if (stateInfo.IsName(AControll.Instance.StateHash_to_record[Animator.StringToHash(AconChar3.BASELAYER_JMOVE_JMOVE1)].Name)) // 上昇
        {
            Main_PlayerScript script = animator.gameObject.GetComponent<Main_PlayerScript>();
            script.Jump1();
        }
        #endregion
        #region 走り
        #endregion
        #region 屈み
        else if (stateInfo.IsName(AControll.Instance.StateHash_to_record[Animator.StringToHash(AconChar3.BASELAYER_CWAIT)].Name)) // かがみ待機
        {
            animator.SetInteger(SceneCommon.INTEGER_ACTIONING, (int)TilesetfileTypeIndex.Crouch);
        }
        #endregion
        #region その他
        else if (stateInfo.IsName(AControll.Instance.StateHash_to_record[Animator.StringToHash(AconChar3.BASELAYER_OBACKSTEP)].Name)) // バックステップ
        {
            animator.SetInteger(SceneCommon.INTEGER_ACTIONING, (int)TilesetfileTypeIndex.Stand);
        }
        else if (stateInfo.IsName(AControll.Instance.StateHash_to_record[Animator.StringToHash(AconChar3.BASELAYER_OGIVEUP)].Name))
        {
            // 投了モーションに入る時。
            //Debug.Log("投了モーション始まり☆ layerIndex = " + layerIndex + " stateInfo.fullPathHash = " + stateInfo.fullPathHash + " animator.name = " + animator.name);

            animator.SetBool(SceneCommon.BOOL_GIVEUPING, true);
        }
        #endregion
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName(AControll.Instance.StateHash_to_record[Animator.StringToHash(AconChar3.BASELAYER_OGIVEUP)].Name))
        {
            // 投了モーション中
            if(2.0f<=stateInfo.normalizedTime % 1 * animator.GetCurrentAnimatorClipInfo(0)[0].clip.frameRate)
            {
                Main_PlayerScript script = animator.gameObject.GetComponent<Main_PlayerScript>();
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
        if (stateInfo.IsName(AControll.Instance.StateHash_to_record[Animator.StringToHash(AconChar3.BASELAYER_JMOVE_JMOVE0)].Name)) // 屈伸が終わった時
        {
            Main_PlayerScript script = animator.gameObject.GetComponent<Main_PlayerScript>();
            script.JMove0Exit();
        }
        else if (stateInfo.IsName(AControll.Instance.StateHash_to_record[Animator.StringToHash(AconChar3.BASELAYER_OGIVEUP)].Name))
        {
            //Debug.Log("投了モーション終わり☆ layerIndex = " + layerIndex + " stateInfo.fullPathHash = " + stateInfo.fullPathHash + " animator.name = " + animator.name);

            // 投了モーションが終わった時。
            animator.SetBool(SceneCommon.BOOL_GIVEUPING, false);

            Main_PlayerScript script = animator.gameObject.GetComponent<Main_PlayerScript>();
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
