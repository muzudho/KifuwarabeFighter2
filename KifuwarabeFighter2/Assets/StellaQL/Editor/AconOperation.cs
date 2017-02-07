//
// Animation Controller Operation
//
using UnityEditor.Animations;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System;

namespace StellaQL
{
    /// <summary>
    /// どれでも関連
    /// </summary>
    public abstract class Operation_Something
    {
        public static void Update(AnimatorController ac, AconData aconData_old, HashSet<UpateReqeustRecord> request, StringBuilder message)
        {
            // テスト出力
            {
                StringBuilder contents = new StringBuilder();
                contents.Append("Update request ");
                contents.Append(request.Count);
                contents.Append(" record sets.");
                contents.AppendLine();
                foreach (UpateReqeustRecord record in request)
                {
                    record.ToCsvLine(contents);
                }
                Debug.Log(contents.ToString());
            }

            // [ステート・フルパス、トランジション番号、コンディション番号]
            Dictionary<string,Dictionary<int, Dictionary<int, Operation_Condition.UpateReqeustRecordSet>>> conditionRecordSet = new Dictionary<string, Dictionary<int, Dictionary<int, Operation_Condition.UpateReqeustRecordSet>>>();

            foreach (UpateReqeustRecord record in request)
            {
                switch (record.Category)
                {
                    case "parameters": Operation_Parameter.Update(ac, record, message); break;
                    case "layers": Operation_Layer.Update(ac, record, message); break;
                    case "stateMachines": Operation_Statemachine.Update(ac, record, message); break;
                    case "states": Operation_State.Update(ac, record, message); break;
                    case "transitions": Operation_Transition.Update(ac, record, message); break;
                    case "conditions":
                        {
                            // 条件を溜め込む。 mode, parameter, threshold の３つが揃って更新ができる。
                            Operation_Condition.UpateReqeustRecordSet buffer;

                            // [ステートフルパス,,]
                            Dictionary<int, Dictionary<int, Operation_Condition.UpateReqeustRecordSet>> conditionRecordSet1;
                            if (conditionRecordSet.ContainsKey(record.Fullpath))
                            {
                                Debug.Log("既存 [ステートフルパス,,]=" + record.Fullpath);
                                conditionRecordSet1 = conditionRecordSet[record.Fullpath];
                            }
                            else
                            {
                                Debug.Log("新規追加 [ステートフルパス,,]=" + record.Fullpath);
                                conditionRecordSet1 = new Dictionary<int, Dictionary<int, Operation_Condition.UpateReqeustRecordSet>>();
                                conditionRecordSet[record.Fullpath] = conditionRecordSet1;
                            }

                            // [ステートフルパス,トランジション番号,]
                            Dictionary<int, Operation_Condition.UpateReqeustRecordSet> conditionRecordSet2;
                            int fullpathTransition = int.Parse(record.FullpathTransition);
                            if (conditionRecordSet1.ContainsKey(fullpathTransition))
                            {
                                Debug.Log("既存 [ステートフルパス,トランジション番号,]=" + record.Fullpath + ", " + fullpathTransition);
                                conditionRecordSet2 = conditionRecordSet1[fullpathTransition];
                            }
                            else
                            {
                                Debug.Log("新規追加 [ステートフルパス,トランジション番号,]=" + record.Fullpath + ", " + fullpathTransition);
                                conditionRecordSet2 = new Dictionary<int, Operation_Condition.UpateReqeustRecordSet>();
                                conditionRecordSet1[fullpathTransition] = conditionRecordSet2;
                            }

                            // [ステートフルパス,トランジション番号,コンディション番号]
                            int fullpathCondition = int.Parse(record.FullpathCondition);
                            if (conditionRecordSet2.ContainsKey(fullpathCondition))
                            {
                                Debug.Log("既存 [ステートフルパス,トランジション番号,コンディション番号]=" + record.Fullpath + ", " + fullpathTransition + ", " + fullpathCondition);
                                buffer = conditionRecordSet2[fullpathCondition];
                            }
                            else
                            {
                                Debug.Log("新規追加 [ステートフルパス,トランジション番号,コンディション番号]=" + record.Fullpath + ", " + fullpathTransition + ", " + fullpathCondition);
                                // 空の要求セットを作成・追加
                                buffer = new Operation_Condition.UpateReqeustRecordSet(fullpathTransition, fullpathCondition);
                                conditionRecordSet2[fullpathCondition] = buffer;
                            }

                            {
                                // 新規作成。
                                //ConditionRecord.AnimatorConditionWrapper wrapper = Operation_Condition.Lookup(ac, record);

                                //ConditionRecord cRecord = wrapper.A Operation_Condition.Lookup(,
                                //    int.Parse(record.FullpathTransition),
                                //    int.Parse(record.FullpathCondition)
                                //    );

                                //UpateReqeustRecord record_mode = new UpateReqeustRecord();
                                //buffer = new Operation_Condition.UpateReqeustRecordSet(wrapper.m_source.mode, wrapper.m_source.threshold, wrapper.m_source.parameter);
                            }

                            // セットに、レコードを追加
                            Debug.Log("追加 record.Name=[" + record.Name + "]");
                            switch (record.Name)
                            {
                                case "parameter": Debug.Log("追加 パラメーター"); buffer.Parameter = record; break;
                                case "mode": Debug.Log("追加 モード"); buffer.Mode = record; break;
                                case "threshold": Debug.Log("追加 スレッショルド"); buffer.Threshold = record; break;
                                default: Debug.Log("追加失敗"); throw new UnityException("未定義のプロパティ名だぜ☆（＞＿＜） record.Name=[" + record.Name + "]");
                            }
                        }
                        break;
                    case "positinos": Operation_Position.Update(ac, record, message); break;
                    default: throw new UnityException("未対応のカテゴリー=["+ record.Category + "]");
                }
            }

            // 条件を消化
            // [ステート・フルパス,トランジション番号,コンディション番号]
            Debug.Log("conditionRecordSet.Count=" + conditionRecordSet.Count);
            foreach (KeyValuePair<string,Dictionary<int, Dictionary<int, Operation_Condition.UpateReqeustRecordSet>>> conditionRecordSetPair in conditionRecordSet)
            {
                // [,トランジション番号,コンディション番号]
                Debug.Log("conditionRecordSetPair.Value.Count=" + conditionRecordSetPair.Value.Count);
                foreach (KeyValuePair<int, Dictionary<int, Operation_Condition.UpateReqeustRecordSet>> conditionRecordSet1Pair in conditionRecordSetPair.Value)
                {
                    // [,,コンディション番号]
                    Debug.Log("conditionRecordSet1Pair.Value.Count=" + conditionRecordSet1Pair.Value.Count);
                    foreach (KeyValuePair<int, Operation_Condition.UpateReqeustRecordSet> conditionRecordSet2Pair in conditionRecordSet1Pair.Value)
                    {
                        // FIXME: 更新要求がなかったフィールドは、前のを引き継ぐのでは。→新規作成なら既存データは無いが、更新の場合は。
                        Operation_Condition.Update(ac, conditionRecordSet2Pair.Value, message);
                    }
                }
            }
        }

        public static bool HasProperty(string name, Dictionary<string,RecordDefinition> definitions, string calling)
        {
            if (definitions.ContainsKey(name)) { return true; }
            else
            {
                StringBuilder sb = new StringBuilder(); int i = 0; foreach (string name2 in StateRecord.Definitions.Keys) { sb.Append("[");sb.Append(i);sb.Append("]"); sb.AppendLine(name2); i++; }
                throw new UnityException(calling + " : 更新できないプロパティ名が指定されたぜ☆（＾～＾） name=[" + name + "] 対応しているのは次の名前だぜ☆ : " + Environment.NewLine + sb.ToString() + " ここまで");
            }
        }
    }

    public abstract class Operation_Parameter
    {
        public static void Update(AnimatorController ac, UpateReqeustRecord request, StringBuilder message)
        {
            throw new UnityException("[" + request.Fullpath + "]パラメーターには未対応だぜ☆（＾～＾） ac=[" + ac.name + "]");
        }
    }

    public abstract class Operation_Layer
    {
        public static void Update(AnimatorController ac, UpateReqeustRecord request, StringBuilder message)
        {
            AnimatorControllerLayer layer = Lookup(ac, request.Fullpath);
            if (null == layer) { throw new UnityException("[" + request.Fullpath + "]レイヤーは見つからなかったぜ☆（＾～＾） ac=[" + ac.name + "]"); }

            if (Operation_Something.HasProperty(request.Name, LayerRecord.Definitions, "レイヤー操作"))
            {
                StateRecord.Definitions[request.Name].Update(layer, request, message);
            }
        }

        #region 検索
        /// <summary>
        /// パスを指定すると ステートマシンを返す。
        /// </summary>
        /// <param name="path">"Base Layer.JMove" といった文字列。</param>
        public static AnimatorControllerLayer Lookup(AnimatorController ac, string path)
        {
            string[] nodes = path.Split('.');
            // [0] レイヤー

            if (nodes.Length < 1) { throw new UnityException("ノード数が１つ未満だったぜ☆（＾～＾） ステートマシン名は無いのかだぜ☆？"); }

            // 最初の名前[0]は、レイヤーを検索する。
            foreach (AnimatorControllerLayer layer in ac.layers)
            {
                if (nodes[0] == layer.name) { return layer; }
            }
            throw new UnityException("レイヤーが見つからないぜ☆（＾～＾）nodes=[" + string.Join("][", nodes) + "]");
            //return null;
        }
        #endregion
    }

    /// <summary>
    /// ステートマシン関連
    /// </summary>
    public abstract class Operation_Statemachine
    {
        public static void Update(AnimatorController ac, UpateReqeustRecord request, StringBuilder message)
        {
            AnimatorStateMachine statemachine = Lookup(ac, request.Fullpath);
            if (null == statemachine) { throw new UnityException("[" + request.Fullpath + "]ステートマシンは見つからなかったぜ☆（＾～＾） ac=[" + ac.name + "]"); }

            if (Operation_Something.HasProperty(request.Name, StatemachineRecord.Definitions, "ステートマシン操作"))
            {
                StateRecord.Definitions[request.Name].Update(statemachine, request, message);
            }
        }

        #region 検索
        /// <summary>
        /// パスを指定すると ステートマシンを返す。
        /// </summary>
        /// <param name="path">"Base Layer.JMove" といった文字列。</param>
        public static AnimatorStateMachine Lookup(AnimatorController ac, string path)
        {
            string[] nodes = path.Split('.');
            // [0～length-1] ステートマシン名

            if (nodes.Length < 1) { throw new UnityException("ノード数が１つ未満だったぜ☆（＾～＾） ステートマシン名は無いのかだぜ☆？ ac.name=[" + ac.name + "]"); }

            // 最初の名前[0]は、レイヤーを検索する。
            AnimatorStateMachine currentMachine = null;
            foreach (AnimatorControllerLayer layer in ac.layers)
            {
                if (nodes[0] == layer.name) { currentMachine = layer.stateMachine; break; }
            }
            if (null == currentMachine) { throw new UnityException("見つからないぜ☆（＾～＾）nodes=[" + string.Join("][", nodes) + "] ac.name=[" + ac.name + "]"); }

            if (2 < nodes.Length) // ステートマシンが途中にある場合、最後のステートマシンまで降りていく。
            {
                currentMachine = GetLeafMachine(ac, currentMachine, nodes);
                if (null == currentMachine) { throw new UnityException("無いノードが指定されたぜ☆（＾～＾）9 currentMachine.name=[" + currentMachine.name + "] nodes=[" + string.Join("][", nodes) + "] ac.name=["+ac.name+"]"); }
            }

            return currentMachine;
        }

        /// <summary>
        /// 分かりづらいが、ノードの[1]～[length-1]を辿って、最後のステートマシンを返す。
        /// </summary>
        private static AnimatorStateMachine GetLeafMachine(AnimatorController ac, AnimatorStateMachine currentMachine, string[] nodes)
        {
            for (int i = Operation_Common.ROOT_NODE_IS_LAYER; i < nodes.Length + Operation_Common.LEAF_NODE_IS_STATE; i++)
            {
                currentMachine = GetChildMachine(currentMachine, nodes[i]);
                if (null == currentMachine) { throw new UnityException("無いノードが指定されたぜ☆（＾～＾）10 i=[" + i + "] node=[" + nodes[i] + "] ac.name=["+ac.name+"]"); }
            }
            return currentMachine;
        }

        private static AnimatorStateMachine GetChildMachine(AnimatorStateMachine machine, string childName)
        {
            foreach (ChildAnimatorStateMachine wrapper in machine.stateMachines)
            {
                if (wrapper.stateMachine.name == childName) { return wrapper.stateMachine; }
            }
            return null;
        }
        #endregion
    }

    /// <summary>
    /// ステートマシン・エニーステート関連
    /// </summary>
    public abstract class Operation_StatemachineAnystate
    {
        /// <summary>
        /// STATEMACHINE ANYSTATE INSERT 用
        /// ２つのステートを トランジションで結ぶ。ステートは複数指定でき、src→dst方向の総当たりで全部結ぶ。
        /// </summary>
        /// <param name="path_src">"Base Layer.JMove.JMove0" といった文字列。</param>
        public static void AddAll(AnimatorController ac, HashSet<AnimatorStateMachine> statemachines_dst, HashSet<AnimatorState> states_dst, StringBuilder message)
        {
            foreach (AnimatorStateMachine statemachine_src in statemachines_dst)
            {
                foreach (AnimatorState state_dst in states_dst)
                {
                    message.Append("Insert: Any State");
                    message.Append(" -> ");
                    message.Append(state_dst.name);

                    statemachine_src.AddAnyStateTransition(state_dst);
                }
            }
        }
    }

    /// <summary>
    /// ステート関連
    /// </summary>
    public abstract class Operation_State
    {
        public static void Update(AnimatorController ac, UpateReqeustRecord request, StringBuilder message)
        {
            AnimatorState state = Lookup(ac, request.Fullpath);
            if (null == state) { throw new UnityException("[" + request.Fullpath + "]ステートは見つからなかったぜ☆（＾～＾） ac=[" + ac.name + "]"); }

            if (Operation_Something.HasProperty(request.Name, StateRecord.Definitions, "ステート操作"))
            {
                StateRecord.Definitions[request.Name].Update(state, request, message);
            }
        }

        #region 検索
        /// <summary>
        /// パスを指定すると ステートを返す。
        /// </summary>
        /// <param name="path">"Base Layer.JMove.JMove0" といった文字列。</param>
        public static AnimatorState Lookup(AnimatorController ac, string path)
        {
            string[] nodes = path.Split('.');
            // [0～length-2] ステートマシン名
            // [length-1] ステート名

            if (nodes.Length < 2) { throw new UnityException("ノード数が２つ未満だったぜ☆（＾～＾） ステートマシン名か、ステート名は無いのかだぜ☆？ path=["+ path + "]"); }

            // 最初の名前[0]は、レイヤーを検索する。
            AnimatorStateMachine currentMachine = null;
            foreach (AnimatorControllerLayer layer in ac.layers)
            {
                if (nodes[0] == layer.name) { currentMachine = layer.stateMachine; break; }
            }
            if (null == currentMachine) { throw new UnityException("見つからないぜ☆（＾～＾）nodes=[" + string.Join("][", nodes) + "]"); }

            if (2 < nodes.Length) // ステートマシンが途中にある場合、最後のステートマシンまで降りていく。
            {
                currentMachine = GetLeafMachine(currentMachine, nodes);
                if (null == currentMachine) { throw new UnityException("無いノードが指定されたぜ☆（＾～＾）9 currentMachine.name=[" + currentMachine.name + "] nodes=[" + string.Join("][", nodes) + "]"); }
            }

            return GetChildState(currentMachine, nodes[nodes.Length - 1]); // レイヤーと葉だけの場合
        }

        /// <summary>
        /// 分かりづらいが、ノードの[1]～[length-1]を辿って、最後のステートマシンを返す。
        /// </summary>
        private static AnimatorStateMachine GetLeafMachine(AnimatorStateMachine currentMachine, string[] nodes)
        {
            for (int i = Operation_Common.ROOT_NODE_IS_LAYER; i < nodes.Length + Operation_Common.LEAF_NODE_IS_STATE; i++)
            {
                currentMachine = GetChildMachine(currentMachine, nodes[i]);
                if (null == currentMachine) { throw new UnityException("無いノードが指定されたぜ☆（＾～＾）10 i=[" + i + "] node=[" + nodes[i] + "]"); }
            }
            return currentMachine;
        }

        private static AnimatorStateMachine GetChildMachine(AnimatorStateMachine machine, string childName)
        {
            foreach (ChildAnimatorStateMachine wrapper in machine.stateMachines)
            {
                if (wrapper.stateMachine.name == childName) { return wrapper.stateMachine; }
            }
            return null;
        }

        private static AnimatorState GetChildState(AnimatorStateMachine machine, string stateName)
        {
            foreach (ChildAnimatorState wrapper in machine.states)
            {
                if (wrapper.state.name == stateName) { return wrapper.state; }
            }
            return null;
        }
        #endregion

        /// <summary>
        /// ステートマシンに、ステートを追加する。
        /// </summary>
        public static void AddAll(AnimatorController ac, HashSet<AnimatorStateMachine> statemachines, Dictionary<string,string> set, StringBuilder message)
        {
            foreach (AnimatorStateMachine statemachine in statemachines)
            {
                foreach (string name in set.Values)// プロパティー名は見ない。
                {
                    message.Append("Insert: "); message.AppendLine(name);
                    statemachine.AddState(name);
                }
            }
        }

        /// <summary>
        /// ステートマシンから、ステートを削除する。
        /// </summary>
        public static void RemoveAll(AnimatorController ac, HashSet<AnimatorStateMachine> statemachines, Dictionary<string, string> set, StringBuilder message)
        {
            foreach (AnimatorStateMachine statemachine in statemachines)
            {
                foreach (string name in set.Values)// プロパティー名は見ない。
                {
                    message.Append("Remove: "); message.AppendLine(name);
                    foreach (ChildAnimatorState caState in statemachine.states)
                    {
                        if (caState.state.name == name)
                        {
                            statemachine.RemoveState(caState.state);
                        }
                    }
                }
            }
        }

        public static void UpdateProperty(AnimatorController ac, Dictionary<string,string> properties, HashSet<AnimatorState> states, StringBuilder message)
        {
            foreach (AnimatorState state in states) // 指定されたステート全て対象
            {
                message.Append("Update: "); message.AppendLine(state.name);
                foreach (KeyValuePair<string,string> pair in properties)
                {
                    switch (pair.Key)
                    {
                        // state.behaviours (not primal)
                        case "cycleOffset": state.cycleOffset = float.Parse(pair.Value); break;
                        case "cycleOffsetParameter": state.cycleOffsetParameter = pair.Value; break;
                        case "cycleOffsetParameterActive": state.cycleOffsetParameterActive = bool.Parse(pair.Value); break;
                        // state.hideFlags (not primal)
                        case "iKOnFeet": state.iKOnFeet = bool.Parse(pair.Value); break;
                        case "mirror": state.mirror = bool.Parse(pair.Value); break;
                        case "mirrorParameter": state.mirrorParameter = pair.Value; break;
                        case "mirrorParameterActive": state.mirrorParameterActive = bool.Parse(pair.Value); break;
                        // state.motion (not primal)
                        case "name": state.name = pair.Value; break;
                        // state.nameHash (read only)
                        case "speed": state.speed = float.Parse(pair.Value); break;
                        case "speedParameter": state.speedParameter = pair.Value; break;
                        case "speedParameterActive": state.speedParameterActive = bool.Parse(pair.Value); break;
                        case "tag": state.tag = pair.Value; break;
                        // state.transitions (not primal)
                        // state.uniqueName (deprecated)
                        // state.uniqueNameHash (deprecated)
                        case "writeDefaultValues": state.writeDefaultValues = bool.Parse(pair.Value); break;
                        default: throw new UnityException("未対応のステート・プロパティー ["+ pair.Key+ "]=[" + pair.Value + "]");
                    }
                }
            }
        }

        public static void Select(AnimatorController ac, HashSet<AnimatorState> states, out HashSet<StateRecord> recordSet, StringBuilder message)
        {
            recordSet = new HashSet<StateRecord>();
            foreach (AnimatorState state in states) // 指定されたステート全て対象
            {
                if (null == state) { throw new UnityException("ヌル・ステートが含まれていたぜ☆（＞＿＜）"); }
                recordSet.Add(new StateRecord(0,0,0,"#NoData",state));
            }
            message.Append("result: "); message.Append(recordSet.Count); message.AppendLine(" records.");
        }
    }

    public abstract class Operation_ChildState
    {
        #region 検索
        /// <summary>
        /// パスを指定すると ステートを返す。
        /// </summary>
        /// <param name="path">"Base Layer.JMove.JMove0" といった文字列。</param>
        public static ChildAnimatorState Lookup(AnimatorController ac, string path)
        {
            string[] nodes = path.Split('.');
            // [0～length-2] ステートマシン名
            // [length-1] ステート名

            if (nodes.Length < 2) { throw new UnityException("ノード数が２つ未満だったぜ☆（＾～＾） ステートマシン名か、ステート名は無いのかだぜ☆？ path=[" + path + "]"); }

            // 最初の名前[0]は、レイヤーを検索する。
            AnimatorStateMachine currentMachine = null;
            foreach (AnimatorControllerLayer layer in ac.layers)
            {
                if (nodes[0] == layer.name) { currentMachine = layer.stateMachine; break; }
            }
            if (null == currentMachine) { throw new UnityException("見つからないぜ☆（＾～＾）nodes=[" + string.Join("][", nodes) + "]"); }

            if (2 < nodes.Length) // ステートマシンが途中にある場合、最後のステートマシンまで降りていく。
            {
                currentMachine = GetLeafMachine(currentMachine, nodes);
                if (null == currentMachine) { throw new UnityException("無いノードが指定されたぜ☆（＾～＾）9 currentMachine.name=[" + currentMachine.name + "] nodes=[" + string.Join("][", nodes) + "]"); }
            }

            return GetChildState(currentMachine, nodes[nodes.Length - 1]); // レイヤーと葉だけの場合
        }

        /// <summary>
        /// 分かりづらいが、ノードの[1]～[length-1]を辿って、最後のステートマシンを返す。
        /// </summary>
        private static AnimatorStateMachine GetLeafMachine(AnimatorStateMachine currentMachine, string[] nodes)
        {
            for (int i = Operation_Common.ROOT_NODE_IS_LAYER; i < nodes.Length + Operation_Common.LEAF_NODE_IS_STATE; i++)
            {
                currentMachine = GetChildMachine(currentMachine, nodes[i]);
                if (null == currentMachine) { throw new UnityException("無いノードが指定されたぜ☆（＾～＾）10 i=[" + i + "] node=[" + nodes[i] + "]"); }
            }
            return currentMachine;
        }

        private static AnimatorStateMachine GetChildMachine(AnimatorStateMachine machine, string childName)
        {
            foreach (ChildAnimatorStateMachine wrapper in machine.stateMachines)
            {
                if (wrapper.stateMachine.name == childName) { return wrapper.stateMachine; }
            }
            return null;
        }

        private static ChildAnimatorState GetChildState(AnimatorStateMachine machine, string stateName)
        {
            foreach (ChildAnimatorState wrapper in machine.states)
            {
                if (wrapper.state.name == stateName) { return wrapper; }
            }
            throw new UnityException("チャイルド・A・ステートが見つからないぜ☆（＾～＾） stateName=["+ stateName + "]");
        }
        #endregion
    }

    /// <summary>
    /// トランジション関連
    /// </summary>
    public abstract class Operation_Transition
    {
        #region 検索
        public static AnimatorStateTransition Lookup(AnimatorController ac, UpateReqeustRecord request)
        {
            if (null == request.FullpathTransition) { throw new UnityException("トランジション番号が指定されていないぜ☆（＾～＾） トランジション番号=[" + request.FullpathTransition + "] ac=[" + ac.name + "]"); }
            int fullpathTransition = int.Parse(request.FullpathTransition);

            AnimatorState state = Operation_State.Lookup(ac, request.Fullpath);
            if (null == state) { throw new UnityException("[" + request.Fullpath + "]ステートは見つからなかったぜ☆（＾～＾） ac=[" + ac.name + "]"); }

            int tNum = 0;
            foreach (AnimatorStateTransition transition in state.transitions)
            {
                if (fullpathTransition == tNum)
                {
                    return transition;
                }
                tNum++;
            }

            // TODO:
            return null;
        }

        /// <summary>
        /// ２つのステートを トランジションで結ぶ。
        /// </summary>
        /// <param name="path_src">"Base Layer.JMove.JMove0" といった文字列。</param>
        public static AnimatorStateTransition Lookup(AnimatorController ac, string path_src, string path_dst)
        {
            AnimatorState state_src = Operation_State.Lookup(ac, path_src);
            AnimatorState state_dst = Operation_State.Lookup(ac, path_dst);

            foreach (AnimatorStateTransition transition in state_src.transitions)
            {
                if (transition.destinationState.name == state_dst.name)
                {
                    return transition;
                }
            }
            return null;
        }
        #endregion

        public static void Update(AnimatorController ac, UpateReqeustRecord record, StringBuilder message)
        {
            if (Operation_Something.HasProperty(record.Name, TransitionRecord.Definitions, "トランジション操作"))
            {
                AnimatorState state = Operation_State.Lookup(ac, record.Fullpath);
                if (null == state) { throw new UnityException("[" + record.Fullpath + "]ステートは見つからなかったぜ☆（＾～＾） ac=[" + ac.name + "]"); }

                int transitionNum = int.Parse(record.FullpathTransition); // トランジション番号

                int num = 0;
                foreach( AnimatorStateTransition transition in state.transitions)
                {
                    if (transitionNum==num)
                    {
                        TransitionRecord.Definitions[record.Name].Update(transition, record, message);
                        break;
                    }
                    num++;
                }
            }
        }

        /// <summary>
        /// ステートマシンの[Any State]からステートへ、トランジションで結ぶ。
        /// </summary>
        public static bool AddAnyState(AnimatorController ac, HashSet<AnimatorStateMachine> statemachines_src, HashSet<AnimatorState> states_dst, StringBuilder message)
        {
            message.Append("Transition.AddAnyState: Source "); message.Append(statemachines_src.Count); message.Append(" states. Destination "); message.Append(states_dst.Count); message.AppendLine(" states.");
            foreach (AnimatorStateMachine statemachine_src in statemachines_src)
            {
                foreach (AnimatorState state_dst in states_dst)
                {
                    message.Append("Insert: "); message.Append(null== statemachine_src ? "ヌル" : statemachine_src.name); message.Append(" -> "); message.AppendLine(null== state_dst ? "ヌル" : state_dst.name);
                    if (null == statemachine_src) { return false; }
                    statemachine_src.AddAnyStateTransition(state_dst);
                }
            }
            message.AppendLine();
            message.AppendLine("----------");
            message.AppendLine("(!)Mension");
            message.AppendLine("----------");
            message.AppendLine("Please, Close window and Open window");
            message.AppendLine(" (Re Open) Animation Controller!!");
            message.AppendLine("If not paint line.");
            message.AppendLine();
            return true;
        }

        /// <summary>
        /// ステートマシンの[Entry]からステートへ、トランジションで結ぶ。
        /// </summary>
        public static bool AddEntryState(AnimatorController ac, HashSet<AnimatorStateMachine> statemachines_src, HashSet<AnimatorState> states_dst, StringBuilder message)
        {
            message.Append("Transition.AddEntryState: Source "); message.Append(statemachines_src.Count); message.Append(" states. Destination "); message.Append(states_dst.Count); message.AppendLine(" states.");
            foreach (AnimatorStateMachine statemachine_src in statemachines_src)
            {
                foreach (AnimatorState state_dst in states_dst)
                {
                    message.Append("Insert: "); message.Append(null == statemachine_src ? "ヌル" : statemachine_src.name); message.Append(" -> "); message.AppendLine(null == state_dst ? "ヌル" : state_dst.name);
                    if (null == statemachine_src) { return false; }
                    statemachine_src.AddEntryTransition(state_dst);
                }
            }
            return true;
        }

        /// <summary>
        /// ステートを[Exit]へ、トランジションで結ぶ。
        /// </summary>
        public static bool AddExitState(AnimatorController ac, HashSet<AnimatorState> states, StringBuilder message)
        {
            message.Append("Transition.AddExitState: "); message.Append(states.Count); message.AppendLine(" states.");
            foreach (AnimatorState state_src in states)
            {
                message.Append("Insert: "); message.Append(null == state_src ? "ヌル" : state_src.name); message.Append(" -> Exit");;
                if (null == state_src) { return false; }
                state_src.AddExitTransition();
            }
            return true;
        }

        /// <summary>
        /// ２つのステートを トランジションで結ぶ。ステートは複数指定でき、src→dst方向の総当たりで全部結ぶ。
        /// </summary>
        /// <param name="path_src">"Base Layer.JMove.JMove0" といった文字列。</param>
        public static void Insert(AnimatorController ac, HashSet<AnimatorState> states_src, HashSet<AnimatorState> states_dst, Dictionary<string, string> properties, StringBuilder message)
        {
            message.Append("Transition.Insert: Source "); message.Append(states_src.Count); message.Append(" states. Destination "); message.Append(states_dst.Count); message.AppendLine(" states.");
            foreach (AnimatorState state_src in states_src) {
                foreach (AnimatorState state_dst in states_dst) {
                    message.Append("Insert: "); message.Append(state_src.name); message.Append(" -> "); message.AppendLine(state_dst.name);
                    AnimatorStateTransition transition = state_src.AddTransition(state_dst);
                    UpdateProperty(ac, transition, properties);
                }
            }
        }

        /// <summary>
        /// ２つのステート間の トランジションを削除する。ステートは複数指定でき、src→dst方向の総当たりで全部削除する。
        /// </summary>
        /// <param name="path_src">"Base Layer.JMove.JMove0" といった文字列。</param>
        public static void RemoveAll(AnimatorController ac, HashSet<AnimatorState> states_src, HashSet<AnimatorState> states_dst, StringBuilder message)
        {
            message.Append("Transition.RemoveAll: Source "); message.Append(states_src.Count); message.Append(" states. Destination "); message.Append(states_dst.Count); message.AppendLine(" states.");

            foreach (AnimatorState state_src in states_src)
            {
                foreach (AnimatorState state_dst in states_dst)
                {
                    foreach (AnimatorStateTransition transition in state_src.transitions)
                    {
                        if (state_dst == transition.destinationState)
                        {
                            state_src.RemoveTransition(transition);
                            message.Append("deleted: ");
                            message.Append(state_src.name);
                            message.Append(" -> ");
                            message.Append(state_dst.name);
                            message.AppendLine();
                            // break; // src → dst 間に複数のトランジションを貼れるみたいなんで、全部消そう☆
                        }
                    }
                }
            }
        }

        public static void Update(AnimatorController ac, HashSet<AnimatorState> states_src, HashSet<AnimatorState> states_dst, Dictionary<string, string> properties, StringBuilder message)
        {
            foreach (AnimatorState state_src in states_src) // 指定されたステート全て対象
            {
                foreach (AnimatorStateTransition transition in state_src.transitions)
                {
                    foreach (AnimatorState state_dst in states_dst) // 指定されたステート全て対象
                    {
                        if (state_dst == transition.destinationState)
                        {
                            message.Append("Update: ");
                            message.Append(state_src.name);
                            message.Append(" -> ");
                            message.Append(state_dst.name);
                            UpdateProperty(ac, transition, properties);
                            //foreach (KeyValuePair<string, string> pair in properties)
                            //{
                            //    switch (pair.Key)
                            //    {
                            //        case "canTransitionToSelf": transition.canTransitionToSelf = bool.Parse(pair.Value); break;
                            //        // transition.conditions (not primal)
                            //        // transition.destinationState (not primal)
                            //        // transition.destinationStateMachine (not primal)
                            //        case "duration": transition.duration = float.Parse(pair.Value); break;
                            //        case "exitTime": transition.exitTime = float.Parse(pair.Value); break;
                            //        case "hasExitTime": transition.hasExitTime = bool.Parse(pair.Value); break;
                            //        case "hasFixedDuration": transition.hasFixedDuration = bool.Parse(pair.Value); break;
                            //        // transition.hideFlags (not primal)
                            //        // transition.interruptionSource (not primal)
                            //        case "isExit": transition.isExit = bool.Parse(pair.Value); break;
                            //        case "mute": transition.mute = bool.Parse(pair.Value); break;
                            //        case "name": transition.name = pair.Value; break;
                            //        case "offset": transition.offset = float.Parse(pair.Value); break;
                            //        case "orderedInterruption": transition.orderedInterruption = bool.Parse(pair.Value); break;
                            //        case "solo": transition.solo = bool.Parse(pair.Value); break;
                            //        default: throw new UnityException("未対応のトランジション・プロパティー [" + pair.Key + "]=[" + pair.Value + "]");
                            //    }
                            //}
                        }
                    }
                }
            }
        }
        public static void UpdateProperty(AnimatorController ac, AnimatorStateTransition transition, Dictionary<string, string> properties)
        {
            foreach (KeyValuePair<string, string> pair in properties)
            {
                switch (pair.Key)
                {
                    case "canTransitionToSelf": transition.canTransitionToSelf = bool.Parse(pair.Value); break;
                    // transition.conditions (not primal)
                    // transition.destinationState (not primal)
                    // transition.destinationStateMachine (not primal)
                    case "duration": transition.duration = float.Parse(pair.Value); break;
                    case "exitTime": transition.exitTime = float.Parse(pair.Value); break;
                    case "hasExitTime": transition.hasExitTime = bool.Parse(pair.Value); break;
                    case "hasFixedDuration": transition.hasFixedDuration = bool.Parse(pair.Value); break;
                    // transition.hideFlags (not primal)
                    // transition.interruptionSource (not primal)
                    case "isExit": transition.isExit = bool.Parse(pair.Value); break;
                    case "mute": transition.mute = bool.Parse(pair.Value); break;
                    case "name": transition.name = pair.Value; break;
                    case "offset": transition.offset = float.Parse(pair.Value); break;
                    case "orderedInterruption": transition.orderedInterruption = bool.Parse(pair.Value); break;
                    case "solo": transition.solo = bool.Parse(pair.Value); break;
                    default: throw new UnityException("未対応のトランジション・プロパティー [" + pair.Key + "]=[" + pair.Value + "]");
                }
            }
        }



        public static void Select(AnimatorController ac, HashSet<AnimatorState> states_src, HashSet<AnimatorState> states_dst, out HashSet<TransitionRecord> recordSet, StringBuilder message)
        {
            recordSet = new HashSet<TransitionRecord>();
            StringBuilder stellaQLComment = new StringBuilder();
            foreach (AnimatorState state_src in states_src) // 指定されたステート全て対象
            {
                foreach (AnimatorStateTransition transition in state_src.transitions)
                {
                    foreach (AnimatorState state_dst in states_dst) // 指定されたステート全て対象
                    {
                        if (state_dst == transition.destinationState)
                        {
                            stellaQLComment.Append("Select: "); stellaQLComment.Append(state_src.name); stellaQLComment.Append(" -> "); stellaQLComment.AppendLine(state_dst.name);
                            recordSet.Add(new TransitionRecord(0,0,0,0,transition, stellaQLComment.ToString()));
                            stellaQLComment.Length = 0;
                        }
                    }
                }
            }
            message.Append("Select: result "); message.Append(recordSet.Count); message.AppendLine(" transitions.");;
        }
    }

    /// <summary>
    /// コンディション関連
    /// </summary>
    public abstract class Operation_Condition
    {
        #region 検索
        public static ConditionRecord.AnimatorConditionWrapper Lookup(AnimatorController ac, UpateReqeustRecord request)
        {
            AnimatorStateTransition transition = Operation_Transition.Lookup(ac, request);
            if(null!= transition)
            {
                return Operation_Condition.Lookup(ac, request);
            }

            // TODO:
            return null;
        }

        public static ConditionRecord.AnimatorConditionWrapper Lookup(AnimatorController ac, AnimatorStateTransition transition, UpateReqeustRecord request)
        {
            int fullpathCondition = int.Parse(request.FullpathCondition);

            int cNum = 0;
            foreach (AnimatorCondition condition in transition.conditions)
            {
                if (fullpathCondition == cNum)
                {
                    return new ConditionRecord.AnimatorConditionWrapper(condition);
                }
                cNum++;
            }

            return new ConditionRecord.AnimatorConditionWrapper(); // 空コンストラクタで生成した場合、.IsNull( ) メソッドでヌルを返す。
        }

        //public static ConditionRecord Lookup(AconData aconData, int lNum, int msNum, int sNum, int tNum, int cNum)
        //{
        //    foreach (ConditionRecord cRecord in aconData.table_condition)
        //    {
        //        if ((int)cRecord.Fields["#layerNum#"] == lNum &&
        //            (int)cRecord.Fields["#machineStateNum#"] == msNum &&
        //            (int)cRecord.Fields["#stateNum#"] == sNum &&
        //            (int)cRecord.Fields["#transitionNum#"] == tNum &&
        //            (int)cRecord.Fields["#conditionNum#"] == cNum                    
        //            )
        //        {
        //            return cRecord;
        //        }
        //    }
        //    return null;
        //}
        #endregion

        public class UpateReqeustRecordSet
        {
            public UpateReqeustRecordSet(int fullpathTransition_forDebug, int fullpathCondition_forDebug)
            {
                this.FullpathTransition_forDebug = fullpathTransition_forDebug;
                this.FullpathCondition_forDebug = fullpathCondition_forDebug;
            }
            //public UpateReqeustRecordSet(UpateReqeustRecord mode, UpateReqeustRecord threshold, UpateReqeustRecord parameter, int fullpathCondition_forDebug)
            //{
            //    this.Mode = mode;
            //    this.Threshold = threshold;
            //    this.Parameter = parameter;
            //    this.FullpathCondition_ForDebug = fullpathCondition_forDebug;
            //}

            /// <summary>
            /// エラーを追跡するための情報として記録。
            /// </summary>
            public int FullpathTransition_forDebug { get; set; }
            /// <summary>
            /// エラーを追跡するための情報として記録。
            /// </summary>
            public int FullpathCondition_forDebug { get; set; }
            public UpateReqeustRecord Mode { get; set; }
            public UpateReqeustRecord Threshold { get; set; }
            public UpateReqeustRecord Parameter { get; set; }

            /// <summary>
            /// どれか１つしか更新要求されていないことがあるので、設定されているレコードを引っ張り出す。
            /// </summary>
            public UpateReqeustRecord RepresentativeRecord {
                get {
                    if (null != Mode) { return Mode; }
                    else if (null != Threshold) { return Threshold; }
                    else if (null != Parameter) { return Parameter; }
                    throw new UnityException(this.Dump_Error("RepresentativeRecord"));
                }
            }
            public string RepresentativeName { get { return RepresentativeRecord.Name; } }
            public string RepresentativeFullpath { get { return RepresentativeRecord.Fullpath; } }
            public int RepresentativeFullpathTransition { get { return int.Parse(RepresentativeRecord.FullpathTransition); } }
            public int RepresentativeFullpathCondition { get { return int.Parse(RepresentativeRecord.FullpathCondition); } }

            public bool TryParameterValue(out string parameter) {
                if (null == Parameter) { parameter = ""; return false; }
                if (""!=Parameter.New) { parameter = Parameter.New; return true; }
                parameter = this.Parameter.Old; return true;
            }
            public bool TryModeValue(out AnimatorConditionMode mode) {
                if (null==Mode) { mode = 0; return false; }
                if ("" != Mode.New) { mode = ConditionRecord.String_to_mode(Mode.New); return true; }
                mode = ConditionRecord.String_to_mode(this.Mode.Old); return true;
            }
            public bool TryThresholdValue(out float threshold) {
                if (null == Threshold) { threshold = 0.0f; return false; }
                if ("" != Threshold.New) { threshold = float.Parse(Threshold.New); return true; }
                threshold = float.Parse(this.Threshold.Old); return true;
            }

            public string Dump_Error(string calling)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("エラー : " + calling + " ");
                sb.AppendLine("fullpathTransition=[" + this.FullpathTransition_forDebug + "]");
                sb.AppendLine("fullpathCondition=[" + this.FullpathCondition_forDebug + "]");
                if (null == Mode) { sb.AppendLine("Modeがヌル☆（＞＿＜）"); }
                if (null == Threshold) { sb.AppendLine("Thresholdがヌル☆（＞＿＜）"); }
                if (null == Parameter) { sb.AppendLine("Parameterがヌル☆（＞＿＜）"); }
                return sb.ToString();
            }
        }


        public static void Update(AnimatorController ac, UpateReqeustRecordSet requestSet, StringBuilder message)
        {
            AnimatorState state = Operation_State.Lookup(ac, requestSet.RepresentativeFullpath);
            if (null == state) { throw new UnityException("[" + requestSet.RepresentativeFullpath + "]ステートは見つからなかったぜ☆（＾～＾） ac.name=[" + ac.name + "]"); }

            int transitionNum = requestSet.RepresentativeFullpathTransition; // トランジション番号
            int conditionNum = requestSet.RepresentativeFullpathCondition; // コンディション番号

            if (Operation_Something.HasProperty(requestSet.RepresentativeName, ConditionRecord.Definitions, "コンディション操作"))
            {
                // トランジション
                AnimatorStateTransition transition = Operation_Transition.Lookup(ac, requestSet.RepresentativeRecord);
                ConditionRecord.AnimatorConditionWrapper wapper = Operation_Condition.Lookup(ac, transition, requestSet.RepresentativeRecord);

                if(wapper.IsNull)
                {
                    // 存在しないコンディション番号だった場合、新規追加する。
                    Insert(transition, requestSet, message);//"velY", AnimatorConditionMode.Greater, 123.456f
                }
                else
                {
                    // 既存のコンディションなら、更新する。
                    if (null != requestSet.Parameter) { ConditionRecord.Definitions[requestSet.RepresentativeName].Update(new ConditionRecord.AnimatorConditionWrapper(wapper.m_source), requestSet.Parameter, message); }
                    if (null != requestSet.Mode) { ConditionRecord.Definitions[requestSet.RepresentativeName].Update(new ConditionRecord.AnimatorConditionWrapper(wapper.m_source), requestSet.Mode, message); }
                    if (null != requestSet.Threshold) { ConditionRecord.Definitions[requestSet.RepresentativeName].Update(new ConditionRecord.AnimatorConditionWrapper(wapper.m_source), requestSet.Threshold, message); }
                }



                //int tNum = 0; // トランジションの何番目か
                //foreach (AnimatorStateTransition transition in state.transitions)
                //{
                //    if (transitionNum == tNum)
                //    {
                //int cNum = 0; // コンディションの何番目か
                //        foreach(AnimatorCondition condition in transition.conditions)
                //        {
                //            if (conditionNum == cNum)
                //            {
                //if (null != recordSet.Parameter) { ConditionRecord.Definitions[recordSet.Name].Update(new ConditionRecord.AnimatorConditionWrapper(condition), recordSet.Parameter, message); }
                //                if (null != recordSet.Mode) { ConditionRecord.Definitions[recordSet.Name].Update(new ConditionRecord.AnimatorConditionWrapper(condition), recordSet.Mode, message); }
                //                if (null != recordSet.Threshold) { ConditionRecord.Definitions[recordSet.Name].Update(new ConditionRecord.AnimatorConditionWrapper(condition), recordSet.Threshold, message); }
                //                goto gt_EndLoop; // 2重ループを脱出
                //    }
                //    cNum++;
                //}
                //// 存在しないコンディション番号だった場合、新規追加する。
                //Insert(transition, recordSet, message);//"velY", AnimatorConditionMode.Greater, 123.456f
                //cNum++;
                //throw new UnityException("存在しないコンディション番号だったぜ☆（＾～＾） 指定コンディション番号=["+ conditionNum + "] ループカウンター=["+cNum+"] コンディション数=["+ transition.conditions.Length + "]");
                //    }
                //    tNum++;
                //}
                //throw new UnityException("存在しないトランジション番号だったぜ☆（＾～＾） record.Fullpath=["+ recordSet.Fullpath + "] ヒットしたステート=[" + state.name+"]  指定トランジション番号=[" + transitionNum + "] ループカウンター=["+tNum+"] トランジション数=[" + state.transitions.Length + "] ac.name=[" + ac.name + "]");
                //gt_EndLoop:
                //;
            }
        }

        /// <summary>
        /// コンディションを新規作成します。
        /// </summary>
        /// <param name="transition"></param>
        /// <param name="name"></param>
        /// <param name="operatorFlag"></param>
        /// <param name="value"></param>
        /// <param name="message"></param>
        public static void Insert(AnimatorStateTransition transition, UpateReqeustRecordSet requestSet, StringBuilder message)
        {
            // float型引数の場合、使える演算子は Greater か less のみ。
            // int型引数の場合、使える演算子は Greater、less、Equals、NotEqual のいずれか。
            // bool型引数の場合、使える演算子は表示上は true、false だが、内部的には推測するに If、IfNot の２つだろうか？

            AnimatorConditionMode mode;
            if (requestSet.TryModeValue(out mode)){
                Debug.Log("FIXME: Insert mode");
            }

            float threshold;
            if (requestSet.TryThresholdValue(out threshold)){
                Debug.Log("FIXME: Insert threshold");
            }

            string parameter;
            if (requestSet.TryParameterValue(out parameter)){
                Debug.Log("FIXME: Insert parameter");
            }

            transition.AddCondition(mode, threshold, parameter);
        }
    }

    /// <summary>
    /// ポジション操作
    /// </summary>
    public abstract class Operation_Position
    {
        public static void Update(AnimatorController ac, UpateReqeustRecord request, StringBuilder message)
        {
            if ("stateMachines" == request.Foreignkeycategory)
            {
                // ステートマシンのポジション
                AnimatorStateMachine statemachine = Operation_Statemachine.Lookup(ac, request.Fullpath);
                if (null == statemachine) { throw new UnityException("[" + request.Fullpath + "]ステートマシンは見つからなかったぜ☆（＾～＾） ac=[" + ac.name + "]"); }

                if (Operation_Something.HasProperty(request.Name, TransitionRecord.Definitions, "ステートマシンのポジション操作"))
                {
                    PositionRecord.Definitions[request.Name].Update(new PositionRecord.PositionWrapper(statemachine, request.FullpathPropertyname), request, message);
                }
            }
            else // ステートのポジション
            {
                ChildAnimatorState caState = Operation_ChildState.Lookup(ac, request.Fullpath); // 構造体☆

                if ("states" == request.Foreignkeycategory)
                {
                    if (Operation_Something.HasProperty(request.Name, TransitionRecord.Definitions, "ステートのポジション操作"))
                    {
                        PositionRecord.Definitions[request.Name].Update(new PositionRecord.PositionWrapper(caState, request.FullpathPropertyname), request, message);
                    }
                }
                else
                {
                    // ステートマシン、ステート以外のポジション
                    throw new UnityException("ステートマシン、ステート以外のポジションは未対応だぜ☆（＾～＾） fullpath=[" + request.Fullpath + "] ac=[" + ac.name + "]");
                }
            }
        }
    }

    public abstract class Operation_Common
    {
        /// <summary>
        /// ノードの最初の１つは　レイヤー番号
        /// </summary>
        public const int ROOT_NODE_IS_LAYER = 1;
        /// <summary>
        /// ノードの最後の１つは　ステート名
        /// </summary>
        public const int LEAF_NODE_IS_STATE = -1;
    }
}
