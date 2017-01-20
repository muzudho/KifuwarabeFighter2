using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hitbox2DOperationScript
{
    public static float GetOffsetX(HitboxIndex hitbox, int serialImage, int slice)
    {
        switch (hitbox)
        {
            case HitboxIndex.Hitbox: return Hitbox2DScript_Hitbox.imageAndSlice_To_OffsetX[serialImage, slice];
            case HitboxIndex.Weakbox: return Hitbox2DScript_Weakbox.imageAndSlice_To_OffsetX[serialImage, slice];
            case HitboxIndex.Strongbox: return Hitbox2DScript_Strongbox.imageAndSlice_To_OffsetX[serialImage, slice];
            default: throw new UnityException("未定義のヒットボックス☆");
        }
    }
    public static float GetOffsetY(HitboxIndex hitbox, int serialImage, int slice)
    {
        switch (hitbox)
        {
            case HitboxIndex.Hitbox: return Hitbox2DScript_Hitbox.imageAndSlice_To_OffsetY[serialImage, slice];
            case HitboxIndex.Weakbox: return Hitbox2DScript_Weakbox.imageAndSlice_To_OffsetY[serialImage, slice];
            case HitboxIndex.Strongbox: return Hitbox2DScript_Strongbox.imageAndSlice_To_OffsetY[serialImage, slice];
            default: throw new UnityException("未定義のヒットボックス☆");
        }
    }
    public static float GetScaleX(HitboxIndex hitbox, int serialImage, int slice)
    {
        switch (hitbox)
        {
            case HitboxIndex.Hitbox: return Hitbox2DScript_Hitbox.imageAndSlice_To_ScaleX[serialImage, slice];
            case HitboxIndex.Weakbox: return Hitbox2DScript_Weakbox.imageAndSlice_To_ScaleX[serialImage, slice];
            case HitboxIndex.Strongbox: return Hitbox2DScript_Strongbox.imageAndSlice_To_ScaleX[serialImage, slice];
            default: throw new UnityException("未定義のヒットボックス☆");
        }
    }
    public static float GetScaleY(HitboxIndex hitbox, int serialImage, int slice)
    {
        switch (hitbox)
        {
            case HitboxIndex.Hitbox: return Hitbox2DScript_Hitbox.imageAndSlice_To_ScaleY[serialImage, slice];
            case HitboxIndex.Weakbox: return Hitbox2DScript_Weakbox.imageAndSlice_To_ScaleY[serialImage, slice];
            case HitboxIndex.Strongbox: return Hitbox2DScript_Strongbox.imageAndSlice_To_ScaleY[serialImage, slice];
            default: throw new UnityException("未定義のヒットボックス☆");
        }
    }

    /// <summary>
    /// キャラクターと画像種類番号から、通し画像番号を取得。
    /// </summary>
    /// <param name="character"></param>
    /// <param name="actioning"></param>
    /// <returns></returns>
    public static int GetSerialImageIndex(CharacterIndex character, ActioningIndex actioning)
    {
        return (int)character * (int)ActioningIndex.Num + (int)actioning;
    }
}
