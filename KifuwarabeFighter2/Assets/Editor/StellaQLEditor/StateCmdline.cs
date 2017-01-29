using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System;
using System.Text;
using SceneMain;

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
        string pathController = "Assets/Resources/AnimatorControllers/AniCon@Char3.controller";
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
            pathController = EditorGUILayout.TextField(pathController);
            // アニメーター・コントローラーを取得。
            AnimatorController ac = (AnimatorController)AssetDatabase.LoadAssetAtPath<AnimatorController>(pathController);//"Assets/Resources/AnimatorControllers/AniCon@Char3.controller"

            GUILayout.Label("Command line (StellaQL)");
            scroll = EditorGUILayout.BeginScrollView(scroll);
            commandline = EditorGUILayout.TextArea(commandline, GUILayout.Height(position.height - 30));
            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("Execute"))
            {
                Debug.Log("Executeボタンを押した☆ myText2=" + commandline);
                StringBuilder message;
                Querier.Execute(ac, commandline, typeof(StateExTable_Test.Attr_Test), StateExTable_Test.Instance.hash_to_exRecord, out message);
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