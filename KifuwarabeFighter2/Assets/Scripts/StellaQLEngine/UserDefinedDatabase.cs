using System.Collections.Generic;

/// <summary>
/// Please, read Step 1 to 8. Assets/Scripts/StellaQLEngine/Test_UserDefinedStateTable.cs
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
            AnimationControllerFilePath_to_table = new Dictionary<string, UserDefinedStateTableable>()
            {
                {"Assets/Scripts/StellaQLEngine/Anicon@StellaQL.controller", SceneStellaQLTest.UserDefinedStateTable.Instance },
                {"Assets/Resources/AnimatorControllers/AniCon@Select.controller", SceneSelect.UserDefinedStateTable.Instance},
                {"Assets/Resources/AnimatorControllers/AniCon@Char3.controller", SceneMain.UserDefinedStateTable.Instance },
            };
            #endregion
        }
    }
}
