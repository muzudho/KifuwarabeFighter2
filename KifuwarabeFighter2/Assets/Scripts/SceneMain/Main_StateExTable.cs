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
            return new StateExRecord(fullpath, Animator.StringToHash(fullpath), (int)attributeEnum);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="cliptype"></param>
        /// <param name="attributeEnum"></param>
        /// <returns></returns>
        public static StateExRecord Build(string fullpath, CliptypeIndex cliptype, StateExTable.Attr attributeEnum)
        {
            return new StateExRecord(fullpath, Animator.StringToHash(fullpath), cliptype, (int)attributeEnum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="fullpathHash"></param>
        /// <param name="attributeEnum">StateExTable.Attr 列挙型にはインターフェースが付けられないので拡張できず不便。int 型にしておくと拡張できる。</param>
        public StateExRecord(string fullpath, int fullpathHash, int attributeEnum) :base(fullpath, fullpathHash, attributeEnum)
        {
            this.Cliptype = -1; // クリップタイプを使わない場合
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="fullpathHash"></param>
        /// <param name="cliptype"></param>
        /// <param name="attributeEnum">StateExTable.Attr 列挙型にはインターフェースが付けられないので拡張できず不便。int 型にしておくと拡張できる。</param>
        public StateExRecord(string fullpath, int fullpathHash, CliptypeIndex cliptype, int attributeEnum):base(fullpath, fullpathHash, attributeEnum)
        {
            this.Cliptype = (int)cliptype;
        }

        public override bool HasFlag_attr(int enumration)
        {
            //Debug.Log("InFlag[" + ((AstateDatabase.Attr)enumration).HasFlag(this.attribute) + "] = [" + ((AstateDatabase.Attr)enumration) + "].HasFlag(" + this.attribute + ")");
            //return ((AstateDatabase.Attr)enumration).HasFlag(this.attribute);

            //Debug.Log("InFlag[" + ((StateExTable.Attr)this.AttributeEnum).HasFlag((StateExTable.Attr)enumration) + "] = [" + this.AttributeEnum + "].HasFlag(" + ((StateExTable.Attr)enumration) + ")");
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
            BusyY = BusyX << 1,
            /// <summary>
            /// ブロック・モーションなら
            /// </summary>
            Block = BusyY << 1,
            /// <summary>
            /// 立ちなら
            /// </summary>
            Stand = Block << 1,
        }

        #region フルパス一覧
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
        #endregion

        static StateExTable()
        {
            Instance = new StateExTable();
        }

        public static StateExTable Instance { get; set; }
        public override Type GetAttributeEnumration() { return typeof(StateExTable.Attr); }

        protected StateExTable()
        {
            List<StateExRecord> temp = new List<StateExRecord>()
            {
                StateExRecord.Build(  StateExTable.FULLNAME_SWAIT, CliptypeIndex.SWait,Attr.None),
                StateExRecord.Build(  StateExTable.FULLNAME_SMOVE, CliptypeIndex.SMove,Attr.None),
                StateExRecord.Build(  StateExTable.FULLNAME_SBLOCKL, CliptypeIndex.SBlockL, Attr.BusyX | Attr.Block),
                StateExRecord.Build(  StateExTable.FULLNAME_SBLOCKM, CliptypeIndex.SBlockM, Attr.BusyX | Attr.Block),
                StateExRecord.Build(  StateExTable.FULLNAME_SBLOCKH, CliptypeIndex.SBlockH, Attr.BusyX | Attr.Block),
                StateExRecord.Build(  StateExTable.FULLNAME_SATKLP, CliptypeIndex.SAtkLP,Attr.BusyX),
                StateExRecord.Build(  StateExTable.FULLNAME_SATKMP, CliptypeIndex.SAtkMP,Attr.BusyX),
                StateExRecord.Build( StateExTable.FULLNAME_SATKHP,  CliptypeIndex.SAtkHP,Attr.BusyX),
                StateExRecord.Build(  StateExTable.FULLNAME_SATKLK, CliptypeIndex.SAtkLK,Attr.BusyX),
                StateExRecord.Build(  StateExTable.FULLNAME_SATKMK, CliptypeIndex.SAtkMK,Attr.BusyX),
                StateExRecord.Build(  StateExTable.FULLNAME_SATKHK, CliptypeIndex.SAtkHK,Attr.BusyX),

                StateExRecord.Build(  StateExTable.FULLNAME_OBACKSTEP, CliptypeIndex.OBackstep,Attr.None),

                StateExRecord.Build(  StateExTable.FULLNAME_JBLOCKL, CliptypeIndex.JBlockL, Attr.BusyX | Attr.Block),
                StateExRecord.Build(  StateExTable.FULLNAME_JBLOCKM, CliptypeIndex.JBlockM, Attr.BusyX | Attr.Block),
                StateExRecord.Build(  StateExTable.FULLNAME_JBLOCKH, CliptypeIndex.JBlockH, Attr.BusyX | Attr.Block),
                StateExRecord.Build(  StateExTable.FULLNAME_JATKLP, CliptypeIndex.JAtkLP,Attr.None),
                StateExRecord.Build(  StateExTable.FULLNAME_JATKMP, CliptypeIndex.JAtkMP,Attr.None),
                StateExRecord.Build(  StateExTable.FULLNAME_JATKHP, CliptypeIndex.JAtkHP,Attr.None),
                StateExRecord.Build( StateExTable.FULLNAME_JATKLK, CliptypeIndex.JAtkLK,Attr.None),
                StateExRecord.Build( StateExTable.FULLNAME_JATKMK,  CliptypeIndex.JAtkMK,Attr.None),
                StateExRecord.Build(  StateExTable.FULLNAME_JATKHK, CliptypeIndex.JAtkHK,Attr.None),

                StateExRecord.Build(  StateExTable.FULLNAME_JMOVE0, CliptypeIndex.JMove0,Attr.BusyX | Attr.BusyY),
                StateExRecord.Build(  StateExTable.FULLNAME_JMOVE1, CliptypeIndex.JMove1,Attr.None),
                StateExRecord.Build( StateExTable.FULLNAME_JMOVE2,  CliptypeIndex.JMove2,Attr.None),
                StateExRecord.Build(  StateExTable.FULLNAME_JMOVE3, CliptypeIndex.JMove3,Attr.None),
                StateExRecord.Build( StateExTable.FULLNAME_JMOVE4, CliptypeIndex.JMove4,Attr.BusyX),

                StateExRecord.Build(  StateExTable.FULLNAME_DBLOCKL, CliptypeIndex.DBlockL, Attr.BusyX | Attr.Block),
                StateExRecord.Build(  StateExTable.FULLNAME_DBLOCKM, CliptypeIndex.DBlockM, Attr.BusyX | Attr.Block),
                StateExRecord.Build(  StateExTable.FULLNAME_DBLOCKH, CliptypeIndex.DBlockH, Attr.BusyX | Attr.Block),
                StateExRecord.Build(  StateExTable.FULLNAME_DATKLP, CliptypeIndex.DAtkLP,Attr.None),
                StateExRecord.Build(  StateExTable.FULLNAME_DATKMP, CliptypeIndex.DAtkMP,Attr.None),
                StateExRecord.Build(  StateExTable.FULLNAME_DATKHP, CliptypeIndex.DAtkHP,Attr.None),
                StateExRecord.Build(  StateExTable.FULLNAME_DATKLK, CliptypeIndex.DAtkLK,Attr.None),
                StateExRecord.Build(  StateExTable.FULLNAME_DATKMK, CliptypeIndex.DAtkMK,Attr.None),
                StateExRecord.Build(  StateExTable.FULLNAME_DATKHK, CliptypeIndex.DAtkHK,Attr.None),

                StateExRecord.Build(  StateExTable.FULLNAME_DMOVE, CliptypeIndex.DMove,Attr.None),

                StateExRecord.Build(  StateExTable.FULLNAME_CBLOCKL, CliptypeIndex.CBlockL, Attr.BusyX | Attr.Block),
                StateExRecord.Build(  StateExTable.FULLNAME_CBLOCKM, CliptypeIndex.CBlockM, Attr.BusyX | Attr.Block),
                StateExRecord.Build(  StateExTable.FULLNAME_CBLOCKH, CliptypeIndex.CBlockH, Attr.BusyX | Attr.Block),
                StateExRecord.Build(  StateExTable.FULLNAME_CATKLP, CliptypeIndex.CAtkLP,Attr.BusyX),
                StateExRecord.Build(  StateExTable.FULLNAME_CATKMP, CliptypeIndex.CAtkMP,Attr.BusyX),
                StateExRecord.Build(  StateExTable.FULLNAME_CATKHP, CliptypeIndex.CAtkHP,Attr.BusyX),
                StateExRecord.Build( StateExTable.FULLNAME_CATKLK, CliptypeIndex.CAtkLK,Attr.BusyX),
                StateExRecord.Build( StateExTable.FULLNAME_CATKMK, CliptypeIndex.CAtkMK,Attr.BusyX),
                StateExRecord.Build(  StateExTable.FULLNAME_CATKHK, CliptypeIndex.CAtkHK,Attr.BusyX),

                StateExRecord.Build(  StateExTable.FULLNAME_CWAIT, CliptypeIndex.CWait,Attr.None),
                StateExRecord.Build(  StateExTable.FULLNAME_CMOVE, CliptypeIndex.CMove,Attr.None),

                StateExRecord.Build(  StateExTable.FULLNAME_OGIVEUP, CliptypeIndex.OGiveup,Attr.None),
                StateExRecord.Build(  StateExTable.FULLNAME_ODOWN_SDAMAGEH, CliptypeIndex.SDamageH,Attr.BusyX),
                StateExRecord.Build(  StateExTable.FULLNAME_ODOWN, CliptypeIndex.ODown,Attr.BusyX),
                StateExRecord.Build(  StateExTable.FULLNAME_OSTANDUP, CliptypeIndex.OStandup,Attr.None),

                StateExRecord.Build(  StateExTable.FULLNAME_SDAMAGEL, CliptypeIndex.SDamageL,Attr.None),
                StateExRecord.Build(  StateExTable.FULLNAME_SDAMAGEM, CliptypeIndex.SDamageM,Attr.None),
                StateExRecord.Build(  StateExTable.FULLNAME_SDAMAGEH, CliptypeIndex.SDamageH,Attr.None),

                StateExRecord.Build(  StateExTable.FULLNAME_JDAMAGEL, CliptypeIndex.JDamageL,Attr.None),
                StateExRecord.Build(  StateExTable.FULLNAME_JDAMAGEM, CliptypeIndex.JDamageM,Attr.None),
                StateExRecord.Build(  StateExTable.FULLNAME_JDAMAGEH, CliptypeIndex.JDamageH,Attr.None),

                StateExRecord.Build(  StateExTable.FULLNAME_DDAMAGEL, CliptypeIndex.DDamageL,Attr.None),
                StateExRecord.Build(  StateExTable.FULLNAME_DDAMAGEM, CliptypeIndex.DDamageM,Attr.None),
                StateExRecord.Build(  StateExTable.FULLNAME_DDAMAGEH, CliptypeIndex.DDamageH,Attr.None),

                StateExRecord.Build(  StateExTable.FULLNAME_CDAMAGEL, CliptypeIndex.CDamageL,Attr.None),
                StateExRecord.Build(  StateExTable.FULLNAME_CDAMAGEM, CliptypeIndex.CDamageM,Attr.None),
                StateExRecord.Build(  StateExTable.FULLNAME_CDAMAGEH, CliptypeIndex.CDamageH,Attr.None),
            };
            //hash_to_exRecord = new Dictionary<int, StateExRecordable>();
            foreach (StateExRecord record in temp) { Hash_to_exRecord.Add(record.FullPathHash, record); }
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

