using System.Collections.Generic;

/// <summary>
/// ネームスペースは StellaQL にしてください。
/// Please set the namespace to StellaQL.
/// </summary>
namespace DojinCircleGrayscale.StellaQL
{
    /// <summary>
    /// あなたのUnityプロジェクトが StellaQL に設定する内容です。
    /// It is what your Unity project sets for StellaQL.
    /// 
    /// StellaQLフォルダーを更新するたびに上書きされて消えてしまわないよう、注意して扱ってください。
    /// Please handle carefully so that it will not be overwritten and deleted every time you update the StellaQL folder.
    /// 
    /// StellaQLフォルダーの外に出しておくといいでしょう。
    /// It's a good idea to put it outside the StellaQL folder.
    /// </summary>
    public class UserSettings : AbstractUserSettings
    {
        static UserSettings()
        {
            Instance = new UserSettings();

            // 重要。
            // Important.
            // 
            // アニメーター・コントローラーのファイルパスと、自動生成したクラスを拡張したインスタンスを紐づけてください。
            // Link the file path of the animator controller to the instance that extended the automatically generated class.
            Instance.AddMappings_AnimatorControllerFilepath_And_UserDefinedInstance( new Dictionary<string, AControllable>(){

                //{ "Assets/StellaQL/AnimatorControllers/Demo_Zoo.controller", StellaQL.Acons.Demo_Zoo.AControl.Instance },
                { "Assets/Resources/AnimatorControllers/Main_Char3.controller", StellaQL.Acons.Main_Char3.AControl.Instance },
                

                // 例。
                // ex.)
                //{"Assets/New Animator Controller.controller", YourNamespace.Newanimatorcontroller.AControl.Instance },
            });
        }

        #region Singleton
        /// <summary>
        /// シングルトン・デザインパターンとして作っています。
        /// I am making this class as a singleton design pattern.
        /// </summary>
        public static UserSettings Instance { get; private set; }
        #endregion
    }
}
