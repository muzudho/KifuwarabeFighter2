using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneSelect {

    /// <summary>
    /// Animator の State に一対一対応☆
    /// </summary>
    public enum AstateIndex
    {
        Stay,
        Move,
        Ready,
        Timeover,
        Num,
    }

    /// <summary>
    /// アニメーターのステート
    /// </summary>
    public class AstateRecord : AstateRecordable
    {
        public string BreadCrumb { get; set; }
        public string Name { get; set; }

        public AstateRecord(string breadCrumb, string name)
        {
            this.BreadCrumb = breadCrumb;
            this.Name = name;
        }

        public bool HasFlag(int enumration)
        {
            return false;
        }
    }

    public class AstateDatabase : AbstractAstateDatabase
    {
        static AstateDatabase()
        {
            Instance = new AstateDatabase();
        }

        public static AstateDatabase Instance { get; set; }

        private AstateDatabase()
        {
            index_to_record = new Dictionary<int, AstateRecordable> ()//AstateIndex
            {
                {(int)AstateIndex.Stay, new AstateRecord(  "Base Layer.", "Stay")},
                {(int)AstateIndex.Move, new AstateRecord(  "Base Layer.", "Move")},
                {(int)AstateIndex.Ready, new AstateRecord(  "Base Layer.", "Ready")},
                {(int)AstateIndex.Timeover, new AstateRecord(  "Base Layer.", "Timeover")},
            };
        }
    }

}
