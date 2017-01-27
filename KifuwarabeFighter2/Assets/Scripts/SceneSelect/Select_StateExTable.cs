using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StellaQL;

namespace SceneSelect {

    /// <summary>
    /// Animator の State に一対一対応☆
    /// </summary>
    public enum StateIndex
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
    public class StateExRecord : AbstractStateExRecord
    {
        public StateExRecord(string breadCrumb, string name)
        {
            this.BreadCrumb = breadCrumb;
            this.Name = name;
        }

        public override bool HasFlag_attr(int enumration)
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

        private StateExTable()
        {
            index_to_exRecord = new Dictionary<int, StateExRecordable> ()//AstateIndex
            {
                {(int)StateIndex.Stay, new StateExRecord(  "Base Layer.", "Stay")},
                {(int)StateIndex.Move, new StateExRecord(  "Base Layer.", "Move")},
                {(int)StateIndex.Ready, new StateExRecord(  "Base Layer.", "Ready")},
                {(int)StateIndex.Timeover, new StateExRecord(  "Base Layer.", "Timeover")},
            };
        }
    }

}
