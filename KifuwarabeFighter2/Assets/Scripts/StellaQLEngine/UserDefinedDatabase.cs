using System.Collections.Generic;

namespace StellaQL
{
    /// <summary>
    /// ユーザー定義データベース
    /// </summary>
    public abstract class UserDefinedDatabase
    {
        static UserDefinedDatabase()
        {
            #region アニメーション・コントローラーと、ユーザー定義テーブルの紐付けを、ユーザー定義データベースに追加して有効化
            AnimationControllerFilePath_to_table = new Dictionary<string, UserDefinedStateTableable>()
            {
                {"Assets/Scripts/StellaQLEngine/Anicon@StellaQL.controller", SceneStellaQLTest.UserDefinedStateTable.Instance },
                {"Assets/Resources/AnimatorControllers/AniCon@Select.controller", SceneSelect.UserDefinedStateTable.Instance},
                {"Assets/Resources/AnimatorControllers/AniCon@Char3.controller", SceneMain.UserDefinedStateTable.Instance },
            };
            #endregion
        }

        public static Dictionary<string,UserDefinedStateTableable> AnimationControllerFilePath_to_table { get; set; }

        public static UserDefinedStateTableable GetUserDefinedStateTable(string path_animationController)
        {
            return AnimationControllerFilePath_to_table[path_animationController];
        }
    }
}
