using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.IO;
using System.Text;

namespace StellaQL
{
    /// <summary>
    /// 参考：「※Editorのみ　アニメーターコントローラーのステート名をぜんぶ取得する(Unity5.1)」 http://shigekix5.wp.xdomain.jp/?p=1153
    /// 参考：「【Unity】文字列をファイルに書き出す【Log出力】」 http://chroske.hatenablog.com/entry/2015/06/29/175830
    /// 参考：「文字コードを指定してテキストファイルに書き込む」 http://dobon.net/vb/dotnet/file/writefile.html
    /// </summary>
    public class StateCmdline : EditorWindow
    {
        string commandline = "now constraction";
        string infoMessage = "Hello World";
        string pathController = "Assets/Resources/AnimatorControllers/AniCon@Char3.controller";
        string filenameWE = "";
        Vector2 scroll;

        /// <summary>
        /// メニューからクリックしたとき。
        /// </summary>
        [MenuItem("Window/State Command line (StellaQL)")]
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

            GUILayout.Label("Command line (StellaQL)");
            scroll = EditorGUILayout.BeginScrollView(scroll);
            commandline = EditorGUILayout.TextArea(commandline, GUILayout.Height(position.height - 30));
            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("Execute"))
            {
                Debug.Log("Executeボタンを押した☆ myText2=" + commandline);
                Querier.Execute(commandline, typeof(StellaQLTest.StateTable.Attr), StellaQLTest.InstanceTable.index_to_exRecord, out infoMessage);
            }

            GUILayout.Space(4.0f);
            if (GUILayout.Button("Export CSV"))
            {
                filenameWE = Path.GetFileNameWithoutExtension(pathController);
                Debug.Log("Start☆（＾～＾）！ filename(without extension) = " + filenameWE);

                // アニメーター・コントローラーを取得。
                AnimatorController ac = (AnimatorController)AssetDatabase.LoadAssetAtPath<AnimatorController>(pathController);//"Assets/Resources/AnimatorControllers/AniCon@Char3.controller"

                StringBuilder sb = new StringBuilder();
                string resultMessage;

                AniconTables.WriteCsv_Parameters(ac, out resultMessage);
                sb.AppendLine(resultMessage);

                AniconTables.ScanAnimatorController(ac, out resultMessage);
                sb.AppendLine(resultMessage);

                AniconTables.WriteCsv_Layer(filenameWE, out resultMessage);
                sb.AppendLine(resultMessage);

                AniconTables.WriteCsv_Statemachine(filenameWE, out resultMessage);
                sb.AppendLine(resultMessage);

                AniconTables.WriteCsv_State(filenameWE, out resultMessage);
                sb.AppendLine(resultMessage);

                AniconTables.WriteCsv_Transition(filenameWE, out resultMessage);
                sb.AppendLine(resultMessage);

                AniconTables.WriteCsv_Condition(filenameWE, out resultMessage);
                sb.AppendLine(resultMessage);

                AniconTables.WriteCsv_Position(filenameWE, out resultMessage);
                sb.AppendLine(resultMessage);

                infoMessage = sb.ToString();
                Repaint();
            }

            GUILayout.Label("Info");
            scroll = EditorGUILayout.BeginScrollView(scroll);
            infoMessage = EditorGUILayout.TextArea(infoMessage, GUILayout.Height(position.height - 30));
            EditorGUILayout.EndScrollView();
        }

    }
}