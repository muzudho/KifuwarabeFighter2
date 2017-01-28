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
    /// アニメーターのステート
    /// </summary>
    public class StateExRecord : AbstractStateExRecord
    {
        public static StateExRecord Build(string fullpath, StateExTable.Attr attributeEnum)
        {
            return new StateExRecord(fullpath, Animator.StringToHash(fullpath), attributeEnum);
        }
        public static StateExRecord Build(string fullpath, CliptypeIndex cliptype, StateExTable.Attr attributeEnum)
        {
            return new StateExRecord(fullpath, Animator.StringToHash(fullpath), cliptype, attributeEnum);
        }
        public StateExRecord(string fullpath, int fullpathHash, StateExTable.Attr attributeEnum):base(fullpath, fullpathHash, (int)attributeEnum)
        {
            this.Cliptype = -1; // クリップタイプを使わない場合
        }
        public StateExRecord(string fullpath, int fullpathHash, CliptypeIndex cliptype, StateExTable.Attr attributeEnum):base(fullpath, fullpathHash, (int)attributeEnum)
        {
            this.Cliptype = (int)cliptype;
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

        public const string FULLNAME_SWAIT = "Base Layer.SWait";
        public const string FULLNAME_SMOVE = "Base Layer.SMove";
        public const string FULLNAME_SBLOCKL = "Base Layer.SBlockL";
        public const string FULLNAME_SBLOCKM = "Base Layer.SBlockM";
        public const string FULLNAME_SBLOCKH = "Base Layer.SBlockH";
        public const string FULLNAME_SATKLP = "Base Layer.SAtkLP";
        public const string FULLNAME_SATKMP = "Base Layer.SAtkMP";
        public const string FULLNAME_SATKHP = "Base Layer.SAtkHP";
        public const string FULLNAME_SATKLK = "Base Layer.SAtkLK";
        public const string FULLNAME_SATKMK = "Base Layer.SAtkMK";
        public const string FULLNAME_SATKHK = "Base Layer.SAtkHK";

        public const string FULLNAME_OBACKSTEP = "Base Layer.OBackstep";

        public const string FULLNAME_JBLOCKL = "Base Layer.JBlockL";
        public const string FULLNAME_JBLOCKM = "Base Layer.JBlockM";
        public const string FULLNAME_JBLOCKH = "Base Layer.JBlockH";
        public const string FULLNAME_JATKLP = "Base Layer.JAtkLP";
        public const string FULLNAME_JATKMP = "Base Layer.JAtkMP";
        public const string FULLNAME_JATKHP = "Base Layer.JAtkHP";
        public const string FULLNAME_JATKLK = "Base Layer.JAtkLK";
        public const string FULLNAME_JATKMK = "Base Layer.JAtkMK";
        public const string FULLNAME_JATKHK = "Base Layer.JAtkHK";

        public const string FULLNAME_JMOVE0 = "Base Layer.JMove.JMove0";
        public const string FULLNAME_JMOVE1 = "Base Layer.JMove.JMove1";
        public const string FULLNAME_JMOVE2 = "Base Layer.JMove.JMove2";
        public const string FULLNAME_JMOVE3 = "Base Layer.JMove.JMove3";
        public const string FULLNAME_JMOVE4 = "Base Layer.JMove.JMove4";

        public const string FULLNAME_DBLOCKL = "Base Layer.DBlockL";
        public const string FULLNAME_DBLOCKM = "Base Layer.DBlockM";
        public const string FULLNAME_DBLOCKH = "Base Layer.DBlockH";
        public const string FULLNAME_DATKLP = "Base Layer.DAtkLP";
        public const string FULLNAME_DATKMP = "Base Layer.DAtkMP";
        public const string FULLNAME_DATKHP = "Base Layer.DAtkHP";
        public const string FULLNAME_DATKLK = "Base Layer.DAtkLK";
        public const string FULLNAME_DATKMK = "Base Layer.DAtkMK";
        public const string FULLNAME_DATKHK = "Base Layer.DAtkHK";

        public const string FULLNAME_DMOVE = "Base Layer.DMove";

        public const string FULLNAME_CBLOCKL = "Base Layer.CBlockL";
        public const string FULLNAME_CBLOCKM = "Base Layer.CBlockM";
        public const string FULLNAME_CBLOCKH = "Base Layer.CBlockH";
        public const string FULLNAME_CATKLP = "Base Layer.CAtkLP";
        public const string FULLNAME_CATKMP = "Base Layer.CAtkMP";
        public const string FULLNAME_CATKHP = "Base Layer.CAtkHP";
        public const string FULLNAME_CATKLK = "Base Layer.CAtkLK";
        public const string FULLNAME_CATKMK = "Base Layer.CAtkMK";
        public const string FULLNAME_CATKHK = "Base Layer.CAtkHK";

        public const string FULLNAME_CWAIT = "Base Layer.CWait";
        public const string FULLNAME_CMOVE = "Base Layer.CMove";

        public const string FULLNAME_OGIVEUP = "Base Layer.OGiveup";
        public const string FULLNAME_ODOWN_SDAMAGEH = "Base Layer.ODown_SDamageH";
        public const string FULLNAME_ODOWN = "Base Layer.ODown";
        public const string FULLNAME_OSTANDUP = "Base Layer.OStandup";

        public const string FULLNAME_SDAMAGEL = "Base Layer.SDamageL";
        public const string FULLNAME_SDAMAGEM = "Base Layer.SDamageM";
        public const string FULLNAME_SDAMAGEH = "Base Layer.SDamageH";

        public const string FULLNAME_JDAMAGEL = "Base Layer.JDamageL";
        public const string FULLNAME_JDAMAGEM = "Base Layer.JDamageM";
        public const string FULLNAME_JDAMAGEH = "Base Layer.JDamageH";

        public const string FULLNAME_DDAMAGEL = "Base Layer.DDamageL";
        public const string FULLNAME_DDAMAGEM = "Base Layer.DDamageM";
        public const string FULLNAME_DDAMAGEH = "Base Layer.DDamageH";

        public const string FULLNAME_CDAMAGEL = "Base Layer.CDamageL";
        public const string FULLNAME_CDAMAGEM = "Base Layer.CDamageM";
        public const string FULLNAME_CDAMAGEH = "Base Layer.CDamageH";

        static StateExTable()
        {
            Instance = new StateExTable();
        }

        public static StateExTable Instance { get; set; }

        private StateExTable()
        {
            fullpath_to_index = new Dictionary<string, int>()
            {
                { FULLNAME_SWAIT,0 },
                { FULLNAME_SMOVE,1 },
                { FULLNAME_SBLOCKL,2 },
                { FULLNAME_SBLOCKM,3 },
                { FULLNAME_SBLOCKH,4 },
                { FULLNAME_SATKLP,5 },
                { FULLNAME_SATKMP,6 },
                { FULLNAME_SATKHP,7 },
                { FULLNAME_SATKLK,8 },
                { FULLNAME_SATKMK,9 },
                { FULLNAME_SATKHK,10 },

                { FULLNAME_OBACKSTEP,11 },

                { FULLNAME_JBLOCKL,12 },
                { FULLNAME_JBLOCKM,13 },
                { FULLNAME_JBLOCKH,14 },
                { FULLNAME_JATKLP,15 },
                { FULLNAME_JATKMP,16 },
                { FULLNAME_JATKHP,17 },
                { FULLNAME_JATKLK,18 },
                { FULLNAME_JATKMK,19 },
                { FULLNAME_JATKHK,20 },

                { FULLNAME_JMOVE0,21 },
                { FULLNAME_JMOVE1,22 },
                { FULLNAME_JMOVE2,23 },
                { FULLNAME_JMOVE3,24 },
                { FULLNAME_JMOVE4,25 },

                { FULLNAME_DBLOCKL,26 },
                { FULLNAME_DBLOCKM,27 },
                { FULLNAME_DBLOCKH,28 },
                { FULLNAME_DATKLP,29 },
                { FULLNAME_DATKMP,30 },
                { FULLNAME_DATKHP,31 },
                { FULLNAME_DATKLK,32 },
                { FULLNAME_DATKMK,33 },
                { FULLNAME_DATKHK,34 },

                { FULLNAME_DMOVE,35 },

                { FULLNAME_CBLOCKL,36 },
                { FULLNAME_CBLOCKM,37 },
                { FULLNAME_CBLOCKH,38 },
                { FULLNAME_CATKLP,39 },
                { FULLNAME_CATKMP,40 },
                { FULLNAME_CATKHP,41 },
                { FULLNAME_CATKLK,42 },
                { FULLNAME_CATKMK,43 },
                { FULLNAME_CATKHK,44 },

                { FULLNAME_CWAIT,45 },
                { FULLNAME_CMOVE,46 },

                { FULLNAME_OGIVEUP,47 },
                { FULLNAME_ODOWN_SDAMAGEH,48 },
                { FULLNAME_ODOWN,49 },
                { FULLNAME_OSTANDUP,50 },

                { FULLNAME_SDAMAGEL,51 },
                { FULLNAME_SDAMAGEM,52 },
                { FULLNAME_SDAMAGEH,53 },

                { FULLNAME_JDAMAGEL,54 },
                { FULLNAME_JDAMAGEM,55 },
                { FULLNAME_JDAMAGEH,56 },

                { FULLNAME_DDAMAGEL,57 },
                { FULLNAME_DDAMAGEM,58 },
                { FULLNAME_DDAMAGEH,59 },

                { FULLNAME_CDAMAGEL,60 },
                { FULLNAME_CDAMAGEM,61 },
                { FULLNAME_CDAMAGEH,62 },
            };
            index_to_exRecord = new Dictionary<int, StateExRecordable>()//[ステートインデックス]
            {
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_SWAIT], StateExRecord.Build(  "Base Layer.SWait", CliptypeIndex.SWait,Attr.None)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_SMOVE],  StateExRecord.Build(  "Base Layer.SMove", CliptypeIndex.SMove,Attr.None)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_SBLOCKL],  StateExRecord.Build(  "Base Layer.SBlockL", CliptypeIndex.SBlockL, Attr.BusyX | Attr.Block)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_SBLOCKM],  StateExRecord.Build(  "Base Layer.SBlockM", CliptypeIndex.SBlockM, Attr.BusyX | Attr.Block)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_SBLOCKH],  StateExRecord.Build(  "Base Layer.SBlockH", CliptypeIndex.SBlockH, Attr.BusyX | Attr.Block)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_SATKLP],  StateExRecord.Build(  "Base Layer.SAtkLP", CliptypeIndex.SAtkLP,Attr.BusyX)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_SATKMP],  StateExRecord.Build(  "Base Layer.SAtkMP", CliptypeIndex.SAtkMP,Attr.BusyX)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_SATKHP],  StateExRecord.Build( "Base Layer.SAtkHP",  CliptypeIndex.SAtkHP,Attr.BusyX)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_SATKLK],  StateExRecord.Build(  "Base Layer.SAtkLK", CliptypeIndex.SAtkLK,Attr.BusyX)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_SATKMK],  StateExRecord.Build(  "Base Layer.SAtkMK", CliptypeIndex.SAtkMK,Attr.BusyX)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_SATKHK],  StateExRecord.Build(  "Base Layer.SAtkHK", CliptypeIndex.SAtkHK,Attr.BusyX)},

                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_OBACKSTEP],  StateExRecord.Build(  "Base Layer.OBackstep", CliptypeIndex.OBackstep,Attr.None)},

                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_JBLOCKL],  StateExRecord.Build(  "Base Layer.JBlockL", CliptypeIndex.JBlockL, Attr.BusyX | Attr.Block)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_JBLOCKM],  StateExRecord.Build(  "Base Layer.JBlockM", CliptypeIndex.JBlockM, Attr.BusyX | Attr.Block)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_JBLOCKH],  StateExRecord.Build(  "Base Layer.JBlockH", CliptypeIndex.JBlockH, Attr.BusyX | Attr.Block)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_JATKLP],  StateExRecord.Build(  "Base Layer.JAtkLP", CliptypeIndex.JAtkLP,Attr.None)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_JATKMP],  StateExRecord.Build(  "Base Layer.JAtkMP", CliptypeIndex.JAtkMP,Attr.None)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_JATKHP],  StateExRecord.Build(  "Base Layer.JAtkHP", CliptypeIndex.JAtkHP,Attr.None)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_JATKLK],  StateExRecord.Build(  "Base Layer.JAtkLK", CliptypeIndex.JAtkLK,Attr.None)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_JATKMK],  StateExRecord.Build( "Base Layer.JAtkMK",  CliptypeIndex.JAtkMK,Attr.None)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_JATKHK],  StateExRecord.Build(  "Base Layer.JAtkHK", CliptypeIndex.JAtkHK,Attr.None)},

                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_JMOVE0],  StateExRecord.Build(  "Base Layer.JMove.JMove0", CliptypeIndex.JMove0,Attr.BusyX | Attr.BusyY)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_JMOVE1],  StateExRecord.Build(  "Base Layer.JMove.JMove1", CliptypeIndex.JMove1,Attr.None)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_JMOVE2],  StateExRecord.Build( "Base Layer.JMove.JMove2",  CliptypeIndex.JMove2,Attr.None)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_JMOVE3],  StateExRecord.Build(  "Base Layer.JMove.JMove3", CliptypeIndex.JMove3,Attr.None)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_JMOVE4],  StateExRecord.Build(  "Base Layer.JMove.JMove4", CliptypeIndex.JMove4,Attr.BusyX)},

                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_DBLOCKL],  StateExRecord.Build(  "Base Layer.DBlockL", CliptypeIndex.DBlockL, Attr.BusyX | Attr.Block)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_DBLOCKM],  StateExRecord.Build(  "Base Layer.DBlockM", CliptypeIndex.DBlockM, Attr.BusyX | Attr.Block)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_DBLOCKH],  StateExRecord.Build(  "Base Layer.DBlockH", CliptypeIndex.DBlockH, Attr.BusyX | Attr.Block)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_DATKLP],  StateExRecord.Build(  "Base Layer.DAtkLP", CliptypeIndex.DAtkLP,Attr.None)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_DATKMP],  StateExRecord.Build(  "Base Layer.DAtkMP", CliptypeIndex.DAtkMP,Attr.None)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_DATKHP],  StateExRecord.Build(  "Base Layer.DAtkHP", CliptypeIndex.DAtkHP,Attr.None)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_DATKLK],  StateExRecord.Build(  "Base Layer.DAtkLK", CliptypeIndex.DAtkLK,Attr.None)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_DATKMK],  StateExRecord.Build(  "Base Layer.DAtkMK", CliptypeIndex.DAtkMK,Attr.None)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_DATKHK],  StateExRecord.Build(  "Base Layer.DAtkHK", CliptypeIndex.DAtkHK,Attr.None)},

                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_DMOVE],  StateExRecord.Build(  "Base Layer.DMove", CliptypeIndex.DMove,Attr.None)},

                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_CBLOCKL],  StateExRecord.Build(  "Base Layer.CBlockL", CliptypeIndex.CBlockL, Attr.BusyX | Attr.Block)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_CBLOCKM],  StateExRecord.Build(  "Base Layer.CBlockM", CliptypeIndex.CBlockM, Attr.BusyX | Attr.Block)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_CBLOCKH],  StateExRecord.Build(  "Base Layer.CBlockH", CliptypeIndex.CBlockH, Attr.BusyX | Attr.Block)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_CATKLP],  StateExRecord.Build(  "Base Layer.CAtkLP", CliptypeIndex.CAtkLP,Attr.BusyX)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_CATKMP],  StateExRecord.Build(  "Base Layer.CAtkMP", CliptypeIndex.CAtkMP,Attr.BusyX)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_CATKHP],  StateExRecord.Build(  "Base Layer.CAtkHP", CliptypeIndex.CAtkHP,Attr.BusyX)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_CATKLK],  StateExRecord.Build(  "Base Layer.CAtkLK", CliptypeIndex.CAtkLK,Attr.BusyX)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_CATKMK],  StateExRecord.Build(  "Base Layer.CAtkMK", CliptypeIndex.CAtkMK,Attr.BusyX)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_CATKHK],  StateExRecord.Build(  "Base Layer.CAtkHK", CliptypeIndex.CAtkHK,Attr.BusyX)},

                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_CWAIT],  StateExRecord.Build(  "Base Layer.CWait", CliptypeIndex.CWait,Attr.None)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_CMOVE],  StateExRecord.Build(  "Base Layer.CMove", CliptypeIndex.CMove,Attr.None)},

                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_OGIVEUP],  StateExRecord.Build(  "Base Layer.OGiveup", CliptypeIndex.OGiveup,Attr.None)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_ODOWN_SDAMAGEH],  StateExRecord.Build(  "Base Layer.ODown_SDamageH", CliptypeIndex.SDamageH,Attr.BusyX)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_ODOWN],  StateExRecord.Build(  "Base Layer.ODown", CliptypeIndex.ODown,Attr.BusyX)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_OSTANDUP],  StateExRecord.Build(  "Base Layer.OStandup", CliptypeIndex.OStandup,Attr.None)},

                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_SDAMAGEL],  StateExRecord.Build(  "Base Layer.SDamageL", CliptypeIndex.SDamageL,Attr.None)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_SDAMAGEM],  StateExRecord.Build(  "Base Layer.SDamageM", CliptypeIndex.SDamageM,Attr.None)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_SDAMAGEH],  StateExRecord.Build(  "Base Layer.SDamageH", CliptypeIndex.SDamageH,Attr.None)},

                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_JDAMAGEL],  StateExRecord.Build(  "Base Layer.JDamageL", CliptypeIndex.JDamageL,Attr.None)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_JDAMAGEM],  StateExRecord.Build(  "Base Layer.JDamageM", CliptypeIndex.JDamageM,Attr.None)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_JDAMAGEH],  StateExRecord.Build(  "Base Layer.JDamageH", CliptypeIndex.JDamageH,Attr.None)},

                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_DDAMAGEL],  StateExRecord.Build(  "Base Layer.DDamageL", CliptypeIndex.DDamageL,Attr.None)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_DDAMAGEM],  StateExRecord.Build(  "Base Layer.DDamageM", CliptypeIndex.DDamageM,Attr.None)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_DDAMAGEH],  StateExRecord.Build(  "Base Layer.DDamageH", CliptypeIndex.DDamageH,Attr.None)},

                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_CDAMAGEL],  StateExRecord.Build(  "Base Layer.CDamageL", CliptypeIndex.CDamageL,Attr.None)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_CDAMAGEM],  StateExRecord.Build(  "Base Layer.CDamageM", CliptypeIndex.CDamageM,Attr.None)},
                {StateExTable.fullpath_to_index[StateExTable.FULLNAME_CDAMAGEH],  StateExRecord.Build(  "Base Layer.CDamageH", CliptypeIndex.CDamageH,Attr.None)},
            };
        }

        /// <summary>
        /// キャラクターと、モーション、現在のフレームを指定することで、通し画像番号とスライス番号を返す。
        /// これにより Hitbox2DScript と連携を取ることができる。
        /// </summary>
        /// <param name="serialTilesetfileIndex"></param>
        /// <param name="slice"></param>
        /// <param name="character"></param>
        /// <param name="motion"></param>
        /// <param name="currentMotionFrame"></param>
        public static void GetSlice(out int serialTilesetfileIndex, out int slice, CharacterIndex character, CliptypeExRecordable cliptypeExRecord, int currentMotionFrame)
        {
            serialTilesetfileIndex = Hitbox2DOperationScript.GetSerialImageIndex(character, (TilesetfileTypeIndex)cliptypeExRecord.TilesetfileTypeIndex);
            slice = cliptypeExRecord.Slices[currentMotionFrame];
        }
    }
}

