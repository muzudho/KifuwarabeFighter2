using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System;
using System.Text;
using SceneStellaQLTest;

namespace StellaQL
{
    /// <summary>
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
            // アニメーター・コントローラーを取得。
            AnimatorController ac = (AnimatorController)AssetDatabase.LoadAssetAtPath<AnimatorController>(path_animatorController);//"Assets/Resources/AnimatorControllers/AniCon@Char3.controller"



            GUILayout.Label("Command line (StellaQL)");
            scroll = EditorGUILayout.BeginScrollView(scroll);
            commandline = EditorGUILayout.TextArea(commandline, GUILayout.Height(position.height - 30));
            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("Execute"))
            {
                Debug.Log("Executeボタンを押した☆ テスト以外のStateExTableの取得方法はまだ");
                StringBuilder message;
                Querier.Execute(ac, commandline,
                    StateExTable.Instance, // FIXME: テスト以外のStateExTableの取得方法はまだ
                    out message);
                infoMessage = message.ToString();
            }

            GUILayout.Space(4.0f);
            if (GUILayout.Button("Export CSV"))
            {
                Debug.Log("Start☆（＾～＾）！ filename(without extension) = " + ac.name);

                StringBuilder message = new StringBuilder();

                AniconTables.WriteCsv_Parameters(ac, message);
                AniconTables.ScanAnimatorController(ac, message);
                AniconTables.WriteCsv_Layer(ac.name, message);
                AniconTables.WriteCsv_Statemachine(ac.name, message);
                AniconTables.WriteCsv_State(ac.name, message);
                AniconTables.WriteCsv_Transition(ac.name, message);
                AniconTables.WriteCsv_Condition(ac.name, message);
                AniconTables.WriteCsv_Position(ac.name, message);

                infoMessage = message.ToString();
                Repaint();
            }

            GUILayout.Label("Info");
            scroll = EditorGUILayout.BeginScrollView(scroll);
            infoMessage = EditorGUILayout.TextArea(infoMessage, GUILayout.Height(position.height - 30));
            EditorGUILayout.EndScrollView();
        }

    }
}