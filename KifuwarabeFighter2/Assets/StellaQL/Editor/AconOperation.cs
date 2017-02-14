//
// Animation Controller Operation
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor.Animations;
using UnityEngine;

namespace StellaQL
{
    /// <summary>
    /// どれでも関連
    /// </summary>
    public abstract class Operation_Something
    {
        public static void ManipulateData(AnimatorControllerWrapper acWrapper, AconData aconData_old, HashSet<DataManipulationRecord> request_packets, StringBuilder info_message)
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
            // レイヤー
            List<DataManipulationRecord> list_layer = new List<DataManipulationRecord>();
            // 二重構造 [ステート・フルパス、トランジション番号]
            Dictionary<string, Dictionary<int, DataManipulationRecord>> largeDic_transition = new Dictionary<string, Dictionary<int, DataManipulationRecord>>();
            // 三重構造 [ステート・フルパス、トランジション番号、コンディション番号]
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
                    case "positinos": Operation_Position.Update(acWrapper.SourceAc, request_packet, info_message); break;
                    default: throw new UnityException("未対応のカテゴリー=["+ request_packet.Category + "]");
                }
            }

            // トランジションを消化
            {
                // 挿入、更新、削除に振り分けたい。
                List<DataManipulationRecord> insertsSet = new List<DataManipulationRecord>();
                List<DataManipulationRecord> deletesSet = new List<DataManipulationRecord>();
                List<DataManipulationRecord> updatesSet = new List<DataManipulationRecord>();
                //Debug.Log("largeDic_transition.Count=" + largeDic_transition.Count);// [ステート・フルパス,トランジション番号]
                foreach (KeyValuePair<string, Dictionary<int, DataManipulationRecord>> request_2wrap in largeDic_transition)
                {
                    Debug.Log("request_2wrap.Value.Count=" + request_2wrap.Value.Count);// [,トランジション番号]
                    foreach (KeyValuePair<int, DataManipulationRecord> request_1wrap in request_2wrap.Value)
                    {
                        AnimatorStateTransition transition = Operation_Transition.Fetch(acWrapper.SourceAc, request_1wrap.Value);// トランジション

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

                foreach (DataManipulationRecord request in insertsSet) { Operation_Transition.Insert(acWrapper.SourceAc, request, info_message); }// 更新を処理
                deletesSet.Sort((DataManipulationRecord a, DataManipulationRecord b) =>
                { // 削除要求を、連番の逆順にする
                    int stringCompareOrder = string.CompareOrdinal(a.Fullpath, b.Fullpath);
                    if (0 != stringCompareOrder) { return stringCompareOrder; } // ステート名の順番はそのまま。
                    else if (int.Parse(a.TransitionNum_ofFullpath) < int.Parse(b.TransitionNum_ofFullpath)) { return -1; } // トランジションの順番は一応後ろから
                    else if (int.Parse(b.TransitionNum_ofFullpath) < int.Parse(a.TransitionNum_ofFullpath)) { return 1; }
                    return 0;
                });
                foreach (DataManipulationRecord request in deletesSet) { Operation_Transition.Delete(acWrapper.SourceAc, request, info_message); }// 削除を処理
                foreach (DataManipulationRecord request in updatesSet) { Operation_Transition.Update(acWrapper.SourceAc, request, info_message); }// 挿入を処理
            }
            // コンディションを消化
            {
                // 挿入、更新、削除に振り分けたい。
                List<Operation_Condition.DataManipulatRecordSet> insertsSet = new List<Operation_Condition.DataManipulatRecordSet>();
                List<Operation_Condition.DataManipulatRecordSet> deletesSet = new List<Operation_Condition.DataManipulatRecordSet>();
                List<Operation_Condition.DataManipulatRecordSet> updatesSet = new List<Operation_Condition.DataManipulatRecordSet>();
                //Debug.Log("conditionRecordSet.Count=" + largeDic_condition.Count);// [ステート・フルパス,トランジション番号,コンディション番号]
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
                                AnimatorStateTransition transition = Operation_Transition.Fetch(acWrapper.SourceAc, conditionRecordSet2Pair.Value.RepresentativeRecord);// トランジション
                                ConditionRecord.AnimatorConditionWrapper wapper = Operation_Condition.Fetch(acWrapper.SourceAc, transition, conditionRecordSet2Pair.Value.RepresentativeRecord);// コンディション

                                if (wapper.IsNull) { insertsSet.Add(conditionRecordSet2Pair.Value); }// 存在しないコンディション番号だった場合、 挿入 に振り分ける。
                                else if (null != conditionRecordSet2Pair.Value.Parameter && conditionRecordSet2Pair.Value.Parameter.IsDelete) { deletesSet.Add(conditionRecordSet2Pair.Value); }// 削除要求の場合、削除 に振り分ける。
                                else { updatesSet.Add(conditionRecordSet2Pair.Value); }// それ以外の場合は、更新 に振り分ける。
                            }
                        }
                    }
                }

                foreach (Operation_Condition.DataManipulatRecordSet requestSet in insertsSet) { Operation_Condition.Insert(acWrapper.SourceAc, requestSet, info_message); }// 更新を処理
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
                foreach (Operation_Condition.DataManipulatRecordSet requestSet in deletesSet) { Operation_Condition.Delete(acWrapper.SourceAc, requestSet, info_message); }// 削除を処理
                foreach (Operation_Condition.DataManipulatRecordSet requestSet in updatesSet) { Operation_Condition.Update(acWrapper.SourceAc, requestSet, info_message); }// 挿入を処理
            }
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
                throw new UnityException(calling + " : 更新できないプロパティ名が指定されたぜ☆（＾～＾） name=[" + name + "] 対応しているのは次の名前だぜ☆ : " + Environment.NewLine + sb.ToString() + " ここまで");
            }
        }
    }

    public abstract class Operation_Parameter
    {
        #region 取得
        /// <summary>
        /// パスを指定すると パラメーターを返す。
        /// </summary>
        /// <param name="parameterName">"Average body length" といった文字列。</param>
        public static AnimatorControllerParameter Fetch(AnimatorController ac, string parameterName)
        {
            foreach (AnimatorControllerParameter parameter in ac.parameters) { if (parameterName == parameter.name) { return parameter; } }
            throw new UnityException("レイヤーが見つからないぜ☆（＾～＾）parameterName=[" + parameterName + "]");
            //return null;
        }
        #endregion

        /// <summary>
        /// UPDATE 要求
        /// </summary>
        public static void Update(AnimatorController ac, DataManipulationRecord request, StringBuilder message)
        {
            if (Operation_Something.HasProperty(request.Name, ParameterRecord.Definitions, "パラメーター操作"))
            {
                AnimatorControllerParameter parameter = Fetch(ac, request.Fullpath);

                // 各プロパティーに UPDATE要求を 振り分ける
                ParameterRecord.Definitions[request.Name].Update(parameter, request, message);
                // throw new UnityException("[" + request.Fullpath + "]パラメーターには未対応だぜ☆（＾～＾） ac=[" + ac.name + "]");
            }
        }
    }

    public abstract class Operation_Layer
    {
        #region 取得
        /// <summary>
        /// パスを指定すると レイヤーを返す。
        /// </summary>
        /// <param name="justLayerName_EndsWithoutDot">"Base Layer" といった文字列。</param>
        public static AnimatorControllerLayer Fetch_JustLayerName(AnimatorController ac, string justLayerName_EndsWithoutDot)
        {
            // 最初の名前のノード[0]は、レイヤーを検索する。
            foreach (AnimatorControllerLayer layer in ac.layers) { if (justLayerName_EndsWithoutDot == layer.name) { return layer; } }
            throw new UnityException("レイヤーが見つからないぜ☆（＾～＾）justLayerName_EndsWithoutDot=[" + justLayerName_EndsWithoutDot + "]");
            //return null;
        }
        #endregion

        #region 検索
        /// <summary>
        /// レイヤー名（正規表現ではない）を指定すると レイヤー配列のインデックスを返す。
        /// - レイヤー名にドット(.)が含まれていると StellaQL は様々なところで正常に動作しないかもしれない。
        /// </summary>
        /// <param name="path">"Base Layer" といった文字列。</param>
        public static int IndexOf_ByJustLayerName(AnimatorController ac, string justLayerName)
        {
            for (int lNum=0; lNum< ac.layers.Length; lNum++)
            {
                AnimatorControllerLayer layer = ac.layers[lNum];
                if (justLayerName == layer.name) { return lNum; }
            }

            {
                StringBuilder sb = new StringBuilder(); foreach (AnimatorControllerLayer layer in ac.layers) { sb.Append(layer.name); sb.Append(" "); }
                throw new UnityException("レイヤーが見つからないぜ☆（＾～＾）justLayerName=[" + justLayerName + "] sb:"+sb.ToString());
            }
        }
        #endregion

        public static AnimatorControllerLayer DeepCopy(AnimatorControllerLayer old)
        {
            AnimatorControllerLayer relive = new AnimatorControllerLayer();
            relive.avatarMask = old.avatarMask;
            relive.blendingMode = old.blendingMode;
            relive.defaultWeight = old.defaultWeight;
            relive.iKPass = old.iKPass;
            relive.name = old.name;
            relive.stateMachine = Operation_Statemachine.DeepCopy(old.stateMachine);
            relive.syncedLayerAffectsTiming = old.syncedLayerAffectsTiming;
            relive.syncedLayerIndex = old.syncedLayerIndex;
            return relive;
        }

        public static void DumpLog(AnimatorControllerWrapper acWrapper)
        {
            {
                int lNum = 0;
                foreach (AnimatorControllerLayer layer in acWrapper.SourceAc.layers)
                {
                    Debug.Log("（＾～＾）オリジナルレイヤーの中身◆ lNum=[" + lNum + "] blendingMode=[" + layer.blendingMode + "] defaultWeight=[" + layer.defaultWeight + "] iKPass=[" + layer.iKPass + "] name=[" + layer.name + "] syncedLayerAffectsTiming=[" + layer.syncedLayerAffectsTiming + "] syncedLayerIndex=[" + layer.syncedLayerIndex + "]");
                    lNum++;
                }
            }
            {
                int lNum = 0;
                foreach (AnimatorControllerLayer layer in acWrapper.CopiedLayers)
                {
                    Debug.Log("（＾～＾）コピーレイヤーの中身◆ lNum=[" + lNum + "] blendingMode=[" + layer.blendingMode + "] defaultWeight=[" + layer.defaultWeight + "] iKPass=[" + layer.iKPass + "] name=[" + layer.name + "] syncedLayerAffectsTiming=[" + layer.syncedLayerAffectsTiming + "] syncedLayerIndex=[" + layer.syncedLayerIndex + "]");
                    lNum++;
                }
            }
        }

        /// <summary>
        /// 元からある全てのレイヤーを削除します。ただし、レイヤーの数を１個未満にすることはできないため、最後にダミーを１個残します。
        /// 工夫：シンク（参照）関係があるため、子関係のレイヤーから消していきます。
        /// </summary>
        /// <param name="acWrapper"></param>
        public static void DeleteAllLayers_AndPutDammy(AnimatorControllerWrapper acWrapper)
        {
            acWrapper.SourceAc.AddLayer(new AnimatorControllerLayer()); // ダミーを末尾に追加。
            const int dammy = 1;

            while (dammy < acWrapper.SourceAc.layers.Length) // ダミー以外、全部消す
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
                for (int lNum = acWrapper.SourceAc.layers.Length - 1 - dammy; -1 < lNum; lNum--) // 末尾のダミーを除く
                {
                    if (parentFlags[lNum])
                    {
                        acWrapper.SourceAc.RemoveLayer(lNum);
                    }
                }
            }
        }

        /// <summary>
        /// 全レイヤーを一旦退避して　アニメーション・コントローラーから全部削除し、再び全レイヤーを再追加します。
        /// ・レイヤー・インデックスでリンクを貼ってきている部分に考慮しています。順番の変更が起こらないように全削除、全再追加します。
        /// なんでUnityがこういう設計にしたのか推測すると、AddLayerのタイミングでプロパティ変更の適用を掛けているんじゃないだろうか。
        /// 
        /// 工夫で解消する点: 全部のレイヤーを削除することはできない。（最低１つのレイヤーを残しておく必要がある）
        /// </summary>
        /// <param name="i"></param>
        public static void RefreshAllLayers(AnimatorControllerWrapper acWrapper)
        {
            // 中身を確認してみよう。
            DumpLog(acWrapper);

            //{
            //    int lLen = acWrapper.CopiedLayers.Count;
            //    for (int lNum = 0; lNum < lLen; lNum++)
            //    {
            //        acWrapper.SourceAc.AddLayer(acWrapper.CopiedLayers[lNum]); // 追加し直す。
            //    }
            //}

            //List<AnimatorControllerLayer> reliveLayers = new List<AnimatorControllerLayer>(); // 退避先
            //foreach (AnimatorControllerLayer copiedLayer in ac.layers)
            //{
            //    // 全てのオブジェクトは破棄されてしまうので、移しておく。
            //    AnimatorControllerLayer reliveLayer = copiedLayer;
            //    //AnimatorControllerLayer reliveLayer = DeepCopy(actualLayer);
            //    reliveLayers.Add(copiedLayer); // 全退避！
            //}

            //{
            //    for (int lNum = acWrapper.SourceAc.layers.Length - 1; 0 < lNum; lNum--) // [0]の要素は消さない
            //    {
            //        acWrapper.SourceAc.RemoveLayer(lNum); // [0]の要素以外を全削除！
            //    }
            //    acWrapper.SourceAc.AddLayer(new AnimatorControllerLayer()); // ダミーを[1]に追加。
            //    acWrapper.SourceAc.RemoveLayer(0); // [0]の要素を消せるようになったので消す。ダミーは[0]に繰り上がる。
            //}

            //DeleteAllLayers_AndPutDammy(acWrapper);
            foreach (AnimatorControllerLayer copiedLayer in acWrapper.CopiedLayers)
            {
                acWrapper.SourceAc.AddLayer(copiedLayer); // 全再追加！ このときプロパティーの設定がUnityに反映されるはず。
            }
            //{
            //    acWrapper.SourceAc.RemoveLayer(0); // 先頭の[0]にあるダミーを消す。
            //}

            //// 同じ名前のレイヤーは存在しない前提のアルゴリズム。（スクリプトでAddLayer( )すると同名のレイヤーも追加できてしまう）
            //int oldLayerIndex = IndexOf_ByJustLayerName(i.SourceAc, i.SourceCopiedLayer.name); // 変更前のレイヤーの配列インデックスを覚えておく。
            //i.SourceAc.AddLayer(i.SourceCopiedLayer); // レイヤーを追加する。（同名のレイヤーが２つできている）
            //i.SourceAc.RemoveLayer(oldLayerIndex); // 変更前のレイヤーを削除する。

            //string newLayerName = i.SourceAc.layers[(i).SourceAc.layers.Length - 1].name; // 新しいレイヤー名を覚えておく。
            //i.SourceAc.RemoveLayer(IndexOf_ByJustLayerName(i.SourceAc, i.SourceCopiedLayer.name)); // 変更前のレイヤーを削除する。
            //i.SourceAc.AddLayer(i.SourceCopiedLayer); // 古い名前のレイヤーをもう１回追加。今度はレイヤー名はそのままのはず。
            //i.SourceAc.RemoveLayer(IndexOf_ByJustLayerName(i.SourceAc, newLayerName)); // 新しいレイヤー名のレイヤーを削除する。
        }

        public static void Update(AnimatorControllerWrapper acWrapper, DataManipulationRecord request, StringBuilder message)
        {
            //Debug.Log("レイヤーの更新要求☆（＾～＾） request.Fullpath=["+ request.Fullpath + "]");
            int caret = 0;
            FullpathTokens ft = new FullpathTokens();
            FullpathSyntaxP.Fixed_LayerName(request.Fullpath, ref caret, ref ft);
            AnimatorControllerLayer layer = Fetch_JustLayerName(acWrapper.SourceAc, ft.LayerNameEndsWithoutDot);
            int layerIndex = IndexOf_ByJustLayerName(acWrapper.SourceAc, request.Fullpath);
            //if (null == layer) { throw new UnityException("[" + request.Fullpath + "]レイヤーは見つからなかったぜ☆（＾～＾） ac=[" + ac.name + "]"); }
            if (layerIndex<0) { throw new UnityException("[" + request.Fullpath + "]レイヤーは見つからなかったぜ☆（＾～＾） ac=[" + acWrapper.SourceAc.name + "]"); }

            if (Operation_Something.HasProperty(request.Name, LayerRecord.Definitions, "レイヤー操作"))
            {
                //StringBuilder sb = new StringBuilder(); int i = 0; foreach (string name2 in LayerRecord.Definitions.Keys) { sb.Append("["); sb.Append(i); sb.Append("]"); sb.AppendLine(name2); i++; }
                //Debug.Log("レイヤーのプロパティーの更新要求☆（＾～＾） request.Fullpath=[" + request.Fullpath + "] request.Name=[" + request.Name + "] sb:"+sb.ToString());
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
            for (int lNum = ac.layers.Length - 1; -1 < lNum; lNum--) // 順序が繰り下がらないように、後ろから削除していく
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

    public abstract class Operation_ChildStatemachine
    {
        public static ChildAnimatorStateMachine[] DeepCopy(ChildAnimatorStateMachine[] oldItems)
        {
            ChildAnimatorStateMachine[] reliveItems = new ChildAnimatorStateMachine[oldItems.Length];
            int i = 0;
            foreach (ChildAnimatorStateMachine old in oldItems)
            {
                reliveItems[i] = DeepCopy(old);
                i++;
            }
            return reliveItems;
        }

        public static ChildAnimatorStateMachine DeepCopy(ChildAnimatorStateMachine old)
        {
            ChildAnimatorStateMachine relive = new ChildAnimatorStateMachine();
            relive.position = old.position;
            relive.stateMachine = Operation_Statemachine.DeepCopy(old.stateMachine);
            return relive;
        }
    }

    /// <summary>
    /// ステートマシン関連
    /// </summary>
    public abstract class Operation_Statemachine
    {
        #region 取得
        /// <summary>
        /// パスを指定すると ステートマシンを返す。
        /// </summary>
        /// <param name="query">"Base Layer.JMove" といった文字列。</param>
        public static AnimatorStateMachine Fetch(AnimatorController ac, FullpathTokens ft, AnimatorControllerLayer layer)
        {
            AnimatorStateMachine currentMachine = layer.stateMachine;

            if (0 < ft.StatemachineNamesEndsWithoutDot.Count) // ステートマシンが途中にある場合、最後のステートマシンまで降りていく。
            {
                currentMachine = GetLeafMachine(ac, currentMachine, ft.StatemachineNamesEndsWithoutDot);
                if (null == currentMachine) { throw new UnityException("無いノードが指定されたぜ☆（＾～＾）9 currentMachine.name=[" + currentMachine.name + "] nodes=[" + string.Join("][", ft.StatemachineNamesEndsWithoutDot.ToArray()) + "] ac.name=["+ac.name+"]"); }
            }

            return currentMachine;
        }

        /// <summary>
        /// 分かりづらいが、ノードの[1]～[length-1]を辿って、最後のステートマシンを返す。
        /// </summary>
        private static AnimatorStateMachine GetLeafMachine(AnimatorController ac, AnimatorStateMachine currentMachine, List<string> statemachineNamesEndsWithoutDot)// string[] nodes
        {
            //for (int i = Operation_Common.ROOT_NODE_IS_LAYER; i < nodes.Length + Operation_Common.LEAF_NODE_IS_STATE; i++)
            for (int i = 0; i < statemachineNamesEndsWithoutDot.Count; i++)
            {
                currentMachine = FetchChildMachine(currentMachine, statemachineNamesEndsWithoutDot[i]);
                if (null == currentMachine) { throw new UnityException("無いノードが指定されたぜ☆（＾～＾）10 i=[" + i + "] statemachineNamesEndsWithoutDot[i]=[" + statemachineNamesEndsWithoutDot[i] + "] ac.name=["+ac.name+"]"); }
            }
            return currentMachine;
        }

        /// <summary>
        /// ノード名から、ステートマシンを取得する。
        /// </summary>
        private static AnimatorStateMachine FetchChildMachine(AnimatorStateMachine machine, string childName)
        {
            foreach (ChildAnimatorStateMachine wrapper in machine.stateMachines)
            {
                if (wrapper.stateMachine.name == childName) { return wrapper.stateMachine; }
            }
            return null;
        }
        #endregion

        public static AnimatorStateMachine DeepCopy(AnimatorStateMachine old)
        {
            AnimatorStateMachine relive = new AnimatorStateMachine();
            relive.anyStatePosition = old.anyStatePosition;
            relive.anyStateTransitions = old.anyStateTransitions;
            relive.behaviours = old.behaviours;
            relive.defaultState = old.defaultState;
            relive.entryPosition = old.entryPosition;
            relive.entryTransitions = old.entryTransitions;
            relive.exitPosition = old.exitPosition;
            relive.hideFlags = old.hideFlags;
            relive.name = old.name;
            relive.parentStateMachinePosition = old.parentStateMachinePosition;
            relive.stateMachines = Operation_ChildStatemachine.DeepCopy(old.stateMachines);
            relive.states = old.states;
            return relive;
        }

        public static void Update(AnimatorController ac, DataManipulationRecord request, StringBuilder message)
        {
            int caret = 0;
            FullpathTokens ft = new FullpathTokens();
            if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames(request.Fullpath, ref caret, ref ft)) { throw new UnityException("[" + request.Fullpath + "]パース失敗だぜ☆（＾～＾） ac=[" + ac.name + "]"); }

            AnimatorControllerLayer layer = Operation_Layer.Fetch_JustLayerName(ac, ft.LayerNameEndsWithoutDot);

            AnimatorStateMachine statemachine = Fetch(ac, ft, layer);
            if (null == statemachine) { throw new UnityException("[" + request.Fullpath + "]ステートマシンは見つからなかったぜ☆（＾～＾） ac=[" + ac.name + "]"); }

            if (Operation_Something.HasProperty(request.Name, StatemachineRecord.Definitions, "ステートマシン操作"))
            {
                StatemachineRecord.Definitions[request.Name].Update(new StatemachineRecord.Wrapper(statemachine,
                    ft.StatemachinePath // 例えばフルパスが "Base Layer.Alpaca.Bear.Cat.Dog" のとき、"Alpaca.Bear.Cat"。
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

    public abstract class Operation_ChildState
    {
        #region 取得
        /// <summary>
        /// パスを指定すると ステートを返す。
        /// </summary>
        /// <param name="path">"Base Layer.JMove.JMove0" といった文字列。</param>
        public static ChildAnimatorState Fetch(AnimatorController ac, string path)
        {
            string[] nodes = path.Split('.'); // [0～length-2] ステートマシン名、[length-1] ステート名　（[0]はレイヤー名かも）
            if (nodes.Length < 2) { throw new UnityException("ノード数が２つ未満だったぜ☆（＾～＾） ステートマシン名か、ステート名は無いのかだぜ☆？ path=[" + path + "]"); }

            // 最初の名前[0]は、レイヤーを検索する。
            AnimatorStateMachine currentMachine = null;
            foreach (AnimatorControllerLayer layer in ac.layers) { if (nodes[0] == layer.name) { currentMachine = layer.stateMachine; break; } }
            if (null == currentMachine) { throw new UnityException("見つからないぜ☆（＾～＾）nodes=[" + string.Join("][", nodes) + "]"); }

            if (2 < nodes.Length) // ステートマシンが途中にある場合、最後のステートマシンまで降りていく。
            {
                currentMachine = FetchLeafMachine(currentMachine, nodes);
                if (null == currentMachine) { throw new UnityException("無いノードが指定されたぜ☆（＾～＾）9 currentMachine.name=[" + currentMachine.name + "] nodes=[" + string.Join("][", nodes) + "]"); }
            }

            return FetchChildState(currentMachine, nodes[nodes.Length - 1]); // レイヤーと葉だけの場合
        }

        /// <summary>
        /// 分かりづらいが、ノードの[1]～[length-1]を辿って、最後のステートマシンを返す。
        /// </summary>
        private static AnimatorStateMachine FetchLeafMachine(AnimatorStateMachine currentMachine, string[] nodes)
        {
            for (int i = Operation_Common.ROOT_NODE_IS_LAYER; i < nodes.Length + Operation_Common.LEAF_NODE_IS_STATE; i++)
            {
                currentMachine = FetchChildMachine(currentMachine, nodes[i]);
                if (null == currentMachine) { throw new UnityException("無いノードが指定されたぜ☆（＾～＾）10 i=[" + i + "] node=[" + nodes[i] + "]"); }
            }
            return currentMachine;
        }

        private static AnimatorStateMachine FetchChildMachine(AnimatorStateMachine machine, string childName)
        {
            foreach (ChildAnimatorStateMachine wrapper in machine.stateMachines)
            {
                if (wrapper.stateMachine.name == childName) { return wrapper.stateMachine; }
            }
            return null;
        }

        private static ChildAnimatorState FetchChildState(AnimatorStateMachine machine, string stateName)
        {
            foreach (ChildAnimatorState wrapper in machine.states)
            {
                if (wrapper.state.name == stateName) { return wrapper; }
            }
            throw new UnityException("チャイルド・A・ステートが見つからないぜ☆（＾～＾） stateName=[" + stateName + "]");
        }
        #endregion

        public static ChildAnimatorState DeepCopy(ChildAnimatorState old)
        {
            ChildAnimatorState relive = new ChildAnimatorState();
            relive.position = old.position;
            relive.state = Operation_State.DeepCopy( old.state);
            return relive;
        }

    }

    /// <summary>
    /// ステート関連
    /// </summary>
    public abstract class Operation_State
    {
        #region 取得
        /// <summary>
        /// パス・トークンを指定すると ステートを返す。
        /// </summary>
        public static AnimatorState Fetch(AnimatorController ac, FullpathTokens ft)
        {
            // 最初の名前[0]は、レイヤーを検索する。
            AnimatorControllerLayer layer = Operation_Layer.Fetch_JustLayerName(ac, ft.LayerNameEndsWithoutDot);
            AnimatorStateMachine currentMachine = layer.stateMachine;
            if (null == currentMachine) { throw new UnityException("見つからないぜ☆（＾～＾）nodes=[" + ft.StatemachinePath + "]"); }

            if (0 < ft.StatemachineNamesEndsWithoutDot.Count) // ステートマシンが途中にある場合、最後のステートマシンまで降りていく。
            {
                currentMachine = FetchLeafMachine(currentMachine, ft.StatemachineNamesEndsWithoutDot);
                if (null == currentMachine) { throw new UnityException("無いノードが指定されたぜ☆（＾～＾）9 currentMachine.name=[" + currentMachine.name + "] nodes=[" + ft.StatemachinePath + "]"); }
            }

            return FetchChildState(currentMachine, ft.StateName); // 葉
        }

        /// <summary>
        /// 分かりづらいが、ノードの[1]～[length-1]を辿って、最後のステートマシンを返す。
        /// </summary>
        private static AnimatorStateMachine FetchLeafMachine(AnimatorStateMachine currentMachine, List<string> statemachineNamesEndsWithoutDot)// string[] nodes
        {
            //for (int i = Operation_Common.ROOT_NODE_IS_LAYER; i < nodes.Length + Operation_Common.LEAF_NODE_IS_STATE; i++)
            for (int i = 0; i < statemachineNamesEndsWithoutDot.Count; i++)
            {
                currentMachine = FetchChildMachine(currentMachine, statemachineNamesEndsWithoutDot[i]);
                if (null == currentMachine) { throw new UnityException("無いノードが指定されたぜ☆（＾～＾）10 i=[" + i + "] node=[" + statemachineNamesEndsWithoutDot[i] + "]"); }
            }
            return currentMachine;
        }

        private static AnimatorStateMachine FetchChildMachine(AnimatorStateMachine machine, string childName)
        {
            foreach (ChildAnimatorStateMachine wrapper in machine.stateMachines)
            {
                if (wrapper.stateMachine.name == childName) { return wrapper.stateMachine; }
            }
            return null;
        }

        private static AnimatorState FetchChildState(AnimatorStateMachine machine, string stateName)
        {
            foreach (ChildAnimatorState wrapper in machine.states)
            {
                if (wrapper.state.name == stateName) { return wrapper.state; }
            }
            return null;
        }
        #endregion

        public static AnimatorState DeepCopy(AnimatorState old)
        {
            AnimatorState relive = new AnimatorState();
            relive.behaviours = old.behaviours;
            relive.cycleOffset = relive.cycleOffset;
            relive.cycleOffsetParameter = relive.cycleOffsetParameter;
            relive.cycleOffsetParameterActive = relive.cycleOffsetParameterActive;
            relive.hideFlags = relive.hideFlags;
            relive.iKOnFeet = relive.iKOnFeet;
            relive.mirror = relive.mirror;
            relive.mirrorParameter = relive.mirrorParameter;
            relive.mirrorParameterActive = relive.mirrorParameterActive;
            relive.motion = relive.motion;
            relive.name = relive.name;
            // relive.nameHash = relive.nameHash;
            relive.speed = relive.speed;
            relive.speedParameter = relive.speedParameter;
            relive.speedParameterActive = relive.speedParameterActive;
            relive.tag = relive.tag;
            relive.transitions = relive.transitions;
            // relive.uniqueName = relive.uniqueName;
            // relive.uniqueNameHash = relive.uniqueNameHash;
            relive.writeDefaultValues = relive.writeDefaultValues;
            return relive;
        }

        public static void Update(AnimatorController ac, DataManipulationRecord request, StringBuilder message)
        {
            int caret = 0;
            FullpathTokens ft = new FullpathTokens();
            if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(request.Fullpath, ref caret, ref ft)) { throw new UnityException("[" + request.Fullpath + "]パース失敗だぜ☆（＾～＾） ac=[" + ac.name + "]"); }

            AnimatorState state = Fetch(ac, ft);
            if (null == state) { throw new UnityException("[" + request.Fullpath + "]ステートは見つからなかったぜ☆（＾～＾） ac=[" + ac.name + "]"); }

            if (Operation_Something.HasProperty(request.Name, StateRecord.Definitions, "ステート操作"))
            {
                StateRecord.Definitions[request.Name].Update(new StateRecord.Wrapper(state), request, message);
                //,ft.StatemachinePath
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

                    for (int caNum = statemachine.states.Length - 1; -1 < caNum; caNum--) // 順序が繰り下がらないように、後ろから削除していく
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
                if (null == state) { throw new UnityException("検索結果にヌル・ステートが含まれていたぜ☆（＞＿＜） states.Count=[" + states.Count + "]"); }
                recordSet.Add(new StateRecord(0,0,0, state));
                //,"#NoData"
            }
            message.Append("result: "); message.Append(recordSet.Count); message.AppendLine(" records.");
        }
    }

    /// <summary>
    /// トランジション関連
    /// </summary>
    public abstract class Operation_Transition
    {
        #region 取得
        public static AnimatorStateTransition Fetch(AnimatorController ac, DataManipulationRecord request)
        {
            if (null == request.TransitionNum_ofFullpath) { throw new UnityException("トランジション番号が指定されていないぜ☆（＾～＾） トランジション番号=[" + request.TransitionNum_ofFullpath + "] ac=[" + ac.name + "]"); }
            int fullpathTransition = int.Parse(request.TransitionNum_ofFullpath);

            int caret = 0;
            FullpathTokens ft = new FullpathTokens();
            if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(request.Fullpath, ref caret, ref ft)) { throw new UnityException("[" + request.Fullpath + "]パース失敗だぜ☆（＾～＾） ac=[" + ac.name + "]"); }

            AnimatorState state = Operation_State.Fetch(ac, ft);
            if (null == state) { throw new UnityException("[" + request.Fullpath + "]ステートは見つからなかったぜ☆（＾～＾） ac=[" + ac.name + "]"); }

            int tNum = 0;
            foreach (AnimatorStateTransition transition in state.transitions)
            {
                if (fullpathTransition == tNum) { return transition; }
                tNum++;
            }

            return null;// TODO:
        }

        /// <summary>
        /// ２つのステートを トランジションで結ぶ。
        /// </summary>
        /// <param name="path_src">"Base Layer.JMove.JMove0" といった文字列。</param>
        public static AnimatorStateTransition Fetch(AnimatorController ac, string path_src, string path_dst)
        {
            AnimatorState state_src;
            {
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(path_src, ref caret, ref ft)) { throw new UnityException("[" + path_src + "]パース失敗だぜ☆（＾～＾） ac=[" + ac.name + "]"); }
                state_src = Operation_State.Fetch(ac, ft);
            }
            AnimatorState state_dst;
            {
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(path_dst, ref caret, ref ft)) { throw new UnityException("[" + path_dst + "]パース失敗だぜ☆（＾～＾） ac=[" + ac.name + "]"); }
                state_dst = Operation_State.Fetch(ac, ft);
            }

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
            AnimatorState sourceState; // 遷移元のステート
            {
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(request.Fullpath, ref caret, ref ft)) { throw new UnityException("[" + request.Fullpath + "]パース失敗だぜ☆（＾～＾） ac=[" + ac.name + "]"); }
                sourceState = Operation_State.Fetch(ac, ft);
            }
            //AnimatorStateTransition sourceTransition = Operation_Transition.Lookup(ac, request); // トランジション

            string destinationFullpath = request.New;// TODO: 遷移先のステートを指定する？
            AnimatorState destinationState; // 遷移先のステート
            {
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(destinationFullpath, ref caret, ref ft)) { throw new UnityException("[" + destinationFullpath + "]パース失敗だぜ☆（＾～＾） ac=[" + ac.name + "]"); }
                destinationState = Operation_State.Fetch(ac, ft);
            }
            sourceState.AddTransition(destinationState);
        }
        public static void Update(AnimatorController ac, DataManipulationRecord request, StringBuilder message)
        {
            if (Operation_Something.HasProperty(request.Name, TransitionRecord.Definitions, "トランジション操作"))
            {
                AnimatorState state;
                {
                    int caret = 0;
                    FullpathTokens ft = new FullpathTokens();
                    if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(request.Fullpath, ref caret, ref ft)) { throw new UnityException("[" + request.Fullpath + "]パース失敗だぜ☆（＾～＾） ac=[" + ac.name + "]"); }
                    state = Operation_State.Fetch(ac, ft);
                }
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
            AnimatorState sourceState; // 遷移元のステート
            {
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(request.Fullpath, ref caret, ref ft)) { throw new UnityException("[" + request.Fullpath + "]パース失敗だぜ☆（＾～＾） ac=[" + ac.name + "]"); }
                sourceState = Operation_State.Fetch(ac, ft);
            }

            AnimatorStateTransition sourceTransition = Fetch(ac, request); // 削除するトランジション
            sourceState.RemoveTransition(sourceTransition);
        }

        /// <summary>
        /// ステートマシンの[Any State]からステートへ、トランジションで結ぶ。
        /// </summary>
        public static bool AddAnyState(AnimatorController ac, HashSet<AnimatorStateMachine> statemachines_src, HashSet<AnimatorState> states_dst, StringBuilder info_message)
        {
            info_message.Append("Transition.AddAnyState: Source "); info_message.Append(statemachines_src.Count); info_message.Append(" states. Destination "); info_message.Append(states_dst.Count); info_message.AppendLine(" states.");
            foreach (AnimatorStateMachine statemachine_src in statemachines_src)
            {
                foreach (AnimatorState state_dst in states_dst)
                {
                    // info_message.Append("Insert: "); info_message.Append(null== statemachine_src ? "ヌル" : statemachine_src.name); info_message.Append(" -> "); info_message.AppendLine(null== state_dst ? "ヌル" : state_dst.name);
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
            info_message.Append("Transition.AddEntryState: Source "); info_message.Append(statemachines_src.Count); info_message.Append(" states. Destination "); info_message.Append(states_dst.Count); info_message.AppendLine(" states.");
            foreach (AnimatorStateMachine statemachine_src in statemachines_src)
            {
                foreach (AnimatorState state_dst in states_dst)
                {
                    //info_message.Append("Insert: "); info_message.Append(null == statemachine_src ? "ヌル" : statemachine_src.name); info_message.Append(" -> "); info_message.AppendLine(null == state_dst ? "ヌル" : state_dst.name);
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
                //message.Append("Insert: "); message.Append(null == state_src ? "ヌル" : state_src.name); message.Append(" -> Exit");;
                if (null == state_src) { return false; }
                state_src.AddExitTransition();
            }
            return true;
        }

        /// <summary>
        /// ２つのステートを トランジションで結ぶ。ステートは複数指定でき、src→dst方向の総当たりで全部結ぶ。
        /// </summary>
        /// <param name="path_src">"Base Layer.JMove.JMove0" といった文字列。</param>
        public static void Insert(AnimatorController ac, HashSet<AnimatorState> states_src, HashSet<AnimatorState> states_dst, Dictionary<string, string> properties, StringBuilder info_message)
        {
            info_message.Append("Transition.Insert: Source "); info_message.Append(states_src.Count); info_message.Append(" states. Destination "); info_message.Append(states_dst.Count); info_message.AppendLine(" states.");
            foreach (AnimatorState state_src in states_src) {
                foreach (AnimatorState state_dst in states_dst) {
                    //message.Append("Insert: "); message.Append(state_src.name); message.Append(" -> "); message.AppendLine(state_dst.name);
                    AnimatorStateTransition transition = state_src.AddTransition(state_dst);
                    UpdateProperty(ac, transition, properties);
                }
            }
        }

        /// <summary>
        /// ２つのステート間の トランジションを削除する。ステートは複数指定でき、src→dst方向の総当たりで全部削除する。
        /// </summary>
        /// <param name="path_src">"Base Layer.JMove.JMove0" といった文字列。</param>
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
                            // break; // src → dst 間に複数のトランジションを貼れるみたいなんで、全部消そう☆
                        }
                    }
                }
            }
        }

        public static void Update(AnimatorController ac, HashSet<AnimatorState> states_src, HashSet<AnimatorState> states_dst, Dictionary<string, string> properties, StringBuilder info_message)
        {
            foreach (AnimatorState state_src in states_src) // 指定されたステート全て対象
            {
                foreach (AnimatorStateTransition transition in state_src.transitions)
                {
                    foreach (AnimatorState state_dst in states_dst) // 指定されたステート全て対象
                    {
                        if (state_dst == transition.destinationState)
                        {
                            //message.Append("Update: ");
                            //message.Append(state_src.name);
                            //message.Append(" -> ");
                            //message.Append(state_dst.name);
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
                    default: throw new UnityException("未対応のトランジション・プロパティー [" + pair.Key + "]=[" + pair.Value + "]");
                }
            }
        }

        public static void Select(AnimatorController ac, HashSet<AnimatorState> states_src, HashSet<AnimatorState> states_dst, out HashSet<TransitionRecord> hitRecords, StringBuilder message)
        {
            hitRecords = new HashSet<TransitionRecord>();
            StringBuilder stellaQLComment = new StringBuilder(); // SELECT文で出力するシート用の人間に読みやすいように補足する説明
            foreach (AnimatorState state_src in states_src) // 指定されたステート全て対象
            {
                foreach (AnimatorStateTransition transition in state_src.transitions)
                {
                    foreach (AnimatorState state_dst in states_dst) // 指定されたステート全て対象
                    {
                        if (state_dst == transition.destinationState)
                        {
                            stellaQLComment.Append("Select: "); stellaQLComment.Append(state_src.name); stellaQLComment.Append(" -> "); stellaQLComment.Append(state_dst.name); // CSVにするので改行は入れない。
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
        #region 取得
        public static ConditionRecord.AnimatorConditionWrapper Fetch(AnimatorController ac, DataManipulationRecord request)
        {
            AnimatorStateTransition transition = Operation_Transition.Fetch(ac, request);
            if (null != transition) { return Operation_Condition.Fetch(ac, request); }
            
            return null;// TODO:
        }

        public static ConditionRecord.AnimatorConditionWrapper Fetch(AnimatorController ac, AnimatorStateTransition transition, DataManipulationRecord request)
        {
            int fullpathCondition = int.Parse(request.ConditionNum_ofFullpath);

            int cNum = 0;
            foreach (AnimatorCondition condition in transition.conditions)
            {
                if (fullpathCondition == cNum) { return new ConditionRecord.AnimatorConditionWrapper(condition); }
                cNum++;
            }

            return new ConditionRecord.AnimatorConditionWrapper(); // 空コンストラクタで生成した場合、.IsNull( ) メソッドでヌルを返す。
        }
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
            AnimatorStateTransition transition = Operation_Transition.Fetch(ac, requestSet.RepresentativeRecord); // １つ上のオブジェクト（トランジション）
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
            AnimatorStateTransition transition = Operation_Transition.Fetch(ac, requestSet.RepresentativeRecord);// トランジション
            ConditionRecord.AnimatorConditionWrapper wapper = Fetch(ac, transition, requestSet.RepresentativeRecord);
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
            AnimatorStateTransition transition = Operation_Transition.Fetch(ac, requestSet.RepresentativeRecord);// トランジション
            ConditionRecord.AnimatorConditionWrapper wapper = Operation_Condition.Fetch(ac, transition, requestSet.RepresentativeRecord);
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
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames(request.Fullpath, ref caret, ref ft)) { throw new UnityException("[" + request.Fullpath + "]パース失敗だぜ☆（＾～＾） ac=[" + ac.name + "]"); }

                AnimatorControllerLayer layer = Operation_Layer.Fetch_JustLayerName(ac, ft.LayerNameEndsWithoutDot);
                AnimatorStateMachine statemachine = Operation_Statemachine.Fetch(ac, ft, layer);

                if (null == statemachine) { throw new UnityException("[" + request.Fullpath + "]ステートマシンは見つからなかったぜ☆（＾～＾） ac=[" + ac.name + "]"); }

                if (Operation_Something.HasProperty(request.Name, PositionRecord.Definitions, "ステートマシンのポジション操作"))
                {
                    PositionRecord.Definitions[request.Name].Update(new PositionRecord.PositionWrapper(statemachine, request.Propertyname_ofFullpath), request, message);
                }
            }
            else // ステートのポジション
            {
                ChildAnimatorState caState = Operation_ChildState.Fetch(ac, request.Fullpath); // 構造体☆

                if ("states" == request.Foreignkeycategory)
                {
                    if (Operation_Something.HasProperty(request.Name, PositionRecord.Definitions, "ステートのポジション操作"))
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

    public abstract class Operation_AvatarMask
    {
        #region 検索
        public static AvatarMask Lookup_incomplete(string name)
        {
            //AnimatorController ac,
            return null;
            //AssetDatabase.get
            //ac.layers[0].avatarMask
        }
        #endregion
    }

    public abstract class Operation_AnimatorLayerBlendingMode
    {
        #region 取得
        /// <summary>
        /// 参照:「列挙体のメンバの値や名前を列挙する」http://dobon.net/vb/dotnet/programing/enumgetvalues.html
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static HashSet<AnimatorLayerBlendingMode> Fetch(string blendingModeName_regex)
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
        #endregion
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
