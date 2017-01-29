using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StellaQL;
using System;

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
        public StateExRecord(string fullpath, int fullpathHash) :base(fullpath, fullpathHash, new HashSet<int>() { })
        {
        }

        public override bool HasFlag_attr(HashSet<int> attributeBitfield)
        {
            return false;
        }
    }

    public class StateExTable : AbstractStateExTable
    {
        static StateExTable()
        {
            Instance = new StateExTable();
        }
        public static StateExTable Instance { get; set; }

        public const string TAG_ZERO = "Zero";

        public const string STATE_STAY = "Base Layer.Stay";
        public const string STATE_MOVE = "Base Layer.Move";
        public const string STATE_READY = "Base Layer.Ready";
        public const string STATE_TIMEOVER = "Base Layer.Timeover";

        protected StateExTable()
        {
            String_to_tagHash = Code.HashsDic(new []{
                TAG_ZERO
            });

            List<StateExRecordable> temp = new List<StateExRecordable>()
            {
                StateExRecord.Build( STATE_STAY),
                StateExRecord.Build( STATE_MOVE),
                StateExRecord.Build( STATE_READY),
                StateExRecord.Build( STATE_TIMEOVER),
            };
            foreach (StateExRecordable record in temp) { Hash_to_exRecord.Add(record.FullPathHash, record); }
        }
    }

}
