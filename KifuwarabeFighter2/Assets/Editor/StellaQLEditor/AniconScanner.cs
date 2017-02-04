using System.Collections.Generic;
using System.Text;
using UnityEditor.Animations;
using UnityEngine;

namespace StellaQL
{
    public abstract class AbstractAniconScanner
    {
        public AbstractAniconScanner(bool parameterScan)
        {
            this.parameterScan = parameterScan;
        }
        bool parameterScan;

        void ScanRecursive(string path, AnimatorStateMachine stateMachine, Dictionary<string, AnimatorStateMachine> statemachineList_flat)
        {
            path += stateMachine.name + ".";
            statemachineList_flat.Add(path, stateMachine);

            foreach (ChildAnimatorStateMachine caStateMachine in stateMachine.stateMachines)
            {
                ScanRecursive(path, caStateMachine.stateMachine, statemachineList_flat);
            }
        }

        public void ScanAnimatorController(AnimatorController ac, out AniconData aniconData, StringBuilder message)
        {
            message.AppendLine("Animator controller Scanning...☆（＾～＾）");
            aniconData = new AniconData();

            // パラメーター
            if(parameterScan){
                AnimatorControllerParameter[] acpArray = ac.parameters;
                int num = 0;
                foreach (AnimatorControllerParameter acp in acpArray)
                {
                    OnParameter(aniconData, num, acp);
                    num++;
                }
            }

            foreach (AnimatorControllerLayer layer in ac.layers)//レイヤー
            {
                if(OnLayer(aniconData, layer))
                {
                    Dictionary<string, AnimatorStateMachine> statemachineList_flat = new Dictionary<string, AnimatorStateMachine>(); // フルパス, ステートマシン
                    ScanRecursive("", layer.stateMachine, statemachineList_flat);// 再帰をスキャンして、フラットにする。
                    foreach (KeyValuePair<string, AnimatorStateMachine> statemachine_pair in statemachineList_flat)
                    { // ステート・マシン
                        if(OnStatemachine(aniconData, statemachine_pair.Key, statemachine_pair.Value))
                        {
                            foreach (ChildAnimatorState caState in statemachine_pair.Value.states)
                            { //ステート（ラッパー）
                                if(OnState(aniconData, statemachine_pair.Key, caState))
                                {
                                    foreach (AnimatorStateTransition transition in caState.state.transitions)
                                    { // トランジション
                                        if(OnTransition(aniconData, transition))
                                        {
                                            foreach (AnimatorCondition aniCondition in transition.conditions)
                                            { // コンディション
                                                OnCondition(aniconData, aniCondition);
                                            } // コンディション
                                        }
                                    }//トランジション
                                }
                            }//ステート（ラッパー）
                        }
                    }//ステートマシン
                }
            }//レイヤー

            message.AppendLine("Scanned☆（＾▽＾）");
        }

        public virtual void OnParameter(AniconData aniconData, int num, AnimatorControllerParameter acp) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aniconData"></param>
        /// <param name="layer"></param>
        /// <returns>下位を検索するなら真</returns>
        public virtual bool OnLayer(AniconData aniconData, AnimatorControllerLayer layer) { return false; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aniconData"></param>
        /// <param name="fullnameEndWithDot"></param>
        /// <param name="statemachine"></param>
        /// <returns>下位を検索するなら真</returns>
        public virtual bool OnStatemachine(AniconData aniconData, string fullnameEndWithDot, AnimatorStateMachine statemachine) { return false; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aniconData"></param>
        /// <param name="parentPath"></param>
        /// <param name="caState"></param>
        /// <returns>下位を検索するなら真</returns>
        public virtual bool OnState(AniconData aniconData, string parentPath, ChildAnimatorState caState) { return false; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aniconData"></param>
        /// <param name="transition"></param>
        /// <returns>下位を検索するなら真</returns>
        public virtual bool OnTransition(AniconData aniconData, AnimatorStateTransition transition) { return false; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aniconData"></param>
        /// <param name="aniCondition"></param>
        /// <returns>下位を検索するなら真</returns>
        public virtual bool OnCondition(AniconData aniconData, AnimatorCondition aniCondition) { return false; }
    }

    /// <summary>
    /// 全走査。
    /// </summary>
    public class AniconScanner : AbstractAniconScanner
    {
        public AniconScanner() : base(true)
        {

        }

        public override void OnParameter(AniconData aniconData, int num, AnimatorControllerParameter acp)
        {
            ParameterRecord record = new ParameterRecord(num, acp.name, acp.defaultBool, acp.defaultFloat, acp.defaultInt, acp.nameHash);
            aniconData.table_parameter.Add(record);
        }

        public override bool OnLayer(AniconData aniconData, AnimatorControllerLayer layer)
        {
            LayerRecord layerRecord = new LayerRecord(aniconData.table_layer.Count, layer);
            aniconData.table_layer.Add(layerRecord); return true;
        }

        public override bool OnStatemachine(AniconData aniconData, string fullnameEndWithDot, AnimatorStateMachine statemachine)
        {
            StatemachineRecord stateMachineRecord = new StatemachineRecord(aniconData.table_layer.Count, aniconData.table_statemachine.Count, fullnameEndWithDot, statemachine, aniconData.table_position);
            aniconData.table_statemachine.Add(stateMachineRecord); return true;
        }

        public override bool OnState(AniconData aniconData, string parentPath, ChildAnimatorState caState)
        {
            StateRecord stateRecord = StateRecord.CreateInstance(aniconData.table_layer.Count, aniconData.table_statemachine.Count, aniconData.table_state.Count, parentPath, caState, aniconData.table_position);
            aniconData.table_state.Add(stateRecord); return true;
        }

        public override bool OnTransition(AniconData aniconData, AnimatorStateTransition transition)
        {
            TransitionRecord transitionRecord = new TransitionRecord(aniconData.table_layer.Count, aniconData.table_statemachine.Count, aniconData.table_state.Count, aniconData.table_transition.Count, transition, "");
            aniconData.table_transition.Add(transitionRecord); return true;
        }

        public override bool OnCondition(AniconData aniconData, AnimatorCondition aniCondition)
        {
            ConditionRecord conditionRecord = new ConditionRecord(aniconData.table_layer.Count, aniconData.table_statemachine.Count, aniconData.table_state.Count, aniconData.table_transition.Count, aniconData.table_condition.Count, aniCondition);
            aniconData.table_condition.Add(conditionRecord); return true;
        }
    }

    /// <summary>
    /// ステートマシン、ステートを全部走査。
    /// </summary>
    public class AniconStateNameScanner : AbstractAniconScanner
    {
        public AniconStateNameScanner() : base(false)
        {
            fullpathSet = new HashSet<string>();
        }
        public HashSet<string> fullpathSet;

        public override bool OnLayer(AniconData aniconData, AnimatorControllerLayer layer)
        {
            return true;
        }

        public override bool OnStatemachine(AniconData aniconData, string fullnameEndWithDot, AnimatorStateMachine statemachine)
        {
            fullpathSet.Add(fullnameEndWithDot); return true;
        }

        public override bool OnState(AniconData aniconData, string parentPath, ChildAnimatorState caState)
        {
            fullpathSet.Add(parentPath + caState.state.name); return false;
        }
    }
}
