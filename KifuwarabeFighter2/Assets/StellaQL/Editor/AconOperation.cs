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
            // 二重構造 [ステート・フルパス、トランジション番号,命令リスト] ※1つのトランジションに複数の命令が飛んでくることはある。
            Dictionary<string, Dictionary<int, List<DataManipulationRecord>>> wrap3Dic_transition = new Dictionary<string, Dictionary<int, List<DataManipulationRecord>>>();
            // 三重構造 [ステート・フルパス、トランジション番号、コンディション番号] ※１つのコンディションは１～３フィールドで１つの更新要求になる。FIXME: １つのコンディションに複数の命令が飛んでくることはないのかだぜ☆？（＾～＾）
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
                            Dictionary<int, List<DataManipulationRecord>> wrap2Dic; // [ステートフルパス,]
                            if (wrap3Dic_transition.ContainsKey(request_packet.Fullpath)) { wrap2Dic = wrap3Dic_transition[request_packet.Fullpath]; } // 既存
                            else { wrap2Dic = new Dictionary<int, List<DataManipulationRecord>>(); wrap3Dic_transition[request_packet.Fullpath] = wrap2Dic; } // 新規追加

                            int tNum = int.Parse(request_packet.TransitionNum_ofFullpath); // トランジション番号
                            if (!wrap2Dic.ContainsKey(tNum))
                            {
                                //ebug.Log("トランジションの新しい要求: tNum=[" + tNum + "]");
                                wrap2Dic.Add(tNum, new List<DataManipulationRecord>());
                            }// １つのトランジションに複数の命令がくることもある。
                            else
                            {
                                //ebug.Log("トランジションの既存の要求: tNum=[" + tNum + "]");
                            }
                            wrap2Dic[tNum].Add(request_packet);
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

                            Debug.Log("コンディション条件まとめ request_packet.Name=["+ request_packet.Name + "]");
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

            #region トランジションを消化
            {
                // 挿入、更新、削除、接続先変更　に振り分けたい。
                List<DataManipulationRecord> insertsNew_Set = new List<DataManipulationRecord>();
                List<DataManipulationRecord> insertsChangeDestination_Set = new List<DataManipulationRecord>();
                List<DataManipulationRecord> deletesSet = new List<DataManipulationRecord>();
                List<DataManipulationRecord> updatesSet = new List<DataManipulationRecord>();
                //ebug.Log("largeDic_transition.Count=" + largeDic_transition.Count);// [ステート・フルパス,トランジション番号]
                foreach (KeyValuePair<string, Dictionary<int, List<DataManipulationRecord>>> wrap3_request in wrap3Dic_transition)
                {
                    //ebug.Log("wrap3_request.Value.Count=" + wrap3_request.Value.Count);// [,トランジション番号]
                    foreach (KeyValuePair<int, List<DataManipulationRecord>> wrap2_request in wrap3_request.Value)
                    {
                        //ebug.Log("wrap2_request.Value.Count=" + wrap2_request.Value.Count);
                        if (0<wrap2_request.Value.Count) // 同じトランジションに複数の命令がある☆（＾▽＾）
                        {
                            AnimatorStateTransition transition = AconFetcher.FetchTransition(acWrapper.SourceAc, wrap2_request.Value[0]);// 先頭要素だけ見れば十分☆

                            foreach (DataManipulationRecord request in wrap2_request.Value)
                            {
                                //ebug.Log("request.Name=" + request.Name);

                                if ("#DestinationFullpath#" == request.Name) // トランジションの遷移先へのデータ操作命令
                                {
                                    if ("" != request.New)
                                    {// New に指定があり
                                        if ("" == request.Old) {
                                            //ebug.Log("insertsNew_Set request.Name=" + request.Name);
                                            insertsNew_Set.Add(request); }// Old に指定がなければ新規挿入
                                        else {
                                            //ebug.Log("insertsChangeDestination_Set request.Name=" + request.Name);
                                            insertsChangeDestination_Set.Add(request);
                                        }// Old に指定があれば接続先変更
                                    }
                                    else if(request.IsClear) {
                                        //ebug.Log("deletesSet request.Name=" + request.Name);

                                        deletesSet.Add(request); }// 削除要求の場合、削除 に振り分ける。
                                    else {
                                        throw new UnityException("トランジションの遷移先に対して未対応の更新要求だぜ☆（＾～＾） request.Name=["+ request.Name + "] request.Old=["+ request.Old + "] request.New=["+ request.New + "]");
                                    }
                                }
                                else
                                {
                                    if (request.IsClear)
                                    {
                                        //ebug.Log("deletesSet request.Name=" + request.Name);

                                        deletesSet.Add(request);
                                    }// 削除要求の場合、削除 に振り分ける。
                                    else {
                                        if (HasProperty(request.Name, TransitionRecord.Definitions, "トランジション操作"))
                                        {
                                            //ebug.Log("updatesSet request.Name=" + request.Name);
                                            updatesSet.Add(request);
                                        }
                                    }// それ以外の場合は、更新 に振り分ける。
                                }
                            }
                        }
                    }
                }

                foreach (DataManipulationRecord request in insertsNew_Set) { Operation_Transition.Insert_New(acWrapper.SourceAc, request, info_message); }// 新規挿入
                foreach (DataManipulationRecord request in insertsChangeDestination_Set) { Operation_Transition.Insert_ChangeDestination(acWrapper.SourceAc, request, info_message); }// 遷移先変更
                deletesSet.Sort((DataManipulationRecord a, DataManipulationRecord b) =>
                { // 削除要求を、連番の逆順にする
                    int stringCompareOrder = string.CompareOrdinal(a.Fullpath, b.Fullpath);
                    if (0 != stringCompareOrder) { return stringCompareOrder; } // ステート名の順番はそのまま。
                    else if (int.Parse(a.TransitionNum_ofFullpath) < int.Parse(b.TransitionNum_ofFullpath)) { return -1; } // トランジションの順番は一応後ろから
                    else if (int.Parse(b.TransitionNum_ofFullpath) < int.Parse(a.TransitionNum_ofFullpath)) { return 1; }
                    return 0;
                });
                foreach (DataManipulationRecord request in deletesSet) { Operation_Transition.Delete(acWrapper.SourceAc, request, info_message); }// 削除を処理
                foreach (DataManipulationRecord request in updatesSet) { Operation_Transition.Update(acWrapper.SourceAc, request, info_message); }// 更新
            }
            #endregion
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
                                AnimatorStateTransition transition = AconFetcher.FetchTransition_ByFullpath(acWrapper.SourceAc,
                                    conditionRecordSet2Pair.Value.RepresentativeFullpath,
                                    conditionRecordSet2Pair.Value.RepresentativeFullpathTransition
                                    );// トランジション
                                ConditionRecord.AnimatorConditionWrapper wapper = AconFetcher.FetchCondition(
                                    acWrapper.SourceAc,
                                    transition,
                                    conditionRecordSet2Pair.Value.RepresentativeFullpathCondition
                                    );// コンディション

                                if (wapper.IsNull) { insertsSet.Add(conditionRecordSet2Pair.Value); }// 存在しないコンディション番号だった場合、 挿入 に振り分ける。
                                else if (null != conditionRecordSet2Pair.Value.Parameter && conditionRecordSet2Pair.Value.Parameter.IsClear) { deletesSet.Add(conditionRecordSet2Pair.Value); }// 削除要求の場合、削除 に振り分ける。
                                else { updatesSet.Add(conditionRecordSet2Pair.Value); }// それ以外の場合は、更新 に振り分ける。
                            }
                        }
                    }
                }

                foreach (Operation_Condition.DataManipulatRecordSet requestSet in insertsSet) {
                    Debug.Log("コンディション Insert");
                    Operation_Condition.Insert(acWrapper.SourceAc, requestSet, info_message);
                }// 更新を処理
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
                foreach (Operation_Condition.DataManipulatRecordSet requestSet in deletesSet) {
                    Debug.Log("コンディション Delete");
                    Operation_Condition.Delete(acWrapper.SourceAc, requestSet, info_message);
                }// 削除を処理
                foreach (Operation_Condition.DataManipulatRecordSet requestSet in updatesSet) {
                    Debug.Log("コンディション Update");
                    Operation_Condition.Update(acWrapper.SourceAc, requestSet, info_message);
                }// 挿入を処理
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
        /// <summary>
        /// UPDATE 要求
        /// </summary>
        public static void Update(AnimatorController ac, DataManipulationRecord request, StringBuilder message)
        {
            if (Operation_Something.HasProperty(request.Name, ParameterRecord.Definitions, "パラメーター操作"))
            {
                AnimatorControllerParameter parameter = AconFetcher.FetchParameter(ac, request.Fullpath);

                // 各プロパティーに UPDATE要求を 振り分ける
                ParameterRecord.Definitions[request.Name].Update(parameter, request, message);
                // throw new UnityException("[" + request.Fullpath + "]パラメーターには未対応だぜ☆（＾～＾） ac=[" + ac.name + "]");
            }
        }
    }

    public abstract class Operation_Layer
    {
        #region 複製
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
            //ebug.Log("レイヤーの更新要求☆（＾～＾） request.Fullpath=["+ request.Fullpath + "]");
            int caret = 0;
            FullpathTokens ft = new FullpathTokens();
            FullpathSyntaxP.Fixed_LayerName(request.Fullpath, ref caret, ref ft);
            AnimatorControllerLayer layer = AconFetcher.FetchLayer_JustLayerName(acWrapper.SourceAc, ft.LayerNameEndsWithoutDot);
            int layerIndex = IndexOf_ByJustLayerName(acWrapper.SourceAc, request.Fullpath);
            //if (null == layer) { throw new UnityException("[" + request.Fullpath + "]レイヤーは見つからなかったぜ☆（＾～＾） ac=[" + ac.name + "]"); }
            if (layerIndex<0) { throw new UnityException("[" + request.Fullpath + "]レイヤーは見つからなかったぜ☆（＾～＾） ac=[" + acWrapper.SourceAc.name + "]"); }

            if (Operation_Something.HasProperty(request.Name, LayerRecord.Definitions, "レイヤー操作"))
            {
                //StringBuilder sb = new StringBuilder(); int i = 0; foreach (string name2 in LayerRecord.Definitions.Keys) { sb.Append("["); sb.Append(i); sb.Append("]"); sb.AppendLine(name2); i++; }
                //ebug.Log("レイヤーのプロパティーの更新要求☆（＾～＾） request.Fullpath=[" + request.Fullpath + "] request.Name=[" + request.Name + "] sb:"+sb.ToString());
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
        #region 複製
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
        #endregion

        public static void Update(AnimatorController ac, DataManipulationRecord request, StringBuilder message)
        {
            int caret = 0;
            FullpathTokens ft = new FullpathTokens();
            if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames(request.Fullpath, ref caret, ref ft)) { throw new UnityException("[" + request.Fullpath + "]パース失敗だぜ☆（＾～＾） ac=[" + ac.name + "]"); }

            AnimatorControllerLayer layer = AconFetcher.FetchLayer_JustLayerName(ac, ft.LayerNameEndsWithoutDot);

            AnimatorStateMachine statemachine = AconFetcher.FetchStatemachine(ac, ft, layer);
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
        #region 複製
        public static ChildAnimatorState DeepCopy(ChildAnimatorState old)
        {
            ChildAnimatorState relive = new ChildAnimatorState();
            relive.position = old.position;
            relive.state = Operation_State.DeepCopy(old.state);
            return relive;
        }
        #endregion
    }

    /// <summary>
    /// ステート関連
    /// </summary>
    public abstract class Operation_State
    {
        #region 複製
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
        #endregion

        public static void Update(AnimatorController ac, DataManipulationRecord request, StringBuilder message)
        {
            int caret = 0;
            FullpathTokens ft = new FullpathTokens();
            if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(request.Fullpath, ref caret, ref ft)) { throw new UnityException("[" + request.Fullpath + "]パース失敗だぜ☆（＾～＾） ac=[" + ac.name + "]"); }

            AnimatorState state = AconFetcher.FetchState(ac, ft);
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
        #endregion
        #region 複製
        /// <summary>
        /// 容れ物を変える程度に使う。
        /// </summary>
        public static AnimatorStateTransition ShallowCopy_ExceptDestinaionState(AnimatorStateTransition dst, AnimatorStateTransition src)
        {
            dst.canTransitionToSelf = src.canTransitionToSelf;
            dst.conditions = src.conditions;
            // 遷移先は除く dst.destinationState = src.destinationState;
            dst.destinationStateMachine = src.destinationStateMachine; // これはコピーする☆
            dst.duration = src.duration;
            dst.exitTime = src.exitTime;
            dst.hasExitTime = src.hasExitTime;
            dst.hasFixedDuration = src.hasFixedDuration;
            dst.hideFlags = src.hideFlags;
            dst.interruptionSource = src.interruptionSource;
            dst.isExit = src.isExit;
            dst.mute = src.mute;
            dst.name = src.name;
            dst.offset = src.offset;
            dst.orderedInterruption = src.orderedInterruption;
            dst.solo = src.solo;
            return dst;
        }
        #endregion

        /// <summary>
        /// 新規挿入
        /// </summary>
        public static void Insert_New(AnimatorController ac, DataManipulationRecord request, StringBuilder message)
        {
            AnimatorState srcState; // 遷移元のステート
            {
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(request.Fullpath, ref caret, ref ft)) { throw new UnityException("[" + request.Fullpath + "]パース失敗だぜ☆（＾～＾） ac=[" + ac.name + "]"); }
                srcState = AconFetcher.FetchState(ac, ft);
            }
            //AnimatorStateTransition sourceTransition = Operation_Transition.Lookup(ac, request); // トランジション

            AnimatorState dstState; // 遷移先のステート
            {
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(request.New, ref caret, ref ft)) { throw new UnityException("[" + request.New + "]パース失敗だぜ☆（＾～＾） ac=[" + ac.name + "]"); }
                dstState = AconFetcher.FetchState(ac, ft);
            }
            srcState.AddTransition(dstState);
        }
        /// <summary>
        /// 遷移先変更
        /// </summary>
        public static void Insert_ChangeDestination(AnimatorController ac, DataManipulationRecord request, StringBuilder message)
        {
            // ebug.Log("Insert_ChangeDestination: 開始");
            AnimatorState srcState; // 遷移元のステート
            {
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(request.Fullpath, ref caret, ref ft)) { throw new UnityException("[" + request.Fullpath + "]パース失敗だぜ☆（＾～＾） ac=[" + ac.name + "]"); }
                srcState = AconFetcher.FetchState(ac, ft);
            }
            // ebug.Log("Insert_ChangeDestination: srcState.name=[" + srcState.name +"]" );

            AnimatorState dstOldState; // 古い遷移先のステート
            {
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(request.Old, ref caret, ref ft)) { throw new UnityException("[" + request.Old + "]パース失敗だぜ☆（＾～＾） ac=[" + ac.name + "]"); }
                dstOldState = AconFetcher.FetchState(ac, ft);
            }
            // ebug.Log("Insert_ChangeDestination: dstOldState.name=[" + dstOldState.name + "]");

            AnimatorState dstNewState; // 新しい遷移先のステート
            {
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(request.New, ref caret, ref ft)) { throw new UnityException("[" + request.New + "]パース失敗だぜ☆（＾～＾） ac=[" + ac.name + "]"); }
                dstNewState = AconFetcher.FetchState(ac, ft);
            }
            // ebug.Log("Insert_ChangeDestination: dstNewState.name=[" + dstNewState.name + "]");

            List<AnimatorStateTransition> removeList = new List<AnimatorStateTransition>();
            // FIXME: ほんとは トランジション番号も指定されているのでは？
            List<AnimatorStateTransition> oldTransitions = AconFetcher.FetchTransition_SrcDst(ac, request.Fullpath, request.Old); // 削除するトランジションを全て取得
            // ebug.Log("Insert_ChangeDestination: oldTransitions.Count=[" + oldTransitions.Count + "]");
            foreach (AnimatorStateTransition oldTransition in oldTransitions)
            {
                srcState.AddTransition(dstNewState);    // トランジションを追加する。
                // ebug.Log("Insert_ChangeDestination: トランジション追加☆ [" + srcState.name + "]->["+ dstNewState.name + "]");
                ShallowCopy_ExceptDestinaionState(srcState.transitions[srcState.transitions.Length - 1], oldTransition);   // 遷移先ステート以外の中身を写しておく。
                removeList.Add(oldTransition);
            }
            // ebug.Log("Insert_ChangeDestination: removeList.Count=[" + removeList.Count + "]");

            foreach (AnimatorStateTransition removeeTransition in removeList)
            {
                // ebug.Log("Insert_ChangeDestination: トランジション削除☆ ->[" + removeeTransition.destinationState.name + "]"); // 削除する前にアクセスすること。
                srcState.RemoveTransition(removeeTransition);   // トランジションを削除する。
            }
        }
        public static void Update(AnimatorController ac, DataManipulationRecord request, StringBuilder message)
        {
            //ebug.Log("Operation_Transition Update 開始 request.Name=" + request.Name);
            AnimatorState srcState; // 遷移元ステート
            {
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(request.Fullpath, ref caret, ref ft)) { throw new UnityException("[" + request.Fullpath + "]パース失敗だぜ☆（＾～＾） ac=[" + ac.name + "]"); }
                srcState = AconFetcher.FetchState(ac, ft);
            }
            if (null == srcState) { throw new UnityException("[" + request.Fullpath + "]ステートは見つからなかったぜ☆（＾～＾） ac=[" + ac.name + "]"); }

            int transitionNum = int.Parse(request.TransitionNum_ofFullpath); // トランジション番号
            //ebug.Log("Operation_Transition Update 開始 transitionNum=" + transitionNum);

            int tNum = 0;
            foreach( AnimatorStateTransition transition in srcState.transitions)
            {
                if (transitionNum==tNum)
                {
                    //ebug.Log("Operation_Transition Update 実行前 tNum=[" + tNum + "] request.Name=["+ request.Name + "]");
                    TransitionRecord.Definitions[request.Name].Update(transition, request, message);
                    break;
                }
                tNum++;
            }
        }
        /// <summary>
        /// トランジションを削除します。
        /// トランジション番号の大きい物から順に削除してください。トランジション番号の小さい物から削除すると番号が繰り上がってしまうため。
        /// </summary>
        public static void Delete(AnimatorController ac, DataManipulationRecord request, StringBuilder message)
        {
            AnimatorState srcState; // 遷移元のステート
            {
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(request.Fullpath, ref caret, ref ft)) { throw new UnityException("[" + request.Fullpath + "]パース失敗だぜ☆（＾～＾） ac=[" + ac.name + "]"); }
                srcState = AconFetcher.FetchState(ac, ft);
            }

            AnimatorStateTransition transition = AconFetcher.FetchTransition(ac, request); // 削除するトランジション
            srcState.RemoveTransition(transition);
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
            Debug.Log("コンディション Insert 開始 Fullpath=[" + requestSet.RepresentativeFullpath+ "] requestSet.FullpathTransition_forDebug=[" + requestSet.FullpathTransition_forDebug+ "] requestSet.FullpathCondition_forDebug=[" + requestSet.FullpathCondition_forDebug+"]");
            AnimatorStateTransition transition = AconFetcher.FetchTransition_ByFullpath(ac,
                requestSet.RepresentativeFullpath,
                requestSet.RepresentativeFullpathTransition
                ); // １つ上のオブジェクト（トランジション）
            AnimatorConditionMode mode;     if (requestSet.TryModeValue(out mode))              { Debug.Log("FIXME: Insert mode"); }
            float threshold;                if (requestSet.TryThresholdValue(out threshold))    { Debug.Log("FIXME: Insert threshold"); }
            string parameter;               if (requestSet.TryParameterValue(out parameter))    { Debug.Log("FIXME: Insert parameter"); }
            transition.AddCondition(mode, threshold, parameter);
        }
        public static void Update(AnimatorController ac, DataManipulatRecordSet requestSet, StringBuilder message)
        {
            Debug.Log("コンディション Update 開始");
            // float型引数の場合、使える演算子は Greater か less のみ。
            // int型引数の場合、使える演算子は Greater、less、Equals、NotEqual のいずれか。
            // bool型引数の場合、使える演算子は表示上は true、false だが、内部的には推測するに If、IfNot の２つだろうか？
            AnimatorStateTransition transition = AconFetcher.FetchTransition_ByFullpath(ac,
                requestSet.RepresentativeFullpath,
                requestSet.RepresentativeFullpathTransition
                );// トランジション

            // struct をラッピングして返す
            ConditionRecord.AnimatorConditionWrapper wapper = AconFetcher.FetchCondition(ac, transition, requestSet.RepresentativeFullpathCondition);
            if (null != requestSet.Mode) {ConditionRecord.Definitions[requestSet.Mode.Name].Update(wapper, requestSet.Mode, message);}
            if (null != requestSet.Threshold) {ConditionRecord.Definitions[requestSet.Threshold.Name].Update(wapper, requestSet.Threshold, message);}
            if (null != requestSet.Parameter) {ConditionRecord.Definitions[requestSet.Parameter.Name].Update(wapper, requestSet.Parameter, message);}
        }
        /// <summary>
        /// コンディションを削除します。
        /// コンディション番号の大きい物から順に削除してください。コンディション番号の小さい物から削除すると番号が繰り上がってしまうため。
        /// </summary>
        public static void Delete(AnimatorController ac, DataManipulatRecordSet requestSet, StringBuilder message)
        {
            Debug.Log("コンディション Delete 開始");
            AnimatorStateTransition transition = AconFetcher.FetchTransition_ByFullpath(ac,
                requestSet.RepresentativeFullpath,
                requestSet.RepresentativeFullpathTransition
                );// トランジション
            ConditionRecord.AnimatorConditionWrapper wapper = AconFetcher.FetchCondition(ac, transition, requestSet.RepresentativeFullpathCondition);
            transition.RemoveCondition(wapper.m_source);
        }

        #region プロパティー更新機構
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
                        default: throw new UnityException("コンディション更新：未対応のプロパティ propertyName[" + propertyName + "] conditionNum_target=["+ conditionNum_target + "]");
                    }
                }
            }
            //condition_w.mode = AnimatorConditionMode.Less; // セッターは機能していないのでは？

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
            if ("stateMachines" == request.Foreignkeycategory)
            {
                // ステートマシンのポジション
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames(request.Fullpath, ref caret, ref ft)) { throw new UnityException("[" + request.Fullpath + "]パース失敗だぜ☆（＾～＾） ac=[" + ac.name + "]"); }

                AnimatorControllerLayer layer = AconFetcher.FetchLayer_JustLayerName(ac, ft.LayerNameEndsWithoutDot);
                AnimatorStateMachine statemachine = AconFetcher.FetchStatemachine(ac, ft, layer);

                if (null == statemachine) { throw new UnityException("[" + request.Fullpath + "]ステートマシンは見つからなかったぜ☆（＾～＾） ac=[" + ac.name + "]"); }

                if (Operation_Something.HasProperty(request.Name, PositionRecord.Definitions, "ステートマシンのポジション操作"))
                {
                    PositionRecord.Definitions[request.Name].Update(new PositionRecord.PositionWrapper(statemachine, request.Propertyname_ofFullpath), request, message);
                }
            }
            else // ステートのポジション
            {
                ChildAnimatorState caState = AconFetcher.FetchChildstate(ac, request.Fullpath); // 構造体☆

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
