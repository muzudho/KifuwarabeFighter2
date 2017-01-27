using System.Collections.Generic;

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
    /// アニメーション・クリップの種類
    /// （キャラクター＠モーション名　となっているとして、モーション名の部分）
    /// </summary>
    public struct CliptypeRecord
    {
        public int[] slices;
        public ActioningIndex actioning;

        public CliptypeRecord(int[] slices, ActioningIndex actioning)
        {
            this.slices = slices;
            this.actioning = actioning;
        }
    }

    public abstract class CliptypeDatabase
    {

        static CliptypeDatabase()
        {
            index_to_record = new Dictionary<CliptypeIndex, CliptypeRecord>()
            {
                { CliptypeIndex.SWait, new CliptypeRecord( new int[] { 0,1,2,3 } ,ActioningIndex.Stand)},
                { CliptypeIndex.SMove, new CliptypeRecord( new int[] { 4,5,6,7 } ,ActioningIndex.Stand)},
                { CliptypeIndex.SBlockL, new CliptypeRecord( new int[] { 8,9 } ,ActioningIndex.Stand)},
                { CliptypeIndex.SBlockM, new CliptypeRecord( new int[] { 8,9,10 } ,ActioningIndex.Stand)},
                { CliptypeIndex.SBlockH, new CliptypeRecord( new int[] { 8,9,10,11,9 } ,ActioningIndex.Stand)},
                { CliptypeIndex.SDamageL, new CliptypeRecord( new int[] { 12,13 } ,ActioningIndex.Stand)},
                { CliptypeIndex.SDamageM, new CliptypeRecord( new int[] { 12,13,14 } ,ActioningIndex.Stand)},
                { CliptypeIndex.SDamageH, new CliptypeRecord( new int[] { 12, 13, 14,15,13 } ,ActioningIndex.Stand)},
                { CliptypeIndex.SAtkLP,new CliptypeRecord( new int[] { 16,17 },ActioningIndex.Stand)},
                { CliptypeIndex.SAtkMP,new CliptypeRecord( new int[] { 16, 17,18 },ActioningIndex.Stand)},
                { CliptypeIndex.SAtkHP,new CliptypeRecord( new int[] { 16, 17, 18,19,17 },ActioningIndex.Stand)},
                { CliptypeIndex.SAtkLK,new CliptypeRecord( new int[] { 20,21 },ActioningIndex.Stand)},
                { CliptypeIndex.SAtkMK,new CliptypeRecord( new int[] { 20, 21, 22 },ActioningIndex.Stand)},
                { CliptypeIndex.SAtkHK,new CliptypeRecord( new int[] { 20, 21, 22, 23,21 },ActioningIndex.Stand)},

                { CliptypeIndex.JMove0, new CliptypeRecord( new int[] { 0,1 },ActioningIndex.Jump) },
                { CliptypeIndex.JMove1, new CliptypeRecord( new int[] { 2,3 },ActioningIndex.Jump) },
                { CliptypeIndex.JMove2, new CliptypeRecord( new int[] { 4 },ActioningIndex.Jump) },
                { CliptypeIndex.JMove3, new CliptypeRecord( new int[] { 5,6 },ActioningIndex.Jump) },
                { CliptypeIndex.JMove4, new CliptypeRecord( new int[] { 7 },ActioningIndex.Jump) },
                { CliptypeIndex.JBlockL, new CliptypeRecord( new int[] { 8,9 },ActioningIndex.Jump) },
                { CliptypeIndex.JBlockM, new CliptypeRecord( new int[] { 8,9,10 },ActioningIndex.Jump) },
                { CliptypeIndex.JBlockH, new CliptypeRecord( new int[] { 8,9,10,11,9 },ActioningIndex.Jump) },
                { CliptypeIndex.JDamageL, new CliptypeRecord( new int[] { 12,13 },ActioningIndex.Jump) },
                { CliptypeIndex.JDamageM, new CliptypeRecord( new int[] { 12,13,14 },ActioningIndex.Jump) },
                { CliptypeIndex.JDamageH, new CliptypeRecord( new int[] { 12, 13, 14,15,13 },ActioningIndex.Jump) },
                { CliptypeIndex.JAtkLP,new CliptypeRecord( new int[] { 16,17 },ActioningIndex.Jump)},
                { CliptypeIndex.JAtkMP,new CliptypeRecord( new int[] { 16, 17,18 },ActioningIndex.Jump)},
                { CliptypeIndex.JAtkHP,new CliptypeRecord( new int[] { 16, 17, 18,19,17 },ActioningIndex.Jump)},
                { CliptypeIndex.JAtkLK,new CliptypeRecord( new int[] { 20,21 },ActioningIndex.Jump)},
                { CliptypeIndex.JAtkMK,new CliptypeRecord( new int[] { 20, 21, 22 },ActioningIndex.Jump)},
                { CliptypeIndex.JAtkHK,new CliptypeRecord( new int[] { 20, 21, 22, 23,21 },ActioningIndex.Jump)},

                { CliptypeIndex.DMove, new CliptypeRecord( new int[] { 0,1,2,3,4,5,6,7 },ActioningIndex.Dash) },
                { CliptypeIndex.DBlockL, new CliptypeRecord( new int[] { 8,9 },ActioningIndex.Dash) },
                { CliptypeIndex.DBlockM, new CliptypeRecord( new int[] { 8,9,10 },ActioningIndex.Dash) },
                { CliptypeIndex.DBlockH, new CliptypeRecord( new int[] { 8,9,10,11,9 },ActioningIndex.Dash) },
                { CliptypeIndex.DDamageL, new CliptypeRecord( new int[] { 12,13 },ActioningIndex.Dash) },
                { CliptypeIndex.DDamageM, new CliptypeRecord( new int[] { 12,13,14 },ActioningIndex.Dash) },
                { CliptypeIndex.DDamageH, new CliptypeRecord( new int[] { 12, 13, 14,15,13 },ActioningIndex.Dash) },
                { CliptypeIndex.DAtkLP,new CliptypeRecord( new int[] { 16,17 },ActioningIndex.Dash)},
                { CliptypeIndex.DAtkMP,new CliptypeRecord( new int[] { 16, 17,18 },ActioningIndex.Dash)},
                { CliptypeIndex.DAtkHP,new CliptypeRecord( new int[] { 16, 17, 18,19,17 },ActioningIndex.Dash)},
                { CliptypeIndex.DAtkLK,new CliptypeRecord( new int[] { 20,21 },ActioningIndex.Dash)},
                { CliptypeIndex.DAtkMK,new CliptypeRecord( new int[] { 20, 21, 22 },ActioningIndex.Dash)},
                { CliptypeIndex.DAtkHK,new CliptypeRecord( new int[] { 20, 21, 22, 23,21 },ActioningIndex.Dash)},

                { CliptypeIndex.CWait,new CliptypeRecord( new int[] { 0,1,2,3 },ActioningIndex.Crouch)},
                { CliptypeIndex.CMove,new CliptypeRecord( new int[] { 4,5,6,7 },ActioningIndex.Crouch)},
                { CliptypeIndex.CBlockL, new CliptypeRecord( new int[] { 8,9 },ActioningIndex.Crouch) },
                { CliptypeIndex.CBlockM, new CliptypeRecord( new int[] { 8,9,10 },ActioningIndex.Crouch) },
                { CliptypeIndex.CBlockH, new CliptypeRecord( new int[] { 8,9,10,11,9 },ActioningIndex.Crouch) },
                { CliptypeIndex.CDamageL, new CliptypeRecord( new int[] { 12,13 },ActioningIndex.Crouch) },
                { CliptypeIndex.CDamageM, new CliptypeRecord( new int[] { 12,13,14 },ActioningIndex.Crouch) },
                { CliptypeIndex.CDamageH, new CliptypeRecord( new int[] { 12, 13, 14,15,13 },ActioningIndex.Crouch) },
                { CliptypeIndex.CAtkLP,new CliptypeRecord( new int[] { 16,17 },ActioningIndex.Crouch)},
                { CliptypeIndex.CAtkMP,new CliptypeRecord( new int[] { 16, 17,18 },ActioningIndex.Crouch)},
                { CliptypeIndex.CAtkHP,new CliptypeRecord( new int[] { 16, 17, 18,19,17 },ActioningIndex.Crouch)},
                { CliptypeIndex.CAtkLK,new CliptypeRecord( new int[] { 20,21 },ActioningIndex.Crouch)},
                { CliptypeIndex.CAtkMK,new CliptypeRecord( new int[] { 20, 21, 22 },ActioningIndex.Crouch)},
                { CliptypeIndex.CAtkHK,new CliptypeRecord( new int[] { 20, 21, 22, 23,21 },ActioningIndex.Crouch)},

                { CliptypeIndex.OBackstep,new CliptypeRecord( new int[] { 0,1,2,3,4,5,6,7 },ActioningIndex.Other)},
                { CliptypeIndex.ODown,new CliptypeRecord( new int[] { 8,9 },ActioningIndex.Other)},
                { CliptypeIndex.OStandup,new CliptypeRecord( new int[] { 10,11 },ActioningIndex.Other)},
                { CliptypeIndex.OGiveup,new CliptypeRecord( new int[] { 12,13,14,15 },ActioningIndex.Other)},
            };
        }

        public static Dictionary<CliptypeIndex, CliptypeRecord> index_to_record;


    }
}
