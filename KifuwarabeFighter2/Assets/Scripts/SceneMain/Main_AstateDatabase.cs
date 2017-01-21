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
    public struct AstateRecord
    {
        public string breadCrumb;
        public string name;
        public AcliptypeIndex acliptype;
        public AstateDatabase.Attr attribute;

        public AstateRecord(string breadCrumb, string name, AcliptypeIndex acliptype, AstateDatabase.Attr attribute)
        {
            this.breadCrumb = breadCrumb;
            this.name = name;
            this.acliptype = acliptype;
            this.attribute = attribute;
        }
    }

    public abstract class AstateDatabase
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
            index_to_record = new Dictionary<AstateIndex, AstateRecord>()
            {
                {AstateIndex.SWait, new AstateRecord(  "Base Layer.", "SWait", AcliptypeIndex.SWait,Attr.None)},
                {AstateIndex.SMove,  new AstateRecord(  "Base Layer.", "SMove", AcliptypeIndex.SMove,Attr.None)},
                {AstateIndex.SBlockL,  new AstateRecord(  "Base Layer.", "SBlockL", AcliptypeIndex.SBlockL, Attr.BusyX | Attr.Block)},
                {AstateIndex.SBlockM,  new AstateRecord(  "Base Layer.", "SBlockM", AcliptypeIndex.SBlockM, Attr.BusyX | Attr.Block)},
                {AstateIndex.SBlockH,  new AstateRecord(  "Base Layer.", "SBlockH", AcliptypeIndex.SBlockH, Attr.BusyX | Attr.Block)},
                {AstateIndex.SAtkLP,  new AstateRecord(  "Base Layer.", "SAtkLP", AcliptypeIndex.SAtkLP,Attr.BusyX)},
                {AstateIndex.SAtkMP,  new AstateRecord(  "Base Layer.", "SAtkMP", AcliptypeIndex.SAtkMP,Attr.BusyX)},
                {AstateIndex.SAtkHP,  new AstateRecord( "Base Layer.", "SAtkHP",  AcliptypeIndex.SAtkHP,Attr.BusyX)},
                {AstateIndex.SAtkLK,  new AstateRecord(  "Base Layer.", "SAtkLK", AcliptypeIndex.SAtkLK,Attr.BusyX)},
                {AstateIndex.SAtkMK,  new AstateRecord(  "Base Layer.", "SAtkMK", AcliptypeIndex.SAtkMK,Attr.BusyX)},
                {AstateIndex.SAtkHK,  new AstateRecord(  "Base Layer.", "SAtkHK", AcliptypeIndex.SAtkHK,Attr.BusyX)},

                {AstateIndex.OBackstep,  new AstateRecord(  "Base Layer.", "OBackstep", AcliptypeIndex.OBackstep,Attr.None)},

                {AstateIndex.JBlockL,  new AstateRecord(  "Base Layer.", "JBlockL", AcliptypeIndex.JBlockL, Attr.BusyX | Attr.Block)},
                {AstateIndex.JBlockM,  new AstateRecord(  "Base Layer.", "JBlockM", AcliptypeIndex.JBlockM, Attr.BusyX | Attr.Block)},
                {AstateIndex.JBlockH,  new AstateRecord(  "Base Layer.", "JBlockH", AcliptypeIndex.JBlockH, Attr.BusyX | Attr.Block)},
                {AstateIndex.JAtkLP,  new AstateRecord(  "Base Layer.", "JAtkLP", AcliptypeIndex.JAtkLP,Attr.None)},
                {AstateIndex.JAtkMP,  new AstateRecord(  "Base Layer.", "JAtkMP", AcliptypeIndex.JAtkMP,Attr.None)},
                {AstateIndex.JAtkHP,  new AstateRecord(  "Base Layer.", "JAtkHP", AcliptypeIndex.JAtkHP,Attr.None)},
                {AstateIndex.JAtkLK,  new AstateRecord(  "Base Layer.", "JAtkLK", AcliptypeIndex.JAtkLK,Attr.None)},
                {AstateIndex.JAtkMK,  new AstateRecord( "Base Layer.", "JAtkMK",  AcliptypeIndex.JAtkMK,Attr.None)},
                {AstateIndex.JAtkHK,  new AstateRecord(  "Base Layer.", "JAtkHK", AcliptypeIndex.JAtkHK,Attr.None)},

                {AstateIndex.JMove0,  new AstateRecord(  "Base Layer.JMove.", "JMove0", AcliptypeIndex.JMove0,Attr.BusyX | Attr.BusyY)},
                {AstateIndex.JMove1,  new AstateRecord(  "Base Layer.JMove.", "JMove1", AcliptypeIndex.JMove1,Attr.None)},
                {AstateIndex.JMove2,  new AstateRecord( "Base Layer.JMove.", "JMove2",  AcliptypeIndex.JMove2,Attr.None)},
                {AstateIndex.JMove3,  new AstateRecord(  "Base Layer.JMove.", "JMove3", AcliptypeIndex.JMove3,Attr.None)},
                {AstateIndex.JMove4,  new AstateRecord(  "Base Layer.JMove.", "JMove4", AcliptypeIndex.JMove4,Attr.BusyX)},

                {AstateIndex.DBlockL,  new AstateRecord(  "Base Layer.", "DBlockL", AcliptypeIndex.DBlockL, Attr.BusyX | Attr.Block)},
                {AstateIndex.DBlockM,  new AstateRecord(  "Base Layer.", "DBlockM", AcliptypeIndex.DBlockM, Attr.BusyX | Attr.Block)},
                {AstateIndex.DBlockH,  new AstateRecord(  "Base Layer.", "DBlockH", AcliptypeIndex.DBlockH, Attr.BusyX | Attr.Block)},
                {AstateIndex.DAtkLP,  new AstateRecord(  "Base Layer.", "DAtkLP", AcliptypeIndex.DAtkLP,Attr.None)},
                {AstateIndex.DAtkMP,  new AstateRecord(  "Base Layer.", "DAtkMP", AcliptypeIndex.DAtkMP,Attr.None)},
                {AstateIndex.DAtkHP,  new AstateRecord(  "Base Layer.", "DAtkHP", AcliptypeIndex.DAtkHP,Attr.None)},
                {AstateIndex.DAtkLK,  new AstateRecord(  "Base Layer.", "DAtkLK", AcliptypeIndex.DAtkLK,Attr.None)},
                {AstateIndex.DAtkMK,  new AstateRecord(  "Base Layer.", "DAtkMK", AcliptypeIndex.DAtkMK,Attr.None)},
                {AstateIndex.DAtkHK,  new AstateRecord(  "Base Layer.", "DAtkHK", AcliptypeIndex.DAtkHK,Attr.None)},

                {AstateIndex.DMove,  new AstateRecord(  "Base Layer.", "DMove", AcliptypeIndex.DMove,Attr.None)},

                {AstateIndex.CBlockL,  new AstateRecord(  "Base Layer.", "CBlockL", AcliptypeIndex.CBlockL, Attr.BusyX | Attr.Block)},
                {AstateIndex.CBlockM,  new AstateRecord(  "Base Layer.", "CBlockM", AcliptypeIndex.CBlockM, Attr.BusyX | Attr.Block)},
                {AstateIndex.CBlockH,  new AstateRecord(  "Base Layer.", "CBlockH", AcliptypeIndex.CBlockH, Attr.BusyX | Attr.Block)},
                {AstateIndex.CAtkLP,  new AstateRecord(  "Base Layer.", "CAtkLP", AcliptypeIndex.CAtkLP,Attr.BusyX)},
                {AstateIndex.CAtkMP,  new AstateRecord(  "Base Layer.", "CAtkMP", AcliptypeIndex.CAtkMP,Attr.BusyX)},
                {AstateIndex.CAtkHP,  new AstateRecord(  "Base Layer.", "CAtkHP", AcliptypeIndex.CAtkHP,Attr.BusyX)},
                {AstateIndex.CAtkLK,  new AstateRecord(  "Base Layer.", "CAtkLK", AcliptypeIndex.CAtkLK,Attr.BusyX)},
                {AstateIndex.CAtkMK,  new AstateRecord(  "Base Layer.", "CAtkMK", AcliptypeIndex.CAtkMK,Attr.BusyX)},
                {AstateIndex.CAtkHK,  new AstateRecord(  "Base Layer.", "CAtkHK", AcliptypeIndex.CAtkHK,Attr.BusyX)},

                {AstateIndex.CWait,  new AstateRecord(  "Base Layer.", "CWait", AcliptypeIndex.CWait,Attr.None)},
                {AstateIndex.CMove,  new AstateRecord(  "Base Layer.", "CMove", AcliptypeIndex.CMove,Attr.None)},

                {AstateIndex.OGiveup,  new AstateRecord(  "Base Layer.", "OGiveup", AcliptypeIndex.OGiveup,Attr.None)},
                {AstateIndex.ODown_SDamageH,  new AstateRecord(  "Base Layer.", "ODown_SDamageH", AcliptypeIndex.SDamageH,Attr.BusyX)},
                {AstateIndex.ODown,  new AstateRecord(  "Base Layer.", "ODown", AcliptypeIndex.ODown,Attr.BusyX)},
                {AstateIndex.OStandup,  new AstateRecord(  "Base Layer.", "OStandup", AcliptypeIndex.OStandup,Attr.None)},

                {AstateIndex.SDamageL,  new AstateRecord(  "Base Layer.", "SDamageL", AcliptypeIndex.SDamageL,Attr.None)},
                {AstateIndex.SDamageM,  new AstateRecord(  "Base Layer.", "SDamageM", AcliptypeIndex.SDamageM,Attr.None)},
                {AstateIndex.SDamageH,  new AstateRecord(  "Base Layer.", "SDamageH", AcliptypeIndex.SDamageH,Attr.None)},

                {AstateIndex.JDamageL,  new AstateRecord(  "Base Layer.", "JDamageL", AcliptypeIndex.JDamageL,Attr.None)},
                {AstateIndex.JDamageM,  new AstateRecord(  "Base Layer.", "JDamageM", AcliptypeIndex.JDamageM,Attr.None)},
                {AstateIndex.JDamageH,  new AstateRecord(  "Base Layer.", "JDamageH", AcliptypeIndex.JDamageH,Attr.None)},

                {AstateIndex.DDamageL,  new AstateRecord(  "Base Layer.", "DDamageL", AcliptypeIndex.DDamageL,Attr.None)},
                {AstateIndex.DDamageM,  new AstateRecord(  "Base Layer.", "DDamageM", AcliptypeIndex.DDamageM,Attr.None)},
                {AstateIndex.DDamageH,  new AstateRecord(  "Base Layer.", "DDamageH", AcliptypeIndex.DDamageH,Attr.None)},

                {AstateIndex.CDamageL,  new AstateRecord(  "Base Layer.", "CDamageL", AcliptypeIndex.CDamageL,Attr.None)},
                {AstateIndex.CDamageM,  new AstateRecord(  "Base Layer.", "CDamageM", AcliptypeIndex.CDamageM,Attr.None)},
                {AstateIndex.CDamageH,  new AstateRecord(  "Base Layer.", "CDamageH", AcliptypeIndex.CDamageH,Attr.None)},
            };
        }

        /// <summary>
        /// Animator の state の hash を、state番号に変換☆
        /// </summary>
        public static Dictionary<int, AstateIndex> hash_to_index;
        /// <summary>
        /// Animator の state の名前と、AnimationClipの種類の対応☆　手作業で入力しておく（２度手間）
        /// </summary>
        public static Dictionary<AstateIndex, AstateRecord> index_to_record;
        /// <summary>
        /// シーンの Start( )メソッドで呼び出してください。
        /// </summary>
        public static void InsertAllStates()
        {
            hash_to_index = new Dictionary<int, AstateIndex>();

            for (int iAstate = 0; iAstate < (int)AstateIndex.Num; iAstate++)
            {
                AstateRecord astate = index_to_record[(AstateIndex)iAstate];
                hash_to_index.Add(Animator.StringToHash(astate.breadCrumb + astate.name), (AstateIndex)iAstate);
            }
        }

        /// <summary>
        /// 現在のアニメーター・ステートに対応したデータを取得。
        /// </summary>
        /// <returns></returns>
        public static AstateRecord GetCurrentAstateRecord(Animator animator)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            if (!AstateDatabase.hash_to_index.ContainsKey(state.fullPathHash))
            {
                throw new UnityException("もしかして列挙型に登録していないステートがあるんじゃないか☆（＾～＾）？　ハッシュは[" + state.fullPathHash + "]だぜ☆");
            }

            AstateIndex index = AstateDatabase.hash_to_index[state.fullPathHash];

            if (AstateDatabase.index_to_record.ContainsKey(index))
            {
                return AstateDatabase.index_to_record[index];
            }

            throw new UnityException("[" + index + "]のデータが無いぜ☆　なんでだろな☆？（＾～＾）");
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
        public static void Select(out int serialImageIndex, out int slice, CharacterIndex character, AcliptypeRecord aclipTypeRecord, int currentMotionFrame)
        {
            slice = aclipTypeRecord.slices[currentMotionFrame];

            serialImageIndex = Hitbox2DOperationScript.GetSerialImageIndex(character, aclipTypeRecord.actioning);
        }

    }
}

