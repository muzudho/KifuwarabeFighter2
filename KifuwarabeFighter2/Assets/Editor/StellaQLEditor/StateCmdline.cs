using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System;
using System.Text;
using SceneStellaQLTest;
using StellaQL;
using System.Reflection;

/// <summary>
/// タブ名にクラスのフルパスが出てくるので、StellaQL ネームスペースに入れない。
/// 
/// 参考：「※Editorのみ　アニメーターコントローラーのステート名をぜんぶ取得する(Unity5.1)」 http://shigekix5.wp.xdomain.jp/?p=1153
/// 参考：「【Unity】文字列をファイルに書き出す【Log出力】」 http://chroske.hatenablog.com/entry/2015/06/29/175830
/// 参考：「文字コードを指定してテキストファイルに書き込む」 http://dobon.net/vb/dotnet/file/writefile.html
/// </summary>
public class StateCmdline : EditorWindow
{
    /// <summary>
    /// "\n" だけの改行は対応していないので、Environment.NewLine を使うこと。
    /// </summary>
    string commandline = "# Sample" + Environment.NewLine+
        "STATE SELECT" + Environment.NewLine +
        "WHERE \".*Dog\"" + Environment.NewLine;
    string infoMessage = "Konnichiwa.";
    string path_animatorController = "Assets/Scripts/StellaQLEngine/Anicon@StellaQL.controller"; //"Assets/Resources/AnimatorControllers/AniCon@Char3.controller";
    Vector2 scroll;

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

        var evt = Event.current;
        var dropArea = GUILayoutUtility.GetRect(0.0f, 20.0f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Animation Controller Drag & Drop");
        path_animatorController = EditorGUILayout.TextField(path_animatorController);
        //マウス位置が GUI の範囲内であれば
        if (dropArea.Contains(evt.mousePosition))
        {
            switch (evt.type)
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
                                AnimatorController anicon = draggedObject as AnimatorController;
                                if (null!= anicon)
                                {
                                    path_animatorController = AssetDatabase.GetAssetPath(anicon.GetInstanceID());
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

        StringBuilder message = new StringBuilder();

        // アニメーター・コントローラーを取得。
        AnimatorController ac = (AnimatorController)AssetDatabase.LoadAssetAtPath<AnimatorController>(path_animatorController);//"Assets/Resources/AnimatorControllers/AniCon@Char3.controller"
        if (!UserDefinedDatabase.Instance.AnimationControllerFilePath_to_table.ContainsKey(path_animatorController))
        {
            GUILayout.Label("Animator controller No Data", EditorStyles.boldLabel);
            GUILayout.Label("(UserDefinedDatabase.cs)", EditorStyles.boldLabel);
            message = new StringBuilder();
            message.AppendLine("指定されたパスは登録されていないぜ☆（＾～＾） path_animatorController=[" + path_animatorController + "]");
            message.AppendLine("登録されている拡張テーブル " + UserDefinedDatabase.Instance.AnimationControllerFilePath_to_table.Count + " 件");
            foreach (string path in UserDefinedDatabase.Instance.AnimationControllerFilePath_to_table.Keys)
            {
                message.AppendLine("[" + path + "]");
            }
        }
        else
        {
            UserDefinedStateTableable userDefinedStateTable = UserDefinedDatabase.Instance.AnimationControllerFilePath_to_table[path_animatorController];

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

        GUILayout.Space(4.0f);
        if (GUILayout.Button("Export CSV"))
        {
            Debug.Log("Start☆（＾～＾）！ filename(without extension) = " + ac.name);

            AniconData aniconData;
            AniconDataUtility.ScanAnimatorController(ac, out aniconData, message);
            bool outputDefinition = false;
            for (int i=0; i<2; i++)
            {
                if (i == 1) { outputDefinition = true; }
                AniconDataUtility.WriteCsv_Parameters(aniconData, ac.name, outputDefinition, message);
                AniconDataUtility.WriteCsv_Layers(aniconData, ac.name, outputDefinition, message);
                AniconDataUtility.WriteCsv_Statemachines(aniconData, ac.name, outputDefinition, message);
                AniconDataUtility.WriteCsv_States(aniconData, ac.name, outputDefinition, message);
                AniconDataUtility.WriteCsv_Transitions(aniconData, ac.name, outputDefinition, message);
                AniconDataUtility.WriteCsv_Conditions(aniconData, ac.name, outputDefinition, message);
                AniconDataUtility.WriteCsv_Positions(aniconData, ac.name, outputDefinition, message);
            }

            infoMessage = message.ToString();
            Repaint();
        }

        if (GUILayout.Button("Command Reference"))
        {
            Reference.ToContents(message);
            infoMessage = message.ToString();
            Repaint();
        }

        GUILayout.Label("Info");
        scroll = EditorGUILayout.BeginScrollView(scroll);
        infoMessage = EditorGUILayout.TextArea(infoMessage, GUILayout.Height(position.height - 30));
        EditorGUILayout.EndScrollView();
    }

}
