using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StellaQL;

namespace SceneSelect {

    /// <summary>
    /// アニメーターのステート
    /// </summary>
    public class StateExRecord : AbstractStateExRecord
    {
        public static StateExRecord Build(string fullpath)
        {
            return new StateExRecord(fullpath, Animator.StringToHash(fullpath));
        }
        public StateExRecord(string fullpath, int fullpathHash) :base(fullpath, fullpathHash, - 1)
        {
        }

        public override bool HasFlag_attr(int enumration)
        {
            return false;
        }
    }

    public class StateExTable : AbstractStateExTable
    {
        public const string FULLNAME_STAY = "Base Layer.Stay";
        public const string FULLNAME_MOVE = "Base Layer.Move";
        public const string FULLNAME_READY = "Base Layer.Ready";
        public const string FULLNAME_TIMEOVER = "Base Layer.Timeover";

        static StateExTable()
        {
            Instance = new StateExTable();
        }

        public static StateExTable Instance { get; set; }

        protected StateExTable()
        {
            List<StateExRecordable> temp = new List<StateExRecordable>()
            {
                StateExRecord.Build( StateExTable.FULLNAME_STAY),
                StateExRecord.Build( StateExTable.FULLNAME_MOVE),
                StateExRecord.Build( StateExTable.FULLNAME_READY),
                StateExRecord.Build( StateExTable.FULLNAME_TIMEOVER),
            };
            //hash_to_exRecord = new Dictionary<int, StateExRecordable>();
            foreach (StateExRecordable record in temp) { hash_to_exRecord.Add(record.FullPathHash, record); }
        }
    }

}
