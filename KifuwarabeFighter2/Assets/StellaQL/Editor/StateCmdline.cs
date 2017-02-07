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
    #region プロパティー
    /// <summary>
    /// "\n" だけの改行は対応していないので、Environment.NewLine を使うこと。
    /// </summary>
    string commandline = "# Sample" + Environment.NewLine+
        "STATE SELECT" + Environment.NewLine +
        "WHERE \".*Dog\"" + Environment.NewLine;
    string infoMessage = "Konnichiwa.";
    string path_animatorController = FileUtility_Engine.PATH_ANIMATOR_CONTROLLER_FOR_DEMO_TEST;
    Vector2 scroll;
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

    void OnGUI()
    {
        GUILayout.Label("Animator controller", EditorStyles.boldLabel);
        #region ドラッグ＆ドロップ エリア
        var dropArea = GUILayoutUtility.GetRect(0.0f, 20.0f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Animation Controller Drag & Drop");
        path_animatorController = EditorGUILayout.TextField(path_animatorController);
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
                                if (null!= ac_temp)
                                {
                                    path_animatorController = AssetDatabase.GetAssetPath(ac_temp.GetInstanceID());
                                }
                            }
                            HandleUtility.Repaint();
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

        StringBuilder message = new StringBuilder(); // 出力メッセージ
        // アニメーター・コントローラーを取得。
        AnimatorController ac = AssetDatabase.LoadAssetAtPath<AnimatorController>(path_animatorController);

        bool isActivate;
        #region アニメーターコントローラー読込み可否判定
        if (!UserDefinedDatabase.Instance.AnimationControllerFilePath_to_table.ContainsKey(path_animatorController))
        {
            message = new StringBuilder();

            string row;
            row = "Failure.";                       GUILayout.Label(row, EditorStyles.boldLabel); message.Append(row);
            row = "Please, Animator controller";    GUILayout.Label(row, EditorStyles.boldLabel); message.Append(row);
            row = " set path to ";                  GUILayout.Label(row, EditorStyles.boldLabel); message.Append(row);
            row = String.Concat("(", FileUtility_Engine.PATH_USER_DEFINED_DATABASE, ")"); GUILayout.Label(row, EditorStyles.boldLabel); message.Append(row);
            message.AppendLine();

            UserDefinedDatabase.Instance.Dump_Presentable(message);
            infoMessage = message.ToString();
            isActivate = false;
        }
        else
        {
            isActivate = true;
        }
        #endregion
        if (isActivate)
        {
            #region フルパス定数作成ボタン
            if (GUILayout.Button("Generate fullpath constant C#"))
            {
                message.AppendLine("Create State Const Start☆（＾～＾）！ filename(without extension) = " + ac.name);
                FullpathConstantGenerator.WriteCshapScript(ac, message);

                infoMessage = message.ToString();
                Debug.Log(infoMessage);
                Repaint();
            }
            GUILayout.Space(4.0f);
            #endregion
            #region テキストエリアとコマンド実行ボタン
            {
                AControllable userDefinedStateTable = UserDefinedDatabase.Instance.AnimationControllerFilePath_to_table[path_animatorController];

                GUILayout.Label("Command line (StellaQL)");
                scroll = EditorGUILayout.BeginScrollView(scroll);
                commandline = EditorGUILayout.TextArea(commandline, GUILayout.Height(position.height - 30));
                EditorGUILayout.EndScrollView();

                if (GUILayout.Button("Execute"))
                {
                    Debug.Log("Executeボタンを押した☆");
                    Querier.Execute(ac, commandline, userDefinedStateTable, message);
                    Repaint();
                    //HandleUtility.Repaint();
                    infoMessage = message.ToString();
                }
            }
            #endregion
            #region コマンド リファレンス ボタン
            if (GUILayout.Button("Command Reference"))
            {
                Reference.ToContents(message);
                infoMessage = message.ToString();
                Repaint();
            }
            #endregion
            #region スプレッドシート出力ボタン
            // 実際は CSV形式ファイルを出力する
            GUILayout.Space(4.0f);
            if (GUILayout.Button("Export spread sheet")) // Export CSV
            {
                Debug.Log("Export spread sheet Start☆（＾～＾）！ filename(without extension) = " + ac.name);
                message.AppendLine("Please, Use Libre Office Calc.");
                message.Append("And use macro application.");
                message.Append("location: "); message.AppendLine(StellaQLWriter.Filepath_StellaQLMacroApplicationOds());

                AconScanner aconScanner = new AconScanner();
                aconScanner.ScanAnimatorController(ac, message);
                AconData aconData = aconScanner.AconData;
                bool outputDefinition = false;
                for (int i = 0; i < 2; i++)
                {
                    if (i == 1) { outputDefinition = true; }
                    AconDataUtility.WriteCsv_Parameters(aconData, ac.name, outputDefinition, message);
                    AconDataUtility.WriteCsv_Layers(aconData, ac.name, outputDefinition, message);
                    AconDataUtility.WriteCsv_Statemachines(aconData, ac.name, outputDefinition, message);
                    AconDataUtility.WriteCsv_States(aconData, ac.name, outputDefinition, message);
                    AconDataUtility.WriteCsv_Transitions(aconData, ac.name, outputDefinition, message);
                    AconDataUtility.WriteCsv_Conditions(aconData, ac.name, outputDefinition, message);
                    AconDataUtility.WriteCsv_Positions(aconData, ac.name, outputDefinition, message);
                }

                infoMessage = message.ToString();
                Repaint();
            }
            #endregion
            #region スプレッドシート入力ボタン
            // 実際はCSV形式ファイルを出力する
            if (GUILayout.Button("Import spread sheet")) // Import CSV
            {
                message.AppendLine("Import spread sheet Start☆（＾～＾）！ filename(without extension) = " + ac.name);

                // 現状のデータ
                AconScanner aconScanner = new AconScanner();
                aconScanner.ScanAnimatorController(ac, message);
                AconData aconData = aconScanner.AconData;

                HashSet<DataManipulationRecord> updateRequest;
                StellaQLReader.ReadUpdateRequestCsv(out updateRequest, message); // CSVファイル読取
                Operation_Something.ManipulateData(ac, aconData, updateRequest, message); // 更新を実行
                StellaQLReader.DeleteUpdateRequestCsv(message);

                infoMessage = message.ToString();
                Debug.Log(infoMessage);
                Repaint(); // 他のウィンドウはリフレッシュしてくれないみたいだ。

                //// リフレクションを利用して、インスペクター・ウィンドウを再描画できるだろうか？
                //// 出典 : unity 「Type of Inspector」 http://answers.unity3d.com/questions/948806/type-of-inspector.html
                //{
                //    var editorAsm = typeof(Editor).Assembly; // リフレクションを利用する
                //    var inspWndType = editorAsm.GetType("UnityEditor.InspectorWindow"); // インスペクター・ウィンドウの型
                //    var window = EditorWindow.GetWindow<StateCmdline>(inspWndType);
                //    window.Repaint();
                //}
            }
            #endregion
        }

        #region メッセージ出力欄
        GUILayout.Label("Info");
        scroll = EditorGUILayout.BeginScrollView(scroll);
        infoMessage = EditorGUILayout.TextArea(infoMessage, GUILayout.Height(position.height - 30));
        EditorGUILayout.EndScrollView();
        #endregion
    }

}
