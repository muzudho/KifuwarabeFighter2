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
        public static void ManipulateData(AnimatorController ac, AconData aconData_old, HashSet<DataManipulationRecord> request_packets, StringBuilder info_message)
        {
            // テスト出力
            {
                StringBuilder contents = new StringBuilder();
                contents.Append("Update request ");
                contents.Append(request_packets.Count);
                contents.Append(" record sets.");
                contents.AppendLine();
                foreach (DataManipulationRecord request_packet in request_packets)
                {
                    request_packet.ToCsvLine(contents);
                }
                Debug.Log(contents.ToString());
            }

            // 更新要求を、届け先別に仕分ける。コレクションを入れ子にし、フルパスで更新要求を仕分ける。
            // 二重構造 [ステート・フルパス、トランジション番号]
            Dictionary<string, Dictionary<int, DataManipulationRecord>> largeDic_transition = new Dictionary<string, Dictionary<int, DataManipulationRecord>>();
            // 三重構造 [ステート・フルパス、トランジション番号、コンディション番号]
            Dictionary<string,Dictionary<int, Dictionary<int, Operation_Condition.DataManipulatRecordSet>>> largeDic_condition = new Dictionary<string, Dictionary<int, Dictionary<int, Operation_Condition.DataManipulatRecordSet>>>();
            foreach (DataManipulationRecord request_packet in request_packets)
            {
                switch (request_packet.Category)
                {
                    case "parameters":      Operation_Parameter     .ManipulateData(ac, request_packet, info_message); break;
                    case "layers":          Operation_Layer         .Update(ac, request_packet, info_message); break;
                    case "stateMachines":   Operation_Statemachine  .Update(ac, request_packet, info_message); break;
                    case "states":          Operation_State         .Update(ac, request_packet, info_message); break;
                    case "transitions":
                        {
                            // 二重構造
                            Dictionary<int, DataManipulationRecord> middleDic; // [ステートフルパス,]
                            if (largeDic_transition.ContainsKey(request_packet.Fullpath)) { middleDic = largeDic_transition[request_packet.Fullpath]; } // 既存
                            else { middleDic = new Dictionary<int, DataManipulationRecord>(); largeDic_transition[request_packet.Fullpath] = middleDic; } // 新規追加

                            int tNum = int.Parse(request_packet.TransitionNum_ofFullpath); // トランジション番号
                            middleDic.Add(tNum, request_packet);
                        }
                        break;
                    case "conditions":
                        {
                            Operation_Condition.DataManipulatRecordSet request_buffer; // 条件を溜め込む。 mode, parameter, threshold の３つが揃って更新ができる。

                            // 三重構造
                            Dictionary<int, Dictionary<int, Operation_Condition.DataManipulatRecordSet>> middleDic; // [ステートフルパス,,]
                            if (largeDic_condition.ContainsKey(request_packet.Fullpath)) { middleDic = largeDic_condition[request_packet.Fullpath]; } // 既存
                            else { middleDic = new Dictionary<int, Dictionary<int, Operation_Condition.DataManipulatRecordSet>>(); largeDic_condition[request_packet.Fullpath] = middleDic; } // 新規追加
                            
                            Dictionary<int, Operation_Condition.DataManipulatRecordSet> smallDic; // [ステートフルパス,トランジション番号,]
                            int tNum = int.Parse(request_packet.TransitionNum_ofFullpath);
                            if (middleDic.ContainsKey(tNum)) { smallDic = middleDic[tNum]; } // 既存
                            else { smallDic = new Dictionary<int, Operation_Condition.DataManipulatRecordSet>(); middleDic[tNum] = smallDic; } // 新規追加
                            
                            int cNum = int.Parse(request_packet.ConditionNum_ofFullpath); // [ステートフルパス,トランジション番号,コンディション番号]
                            if (smallDic.ContainsKey(cNum)) { request_buffer = smallDic[cNum]; } // 既存
                            else { request_buffer = new Operation_Condition.DataManipulatRecordSet(tNum, cNum); smallDic[cNum] = request_buffer; }// 空っぽの要求セットを新規作成・追加

                            switch (request_packet.Name) // 複数行に分かれていた命令を、１つのセットにまとめる
                            {
                                case "parameter":   request_buffer.Parameter    = request_packet; break;
                                case "mode":        request_buffer.Mode         = request_packet; break;
                                case "threshold":   request_buffer.Threshold    = request_packet; break;
                                default:            throw new UnityException("未定義のプロパティ名だぜ☆（＞＿＜） record.Name=[" + request_packet.Name + "]"); //Debug.Log("追加失敗");
                            }
                        }
                        break;
                    case "positinos": Operation_Position.Update(ac, request_packet, info_message); break;
                    default: throw new UnityException("未対応のカテゴリー=["+ request_packet.Category + "]");
                }
            }

            // トランジションを消化
            {
                // 挿入、更新、削除に振り分けたい。
                List<DataManipulationRecord> insertsSet = new List<DataManipulationRecord>();
                List<DataManipulationRecord> deletesSet = new List<DataManipulationRecord>();
                List<DataManipulationRecord> updatesSet = new List<DataManipulationRecord>();
                Debug.Log("largeDic_transition.Count=" + largeDic_transition.Count);// [ステート・フルパス,トランジション番号]
                foreach (KeyValuePair<string, Dictionary<int, DataManipulationRecord>> request_2wrap in largeDic_transition)
                {
                    Debug.Log("request_2wrap.Value.Count=" + request_2wrap.Value.Count);// [,トランジション番号]
                    foreach (KeyValuePair<int, DataManipulationRecord> request_1wrap in request_2wrap.Value)
                    {
                        AnimatorStateTransition transition = Operation_Transition.Lookup(ac, request_1wrap.Value);// トランジション

                        if ("#DestinationFullpath#" == request_1wrap.Value.Name)
                        {
                            if ("" != request_1wrap.Value.New) { insertsSet.Add(request_1wrap.Value); }// Newに指定があれば、 挿入 に振り分ける。
                            else if (request_1wrap.Value.IsDelete) { deletesSet.Add(request_1wrap.Value); }// 削除要求の場合、削除 に振り分ける。
                            else {
                                if (HasProperty(request_1wrap.Value.Name, TransitionRecord.Definitions, "トランジション操作"))
                                {
                                    updatesSet.Add(request_1wrap.Value);
                                }
                            }// それ以外の場合は、更新 に振り分ける。
                        }
                    }
                }

                foreach (DataManipulationRecord request in insertsSet) { Operation_Transition.Insert(ac, request, info_message); }// 更新を処理
                deletesSet.Sort((DataManipulationRecord a, DataManipulationRecord b) =>
                { // 削除要求を、連番の逆順にする
                    int stringCompareOrder = string.CompareOrdinal(a.Fullpath, b.Fullpath);
                    if (0 != stringCompareOrder) { return stringCompareOrder; } // ステート名の順番はそのまま。
                    else if (int.Parse(a.TransitionNum_ofFullpath) < int.Parse(b.TransitionNum_ofFullpath)) { return -1; } // トランジションの順番は一応後ろから
                    else if (int.Parse(b.TransitionNum_ofFullpath) < int.Parse(a.TransitionNum_ofFullpath)) { return 1; }
                    return 0;
                });
                foreach (DataManipulationRecord request in deletesSet) { Operation_Transition.Delete(ac, request, info_message); }// 削除を処理
                foreach (DataManipulationRecord request in updatesSet) { Operation_Transition.Update(ac, request, info_message); }// 挿入を処理
            }
            // コンディションを消化
            {
                // 挿入、更新、削除に振り分けたい。
                List<Operation_Condition.DataManipulatRecordSet> insertsSet = new List<Operation_Condition.DataManipulatRecordSet>();
                List<Operation_Condition.DataManipulatRecordSet> deletesSet = new List<Operation_Condition.DataManipulatRecordSet>();
                List<Operation_Condition.DataManipulatRecordSet> updatesSet = new List<Operation_Condition.DataManipulatRecordSet>();
                Debug.Log("conditionRecordSet.Count=" + largeDic_condition.Count);// [ステート・フルパス,トランジション番号,コンディション番号]
                foreach (KeyValuePair<string, Dictionary<int, Dictionary<int, Operation_Condition.DataManipulatRecordSet>>> conditionRecordSetPair in largeDic_condition)
                {
                    Debug.Log("conditionRecordSetPair.Value.Count=" + conditionRecordSetPair.Value.Count);// [,トランジション番号,コンディション番号]
                    foreach (KeyValuePair<int, Dictionary<int, Operation_Condition.DataManipulatRecordSet>> conditionRecordSet1Pair in conditionRecordSetPair.Value)
                    {
                        Debug.Log("conditionRecordSet1Pair.Value.Count=" + conditionRecordSet1Pair.Value.Count);// [,,コンディション番号]
                        foreach (KeyValuePair<int, Operation_Condition.DataManipulatRecordSet> conditionRecordSet2Pair in conditionRecordSet1Pair.Value)
                        {
                            if (Operation_Something.HasProperty(conditionRecordSet2Pair.Value.RepresentativeName, ConditionRecord.Definitions, "コンディション操作"))
                            {
                                AnimatorStateTransition transition = Operation_Transition.Lookup(ac, conditionRecordSet2Pair.Value.RepresentativeRecord);// トランジション
                                ConditionRecord.AnimatorConditionWrapper wapper = Operation_Condition.Lookup(ac, transition, conditionRecordSet2Pair.Value.RepresentativeRecord);// コンディション

                                if (wapper.IsNull) { insertsSet.Add(conditionRecordSet2Pair.Value); }// 存在しないコンディション番号だった場合、 挿入 に振り分ける。
                                else if (null != conditionRecordSet2Pair.Value.Parameter && conditionRecordSet2Pair.Value.Parameter.IsDelete) { deletesSet.Add(conditionRecordSet2Pair.Value); }// 削除要求の場合、削除 に振り分ける。
                                else { updatesSet.Add(conditionRecordSet2Pair.Value); }// それ以外の場合は、更新 に振り分ける。
                            }
                        }
                    }
                }

                foreach (Operation_Condition.DataManipulatRecordSet requestSet in insertsSet) { Operation_Condition.Insert(ac, requestSet, info_message); }// 更新を処理
                deletesSet.Sort((Operation_Condition.DataManipulatRecordSet a, Operation_Condition.DataManipulatRecordSet b) =>
                { // 削除要求を、連番の逆順にする
                    int stringCompareOrder = string.CompareOrdinal(a.RepresentativeFullpath, b.RepresentativeFullpath);
                    if (0 != stringCompareOrder) { return stringCompareOrder; } // ステート名の順番はそのまま。
                    else if (a.RepresentativeFullpathTransition < b.RepresentativeFullpathTransition) { return -1; } // トランジションの順番は一応後ろから
                    else if (b.RepresentativeFullpathTransition < a.RepresentativeFullpathTransition) { return 1; }
                    else if (a.RepresentativeFullpathCondition < b.RepresentativeFullpathCondition) { return -1; } // コンディションの削除は後ろから
                    else if (b.RepresentativeFullpathCondition < a.RepresentativeFullpathCondition) { return 1; }
                    return 0;
                });
                foreach (Operation_Condition.DataManipulatRecordSet requestSet in deletesSet) { Operation_Condition.Delete(ac, requestSet, info_message); }// 削除を処理
                foreach (Operation_Condition.DataManipulatRecordSet requestSet in updatesSet) { Operation_Condition.Update(ac, requestSet, info_message); }// 挿入を処理
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
        public static void ManipulateData(AnimatorController ac, DataManipulationRecord request, StringBuilder message)
        {
            throw new UnityException("[" + request.Fullpath + "]パラメーターには未対応だぜ☆（＾～＾） ac=[" + ac.name + "]");
        }
    }

    public abstract class Operation_Layer
    {
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

        public static void Update(AnimatorController ac, DataManipulationRecord request, StringBuilder message)
        {
            AnimatorControllerLayer layer = Lookup(ac, request.Fullpath);
            if (null == layer) { throw new UnityException("[" + request.Fullpath + "]レイヤーは見つからなかったぜ☆（＾～＾） ac=[" + ac.name + "]"); }

            if (Operation_Something.HasProperty(request.Name, LayerRecord.Definitions, "レイヤー操作"))
            {
                StateRecord.Definitions[request.Name].Update(layer, request, message);
            }
        }
    }

    /// <summary>
    /// ステートマシン関連
    /// </summary>
    public abstract class Operation_Statemachine
    {
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

        public static void Update(AnimatorController ac, DataManipulationRecord request, StringBuilder message)
        {
            AnimatorStateMachine statemachine = Lookup(ac, request.Fullpath);
            if (null == statemachine) { throw new UnityException("[" + request.Fullpath + "]ステートマシンは見つからなかったぜ☆（＾～＾） ac=[" + ac.name + "]"); }

            if (Operation_Something.HasProperty(request.Name, StatemachineRecord.Definitions, "ステートマシン操作"))
            {
                StateRecord.Definitions[request.Name].Update(statemachine, request, message);
            }
        }
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

        public static void Update(AnimatorController ac, DataManipulationRecord request, StringBuilder message)
        {
            AnimatorState state = Lookup(ac, request.Fullpath);
            if (null == state) { throw new UnityException("[" + request.Fullpath + "]ステートは見つからなかったぜ☆（＾～＾） ac=[" + ac.name + "]"); }

            if (Operation_Something.HasProperty(request.Name, StateRecord.Definitions, "ステート操作"))
            {
                StateRecord.Definitions[request.Name].Update(state, request, message);
            }
        }

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
        public static AnimatorStateTransition Lookup(AnimatorController ac, DataManipulationRecord request)
        {
            if (null == request.TransitionNum_ofFullpath) { throw new UnityException("トランジション番号が指定されていないぜ☆（＾～＾） トランジション番号=[" + request.TransitionNum_ofFullpath + "] ac=[" + ac.name + "]"); }
            int fullpathTransition = int.Parse(request.TransitionNum_ofFullpath);

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

        public static void Insert(AnimatorController ac, DataManipulationRecord request, StringBuilder message)
        {
            AnimatorState sourceState = Operation_State.Lookup(ac, request.Fullpath); // 遷移元のステート
            //AnimatorStateTransition sourceTransition = Operation_Transition.Lookup(ac, request); // トランジション

            // TODO: 遷移先のステートを指定する？
            string destinationFullpath = request.New;
            AnimatorState destinationState = Operation_State.Lookup(ac, destinationFullpath); // 遷移先のステート
            sourceState.AddTransition(destinationState);
        }
        public static void Update(AnimatorController ac, DataManipulationRecord request, StringBuilder message)
        {
            if (Operation_Something.HasProperty(request.Name, TransitionRecord.Definitions, "トランジション操作"))
            {
                AnimatorState state = Operation_State.Lookup(ac, request.Fullpath);
                if (null == state) { throw new UnityException("[" + request.Fullpath + "]ステートは見つからなかったぜ☆（＾～＾） ac=[" + ac.name + "]"); }

                int transitionNum = int.Parse(request.TransitionNum_ofFullpath); // トランジション番号

                int num = 0;
                foreach( AnimatorStateTransition transition in state.transitions)
                {
                    if (transitionNum==num)
                    {
                        TransitionRecord.Definitions[request.Name].Update(transition, request, message);
                        break;
                    }
                    num++;
                }
            }
        }
        /// <summary>
        /// トランジションを削除します。
        /// トランジション番号の大きい物から順に削除してください。トランジション番号の小さい物から削除すると番号が繰り上がってしまうため。
        /// </summary>
        public static void Delete(AnimatorController ac, DataManipulationRecord request, StringBuilder message)
        {
            AnimatorState sourceState = Operation_State.Lookup(ac, request.Fullpath); // 遷移元のステート
            AnimatorStateTransition sourceTransition = Lookup(ac, request); // 削除するトランジション
            sourceState.RemoveTransition(sourceTransition);
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
        public static ConditionRecord.AnimatorConditionWrapper Lookup(AnimatorController ac, DataManipulationRecord request)
        {
            AnimatorStateTransition transition = Operation_Transition.Lookup(ac, request);
            if(null!= transition)
            {
                return Operation_Condition.Lookup(ac, request);
            }

            // TODO:
            return null;
        }

        public static ConditionRecord.AnimatorConditionWrapper Lookup(AnimatorController ac, AnimatorStateTransition transition, DataManipulationRecord request)
        {
            int fullpathCondition = int.Parse(request.ConditionNum_ofFullpath);

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

        public class DataManipulatRecordSet
        {
            public DataManipulatRecordSet(int fullpathTransition_forDebug, int fullpathCondition_forDebug)
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
            public DataManipulationRecord Mode { get; set; }
            public DataManipulationRecord Threshold { get; set; }
            public DataManipulationRecord Parameter { get; set; }

            /// <summary>
            /// どれか１つしか更新要求されていないことがあるので、設定されているレコードを引っ張り出す。
            /// </summary>
            public DataManipulationRecord RepresentativeRecord {
                get {
                    if (null != Mode) { return Mode; }
                    else if (null != Threshold) { return Threshold; }
                    else if (null != Parameter) { return Parameter; }
                    throw new UnityException(this.Dump_Error("RepresentativeRecord"));
                }
            }
            public string RepresentativeName { get { return RepresentativeRecord.Name; } }
            public string RepresentativeFullpath { get { return RepresentativeRecord.Fullpath; } }
            public int RepresentativeFullpathTransition { get { return int.Parse(RepresentativeRecord.TransitionNum_ofFullpath); } }
            public int RepresentativeFullpathCondition { get { return int.Parse(RepresentativeRecord.ConditionNum_ofFullpath); } }

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

        public static void Insert(AnimatorController ac, DataManipulatRecordSet requestSet, StringBuilder message)
        {
            AnimatorStateTransition transition = Operation_Transition.Lookup(ac, requestSet.RepresentativeRecord); // １つ上のオブジェクト（トランジション）
            AnimatorConditionMode mode;     if (requestSet.TryModeValue(out mode))              { Debug.Log("FIXME: Insert mode"); }
            float threshold;                if (requestSet.TryThresholdValue(out threshold))    { Debug.Log("FIXME: Insert threshold"); }
            string parameter;               if (requestSet.TryParameterValue(out parameter))    { Debug.Log("FIXME: Insert parameter"); }
            transition.AddCondition(mode, threshold, parameter);
        }
        public static void Update(AnimatorController ac, DataManipulatRecordSet requestSet, StringBuilder message)
        {
            // float型引数の場合、使える演算子は Greater か less のみ。
            // int型引数の場合、使える演算子は Greater、less、Equals、NotEqual のいずれか。
            // bool型引数の場合、使える演算子は表示上は true、false だが、内部的には推測するに If、IfNot の２つだろうか？
            AnimatorStateTransition transition = Operation_Transition.Lookup(ac, requestSet.RepresentativeRecord);// トランジション
            ConditionRecord.AnimatorConditionWrapper wapper = Lookup(ac, transition, requestSet.RepresentativeRecord);
            if (null != requestSet.Mode) { ConditionRecord.Definitions[requestSet.RepresentativeName].Update(new ConditionRecord.AnimatorConditionWrapper(wapper.m_source), requestSet.Mode, message); }
            if (null != requestSet.Threshold) { ConditionRecord.Definitions[requestSet.RepresentativeName].Update(new ConditionRecord.AnimatorConditionWrapper(wapper.m_source), requestSet.Threshold, message); }
            if (null != requestSet.Parameter) { ConditionRecord.Definitions[requestSet.RepresentativeName].Update(new ConditionRecord.AnimatorConditionWrapper(wapper.m_source), requestSet.Parameter, message); }
        }
        /// <summary>
        /// コンディションを削除します。
        /// コンディション番号の大きい物から順に削除してください。コンディション番号の小さい物から削除すると番号が繰り上がってしまうため。
        /// </summary>
        public static void Delete(AnimatorController ac, DataManipulatRecordSet requestSet, StringBuilder message)
        {
            AnimatorStateTransition transition = Operation_Transition.Lookup(ac, requestSet.RepresentativeRecord);// トランジション
            ConditionRecord.AnimatorConditionWrapper wapper = Operation_Condition.Lookup(ac, transition, requestSet.RepresentativeRecord);
            transition.RemoveCondition(wapper.m_source);
        }
    }

    /// <summary>
    /// ポジション操作
    /// </summary>
    public abstract class Operation_Position
    {
        public static void Update(AnimatorController ac, DataManipulationRecord request, StringBuilder message)
        {
            if ("stateMachines" == request.Foreignkeycategory)
            {
                // ステートマシンのポジション
                AnimatorStateMachine statemachine = Operation_Statemachine.Lookup(ac, request.Fullpath);
                if (null == statemachine) { throw new UnityException("[" + request.Fullpath + "]ステートマシンは見つからなかったぜ☆（＾～＾） ac=[" + ac.name + "]"); }

                if (Operation_Something.HasProperty(request.Name, TransitionRecord.Definitions, "ステートマシンのポジション操作"))
                {
                    PositionRecord.Definitions[request.Name].Update(new PositionRecord.PositionWrapper(statemachine, request.Propertyname_ofFullpath), request, message);
                }
            }
            else // ステートのポジション
            {
                ChildAnimatorState caState = Operation_ChildState.Lookup(ac, request.Fullpath); // 構造体☆

                if ("states" == request.Foreignkeycategory)
                {
                    if (Operation_Something.HasProperty(request.Name, TransitionRecord.Definitions, "ステートのポジション操作"))
                    {
                        PositionRecord.Definitions[request.Name].Update(new PositionRecord.PositionWrapper(caState, request.Propertyname_ofFullpath), request, message);
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
