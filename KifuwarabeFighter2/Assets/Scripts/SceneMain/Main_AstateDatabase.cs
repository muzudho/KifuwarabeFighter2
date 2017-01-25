using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main シーン
/// </summary>
namespace SceneMain
{
    /// <summary>
    /// Animator の State に一対一対応☆
    /// </summary>
    public enum AstateIndex
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
    public class AstateRecord : AbstractAstateRecord
    {
        public AcliptypeIndex acliptype;
        //public int attribute;//AstateDatabase.Attr

        public AstateRecord(string breadCrumb, string name, AcliptypeIndex acliptype, AstateDatabase.Attr attribute)
        {
            this.BreadCrumb = breadCrumb;
            this.Name = name;
            this.acliptype = acliptype;
            this.AttributeEnum = (int)attribute;
        }

        public override bool HasFlag(int enumration)
        {
            //Debug.Log("InFlag[" + ((AstateDatabase.Attr)enumration).HasFlag(this.attribute) + "] = [" + ((AstateDatabase.Attr)enumration) + "].HasFlag(" + this.attribute + ")");
            //return ((AstateDatabase.Attr)enumration).HasFlag(this.attribute);

            Debug.Log("InFlag[" + ((AstateDatabase.Attr)this.AttributeEnum).HasFlag((AstateDatabase.Attr)enumration) + "] = [" + this.AttributeEnum + "].HasFlag(" + ((AstateDatabase.Attr)enumration) + ")");
            return ((AstateDatabase.Attr)this.AttributeEnum).HasFlag((AstateDatabase.Attr)enumration);
        }
    }

    public class AstateDatabase : AbstractAstateDatabase
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

        static AstateDatabase()
        {
            Instance = new AstateDatabase();
        }

        public static AstateDatabase Instance { get; set; }

        private AstateDatabase()
        {
            index_to_record = new Dictionary<int, AstateRecordable>()//AstateIndex
            {
                {(int)AstateIndex.SWait, new AstateRecord(  "Base Layer.", "SWait", AcliptypeIndex.SWait,Attr.None)},
                {(int)AstateIndex.SMove,  new AstateRecord(  "Base Layer.", "SMove", AcliptypeIndex.SMove,Attr.None)},
                {(int)AstateIndex.SBlockL,  new AstateRecord(  "Base Layer.", "SBlockL", AcliptypeIndex.SBlockL, Attr.BusyX | Attr.Block)},
                {(int)AstateIndex.SBlockM,  new AstateRecord(  "Base Layer.", "SBlockM", AcliptypeIndex.SBlockM, Attr.BusyX | Attr.Block)},
                {(int)AstateIndex.SBlockH,  new AstateRecord(  "Base Layer.", "SBlockH", AcliptypeIndex.SBlockH, Attr.BusyX | Attr.Block)},
                {(int)AstateIndex.SAtkLP,  new AstateRecord(  "Base Layer.", "SAtkLP", AcliptypeIndex.SAtkLP,Attr.BusyX)},
                {(int)AstateIndex.SAtkMP,  new AstateRecord(  "Base Layer.", "SAtkMP", AcliptypeIndex.SAtkMP,Attr.BusyX)},
                {(int)AstateIndex.SAtkHP,  new AstateRecord( "Base Layer.", "SAtkHP",  AcliptypeIndex.SAtkHP,Attr.BusyX)},
                {(int)AstateIndex.SAtkLK,  new AstateRecord(  "Base Layer.", "SAtkLK", AcliptypeIndex.SAtkLK,Attr.BusyX)},
                {(int)AstateIndex.SAtkMK,  new AstateRecord(  "Base Layer.", "SAtkMK", AcliptypeIndex.SAtkMK,Attr.BusyX)},
                {(int)AstateIndex.SAtkHK,  new AstateRecord(  "Base Layer.", "SAtkHK", AcliptypeIndex.SAtkHK,Attr.BusyX)},

                {(int)AstateIndex.OBackstep,  new AstateRecord(  "Base Layer.", "OBackstep", AcliptypeIndex.OBackstep,Attr.None)},

                {(int)AstateIndex.JBlockL,  new AstateRecord(  "Base Layer.", "JBlockL", AcliptypeIndex.JBlockL, Attr.BusyX | Attr.Block)},
                {(int)AstateIndex.JBlockM,  new AstateRecord(  "Base Layer.", "JBlockM", AcliptypeIndex.JBlockM, Attr.BusyX | Attr.Block)},
                {(int)AstateIndex.JBlockH,  new AstateRecord(  "Base Layer.", "JBlockH", AcliptypeIndex.JBlockH, Attr.BusyX | Attr.Block)},
                {(int)AstateIndex.JAtkLP,  new AstateRecord(  "Base Layer.", "JAtkLP", AcliptypeIndex.JAtkLP,Attr.None)},
                {(int)AstateIndex.JAtkMP,  new AstateRecord(  "Base Layer.", "JAtkMP", AcliptypeIndex.JAtkMP,Attr.None)},
                {(int)AstateIndex.JAtkHP,  new AstateRecord(  "Base Layer.", "JAtkHP", AcliptypeIndex.JAtkHP,Attr.None)},
                {(int)AstateIndex.JAtkLK,  new AstateRecord(  "Base Layer.", "JAtkLK", AcliptypeIndex.JAtkLK,Attr.None)},
                {(int)AstateIndex.JAtkMK,  new AstateRecord( "Base Layer.", "JAtkMK",  AcliptypeIndex.JAtkMK,Attr.None)},
                {(int)AstateIndex.JAtkHK,  new AstateRecord(  "Base Layer.", "JAtkHK", AcliptypeIndex.JAtkHK,Attr.None)},

                {(int)AstateIndex.JMove0,  new AstateRecord(  "Base Layer.JMove.", "JMove0", AcliptypeIndex.JMove0,Attr.BusyX | Attr.BusyY)},
                {(int)AstateIndex.JMove1,  new AstateRecord(  "Base Layer.JMove.", "JMove1", AcliptypeIndex.JMove1,Attr.None)},
                {(int)AstateIndex.JMove2,  new AstateRecord( "Base Layer.JMove.", "JMove2",  AcliptypeIndex.JMove2,Attr.None)},
                {(int)AstateIndex.JMove3,  new AstateRecord(  "Base Layer.JMove.", "JMove3", AcliptypeIndex.JMove3,Attr.None)},
                {(int)AstateIndex.JMove4,  new AstateRecord(  "Base Layer.JMove.", "JMove4", AcliptypeIndex.JMove4,Attr.BusyX)},

                {(int)AstateIndex.DBlockL,  new AstateRecord(  "Base Layer.", "DBlockL", AcliptypeIndex.DBlockL, Attr.BusyX | Attr.Block)},
                {(int)AstateIndex.DBlockM,  new AstateRecord(  "Base Layer.", "DBlockM", AcliptypeIndex.DBlockM, Attr.BusyX | Attr.Block)},
                {(int)AstateIndex.DBlockH,  new AstateRecord(  "Base Layer.", "DBlockH", AcliptypeIndex.DBlockH, Attr.BusyX | Attr.Block)},
                {(int)AstateIndex.DAtkLP,  new AstateRecord(  "Base Layer.", "DAtkLP", AcliptypeIndex.DAtkLP,Attr.None)},
                {(int)AstateIndex.DAtkMP,  new AstateRecord(  "Base Layer.", "DAtkMP", AcliptypeIndex.DAtkMP,Attr.None)},
                {(int)AstateIndex.DAtkHP,  new AstateRecord(  "Base Layer.", "DAtkHP", AcliptypeIndex.DAtkHP,Attr.None)},
                {(int)AstateIndex.DAtkLK,  new AstateRecord(  "Base Layer.", "DAtkLK", AcliptypeIndex.DAtkLK,Attr.None)},
                {(int)AstateIndex.DAtkMK,  new AstateRecord(  "Base Layer.", "DAtkMK", AcliptypeIndex.DAtkMK,Attr.None)},
                {(int)AstateIndex.DAtkHK,  new AstateRecord(  "Base Layer.", "DAtkHK", AcliptypeIndex.DAtkHK,Attr.None)},

                {(int)AstateIndex.DMove,  new AstateRecord(  "Base Layer.", "DMove", AcliptypeIndex.DMove,Attr.None)},

                {(int)AstateIndex.CBlockL,  new AstateRecord(  "Base Layer.", "CBlockL", AcliptypeIndex.CBlockL, Attr.BusyX | Attr.Block)},
                {(int)AstateIndex.CBlockM,  new AstateRecord(  "Base Layer.", "CBlockM", AcliptypeIndex.CBlockM, Attr.BusyX | Attr.Block)},
                {(int)AstateIndex.CBlockH,  new AstateRecord(  "Base Layer.", "CBlockH", AcliptypeIndex.CBlockH, Attr.BusyX | Attr.Block)},
                {(int)AstateIndex.CAtkLP,  new AstateRecord(  "Base Layer.", "CAtkLP", AcliptypeIndex.CAtkLP,Attr.BusyX)},
                {(int)AstateIndex.CAtkMP,  new AstateRecord(  "Base Layer.", "CAtkMP", AcliptypeIndex.CAtkMP,Attr.BusyX)},
                {(int)AstateIndex.CAtkHP,  new AstateRecord(  "Base Layer.", "CAtkHP", AcliptypeIndex.CAtkHP,Attr.BusyX)},
                {(int)AstateIndex.CAtkLK,  new AstateRecord(  "Base Layer.", "CAtkLK", AcliptypeIndex.CAtkLK,Attr.BusyX)},
                {(int)AstateIndex.CAtkMK,  new AstateRecord(  "Base Layer.", "CAtkMK", AcliptypeIndex.CAtkMK,Attr.BusyX)},
                {(int)AstateIndex.CAtkHK,  new AstateRecord(  "Base Layer.", "CAtkHK", AcliptypeIndex.CAtkHK,Attr.BusyX)},

                {(int)AstateIndex.CWait,  new AstateRecord(  "Base Layer.", "CWait", AcliptypeIndex.CWait,Attr.None)},
                {(int)AstateIndex.CMove,  new AstateRecord(  "Base Layer.", "CMove", AcliptypeIndex.CMove,Attr.None)},

                {(int)AstateIndex.OGiveup,  new AstateRecord(  "Base Layer.", "OGiveup", AcliptypeIndex.OGiveup,Attr.None)},
                {(int)AstateIndex.ODown_SDamageH,  new AstateRecord(  "Base Layer.", "ODown_SDamageH", AcliptypeIndex.SDamageH,Attr.BusyX)},
                {(int)AstateIndex.ODown,  new AstateRecord(  "Base Layer.", "ODown", AcliptypeIndex.ODown,Attr.BusyX)},
                {(int)AstateIndex.OStandup,  new AstateRecord(  "Base Layer.", "OStandup", AcliptypeIndex.OStandup,Attr.None)},

                {(int)AstateIndex.SDamageL,  new AstateRecord(  "Base Layer.", "SDamageL", AcliptypeIndex.SDamageL,Attr.None)},
                {(int)AstateIndex.SDamageM,  new AstateRecord(  "Base Layer.", "SDamageM", AcliptypeIndex.SDamageM,Attr.None)},
                {(int)AstateIndex.SDamageH,  new AstateRecord(  "Base Layer.", "SDamageH", AcliptypeIndex.SDamageH,Attr.None)},

                {(int)AstateIndex.JDamageL,  new AstateRecord(  "Base Layer.", "JDamageL", AcliptypeIndex.JDamageL,Attr.None)},
                {(int)AstateIndex.JDamageM,  new AstateRecord(  "Base Layer.", "JDamageM", AcliptypeIndex.JDamageM,Attr.None)},
                {(int)AstateIndex.JDamageH,  new AstateRecord(  "Base Layer.", "JDamageH", AcliptypeIndex.JDamageH,Attr.None)},

                {(int)AstateIndex.DDamageL,  new AstateRecord(  "Base Layer.", "DDamageL", AcliptypeIndex.DDamageL,Attr.None)},
                {(int)AstateIndex.DDamageM,  new AstateRecord(  "Base Layer.", "DDamageM", AcliptypeIndex.DDamageM,Attr.None)},
                {(int)AstateIndex.DDamageH,  new AstateRecord(  "Base Layer.", "DDamageH", AcliptypeIndex.DDamageH,Attr.None)},

                {(int)AstateIndex.CDamageL,  new AstateRecord(  "Base Layer.", "CDamageL", AcliptypeIndex.CDamageL,Attr.None)},
                {(int)AstateIndex.CDamageM,  new AstateRecord(  "Base Layer.", "CDamageM", AcliptypeIndex.CDamageM,Attr.None)},
                {(int)AstateIndex.CDamageH,  new AstateRecord(  "Base Layer.", "CDamageH", AcliptypeIndex.CDamageH,Attr.None)},
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
        public static void GetSlice(out int serialImageIndex, out int slice, CharacterIndex character, AcliptypeRecord aclipTypeRecord, int currentMotionFrame)
        {
            slice = aclipTypeRecord.slices[currentMotionFrame];

            serialImageIndex = Hitbox2DOperationScript.GetSerialImageIndex(character, aclipTypeRecord.actioning);
        }
    }
}

