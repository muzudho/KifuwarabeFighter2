using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackCollider2DDatabaseScript
{
    /// <summary>
    /// キャラクター１体あたり、何枚の画像で構成されているか。
    /// </summary>
    private const int DIVIDED_IMAGES = 1; //5;


    /// <summary>
    /// キャラクターと画像種類番号から、通し画像番号を取得。
    /// </summary>
    /// <param name="character"></param>
    /// <param name="image"></param>
    /// <returns></returns>
    public static int GetSerialImageIndex(CharacterIndex character, CharacterImageIndex image)
    {
        return (int)character * DIVIDED_IMAGES + (int)image;
    }

    public static MotionIndex ClipName_to_Motion(CharacterIndex character, string clipName)
    {
        if (CommonScript.CharacterAndMotion_To_Clip[(int)character, (int)MotionIndex.StandWait] == clipName)
        {
            return MotionIndex.StandWait;// 立ち待機
        }
        else if (CommonScript.CharacterAndMotion_To_Clip[(int)character, (int)MotionIndex.StandLP] == clipName)
        {
            return MotionIndex.StandLP; // 立ち弱パンチ
        }
        else if (CommonScript.CharacterAndMotion_To_Clip[(int)character, (int)MotionIndex.StandMP] == clipName)
        {
            return MotionIndex.StandMP;// 立ち中パンチ
        }
        else if (CommonScript.CharacterAndMotion_To_Clip[(int)character, (int)MotionIndex.StandHP] == clipName)
        {
            return MotionIndex.StandHP;// 立ち強パンチ
        }
        else if (CommonScript.CharacterAndMotion_To_Clip[(int)character, (int)MotionIndex.StandLK] == clipName)
        {
            return MotionIndex.StandLK;// 立ち弱キック
        }
        else if (CommonScript.CharacterAndMotion_To_Clip[(int)character, (int)MotionIndex.StandMK] == clipName)
        {
            return MotionIndex.StandMK;// 立ち中キック
        }
        else if (CommonScript.CharacterAndMotion_To_Clip[(int)character, (int)MotionIndex.StandHK] == clipName)
        {
            return MotionIndex.StandHK;// 立ち強キック
        }

        //throw new UnityException("モーション判別不能☆（＾▽＾） character = " + character + " clipName = " + clipName);
        // 当たり判定が無いなどの理由で、モーションを判別しなくてよいもの。
        return MotionIndex.Num;
    }

    public static void Select(out int serialImageIndex, out int slice, CharacterIndex character, MotionIndex motion, int currentMotionFrame)
    {
        CharacterImageIndex characterImageIndex = CharacterImageIndex.Num;
        slice = -1;

        switch(motion)
        {
            case MotionIndex.StandWait:
                {
                    // 立ち待機
                    characterImageIndex = CharacterImageIndex.Stand;
                    switch (currentMotionFrame)
                    {
                        case 0: slice = 0; break;
                        case 1: slice = 1; break;
                        case 2: slice = 2; break;
                        case 3: slice = 3; break;
                    }
                }
                break;
            case MotionIndex.StandLP:
                {
                    // 立ち弱パンチ
                    characterImageIndex = CharacterImageIndex.Stand;
                    switch (currentMotionFrame)
                    {
                        case 0: slice = 8; break;
                        case 1: slice = 9; break;
                    }
                }
                break;
            case MotionIndex.StandMP:
                {
                    // 立ち中パンチ
                    characterImageIndex = CharacterImageIndex.Stand;
                    switch (currentMotionFrame)
                    {
                        case 0: slice = 8; break;
                        case 1: slice = 9; break;
                        case 2: slice = 10; break;
                    }
                }
                break;
            case MotionIndex.StandHP:
                {
                    // 立ち強パンチ
                    characterImageIndex = CharacterImageIndex.Stand;
                    switch (currentMotionFrame)
                    {
                        case 0: slice = 8; break;
                        case 1: slice = 9; break;
                        case 2: slice = 10; break;
                        case 3: slice = 11; break;
                        case 4: slice = 9; break;
                    }
                }
                break;
            case MotionIndex.StandLK:
                {
                    // 立ち弱キック
                    characterImageIndex = CharacterImageIndex.Stand;
                    switch (currentMotionFrame)
                    {
                        case 0: slice = 16; break;
                        case 1: slice = 17; break;
                    }
                }
                break;
            case MotionIndex.StandMK:
                {
                    // 立ち中キック
                    characterImageIndex = CharacterImageIndex.Stand;
                    switch (currentMotionFrame)
                    {
                        case 0: slice = 16; break;
                        case 1: slice = 17; break;
                        case 2: slice = 18; break;
                    }
                }
                break;
            case MotionIndex.StandHK:
                {
                    // 立ち強キック
                    characterImageIndex = CharacterImageIndex.Stand;
                    switch (currentMotionFrame)
                    {
                        case 0: slice = 16; break;
                        case 1: slice = 17; break;
                        case 2: slice = 18; break;
                        case 3: slice = 19; break;
                        case 4: slice = 17; break;
                    }
                }
                break;
            default:
                break;
        }

        serialImageIndex = AttackCollider2DDatabaseScript.GetSerialImageIndex(character, characterImageIndex);
    }
}
