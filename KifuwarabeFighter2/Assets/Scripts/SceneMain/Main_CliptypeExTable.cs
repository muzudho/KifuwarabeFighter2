using System.Collections.Generic;
using StellaQL;

namespace SceneMain
{
    /// <summary>
    /// AnimationClip の種類に一対一対応☆
    /// </summary>
    public enum CliptypeIndex
    {
        SWait,
        SAtkLP,
        SAtkMP,
        SAtkHP,
        SAtkLK,
        SAtkMK,
        SAtkHK,
        JMove0,
        JMove1,
        JMove2,
        JMove3,
        JMove4,
        DMove,
        OBackstep,
        SDamageM,
        SDamageL,
        SDamageH,
        ODown,
        OStandup,
        OGiveup,
        SMove,
        SBlockL,
        SBlockM,
        SBlockH,
        JAtkLP,
        JAtkMP,
        JAtkHP,
        JAtkLK,
        JAtkMK,
        JAtkHK,
        JBlockL,
        JBlockM,
        JBlockH,
        JDamageL,
        JDamageM,
        JDamageH,
        DAtkLP,
        DAtkMP,
        DAtkHP,
        DAtkLK,
        DAtkMK,
        DAtkHK,
        DBlockL,
        DBlockM,
        DBlockH,
        DDamageL,
        DDamageM,
        DDamageH,
        CWait,
        CMove,
        CAtkLP,
        CAtkMP,
        CAtkHP,
        CAtkLK,
        CAtkMK,
        CAtkHK,
        CBlockL,
        CBlockM,
        CBlockH,
        CDamageL,
        CDamageM,
        CDamageH,
        Num,
    }

    /// <summary>
    /// スライスする画像の元となるファイルのインデックス。
    /// １キャラに付き、立ちモーション、ジャンプモーションなどに画像を分けていると想定。
    /// キャラクターの行動、キャラクター画像とも対応。
    /// Unityのテキストボックスで数字を直打ちしているので、対応する数字も保つこと。
    /// 旧名： ActioningIndex
    /// </summary>
    public enum TilesetfileTypeIndex
    {
        /// <summary>
        /// [0]
        /// </summary>
        Stand,
        Jump,
        Dash,
        Crouch,
        Other,
        /// <summary>
        /// 列挙型の要素数、または未使用の値として使用。
        /// </summary>
        Num
    }

    /// <summary>
    /// アニメーション・クリップの種類
    /// （キャラクター＠モーション名　となっているとして、モーション名の部分）
    /// </summary>
    public class CliptypeExRecord : AbstractCliptypeExRecord
    {
        public CliptypeExRecord(int[] slices, TilesetfileTypeIndex actioning) : base(slices, (int)actioning)
        {

        }
    }

    public class CliptypeExTable : AbstractCliptypeExTable
    {
        static CliptypeExTable()
        {
            Instance = new CliptypeExTable();
        }

        public static CliptypeExTable Instance { get; set; }

        CliptypeExTable()
        {
            Cliptype_to_exRecord = new Dictionary<int, CliptypeExRecordable>()//[CliptypeIndex]
            {
                { (int)CliptypeIndex.SWait, new CliptypeExRecord( new int[] { 0,1,2,3 } ,TilesetfileTypeIndex.Stand)},
                { (int)CliptypeIndex.SMove, new CliptypeExRecord( new int[] { 4,5,6,7 } ,TilesetfileTypeIndex.Stand)},
                { (int)CliptypeIndex.SBlockL, new CliptypeExRecord( new int[] { 8,9 } ,TilesetfileTypeIndex.Stand)},
                { (int)CliptypeIndex.SBlockM, new CliptypeExRecord( new int[] { 8,9,10 } ,TilesetfileTypeIndex.Stand)},
                { (int)CliptypeIndex.SBlockH, new CliptypeExRecord( new int[] { 8,9,10,11,9 } ,TilesetfileTypeIndex.Stand)},
                { (int)CliptypeIndex.SDamageL, new CliptypeExRecord( new int[] { 12,13 } ,TilesetfileTypeIndex.Stand)},
                { (int)CliptypeIndex.SDamageM, new CliptypeExRecord( new int[] { 12,13,14 } ,TilesetfileTypeIndex.Stand)},
                { (int)CliptypeIndex.SDamageH, new CliptypeExRecord( new int[] { 12, 13, 14,15,13 } ,TilesetfileTypeIndex.Stand)},
                { (int)CliptypeIndex.SAtkLP,new CliptypeExRecord( new int[] { 16,17 },TilesetfileTypeIndex.Stand)},
                { (int)CliptypeIndex.SAtkMP,new CliptypeExRecord( new int[] { 16, 17,18 },TilesetfileTypeIndex.Stand)},
                { (int)CliptypeIndex.SAtkHP,new CliptypeExRecord( new int[] { 16, 17, 18,19,17 },TilesetfileTypeIndex.Stand)},
                { (int)CliptypeIndex.SAtkLK,new CliptypeExRecord( new int[] { 20,21 },TilesetfileTypeIndex.Stand)},
                { (int)CliptypeIndex.SAtkMK,new CliptypeExRecord( new int[] { 20, 21, 22 },TilesetfileTypeIndex.Stand)},
                { (int)CliptypeIndex.SAtkHK,new CliptypeExRecord( new int[] { 20, 21, 22, 23,21 },TilesetfileTypeIndex.Stand)},

                { (int)CliptypeIndex.JMove0, new CliptypeExRecord( new int[] { 0,1 },TilesetfileTypeIndex.Jump) },
                { (int)CliptypeIndex.JMove1, new CliptypeExRecord( new int[] { 2,3 },TilesetfileTypeIndex.Jump) },
                { (int)CliptypeIndex.JMove2, new CliptypeExRecord( new int[] { 4 },TilesetfileTypeIndex.Jump) },
                { (int)CliptypeIndex.JMove3, new CliptypeExRecord( new int[] { 5,6 },TilesetfileTypeIndex.Jump) },
                { (int)CliptypeIndex.JMove4, new CliptypeExRecord( new int[] { 7 },TilesetfileTypeIndex.Jump) },
                { (int)CliptypeIndex.JBlockL, new CliptypeExRecord( new int[] { 8,9 },TilesetfileTypeIndex.Jump) },
                { (int)CliptypeIndex.JBlockM, new CliptypeExRecord( new int[] { 8,9,10 },TilesetfileTypeIndex.Jump) },
                { (int)CliptypeIndex.JBlockH, new CliptypeExRecord( new int[] { 8,9,10,11,9 },TilesetfileTypeIndex.Jump) },
                { (int)CliptypeIndex.JDamageL, new CliptypeExRecord( new int[] { 12,13 },TilesetfileTypeIndex.Jump) },
                { (int)CliptypeIndex.JDamageM, new CliptypeExRecord( new int[] { 12,13,14 },TilesetfileTypeIndex.Jump) },
                { (int)CliptypeIndex.JDamageH, new CliptypeExRecord( new int[] { 12, 13, 14,15,13 },TilesetfileTypeIndex.Jump) },
                { (int)CliptypeIndex.JAtkLP,new CliptypeExRecord( new int[] { 16,17 },TilesetfileTypeIndex.Jump)},
                { (int)CliptypeIndex.JAtkMP,new CliptypeExRecord( new int[] { 16, 17,18 },TilesetfileTypeIndex.Jump)},
                { (int)CliptypeIndex.JAtkHP,new CliptypeExRecord( new int[] { 16, 17, 18,19,17 },TilesetfileTypeIndex.Jump)},
                { (int)CliptypeIndex.JAtkLK,new CliptypeExRecord( new int[] { 20,21 },TilesetfileTypeIndex.Jump)},
                { (int)CliptypeIndex.JAtkMK,new CliptypeExRecord( new int[] { 20, 21, 22 },TilesetfileTypeIndex.Jump)},
                { (int)CliptypeIndex.JAtkHK,new CliptypeExRecord( new int[] { 20, 21, 22, 23,21 },TilesetfileTypeIndex.Jump)},

                { (int)CliptypeIndex.DMove, new CliptypeExRecord( new int[] { 0,1,2,3,4,5,6,7 },TilesetfileTypeIndex.Dash) },
                { (int)CliptypeIndex.DBlockL, new CliptypeExRecord( new int[] { 8,9 },TilesetfileTypeIndex.Dash) },
                { (int)CliptypeIndex.DBlockM, new CliptypeExRecord( new int[] { 8,9,10 },TilesetfileTypeIndex.Dash) },
                { (int)CliptypeIndex.DBlockH, new CliptypeExRecord( new int[] { 8,9,10,11,9 },TilesetfileTypeIndex.Dash) },
                { (int)CliptypeIndex.DDamageL, new CliptypeExRecord( new int[] { 12,13 },TilesetfileTypeIndex.Dash) },
                { (int)CliptypeIndex.DDamageM, new CliptypeExRecord( new int[] { 12,13,14 },TilesetfileTypeIndex.Dash) },
                { (int)CliptypeIndex.DDamageH, new CliptypeExRecord( new int[] { 12, 13, 14,15,13 },TilesetfileTypeIndex.Dash) },
                { (int)CliptypeIndex.DAtkLP,new CliptypeExRecord( new int[] { 16,17 },TilesetfileTypeIndex.Dash)},
                { (int)CliptypeIndex.DAtkMP,new CliptypeExRecord( new int[] { 16, 17,18 },TilesetfileTypeIndex.Dash)},
                { (int)CliptypeIndex.DAtkHP,new CliptypeExRecord( new int[] { 16, 17, 18,19,17 },TilesetfileTypeIndex.Dash)},
                { (int)CliptypeIndex.DAtkLK,new CliptypeExRecord( new int[] { 20,21 },TilesetfileTypeIndex.Dash)},
                { (int)CliptypeIndex.DAtkMK,new CliptypeExRecord( new int[] { 20, 21, 22 },TilesetfileTypeIndex.Dash)},
                { (int)CliptypeIndex.DAtkHK,new CliptypeExRecord( new int[] { 20, 21, 22, 23,21 },TilesetfileTypeIndex.Dash)},

                { (int)CliptypeIndex.CWait,new CliptypeExRecord( new int[] { 0,1,2,3 },TilesetfileTypeIndex.Crouch)},
                { (int)CliptypeIndex.CMove,new CliptypeExRecord( new int[] { 4,5,6,7 },TilesetfileTypeIndex.Crouch)},
                { (int)CliptypeIndex.CBlockL, new CliptypeExRecord( new int[] { 8,9 },TilesetfileTypeIndex.Crouch) },
                { (int)CliptypeIndex.CBlockM, new CliptypeExRecord( new int[] { 8,9,10 },TilesetfileTypeIndex.Crouch) },
                { (int)CliptypeIndex.CBlockH, new CliptypeExRecord( new int[] { 8,9,10,11,9 },TilesetfileTypeIndex.Crouch) },
                { (int)CliptypeIndex.CDamageL, new CliptypeExRecord( new int[] { 12,13 },TilesetfileTypeIndex.Crouch) },
                { (int)CliptypeIndex.CDamageM, new CliptypeExRecord( new int[] { 12,13,14 },TilesetfileTypeIndex.Crouch) },
                { (int)CliptypeIndex.CDamageH, new CliptypeExRecord( new int[] { 12, 13, 14,15,13 },TilesetfileTypeIndex.Crouch) },
                { (int)CliptypeIndex.CAtkLP,new CliptypeExRecord( new int[] { 16,17 },TilesetfileTypeIndex.Crouch)},
                { (int)CliptypeIndex.CAtkMP,new CliptypeExRecord( new int[] { 16, 17,18 },TilesetfileTypeIndex.Crouch)},
                { (int)CliptypeIndex.CAtkHP,new CliptypeExRecord( new int[] { 16, 17, 18,19,17 },TilesetfileTypeIndex.Crouch)},
                { (int)CliptypeIndex.CAtkLK,new CliptypeExRecord( new int[] { 20,21 },TilesetfileTypeIndex.Crouch)},
                { (int)CliptypeIndex.CAtkMK,new CliptypeExRecord( new int[] { 20, 21, 22 },TilesetfileTypeIndex.Crouch)},
                { (int)CliptypeIndex.CAtkHK,new CliptypeExRecord( new int[] { 20, 21, 22, 23,21 },TilesetfileTypeIndex.Crouch)},

                { (int)CliptypeIndex.OBackstep,new CliptypeExRecord( new int[] { 0,1,2,3,4,5,6,7 },TilesetfileTypeIndex.Other)},
                { (int)CliptypeIndex.ODown,new CliptypeExRecord( new int[] { 8,9 },TilesetfileTypeIndex.Other)},
                { (int)CliptypeIndex.OStandup,new CliptypeExRecord( new int[] { 10,11 },TilesetfileTypeIndex.Other)},
                { (int)CliptypeIndex.OGiveup,new CliptypeExRecord( new int[] { 12,13,14,15 },TilesetfileTypeIndex.Other)},
            };
        }
    }
}
