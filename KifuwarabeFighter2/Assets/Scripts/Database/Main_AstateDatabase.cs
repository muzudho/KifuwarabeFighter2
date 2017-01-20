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
    /// アニメーターのステート
    /// </summary>
    public struct AstateRecord
    {
        public AcliptypeIndex acliptype;
        public string breadCrumb;
        public string name;
        public AstateAttribute attribute;

        public AstateRecord(string breadCrumb, string name, AcliptypeIndex acliptype, AstateAttribute attribute)
        {
            this.breadCrumb = breadCrumb;
            this.name = name;
            this.acliptype = acliptype;
            this.attribute = attribute;
        }
    }

    [Flags]
    public enum AstateAttribute
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
    }

    public abstract class AstateDatabase
    {

        static AstateDatabase()
        {
            #region アニメーターのステート
            index_to_record = new Dictionary<AstateIndex, AstateRecord>()
        {
            {AstateIndex.SWait, new AstateRecord(  "Base Layer.", "SWait", AcliptypeIndex.SWait,AstateAttribute.None)},
            {AstateIndex.SMove,  new AstateRecord(  "Base Layer.", "SMove", AcliptypeIndex.SMove,AstateAttribute.None)},
            {AstateIndex.SBlockL,  new AstateRecord(  "Base Layer.", "SBlockL", AcliptypeIndex.SBlockL,AstateAttribute.None)},
            {AstateIndex.SBlockM,  new AstateRecord(  "Base Layer.", "SBlockM", AcliptypeIndex.SBlockM,AstateAttribute.None)},
            {AstateIndex.SBlockH,  new AstateRecord(  "Base Layer.", "SBlockH", AcliptypeIndex.SBlockH,AstateAttribute.None)},
            {AstateIndex.SAtkLP,  new AstateRecord(  "Base Layer.", "SAtkLP", AcliptypeIndex.SAtkLP,AstateAttribute.BusyX)},
            {AstateIndex.SAtkMP,  new AstateRecord(  "Base Layer.", "SAtkMP", AcliptypeIndex.SAtkMP,AstateAttribute.BusyX)},
            {AstateIndex.SAtkHP,  new AstateRecord( "Base Layer.", "SAtkHP",  AcliptypeIndex.SAtkHP,AstateAttribute.BusyX)},
            {AstateIndex.SAtkLK,  new AstateRecord(  "Base Layer.", "SAtkLK", AcliptypeIndex.SAtkLK,AstateAttribute.BusyX)},
            {AstateIndex.SAtkMK,  new AstateRecord(  "Base Layer.", "SAtkMK", AcliptypeIndex.SAtkMK,AstateAttribute.BusyX)},
            {AstateIndex.SAtkHK,  new AstateRecord(  "Base Layer.", "SAtkHK", AcliptypeIndex.SAtkHK,AstateAttribute.BusyX)},

            {AstateIndex.OBackstep,  new AstateRecord(  "Base Layer.", "OBackstep", AcliptypeIndex.OBackstep,AstateAttribute.None)},

            {AstateIndex.JBlockL,  new AstateRecord(  "Base Layer.", "JBlockL", AcliptypeIndex.JBlockL,AstateAttribute.None)},
            {AstateIndex.JBlockM,  new AstateRecord(  "Base Layer.", "JBlockM", AcliptypeIndex.JBlockM,AstateAttribute.None)},
            {AstateIndex.JBlockH,  new AstateRecord(  "Base Layer.", "JBlockH", AcliptypeIndex.JBlockH,AstateAttribute.None)},
            {AstateIndex.JAtkLP,  new AstateRecord(  "Base Layer.", "JAtkLP", AcliptypeIndex.JAtkLP,AstateAttribute.None)},
            {AstateIndex.JAtkMP,  new AstateRecord(  "Base Layer.", "JAtkMP", AcliptypeIndex.JAtkMP,AstateAttribute.None)},
            {AstateIndex.JAtkHP,  new AstateRecord(  "Base Layer.", "JAtkHP", AcliptypeIndex.JAtkHP,AstateAttribute.None)},
            {AstateIndex.JAtkLK,  new AstateRecord(  "Base Layer.", "JAtkLK", AcliptypeIndex.JAtkLK,AstateAttribute.None)},
            {AstateIndex.JAtkMK,  new AstateRecord( "Base Layer.", "JAtkMK",  AcliptypeIndex.JAtkMK,AstateAttribute.None)},
            {AstateIndex.JAtkHK,  new AstateRecord(  "Base Layer.", "JAtkHK", AcliptypeIndex.JAtkHK,AstateAttribute.None)},

            {AstateIndex.JMove0,  new AstateRecord(  "Base Layer.JMove.", "JMove0", AcliptypeIndex.JMove0,AstateAttribute.BusyX | AstateAttribute.BusyY)},
            {AstateIndex.JMove1,  new AstateRecord(  "Base Layer.JMove.", "JMove1", AcliptypeIndex.JMove1,AstateAttribute.None)},
            {AstateIndex.JMove2,  new AstateRecord( "Base Layer.JMove.", "JMove2",  AcliptypeIndex.JMove2,AstateAttribute.None)},
            {AstateIndex.JMove3,  new AstateRecord(  "Base Layer.JMove.", "JMove3", AcliptypeIndex.JMove3,AstateAttribute.None)},
            {AstateIndex.JMove4,  new AstateRecord(  "Base Layer.JMove.", "JMove4", AcliptypeIndex.JMove4,AstateAttribute.BusyX)},

            {AstateIndex.DBlockL,  new AstateRecord(  "Base Layer.", "DBlockL", AcliptypeIndex.DBlockL,AstateAttribute.None)},
            {AstateIndex.DBlockM,  new AstateRecord(  "Base Layer.", "DBlockM", AcliptypeIndex.DBlockM,AstateAttribute.None)},
            {AstateIndex.DBlockH,  new AstateRecord(  "Base Layer.", "DBlockH", AcliptypeIndex.DBlockH,AstateAttribute.None)},
            {AstateIndex.DAtkLP,  new AstateRecord(  "Base Layer.", "DAtkLP", AcliptypeIndex.DAtkLP,AstateAttribute.None)},
            {AstateIndex.DAtkMP,  new AstateRecord(  "Base Layer.", "DAtkMP", AcliptypeIndex.DAtkMP,AstateAttribute.None)},
            {AstateIndex.DAtkHP,  new AstateRecord(  "Base Layer.", "DAtkHP", AcliptypeIndex.DAtkHP,AstateAttribute.None)},
            {AstateIndex.DAtkLK,  new AstateRecord(  "Base Layer.", "DAtkLK", AcliptypeIndex.DAtkLK,AstateAttribute.None)},
            {AstateIndex.DAtkMK,  new AstateRecord(  "Base Layer.", "DAtkMK", AcliptypeIndex.DAtkMK,AstateAttribute.None)},
            {AstateIndex.DAtkHK,  new AstateRecord(  "Base Layer.", "DAtkHK", AcliptypeIndex.DAtkHK,AstateAttribute.None)},

            {AstateIndex.DMove,  new AstateRecord(  "Base Layer.", "DMove", AcliptypeIndex.DMove,AstateAttribute.None)},

            {AstateIndex.CBlockL,  new AstateRecord(  "Base Layer.", "CBlockL", AcliptypeIndex.CBlockL,AstateAttribute.None)},
            {AstateIndex.CBlockM,  new AstateRecord(  "Base Layer.", "CBlockM", AcliptypeIndex.CBlockM,AstateAttribute.None)},
            {AstateIndex.CBlockH,  new AstateRecord(  "Base Layer.", "CBlockH", AcliptypeIndex.CBlockH,AstateAttribute.None)},
            {AstateIndex.CAtkLP,  new AstateRecord(  "Base Layer.", "CAtkLP", AcliptypeIndex.CAtkLP,AstateAttribute.BusyX)},
            {AstateIndex.CAtkMP,  new AstateRecord(  "Base Layer.", "CAtkMP", AcliptypeIndex.CAtkMP,AstateAttribute.BusyX)},
            {AstateIndex.CAtkHP,  new AstateRecord(  "Base Layer.", "CAtkHP", AcliptypeIndex.CAtkHP,AstateAttribute.BusyX)},
            {AstateIndex.CAtkLK,  new AstateRecord(  "Base Layer.", "CAtkLK", AcliptypeIndex.CAtkLK,AstateAttribute.BusyX)},
            {AstateIndex.CAtkMK,  new AstateRecord(  "Base Layer.", "CAtkMK", AcliptypeIndex.CAtkMK,AstateAttribute.BusyX)},
            {AstateIndex.CAtkHK,  new AstateRecord(  "Base Layer.", "CAtkHK", AcliptypeIndex.CAtkHK,AstateAttribute.BusyX)},

            {AstateIndex.CWait,  new AstateRecord(  "Base Layer.", "CWait", AcliptypeIndex.CWait,AstateAttribute.None)},
            {AstateIndex.CMove,  new AstateRecord(  "Base Layer.", "CMove", AcliptypeIndex.CMove,AstateAttribute.None)},

            {AstateIndex.OGiveup,  new AstateRecord(  "Base Layer.", "OGiveup", AcliptypeIndex.OGiveup,AstateAttribute.None)},
            {AstateIndex.ODown_SDamageH,  new AstateRecord(  "Base Layer.", "ODown_SDamageH", AcliptypeIndex.SDamageH,AstateAttribute.BusyX)},
            {AstateIndex.ODown,  new AstateRecord(  "Base Layer.", "ODown", AcliptypeIndex.ODown,AstateAttribute.BusyX)},
            {AstateIndex.OStandup,  new AstateRecord(  "Base Layer.", "OStandup", AcliptypeIndex.OStandup,AstateAttribute.None)},

            {AstateIndex.SDamageL,  new AstateRecord(  "Base Layer.", "SDamageL", AcliptypeIndex.SDamageL,AstateAttribute.None)},
            {AstateIndex.SDamageM,  new AstateRecord(  "Base Layer.", "SDamageM", AcliptypeIndex.SDamageM,AstateAttribute.None)},
            {AstateIndex.SDamageH,  new AstateRecord(  "Base Layer.", "SDamageH", AcliptypeIndex.SDamageH,AstateAttribute.None)},

            {AstateIndex.JDamageL,  new AstateRecord(  "Base Layer.", "JDamageL", AcliptypeIndex.JDamageL,AstateAttribute.None)},
            {AstateIndex.JDamageM,  new AstateRecord(  "Base Layer.", "JDamageM", AcliptypeIndex.JDamageM,AstateAttribute.None)},
            {AstateIndex.JDamageH,  new AstateRecord(  "Base Layer.", "JDamageH", AcliptypeIndex.JDamageH,AstateAttribute.None)},

            {AstateIndex.DDamageL,  new AstateRecord(  "Base Layer.", "DDamageL", AcliptypeIndex.DDamageL,AstateAttribute.None)},
            {AstateIndex.DDamageM,  new AstateRecord(  "Base Layer.", "DDamageM", AcliptypeIndex.DDamageM,AstateAttribute.None)},
            {AstateIndex.DDamageH,  new AstateRecord(  "Base Layer.", "DDamageH", AcliptypeIndex.DDamageH,AstateAttribute.None)},

            {AstateIndex.CDamageL,  new AstateRecord(  "Base Layer.", "CDamageL", AcliptypeIndex.CDamageL,AstateAttribute.None)},
            {AstateIndex.CDamageM,  new AstateRecord(  "Base Layer.", "CDamageM", AcliptypeIndex.CDamageM,AstateAttribute.None)},
            {AstateIndex.CDamageH,  new AstateRecord(  "Base Layer.", "CDamageH", AcliptypeIndex.CDamageH,AstateAttribute.None)},
        };
            #endregion
        }

        #region アニメーターのステート名
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
        #endregion
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

                //Debug.Log("ReadAstateHashs: iAstate = " + iAstate + " breadCrumb = " + breadCrumb + " name = " + name + " hash = " + hash + " aclipType = " + aclipType );
            }
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

