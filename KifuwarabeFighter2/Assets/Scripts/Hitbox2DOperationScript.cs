using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hitbox2DOperationScript
{
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
