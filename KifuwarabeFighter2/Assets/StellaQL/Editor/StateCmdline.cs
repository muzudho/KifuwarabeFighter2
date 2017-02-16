using StellaQL;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

/// <summary>
/// Since the full path of the class appears in the tab of the window, this class can not be put in the namespace.
/// 
/// I referred it.
/// Get state names : http://shigekix5.wp.xdomain.jp/?p=1153
/// Write log :  http://chroske.hatenablog.com/entry/2015/06/29/175830
/// Write text : http://dobon.net/vb/dotnet/file/writefile.html
/// </summary>
public class StateCmdline : EditorWindow
{
    public StateCmdline()
    {
        info_message = new StringBuilder(); // message.
    }

    #region properties
    string oldPath_animatorController = FileUtility_Engine.PATH_ANIMATOR_CONTROLLER_FOR_DEMO_TEST;
    AnimatorController m_ac;
    /// <summary>
    /// Since only "\ n" newlines are not supported, use Environment.NewLine.
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
    /// When clicking from the menu.
    /// </summary>
    [MenuItem("Window/State Machine Command line (StellaQL)")]
    static void Init()
    {
        // Just get an instance of the window and open it.
        StateCmdline window = (StateCmdline)EditorWindow.GetWindow(typeof(StateCmdline));
        window.Show();
    }

    public const string BUTTON_LABEL_GENERATE_FULLPATH = "Generate C# (Fullpath of all states)";

    void OnGUI()
    {

        if (null== m_ac)
        {
            m_ac = AssetDatabase.LoadAssetAtPath<AnimatorController>(oldPath_animatorController);// Re-acquire animator controller.
        }
        bool repaint_allWindow = false;
        bool show_tutorial = false;
        bool show_commandReference = false;

        #region Tutorial button
        if (GUILayout.Button("Tutorial (チュートリアル)"))
        {
            show_tutorial = true;
            Repaint(); // It seems that other windows do not refresh.
            repaint_allWindow = true;
        }
        GUILayout.Space(4.0f);
        #endregion
        GUILayout.Label("Animator controller", EditorStyles.boldLabel);
        #region Drag and drop area
        var dropArea = GUILayoutUtility.GetRect(0.0f, 20.0f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Animator Controller Drag & Drop");
        string droppedPath_animatorController = "";
        // If the mouse position is within the GUI range
        if (dropArea.Contains(Event.current.mousePosition))
        {
            switch (Event.current.type)
            {
                case EventType.DragUpdated: // Mouse hover
                    {
                        // If the object being dragged is a referenceable object
                        if (0 < DragAndDrop.objectReferences.Length)
                        {
                            // Accept objects
                            DragAndDrop.AcceptDrag();

                            // Make the shape of the mouse cursor look like it can accept this object
                            DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                        }
                    }
                    break;

                case EventType.DragPerform: // After dropping
                    {
                        // Link what you are dragging with the current control ID
                        DragAndDrop.activeControlID = DragAndDrop.objectReferences[0].GetInstanceID();

                        // If the object being dropped is a referenceable object
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
            // Different animator controller path was dropped
            oldPath_animatorController = droppedPath_animatorController;
            isChanged_animatorController = true;
        }

        string newPath_animatorController;
        newPath_animatorController = EditorGUILayout.TextField(oldPath_animatorController);

        if (oldPath_animatorController != newPath_animatorController)
        {
            // Different animator controller path was entered
            oldPath_animatorController = newPath_animatorController;
            isChanged_animatorController = true;
        }
        #endregion

        if (isChanged_animatorController)
        {
            // Re-fetch animator controller.
            m_ac = AssetDatabase.LoadAssetAtPath<AnimatorController>(oldPath_animatorController);
        }

        #region Create full path constant button
        if (GUILayout.Button(BUTTON_LABEL_GENERATE_FULLPATH))
        {
            Debug.Log("ジェネレーターボタンを押したぜ☆（＾～＾）");
            info_message.Append("Generate fullpath Start. filename(without extension) = "); info_message.Append(m_ac.name); info_message.AppendLine();
            FullpathConstantGenerator.WriteCshapScript(m_ac, info_message);
            info_message.Append("Generate fullpath End. filename(without extension) = "); info_message.Append(m_ac.name); info_message.AppendLine();
            Debug.Log("ファイルを生成したぜ☆（＾～＾） info=[" + info_message.ToString() + "]");
        }
        GUILayout.Space(4.0f);
        #endregion

        bool isActivate_aconState;
        #region Acon state readability judgment
        if (!UserDefinedDatabase.Instance.AnimationControllerFilePath_to_table.ContainsKey(oldPath_animatorController))
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
            GUILayout.Label(String.Concat("    to ",FileUtility_Engine.PATH_USER_DEFINED_DATABASE), EditorStyles.boldLabel);
            step++;

            UserDefinedDatabase.Instance.Dump_Presentable(info_message);
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
                    AControllable userDefinedStateTable = UserDefinedDatabase.Instance.AnimationControllerFilePath_to_table[oldPath_animatorController];
                    SequenceQuerier.Execute(m_ac, commandline, userDefinedStateTable, info_message);
                    Repaint();
                    repaint_allWindow = true;
                    info_message.Append("Execute end."); info_message.AppendLine();
                }
            }
            #endregion
            #region Command reference button
            if (GUILayout.Button("Command reference (コマンド一覧)"))
            {
                show_commandReference = true;
                Repaint();
                repaint_allWindow = true;
            }
            GUILayout.Space(4.0f);
            #endregion
            #region Spreadsheet output button
            // Actually output CSV format file
            GUILayout.Space(4.0f);
            if (GUILayout.Button("Export spread sheet")) // Export CSV
            {
                info_message.Append("Export spread sheet Start. filename(without extension) = "); info_message.Append(m_ac.name); info_message.AppendLine();
                info_message.Append("Please, Use Libre Office Calc."); info_message.AppendLine();
                info_message.Append("And use macro application."); info_message.AppendLine();
                info_message.Append("location: "); info_message.Append(StellaQLWriter.Filepath_StellaQLMacroApplicationOds()); info_message.AppendLine();

                AconScanner aconScanner = new AconScanner();
                aconScanner.ScanAnimatorController(m_ac, info_message);
                AconData aconData = aconScanner.AconData;
                bool outputDefinition = false;
                for (int i = 0; i < 2; i++)
                {
                    if (i == 1) { outputDefinition = true; }
                    AconDataUtility.WriteCsv_Parameters(aconData, m_ac.name, outputDefinition, info_message);
                    AconDataUtility.WriteCsv_Layers(aconData, m_ac.name, outputDefinition, info_message);
                    AconDataUtility.WriteCsv_Statemachines(aconData, m_ac.name, outputDefinition, info_message);
                    AconDataUtility.WriteCsv_States(aconData, m_ac.name, outputDefinition, info_message);
                    AconDataUtility.WriteCsv_Transitions(aconData, m_ac.name, outputDefinition, info_message);
                    AconDataUtility.WriteCsv_Conditions(aconData, m_ac.name, outputDefinition, info_message);
                    AconDataUtility.WriteCsv_Positions(aconData, m_ac.name, outputDefinition, info_message);
                }
            }
            #endregion
            #region Spreadsheet input button
            // Actually output CSV format file
            if (GUILayout.Button("Import spread sheet")) // Import CSV
            {
                info_message.Append("Import spread sheet Start. filename(without extension) = "); info_message.Append(m_ac.name); info_message.AppendLine();

                // Current data
                AconScanner aconScanner = new AconScanner();
                aconScanner.ScanAnimatorController(m_ac, info_message);
                AconData aconData_scanned = aconScanner.AconData;

                HashSet<DataManipulationRecord> updateRequest;
                StellaQLReader.ReadUpdateRequestCsv(out updateRequest, info_message); // CSV file reading
                AnimatorControllerWrapper acWrapper = new AnimatorControllerWrapper(m_ac);
                Operation_Something.ManipulateData(acWrapper, aconData_scanned, updateRequest, info_message); // Perform update
                //Operation_Layer.RefreshAllLayers(acWrapper); // Reflect the properties of the edited layer.
                StellaQLReader.DeleteUpdateRequestCsv(info_message);

                Repaint(); // It seems that other windows do not refresh.
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

                // Not all of this works for animator windows
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

            // After the notice that the play button was pressed.
            if (show_tutorial)
            {
                info_message.AppendLine(); // Leave one line between the note and the note.
                Tutorial.ToContents(info_message);
            }
            if (show_commandReference)
            {
                info_message.AppendLine(); // Leave one line between the note and the note.
                Reference.ToContents(info_message);
            }

        }
        #endregion

        #region Message output field
        {
            GUILayout.Label("Info");
            if (0 < info_message.Length)
            {
                info_message_ofTextbox = info_message.ToString(); // update
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
