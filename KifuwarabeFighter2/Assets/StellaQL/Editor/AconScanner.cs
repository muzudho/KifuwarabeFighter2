using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace DojinCircleGrayscale.StellaQL
{
    public abstract class AbstractAconScanner
    {
        public AbstractAconScanner(bool parameterScan)
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
                if (null == caStateMachine.stateMachine) { throw new UnityException("Child statemachine is null. parent stateMachine.name=[" + stateMachine.name+"]"); }
                ScanRecursive(path, caStateMachine.stateMachine, statemachineList_flat);
            }
        }

        public void ScanAnimatorController(AnimatorController ac, StringBuilder info_message)
        {
            info_message.AppendLine("Animator controller Scanning...");

            // パラメーター
            if (parameterScan){
                AnimatorControllerParameter[] acpArray = ac.parameters;
                int num = 0;
                foreach (AnimatorControllerParameter acp in acpArray)
                {
                    OnParameter( num, acp);
                    num++;
                }
            }

            int lNum = 0;
            // レイヤー
            foreach (AnimatorControllerLayer layer in ac.layers)
            {
                if(OnLayer( layer, lNum))
                {
                    // フルパス, ステートマシン
                    Dictionary<string, AnimatorStateMachine> statemachineList_flat = new Dictionary<string, AnimatorStateMachine>();

                    // レイヤーは、ステートマシンを持っていないことがある。他のレイヤーのステートマシンを参照してるのだろう。

                    // 次のレイヤーへ
                    if (null == layer.stateMachine) { continue; }

                    // 再帰をスキャンして、フラットにする。
                    ScanRecursive("", layer.stateMachine, statemachineList_flat);

                    int smNum = 0;
                    foreach (KeyValuePair<string, AnimatorStateMachine> statemachine_pair in statemachineList_flat)
                    {
                        // ステート・マシン

                        FullpathTokens ft1 = new FullpathTokens();
                        int caret1 = 0;
                        if(!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames(statemachine_pair.Key, ref caret1, ref ft1)) {
                            throw new UnityException("Parse failure. [" + statemachine_pair.Key + "] ac=[" + ac.name + "]"); }

                        if(OnStatemachine(
                            statemachine_pair.Key,

                            // 例えばフルパスが "Base Layer.Alpaca.Bear.Cat.Dog" のとき、"Alpaca.Bear.Cat"。
                            ft1.StatemachinePath,

                            statemachine_pair.Value, lNum, smNum))
                        {
                            int sNum = 0;
                            foreach (ChildAnimatorState caState in statemachine_pair.Value.states)
                            {
                                // ステート（ラッパー）

                                if (OnState( statemachine_pair.Key, caState, lNum, smNum, sNum))
                                {
                                    // トランジション番号
                                    int tNum = 0;
                                    foreach (AnimatorStateTransition transition in caState.state.transitions)
                                    {
                                        if(OnTransition( transition, lNum, smNum, sNum, tNum))
                                        {
                                            // コンディション番号
                                            int cNum = 0;
                                            foreach (AnimatorCondition condition in transition.conditions)
                                            {
                                                OnCondition( condition, lNum, smNum, sNum, tNum, cNum);
                                                cNum++;
                                            }
                                        }
                                        tNum++;
                                    }// トランジション
                                }
                                sNum++;
                            }// ステート（ラッパー）
                        }
                        smNum++;
                    }// ステートマシン
                }
                lNum++;
            }// レイヤー

            OnScanned(info_message);
            info_message.AppendLine("Scanned.");
        }

        public virtual void OnParameter(int num, AnimatorControllerParameter acp) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="layer"></param>
        /// <returns>下位を検索するなら真</returns>
        public virtual bool OnLayer(AnimatorControllerLayer layer, int lNum) { return false; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullnameEndWithDot"></param>
        /// <param name="statemachine"></param>
        /// <returns>下位を検索するなら真</returns>
        public virtual bool OnStatemachine(string fullnameEndWithDot, string statemachinePath, AnimatorStateMachine statemachine, int lNum, int smNum) { return false; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentPath"></param>
        /// <param name="caState"></param>
        /// <returns>下位を検索するなら真</returns>
        public virtual bool OnState(string parentPath, ChildAnimatorState caState, int lNum, int smNum, int sNum) { return false; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transition"></param>
        /// <returns>下位を検索するなら真</returns>
        public virtual bool OnTransition(AnimatorStateTransition transition, int lNum, int smNum, int sNum, int tNum) { return false; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns>下位を検索するなら真</returns>
        public virtual bool OnCondition(AnimatorCondition condition, int lNum, int smNum, int sNum, int tNum, int cNum) { return false; }

        public virtual void OnScanned(StringBuilder info_message) { }
    }

    /// <summary>
    /// 全走査。
    /// </summary>
    public class AconScanner : AbstractAconScanner
    {
        public AconScanner() : base(true)
        {
            AconDocument = new AconDocument();
            m_motionCounter_ = new Dictionary<string, MotionRecord.Wrapper>();
        }
        public AconDocument AconDocument { get; private set; }
        private Dictionary<string, MotionRecord.Wrapper> m_motionCounter_ { get; }

        public override void OnParameter( int num, AnimatorControllerParameter acp)
        {
            ParameterRecord record = new ParameterRecord(num, acp.name, acp.defaultBool, acp.defaultFloat, acp.defaultInt, acp.nameHash, acp.type);
            AconDocument.parameters.Add(record);
        }

        public override bool OnLayer( AnimatorControllerLayer layer, int lNum)
        {
            LayerRecord layerRecord = new LayerRecord(
                lNum,
                layer);
            AconDocument.layers.Add(layerRecord); return true;
        }

        public override bool OnStatemachine(string fullnameEndWithDot, string statemachinePath, AnimatorStateMachine statemachine, int lNum, int smNum)
        {
            StatemachineRecord stateMachineRecord = new StatemachineRecord(
                lNum,
                smNum,
                statemachinePath,
                statemachine, AconDocument.positions);
            AconDocument.statemachines.Add(stateMachineRecord); return true;
        }

        /// <param name="parentPath"></param>
        /// <param name="caState"></param>
        /// <param name="lNum">レイヤー番号</param>
        /// <param name="smNum">ステートマシン番号</param>
        /// <param name="sNum">ステート番号</param>
        /// <returns></returns>
        public override bool OnState( string parentPath, ChildAnimatorState caState, int lNum, int smNum, int sNum)
        {
            StateRecord stateRecord = StateRecord.CreateInstance(
                lNum,
                smNum,
                sNum,
                parentPath,
                caState, AconDocument.positions);
            AconDocument.states.Add(stateRecord);

            // モーション・スキャン
            if (null != caState.state.motion)
            {
                Motion motion = caState.state.motion;
                string assetPath = AssetDatabase.GetAssetPath(motion.GetInstanceID());
                //ebug.Log(" motion.GetType()=[" + motion.GetType().ToString() + "] assetPath=["+ assetPath + "]");

                if (m_motionCounter_.ContainsKey(assetPath))
                {
                    // 既存のモーションを複数回使うことはある。
                    m_motionCounter_[assetPath].CountOfAttachDestination++;
                }
                else
                {
                    m_motionCounter_.Add(assetPath, new MotionRecord.Wrapper(caState.state.motion,1));
                }
            }

            return true;
        }

        /// <param name="transition"></param>
        /// <param name="lNum">レイヤー番号</param>
        /// <param name="smNum">ステートマシン番号</param>
        /// <param name="sNum">ステート番号</param>
        /// <param name="tNum">トランジション番号</param>
        /// <returns></returns>
        public override bool OnTransition( AnimatorStateTransition transition, int lNum, int smNum, int sNum, int tNum)
        {
            TransitionRecord transitionRecord = new TransitionRecord(
                lNum,
                smNum,
                sNum,
                tNum,
                transition, "");
            AconDocument.transitions.Add(transitionRecord); return true;
        }

        /// <param name="condition"></param>
        /// <param name="lNum">レイヤー番号</param>
        /// <param name="smNum">ステートマシン番号</param>
        /// <param name="sNum">ステート番号</param>
        /// <param name="tNum">トランジション番号</param>
        /// <param name="cNum">コンディション番号</param>
        /// <returns></returns>
        public override bool OnCondition( AnimatorCondition condition, int lNum, int smNum, int sNum, int tNum, int cNum)
        {
            ConditionRecord conditionRecord = new ConditionRecord(
                lNum,
                smNum,
                sNum,
                tNum,
                cNum,
                condition);
            AconDocument.conditions.Add(conditionRecord); return true;
        }

        public override void OnScanned(StringBuilder info_message)
        {
            // 参照されていないモーションを探す
            info_message.AppendLine("Assets folder Scanning...");
            string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();
            foreach (string assetPath in allAssetPaths)
            {
                if (assetPath.EndsWith(".anim",true, System.Globalization.CultureInfo.CurrentCulture) && !m_motionCounter_.ContainsKey(assetPath))
                {
                    // 参照されていないモーション
                    Motion unreferencedMotion = AssetDatabase.LoadAssetAtPath<Motion>(assetPath);
                    m_motionCounter_.Add(assetPath, new MotionRecord.Wrapper(unreferencedMotion, 0));
                }
            }

            // モーションを移し替える。
            foreach (KeyValuePair<string, MotionRecord.Wrapper> item in m_motionCounter_)
            {
                AconDocument.motions.Add(new MotionRecord(item.Key, item.Value.CountOfAttachDestination, item.Value.Source));
            }
        }
    }

    /// <summary>
    /// ステートマシン、ステートを全部走査。
    /// </summary>
    public class AconStateNameScanner : AbstractAconScanner
    {
        public AconStateNameScanner() : base(false)
        {
            FullpathSet = new HashSet<string>();
        }
        public HashSet<string> FullpathSet { get; private set; }

        public override bool OnLayer( AnimatorControllerLayer layer, int lNum)
        {
            return true;
        }

        public override bool OnStatemachine(string fullnameEndWithDot, string statemachinePath, AnimatorStateMachine statemachine, int lNum, int smNum)
        {
            FullpathSet.Add(fullnameEndWithDot); return true;
        }

        public override bool OnState( string parentPath, ChildAnimatorState caState, int lNum, int smNum, int sNum)
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
