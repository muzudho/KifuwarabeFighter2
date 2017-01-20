using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneMain
{
    #region アニメーション・クリップの種類
    /// <summary>
    /// AnimationClip の種類に一対一対応☆
    /// </summary>
    public enum AcliptypeIndex
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
    #endregion
    /// <summary>
    /// アニメーション・クリップの種類
    /// （キャラクター＠モーション名　となっているとして、モーション名の部分）
    /// </summary>
    public struct AcliptypeRecord
    {
        public int[] slices;
        public ActioningIndex actioning;

        public AcliptypeRecord(int[] slices, ActioningIndex actioning)
        {
            this.slices = slices;
            this.actioning = actioning;
        }
    }

    public abstract class AcliptypeDatabase
    {

        static AcliptypeDatabase()
        {
            #region アニメーション・クリップの種類
            index_to_record = new Dictionary<AcliptypeIndex, AcliptypeRecord>()
        {
            { AcliptypeIndex.SWait, new AcliptypeRecord( new int[] { 0,1,2,3 } ,ActioningIndex.Stand)},
            { AcliptypeIndex.SMove, new AcliptypeRecord( new int[] { 4,5,6,7 } ,ActioningIndex.Stand)},
            { AcliptypeIndex.SBlockL, new AcliptypeRecord( new int[] { 8,9 } ,ActioningIndex.Stand)},
            { AcliptypeIndex.SBlockM, new AcliptypeRecord( new int[] { 8,9,10 } ,ActioningIndex.Stand)},
            { AcliptypeIndex.SBlockH, new AcliptypeRecord( new int[] { 8,9,10,11,9 } ,ActioningIndex.Stand)},
            { AcliptypeIndex.SDamageL, new AcliptypeRecord( new int[] { 12,13 } ,ActioningIndex.Stand)},
            { AcliptypeIndex.SDamageM, new AcliptypeRecord( new int[] { 12,13,14 } ,ActioningIndex.Stand)},
            { AcliptypeIndex.SDamageH, new AcliptypeRecord( new int[] { 12, 13, 14,15,13 } ,ActioningIndex.Stand)},
            { AcliptypeIndex.SAtkLP,new AcliptypeRecord( new int[] { 16,17 },ActioningIndex.Stand)},
            { AcliptypeIndex.SAtkMP,new AcliptypeRecord( new int[] { 16, 17,18 },ActioningIndex.Stand)},
            { AcliptypeIndex.SAtkHP,new AcliptypeRecord( new int[] { 16, 17, 18,19,17 },ActioningIndex.Stand)},
            { AcliptypeIndex.SAtkLK,new AcliptypeRecord( new int[] { 20,21 },ActioningIndex.Stand)},
            { AcliptypeIndex.SAtkMK,new AcliptypeRecord( new int[] { 20, 21, 22 },ActioningIndex.Stand)},
            { AcliptypeIndex.SAtkHK,new AcliptypeRecord( new int[] { 20, 21, 22, 23,21 },ActioningIndex.Stand)},

            { AcliptypeIndex.JMove0, new AcliptypeRecord( new int[] { 0,1 },ActioningIndex.Jump) },
            { AcliptypeIndex.JMove1, new AcliptypeRecord( new int[] { 2,3 },ActioningIndex.Jump) },
            { AcliptypeIndex.JMove2, new AcliptypeRecord( new int[] { 4 },ActioningIndex.Jump) },
            { AcliptypeIndex.JMove3, new AcliptypeRecord( new int[] { 5,6 },ActioningIndex.Jump) },
            { AcliptypeIndex.JMove4, new AcliptypeRecord( new int[] { 7 },ActioningIndex.Jump) },
            { AcliptypeIndex.JBlockL, new AcliptypeRecord( new int[] { 8,9 },ActioningIndex.Jump) },
            { AcliptypeIndex.JBlockM, new AcliptypeRecord( new int[] { 8,9,10 },ActioningIndex.Jump) },
            { AcliptypeIndex.JBlockH, new AcliptypeRecord( new int[] { 8,9,10,11,9 },ActioningIndex.Jump) },
            { AcliptypeIndex.JDamageL, new AcliptypeRecord( new int[] { 12,13 },ActioningIndex.Jump) },
            { AcliptypeIndex.JDamageM, new AcliptypeRecord( new int[] { 12,13,14 },ActioningIndex.Jump) },
            { AcliptypeIndex.JDamageH, new AcliptypeRecord( new int[] { 12, 13, 14,15,13 },ActioningIndex.Jump) },
            { AcliptypeIndex.JAtkLP,new AcliptypeRecord( new int[] { 16,17 },ActioningIndex.Jump)},
            { AcliptypeIndex.JAtkMP,new AcliptypeRecord( new int[] { 16, 17,18 },ActioningIndex.Jump)},
            { AcliptypeIndex.JAtkHP,new AcliptypeRecord( new int[] { 16, 17, 18,19,17 },ActioningIndex.Jump)},
            { AcliptypeIndex.JAtkLK,new AcliptypeRecord( new int[] { 20,21 },ActioningIndex.Jump)},
            { AcliptypeIndex.JAtkMK,new AcliptypeRecord( new int[] { 20, 21, 22 },ActioningIndex.Jump)},
            { AcliptypeIndex.JAtkHK,new AcliptypeRecord( new int[] { 20, 21, 22, 23,21 },ActioningIndex.Jump)},

            { AcliptypeIndex.DMove, new AcliptypeRecord( new int[] { 0,1,2,3,4,5,6,7 },ActioningIndex.Dash) },
            { AcliptypeIndex.DBlockL, new AcliptypeRecord( new int[] { 8,9 },ActioningIndex.Dash) },
            { AcliptypeIndex.DBlockM, new AcliptypeRecord( new int[] { 8,9,10 },ActioningIndex.Dash) },
            { AcliptypeIndex.DBlockH, new AcliptypeRecord( new int[] { 8,9,10,11,9 },ActioningIndex.Dash) },
            { AcliptypeIndex.DDamageL, new AcliptypeRecord( new int[] { 12,13 },ActioningIndex.Dash) },
            { AcliptypeIndex.DDamageM, new AcliptypeRecord( new int[] { 12,13,14 },ActioningIndex.Dash) },
            { AcliptypeIndex.DDamageH, new AcliptypeRecord( new int[] { 12, 13, 14,15,13 },ActioningIndex.Dash) },
            { AcliptypeIndex.DAtkLP,new AcliptypeRecord( new int[] { 16,17 },ActioningIndex.Dash)},
            { AcliptypeIndex.DAtkMP,new AcliptypeRecord( new int[] { 16, 17,18 },ActioningIndex.Dash)},
            { AcliptypeIndex.DAtkHP,new AcliptypeRecord( new int[] { 16, 17, 18,19,17 },ActioningIndex.Dash)},
            { AcliptypeIndex.DAtkLK,new AcliptypeRecord( new int[] { 20,21 },ActioningIndex.Dash)},
            { AcliptypeIndex.DAtkMK,new AcliptypeRecord( new int[] { 20, 21, 22 },ActioningIndex.Dash)},
            { AcliptypeIndex.DAtkHK,new AcliptypeRecord( new int[] { 20, 21, 22, 23,21 },ActioningIndex.Dash)},

            { AcliptypeIndex.CWait,new AcliptypeRecord( new int[] { 0,1,2,3 },ActioningIndex.Crouch)},
            { AcliptypeIndex.CMove,new AcliptypeRecord( new int[] { 4,5,6,7 },ActioningIndex.Crouch)},
            { AcliptypeIndex.CBlockL, new AcliptypeRecord( new int[] { 8,9 },ActioningIndex.Crouch) },
            { AcliptypeIndex.CBlockM, new AcliptypeRecord( new int[] { 8,9,10 },ActioningIndex.Crouch) },
            { AcliptypeIndex.CBlockH, new AcliptypeRecord( new int[] { 8,9,10,11,9 },ActioningIndex.Crouch) },
            { AcliptypeIndex.CDamageL, new AcliptypeRecord( new int[] { 12,13 },ActioningIndex.Crouch) },
            { AcliptypeIndex.CDamageM, new AcliptypeRecord( new int[] { 12,13,14 },ActioningIndex.Crouch) },
            { AcliptypeIndex.CDamageH, new AcliptypeRecord( new int[] { 12, 13, 14,15,13 },ActioningIndex.Crouch) },
            { AcliptypeIndex.CAtkLP,new AcliptypeRecord( new int[] { 16,17 },ActioningIndex.Crouch)},
            { AcliptypeIndex.CAtkMP,new AcliptypeRecord( new int[] { 16, 17,18 },ActioningIndex.Crouch)},
            { AcliptypeIndex.CAtkHP,new AcliptypeRecord( new int[] { 16, 17, 18,19,17 },ActioningIndex.Crouch)},
            { AcliptypeIndex.CAtkLK,new AcliptypeRecord( new int[] { 20,21 },ActioningIndex.Crouch)},
            { AcliptypeIndex.CAtkMK,new AcliptypeRecord( new int[] { 20, 21, 22 },ActioningIndex.Crouch)},
            { AcliptypeIndex.CAtkHK,new AcliptypeRecord( new int[] { 20, 21, 22, 23,21 },ActioningIndex.Crouch)},

            { AcliptypeIndex.OBackstep,new AcliptypeRecord( new int[] { 0,1,2,3,4,5,6,7 },ActioningIndex.Other)},
            { AcliptypeIndex.ODown,new AcliptypeRecord( new int[] { 8,9 },ActioningIndex.Other)},
            { AcliptypeIndex.OStandup,new AcliptypeRecord( new int[] { 10,11 },ActioningIndex.Other)},
            { AcliptypeIndex.OGiveup,new AcliptypeRecord( new int[] { 12,13,14,15 },ActioningIndex.Other)},
        };
            #endregion
        }

        public static Dictionary<AcliptypeIndex, AcliptypeRecord> index_to_record;


    }
}
