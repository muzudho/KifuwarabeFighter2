using DojinCircleGrayscale.StellaQL;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

/// <summary>
/// ウィンドウのタブ名にクラスのフルパスが出てくるので、このクラスは StellaQL ネームスペースに入れない。
/// 
/// 参考 : ステート名の取得 : 「※Editorのみ　アニメーターコントローラーのステート名をぜんぶ取得する(Unity5.1)」 http://shigekix5.wp.xdomain.jp/?p=1153
/// 参考 : ログ出力        : 「【Unity】文字列をファイルに書き出す【Log出力】」 http://chroske.hatenablog.com/entry/2015/06/29/175830
/// 参考 : テキスト書き込み : 「文字コードを指定してテキストファイルに書き込む」 http://dobon.net/vb/dotnet/file/writefile.html
/// </summary>
public class StateMachineQueryStellaQL : EditorWindow
{
    public StateMachineQueryStellaQL()
    {
        // テキストボックスに表示するメッセージ
        info_message = new StringBuilder();
    }

    #region properties
    string oldPath_animatorController = FileUtility_Engine.PATH_ANIMATOR_CONTROLLER_FOR_DEMO_TEST;
    AnimatorController m_ac;
    /// <summary>
    /// "\n" だけの改行は対応していないので、Environment.NewLine を使うこと
    /// </summary>
    string commandline = "# Sample" + Environment.NewLine+
        "STATE SELECT" + Environment.NewLine +
        "WHERE" + Environment.NewLine +
        @"    "".*Dog""" + Environment.NewLine +
        "THE" + Environment.NewLine +
        @"    Zoo001" + Environment.NewLine +
        @";" + Environment.NewLine +
        "STATE SELECT" + Environment.NewLine +
        "WHERE" + Environment.NewLine +
        @"    "".*Cat""" + Environment.NewLine +
        "THE" + Environment.NewLine +
        @"    Zoo002" + Environment.NewLine +
        "";
    string info_message_ofTextbox = "Konnichiwa.";
    static StringBuilder info_message;
    Vector2 scroll_commandBox;
    Vector2 scroll_infoBox;
    #endregion

    /// <summary>
    /// メニューからクリックしたとき。
    /// </summary>
    [MenuItem("Tool/Dojin Circle Grayscale/State Machine Query (StellaQL)")]
    static void Init()
    {
        // ウィンドウのインスタンスを取得して開くことだけする。
        StateMachineQueryStellaQL window = (StateMachineQueryStellaQL)EditorWindow.GetWindow(typeof(StateMachineQueryStellaQL));
        window.Show();
    }

    public const string BUTTON_LABEL_GENERATE_FULLPATH = "Generate C# (Fullpath of states)";

    void OnGUI()
    {

        if (null== m_ac)
        {
            // アニメーター・コントローラーを再取得。
            m_ac = AssetDatabase.LoadAssetAtPath<AnimatorController>(oldPath_animatorController);
        }
        bool repaint_allWindow = false;

        GUILayout.Label("Animator controller", EditorStyles.boldLabel);
        #region Drag and drop area
        var dropArea = GUILayoutUtility.GetRect(0.0f, 20.0f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Animator Controller Drag & Drop");
        string droppedPath_animatorController = "";
        // マウス位置が GUI の範囲内であれば
        if (dropArea.Contains(Event.current.mousePosition))
        {
            switch (Event.current.type)
            {
                // マウス・ホバー中
                case EventType.DragUpdated:
                    {
                        // ドラッグしているのが参照可能なオブジェクトの場合
                        if (0 < DragAndDrop.objectReferences.Length)
                        {
                            //オブジェクトを受け入れる
                            DragAndDrop.AcceptDrag();

                            // マウス・カーソルの形状を、このオブジェクトを受け入れられるという見た目にする
                            DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                        }
                    }
                    break;

                // ドロップしたら
                case EventType.DragPerform:
                    {
                        // ドラッグしているものを現在のコントロール ID と紐付ける
                        DragAndDrop.activeControlID = DragAndDrop.objectReferences[0].GetInstanceID();

                        // ドロップしているのが参照可能なオブジェクトの場合
                        if (DragAndDrop.objectReferences.Length == 1)
                        {
                            foreach (var draggedObject in DragAndDrop.objectReferences)
                            {
                                AnimatorController ac_temp = draggedObject as AnimatorController;
                                if (null != ac_temp)
                                {
                                    droppedPath_animatorController = AssetDatabase.GetAssetPath(ac_temp.GetInstanceID());
                                    Repaint();
                                    repaint_allWindow = true;
                                }
                            }
                        }
                    }
                    break;

                    //case EventType.DragExited: // ?
            }
        }
        #endregion

        bool isChanged_animatorController = false;
        #region Animator controller path
        if (""!= droppedPath_animatorController && oldPath_animatorController != droppedPath_animatorController)
        {
            // 異なるアニメーター・コントローラーのパスがドロップされた
            oldPath_animatorController = droppedPath_animatorController;
            isChanged_animatorController = true;
        }

        string newPath_animatorController;
        newPath_animatorController = EditorGUILayout.TextField(oldPath_animatorController);

        if (oldPath_animatorController != newPath_animatorController)
        {
            // 異なるアニメーター・コントローラーのパスが入力された
            oldPath_animatorController = newPath_animatorController;
            isChanged_animatorController = true;
        }
        #endregion

        if (isChanged_animatorController)
        {
            // アニメーター・コントローラーを再取得。
            m_ac = AssetDatabase.LoadAssetAtPath<AnimatorController>(oldPath_animatorController);
        }

        #region Create full path constant button
        if (GUILayout.Button(BUTTON_LABEL_GENERATE_FULLPATH))
        {
            info_message.Append("Generate fullpath Start. filename(without extension) = "); info_message.Append(m_ac.name); info_message.AppendLine();
            FullpathConstantGenerator.WriteCshapScript(m_ac, info_message);
            info_message.Append("Generate fullpath End. filename(without extension) = "); info_message.Append(m_ac.name); info_message.AppendLine();
        }
        GUILayout.Space(4.0f);
        #endregion

        bool isActivate_aconState;
        #region Acon state readability judgment
        if (!UserSettings.Instance.AnimationControllerFilepath_to_userDefinedInstance.ContainsKey(oldPath_animatorController))
        {
            int step = 1;
            if (""== oldPath_animatorController)
            {
                GUILayout.Label("Failure.", EditorStyles.boldLabel);
                GUILayout.Label(String.Concat("(", step, ") Please drag and drop"), EditorStyles.boldLabel);
                GUILayout.Label("    your animator controller", EditorStyles.boldLabel);
                GUILayout.Label("    to the box above.", EditorStyles.boldLabel);
                step++;
            }
            GUILayout.Label(String.Concat("(",step,") Please add the"), EditorStyles.boldLabel);
            GUILayout.Label("    path of your animator controller", EditorStyles.boldLabel);
            GUILayout.Label(String.Concat("    to ",FileUtility_Engine.PATH_USER_SETTINGS), EditorStyles.boldLabel);
            step++;

            UserSettings.Instance.Dump_Presentable(info_message);
            isActivate_aconState = false;
        }
        else
        {
            isActivate_aconState = true;
        }
        #endregion
        if (isActivate_aconState)
        {
            #region Query text area
            {
                GUILayout.Label("Query (StellaQL)");
                scroll_commandBox = EditorGUILayout.BeginScrollView(scroll_commandBox);
                commandline = EditorGUILayout.TextArea(commandline);//, GUILayout.Height(position.height - 30)
                EditorGUILayout.EndScrollView();
            }
            #endregion
            #region Execution button
            {
                if (GUILayout.Button("Execute"))
                {
                    info_message.Append("I pressed the Execute button."); info_message.AppendLine();
                    AControllable userDefinedStateTable = UserSettings.Instance.AnimationControllerFilepath_to_userDefinedInstance[oldPath_animatorController];
                    SequenceQuerier.Execute(m_ac, commandline, userDefinedStateTable, info_message);
                    Repaint();
                    repaint_allWindow = true;
                    info_message.Append("Execute end."); info_message.AppendLine();
                }
            }
            #endregion
            GUILayout.Space(4.0f);
            #region Export Spreadsheet button
            // 実際は CSV形式ファイルを出力する
            GUILayout.Space(4.0f);
            if (GUILayout.Button("Export spread sheet")) // Export CSV
            {
                info_message.Append("Export spread sheet Start. filename(without extension) = "); info_message.Append(m_ac.name); info_message.AppendLine();
                info_message.Append("Please, Use Libre Office Calc."); info_message.AppendLine();
                info_message.Append("And use macro application."); info_message.AppendLine();
                info_message.Append("location: "); info_message.Append(FileUtility_Editor.Filepath_StellaQLMacroApplicationOds()); info_message.AppendLine();

                AconScanner aconScanner = new AconScanner();
                aconScanner.ScanAnimatorController(m_ac, info_message);
                AconDocument aconDocument = aconScanner.AconDocument;
                bool outputDefinition = false;
                for (int i = 0; i < 2; i++)
                {
                    if (i == 1) { outputDefinition = true; }

                    // 参照 : Cannot convert HashSet to IReadOnlyCollection : http://stackoverflow.com/questions/32762631/cannot-convert-hashset-to-ireadonlycollection
                    {
                        StringBuilder contents = new StringBuilder();
                        AconDataUtility.CreateCsvTable(AconDataUtility.ToHash(aconDocument.parameters), ParameterRecord.Empty, outputDefinition, contents);
                        FileUtility_Editor.Write(FileUtility_Editor.Filepath_LogParameters(m_ac.name, outputDefinition), contents, info_message);
                    }
                    {
                        StringBuilder contents = new StringBuilder();
                        AconDataUtility.CreateCsvTable(AconDataUtility.ToHash(aconDocument.layers), LayerRecord.Empty, outputDefinition, contents);
                        FileUtility_Editor.Write(FileUtility_Editor.Filepath_LogLayer(m_ac.name, outputDefinition), contents, info_message);
                    }
                    {
                        StringBuilder contents = new StringBuilder();
                        AconDataUtility.CreateCsvTable(AconDataUtility.ToHash(aconDocument.statemachines), StatemachineRecord.Empty, outputDefinition, contents);
                        FileUtility_Editor.Write(FileUtility_Editor.Filepath_LogStatemachine(m_ac.name, outputDefinition), contents, info_message);
                    }
                    {
                        StringBuilder contents = new StringBuilder();
                        AconDataUtility.CreateCsvTable(AconDataUtility.ToHash(aconDocument.states), StateRecord.Empty, outputDefinition, contents);
                        FileUtility_Editor.Write(FileUtility_Editor.Filepath_LogStates(m_ac.name, outputDefinition), contents, info_message);
                    }
                    {
                        StringBuilder contents = new StringBuilder();
                        AconDataUtility.CreateCsvTable(AconDataUtility.ToHash(aconDocument.transitions), TransitionRecord.Empty, outputDefinition, contents);
                        FileUtility_Editor.Write(FileUtility_Editor.Filepath_LogTransition(m_ac.name, outputDefinition), contents, info_message);
                    }
                    {
                        StringBuilder contents = new StringBuilder();
                        AconDataUtility.CreateCsvTable(AconDataUtility.ToHash(aconDocument.conditions), ConditionRecord.Empty, outputDefinition, contents);
                        FileUtility_Editor.Write(FileUtility_Editor.Filepath_LogConditions(m_ac.name, outputDefinition), contents, info_message);
                    }
                    {
                        StringBuilder contents = new StringBuilder();
                        AconDataUtility.CreateCsvTable(AconDataUtility.ToHash(aconDocument.positions), PositionRecord.Empty, outputDefinition, contents);
                        FileUtility_Editor.Write(FileUtility_Editor.Filepath_LogPositions(m_ac.name, outputDefinition), contents, info_message);
                    }
                    {
                        StringBuilder contents = new StringBuilder();
                        AconDataUtility.CreateCsvTable(AconDataUtility.ToHash(aconDocument.motions), MotionRecord.Empty, outputDefinition, contents);
                        FileUtility_Editor.Write(FileUtility_Editor.Filepath_LogMotions(m_ac.name, outputDefinition), contents, info_message);
                    }
                }
            }
            #endregion
            #region Import Spreadsheet button
            // 実際はCSV形式ファイルを出力する
            if (GUILayout.Button("Import spread sheet")) // Import CSV
            {
                info_message.Append("Import spread sheet Start. filename(without extension) = "); info_message.Append(m_ac.name); info_message.AppendLine();

                // 現状のデータ
                AconScanner aconScanner = new AconScanner();
                aconScanner.ScanAnimatorController(m_ac, info_message);
                AconDocument aconData_scanned = aconScanner.AconDocument;

                HashSet<DataManipulationRecord> updateRequest;
                // CSVファイル読取
                FileUtility_Editor.ReadUpdateRequestCsv(out updateRequest, info_message);
                AnimatorControllerWrapper acWrapper = new AnimatorControllerWrapper(m_ac);
                // 更新を実行
                Operation_Something.ManipulateData(acWrapper, aconData_scanned, updateRequest, info_message);

                // 編集したレイヤーのプロパティーを反映させる。
                //Operation_Layer.RefreshAllLayers(acWrapper);

                FileUtility_Editor.DeleteUpdateRequestCsv(info_message);

                // 他のウィンドウはリフレッシュしてくれないみたいだ。
                Repaint();
                repaint_allWindow = true;
                info_message.Append("Import spread sheet End. filename(without extension) = "); info_message.Append(m_ac.name); info_message.AppendLine();
                //info_message.AppendLine("Please, Refresh Animator window.");
                //info_message.AppendLine("  case 1: (1) mouse right button click on Animator window tab.");
                //info_message.AppendLine("          (2) [Close Tab] click.");
                //info_message.AppendLine("          (3) menu [Window] - [Animator] click.");
                //info_message.AppendLine("  case 2: Click [Auto Live Link] Button on right top corner, twice(toggle).");
            }
            #endregion
        }

        #region Various Refresh
        {
            if (repaint_allWindow)
            {

                UnityEditor.EditorApplication.isPlaying = true; // Play.
                info_message.AppendLine("I'm sorry!");
                info_message.AppendLine("    I clickeded the play button!");
                info_message.AppendLine("Because, This is for");
                info_message.AppendLine("    refreshing the animator window!");
                info_message.AppendLine("Please, Push back the play button.");

                // これ全部、アニメーター・ウィンドウには効かない
                //{
                //    Repaint();
                //    EditorApplication.RepaintAnimationWindow();
                //    EditorApplication.RepaintHierarchyWindow();
                //    EditorApplication.RepaintProjectWindow();
                //    UnityEditor.HandleUtility.Repaint();
                //    UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
                //    EditorWindow animatorWindow = EditorWindow.GetWindow<EditorWindow>("Animator");
                //    animatorWindow.Repaint();
                //}
            }
        }
        #endregion

        #region Message output field
        {
            GUILayout.Label("Info");
            if (0 < info_message.Length)
            {
                // 更新
                info_message_ofTextbox = info_message.ToString();
                info_message.Length = 0;
            }

            scroll_infoBox = EditorGUILayout.BeginScrollView(scroll_infoBox);
            info_message_ofTextbox = EditorGUILayout.TextArea(info_message_ofTextbox);//, GUILayout.Height(position.height - 30)
                                                                                        // Repaint();
            EditorGUILayout.EndScrollView();
        }
        #endregion
    }
}
