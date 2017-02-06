using System.Collections.Generic;
using System.Text;

/// <summary>
/// Please, read Step 1 to 8. Assets/StellaQL/Demo_Zoo.cs
/// Step 9 here.
/// </summary>
namespace StellaQL
{
    /// <summary>
    /// ユーザー定義データベース
    /// </summary>
    public class UserDefinedDatabase : AbstractUserDefinedDatabase
    {
        static UserDefinedDatabase() { Instance = new UserDefinedDatabase(); }
        public static UserDefinedDatabase Instance { get; private set; }

        UserDefinedDatabase()
        {
            #region (Step 9.) Link A to B. A is animation controller file path. B is UserDefinedStateTable instance. (アニメーション・コントローラーと、ユーザー定義テーブルの紐付けを、ユーザー定義データベースに追加して有効化)
            AnimationControllerFilePath_to_table = new Dictionary<string, AControllable>()
            {
                {FileUtility_Engine.PATH_ANIMATOR_CONTROLLER_FOR_DEMO_TEST, StellaQL.Acons.Demo_Zoo.AControl.Instance },
                {"Assets/Resources/AnimatorControllers/Select_Cursor.controller", StellaQL.Acons.Select_Cursor.AControl.Instance},
                {"Assets/Resources/AnimatorControllers/Main_Char3.controller", StellaQL.Acons.Main_Char3.AControl.Instance },
            };
            #endregion
        }

        /// <summary>
        /// エラー表示時に利用
        /// </summary>
        public void Dump_Presentable(StringBuilder message)
        {
            message.Append("Registerd "); message.Append(AnimationControllerFilePath_to_table.Count); message.AppendLine(" paths.");
            int i = 0;
            foreach (string path in AnimationControllerFilePath_to_table.Keys)
            {
                message.Append("["); message.Append(i); message.Append("]"); message.AppendLine(path);
                i++;
            }
        }
    }
}
