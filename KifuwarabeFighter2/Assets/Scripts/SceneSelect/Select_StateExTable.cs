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

        private StateExTable()
        {
            fullpath_to_index = new Dictionary<string, int>()
            {
                {FULLNAME_STAY,0 },
                {FULLNAME_MOVE,1 },
                {FULLNAME_READY,2},
                {FULLNAME_TIMEOVER,3 },
            };
            index_to_exRecord = new Dictionary<int, StateExRecordable> ()//AstateIndex
            {
                {fullpath_to_index[FULLNAME_STAY], StateExRecord.Build( StateExTable.FULLNAME_STAY)},
                {fullpath_to_index[FULLNAME_MOVE], StateExRecord.Build( StateExTable.FULLNAME_MOVE)},
                {fullpath_to_index[FULLNAME_READY], StateExRecord.Build( StateExTable.FULLNAME_READY)},
                {fullpath_to_index[FULLNAME_TIMEOVER], StateExRecord.Build( StateExTable.FULLNAME_TIMEOVER)},
            };
        }
    }

}
