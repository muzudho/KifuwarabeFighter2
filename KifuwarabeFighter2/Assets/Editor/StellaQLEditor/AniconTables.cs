//
// Animation Controller Tables
//
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor.Animations;
using UnityEngine;

namespace StellaQL
{
    /// <summary>
    /// レイヤー
    /// </summary>
    public class LayerRecord
    {
        #region プロパティー
        /// <summary>
        /// レイヤー行番号
        /// </summary>
        public int layerNum;

        /// <summary>
        /// レイヤー名
        /// </summary>
        public string name;

        /// <summary>
        /// 略
        /// </summary>
        public string avatarMask;

        /// <summary>
        /// 略
        /// </summary>
        public string blendingMode;

        /// <summary>
        /// 
        /// </summary>
        public float defaultWeight;

        /// <summary>
        /// 
        /// </summary>
        public bool iKPass;

        /// <summary>
        /// 
        /// </summary>
        public bool syncedLayerAffectsTiming;

        /// <summary>
        /// 
        /// </summary>
        public int syncedLayerIndex;
        #endregion

        public LayerRecord(int num, AnimatorControllerLayer layer)
        {
            layerNum = num;
            name = layer.name;
            avatarMask = layer.avatarMask == null ? "" : layer.avatarMask.ToString();
            blendingMode = layer.blendingMode.ToString();
            defaultWeight = layer.defaultWeight;
            iKPass = layer.iKPass;
            syncedLayerAffectsTiming = layer.syncedLayerAffectsTiming;
            syncedLayerIndex = layer.syncedLayerIndex;
        }

        public void CreateCsv(StringBuilder sb)
        {
            sb.Append(layerNum); sb.Append(","); // レイヤー行番号
            sb.Append(name); sb.Append(","); // レイヤー名
            sb.Append(avatarMask); sb.Append(",");
            sb.Append(blendingMode); sb.Append(",");
            sb.Append(defaultWeight); sb.Append(",");
            sb.Append(iKPass); sb.Append(",");
            sb.Append(syncedLayerAffectsTiming); sb.Append(",");
            sb.Append(syncedLayerIndex); sb.Append(",");
        }

        public static string ColumnNameCsv { get { return "LayerNum,LayerName,AvatarMask,BlendingMode,DefaultWeight,IKPass,SyncedLayerAffectsTiming,SyncedLayerIndex,"; } }
    }

    /// <summary>
    /// ステートマシン
    /// </summary>
    public class StatemachineRecord
    {
        public int layerNum;
        public int machineStateNum;
        public string anyStateTransitions;
        public string behaviours;
        public string defaultState;
        public string entryTransitions;
        public string hideFlags;
        public string name;

        public StatemachineRecord(int layerNum, int machineStateNum, AnimatorStateMachine stateMachine, List<PositionRecord> positionsTable)
        {
            this.layerNum = layerNum;
            this.machineStateNum = machineStateNum;

            if (stateMachine.anyStatePosition != null)
            {
                positionsTable.Add(new PositionRecord(layerNum, machineStateNum, -1, -1, -1, "anyStatePosition", stateMachine.anyStatePosition));
            }

            anyStateTransitions = stateMachine.anyStateTransitions == null ? "" : stateMachine.anyStateTransitions.ToString();
            behaviours = stateMachine.behaviours == null ? "" : stateMachine.behaviours.ToString();
            defaultState = stateMachine.defaultState == null ? "" : stateMachine.defaultState.ToString();

            if (stateMachine.entryPosition != null)
            {
                positionsTable.Add(new PositionRecord(layerNum, machineStateNum, -1, -1, -1, "entryPosition", stateMachine.entryPosition));
                //entryPosition = stateMachine.entryPosition == null ? "" : stateMachine.entryPosition.ToString();
            }

            entryTransitions = stateMachine.entryTransitions == null ? "" : stateMachine.entryTransitions.ToString();

            if (stateMachine.exitPosition != null)
            {
                positionsTable.Add(new PositionRecord(layerNum, machineStateNum, -1, -1, -1, "exitPosition", stateMachine.exitPosition));
                //exitPosition = stateMachine.exitPosition == null ? "" : stateMachine.exitPosition.ToString();
            }

            hideFlags = stateMachine.hideFlags.ToString();
            name = stateMachine.name;

            if (stateMachine.parentStateMachinePosition != null)
            {
                positionsTable.Add(new PositionRecord(layerNum, machineStateNum, -1, -1, -1, "parentStateMachinePosition", stateMachine.parentStateMachinePosition));
                //parentStateMachinePosition = stateMachine.parentStateMachinePosition == null ? "" : stateMachine.parentStateMachinePosition.ToString();
            }
        }

        public void CreateCsv(StringBuilder sb)
        {
            sb.Append(layerNum); sb.Append(",");
            sb.Append(machineStateNum); sb.Append(",");
            sb.Append(anyStateTransitions); sb.Append(",");
            sb.Append(behaviours); sb.Append(",");
            sb.Append(defaultState); sb.Append(",");
            sb.Append(entryTransitions); sb.Append(",");
            sb.Append(hideFlags); sb.Append(",");
            sb.Append(name); sb.Append(",");
        }

        public static string ColumnNameCsv { get { return "LayerNum,MachineStateNum,AnyStateTransitions,Behaviours,DefaultState,EntryTransitions,HideFlags,Name,"; } }
    }

    /// <summary>
    /// ステート
    /// </summary>
    public class StateRecord
    {
        #region プロパティー
        public int layerNum;
        public int machineStateNum;
        public int stateNum;

        //
        public float cycleOffset;
        public string cycleOffsetParameter;
        public string hideFlags;
        public bool iKOnFeet;
        public bool mirror;
        public string mirrorParameter;
        public bool mirrorParameterActive;
        public string motion_name;
        public string name;
        public int nameHash;
        public float speed;
        public string speedParameter;
        public bool speedParameterActive;
        public string tag;
        public bool writeDefaultValues;
        #endregion

        public StateRecord(int layerNum, int machineStateNum, int stateNum, ChildAnimatorState caState, List<PositionRecord> positionsTable)
        {
            this.layerNum = layerNum;
            this.machineStateNum = machineStateNum;
            this.stateNum = stateNum;

            positionsTable.Add(new PositionRecord(layerNum, machineStateNum, stateNum, -1, -1, "position", caState.position));

            AnimatorState state = caState.state; ;
            cycleOffset = state.cycleOffset;
            cycleOffsetParameter = state.cycleOffsetParameter;
            hideFlags = state.hideFlags.ToString();
            iKOnFeet = state.iKOnFeet;
            mirror = state.mirror;
            mirrorParameter = state.mirrorParameter;
            mirrorParameterActive = state.mirrorParameterActive;
            motion_name = state.motion == null ? "" : state.motion.name; // とりあえず名前だけ☆
            name = state.name;
            nameHash = state.nameHash;
            speed = state.speed;
            speedParameter = state.speedParameter;
            speedParameterActive = state.speedParameterActive;
            tag = state.tag;
            writeDefaultValues = state.writeDefaultValues;
        }

        public void CreateCsv(StringBuilder sb)
        {
            sb.Append(layerNum); sb.Append(",");
            sb.Append(machineStateNum); sb.Append(",");
            sb.Append(stateNum); sb.Append(",");

            //
            sb.Append(cycleOffset); sb.Append(",");
            sb.Append(cycleOffsetParameter); sb.Append(",");
            sb.Append(hideFlags); sb.Append(",");
            sb.Append(iKOnFeet); sb.Append(",");
            sb.Append(mirror); sb.Append(",");
            sb.Append(mirrorParameter); sb.Append(",");
            sb.Append(mirrorParameterActive); sb.Append(",");
            sb.Append(motion_name); sb.Append(",");
            sb.Append(name); sb.Append(",");
            sb.Append(nameHash); sb.Append(",");
            sb.Append(speed); sb.Append(",");
            sb.Append(speedParameter); sb.Append(",");
            sb.Append(speedParameterActive); sb.Append(",");
            sb.Append(tag); sb.Append(",");
            sb.Append(writeDefaultValues); sb.Append(",");
        }

        public static string ColumnNameCsv { get { return "LayerNum,MachineStateNum,StateNum,CycleOffset,CycleOffsetParameter,HideFlags,IKOnFeet,Mirror,MirrorParameter,MirrorParameterActive,Motion,Name,NameHash,Speed,SpeedParameter,SpeedParameterActive,Tag,WriteDefaultValues,"; } }
    }

    /// <summary>
    /// トランジション
    /// ※コンディションは別テーブル
    /// </summary>
    public class TransitionRecord
    {
        #region プロパティー
        public int layerNum;
        public int machineStateNum;
        public int stateNum;
        public int transitionNum;
        public bool canTransitionToSelf;
        public string destinationState_name;
        public int destinationState_nameHash;
        public string destinationStateMachine_name;
        public float duration;
        public float exitTime;
        public bool hasExitTime;
        public bool hasFixedDuration;
        public string hideFlags;
        public string interruptionSource;
        public bool isExit;
        public bool mute;
        public string name;
        public float offset;
        public bool orderedInterruption;
        public bool solo;
        #endregion

        public TransitionRecord(int layerNum, int machineStateNum, int stateNum, int transitionNum, AnimatorStateTransition transition)
        {
            this.layerNum = layerNum;
            this.machineStateNum = machineStateNum;
            this.stateNum = stateNum;
            this.transitionNum = transitionNum;
            canTransitionToSelf = transition.canTransitionToSelf;
            //conditions = transition.conditions.ToString();

            // 名前のみ取得
            destinationState_name = transition.destinationState == null ? "" : transition.destinationState.name;
            destinationState_nameHash = transition.destinationState == null ? 0 : transition.destinationState.nameHash;

            // 名前のみ取得
            destinationStateMachine_name = transition.destinationStateMachine == null ? "" : transition.destinationStateMachine.name;

            duration = transition.duration;
            exitTime = transition.exitTime;
            hasExitTime = transition.hasExitTime;
            hasFixedDuration = transition.hasFixedDuration;
            hideFlags = transition.hideFlags.ToString();
            interruptionSource = transition.interruptionSource.ToString();
            isExit = transition.isExit;
            mute = transition.mute;
            name = transition.name;
            offset = transition.offset;
            orderedInterruption = transition.orderedInterruption;
            solo = transition.solo;
        }

        public void CreateCsv(StringBuilder sb)
        {
            sb.Append(layerNum); sb.Append(",");
            sb.Append(machineStateNum); sb.Append(",");
            sb.Append(stateNum); sb.Append(",");
            sb.Append(transitionNum); sb.Append(",");
            sb.Append(canTransitionToSelf); sb.Append(",");
            sb.Append(destinationState_name); sb.Append(",");
            sb.Append(destinationState_nameHash); sb.Append(",");
            sb.Append(destinationStateMachine_name); sb.Append(",");
            sb.Append(duration); sb.Append(",");
            sb.Append(exitTime); sb.Append(",");
            sb.Append(hasExitTime); sb.Append(",");
            sb.Append(hasFixedDuration); sb.Append(",");
            sb.Append(hideFlags); sb.Append(",");
            sb.Append(interruptionSource); sb.Append(",");
            sb.Append(isExit); sb.Append(",");
            sb.Append(mute); sb.Append(",");
            sb.Append(name); sb.Append(",");
            sb.Append(offset); sb.Append(",");
            sb.Append(orderedInterruption); sb.Append(",");
            sb.Append(solo); sb.Append(",");
        }

        public static string ColumnNameCsv { get { return "LayerNum,MachineStateNum,StateNum,TransitionNum,CanTransitionToSelf,DestinationState_Name,DestinationState_NameHash,DestinationStateMachine,Duration,ExitTime,HasExitTime,HasFixedDuration,HideFlags,InterruptionSource,IsExit,Mute,Name,Offset,OrderedInterruption,Solo,"; } }
        //,Conditions
    }

    /// <summary>
    /// コンディション
    /// </summary>
    public class ConditionRecord
    {
        public int layerNum;
        public int machineStateNum;
        public int stateNum;
        public int transitionNum;
        public int conditionNum;
        public string mode;
        public string parameter;
        public float threshold;

        public ConditionRecord(int layerNum, int machineStateNum, int stateNum, int transitionNum, int conditionNum, AnimatorCondition condition)
        {
            this.layerNum = layerNum;
            this.machineStateNum = machineStateNum;
            this.stateNum = stateNum;
            this.transitionNum = transitionNum;
            this.conditionNum = conditionNum;
            mode = condition.mode.ToString();
            parameter = condition.parameter;
            threshold = condition.threshold;
        }

        public void CreateCsv(StringBuilder sb)
        {
            sb.Append(layerNum); sb.Append(",");
            sb.Append(machineStateNum); sb.Append(",");
            sb.Append(stateNum); sb.Append(",");
            sb.Append(transitionNum); sb.Append(",");
            sb.Append(conditionNum); sb.Append(",");
            sb.Append(mode); sb.Append(",");
            sb.Append(parameter); sb.Append(",");
            sb.Append(threshold); sb.Append(",");
        }

        public static string ColumnNameCsv { get { return "LayerNum,MachineStateNum,StateNum,TransitionNum,ConditionNum,Mode,Parameter,Threshold,"; } }
    }

    /// <summary>
    /// ポジション
    /// </summary>
    public class PositionRecord
    {
        #region プロパティー
        public int layerNum;
        public int machineStateNum;
        public int stateNum;
        public int transitionNum;
        public int conditionNum;
        public string proertyName;

        //
        public float magnitude;
        public string normalized;
        public float normalizedX;
        public float normalizedY;
        public float normalizedZ;
        public float sqrMagnitude;
        public float x;
        public float y;
        public float z;
        #endregion

        public PositionRecord(
            int layerNum,
            int machineStateNum,
            int stateNum,
            int transitionNum,
            int conditionNum,
            string proertyName,
            Vector3 position)
        {
            this.layerNum = layerNum;
            this.machineStateNum = machineStateNum;
            this.stateNum = stateNum;
            this.transitionNum = transitionNum;
            this.conditionNum = conditionNum;
            this.proertyName = proertyName;

            magnitude = position.magnitude;
            //normalized = position.normalized == null ? "" : "(解析未対応)";
            normalized = position.normalized == null ? "" : Util_CsvParser.EscapeCell(position.normalized.ToString());
            //normalized = position.normalized == null ? "" : Util_CsvParser.CellList_to_csvLine( Util_CsvParser.CsvLine_to_cellList(position.normalized.ToString()));
            normalizedX = position.x;
            normalizedY = position.y;
            normalizedZ = position.z;
            sqrMagnitude = position.sqrMagnitude;
            x = position.x;
            y = position.y;
            z = position.z;
        }

        public void CreateCsv(StringBuilder sb)
        {
            sb.Append(layerNum); sb.Append(",");
            sb.Append(machineStateNum); sb.Append(",");
            sb.Append(stateNum); sb.Append(",");
            sb.Append(transitionNum); sb.Append(",");
            sb.Append(conditionNum); sb.Append(",");
            sb.Append(proertyName); sb.Append(",");

            sb.Append(magnitude); sb.Append(",");
            sb.Append(normalized); sb.Append(",");
            sb.Append(normalizedX); sb.Append(",");
            sb.Append(normalizedY); sb.Append(",");
            sb.Append(normalizedZ); sb.Append(",");
            sb.Append(sqrMagnitude); sb.Append(",");
            sb.Append(x); sb.Append(",");
            sb.Append(y); sb.Append(",");
            sb.Append(z); sb.Append(",");
        }

        public static string ColumnNameCsv { get { return "LayerNum,MachineStateNum,StateNum,TransitionNum,ConditionNum,PropertyName,Magnitude,Normalized,NormalizedX,NormalizedY,NormalizedZ,SqrMagnitude,X,Y,Z,"; } }
    }

    public abstract class AniconTables
    {
        public static List<LayerRecord> table_layer = new List<LayerRecord>();
        public static List<StatemachineRecord> table_statemachine = new List<StatemachineRecord>();
        public static List<StateRecord> table_state = new List<StateRecord>();
        public static List<TransitionRecord> table_transition = new List<TransitionRecord>();
        public static List<ConditionRecord> table_condition = new List<ConditionRecord>();
        public static List<PositionRecord> table_position = new List<PositionRecord>();

        public static void WriteCsv_Parameters(AnimatorController ac, out string resultMessage)
        {
            Debug.Log("Parameters Scanning...☆（＾～＾）");

            StringBuilder sb = new StringBuilder();
            // 見出し列
            sb.AppendLine("Num,Name,Bool,Float,Int,NameHash");

            AnimatorControllerParameter[] acpArray = ac.parameters;
            int num = 0;
            foreach (AnimatorControllerParameter acp in acpArray)
            {
                sb.Append(num);
                sb.Append(",");
                sb.Append(acp.name);
                sb.Append(",");
                sb.Append(acp.defaultBool);
                sb.Append(",");
                sb.Append(acp.defaultFloat);
                sb.Append(",");
                sb.Append(acp.defaultInt);
                sb.Append(",");
                sb.Append(acp.nameHash);
                sb.Append(",");

                sb.AppendLine();
                num++;
            }

            //Debug.Log(sb.ToString());
            string filepath = "./_log_anicon_parameters.csv";
            File.WriteAllText(filepath, sb.ToString());
            resultMessage = "Writed☆（＾▽＾） " + Path.GetFullPath(filepath);
            Debug.Log(resultMessage);
        }

        public static void WriteCsv_Layer(string filenameWE, out string resultMessage)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(LayerRecord.ColumnNameCsv); sb.AppendLine();

            foreach (LayerRecord layerRecord in table_layer)
            {
                layerRecord.CreateCsv(sb);
                sb.AppendLine();
            }

            string filepath = "./_log_(" + filenameWE + ")layers.csv";
            File.WriteAllText(filepath, sb.ToString());
            resultMessage = "Writed☆（＾▽＾） " + Path.GetFullPath(filepath);
            Debug.Log(resultMessage);
        }

        public static void WriteCsv_Statemachine(string filenameWE, out string resultMessage)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(StatemachineRecord.ColumnNameCsv); sb.AppendLine();

            foreach (StatemachineRecord stateMachine in table_statemachine)
            {
                stateMachine.CreateCsv(sb);
                sb.AppendLine();
            }

            string filepath = "./_log_(" + filenameWE + ")stateMachines.csv";
            File.WriteAllText(filepath, sb.ToString());
            resultMessage = "Writed☆（＾▽＾） " + Path.GetFullPath(filepath);
            Debug.Log(resultMessage);
        }

        public static void WriteCsv_State(string filenameWE, out string resultMessage)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(StateRecord.ColumnNameCsv); sb.AppendLine();

            foreach (StateRecord stateRecord in table_state)
            {
                stateRecord.CreateCsv(sb);
                sb.AppendLine();
            }

            string filepath = "./_log_(" + filenameWE + ")states.csv";
            File.WriteAllText(filepath, sb.ToString());
            resultMessage = "Writed☆（＾▽＾） " + Path.GetFullPath(filepath);
            Debug.Log(resultMessage);
        }

        public static void WriteCsv_Transition(string filenameWE, out string resultMessage)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(TransitionRecord.ColumnNameCsv); sb.AppendLine();

            foreach (TransitionRecord transitionRecord in table_transition)
            {
                transitionRecord.CreateCsv(sb);
                sb.AppendLine();
            }

            string filepath = "./_log_(" + filenameWE + ")transitions.csv";
            File.WriteAllText(filepath, sb.ToString());
            resultMessage = "Writed☆（＾▽＾） " + Path.GetFullPath(filepath);
            Debug.Log(resultMessage);
        }

        public static void WriteCsv_Condition(string filenameWE, out string resultMessage)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ConditionRecord.ColumnNameCsv); sb.AppendLine();

            foreach (ConditionRecord conditionRecord in table_condition)
            {
                conditionRecord.CreateCsv(sb);
                sb.AppendLine();
            }

            string filepath = "./_log_(" + filenameWE + ")conditions.csv";
            File.WriteAllText(filepath, sb.ToString());
            resultMessage = "Writed☆（＾▽＾） " + Path.GetFullPath(filepath);
            Debug.Log(resultMessage);
        }

        public static void WriteCsv_Position(string filenameWE, out string resultMessage)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(PositionRecord.ColumnNameCsv); sb.AppendLine();

            foreach (PositionRecord positionRecord in table_position)
            {
                positionRecord.CreateCsv(sb);
                sb.AppendLine();
            }

            string filepath = "./_log_(" + filenameWE + ")positions.csv";
            File.WriteAllText(filepath, sb.ToString());
            resultMessage = "Writed☆（＾▽＾） " + Path.GetFullPath(filepath);
            Debug.Log(resultMessage);
        }

        private static void ScanRecursive(List<AnimatorStateMachine> aStateMachineList, AnimatorStateMachine stateMachine)
        {
            aStateMachineList.Add(stateMachine);

            foreach (ChildAnimatorStateMachine caStateMachine in stateMachine.stateMachines)
            {
                ScanRecursive(aStateMachineList, caStateMachine.stateMachine);
            }
        }

        public static void ScanAnimatorController(AnimatorController ac, out string resultMessage)
        {
            Debug.Log("States Scanning...☆（＾～＾）");
            table_layer.Clear();
            table_state.Clear();
            table_transition.Clear();
            table_condition.Clear();
            table_position.Clear();

            foreach (AnimatorControllerLayer layer in ac.layers)//レイヤー
            {
                LayerRecord layerRecord = new LayerRecord(table_layer.Count, layer);
                table_layer.Add(layerRecord);

                // ステート・マシン
                List<AnimatorStateMachine> stateMachineList = new List<AnimatorStateMachine>();
                ScanRecursive(stateMachineList, layer.stateMachine);
                foreach (AnimatorStateMachine stateMachine in stateMachineList)
                {
                    StatemachineRecord stateMachineRecord = new StatemachineRecord(table_layer.Count, table_statemachine.Count, stateMachine, table_position);
                    table_statemachine.Add(stateMachineRecord);

                    foreach (ChildAnimatorState caState in stateMachine.states)
                    {
                        StateRecord stateRecord = new StateRecord(table_layer.Count, table_statemachine.Count, table_state.Count, caState, table_position);
                        table_state.Add(stateRecord);

                        foreach (AnimatorStateTransition transition in caState.state.transitions)
                        {
                            TransitionRecord transitionRecord = new TransitionRecord(table_layer.Count, table_statemachine.Count, table_state.Count, table_transition.Count, transition);
                            table_transition.Add(transitionRecord);

                            foreach (AnimatorCondition aniCondition in transition.conditions)
                            {
                                ConditionRecord conditionRecord = new ConditionRecord(table_layer.Count, table_statemachine.Count, table_state.Count, table_transition.Count, table_condition.Count, aniCondition);
                                table_condition.Add(conditionRecord);
                            } // コンディション
                        }//トランジション
                    }//ステート
                }

            }//レイヤー

            resultMessage = "Scanned☆（＾▽＾）";
            Debug.Log(resultMessage);
        }

    }
}
