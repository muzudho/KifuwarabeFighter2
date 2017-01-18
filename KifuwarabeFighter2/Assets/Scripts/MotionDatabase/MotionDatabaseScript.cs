using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アニメーション・クリップの種類
/// （キャラクター＠モーション名　となっているとして、モーション名の部分）
/// </summary>
public struct AclipTypeRecord
{
    public int[] slices;
    public ActioningIndex actioning;

    public AclipTypeRecord(int[] slices, ActioningIndex actioning)
    {
        this.slices = slices;
        this.actioning = actioning;
    }
}
/// <summary>
/// アニメーターのステート
/// </summary>
public struct AstateRecord
{
    public MotionDatabaseScript.AclipTypeIndex aclipType;
    public string breadCrumb;
    public string name;
    public AstateAttribute attribute;

    public AstateRecord(MotionDatabaseScript.AclipTypeIndex aclipType, string breadCrumb, string name, AstateAttribute attribute)
    {
        this.aclipType = aclipType;
        this.breadCrumb = breadCrumb;
        this.name = name;
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
}

public abstract class MotionDatabaseScript {

    static MotionDatabaseScript()
    {
        #region アニメーション・クリップの種類
        aclipType_to_record = new Dictionary<AclipTypeIndex, AclipTypeRecord>()
        {
            { AclipTypeIndex.SWait, new AclipTypeRecord( new int[] { 0,1,2,3 } ,ActioningIndex.Stand)},
            { AclipTypeIndex.SMove, new AclipTypeRecord( new int[] { 4,5,6,7 } ,ActioningIndex.Stand)},
            { AclipTypeIndex.SBlockL, new AclipTypeRecord( new int[] { 8,9 } ,ActioningIndex.Stand)},
            { AclipTypeIndex.SBlockM, new AclipTypeRecord( new int[] { 8,9,10 } ,ActioningIndex.Stand)},
            { AclipTypeIndex.SBlockH, new AclipTypeRecord( new int[] { 8,9,10,11,9 } ,ActioningIndex.Stand)},
            { AclipTypeIndex.SDamageL, new AclipTypeRecord( new int[] { 12,13 } ,ActioningIndex.Stand)},
            { AclipTypeIndex.SDamageM, new AclipTypeRecord( new int[] { 12,13,14 } ,ActioningIndex.Stand)},
            { AclipTypeIndex.SDamageH, new AclipTypeRecord( new int[] { 12, 13, 14,15,13 } ,ActioningIndex.Stand)},
            { AclipTypeIndex.SAtkLP,new AclipTypeRecord( new int[] { 16,17 },ActioningIndex.Stand)},
            { AclipTypeIndex.SAtkMP,new AclipTypeRecord( new int[] { 16, 17,18 },ActioningIndex.Stand)},
            { AclipTypeIndex.SAtkHP,new AclipTypeRecord( new int[] { 16, 17, 18,19,17 },ActioningIndex.Stand)},
            { AclipTypeIndex.SAtkLK,new AclipTypeRecord( new int[] { 20,21 },ActioningIndex.Stand)},
            { AclipTypeIndex.SAtkMK,new AclipTypeRecord( new int[] { 20, 21, 22 },ActioningIndex.Stand)},
            { AclipTypeIndex.SAtkHK,new AclipTypeRecord( new int[] { 20, 21, 22, 23,21 },ActioningIndex.Stand)},

            { AclipTypeIndex.JMove0, new AclipTypeRecord( new int[] { 0,1 },ActioningIndex.Jump) },
            { AclipTypeIndex.JMove1, new AclipTypeRecord( new int[] { 2,3 },ActioningIndex.Jump) },
            { AclipTypeIndex.JMove2, new AclipTypeRecord( new int[] { 4 },ActioningIndex.Jump) },
            { AclipTypeIndex.JMove3, new AclipTypeRecord( new int[] { 5,6 },ActioningIndex.Jump) },
            { AclipTypeIndex.JMove4, new AclipTypeRecord( new int[] { 7 },ActioningIndex.Jump) },
            { AclipTypeIndex.JBlockL, new AclipTypeRecord( new int[] { 8,9 },ActioningIndex.Jump) },
            { AclipTypeIndex.JBlockM, new AclipTypeRecord( new int[] { 8,9,10 },ActioningIndex.Jump) },
            { AclipTypeIndex.JBlockH, new AclipTypeRecord( new int[] { 8,9,10,11,9 },ActioningIndex.Jump) },
            { AclipTypeIndex.JDamageL, new AclipTypeRecord( new int[] { 12,13 },ActioningIndex.Jump) },
            { AclipTypeIndex.JDamageM, new AclipTypeRecord( new int[] { 12,13,14 },ActioningIndex.Jump) },
            { AclipTypeIndex.JDamageH, new AclipTypeRecord( new int[] { 12, 13, 14,15,13 },ActioningIndex.Jump) },
            { AclipTypeIndex.JAtkLP,new AclipTypeRecord( new int[] { 16,17 },ActioningIndex.Jump)},
            { AclipTypeIndex.JAtkMP,new AclipTypeRecord( new int[] { 16, 17,18 },ActioningIndex.Jump)},
            { AclipTypeIndex.JAtkHP,new AclipTypeRecord( new int[] { 16, 17, 18,19,17 },ActioningIndex.Jump)},
            { AclipTypeIndex.JAtkLK,new AclipTypeRecord( new int[] { 20,21 },ActioningIndex.Jump)},
            { AclipTypeIndex.JAtkMK,new AclipTypeRecord( new int[] { 20, 21, 22 },ActioningIndex.Jump)},
            { AclipTypeIndex.JAtkHK,new AclipTypeRecord( new int[] { 20, 21, 22, 23,21 },ActioningIndex.Jump)},

            { AclipTypeIndex.DMove, new AclipTypeRecord( new int[] { 0,1,2,3,4,5,6,7 },ActioningIndex.Dash) },
            { AclipTypeIndex.DBlockL, new AclipTypeRecord( new int[] { 8,9 },ActioningIndex.Dash) },
            { AclipTypeIndex.DBlockM, new AclipTypeRecord( new int[] { 8,9,10 },ActioningIndex.Dash) },
            { AclipTypeIndex.DBlockH, new AclipTypeRecord( new int[] { 8,9,10,11,9 },ActioningIndex.Dash) },
            { AclipTypeIndex.DDamageL, new AclipTypeRecord( new int[] { 12,13 },ActioningIndex.Dash) },
            { AclipTypeIndex.DDamageM, new AclipTypeRecord( new int[] { 12,13,14 },ActioningIndex.Dash) },
            { AclipTypeIndex.DDamageH, new AclipTypeRecord( new int[] { 12, 13, 14,15,13 },ActioningIndex.Dash) },
            { AclipTypeIndex.DAtkLP,new AclipTypeRecord( new int[] { 16,17 },ActioningIndex.Dash)},
            { AclipTypeIndex.DAtkMP,new AclipTypeRecord( new int[] { 16, 17,18 },ActioningIndex.Dash)},
            { AclipTypeIndex.DAtkHP,new AclipTypeRecord( new int[] { 16, 17, 18,19,17 },ActioningIndex.Dash)},
            { AclipTypeIndex.DAtkLK,new AclipTypeRecord( new int[] { 20,21 },ActioningIndex.Dash)},
            { AclipTypeIndex.DAtkMK,new AclipTypeRecord( new int[] { 20, 21, 22 },ActioningIndex.Dash)},
            { AclipTypeIndex.DAtkHK,new AclipTypeRecord( new int[] { 20, 21, 22, 23,21 },ActioningIndex.Dash)},

            { AclipTypeIndex.CWait,new AclipTypeRecord( new int[] { 0,1,2,3 },ActioningIndex.Crouch)},
            { AclipTypeIndex.CMove,new AclipTypeRecord( new int[] { 4,5,6,7 },ActioningIndex.Crouch)},
            { AclipTypeIndex.CBlockL, new AclipTypeRecord( new int[] { 8,9 },ActioningIndex.Crouch) },
            { AclipTypeIndex.CBlockM, new AclipTypeRecord( new int[] { 8,9,10 },ActioningIndex.Crouch) },
            { AclipTypeIndex.CBlockH, new AclipTypeRecord( new int[] { 8,9,10,11,9 },ActioningIndex.Crouch) },
            { AclipTypeIndex.CDamageL, new AclipTypeRecord( new int[] { 12,13 },ActioningIndex.Crouch) },
            { AclipTypeIndex.CDamageM, new AclipTypeRecord( new int[] { 12,13,14 },ActioningIndex.Crouch) },
            { AclipTypeIndex.CDamageH, new AclipTypeRecord( new int[] { 12, 13, 14,15,13 },ActioningIndex.Crouch) },
            { AclipTypeIndex.CAtkLP,new AclipTypeRecord( new int[] { 16,17 },ActioningIndex.Crouch)},
            { AclipTypeIndex.CAtkMP,new AclipTypeRecord( new int[] { 16, 17,18 },ActioningIndex.Crouch)},
            { AclipTypeIndex.CAtkHP,new AclipTypeRecord( new int[] { 16, 17, 18,19,17 },ActioningIndex.Crouch)},
            { AclipTypeIndex.CAtkLK,new AclipTypeRecord( new int[] { 20,21 },ActioningIndex.Crouch)},
            { AclipTypeIndex.CAtkMK,new AclipTypeRecord( new int[] { 20, 21, 22 },ActioningIndex.Crouch)},
            { AclipTypeIndex.CAtkHK,new AclipTypeRecord( new int[] { 20, 21, 22, 23,21 },ActioningIndex.Crouch)},

            { AclipTypeIndex.OBackstep,new AclipTypeRecord( new int[] { 0,1,2,3,4,5,6,7 },ActioningIndex.Other)},
            { AclipTypeIndex.ODown,new AclipTypeRecord( new int[] { 8,9 },ActioningIndex.Other)},
            { AclipTypeIndex.OStandup,new AclipTypeRecord( new int[] { 10,11 },ActioningIndex.Other)},
            { AclipTypeIndex.OGiveup,new AclipTypeRecord( new int[] { 12,13,14,15 },ActioningIndex.Other)},
        };
        #endregion
        #region アニメーターのステート
        astate_to_record = new Dictionary<AstateIndex, AstateRecord>()
        {
            {AstateIndex.SWait, new AstateRecord( AclipTypeIndex.SWait, "Base Layer.", "SWait", AstateAttribute.None)},
            {AstateIndex.SMove,  new AstateRecord( AclipTypeIndex.SMove, "Base Layer.", "SMove", AstateAttribute.None)},
            {AstateIndex.SBlockL,  new AstateRecord( AclipTypeIndex.SBlockL, "Base Layer.", "SBlockL", AstateAttribute.None)},
            {AstateIndex.SBlockM,  new AstateRecord( AclipTypeIndex.SBlockM, "Base Layer.", "SBlockM", AstateAttribute.None)},
            {AstateIndex.SBlockH,  new AstateRecord( AclipTypeIndex.SBlockH, "Base Layer.", "SBlockH", AstateAttribute.None)},
            {AstateIndex.SAtkLP,  new AstateRecord( AclipTypeIndex.SAtkLP, "Base Layer.", "SAtkLP", AstateAttribute.BusyX)},
            {AstateIndex.SAtkMP,  new AstateRecord( AclipTypeIndex.SAtkMP, "Base Layer.", "SAtkMP", AstateAttribute.BusyX)},
            {AstateIndex.SAtkHP,  new AstateRecord( AclipTypeIndex.SAtkHP, "Base Layer.", "SAtkHP", AstateAttribute.BusyX)},
            {AstateIndex.SAtkLK,  new AstateRecord( AclipTypeIndex.SAtkLK, "Base Layer.", "SAtkLK", AstateAttribute.BusyX)},
            {AstateIndex.SAtkMK,  new AstateRecord( AclipTypeIndex.SAtkMK, "Base Layer.", "SAtkMK", AstateAttribute.BusyX)},
            {AstateIndex.SAtkHK,  new AstateRecord( AclipTypeIndex.SAtkHK, "Base Layer.", "SAtkHK", AstateAttribute.BusyX)},

            {AstateIndex.OBackstep,  new AstateRecord( AclipTypeIndex.OBackstep, "Base Layer.", "OBackstep", AstateAttribute.None)},

            {AstateIndex.JBlockL,  new AstateRecord( AclipTypeIndex.JBlockL, "Base Layer.", "JBlockL", AstateAttribute.None)},
            {AstateIndex.JBlockM,  new AstateRecord( AclipTypeIndex.JBlockM, "Base Layer.", "JBlockM", AstateAttribute.None)},
            {AstateIndex.JBlockH,  new AstateRecord( AclipTypeIndex.JBlockH, "Base Layer.", "JBlockH", AstateAttribute.None)},
            {AstateIndex.JAtkLP,  new AstateRecord( AclipTypeIndex.JAtkLP, "Base Layer.", "JAtkLP", AstateAttribute.None)},
            {AstateIndex.JAtkMP,  new AstateRecord( AclipTypeIndex.JAtkMP, "Base Layer.", "JAtkMP", AstateAttribute.None)},
            {AstateIndex.JAtkHP,  new AstateRecord( AclipTypeIndex.JAtkHP, "Base Layer.", "JAtkHP", AstateAttribute.None)},
            {AstateIndex.JAtkLK,  new AstateRecord( AclipTypeIndex.JAtkLK, "Base Layer.", "JAtkLK", AstateAttribute.None)},
            {AstateIndex.JAtkMK,  new AstateRecord( AclipTypeIndex.JAtkMK, "Base Layer.", "JAtkMK", AstateAttribute.None)},
            {AstateIndex.JAtkHK,  new AstateRecord( AclipTypeIndex.JAtkHK, "Base Layer.", "JAtkHK", AstateAttribute.None)},

            {AstateIndex.JMove0,  new AstateRecord( AclipTypeIndex.JMove0, "Base Layer.JMove.", "JMove0", AstateAttribute.BusyX)},
            {AstateIndex.JMove1,  new AstateRecord( AclipTypeIndex.JMove1, "Base Layer.JMove.", "JMove1", AstateAttribute.None)},
            {AstateIndex.JMove2,  new AstateRecord( AclipTypeIndex.JMove2, "Base Layer.JMove.", "JMove2", AstateAttribute.None)},
            {AstateIndex.JMove3,  new AstateRecord( AclipTypeIndex.JMove3, "Base Layer.JMove.", "JMove3", AstateAttribute.None)},
            {AstateIndex.JMove4,  new AstateRecord( AclipTypeIndex.JMove4, "Base Layer.JMove.", "JMove4", AstateAttribute.BusyX)},

            {AstateIndex.DBlockL,  new AstateRecord( AclipTypeIndex.DBlockL, "Base Layer.", "DBlockL", AstateAttribute.None)},
            {AstateIndex.DBlockM,  new AstateRecord( AclipTypeIndex.DBlockM, "Base Layer.", "DBlockM", AstateAttribute.None)},
            {AstateIndex.DBlockH,  new AstateRecord( AclipTypeIndex.DBlockH, "Base Layer.", "DBlockH", AstateAttribute.None)},
            {AstateIndex.DAtkLP,  new AstateRecord( AclipTypeIndex.DAtkLP, "Base Layer.", "DAtkLP", AstateAttribute.None)},
            {AstateIndex.DAtkMP,  new AstateRecord( AclipTypeIndex.DAtkMP, "Base Layer.", "DAtkMP", AstateAttribute.None)},
            {AstateIndex.DAtkHP,  new AstateRecord( AclipTypeIndex.DAtkHP, "Base Layer.", "DAtkHP", AstateAttribute.None)},
            {AstateIndex.DAtkLK,  new AstateRecord( AclipTypeIndex.DAtkLK, "Base Layer.", "DAtkLK", AstateAttribute.None)},
            {AstateIndex.DAtkMK,  new AstateRecord( AclipTypeIndex.DAtkMK, "Base Layer.", "DAtkMK", AstateAttribute.None)},
            {AstateIndex.DAtkHK,  new AstateRecord( AclipTypeIndex.DAtkHK, "Base Layer.", "DAtkHK", AstateAttribute.None)},

            {AstateIndex.DMove,  new AstateRecord( AclipTypeIndex.DMove, "Base Layer.", "DMove", AstateAttribute.None)},

            {AstateIndex.CBlockL,  new AstateRecord( AclipTypeIndex.CBlockL, "Base Layer.", "CBlockL", AstateAttribute.None)},
            {AstateIndex.CBlockM,  new AstateRecord( AclipTypeIndex.CBlockM, "Base Layer.", "CBlockM", AstateAttribute.None)},
            {AstateIndex.CBlockH,  new AstateRecord( AclipTypeIndex.CBlockH, "Base Layer.", "CBlockH", AstateAttribute.None)},
            {AstateIndex.CAtkLP,  new AstateRecord( AclipTypeIndex.CAtkLP, "Base Layer.", "CAtkLP", AstateAttribute.BusyX)},
            {AstateIndex.CAtkMP,  new AstateRecord( AclipTypeIndex.CAtkMP, "Base Layer.", "CAtkMP", AstateAttribute.BusyX)},
            {AstateIndex.CAtkHP,  new AstateRecord( AclipTypeIndex.CAtkHP, "Base Layer.", "CAtkHP", AstateAttribute.BusyX)},
            {AstateIndex.CAtkLK,  new AstateRecord( AclipTypeIndex.CAtkLK, "Base Layer.", "CAtkLK", AstateAttribute.BusyX)},
            {AstateIndex.CAtkMK,  new AstateRecord( AclipTypeIndex.CAtkMK, "Base Layer.", "CAtkMK", AstateAttribute.BusyX)},
            {AstateIndex.CAtkHK,  new AstateRecord( AclipTypeIndex.CAtkHK, "Base Layer.", "CAtkHK", AstateAttribute.BusyX)},

            {AstateIndex.CWait,  new AstateRecord( AclipTypeIndex.CWait, "Base Layer.", "CWait", AstateAttribute.None)},
            {AstateIndex.CMove,  new AstateRecord( AclipTypeIndex.CMove, "Base Layer.", "CMove", AstateAttribute.None)},

            {AstateIndex.OGiveup,  new AstateRecord( AclipTypeIndex.OGiveup, "Base Layer.", "OGiveup", AstateAttribute.None)},
            {AstateIndex.ODown_SDamageH,  new AstateRecord( AclipTypeIndex.SDamageH, "Base Layer.", "ODown_SDamageH", AstateAttribute.BusyX)},
            {AstateIndex.ODown,  new AstateRecord( AclipTypeIndex.ODown, "Base Layer.", "ODown", AstateAttribute.BusyX)},
            {AstateIndex.OStandup,  new AstateRecord( AclipTypeIndex.OStandup, "Base Layer.", "OStandup", AstateAttribute.None)},

            {AstateIndex.SDamageL,  new AstateRecord( AclipTypeIndex.SDamageL, "Base Layer.", "SDamageL", AstateAttribute.None)},
            {AstateIndex.SDamageM,  new AstateRecord( AclipTypeIndex.SDamageM, "Base Layer.", "SDamageM", AstateAttribute.None)},
            {AstateIndex.SDamageH,  new AstateRecord( AclipTypeIndex.SDamageH, "Base Layer.", "SDamageH", AstateAttribute.None)},

            {AstateIndex.JDamageL,  new AstateRecord( AclipTypeIndex.JDamageL, "Base Layer.", "JDamageL", AstateAttribute.None)},
            {AstateIndex.JDamageM,  new AstateRecord( AclipTypeIndex.JDamageM, "Base Layer.", "JDamageM", AstateAttribute.None)},
            {AstateIndex.JDamageH,  new AstateRecord( AclipTypeIndex.JDamageH, "Base Layer.", "JDamageH", AstateAttribute.None)},

            {AstateIndex.DDamageL,  new AstateRecord( AclipTypeIndex.DDamageL, "Base Layer.", "DDamageL", AstateAttribute.None)},
            {AstateIndex.DDamageM,  new AstateRecord( AclipTypeIndex.DDamageM, "Base Layer.", "DDamageM", AstateAttribute.None)},
            {AstateIndex.DDamageH,  new AstateRecord( AclipTypeIndex.DDamageH, "Base Layer.", "DDamageH", AstateAttribute.None)},

            {AstateIndex.CDamageL,  new AstateRecord( AclipTypeIndex.CDamageL, "Base Layer.", "CDamageL", AstateAttribute.None)},
            {AstateIndex.CDamageM,  new AstateRecord( AclipTypeIndex.CDamageM, "Base Layer.", "CDamageM", AstateAttribute.None)},
            {AstateIndex.CDamageH,  new AstateRecord( AclipTypeIndex.CDamageH, "Base Layer.", "CDamageH", AstateAttribute.None)},
        };
        #endregion
    }

    public static Dictionary<AclipTypeIndex, AclipTypeRecord> aclipType_to_record;
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
    #region アニメーション・クリップの種類
    /// <summary>
    /// AnimationClip の種類に一対一対応☆
    /// </summary>
    public enum AclipTypeIndex
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
    /// Animator の state の名前と、AnimationClipの種類の対応☆　手作業で入力しておく（２度手間）
    /// </summary>
    public static Dictionary<AstateIndex, AstateRecord> astate_to_record;
    /// <summary>
    /// Animator の state の hash を、AnimationClipの種類に変換☆
    /// </summary>
    public static Dictionary<int, AclipTypeIndex> astateHash_to_aclipType;
    /// <summary>
    /// Animator の state の hash を、state番号に変換☆
    /// </summary>
    public static Dictionary<int, AstateIndex> astateHash_to_astate;
    /// <summary>
    /// シーンの Start( )メソッドで呼び出してください。
    /// </summary>
    public static void ReadAstateHashs()
    {
        astateHash_to_aclipType = new Dictionary<int, AclipTypeIndex>();
        astateHash_to_astate = new Dictionary<int, AstateIndex>();

        for (int iAstate = 0; iAstate <(int)AstateIndex.Num; iAstate++)
        {
            //if (!astate_to_record.ContainsKey((AstateIndex)iAstate))
            //{
            //    throw new UnityException("ステート["+ iAstate + "]に対応するアニメーション・クリップ種類が無いぜ☆");
            //}
            string breadCrumb = astate_to_record[(AstateIndex)iAstate].breadCrumb;
            string name = astate_to_record[(AstateIndex)iAstate].name;
            int hash = Animator.StringToHash(breadCrumb + name);
            astateHash_to_astate.Add(hash, (AstateIndex)iAstate);

            AclipTypeIndex aclipType = astate_to_record[(AstateIndex)iAstate].aclipType;
            astateHash_to_aclipType.Add(hash, aclipType);
            Debug.Log("ReadAstateHashs: iAstate = " + iAstate + " breadCrumb = " + breadCrumb + " name = " + name + " hash = " + hash + " aclipType = " + aclipType );
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
    public static void Select(out int serialImageIndex, out int slice, CharacterIndex character, AclipTypeRecord aclipTypeRecord, int currentMotionFrame)
    {
        slice = aclipTypeRecord.slices[currentMotionFrame];

        serialImageIndex = Hitbox2DDatabaseScript.GetSerialImageIndex(character, aclipTypeRecord.actioning);
    }

}
