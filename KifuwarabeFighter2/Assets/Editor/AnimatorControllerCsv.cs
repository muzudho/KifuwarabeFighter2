using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace SceneMain
{
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

        public void CreateCsv(StringBuilder sb, out string commasBatch0)
        {
            int commas0 = 0;

            sb.Append(layerNum); // レイヤー行番号
            sb.Append(",");
            commas0++;
            // int 

            sb.Append(name); // レイヤー名
            sb.Append(",");
            commas0++;
            // string

            sb.Append(avatarMask);
            sb.Append(",");
            commas0++;
            // 略

            sb.Append(blendingMode);
            sb.Append(",");
            commas0++;
            // 略

            sb.Append(defaultWeight);
            sb.Append(",");
            commas0++;
            // float

            sb.Append(iKPass);
            sb.Append(",");
            commas0++;
            // bool

            sb.Append(syncedLayerAffectsTiming);
            sb.Append(",");
            commas0++;
            // bool

            sb.Append(syncedLayerIndex);
            sb.Append(",");
            commas0++;
            // int

            {
                StringBuilder sbComma0 = new StringBuilder();
                for (int i = 0; i < commas0; i++)
                {
                    sbComma0.Append(",");
                }
                commasBatch0 = sbComma0.ToString();
            }
        }

        public static string ColumnNameCsv { get { return "LayerNum,LayerName,AvatarMask,BlendingMode,DefaultWeight,IKPass,SyncedLayerAffectsTiming,SyncedLayerIndex,"; } }
    }

    public class TransitionRecord
    {
        #region プロパティー
        public int layerNum;
        public int stateNum;
        public int transitionNum;
        public bool canTransitionToSelf;
        /// <summary>
        /// FIXME: 略
        /// </summary>
        public string conditions;
        public string destinationState;
        public string destinationStateMachine;
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

        public TransitionRecord(int layerNum, int stateNum, int transitionNum, AnimatorStateTransition transition)
        {
            this.layerNum = layerNum;
            this.stateNum = stateNum;
            this.transitionNum = transitionNum;
            canTransitionToSelf = transition.canTransitionToSelf;
            conditions = transition.conditions.ToString();
            destinationState = transition.destinationState == null ? "" : transition.destinationState.ToString();
            destinationStateMachine = transition.destinationStateMachine == null ? "" : transition.destinationStateMachine.ToString();
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

        public void CreateCsv(StringBuilder sb, out string commasBatch0)
        {
            int commas0 = 0;

            sb.Append(layerNum);
            sb.Append(",");
            commas0++;

            sb.Append(stateNum);
            sb.Append(",");
            commas0++;

            sb.Append(transitionNum);
            sb.Append(",");
            commas0++;

            sb.Append(canTransitionToSelf);
            sb.Append(",");
            commas0++;

            sb.Append(conditions);
            sb.Append(",");
            commas0++;

            sb.Append(destinationState);
            sb.Append(",");
            commas0++;

            sb.Append(destinationStateMachine);
            sb.Append(",");
            commas0++;

            sb.Append(duration);
            sb.Append(",");
            commas0++;

            sb.Append(exitTime);
            sb.Append(",");
            commas0++;

            sb.Append(hasExitTime);
            sb.Append(",");
            commas0++;

            sb.Append(hasFixedDuration);
            sb.Append(",");
            commas0++;

            sb.Append(hideFlags);
            sb.Append(",");
            commas0++;

            sb.Append(interruptionSource);
            sb.Append(",");
            commas0++;

            sb.Append(isExit);
            sb.Append(",");
            commas0++;

            sb.Append(mute);
            sb.Append(",");
            commas0++;

            sb.Append(name);
            sb.Append(",");
            commas0++;

            sb.Append(offset);
            sb.Append(",");
            commas0++;

            sb.Append(orderedInterruption);
            sb.Append(",");
            commas0++;

            sb.Append(solo);
            sb.Append(",");
            commas0++;

            {
                StringBuilder sbComma0 = new StringBuilder();
                for (int i = 0; i < commas0; i++)
                {
                    sbComma0.Append(",");
                }
                commasBatch0 = sbComma0.ToString();
            }
        }

        public static string ColumnNameCsv { get { return "LayerNum,StateNum,TransitionNum,CanTransitionToSelf,Conditions,DestinationState,DestinationStateMachine,Duration,ExitTime,HasExitTime,HasFixedDuration,HideFlags,InterruptionSource,IsExit,Mute,Name,Offset,OrderedInterruption,Solo,"; } }
    }

    public class StateRecord
    {
        public int layerNum;
        public int stateNum;
        public float cycleOffset;
        public string cycleOffsetParameter;
        public string hideFlags;
        public bool iKOnFeet;
        public bool mirror;
        public string mirrorParameter;
        public bool mirrorParameterActive;
        public string motion;
        public string name;
        public int nameHash;
        public float speed;
        public string speedParameter;
        public bool speedParameterActive;
        public string tag;
        public bool writeDefaultValues;

        public StateRecord(int layerNum, int stateNum, AnimatorState state)
        {
            this.layerNum = layerNum;
            this.stateNum = stateNum;
            cycleOffset = state.cycleOffset;
            cycleOffsetParameter = state.cycleOffsetParameter;
            hideFlags = state.hideFlags.ToString();
            iKOnFeet = state.iKOnFeet;
            mirror = state.mirror;
            mirrorParameter = state.mirrorParameter;
            mirrorParameterActive = state.mirrorParameterActive;
            motion = state.motion == null ? "" : state.motion.ToString();
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
            sb.Append(stateNum); sb.Append(",");
            sb.Append(name); sb.Append(",");
            sb.Append(cycleOffset); sb.Append(",");
            sb.Append(cycleOffsetParameter); sb.Append(",");
            sb.Append(hideFlags); sb.Append(",");
            sb.Append(iKOnFeet); sb.Append(",");
            sb.Append(mirror); sb.Append(",");
            sb.Append(mirrorParameter); sb.Append(",");
            sb.Append(mirrorParameterActive); sb.Append(",");
            sb.Append(motion); sb.Append(",");
            sb.Append(nameHash); sb.Append(",");
            sb.Append(speed); sb.Append(",");
            sb.Append(speedParameter); sb.Append(",");
            sb.Append(speedParameterActive); sb.Append(",");
            sb.Append(tag); sb.Append(",");
            sb.Append(writeDefaultValues); sb.Append(",");
        }

        public static string ColumnNameCsv { get { return "StateName,CycleOffset,CycleOffsetParameter,HideFlags,IKOnFeet,Mirror,MirrorParameter,MirrorParameterActive,Motion,NameHash,Speed,SpeedParameter,SpeedParameterActive,Tag,WriteDefaultValues,"; } }
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
        Vector2 scroll;
        List<LayerRecord> layersTabel = new List<LayerRecord>();
        List<StateRecord> statesTabel = new List<StateRecord>();
        List<TransitionRecord> transitionsTabel = new List<TransitionRecord>();

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
            GUILayout.Label("Animator CSV(Main Scene)", EditorStyles.boldLabel);
            pathController = EditorGUILayout.TextField(pathController);

            if (GUILayout.Button("Output CSV(Main Scene)"))
            {
                Debug.Log("Start☆（＾～＾）！");

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

                WriteStatesTableCsv(out resultMessage);
                sb.AppendLine(resultMessage);

                WriteTransitionsTableCsv(out resultMessage);
                sb.AppendLine(resultMessage);

                myText = sb.ToString();
            }

            scroll = EditorGUILayout.BeginScrollView(scroll);
            myText = EditorGUILayout.TextArea(myText, GUILayout.Height(position.height - 30));
            EditorGUILayout.EndScrollView();
        }

        void WriteLayersTableCsv(out string resultMessage)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(LayerRecord.ColumnNameCsv);
            sb.AppendLine();

            int layerNum = 0;
            foreach (LayerRecord layerRecord in layersTabel)
            {
                string commasBatch0;
                layerRecord.CreateCsv(sb, out commasBatch0);
                sb.AppendLine();
                layerNum++;
            }

            string filepath = "./_db_anicon_layers.csv";
            File.WriteAllText(filepath, sb.ToString());
            resultMessage = "Writed☆（＾▽＾） " + Path.GetFullPath(filepath);
            Debug.Log(resultMessage);
        }

        void WriteStatesTableCsv(out string resultMessage)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(StateRecord.ColumnNameCsv);
            sb.AppendLine();

            int layerNum = 0;
            foreach (StateRecord stateRecord in statesTabel)
            {
                stateRecord.CreateCsv(sb);
                sb.AppendLine();
                layerNum++;
            }

            string filepath = "./_db_anicon_states.csv";
            File.WriteAllText(filepath, sb.ToString());
            resultMessage = "Writed☆（＾▽＾） " + Path.GetFullPath(filepath);
            Debug.Log(resultMessage);
        }

        void WriteTransitionsTableCsv(out string resultMessage)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(TransitionRecord.ColumnNameCsv);
            sb.AppendLine();

            foreach (TransitionRecord transitionRecord in transitionsTabel)
            {
                string commasBatch0;
                transitionRecord.CreateCsv(sb, out commasBatch0);
                sb.AppendLine();
            }

            string filepath = "./_db_anicon_transitions.csv";
            File.WriteAllText(filepath, sb.ToString());
            resultMessage = "Writed☆（＾▽＾） " + Path.GetFullPath(filepath);
            Debug.Log(resultMessage);
        }

        void WriteParametersCsv(AnimatorController ac, out string resultMessage)
        {
            Debug.Log("Parameters Scanning...☆（＾～＾）");

            StringBuilder sb = new StringBuilder();
            // 見出し列
            sb.Append("Num,Name,Bool,Float,Int,NameHash");

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
            File.WriteAllText("./_db_anicon_parameters.csv", sb.ToString());
            resultMessage = "Writed☆（＾▽＾） " + Path.GetFullPath("./_db_anicon_parameters.csv");
            Debug.Log(resultMessage);
        }

        void ScanAnimatorController(AnimatorController ac, out string resultMessage)
        {
            Debug.Log("States Scanning...☆（＾～＾）");

            int layerNum = 0;
            foreach (AnimatorControllerLayer layer in ac.layers)
            {
                LayerRecord layerRecord = new LayerRecord(layerNum, layer);
                layersTabel.Add(layerRecord);

                ChildAnimatorState[] caStates = ac.layers[layerNum].stateMachine.states;
                int stateNum = 0;
                foreach (ChildAnimatorState caState in caStates)
                {
                    StateRecord stateRecord = new StateRecord(layerNum,stateNum, caState.state);
                    statesTabel.Add(stateRecord);

                    int transitionNum = 0;
                    foreach (AnimatorStateTransition transition in caState.state.transitions)
                    {
                        /*
                            sb.Append(caState.position.x);
                            sb.Append(",");
                            sb.Append(caState.position.y);
                            sb.Append(",");
                            sb.Append(caState.position.z);
                            sb.Append(",");
                        */

                        TransitionRecord transitionRecord = new TransitionRecord(layerNum, stateNum, transitionNum, transition);
                        transitionsTabel.Add(transitionRecord);
                        transitionNum++;
                    }//トランジション

                    stateNum++;
                }//ステート

                layerNum++;
            }//レイヤー

            resultMessage = "Scanned☆（＾▽＾）";
            Debug.Log(resultMessage);
        }
    }
}
