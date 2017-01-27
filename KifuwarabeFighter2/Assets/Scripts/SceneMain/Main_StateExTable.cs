using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StellaQL;

/// <summary>
/// Main シーン
/// </summary>
namespace SceneMain
{
    /// <summary>
    /// Animator の State に一対一対応☆
    /// </summary>
    public enum StateIndex
    {
        SWait,
        SMove,

        SBlockL,
        SBlockM,
        SBlockH,
        SAtkLP,
        SAtkMP,
        SAtkHP,
        SAtkLK,
        SAtkMK,
        SAtkHK,

        OBackstep,

        JBlockL,
        JBlockM,
        JBlockH,
        JAtkLP,
        JAtkMP,
        JAtkHP,
        JAtkLK,
        JAtkMK,
        JAtkHK,

        JMove0,
        JMove1,
        JMove2,
        JMove3,
        JMove4,

        DBlockL,
        DBlockM,
        DBlockH,
        DAtkLP,
        DAtkMP,
        DAtkHP,
        DAtkLK,
        DAtkMK,
        DAtkHK,

        DMove,

        CBlockL,
        CBlockM,
        CBlockH,
        CAtkLP,
        CAtkMP,
        CAtkHP,
        CAtkLK,
        CAtkMK,
        CAtkHK,

        CWait,
        CMove,

        OGiveup,
        ODown_SDamageH,
        ODown,
        OStandup,

        SDamageL,
        SDamageM,
        SDamageH,

        JDamageL,
        JDamageM,
        JDamageH,

        DDamageL,
        DDamageM,
        DDamageH,

        CDamageL,
        CDamageM,
        CDamageH,

        Num,
    }

    /// <summary>
    /// アニメーターのステート
    /// </summary>
    public class StateExRecord : AbstractStateExRecord
    {
        public CliptypeIndex acliptype;

        public StateExRecord(string breadCrumb, string name, CliptypeIndex acliptype, StateExTable.Attr attribute)
        {
            this.BreadCrumb = breadCrumb;
            this.Name = name;
            this.acliptype = acliptype;
            this.AttributeEnum = (int)attribute;
        }

        public override bool HasFlag_attr(int enumration)
        {
            //Debug.Log("InFlag[" + ((AstateDatabase.Attr)enumration).HasFlag(this.attribute) + "] = [" + ((AstateDatabase.Attr)enumration) + "].HasFlag(" + this.attribute + ")");
            //return ((AstateDatabase.Attr)enumration).HasFlag(this.attribute);

            Debug.Log("InFlag[" + ((StateExTable.Attr)this.AttributeEnum).HasFlag((StateExTable.Attr)enumration) + "] = [" + this.AttributeEnum + "].HasFlag(" + ((StateExTable.Attr)enumration) + ")");
            return ((StateExTable.Attr)this.AttributeEnum).HasFlag((StateExTable.Attr)enumration);
        }
    }

    public class StateExTable : AbstractStateExTable
    {
        /// <summary>
        /// AstateAttribute. 略したいので子クラスとして名称を縮めた。
        /// </summary>
        [Flags]
        public enum Attr
        {
            None = 0,
            /// <summary>
            /// キャラクターが、レバーのＸ軸の入力を受け取れる状態でないとき。
            /// </summary>
            BusyX = 0x01,
            /// <summary>
            /// キャラクターが、レバーのＹ軸の入力を受け取れる状態でないとき。
            /// </summary>
            BusyY = 0x01 << 1,
            /// <summary>
            /// ブロック・モーションなら
            /// </summary>
            Block = 0x01 << 2,
        }

        static StateExTable()
        {
            Instance = new StateExTable();
        }

        public static StateExTable Instance { get; set; }

        private StateExTable()
        {
            index_to_exRecord = new Dictionary<int, StateExRecordable>()//[StateIndex]
            {
                {(int)StateIndex.SWait, new StateExRecord(  "Base Layer.", "SWait", CliptypeIndex.SWait,Attr.None)},
                {(int)StateIndex.SMove,  new StateExRecord(  "Base Layer.", "SMove", CliptypeIndex.SMove,Attr.None)},
                {(int)StateIndex.SBlockL,  new StateExRecord(  "Base Layer.", "SBlockL", CliptypeIndex.SBlockL, Attr.BusyX | Attr.Block)},
                {(int)StateIndex.SBlockM,  new StateExRecord(  "Base Layer.", "SBlockM", CliptypeIndex.SBlockM, Attr.BusyX | Attr.Block)},
                {(int)StateIndex.SBlockH,  new StateExRecord(  "Base Layer.", "SBlockH", CliptypeIndex.SBlockH, Attr.BusyX | Attr.Block)},
                {(int)StateIndex.SAtkLP,  new StateExRecord(  "Base Layer.", "SAtkLP", CliptypeIndex.SAtkLP,Attr.BusyX)},
                {(int)StateIndex.SAtkMP,  new StateExRecord(  "Base Layer.", "SAtkMP", CliptypeIndex.SAtkMP,Attr.BusyX)},
                {(int)StateIndex.SAtkHP,  new StateExRecord( "Base Layer.", "SAtkHP",  CliptypeIndex.SAtkHP,Attr.BusyX)},
                {(int)StateIndex.SAtkLK,  new StateExRecord(  "Base Layer.", "SAtkLK", CliptypeIndex.SAtkLK,Attr.BusyX)},
                {(int)StateIndex.SAtkMK,  new StateExRecord(  "Base Layer.", "SAtkMK", CliptypeIndex.SAtkMK,Attr.BusyX)},
                {(int)StateIndex.SAtkHK,  new StateExRecord(  "Base Layer.", "SAtkHK", CliptypeIndex.SAtkHK,Attr.BusyX)},

                {(int)StateIndex.OBackstep,  new StateExRecord(  "Base Layer.", "OBackstep", CliptypeIndex.OBackstep,Attr.None)},

                {(int)StateIndex.JBlockL,  new StateExRecord(  "Base Layer.", "JBlockL", CliptypeIndex.JBlockL, Attr.BusyX | Attr.Block)},
                {(int)StateIndex.JBlockM,  new StateExRecord(  "Base Layer.", "JBlockM", CliptypeIndex.JBlockM, Attr.BusyX | Attr.Block)},
                {(int)StateIndex.JBlockH,  new StateExRecord(  "Base Layer.", "JBlockH", CliptypeIndex.JBlockH, Attr.BusyX | Attr.Block)},
                {(int)StateIndex.JAtkLP,  new StateExRecord(  "Base Layer.", "JAtkLP", CliptypeIndex.JAtkLP,Attr.None)},
                {(int)StateIndex.JAtkMP,  new StateExRecord(  "Base Layer.", "JAtkMP", CliptypeIndex.JAtkMP,Attr.None)},
                {(int)StateIndex.JAtkHP,  new StateExRecord(  "Base Layer.", "JAtkHP", CliptypeIndex.JAtkHP,Attr.None)},
                {(int)StateIndex.JAtkLK,  new StateExRecord(  "Base Layer.", "JAtkLK", CliptypeIndex.JAtkLK,Attr.None)},
                {(int)StateIndex.JAtkMK,  new StateExRecord( "Base Layer.", "JAtkMK",  CliptypeIndex.JAtkMK,Attr.None)},
                {(int)StateIndex.JAtkHK,  new StateExRecord(  "Base Layer.", "JAtkHK", CliptypeIndex.JAtkHK,Attr.None)},

                {(int)StateIndex.JMove0,  new StateExRecord(  "Base Layer.JMove.", "JMove0", CliptypeIndex.JMove0,Attr.BusyX | Attr.BusyY)},
                {(int)StateIndex.JMove1,  new StateExRecord(  "Base Layer.JMove.", "JMove1", CliptypeIndex.JMove1,Attr.None)},
                {(int)StateIndex.JMove2,  new StateExRecord( "Base Layer.JMove.", "JMove2",  CliptypeIndex.JMove2,Attr.None)},
                {(int)StateIndex.JMove3,  new StateExRecord(  "Base Layer.JMove.", "JMove3", CliptypeIndex.JMove3,Attr.None)},
                {(int)StateIndex.JMove4,  new StateExRecord(  "Base Layer.JMove.", "JMove4", CliptypeIndex.JMove4,Attr.BusyX)},

                {(int)StateIndex.DBlockL,  new StateExRecord(  "Base Layer.", "DBlockL", CliptypeIndex.DBlockL, Attr.BusyX | Attr.Block)},
                {(int)StateIndex.DBlockM,  new StateExRecord(  "Base Layer.", "DBlockM", CliptypeIndex.DBlockM, Attr.BusyX | Attr.Block)},
                {(int)StateIndex.DBlockH,  new StateExRecord(  "Base Layer.", "DBlockH", CliptypeIndex.DBlockH, Attr.BusyX | Attr.Block)},
                {(int)StateIndex.DAtkLP,  new StateExRecord(  "Base Layer.", "DAtkLP", CliptypeIndex.DAtkLP,Attr.None)},
                {(int)StateIndex.DAtkMP,  new StateExRecord(  "Base Layer.", "DAtkMP", CliptypeIndex.DAtkMP,Attr.None)},
                {(int)StateIndex.DAtkHP,  new StateExRecord(  "Base Layer.", "DAtkHP", CliptypeIndex.DAtkHP,Attr.None)},
                {(int)StateIndex.DAtkLK,  new StateExRecord(  "Base Layer.", "DAtkLK", CliptypeIndex.DAtkLK,Attr.None)},
                {(int)StateIndex.DAtkMK,  new StateExRecord(  "Base Layer.", "DAtkMK", CliptypeIndex.DAtkMK,Attr.None)},
                {(int)StateIndex.DAtkHK,  new StateExRecord(  "Base Layer.", "DAtkHK", CliptypeIndex.DAtkHK,Attr.None)},

                {(int)StateIndex.DMove,  new StateExRecord(  "Base Layer.", "DMove", CliptypeIndex.DMove,Attr.None)},

                {(int)StateIndex.CBlockL,  new StateExRecord(  "Base Layer.", "CBlockL", CliptypeIndex.CBlockL, Attr.BusyX | Attr.Block)},
                {(int)StateIndex.CBlockM,  new StateExRecord(  "Base Layer.", "CBlockM", CliptypeIndex.CBlockM, Attr.BusyX | Attr.Block)},
                {(int)StateIndex.CBlockH,  new StateExRecord(  "Base Layer.", "CBlockH", CliptypeIndex.CBlockH, Attr.BusyX | Attr.Block)},
                {(int)StateIndex.CAtkLP,  new StateExRecord(  "Base Layer.", "CAtkLP", CliptypeIndex.CAtkLP,Attr.BusyX)},
                {(int)StateIndex.CAtkMP,  new StateExRecord(  "Base Layer.", "CAtkMP", CliptypeIndex.CAtkMP,Attr.BusyX)},
                {(int)StateIndex.CAtkHP,  new StateExRecord(  "Base Layer.", "CAtkHP", CliptypeIndex.CAtkHP,Attr.BusyX)},
                {(int)StateIndex.CAtkLK,  new StateExRecord(  "Base Layer.", "CAtkLK", CliptypeIndex.CAtkLK,Attr.BusyX)},
                {(int)StateIndex.CAtkMK,  new StateExRecord(  "Base Layer.", "CAtkMK", CliptypeIndex.CAtkMK,Attr.BusyX)},
                {(int)StateIndex.CAtkHK,  new StateExRecord(  "Base Layer.", "CAtkHK", CliptypeIndex.CAtkHK,Attr.BusyX)},

                {(int)StateIndex.CWait,  new StateExRecord(  "Base Layer.", "CWait", CliptypeIndex.CWait,Attr.None)},
                {(int)StateIndex.CMove,  new StateExRecord(  "Base Layer.", "CMove", CliptypeIndex.CMove,Attr.None)},

                {(int)StateIndex.OGiveup,  new StateExRecord(  "Base Layer.", "OGiveup", CliptypeIndex.OGiveup,Attr.None)},
                {(int)StateIndex.ODown_SDamageH,  new StateExRecord(  "Base Layer.", "ODown_SDamageH", CliptypeIndex.SDamageH,Attr.BusyX)},
                {(int)StateIndex.ODown,  new StateExRecord(  "Base Layer.", "ODown", CliptypeIndex.ODown,Attr.BusyX)},
                {(int)StateIndex.OStandup,  new StateExRecord(  "Base Layer.", "OStandup", CliptypeIndex.OStandup,Attr.None)},

                {(int)StateIndex.SDamageL,  new StateExRecord(  "Base Layer.", "SDamageL", CliptypeIndex.SDamageL,Attr.None)},
                {(int)StateIndex.SDamageM,  new StateExRecord(  "Base Layer.", "SDamageM", CliptypeIndex.SDamageM,Attr.None)},
                {(int)StateIndex.SDamageH,  new StateExRecord(  "Base Layer.", "SDamageH", CliptypeIndex.SDamageH,Attr.None)},

                {(int)StateIndex.JDamageL,  new StateExRecord(  "Base Layer.", "JDamageL", CliptypeIndex.JDamageL,Attr.None)},
                {(int)StateIndex.JDamageM,  new StateExRecord(  "Base Layer.", "JDamageM", CliptypeIndex.JDamageM,Attr.None)},
                {(int)StateIndex.JDamageH,  new StateExRecord(  "Base Layer.", "JDamageH", CliptypeIndex.JDamageH,Attr.None)},

                {(int)StateIndex.DDamageL,  new StateExRecord(  "Base Layer.", "DDamageL", CliptypeIndex.DDamageL,Attr.None)},
                {(int)StateIndex.DDamageM,  new StateExRecord(  "Base Layer.", "DDamageM", CliptypeIndex.DDamageM,Attr.None)},
                {(int)StateIndex.DDamageH,  new StateExRecord(  "Base Layer.", "DDamageH", CliptypeIndex.DDamageH,Attr.None)},

                {(int)StateIndex.CDamageL,  new StateExRecord(  "Base Layer.", "CDamageL", CliptypeIndex.CDamageL,Attr.None)},
                {(int)StateIndex.CDamageM,  new StateExRecord(  "Base Layer.", "CDamageM", CliptypeIndex.CDamageM,Attr.None)},
                {(int)StateIndex.CDamageH,  new StateExRecord(  "Base Layer.", "CDamageH", CliptypeIndex.CDamageH,Attr.None)},
            };
        }

        /// <summary>
        /// キャラクターと、モーション、現在のフレームを指定することで、通し画像番号とスライス番号を返す。
        /// これにより Hitbox2DScript と連携を取ることができる。
        /// </summary>
        /// <param name="serialImageIndex"></param>
        /// <param name="slice"></param>
        /// <param name="character"></param>
        /// <param name="motion"></param>
        /// <param name="currentMotionFrame"></param>
        public static void GetSlice(out int serialImageIndex, out int slice, CharacterIndex character, CliptypeRecord aclipTypeRecord, int currentMotionFrame)
        {
            slice = aclipTypeRecord.slices[currentMotionFrame];

            serialImageIndex = Hitbox2DOperationScript.GetSerialImageIndex(character, aclipTypeRecord.actioning);
        }
    }
}

