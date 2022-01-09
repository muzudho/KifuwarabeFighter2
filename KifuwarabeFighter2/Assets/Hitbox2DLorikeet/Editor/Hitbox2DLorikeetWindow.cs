namespace DojinCircleGrayscale.Hitbox2DLorikeet
{
    using UnityEditor;
    using UnityEditor.Animations;
    using UnityEngine;
    using DojinCircleGrayscale.StellaQL;
    using System.Text;
    using DojinCircleGrayscale.Hitbox2DLorikeetMaker;

    public class Hitbox2DLorikeetWindow : EditorWindow
    {
        public Hitbox2DLorikeetWindow()
        {
            // テキストボックスに表示するメッセージ
            info_message = new StringBuilder();
        }

        #region properties
        string oldPath_animatorController = FileUtility_Engine.PATH_ANIMATOR_CONTROLLER_FOR_DEMO_TEST;
        AnimatorController m_ac;
        string info_message_ofTextbox = "Konnichiwa.";
        static StringBuilder info_message;
        Vector2 scroll_infoBox;
        #endregion

        /// <summary>
        /// Unityのメインメニューからクリックしたとき。
        /// </summary>
        [MenuItem("Tool/Dojin Circle Grayscale/Hitbox 2D Lorikeet")]
        static void Init()
        {
            // ウィンドウのインスタンスを取得して開くことだけする。
            Hitbox2DLorikeetWindow window = (Hitbox2DLorikeetWindow)GetWindow(typeof(Hitbox2DLorikeetWindow));
            window.Show();
        }

        public const string BUTTON_LABEL_GENERATE_MOTOR = "Generate C# (Motor)";

        void OnGUI()
        {

            if (null == m_ac)
            {
                // アニメーター・コントローラーを再取得。
                m_ac = AssetDatabase.LoadAssetAtPath<AnimatorController>(oldPath_animatorController);
            }

            MakeDragAndDropArea(out bool isRepaintAllWindow, out string droppedPathAnimatorController);
            MakeAnimatorControllerPath(droppedPathAnimatorController);
            MakeGenerateCSharpMotorButton();
            MakeGenerateCSharpDataButton();
            RefreshMessage(isRepaintAllWindow);
            MakeInfoField();
        }

        /// <summary>
        /// ┌─────────────────────┐
        /// │Animator controller 　　　　　　　　　　　│ … (1) Label
        /// │　　　　　　　　　　　　　　　　　　　　　│
        /// │　┌─────────────────┐　│
        /// │　│Animator Controller Drag & Drop 　│　│ … (2) Drop area
        /// │　└─────────────────┘　│
        /// │　　　　　　　　　　　　　　　　　　　　　│
        /// └─────────────────────┘
        /// </summary>
        /// <param name="outIsRepaintAllWindow"></param>
        /// <param name="outDroppedPathAnimatorController"></param>
        void MakeDragAndDropArea(out bool outIsRepaintAllWindow, out string outDroppedPathAnimatorController)
        {
            outIsRepaintAllWindow = false;

            // (1)
            GUILayout.Label("Animator controller", EditorStyles.boldLabel);

            // (2)
            var dropArea = GUILayoutUtility.GetRect(0.0f, 20.0f, GUILayout.ExpandWidth(true));
            GUI.Box(dropArea, "Animator Controller Drag & Drop");

            outDroppedPathAnimatorController = "";
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
                                        outDroppedPathAnimatorController = AssetDatabase.GetAssetPath(ac_temp.GetInstanceID());
                                        Repaint();
                                        outIsRepaintAllWindow = true;
                                    }
                                }
                            }
                        }
                        break;

                        //case EventType.DragExited: // ?
                }
            }
        }

        void MakeAnimatorControllerPath(string droppedPath_animatorController)
        {
            bool isChanged_animatorController = false;
            if ("" != droppedPath_animatorController && oldPath_animatorController != droppedPath_animatorController)
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

            if (isChanged_animatorController)
            {
                // アニメーター・コントローラーを再取得。
                m_ac = AssetDatabase.LoadAssetAtPath<AnimatorController>(oldPath_animatorController);
            }
        }

        /// <summary>
        /// モーションクラスを生成するボタン
        /// ┌───────────────┐
        /// │　　　　　　　　　　　　　　　│
        /// │　┌───────────┐　│
        /// │　│Generate C# (Motor) 　│　│ … Button
        /// │　└───────────┘　│
        /// │　　　　　　　　　　　　　　　│
        /// └───────────────┘
        /// </summary>
        void MakeGenerateCSharpMotorButton()
        {
            if (GUILayout.Button("Generate C# (Motor)"))
            {
                MotorClassFile.WriteCshapScript(m_ac, info_message);
            }
            GUILayout.Space(4.0f);
        }

        /// <summary>
        /// ┌───────────────┐
        /// │　　　　　　　　　　　　　　　│
        /// │　┌───────────┐　│
        /// │　│Generate C# (Data)　　│　│ … Button
        /// │　└───────────┘　│
        /// │　　　　　　　　　　　　　　　│
        /// └───────────────┘
        /// </summary>
        void MakeGenerateCSharpDataButton()
        {
            if (GUILayout.Button("Generate C# (Data)"))
            {
                DataClassFile.Generate(info_message);
            }
            GUILayout.Space(4.0f);
        }

        void RefreshMessage(bool repaint_allWindow)
        {
            if (repaint_allWindow)
            {
                EditorApplication.isPlaying = true; // Play.
                info_message.AppendLine("I'm sorry!");
                info_message.AppendLine("    I clickeded the play button!");
                info_message.AppendLine("Because, This is for");
                info_message.AppendLine("    refreshing the animator window!");
                info_message.AppendLine("Please, Push back the play button.");
            }
        }

        /// <summary>
        /// Info フィールド
        /// ┌───────────────┐
        /// │　　　　　　　　　　　　　　　│
        /// │　┌───────────┐　│
        /// │　│　　　　　　　　　　　│　│
        /// │　│　　　　　　　　　　　│　│ … Textarea
        /// │　│　　　　　　　　　　　│　│
        /// │　└───────────┘　│
        /// │　　　　　　　　　　　　　　　│
        /// └───────────────┘
        /// </summary>
        void MakeInfoField()
        {
            GUILayout.Label("Info");
            if (0 < info_message.Length)
            {
                // 更新
                info_message_ofTextbox = info_message.ToString();
                info_message.Length = 0;
            }

            scroll_infoBox = EditorGUILayout.BeginScrollView(scroll_infoBox);
            info_message_ofTextbox = EditorGUILayout.TextArea(info_message_ofTextbox);
            EditorGUILayout.EndScrollView();
        }
    }
}

