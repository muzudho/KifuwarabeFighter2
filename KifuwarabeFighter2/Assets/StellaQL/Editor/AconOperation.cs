using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor.Animations;
using UnityEngine;

namespace DojinCircleGrayscale.StellaQL
{
    /// <summary>
    /// どれでも関連
    /// </summary>
    public abstract class Operation_Something
    {
        public static void ManipulateData(AnimatorControllerWrapper acWrapper, AconDocument aconData_old, HashSet<DataManipulationRecord> request_packets, StringBuilder info_message)
        {
            //{
            //    StringBuilder contents = new StringBuilder();
            //    contents.Append("Update request ");
            //    contents.Append(request_packets.Count);
            //    contents.Append(" record sets.");
            //    contents.AppendLine();
            //    foreach (DataManipulationRecord request_packet in request_packets)
            //    {
            //        request_packet.ToCsvLine(contents);
            //    }
            //    Debug.Log(contents.ToString());
            //}

            // 更新要求を、届け先別に仕分ける。コレクションを入れ子にし、フルパスで更新要求を仕分ける。

            // レイヤー
            List<DataManipulationRecord> list_layer = new List<DataManipulationRecord>();

            // 二重構造 [ステート・フルパス、トランジション番号,命令リスト] ※1つのトランジションに複数の命令が飛んでくることはある。
            Dictionary<string, Dictionary<int, List<DataManipulationRecord>>> wrap3Dic_transition = new Dictionary<string, Dictionary<int, List<DataManipulationRecord>>>();

            // 三重構造 [ステート・フルパス、トランジション番号、コンディション番号] ※１つのコンディションは１～３フィールドで１つの更新要求になる。
            Dictionary<string,Dictionary<int, Dictionary<int, Operation_Condition.DataManipulatRecordSet>>> largeDic_condition = new Dictionary<string, Dictionary<int, Dictionary<int, Operation_Condition.DataManipulatRecordSet>>>();
            foreach (DataManipulationRecord request_packet in request_packets)
            {
                switch (request_packet.Category)
                {
                    case "parameters":      Operation_Parameter     .Update(acWrapper.SourceAc, request_packet, info_message); break;
                    case "layers":
                        {
                            list_layer.Add(request_packet);
                        }
                        break;
                    case "stateMachines":   Operation_Statemachine  .Update(acWrapper.SourceAc, request_packet, info_message); break;
                    case "states":          Operation_State         .Update(acWrapper.SourceAc, request_packet, info_message); break;
                    case "transitions":
                        {
                            Dictionary<int, List<DataManipulationRecord>> wrap2Dic;

                            // 既存
                            if (wrap3Dic_transition.ContainsKey(request_packet.Fullpath)) { wrap2Dic = wrap3Dic_transition[request_packet.Fullpath]; }
                            // 新規追加
                            else { wrap2Dic = new Dictionary<int, List<DataManipulationRecord>>(); wrap3Dic_transition[request_packet.Fullpath] = wrap2Dic; }

                            // トランジション番号
                            int tNum = int.Parse(request_packet.TransitionNum_ofFullpath);
                            if (!wrap2Dic.ContainsKey(tNum))
                            {
                                // １つのトランジションに複数の命令がくることもある。
                                wrap2Dic.Add(tNum, new List<DataManipulationRecord>());
                            }
                            wrap2Dic[tNum].Add(request_packet);
                        }
                        break;
                    case "conditions":
                        {
                            // 条件を溜め込む。 mode, parameter, threshold の３つが揃って更新ができる。
                            Operation_Condition.DataManipulatRecordSet request_buffer;

                            Dictionary<int, Dictionary<int, Operation_Condition.DataManipulatRecordSet>> middleDic;
                            // 既存
                            if (largeDic_condition.ContainsKey(request_packet.Fullpath)) { middleDic = largeDic_condition[request_packet.Fullpath]; }
                            // 新規追加
                            else { middleDic = new Dictionary<int, Dictionary<int, Operation_Condition.DataManipulatRecordSet>>(); largeDic_condition[request_packet.Fullpath] = middleDic; }
                            
                            Dictionary<int, Operation_Condition.DataManipulatRecordSet> smallDic;
                            int tNum = int.Parse(request_packet.TransitionNum_ofFullpath);
                            // 既存
                            if (middleDic.ContainsKey(tNum)) { smallDic = middleDic[tNum]; }
                            // 新規追加
                            else { smallDic = new Dictionary<int, Operation_Condition.DataManipulatRecordSet>(); middleDic[tNum] = smallDic; }
                            
                            int cNum = int.Parse(request_packet.ConditionNum_ofFullpath);
                            // 既存
                            if (smallDic.ContainsKey(cNum)) { request_buffer = smallDic[cNum]; }
                            // 新規追加
                            else { request_buffer = new Operation_Condition.DataManipulatRecordSet(tNum, cNum); smallDic[cNum] = request_buffer; }

                            // 複数行に分かれていた命令を、１つのセットにまとめる
                            switch (request_packet.Name)
                            {
                                case "parameter":   request_buffer.Parameter    = request_packet; break;
                                case "mode":        request_buffer.Mode         = request_packet; break;
                                case "threshold":   request_buffer.Threshold    = request_packet; break;
                                default:            throw new UnityException("Not supported property. record.Name=[" + request_packet.Name + "]");
                            }
                        }
                        break;
                    case "positions": Operation_Position.Update(acWrapper.SourceAc, request_packet, info_message); break;
                    default: throw new UnityException("Not supported category. ["+ request_packet.Category + "]");
                }
            }

            #region Execute Transition
            {
                // 挿入、接続先変更、更新、削除　に振り分けたい。
                List<DataManipulationRecord> insertsNew_Set = new List<DataManipulationRecord>();
                List<DataManipulationRecord> insertsChangeDestination_Set = new List<DataManipulationRecord>();
                List<DataManipulationRecord> deletesSet = new List<DataManipulationRecord>();
                List<DataManipulationRecord> updatesSet = new List<DataManipulationRecord>();
                foreach (KeyValuePair<string, Dictionary<int, List<DataManipulationRecord>>> wrap3_request in wrap3Dic_transition)
                {
                    foreach (KeyValuePair<int, List<DataManipulationRecord>> wrap2_request in wrap3_request.Value)
                    {
                        // 同じトランジションに複数の命令がある
                        if (0<wrap2_request.Value.Count)
                        {
                            // 先頭要素だけ見れば十分
                            AnimatorStateTransition transition = AconFetcher.FetchTransition(acWrapper.SourceAc, wrap2_request.Value[0]);

                            foreach (DataManipulationRecord request in wrap2_request.Value)
                            {
                                // トランジションの遷移先へのデータ操作命令
                                if ("#DestinationFullpath#" == request.Name)
                                {
                                    if ("" != request.New)
                                    {
                                        if ("" == request.Old) { insertsNew_Set.Add(request); }
                                        else { insertsChangeDestination_Set.Add(request); }
                                    }
                                    else if(request.IsClear) {deletesSet.Add(request); }
                                    else { throw new UnityException("Not supported. request.Name=["+ request.Name + "] request.Old=["+ request.Old + "] request.New=["+ request.New + "]"); }
                                }
                                else
                                {
                                    if (request.IsClear) { deletesSet.Add(request); }
                                    else {
                                        if (HasProperty(request.Name, TransitionRecord.Definitions, "Transition operation"))
                                        {
                                            updatesSet.Add(request);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // 新規挿入
                foreach (DataManipulationRecord request in insertsNew_Set) { Operation_Transition.Insert_New(acWrapper.SourceAc, request, info_message); }
                // 遷移先変更
                foreach (DataManipulationRecord request in insertsChangeDestination_Set) { Operation_Transition.Insert_ChangeDestination(acWrapper.SourceAc, request, info_message); }
                deletesSet.Sort((DataManipulationRecord a, DataManipulationRecord b) =>
                {
                    // 削除要求を、連番の逆順にする
                    int stringCompareOrder = string.CompareOrdinal(a.Fullpath, b.Fullpath);

                    // ステート名の順番はそのまま。
                    if (0 != stringCompareOrder) { return stringCompareOrder; }

                    // トランジションの順番は一応後ろから
                    else if (int.Parse(a.TransitionNum_ofFullpath) < int.Parse(b.TransitionNum_ofFullpath)) { return -1; }

                    else if (int.Parse(b.TransitionNum_ofFullpath) < int.Parse(a.TransitionNum_ofFullpath)) { return 1; }
                    return 0;
                });
                // 削除を処理
                foreach (DataManipulationRecord request in deletesSet) { Operation_Transition.Delete(acWrapper.SourceAc, request, info_message); }
                // 更新
                foreach (DataManipulationRecord request in updatesSet) { Operation_Transition.Update(acWrapper.SourceAc, request, info_message); }
            }
            #endregion
            #region Execute condition.
            {
                // 挿入、更新、削除に振り分けたい。
                List<Operation_Condition.DataManipulatRecordSet> insertsSet = new List<Operation_Condition.DataManipulatRecordSet>();
                List<Operation_Condition.DataManipulatRecordSet> deletesSet = new List<Operation_Condition.DataManipulatRecordSet>();
                List<Operation_Condition.DataManipulatRecordSet> updatesSet = new List<Operation_Condition.DataManipulatRecordSet>();
                foreach (KeyValuePair<string, Dictionary<int, Dictionary<int, Operation_Condition.DataManipulatRecordSet>>> conditionRecordSetPair in largeDic_condition)
                {
                    foreach (KeyValuePair<int, Dictionary<int, Operation_Condition.DataManipulatRecordSet>> conditionRecordSet1Pair in conditionRecordSetPair.Value)
                    {
                        foreach (KeyValuePair<int, Operation_Condition.DataManipulatRecordSet> conditionRecordSet2Pair in conditionRecordSet1Pair.Value)
                        {
                            if (Operation_Something.HasProperty(conditionRecordSet2Pair.Value.RepresentativeName, ConditionRecord.Definitions, "Condition operation"))
                            {
                                // トランジション
                                AnimatorStateTransition transition = AconFetcher.FetchTransition_ByFullpath(acWrapper.SourceAc,
                                    conditionRecordSet2Pair.Value.RepresentativeFullpath,
                                    conditionRecordSet2Pair.Value.RepresentativeFullpathTransition
                                    );
                                // コンディション
                                ConditionRecord.AnimatorConditionWrapper wapper = AconFetcher.FetchCondition(
                                    acWrapper.SourceAc,
                                    transition,
                                    conditionRecordSet2Pair.Value.RepresentativeFullpathCondition
                                    );

                                // 存在しないコンディション番号だった場合、 挿入 に振り分ける。
                                if (wapper.IsNull) { insertsSet.Add(conditionRecordSet2Pair.Value); }
                                // 削除要求の場合、削除 に振り分ける。
                                else if (null != conditionRecordSet2Pair.Value.Parameter && conditionRecordSet2Pair.Value.Parameter.IsClear) { deletesSet.Add(conditionRecordSet2Pair.Value); }
                                // それ以外の場合は、更新 に振り分ける。
                                else { updatesSet.Add(conditionRecordSet2Pair.Value); }
                            }
                        }
                    }
                }

                // 挿入を処理
                foreach (Operation_Condition.DataManipulatRecordSet requestSet in insertsSet) {
                    Operation_Condition.Insert(acWrapper.SourceAc, requestSet, info_message);
                }
                deletesSet.Sort((Operation_Condition.DataManipulatRecordSet a, Operation_Condition.DataManipulatRecordSet b) =>
                {
                    // 削除要求を、連番の逆順にする
                    int stringCompareOrder = string.CompareOrdinal(a.RepresentativeFullpath, b.RepresentativeFullpath);

                    // ステート名の順番はそのまま。
                    if (0 != stringCompareOrder) { return stringCompareOrder; }

                    // トランジションの順番は一応後ろから
                    else if (a.RepresentativeFullpathTransition < b.RepresentativeFullpathTransition) { return -1; }
                    
                    else if (b.RepresentativeFullpathTransition < a.RepresentativeFullpathTransition) { return 1; }

                    // コンディションの削除は後ろから
                    else if (a.RepresentativeFullpathCondition < b.RepresentativeFullpathCondition) { return -1; }

                    else if (b.RepresentativeFullpathCondition < a.RepresentativeFullpathCondition) { return 1; }
                    return 0;
                });
                // 削除を処理
                foreach (Operation_Condition.DataManipulatRecordSet requestSet in deletesSet) {
                    Debug.Log("Condition Delete");
                    Operation_Condition.Delete(acWrapper.SourceAc, requestSet, info_message);
                }
                // 更新を処理
                foreach (Operation_Condition.DataManipulatRecordSet requestSet in updatesSet) {
                    Debug.Log("Condition Update");
                    Operation_Condition.Update(acWrapper.SourceAc, requestSet, info_message);
                }
            }
            #endregion

            // レイヤーを消化（レイヤーを反映する際に、オブジェクトの全破棄の処理が入って参照リンクが切れることから、最後にやること）
            {
                foreach (DataManipulationRecord request_packet in list_layer)
                {
                    Operation_Layer.Update(acWrapper, request_packet, info_message);
                }
            }
        }

        public static bool HasProperty(string name, Dictionary<string,RecordDefinition> definitions, string calling)
        {
            if (definitions.ContainsKey(name)) { return true; }
            else
            {
                StringBuilder sb = new StringBuilder(); int i = 0; foreach (string name2 in definitions.Keys) { sb.Append("[");sb.Append(i);sb.Append("]"); sb.AppendLine(name2); i++; }
                throw new UnityException(calling + " : Not supported property. name=[" + name + "] Supported properties : " + Environment.NewLine + sb.ToString() + " end.");
            }
        }
    }

    public abstract class Operation_Parameter
    {
        /// <summary>
        /// 更新
        /// </summary>
        public static void Update(AnimatorController ac, DataManipulationRecord request, StringBuilder message)
        {
            if (Operation_Something.HasProperty(request.Name, ParameterRecord.Definitions, "Parameter operation."))
            {
                AnimatorControllerParameter parameter = AconFetcher.FetchParameter(ac, request.Fullpath);
                ParameterRecord.Definitions[request.Name].Update(parameter, request, message);
            }
        }
    }

    public abstract class Operation_Layer
    {
        #region Find
        /// <summary>
        /// レイヤー名（正規表現ではない）を指定すると レイヤー配列のインデックスを返す。
        /// 
        /// FIXME: レイヤー名にドット(.)が含まれていると StellaQL は様々なところで正常に動作しないかもしれない。
        /// </summary>
        /// <param name="justLayerName">ex "Base Layer"</param>
        public static int IndexOf_ByJustLayerName(AnimatorController ac, string justLayerName)
        {
            for (int lNum=0; lNum< ac.layers.Length; lNum++)
            {
                AnimatorControllerLayer layer = ac.layers[lNum];
                if (justLayerName == layer.name) { return lNum; }
            }

            {
                StringBuilder sb = new StringBuilder(); foreach (AnimatorControllerLayer layer in ac.layers) { sb.Append(layer.name); sb.Append(" "); }
                throw new UnityException("Not found layer. justLayerName=[" + justLayerName + "] sb:"+sb.ToString());
            }
        }
        #endregion


        public static void DumpLog(AnimatorControllerWrapper acWrapper)
        {
            {
                int lNum = 0;
                foreach (AnimatorControllerLayer layer in acWrapper.SourceAc.layers)
                {
                    Debug.Log("Dump: original layer: lNum=[" + lNum + "] blendingMode=[" + layer.blendingMode + "] defaultWeight=[" + layer.defaultWeight + "] iKPass=[" + layer.iKPass + "] name=[" + layer.name + "] syncedLayerAffectsTiming=[" + layer.syncedLayerAffectsTiming + "] syncedLayerIndex=[" + layer.syncedLayerIndex + "]");
                    lNum++;
                }
            }
            {
                int lNum = 0;
                foreach (AnimatorControllerLayer layer in acWrapper.CopiedLayers)
                {
                    Debug.Log("Dump: copy layer: lNum=[" + lNum + "] blendingMode=[" + layer.blendingMode + "] defaultWeight=[" + layer.defaultWeight + "] iKPass=[" + layer.iKPass + "] name=[" + layer.name + "] syncedLayerAffectsTiming=[" + layer.syncedLayerAffectsTiming + "] syncedLayerIndex=[" + layer.syncedLayerIndex + "]");
                    lNum++;
                }
            }
        }

        /// <summary>
        /// 元からある全てのレイヤーを削除します。ただし、レイヤーの数を１個未満にすることはできないため、最後にダミーを１個残します。
        /// 
        /// シンク（参照）関係があるため、子関係のレイヤーから消していきます。
        /// </summary>
        /// <param name="acWrapper"></param>
        public static void DeleteAllLayers_AndPutDammy(AnimatorControllerWrapper acWrapper)
        {
            // ダミーを末尾に追加。
            acWrapper.SourceAc.AddLayer(new AnimatorControllerLayer());
            const int dammy = 1;

            // ダミー以外、全部消す
            while (dammy < acWrapper.SourceAc.layers.Length)
            {
                // どこかから参照されていたら真。
                bool[] parentFlags = new bool[acWrapper.SourceAc.layers.Length];
                foreach (AnimatorControllerLayer layer in acWrapper.SourceAc.layers)
                {
                    if (-1<layer.syncedLayerIndex)
                    {
                        parentFlags[layer.syncedLayerIndex] = true;
                    }
                }

                // うしろから、どこからも参照されていないレイヤーを削除する。

                // 末尾のダミーを除く
                for (int lNum = acWrapper.SourceAc.layers.Length - 1 - dammy; -1 < lNum; lNum--)
                {
                    if (parentFlags[lNum])
                    {
                        acWrapper.SourceAc.RemoveLayer(lNum);
                    }
                }
            }
        }

        ///// <summary>
        ///// 全レイヤーを一旦退避して アニメーション・コントローラーから全部削除し、再び全レイヤーを再追加します。
        /////
        ///// レイヤー・インデックスでリンクを貼ってきている部分に考慮しています。順番の変更が起こらないように全削除、全再追加します。
        ///// 
        ///// 全部のレイヤーを削除することはできない。（最低１つのレイヤーを残しておく必要がある）
        ///// </summary>
        ///// <param name="i"></param>
        //public static void RefreshAllLayers(AnimatorControllerWrapper acWrapper)
        //{
        //    //DumpLog(acWrapper);

        //    foreach (AnimatorControllerLayer copiedLayer in acWrapper.CopiedLayers)
        //    {
        //        // 全再追加！ このときプロパティーの設定がUnityに反映されるはず。
        //        acWrapper.SourceAc.AddLayer(copiedLayer);
        //    }
        //}

        public static void Update(AnimatorControllerWrapper acWrapper, DataManipulationRecord request, StringBuilder message)
        {
            int caret = 0;
            FullpathTokens ft = new FullpathTokens();
            FullpathSyntaxP.Fixed_LayerName(request.Fullpath, ref caret, ref ft);
            AnimatorControllerLayer layer = AconFetcher.FetchLayer_JustLayerName(acWrapper.SourceAc, ft.LayerNameEndsWithoutDot);
            int layerIndex = IndexOf_ByJustLayerName(acWrapper.SourceAc, request.Fullpath);
            if (layerIndex<0) { throw new UnityException("Not found layer. [" + request.Fullpath + "] ac=[" + acWrapper.SourceAc.name + "]"); }

            if (Operation_Something.HasProperty(request.Name, LayerRecord.Definitions, "Layer operation."))
            {
                LayerRecord.Definitions[request.Name].Update(new LayerRecord.LayerWrapper(acWrapper, layerIndex), request, message);
            }
        }

        /// <summary>
        /// アニメーター・コントローラーに、レイヤーを追加する。
        /// </summary>
        public static void AddAll(AnimatorController ac, List<string> layernameWords, StringBuilder message)
        {
            foreach (string layerName in layernameWords)
            {
                ac.AddLayer(layerName);
            }
        }

        /// <summary>
        /// アニメーター・コントローラーから、レイヤーを削除する。
        /// </summary>
        public static void RemoveAll(AnimatorController ac, List<string> layernameWords, StringBuilder message)
        {
            // 順序が繰り下がらないように、後ろから削除していく
            for (int lNum = ac.layers.Length - 1; -1 < lNum; lNum--)
            {
                AnimatorControllerLayer layer = ac.layers[lNum];
                foreach (string stateName in layernameWords)
                {
                    Regex regex = new Regex(stateName);
                    if (regex.IsMatch(layer.name))
                    {
                        ac.RemoveLayer(lNum);
                    }
                }
            }
        }
    }

    /// <summary>
    /// ステートマシン関連
    /// </summary>
    public abstract class Operation_Statemachine
    {
        public static void Update(AnimatorController ac, DataManipulationRecord request, StringBuilder message)
        {
            int caret = 0;
            FullpathTokens ft = new FullpathTokens();
            if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames(request.Fullpath, ref caret, ref ft)) { throw new UnityException("Parse failure. [" + request.Fullpath + "] ac=[" + ac.name + "]"); }

            AnimatorControllerLayer layer = AconFetcher.FetchLayer_JustLayerName(ac, ft.LayerNameEndsWithoutDot);

            AnimatorStateMachine statemachine = AconFetcher.FetchStatemachine(ac, ft.StatemachineNamesEndsWithoutDot, layer);
            if (null == statemachine) { throw new UnityException("Not found statemachine. [" + request.Fullpath + "] ac=[" + ac.name + "]"); }

            if (Operation_Something.HasProperty(request.Name, StatemachineRecord.Definitions, "statemachine operation."))
            {
                StatemachineRecord.Definitions[request.Name].Update(new StatemachineRecord.Wrapper(statemachine,
                    // 例えばフルパスが "Base Layer.Alpaca.Bear.Cat.Dog" のとき、"Alpaca.Bear.Cat"。
                    ft.StatemachinePath
                    ), request, message);
            }
        }
    }

    /// <summary>
    /// ステートマシン・エニーステート関連
    /// </summary>
    public abstract class Operation_StatemachineAnystate
    {
        /// <summary>
        /// STATEMACHINE ANYSTATE INSERT
        /// 
        /// ２つのステートを トランジションで結ぶ。ステートは複数指定でき、src→dst方向の総当たりで全部結ぶ。
        /// </summary>
        /// <param name="path_src">"Base Layer.JMove.JMove0" といった文字列</param>
        public static void AddAll(AnimatorController ac, HashSet<AnimatorStateMachine> statemachines_dst, HashSet<AnimatorState> states_dst, StringBuilder message)
        {
            foreach (AnimatorStateMachine statemachine_src in statemachines_dst)
            {
                foreach (AnimatorState state_dst in states_dst)
                {
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
        public static void Update(AnimatorController ac, DataManipulationRecord request, StringBuilder message)
        {
            int caret = 0;
            FullpathTokens ft = new FullpathTokens();
            if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(request.Fullpath, ref caret, ref ft)) { throw new UnityException("Parse failure. [" + request.Fullpath + "] ac=[" + ac.name + "]"); }

            AnimatorState state = AconFetcher.FetchState(ac, ft);
            if (null == state) { throw new UnityException("Not found state. [" + request.Fullpath + "] ac=[" + ac.name + "]"); }

            if (Operation_Something.HasProperty(request.Name, StateRecord.Definitions, "State operation."))
            {
                StateRecord.Definitions[request.Name].Update(new StateRecord.Wrapper(state), request, message);
            }
        }

        /// <summary>
        /// ステートマシンに、ステートを追加する。
        /// </summary>
        public static void AddAll(AnimatorController ac, HashSet<AnimatorStateMachine> statemachines, List<string> statenameWords, StringBuilder message)
        {
            foreach (AnimatorStateMachine statemachine in statemachines)
            {
                foreach (string stateName in statenameWords)
                {
                    statemachine.AddState(stateName);
                }
            }
        }

        /// <summary>
        /// ステートマシンから、ステートを削除する。
        /// </summary>
        public static void RemoveAll(AnimatorController ac, HashSet<AnimatorStateMachine> statemachines, List<string> statenameWords, StringBuilder message)
        {
            foreach (AnimatorStateMachine statemachine in statemachines)
            {
                foreach (string stateName in statenameWords)
                {
                    Regex regex = new Regex(stateName);

                    // 順序が繰り下がらないように、後ろから削除していく
                    for (int caNum = statemachine.states.Length - 1; -1 < caNum; caNum--)
                    {
                        ChildAnimatorState caState = statemachine.states[caNum];

                        if (regex.IsMatch(caState.state.name))
                        {
                            statemachine.RemoveState(caState.state);
                        }
                    }
                }
            }
        }

        public static void UpdateProperty(AnimatorController ac, Dictionary<string,string> properties, HashSet<AnimatorState> states, StringBuilder message)
        {
            // 指定されたステート全て対象
            foreach (AnimatorState state in states)
            {
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
                        default: throw new UnityException("Not supported property. ["+ pair.Key+ "]=[" + pair.Value + "]");
                    }
                }
            }
        }

        public static void Select(AnimatorController ac, HashSet<AnimatorState> states, out HashSet<StateRecord> recordSet, StringBuilder message)
        {
            recordSet = new HashSet<StateRecord>();
            // 指定されたステート全て対象
            foreach (AnimatorState state in states)
            {
                if (null == state) { throw new UnityException("state is null. states.Count=[" + states.Count + "]"); }
                recordSet.Add(new StateRecord(0,0,0, state));
            }
            message.Append("result: "); message.Append(recordSet.Count); message.AppendLine(" records.");
        }
    }

    /// <summary>
    /// トランジション関連
    /// </summary>
    public abstract class Operation_Transition
    {
        /// <summary>
        /// 新規挿入
        /// </summary>
        public static void Insert_New(AnimatorController ac, DataManipulationRecord request, StringBuilder message)
        {
            // 遷移元のステート
            AnimatorState srcState;
            {
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(request.Fullpath, ref caret, ref ft)) { throw new UnityException("Parse failure. [" + request.Fullpath + "] ac=[" + ac.name + "]"); }
                srcState = AconFetcher.FetchState(ac, ft);
            }

            // 遷移先のステート
            AnimatorState dstState;
            {
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(request.New, ref caret, ref ft)) { throw new UnityException("Parse failure. [" + request.New + "] ac=[" + ac.name + "]"); }
                dstState = AconFetcher.FetchState(ac, ft);
            }
            srcState.AddTransition(dstState);
        }
        /// <summary>
        /// 遷移先変更
        /// </summary>
        public static void Insert_ChangeDestination(AnimatorController ac, DataManipulationRecord request, StringBuilder message)
        {
            // 遷移元のステート
            AnimatorState srcState;
            {
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(request.Fullpath, ref caret, ref ft)) { throw new UnityException("Parse failure. [" + request.Fullpath + "] ac=[" + ac.name + "]"); }
                srcState = AconFetcher.FetchState(ac, ft);
            }

            // 古い遷移先のステート
            AnimatorState dstOldState;
            {
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(request.Old, ref caret, ref ft)) { throw new UnityException("Parse failure. [" + request.Old + "] ac=[" + ac.name + "]"); }
                dstOldState = AconFetcher.FetchState(ac, ft);
            }

            // 新しい遷移先のステート
            AnimatorState dstNewState;
            {
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(request.New, ref caret, ref ft)) { throw new UnityException("Parse failure. [" + request.New + "] ac=[" + ac.name + "]"); }
                dstNewState = AconFetcher.FetchState(ac, ft);
            }

            List<AnimatorStateTransition> removeList = new List<AnimatorStateTransition>();
            // FIXME: ほんとは トランジション番号も指定されているのでは？

            // 削除するトランジションを全て取得
            List<AnimatorStateTransition> oldTransitions = AconFetcher.FetchTransition_SrcDst(ac, request.Fullpath, request.Old);
            foreach (AnimatorStateTransition oldTransition in oldTransitions)
            {
                srcState.AddTransition(dstNewState);
                // 遷移先ステート以外の中身を写しておく。
                AconShallowcopy.ShallowcopyTransition_ExceptDestinaionState(srcState.transitions[srcState.transitions.Length - 1], oldTransition);
                removeList.Add(oldTransition);
            }

            foreach (AnimatorStateTransition removeeTransition in removeList)
            {
                srcState.RemoveTransition(removeeTransition);
            }
        }
        public static void Update(AnimatorController ac, DataManipulationRecord request, StringBuilder message)
        {
            // 遷移元ステート
            AnimatorState srcState;
            {
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(request.Fullpath, ref caret, ref ft)) { throw new UnityException("Parse failure. [" + request.Fullpath + "] ac=[" + ac.name + "]"); }
                srcState = AconFetcher.FetchState(ac, ft);
            }
            if (null == srcState) { throw new UnityException("Not found state. [" + request.Fullpath + "] ac=[" + ac.name + "]"); }

            // トランジション番号
            int transitionNum = int.Parse(request.TransitionNum_ofFullpath);

            int tNum = 0;
            foreach( AnimatorStateTransition transition in srcState.transitions)
            {
                if (transitionNum==tNum)
                {
                    TransitionRecord.Definitions[request.Name].Update(transition, request, message);
                    break;
                }
                tNum++;
            }
        }
        /// <summary>
        /// トランジションを削除します。
        /// 
        /// トランジション番号の大きい物から順に削除してください。トランジション番号の小さい物から削除すると番号が繰り上がってしまうため。
        /// </summary>
        public static void Delete(AnimatorController ac, DataManipulationRecord request, StringBuilder message)
        {
            AnimatorState srcState;
            {
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(request.Fullpath, ref caret, ref ft)) { throw new UnityException("Parse failure. [" + request.Fullpath + "] ac=[" + ac.name + "]"); }
                srcState = AconFetcher.FetchState(ac, ft);
            }

            // 削除するトランジション
            AnimatorStateTransition transition = AconFetcher.FetchTransition(ac, request);
            srcState.RemoveTransition(transition);
        }

        /// <summary>
        /// ステートマシンの[Any State]からステートへ、トランジションで結ぶ。
        /// </summary>
        public static bool AddAnyState(AnimatorController ac, HashSet<AnimatorStateMachine> statemachines_src, HashSet<AnimatorState> states_dst, StringBuilder info_message)
        {
            //info_message.Append("Transition.AddAnyState: Source "); info_message.Append(statemachines_src.Count); info_message.Append(" states. Destination "); info_message.Append(states_dst.Count); info_message.AppendLine(" states.");
            foreach (AnimatorStateMachine statemachine_src in statemachines_src)
            {
                foreach (AnimatorState state_dst in states_dst)
                {
                    if (null == statemachine_src) { return false; }
                    statemachine_src.AddAnyStateTransition(state_dst);
                }
            }
            return true;
        }

        /// <summary>
        /// ステートマシンの[Entry]からステートへ、トランジションで結ぶ。
        /// </summary>
        public static bool AddEntryState(AnimatorController ac, HashSet<AnimatorStateMachine> statemachines_src, HashSet<AnimatorState> states_dst, StringBuilder info_message)
        {
            //info_message.Append("Transition.AddEntryState: Source "); info_message.Append(statemachines_src.Count); info_message.Append(" states. Destination "); info_message.Append(states_dst.Count); info_message.AppendLine(" states.");
            foreach (AnimatorStateMachine statemachine_src in statemachines_src)
            {
                foreach (AnimatorState state_dst in states_dst)
                {
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
            //message.Append("Transition.AddExitState: "); message.Append(states.Count); message.AppendLine(" states.");
            foreach (AnimatorState state_src in states)
            {
                if (null == state_src) { return false; }
                state_src.AddExitTransition();
            }
            return true;
        }

        /// <summary>
        /// ２つのステートを トランジションで結ぶ。ステートは複数指定でき、src→dst方向の総当たりで全部結ぶ。
        /// </summary>
        /// <param name="path_src">"Base Layer.JMove.JMove0" といった文字列</param>
        public static void Insert(AnimatorController ac, HashSet<AnimatorState> states_src, HashSet<AnimatorState> states_dst, Dictionary<string, string> properties, StringBuilder info_message)
        {
            // info_message.Append("Transition.Insert: Source "); info_message.Append(states_src.Count); info_message.Append(" states. Destination "); info_message.Append(states_dst.Count); info_message.AppendLine(" states.");
            foreach (AnimatorState state_src in states_src) {
                foreach (AnimatorState state_dst in states_dst) {
                    AnimatorStateTransition transition = state_src.AddTransition(state_dst);
                    UpdateProperty(ac, transition, properties);
                }
            }
        }

        /// <summary>
        /// ２つのステート間の トランジションを削除する。ステートは複数指定でき、src→dst方向の総当たりで全部削除する。
        /// </summary>
        /// <param name="path_src">ex. "Base Layer.JMove.JMove0"</param>
        public static void RemoveAll(AnimatorController ac, HashSet<AnimatorState> states_src, HashSet<AnimatorState> states_dst, StringBuilder info_message)
        {
            info_message.Append("Transition.RemoveAll: Source "); info_message.Append(states_src.Count); info_message.Append(" states. Destination "); info_message.Append(states_dst.Count); info_message.AppendLine(" states.");

            foreach (AnimatorState state_src in states_src)
            {
                foreach (AnimatorState state_dst in states_dst)
                {
                    foreach (AnimatorStateTransition transition in state_src.transitions)
                    {
                        if (state_dst == transition.destinationState)
                        {
                            state_src.RemoveTransition(transition);
                            //info_message.Append("deleted: ");
                            //info_message.Append(state_src.name);
                            //info_message.Append(" -> ");
                            //info_message.Append(state_dst.name);
                            //info_message.AppendLine();
                        }
                    }
                }
            }
        }

        public static void Update(AnimatorController ac, HashSet<AnimatorState> states_src, HashSet<AnimatorState> states_dst, Dictionary<string, string> properties, StringBuilder info_message)
        {
            // 指定されたステート全て対象
            foreach (AnimatorState state_src in states_src)
            {
                foreach (AnimatorStateTransition transition in state_src.transitions)
                {
                    // 指定されたステート全て対象
                    foreach (AnimatorState state_dst in states_dst)
                    {
                        if (state_dst == transition.destinationState)
                        {
                            UpdateProperty(ac, transition, properties);
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
                    default: throw new UnityException("Not supported property. [" + pair.Key + "]=[" + pair.Value + "]");
                }
            }
        }

        public static void Select(AnimatorController ac, HashSet<AnimatorState> states_src, HashSet<AnimatorState> states_dst, out HashSet<TransitionRecord> hitRecords, StringBuilder message)
        {
            hitRecords = new HashSet<TransitionRecord>();
            // SELECT文で出力するシート用の人間に読みやすいように補足する説明
            StringBuilder stellaQLComment = new StringBuilder();
            // 指定されたステート全て対象
            foreach (AnimatorState state_src in states_src)
            {
                foreach (AnimatorStateTransition transition in state_src.transitions)
                {
                    // 指定されたステート全て対象
                    foreach (AnimatorState state_dst in states_dst)
                    {
                        if (state_dst == transition.destinationState)
                        {
                            // CSVにするので改行は入れない。
                            stellaQLComment.Append("Select: "); stellaQLComment.Append(state_src.name); stellaQLComment.Append(" -> "); stellaQLComment.Append(state_dst.name);
                            hitRecords.Add(new TransitionRecord(0,0,0,0,transition, stellaQLComment.ToString()));
                            stellaQLComment.Length = 0;
                        }
                    }
                }
            }
            message.Append("Select: result "); message.Append(hitRecords.Count); message.AppendLine(" transitions.");;
        }
    }

    /// <summary>
    /// コンディション関連
    /// </summary>
    public abstract class Operation_Condition
    {
        public class DataManipulatRecordSet
        {
            public DataManipulatRecordSet(int fullpathTransition_forDebug, int fullpathCondition_forDebug)
            {
                this.FullpathTransition_forDebug = fullpathTransition_forDebug;
                this.FullpathCondition_forDebug = fullpathCondition_forDebug;
            }

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
            DataManipulationRecord RepresentativeRecord {
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
                sb.AppendLine("Error : " + calling + " ");
                sb.AppendLine("fullpathTransition=[" + this.FullpathTransition_forDebug + "]");
                sb.AppendLine("fullpathCondition=[" + this.FullpathCondition_forDebug + "]");
                if (null == Mode) { sb.AppendLine("Mode is null."); }
                if (null == Threshold) { sb.AppendLine("Threshold is null."); }
                if (null == Parameter) { sb.AppendLine("Parameter is null."); }
                return sb.ToString();
            }
        }

        public static void Insert(AnimatorController ac, DataManipulatRecordSet requestSet, StringBuilder message)
        {
            // １つ上のオブジェクト（トランジション）
            AnimatorStateTransition transition = AconFetcher.FetchTransition_ByFullpath(ac,
                requestSet.RepresentativeFullpath,
                requestSet.RepresentativeFullpathTransition
                );
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
            AnimatorStateTransition transition = AconFetcher.FetchTransition_ByFullpath(ac,
                requestSet.RepresentativeFullpath,
                requestSet.RepresentativeFullpathTransition
                );

            // struct をラッピングして返す
            ConditionRecord.AnimatorConditionWrapper wapper = AconFetcher.FetchCondition(ac, transition, requestSet.RepresentativeFullpathCondition);
            if (null != requestSet.Mode) {ConditionRecord.Definitions[requestSet.Mode.Name].Update(wapper, requestSet.Mode, message);}
            if (null != requestSet.Threshold) {ConditionRecord.Definitions[requestSet.Threshold.Name].Update(wapper, requestSet.Threshold, message);}
            if (null != requestSet.Parameter) {ConditionRecord.Definitions[requestSet.Parameter.Name].Update(wapper, requestSet.Parameter, message);}
        }
        /// <summary>
        /// コンディションを削除します。
        /// 
        /// コンディション番号の大きい物から順に削除してください。コンディション番号の小さい物から削除すると番号が繰り上がってしまうため。
        /// </summary>
        public static void Delete(AnimatorController ac, DataManipulatRecordSet requestSet, StringBuilder message)
        {
            AnimatorStateTransition transition = AconFetcher.FetchTransition_ByFullpath(ac,
                requestSet.RepresentativeFullpath,
                requestSet.RepresentativeFullpathTransition
                );
            ConditionRecord.AnimatorConditionWrapper wapper = AconFetcher.FetchCondition(ac, transition, requestSet.RepresentativeFullpathCondition);
            transition.RemoveCondition(wapper.m_source);
        }

        #region Property update.
        public static void UpdateProperty_AndRebuild(AnimatorStateTransition transition, int conditionNum_target, string propertyName, object newValue)
        {
            // 全てのコンディションのコピーを新規作成する。
            List<AconConditionBuilder> cs_new = new List<AconConditionBuilder>();
            foreach (AnimatorCondition c_old in transition.conditions)
            {
                cs_new.Add(new AconConditionBuilder(c_old));
            }

            // 既存のコンディションを全て消す。
            for (int cNum = transition.conditions.Length - 1; 0 < transition.conditions.Length; cNum--)
            {
                transition.RemoveCondition(transition.conditions[cNum]);
            }

            // 新規作成したコンディションに更新を掛ける
            for (int cNum_new = 0; cNum_new < cs_new.Count; cNum_new++)
            {
                if (conditionNum_target == cNum_new)
                {
                    AconConditionBuilder c_new = cs_new[cNum_new];
                    switch (propertyName)
                    {
                        case "mode": c_new.mode = (AnimatorConditionMode)newValue; break;
                        case "threshold": c_new.threshold = (float)newValue; break;
                        case "parameter": c_new.parameter = (string)newValue; break;
                        default: throw new UnityException("Not supported property. propertyName[" + propertyName + "] conditionNum_target=["+ conditionNum_target + "]");
                    }
                }
            }
            // セッターは機能していないのでは？
            //condition_w.mode = AnimatorConditionMode.Less;

            // 新規作成したコンディションを追加し直す
            for (int cNum_w = 0; cNum_w < cs_new.Count; cNum_w++)
            {
                AconConditionBuilder c_new = cs_new[cNum_w];
                transition.AddCondition(c_new.mode, c_new.threshold, c_new.parameter);
            }
        }
        #endregion
    }

    /// <summary>
    /// ポジション操作
    /// </summary>
    public abstract class Operation_Position
    {

        public static void Update(AnimatorController ac, DataManipulationRecord request, StringBuilder message)
        {
            if (Operation_Something.HasProperty(request.Name, PositionRecord.Definitions, "Position operation"))
            {
                if ("stateMachines" == request.Foreignkeycategory)
                {
                    // ステートマシンのポジション
                    int caret = 0;
                    FullpathTokens ft = new FullpathTokens();
                    if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames(request.Fullpath, ref caret, ref ft)) { throw new UnityException("Parse failure. [" + request.Fullpath + "] ac=[" + ac.name + "]"); }

                    PositionRecord.Definitions[request.Name].Update(AconFetcher.FetchPosition(ac, ft.LayerNameEndsWithoutDot, ft.StatemachineNamesEndsWithoutDot, request.Propertyname_ofFullpath), request, message);
                }
                else
                {
                    if ("states" == request.Foreignkeycategory)
                    {
                        // ステートのポジション
                        PositionRecord.Definitions[request.Name].Update(AconFetcher.FetchPosition_OfState(ac, request.Fullpath, request.Propertyname_ofFullpath), request, message);
                    }
                    else
                    {
                        // ステートマシン、ステート以外のポジション
                        throw new UnityException("Positions other than state machines and states are not supported. fullpath=[" + request.Fullpath + "] ac=[" + ac.name + "]");
                    }
                }
            }
        }
    }

    public abstract class Operation_AnimatorLayerBlendingMode
    {
        /// <summary>
        /// 参照 : enumration : 「列挙体のメンバの値や名前を列挙する」 http://dobon.net/vb/dotnet/programing/enumgetvalues.html
        /// </summary>
        public static HashSet<AnimatorLayerBlendingMode> FetchAnimatorLayerBlendingModes(string blendingModeName_regex)
        {
            HashSet<AnimatorLayerBlendingMode> hits = new HashSet<AnimatorLayerBlendingMode>();
            foreach (string enumItemName in Enum.GetNames(typeof(AnimatorLayerBlendingMode)))
            {
                Regex regex = new Regex(blendingModeName_regex);
                if (regex.IsMatch(enumItemName))
                {
                    hits.Add((AnimatorLayerBlendingMode)Enum.Parse(typeof(AnimatorLayerBlendingMode),enumItemName));
                }
            }
            return hits;
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
