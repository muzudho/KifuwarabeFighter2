using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor.Animations;
using UnityEngine;

namespace StellaQL
{
    public abstract class Operation_Something
    {
        public static void ManipulateData(AnimatorControllerWrapper acWrapper, AconData aconData_old, HashSet<DataManipulationRecord> request_packets, StringBuilder info_message)
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

            // Sort the update request by destination. Nest collections, sort update requests with full path.
            // Layer.
            List<DataManipulationRecord> list_layer = new List<DataManipulationRecord>();
            // Transition. [State full path, transition number, request list] There are cases where more than one command comes in one transition.
            Dictionary<string, Dictionary<int, List<DataManipulationRecord>>> wrap3Dic_transition = new Dictionary<string, Dictionary<int, List<DataManipulationRecord>>>();
            // Condition. [State full path, transition number, condition number] One condition becomes one update request in 1 to 3 fields.
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
                            if (wrap3Dic_transition.ContainsKey(request_packet.Fullpath)) { wrap2Dic = wrap3Dic_transition[request_packet.Fullpath]; } // Exists.
                            else { wrap2Dic = new Dictionary<int, List<DataManipulationRecord>>(); wrap3Dic_transition[request_packet.Fullpath] = wrap2Dic; } // New.

                            int tNum = int.Parse(request_packet.TransitionNum_ofFullpath);
                            if (!wrap2Dic.ContainsKey(tNum))
                            {
                                wrap2Dic.Add(tNum, new List<DataManipulationRecord>());
                            }
                            wrap2Dic[tNum].Add(request_packet);
                        }
                        break;
                    case "conditions":
                        {
                            Operation_Condition.DataManipulatRecordSet request_buffer; // Put the conditions. mode, parameter, threshold.

                            Dictionary<int, Dictionary<int, Operation_Condition.DataManipulatRecordSet>> middleDic;
                            if (largeDic_condition.ContainsKey(request_packet.Fullpath)) { middleDic = largeDic_condition[request_packet.Fullpath]; } // Exists.
                            else { middleDic = new Dictionary<int, Dictionary<int, Operation_Condition.DataManipulatRecordSet>>(); largeDic_condition[request_packet.Fullpath] = middleDic; } // New.
                            
                            Dictionary<int, Operation_Condition.DataManipulatRecordSet> smallDic;
                            int tNum = int.Parse(request_packet.TransitionNum_ofFullpath);
                            if (middleDic.ContainsKey(tNum)) { smallDic = middleDic[tNum]; } // Exists.
                            else { smallDic = new Dictionary<int, Operation_Condition.DataManipulatRecordSet>(); middleDic[tNum] = smallDic; } // New.
                            
                            int cNum = int.Parse(request_packet.ConditionNum_ofFullpath);
                            if (smallDic.ContainsKey(cNum)) { request_buffer = smallDic[cNum]; } // Exists.
                            else { request_buffer = new Operation_Condition.DataManipulatRecordSet(tNum, cNum); smallDic[cNum] = request_buffer; } // New.

                            switch (request_packet.Name) // Combine instructions that were divided into multiple lines into one set
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
                // Insert, change connection destination, update, delete want to sort.
                List<DataManipulationRecord> insertsNew_Set = new List<DataManipulationRecord>();
                List<DataManipulationRecord> insertsChangeDestination_Set = new List<DataManipulationRecord>();
                List<DataManipulationRecord> deletesSet = new List<DataManipulationRecord>();
                List<DataManipulationRecord> updatesSet = new List<DataManipulationRecord>();
                foreach (KeyValuePair<string, Dictionary<int, List<DataManipulationRecord>>> wrap3_request in wrap3Dic_transition)
                {
                    foreach (KeyValuePair<int, List<DataManipulationRecord>> wrap2_request in wrap3_request.Value)
                    {
                        if (0<wrap2_request.Value.Count) // There are multiple instructions on the same transition.
                        {
                            AnimatorStateTransition transition = AconFetcher.FetchTransition(acWrapper.SourceAc, wrap2_request.Value[0]);// Just look at the top element is enough.

                            foreach (DataManipulationRecord request in wrap2_request.Value)
                            {
                                if ("#DestinationFullpath#" == request.Name) // A data manipulation instruction to the transition destination of the transition.
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

                foreach (DataManipulationRecord request in insertsNew_Set) { Operation_Transition.Insert_New(acWrapper.SourceAc, request, info_message); } // New.
                foreach (DataManipulationRecord request in insertsChangeDestination_Set) { Operation_Transition.Insert_ChangeDestination(acWrapper.SourceAc, request, info_message); } // Change destination.
                deletesSet.Sort((DataManipulationRecord a, DataManipulationRecord b) =>
                { // Make the deletion request reverse sequential number.
                    int stringCompareOrder = string.CompareOrdinal(a.Fullpath, b.Fullpath);
                    if (0 != stringCompareOrder) { return stringCompareOrder; } // The order of state names remains the same.
                    else if (int.Parse(a.TransitionNum_ofFullpath) < int.Parse(b.TransitionNum_ofFullpath)) { return -1; } // The order of the transition is from behind.
                    else if (int.Parse(b.TransitionNum_ofFullpath) < int.Parse(a.TransitionNum_ofFullpath)) { return 1; }
                    return 0;
                });
                foreach (DataManipulationRecord request in deletesSet) { Operation_Transition.Delete(acWrapper.SourceAc, request, info_message); }// Delete.
                foreach (DataManipulationRecord request in updatesSet) { Operation_Transition.Update(acWrapper.SourceAc, request, info_message); }// Update.
            }
            #endregion
            #region Execute condition.
            {
                // I want to sort it into insert, update, delete.
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
                                AnimatorStateTransition transition = AconFetcher.FetchTransition_ByFullpath(acWrapper.SourceAc,
                                    conditionRecordSet2Pair.Value.RepresentativeFullpath,
                                    conditionRecordSet2Pair.Value.RepresentativeFullpathTransition
                                    );// Transition.
                                ConditionRecord.AnimatorConditionWrapper wapper = AconFetcher.FetchCondition(
                                    acWrapper.SourceAc,
                                    transition,
                                    conditionRecordSet2Pair.Value.RepresentativeFullpathCondition
                                    );// Condition.

                                if (wapper.IsNull) { insertsSet.Add(conditionRecordSet2Pair.Value); }// If it is a condition number that does not exist, it is allocated to insert.
                                else if (null != conditionRecordSet2Pair.Value.Parameter && conditionRecordSet2Pair.Value.Parameter.IsClear) { deletesSet.Add(conditionRecordSet2Pair.Value); }// In the case of a deletion request, it is allocated to deletion.
                                else { updatesSet.Add(conditionRecordSet2Pair.Value); }// In other cases, it is allocated to update.
                            }
                        }
                    }
                }

                foreach (Operation_Condition.DataManipulatRecordSet requestSet in insertsSet) {
                    Debug.Log("Condition Insert");
                    Operation_Condition.Insert(acWrapper.SourceAc, requestSet, info_message);
                }// Insert.
                deletesSet.Sort((Operation_Condition.DataManipulatRecordSet a, Operation_Condition.DataManipulatRecordSet b) =>
                { // Make the deletion request reverse sequential number.
                    int stringCompareOrder = string.CompareOrdinal(a.RepresentativeFullpath, b.RepresentativeFullpath);
                    if (0 != stringCompareOrder) { return stringCompareOrder; } // The order of state names remains the same.
                    else if (a.RepresentativeFullpathTransition < b.RepresentativeFullpathTransition) { return -1; } // The order of the transition is from behind.
                    else if (b.RepresentativeFullpathTransition < a.RepresentativeFullpathTransition) { return 1; }
                    else if (a.RepresentativeFullpathCondition < b.RepresentativeFullpathCondition) { return -1; } // Delete the condition from the back.
                    else if (b.RepresentativeFullpathCondition < a.RepresentativeFullpathCondition) { return 1; }
                    return 0;
                });
                foreach (Operation_Condition.DataManipulatRecordSet requestSet in deletesSet) {
                    Debug.Log("Condition Delete");
                    Operation_Condition.Delete(acWrapper.SourceAc, requestSet, info_message);
                }// Delete.
                foreach (Operation_Condition.DataManipulatRecordSet requestSet in updatesSet) {
                    Debug.Log("Condition Update");
                    Operation_Condition.Update(acWrapper.SourceAc, requestSet, info_message);
                }// Update.
            }
            #endregion
            // Execute layer. (When reflecting the layer, the process of destroying the entire object is entered and the reference link expires, so the last thing to do)
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
        /// UPDATE
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
        /// Specifying a layer name (not a regular expression) returns the index of the layer array.
        /// FIXME: If the layer name contains a dot (.), StellaQL may not work properly in various places.
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
        /// Delete all the original layers. However, since you can not reduce the number of layers to less than 1, leave one dummy at the end.
        /// Since there is a reference relationship, it will erase from the child relation layer.
        /// </summary>
        /// <param name="acWrapper"></param>
        public static void DeleteAllLayers_AndPutDammy(AnimatorControllerWrapper acWrapper)
        {
            acWrapper.SourceAc.AddLayer(new AnimatorControllerLayer()); // Add a dummy to the end.
            const int dammy = 1;

            while (dammy < acWrapper.SourceAc.layers.Length) // Except dummy, erase all
            {
                // True if it was referred from somewhere.
                bool[] parentFlags = new bool[acWrapper.SourceAc.layers.Length];
                foreach (AnimatorControllerLayer layer in acWrapper.SourceAc.layers)
                {
                    if (-1<layer.syncedLayerIndex)
                    {
                        parentFlags[layer.syncedLayerIndex] = true;
                    }
                }

                // Delete layers that are not referred to from anywhere from behind.
                for (int lNum = acWrapper.SourceAc.layers.Length - 1 - dammy; -1 < lNum; lNum--) // Excluding the trailing dummy
                {
                    if (parentFlags[lNum])
                    {
                        acWrapper.SourceAc.RemoveLayer(lNum);
                    }
                }
            }
        }

        ///// <summary>
        ///// Evacuate all layers, delete them entirely from the animation controller, and re-add all the layers again.
        ///// It is considered in the part where the link is pasted by the layer index. In order not to change the order, delete all, re-add all.
        ///// 
        ///// You can not delete all layers. (You need to keep at least one layer)
        ///// </summary>
        ///// <param name="i"></param>
        //public static void RefreshAllLayers(AnimatorControllerWrapper acWrapper)
        //{
        //    //DumpLog(acWrapper);

        //    foreach (AnimatorControllerLayer copiedLayer in acWrapper.CopiedLayers)
        //    {
        //        acWrapper.SourceAc.AddLayer(copiedLayer); // Re-add all! In this case the property settings should be reflected in Unity.
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
        /// Add a layer to the animator controller.
        /// </summary>
        public static void AddAll(AnimatorController ac, List<string> layernameWords, StringBuilder message)
        {
            foreach (string layerName in layernameWords)
            {
                ac.AddLayer(layerName);
            }
        }

        /// <summary>
        /// Delete the layer from the animator controller.
        /// </summary>
        public static void RemoveAll(AnimatorController ac, List<string> layernameWords, StringBuilder message)
        {
            for (int lNum = ac.layers.Length - 1; -1 < lNum; lNum--) // Deletes from the back so that the order does not go down.
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
    /// Statemachine.
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
                    ft.StatemachinePath // If "Base Layer.Alpaca.Bear.Cat.Dog", It is "Alpaca.Bear.Cat".
                    ), request, message);
            }
        }
    }

    /// <summary>
    /// Statemachine's Any state.
    /// </summary>
    public abstract class Operation_StatemachineAnystate
    {
        /// <summary>
        /// STATEMACHINE ANYSTATE INSERT
        /// Transition the two states. More than one state can be specified and all connections are made in the direction of src -> dst direction.
        /// </summary>
        /// <param name="path_src">ex. "Base Layer.JMove.JMove0"</param>
        public static void AddAll(AnimatorController ac, HashSet<AnimatorStateMachine> statemachines_dst, HashSet<AnimatorState> states_dst, StringBuilder message)
        {
            foreach (AnimatorStateMachine statemachine_src in statemachines_dst)
            {
                foreach (AnimatorState state_dst in states_dst)
                {
                    //message.Append("Insert: Any State");
                    //message.Append(" -> ");
                    //message.Append(state_dst.name);
                    statemachine_src.AddAnyStateTransition(state_dst);
                }
            }
        }
    }

    /// <summary>
    /// State.
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
        /// Add state to state machine.
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
        /// Delete the state from the state machine.
        /// </summary>
        public static void RemoveAll(AnimatorController ac, HashSet<AnimatorStateMachine> statemachines, List<string> statenameWords, StringBuilder message)
        {
            foreach (AnimatorStateMachine statemachine in statemachines)
            {
                foreach (string stateName in statenameWords)
                {
                    Regex regex = new Regex(stateName);

                    for (int caNum = statemachine.states.Length - 1; -1 < caNum; caNum--) // Deletes from the back so that the order does not go down
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
            foreach (AnimatorState state in states) // For all specified states
            {
                //message.Append("Update: "); message.AppendLine(state.name);
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
            foreach (AnimatorState state in states) // For all specified states
            {
                if (null == state) { throw new UnityException("state is null. states.Count=[" + states.Count + "]"); }
                recordSet.Add(new StateRecord(0,0,0, state));
            }
            message.Append("result: "); message.Append(recordSet.Count); message.AppendLine(" records.");
        }
    }

    /// <summary>
    /// Transition.
    /// </summary>
    public abstract class Operation_Transition
    {
        /// <summary>
        /// INSERT new
        /// </summary>
        public static void Insert_New(AnimatorController ac, DataManipulationRecord request, StringBuilder message)
        {
            AnimatorState srcState; // Source state
            {
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(request.Fullpath, ref caret, ref ft)) { throw new UnityException("Parse failure. [" + request.Fullpath + "] ac=[" + ac.name + "]"); }
                srcState = AconFetcher.FetchState(ac, ft);
            }

            AnimatorState dstState; // The state of the transition destination
            {
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(request.New, ref caret, ref ft)) { throw new UnityException("Parse failure. [" + request.New + "] ac=[" + ac.name + "]"); }
                dstState = AconFetcher.FetchState(ac, ft);
            }
            srcState.AddTransition(dstState);
        }
        /// <summary>
        /// Change of transition destination.
        /// </summary>
        public static void Insert_ChangeDestination(AnimatorController ac, DataManipulationRecord request, StringBuilder message)
        {
            AnimatorState srcState; // Source state.
            {
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(request.Fullpath, ref caret, ref ft)) { throw new UnityException("Parse failure. [" + request.Fullpath + "] ac=[" + ac.name + "]"); }
                srcState = AconFetcher.FetchState(ac, ft);
            }

            AnimatorState dstOldState; // The state of the old transition destination.
            {
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(request.Old, ref caret, ref ft)) { throw new UnityException("Parse failure. [" + request.Old + "] ac=[" + ac.name + "]"); }
                dstOldState = AconFetcher.FetchState(ac, ft);
            }

            AnimatorState dstNewState; // State of new transition destination.
            {
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(request.New, ref caret, ref ft)) { throw new UnityException("Parse failure. [" + request.New + "] ac=[" + ac.name + "]"); }
                dstNewState = AconFetcher.FetchState(ac, ft);
            }

            List<AnimatorStateTransition> removeList = new List<AnimatorStateTransition>();
            // FIXME: Is it really a transition number also specified?
            List<AnimatorStateTransition> oldTransitions = AconFetcher.FetchTransition_SrcDst(ac, request.Fullpath, request.Old); // Retrieve all transitions to delete.
            foreach (AnimatorStateTransition oldTransition in oldTransitions)
            {
                srcState.AddTransition(dstNewState);
                // Copy contents other than the transition destination state.
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
            AnimatorState srcState; // Source state
            {
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(request.Fullpath, ref caret, ref ft)) { throw new UnityException("Parse failure. [" + request.Fullpath + "] ac=[" + ac.name + "]"); }
                srcState = AconFetcher.FetchState(ac, ft);
            }
            if (null == srcState) { throw new UnityException("Not found state. [" + request.Fullpath + "] ac=[" + ac.name + "]"); }

            int transitionNum = int.Parse(request.TransitionNum_ofFullpath); // Transition number

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
        /// Delete the transition.
        /// Please delete in order from the one with the larger transition number. Because deletion from a thing with a small transition number causes the number to go up.
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

            AnimatorStateTransition transition = AconFetcher.FetchTransition(ac, request); // Transition to delete
            srcState.RemoveTransition(transition);
        }

        /// <summary>
        /// Transition from state machine [Any State] to state.
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
        /// Transition from State Machine [Entry] to State.
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
        /// Connect state by transition to [Exit].
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
        /// Transition the two states. More than one state can be specified and all connections are made in the direction of src -> dst direction.
        /// </summary>
        /// <param name="path_src">ex. "Base Layer.JMove.JMove0"</param>
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
        /// Delete the transition between the two states. More than one state can be specified, and it is totally deleted by brute force in src → dst direction.
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
            foreach (AnimatorState state_src in states_src) // For all specified states
            {
                foreach (AnimatorStateTransition transition in state_src.transitions)
                {
                    foreach (AnimatorState state_dst in states_dst) // For all specified states
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
            StringBuilder stellaQLComment = new StringBuilder(); // Explanation supplemented so that it is easy for human being to read for sheets to be output by SELECT statement
            foreach (AnimatorState state_src in states_src) // For all specified states
            {
                foreach (AnimatorStateTransition transition in state_src.transitions)
                {
                    foreach (AnimatorState state_dst in states_dst) // For all specified states
                    {
                        if (state_dst == transition.destinationState)
                        {
                            stellaQLComment.Append("Select: "); stellaQLComment.Append(state_src.name); stellaQLComment.Append(" -> "); stellaQLComment.Append(state_dst.name); // Since it is set to CSV, no line feed is entered.
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
    /// Condition.
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
            /// It is recorded as information for tracking errors.
            /// </summary>
            public int FullpathTransition_forDebug { get; set; }
            /// <summary>
            /// It is recorded as information for tracking errors.
            /// </summary>
            public int FullpathCondition_forDebug { get; set; }
            public DataManipulationRecord Mode { get; set; }
            public DataManipulationRecord Threshold { get; set; }
            public DataManipulationRecord Parameter { get; set; }

            /// <summary>
            /// Since there is a possibility that only one update request is made, pull out the set record.
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
            AnimatorStateTransition transition = AconFetcher.FetchTransition_ByFullpath(ac,
                requestSet.RepresentativeFullpath,
                requestSet.RepresentativeFullpathTransition
                ); // One object (transition)
            AnimatorConditionMode mode;     if (requestSet.TryModeValue(out mode))              { Debug.Log("FIXME: Insert mode"); }
            float threshold;                if (requestSet.TryThresholdValue(out threshold))    { Debug.Log("FIXME: Insert threshold"); }
            string parameter;               if (requestSet.TryParameterValue(out parameter))    { Debug.Log("FIXME: Insert parameter"); }
            transition.AddCondition(mode, threshold, parameter);
        }
        public static void Update(AnimatorController ac, DataManipulatRecordSet requestSet, StringBuilder message)
        {
            // For float type arguments, only operators that can be used are Greater or Less.
            // For int arguments, the operators that can be used are Greater, Less, Equals, NotEqual.
            // In the case of bool type argument, operators that can be used are displayed as true and false, but internally, are there two If and IfNot?
            AnimatorStateTransition transition = AconFetcher.FetchTransition_ByFullpath(ac,
                requestSet.RepresentativeFullpath,
                requestSet.RepresentativeFullpathTransition
                );

            // Wrap a struct and return it
            ConditionRecord.AnimatorConditionWrapper wapper = AconFetcher.FetchCondition(ac, transition, requestSet.RepresentativeFullpathCondition);
            if (null != requestSet.Mode) {ConditionRecord.Definitions[requestSet.Mode.Name].Update(wapper, requestSet.Mode, message);}
            if (null != requestSet.Threshold) {ConditionRecord.Definitions[requestSet.Threshold.Name].Update(wapper, requestSet.Threshold, message);}
            if (null != requestSet.Parameter) {ConditionRecord.Definitions[requestSet.Parameter.Name].Update(wapper, requestSet.Parameter, message);}
        }
        /// <summary>
        /// Delete the condition.
        /// Please remove items with the highest condition number in descending order. Because deletion from a thing with a small condition number leads up the number.
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
            // Create a new copy of all the conditions.
            List<AconConditionBuilder> cs_new = new List<AconConditionBuilder>();
            foreach (AnimatorCondition c_old in transition.conditions)
            {
                cs_new.Add(new AconConditionBuilder(c_old));
            }

            // Erase all existing conditions.
            for (int cNum = transition.conditions.Length - 1; 0 < transition.conditions.Length; cNum--)
            {
                transition.RemoveCondition(transition.conditions[cNum]);
            }

            // Update the newly created condition.
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
            //condition_w.mode = AnimatorConditionMode.Less; // Is not the setter functioning?

            // Re-add newly created condition
            for (int cNum_w = 0; cNum_w < cs_new.Count; cNum_w++)
            {
                AconConditionBuilder c_new = cs_new[cNum_w];
                transition.AddCondition(c_new.mode, c_new.threshold, c_new.parameter);
            }
        }
        #endregion
    }

    /// <summary>
    /// Position operation
    /// </summary>
    public abstract class Operation_Position
    {

        public static void Update(AnimatorController ac, DataManipulationRecord request, StringBuilder message)
        {
            if (Operation_Something.HasProperty(request.Name, PositionRecord.Definitions, "Position operation"))
            {
                if ("stateMachines" == request.Foreignkeycategory)
                {
                    // Position of state machine
                    int caret = 0;
                    FullpathTokens ft = new FullpathTokens();
                    if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames(request.Fullpath, ref caret, ref ft)) { throw new UnityException("Parse failure. [" + request.Fullpath + "] ac=[" + ac.name + "]"); }

                    PositionRecord.Definitions[request.Name].Update(AconFetcher.FetchPosition(ac, ft.LayerNameEndsWithoutDot, ft.StatemachineNamesEndsWithoutDot, request.Propertyname_ofFullpath), request, message);
                }
                else // State position
                {

                    if ("states" == request.Foreignkeycategory)
                    {
                        PositionRecord.Definitions[request.Name].Update(AconFetcher.FetchPosition_OfState(ac, request.Fullpath, request.Propertyname_ofFullpath), request, message);
                    }
                    else
                    {
                        // State machine, position other than state
                        throw new UnityException("Positions other than state machines and states are not supported. fullpath=[" + request.Fullpath + "] ac=[" + ac.name + "]");
                    }
                }
            }
        }
    }

    public abstract class Operation_AnimatorLayerBlendingMode
    {
        /// <summary>
        /// I referred. enumration. http://dobon.net/vb/dotnet/programing/enumgetvalues.html
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
        /// The first one of the nodes is a layer number
        /// </summary>
        public const int ROOT_NODE_IS_LAYER = 1;
        /// <summary>
        /// The last one of the nodes is a state name
        /// </summary>
        public const int LEAF_NODE_IS_STATE = -1;
    }
}
