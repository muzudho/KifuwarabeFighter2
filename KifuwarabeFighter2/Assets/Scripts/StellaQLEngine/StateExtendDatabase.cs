using System.Collections.Generic;
using UnityEngine;

namespace StellaQL
{
    public abstract class StateExtendDatabase
    {
        static StateExtendDatabase()
        {
            //  データベースに登録。
            AniconFilePath_to_tables = new Dictionary<string, StateExTableable>()
            {
                {"Assets/Scripts/StellaQLEngine/Anicon@StellaQL.controller", SceneStellaQLTest.StateExTable.Instance },
                {"Assets/Resources/AnimatorControllers/AniCon@Select.controller", SceneSelect.StateExTable.Instance},
                {"Assets/Resources/AnimatorControllers/AniCon@Char3.controller", SceneMain.StateExTable.Instance },
            };

        }

        public static Dictionary<string,StateExTableable> AniconFilePath_to_tables { get; set; }

        public static StateExTableable GetTableEx(string path_animationController)
        {
            return AniconFilePath_to_tables[path_animationController];
        }
    }
}
