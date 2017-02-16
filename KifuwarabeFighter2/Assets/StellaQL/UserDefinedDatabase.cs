using System.Collections.Generic;

namespace StellaQL
{
    /// <summary>
    /// 重要。
    /// Important.
    /// 
    /// アニメーター・コントローラーと、自動生成したクラスを紐づけてください。
    /// Link animator controller to generated classes.
    /// </summary>
    public class UserDefinedDatabase : AbstractUserDefinedDatabase
    {
        UserDefinedDatabase()
        {
            AnimationControllerFilePath_to_table = new Dictionary<string, AControllable>()
            {
                //
                // ここに紐付けを並べてください。
                // List here! Animator controller file path to instance.
                //
                {"Assets/StellaQL/AnimatorControllers/Demo_Zoo.controller", StellaQL.Acons.Demo_Zoo.AControl.Instance },
                {"Assets/Resources/AnimatorControllers/Main_Char3.controller", StellaQL.Acons.Main_Char3.AControl.Instance },
                // 例。
                // ex.)
                //{"Assets/Your animator controllers/MainScene_Bird.controller", YourNamespace.MainScene_Bird.AControl.Instance },
                //{"Assets/Your animator controllers/Fish.controller", YourNamespace.Fish.AControl.Instance },
                //{"Assets/Your animator controllers/Tiger.controller", YourNamespace.Tiger.AControl.Instance },
            };
        }

        #region Singleton
        /// <summary>
        /// シングルトン・デザインパターンとして作っています。
        /// I am making this class as a singleton design pattern.
        /// </summary>
        static UserDefinedDatabase() { Instance = new UserDefinedDatabase(); }
        public static UserDefinedDatabase Instance { get; private set; }
        #endregion
    }
}
