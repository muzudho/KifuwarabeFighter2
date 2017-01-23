using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.IO;
using System.Text;
using System.Collections.Generic;

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

public class MachineStateRecord
{
    public int layerNum;
    public int machineStateNum;
    public string anyStateTransitions;
    public string behaviours;
    public string defaultState;
    public string entryTransitions;
    public string hideFlags;
    public string name;

    public MachineStateRecord(int layerNum, int machineStateNum, AnimatorStateMachine stateMachine, List<PositionRecord> positionsTable)
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
/// 使用方法：どのコンディションを持っているかは、Condition テーブルを見ること。
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

/// <summary>
/// 参考：「※Editorのみ　アニメーターコントローラーのステート名をぜんぶ取得する(Unity5.1)」 http://shigekix5.wp.xdomain.jp/?p=1153
/// 参考：「【Unity】文字列をファイルに書き出す【Log出力】」 http://chroske.hatenablog.com/entry/2015/06/29/175830
/// 参考：「文字コードを指定してテキストファイルに書き込む」 http://dobon.net/vb/dotnet/file/writefile.html
/// </summary>
public class AnimatorControllerCsv : EditorWindow
{
    string myText = "Hello World";
    string pathController = "Assets/Resources/AnimatorControllers/AniCon@Char3.controller";
    string filenameWE = "";
    Vector2 scroll;
    List<LayerRecord> layersTabel = new List<LayerRecord>();
    List<MachineStateRecord> stateMachinesTabel = new List<MachineStateRecord>();
    List<StateRecord> statesTabel = new List<StateRecord>();
    List<TransitionRecord> transitionsTabel = new List<TransitionRecord>();
    List<ConditionRecord> conditionsTabel = new List<ConditionRecord>();
    List<PositionRecord> positionsTabel = new List<PositionRecord>();

    /// <summary>
    /// メニューからクリックしたとき。
    /// </summary>
    [MenuItem("Window/(^_^)Animator controller CSV")]
    static void Init()
    {
        // ウィンドウのインスタンスを取得して開くことだけする。
        AnimatorControllerCsv window = (AnimatorControllerCsv)EditorWindow.GetWindow(typeof(AnimatorControllerCsv));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Animator controller  CSV", EditorStyles.boldLabel);
        pathController = EditorGUILayout.TextField(pathController);

        if (GUILayout.Button("Export controller CSV"))
        {
            filenameWE = Path.GetFileNameWithoutExtension(pathController);
            Debug.Log("Start☆（＾～＾）！ filename(without extension) = " + filenameWE);

            // アニメーター・コントローラーを取得。
            AnimatorController ac = (AnimatorController)AssetDatabase.LoadAssetAtPath<AnimatorController>(pathController);//"Assets/Resources/AnimatorControllers/AniCon@Char3.controller"

            StringBuilder sb = new StringBuilder();
            string resultMessage;

            WriteParametersCsv(ac, out resultMessage);
            sb.AppendLine(resultMessage);

            ScanAnimatorController(ac, out resultMessage);
            sb.AppendLine(resultMessage);

            WriteLayersTableCsv(out resultMessage);
            sb.AppendLine(resultMessage);

            WriteMachineStateTableCsv(out resultMessage);
            sb.AppendLine(resultMessage);

            WriteStatesTableCsv(out resultMessage);
            sb.AppendLine(resultMessage);

            WriteTransitionsTableCsv(out resultMessage);
            sb.AppendLine(resultMessage);

            WriteConditionsTableCsv(out resultMessage);
            sb.AppendLine(resultMessage);

            WritePositionsTableCsv(out resultMessage);
            sb.AppendLine(resultMessage);

            myText = sb.ToString();
            Repaint();
        }

        scroll = EditorGUILayout.BeginScrollView(scroll);
        myText = EditorGUILayout.TextArea(myText, GUILayout.Height(position.height - 30));
        EditorGUILayout.EndScrollView();
    }

    void WriteParametersCsv(AnimatorController ac, out string resultMessage)
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

    void WriteLayersTableCsv(out string resultMessage)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(LayerRecord.ColumnNameCsv); sb.AppendLine();

        foreach (LayerRecord layerRecord in layersTabel)
        {
            layerRecord.CreateCsv(sb);
            sb.AppendLine();
        }

        string filepath = "./_log_(" + filenameWE + ")layers.csv";
        File.WriteAllText(filepath, sb.ToString());
        resultMessage = "Writed☆（＾▽＾） " + Path.GetFullPath(filepath);
        Debug.Log(resultMessage);
    }

    void WriteMachineStateTableCsv(out string resultMessage)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(MachineStateRecord.ColumnNameCsv); sb.AppendLine();

        foreach (MachineStateRecord stateMachine in stateMachinesTabel)
        {
            stateMachine.CreateCsv(sb);
            sb.AppendLine();
        }

        string filepath = "./_log_(" + filenameWE + ")stateMachines.csv";
        File.WriteAllText(filepath, sb.ToString());
        resultMessage = "Writed☆（＾▽＾） " + Path.GetFullPath(filepath);
        Debug.Log(resultMessage);
    }

    void WriteStatesTableCsv(out string resultMessage)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(StateRecord.ColumnNameCsv); sb.AppendLine();

        foreach (StateRecord stateRecord in statesTabel)
        {
            stateRecord.CreateCsv(sb);
            sb.AppendLine();
        }

        string filepath = "./_log_("+ filenameWE + ")states.csv";
        File.WriteAllText(filepath, sb.ToString());
        resultMessage = "Writed☆（＾▽＾） " + Path.GetFullPath(filepath);
        Debug.Log(resultMessage);
    }

    void WriteTransitionsTableCsv(out string resultMessage)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(TransitionRecord.ColumnNameCsv); sb.AppendLine();

        foreach (TransitionRecord transitionRecord in transitionsTabel)
        {
            transitionRecord.CreateCsv(sb);
            sb.AppendLine();
        }

        string filepath = "./_log_(" + filenameWE + ")transitions.csv";
        File.WriteAllText(filepath, sb.ToString());
        resultMessage = "Writed☆（＾▽＾） " + Path.GetFullPath(filepath);
        Debug.Log(resultMessage);
    }

    void WriteConditionsTableCsv(out string resultMessage)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(ConditionRecord.ColumnNameCsv); sb.AppendLine();

        foreach (ConditionRecord conditionRecord in conditionsTabel)
        {
            conditionRecord.CreateCsv(sb);
            sb.AppendLine();
        }

        string filepath = "./_log_(" + filenameWE + ")conditions.csv";
        File.WriteAllText(filepath, sb.ToString());
        resultMessage = "Writed☆（＾▽＾） " + Path.GetFullPath(filepath);
        Debug.Log(resultMessage);
    }

    void WritePositionsTableCsv(out string resultMessage)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(PositionRecord.ColumnNameCsv); sb.AppendLine();

        foreach (PositionRecord positionRecord in positionsTabel)
        {
            positionRecord.CreateCsv(sb);
            sb.AppendLine();
        }

        string filepath = "./_log_(" + filenameWE + ")positions.csv";
        File.WriteAllText(filepath, sb.ToString());
        resultMessage = "Writed☆（＾▽＾） " + Path.GetFullPath(filepath);
        Debug.Log(resultMessage);
    }

    void ScanRecursive(List<AnimatorStateMachine> aStateMachineList, AnimatorStateMachine stateMachine)
    {
        aStateMachineList.Add(stateMachine);

        foreach (ChildAnimatorStateMachine caStateMachine in stateMachine.stateMachines)
        {
            ScanRecursive(aStateMachineList, caStateMachine.stateMachine);
        }
    }

    void ScanAnimatorController(AnimatorController ac, out string resultMessage)
    {
        Debug.Log("States Scanning...☆（＾～＾）");
        layersTabel.Clear();
        statesTabel.Clear();
        transitionsTabel.Clear();
        conditionsTabel.Clear();
        positionsTabel.Clear();

        foreach (AnimatorControllerLayer layer in ac.layers)//レイヤー
        {
            LayerRecord layerRecord = new LayerRecord(layersTabel.Count, layer);
            layersTabel.Add(layerRecord);

            // ステート・マシン
            List<AnimatorStateMachine> stateMachineList = new List<AnimatorStateMachine>();
            ScanRecursive(stateMachineList, layer.stateMachine);
            foreach (AnimatorStateMachine stateMachine in stateMachineList)
            {
                MachineStateRecord stateMachineRecord = new MachineStateRecord(layersTabel.Count, stateMachinesTabel.Count, stateMachine, positionsTabel);
                stateMachinesTabel.Add(stateMachineRecord);

                foreach (ChildAnimatorState caState in stateMachine.states)
                {
                    StateRecord stateRecord = new StateRecord(layersTabel.Count, stateMachinesTabel.Count, statesTabel.Count, caState, positionsTabel);
                    statesTabel.Add(stateRecord);

                    foreach (AnimatorStateTransition transition in caState.state.transitions)
                    {
                        TransitionRecord transitionRecord = new TransitionRecord(layersTabel.Count, stateMachinesTabel.Count, statesTabel.Count, transitionsTabel.Count, transition);
                        transitionsTabel.Add(transitionRecord);

                        foreach (AnimatorCondition aniCondition in transition.conditions)
                        {
                            ConditionRecord conditionRecord = new ConditionRecord(layersTabel.Count, stateMachinesTabel.Count, statesTabel.Count, transitionsTabel.Count, conditionsTabel.Count, aniCondition);
                            conditionsTabel.Add(conditionRecord);
                        } // コンディション
                    }//トランジション
                }//ステート
            }

        }//レイヤー

        resultMessage = "Scanned☆（＾▽＾）";
        Debug.Log(resultMessage);
    }
}
