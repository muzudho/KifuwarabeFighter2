using StellaQL;
using System;
using System.Text;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// タブ名にクラスのフルパスが出てくるので、StellaQL ネームスペースに入れない。
/// 
/// 参考：「※Editorのみ　アニメーターコントローラーのステート名をぜんぶ取得する(Unity5.1)」 http://shigekix5.wp.xdomain.jp/?p=1153
/// 参考：「【Unity】文字列をファイルに書き出す【Log出力】」 http://chroske.hatenablog.com/entry/2015/06/29/175830
/// 参考：「文字コードを指定してテキストファイルに書き込む」 http://dobon.net/vb/dotnet/file/writefile.html
/// </summary>
public class StateCmdline : EditorWindow
{
    public StateCmdline()
    {
        info_message = new StringBuilder(); // 情報メッセージ
    }

    #region プロパティー
    string oldPath_animatorController = FileUtility_Engine.PATH_ANIMATOR_CONTROLLER_FOR_DEMO_TEST;
    AnimatorController m_ac;
    /// <summary>
    /// "\n" だけの改行は対応していないので、Environment.NewLine を使うこと。
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
    static StringBuilder info_message; // 情報メッセージ
    Vector2 scroll_commandBox;
    Vector2 scroll_infoBox;
    #endregion

    /// <summary>
    /// メニューからクリックしたとき。
    /// </summary>
    [MenuItem("Window/State Machine Command line (StellaQL)")]
    static void Init()
    {
        // ウィンドウのインスタンスを取得して開くことだけする。
        StateCmdline window = (StateCmdline)EditorWindow.GetWindow(typeof(StateCmdline));
        window.Show();
    }

    public const string BUTTON_LABEL_GENERATE_FULLPATH = "Generate C# (Fullpath of all states)";

    void OnGUI()
    {
        if (null== m_ac)
        {
            m_ac = AssetDatabase.LoadAssetAtPath<AnimatorController>(oldPath_animatorController);// アニメーター・コントローラーを再取得。
        }
        bool repaint_allWindow = false;
        bool show_reference = false;

        #region 使い方ボタン
        if (GUILayout.Button("How to use (使い方)"))
        {
            show_reference = true;
            Repaint(); // 他のウィンドウはリフレッシュしてくれないみたいだ。
            repaint_allWindow = true;
        }
        GUILayout.Space(4.0f);
        #endregion
        GUILayout.Label("Animator controller", EditorStyles.boldLabel);
        #region ドラッグ＆ドロップ エリア
        var dropArea = GUILayoutUtility.GetRect(0.0f, 20.0f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Animation Controller Drag & Drop");
        string droppedPath_animatorController = "";
        //マウス位置が GUI の範囲内であれば
        if (dropArea.Contains(Event.current.mousePosition))
        {
            switch (Event.current.type)
            {
                case EventType.DragUpdated: // マウス・ホバー中☆
                    {
                        // ドラッグしているのが参照可能なオブジェクトの場合
                        if (0 < DragAndDrop.objectReferences.Length)
                        {
                            //Debug.Log("DragAndDrop.objectReferences.Length=[" + DragAndDrop.objectReferences.Length + "]");
                            //オブジェクトを受け入れる
                            DragAndDrop.AcceptDrag();

                            // マウス・カーソルの形状を、このオブジェクトを受け入れられるという見た目にする
                            DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                        }
                    }
                    break;

                case EventType.DragPerform: // ドロップしたら☆
                    {
                        // ドラッグしているものを現在のコントロール ID と紐付ける
                        DragAndDrop.activeControlID = DragAndDrop.objectReferences[0].GetInstanceID();

                        //ドロップしているのが参照可能なオブジェクトの場合
                        if (DragAndDrop.objectReferences.Length == 1)
                        {
                            foreach (var draggedObject in DragAndDrop.objectReferences)
                            {
                                AnimatorController ac_temp = draggedObject as AnimatorController;
                                if (null != ac_temp)
                                {
                                    droppedPath_animatorController = AssetDatabase.GetAssetPath(ac_temp.GetInstanceID());
                                    Repaint(); // 他のウィンドウはリフレッシュしてくれないみたいだ。
                                    repaint_allWindow = true;
                                }
                            }
                        }
                        //else
                        //{
                        //    Debug.Log("ドロップするものがないぜ☆！");
                        //}
                    }
                    break;

                    //case EventType.DragExited: // ドロップ？？
                    //    Debug.Log("ドロップ抜け☆（＾～＾）");
                    //    break;
            }
        }
        #endregion

        bool isChanged_animatorController = false;
        #region アニメーター・コントローラー・パス
        if (""!= droppedPath_animatorController && oldPath_animatorController != droppedPath_animatorController)
        {
            Debug.Log("異なるアニメーター・コントローラーのパスがドロップされたぜ☆（＾～＾） old=[" + oldPath_animatorController + "] dropped=[" + droppedPath_animatorController + "]");
            oldPath_animatorController = droppedPath_animatorController;
            isChanged_animatorController = true;
        }

        string newPath_animatorController;
        newPath_animatorController = EditorGUILayout.TextField(oldPath_animatorController);

        if (oldPath_animatorController != newPath_animatorController)
        {
            Debug.Log("異なるアニメーター・コントローラーのパスが入力されたぜ☆（＾～＾） old=[" + oldPath_animatorController + "] new=[" + newPath_animatorController + "]");
            oldPath_animatorController = newPath_animatorController;
            isChanged_animatorController = true;
        }
        #endregion

        bool isActivate_animatorController;
        #region アニメーターコントローラー読込み可否判定
        if (!UserDefinedDatabase.Instance.AnimationControllerFilePath_to_table.ContainsKey(oldPath_animatorController))
        {
            GUILayout.Label("Failure.", EditorStyles.boldLabel);
            GUILayout.Label("Please, Animator controller", EditorStyles.boldLabel);
            GUILayout.Label(" set path to ", EditorStyles.boldLabel);
            GUILayout.Label(String.Concat("(", FileUtility_Engine.PATH_USER_DEFINED_DATABASE, ")"), EditorStyles.boldLabel);

            UserDefinedDatabase.Instance.Dump_Presentable(info_message);
            isActivate_animatorController = false;
        }
        else
        {
            isActivate_animatorController = true;
        }
        #endregion
        if (isActivate_animatorController)
        {
            if (isChanged_animatorController)
            {
                // アニメーター・コントローラーを再取得。
                m_ac = AssetDatabase.LoadAssetAtPath<AnimatorController>(oldPath_animatorController);
            }

            #region フルパス定数作成ボタン
            if (GUILayout.Button(BUTTON_LABEL_GENERATE_FULLPATH))
            {
                info_message.Append("Generate fullpath Start☆（＾～＾）！ filename(without extension) = "); info_message.Append(m_ac.name); info_message.AppendLine();
                FullpathConstantGenerator.WriteCshapScript(m_ac, info_message);
                info_message.Append("Generate fullpath End☆（＾▽＾）！ filename(without extension) = "); info_message.Append(m_ac.name); info_message.AppendLine();
            }
            GUILayout.Space(4.0f);
            #endregion
            #region コマンド・テキストエリア
            {
                GUILayout.Label("Command line (StellaQL)");
                scroll_commandBox = EditorGUILayout.BeginScrollView(scroll_commandBox);
                commandline = EditorGUILayout.TextArea(commandline);//, GUILayout.Height(position.height - 30)
                EditorGUILayout.EndScrollView();
            }
            #endregion
            #region コマンド実行ボタン
            {
                if (GUILayout.Button("Execute"))
                {
                    info_message.Append("Executeボタンを押した☆（＾～＾）！ "); info_message.AppendLine();
                    AControllable userDefinedStateTable = UserDefinedDatabase.Instance.AnimationControllerFilePath_to_table[oldPath_animatorController];
                    SequenceQuerier.Execute(m_ac, commandline, userDefinedStateTable, info_message);
                    //{
                    //    int caret = 0;
                    //    Querier.Execute(m_ac, commandline, ref caret, userDefinedStateTable, info_message);
                    //}
                    Repaint(); // 他のウィンドウはリフレッシュしてくれないみたいだ。
                    repaint_allWindow = true;
                    info_message.Append("Execute終わり☆（＾▽＾）！ "); info_message.AppendLine();
                }
            }
            #endregion
            #region スプレッドシート出力ボタン
            // 実際は CSV形式ファイルを出力する
            GUILayout.Space(4.0f);
            if (GUILayout.Button("Export spread sheet")) // Export CSV
            {
                info_message.Append("Export spread sheet Start☆（＾～＾）！ filename(without extension) = "); info_message.Append(m_ac.name); info_message.AppendLine();
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
            #region スプレッドシート入力ボタン
            // 実際はCSV形式ファイルを出力する
            if (GUILayout.Button("Import spread sheet")) // Import CSV
            {
                info_message.Append("Import spread sheet Start☆（＾～＾）！ filename(without extension) = "); info_message.Append(m_ac.name); info_message.AppendLine();

                // 現状のデータ
                AconScanner aconScanner = new AconScanner();
                aconScanner.ScanAnimatorController(m_ac, info_message);
                AconData aconData_scanned = aconScanner.AconData;

                HashSet<DataManipulationRecord> updateRequest;
                StellaQLReader.ReadUpdateRequestCsv(out updateRequest, info_message); // CSVファイル読取
                AnimatorControllerWrapper acWrapper = new AnimatorControllerWrapper(m_ac);
                Operation_Something.ManipulateData(acWrapper, aconData_scanned, updateRequest, info_message); // 更新を実行
                //Operation_Layer.RefreshAllLayers(acWrapper); // 編集したレイヤーのプロパティーを反映させる。
                StellaQLReader.DeleteUpdateRequestCsv(info_message);

                Repaint(); // 他のウィンドウはリフレッシュしてくれないみたいだ。
                repaint_allWindow = true;
                info_message.Append("Import spread sheet End☆（＾▽＾）！ filename(without extension) = "); info_message.Append(m_ac.name); info_message.AppendLine();
                //info_message.AppendLine("Please, Refresh Animator window.");
                //info_message.AppendLine("  case 1: (1) mouse right button click on Animator window tab.");
                //info_message.AppendLine("          (2) [Close Tab] click.");
                //info_message.AppendLine("          (3) menu [Window] - [Animator] click.");
                //info_message.AppendLine("  case 2: Click [Auto Live Link] Button on right top corner, twice(toggle).");
            }
            #endregion
        }

        #region 各種リフレッシュ
        {
            if (repaint_allWindow)
            {

                UnityEditor.EditorApplication.isPlaying = true; // 再生する
                info_message.AppendLine("失敬！ 再生ボタンを押し戻してくれだぜ☆（＾▽＾）");
                info_message.AppendLine("I'm sorry!");
                info_message.AppendLine("    I clickeded the play button!");
                info_message.AppendLine("Because, This is for");
                info_message.AppendLine("    refreshing the animator window!");
                info_message.AppendLine("Please, Push back the play button.");

                // これ全部、アニメーター・ウィンドウには効かない
                //{
                //    Repaint(); // 他のウィンドウはリフレッシュしてくれないみたいだ。
                //    EditorApplication.RepaintAnimationWindow();
                //    EditorApplication.RepaintHierarchyWindow();
                //    EditorApplication.RepaintProjectWindow();
                //    UnityEditor.HandleUtility.Repaint();
                //    UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
                //    EditorWindow animatorWindow = EditorWindow.GetWindow<EditorWindow>("Animator");
                //    animatorWindow.Repaint();
                //}
            }

            // 再生ボタンを押したという注意書きのあとで。
            if (show_reference)
            {
                info_message.AppendLine(); // 注意書きとの間に、1行隙間を空けておく。
                Reference.ToContents(info_message);
            }

            //if (isRefreshInspectorWindow)
            //{
            //    // リフレクションを利用して、インスペクター・ウィンドウを再描画できるだろうか？
            //    // 出典 : unity 「Type of Inspector」 http://answers.unity3d.com/questions/948806/type-of-inspector.html
            //    {
            //        var editorAsm = typeof(Editor).Assembly; // リフレクションを利用する
            //        var wndType = editorAsm.GetType("UnityEditor.InspectorWindow"); // インスペクター・ウィンドウの型
            //        var targetWindow = EditorWindow.GetWindow<StateCmdline>(wndType);
            //        targetWindow.Repaint();
            //    }
            //}
        }
        #endregion

        #region メッセージ出力欄
        {
            GUILayout.Label("Info");
            if (0 < info_message.Length)
            {
                info_message_ofTextbox = info_message.ToString(); // 更新
                info_message.Length = 0;
            }
            scroll_infoBox = EditorGUILayout.BeginScrollView(scroll_infoBox);
            info_message_ofTextbox = EditorGUILayout.TextArea(info_message_ofTextbox);//, GUILayout.Height(position.height - 30)
            EditorGUILayout.EndScrollView();
        }
        #endregion
    }
}
