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

        public void ScanAnimatorController(AnimatorController ac, StringBuilder message)
        {
            message.AppendLine("Animator controller Scanning...☆（＾～＾）");

            // パラメーター
            if(parameterScan){
                AnimatorControllerParameter[] acpArray = ac.parameters;
                int num = 0;
                foreach (AnimatorControllerParameter acp in acpArray)
                {
                    OnParameter( num, acp);
                    num++;
                }
            }

            foreach (AnimatorControllerLayer layer in ac.layers)//レイヤー
            {
                if(OnLayer( layer))
                {
                    Dictionary<string, AnimatorStateMachine> statemachineList_flat = new Dictionary<string, AnimatorStateMachine>(); // フルパス, ステートマシン
                    ScanRecursive("", layer.stateMachine, statemachineList_flat);// 再帰をスキャンして、フラットにする。
                    foreach (KeyValuePair<string, AnimatorStateMachine> statemachine_pair in statemachineList_flat)
                    { // ステート・マシン
                        if(OnStatemachine( statemachine_pair.Key, statemachine_pair.Value))
                        {
                            foreach (ChildAnimatorState caState in statemachine_pair.Value.states)
                            { //ステート（ラッパー）
                                if(OnState( statemachine_pair.Key, caState))
                                {
                                    foreach (AnimatorStateTransition transition in caState.state.transitions)
                                    { // トランジション
                                        if(OnTransition( transition))
                                        {
                                            foreach (AnimatorCondition aniCondition in transition.conditions)
                                            { // コンディション
                                                OnCondition( aniCondition);
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

        public virtual void OnParameter(int num, AnimatorControllerParameter acp) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aniconData"></param>
        /// <param name="layer"></param>
        /// <returns>下位を検索するなら真</returns>
        public virtual bool OnLayer(AnimatorControllerLayer layer) { return false; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aniconData"></param>
        /// <param name="fullnameEndWithDot"></param>
        /// <param name="statemachine"></param>
        /// <returns>下位を検索するなら真</returns>
        public virtual bool OnStatemachine(string fullnameEndWithDot, AnimatorStateMachine statemachine) { return false; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aniconData"></param>
        /// <param name="parentPath"></param>
        /// <param name="caState"></param>
        /// <returns>下位を検索するなら真</returns>
        public virtual bool OnState(string parentPath, ChildAnimatorState caState) { return false; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aniconData"></param>
        /// <param name="transition"></param>
        /// <returns>下位を検索するなら真</returns>
        public virtual bool OnTransition(AnimatorStateTransition transition) { return false; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aniconData"></param>
        /// <param name="aniCondition"></param>
        /// <returns>下位を検索するなら真</returns>
        public virtual bool OnCondition(AnimatorCondition aniCondition) { return false; }
    }

    /// <summary>
    /// 全走査。
    /// </summary>
    public class AniconScanner : AbstractAniconScanner
    {
        public AniconScanner() : base(true)
        {
            AniconData = new AniconData();
        }
        public AniconData AniconData { get; private set; }

        public override void OnParameter( int num, AnimatorControllerParameter acp)
        {
            ParameterRecord record = new ParameterRecord(num, acp.name, acp.defaultBool, acp.defaultFloat, acp.defaultInt, acp.nameHash);
            AniconData.table_parameter.Add(record);
        }

        public override bool OnLayer( AnimatorControllerLayer layer)
        {
            LayerRecord layerRecord = new LayerRecord(AniconData.table_layer.Count, layer);
            AniconData.table_layer.Add(layerRecord); return true;
        }

        public override bool OnStatemachine( string fullnameEndWithDot, AnimatorStateMachine statemachine)
        {
            StatemachineRecord stateMachineRecord = new StatemachineRecord(AniconData.table_layer.Count, AniconData.table_statemachine.Count, fullnameEndWithDot, statemachine, AniconData.table_position);
            AniconData.table_statemachine.Add(stateMachineRecord); return true;
        }

        public override bool OnState( string parentPath, ChildAnimatorState caState)
        {
            StateRecord stateRecord = StateRecord.CreateInstance(AniconData.table_layer.Count, AniconData.table_statemachine.Count, AniconData.table_state.Count, parentPath, caState, AniconData.table_position);
            AniconData.table_state.Add(stateRecord); return true;
        }

        public override bool OnTransition( AnimatorStateTransition transition)
        {
            TransitionRecord transitionRecord = new TransitionRecord(AniconData.table_layer.Count, AniconData.table_statemachine.Count, AniconData.table_state.Count, AniconData.table_transition.Count, transition, "");
            AniconData.table_transition.Add(transitionRecord); return true;
        }

        public override bool OnCondition( AnimatorCondition aniCondition)
        {
            ConditionRecord conditionRecord = new ConditionRecord(AniconData.table_layer.Count, AniconData.table_statemachine.Count, AniconData.table_state.Count, AniconData.table_transition.Count, AniconData.table_condition.Count, aniCondition);
            AniconData.table_condition.Add(conditionRecord); return true;
        }
    }

    /// <summary>
    /// ステートマシン、ステートを全部走査。
    /// </summary>
    public class AniconStateNameScanner : AbstractAniconScanner
    {
        public AniconStateNameScanner() : base(false)
        {
            FullpathSet = new HashSet<string>();
        }
        public HashSet<string> FullpathSet { get; private set; }

        public override bool OnLayer( AnimatorControllerLayer layer)
        {
            return true;
        }

        public override bool OnStatemachine( string fullnameEndWithDot, AnimatorStateMachine statemachine)
        {
            FullpathSet.Add(fullnameEndWithDot); return true;
        }

        public override bool OnState( string parentPath, ChildAnimatorState caState)
        {
            FullpathSet.Add(parentPath + caState.state.name); return false;
        }

        public string Dump()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string path in this.FullpathSet)
            {
                sb.Append(path);
                sb.Append(" To26= ");
                sb.AppendLine(FullpathConstantGenerator.String_split_toUppercaseAlphabetFigureOnly_join(path,".","_"));
            }
            return sb.ToString();
        }
    }
}
