﻿using System.Collections.Generic;
using Assets.Scripts.Model.Dto.Input;
using UnityEngine;

public enum SceneIndex
{
    Title,
    Select,
    Fight,
    Result,
    Num
}
/// <summary>
/// 対局の結果☆
/// </summary>
public enum Result
{
    Player1_Win,
    Player2_Win,
    //Double_KnockOut,
    None
}
/// <summary>
/// 使用キャラクター。
/// </summary>
public enum CharacterIndex
{
    Kifuwarabe,
    Ponahiko,
    Roborinko,
    TofuMan,
    /// <summary>
    /// 列挙型の要素数、または未使用の値として使用。
    /// </summary>
    Num,
}
public enum ResultFaceSpriteIndex
{
    All,
    Win,
    Lose
}

// どこからでも使われるぜ☆
public class CommonScript
{
    static CommonScript()
    {
        Result = Result.None;
        computerFlags = new Dictionary<PlayerNum, bool>
        {
            { PlayerNum.N1, true },
            { PlayerNum.N2, true },
        };
        UseCharacters = new Dictionary<PlayerNum, CharacterIndex>()
        {
            { PlayerNum.N1, CharacterIndex.Kifuwarabe },
            { PlayerNum.N2, CharacterIndex.Kifuwarabe },
        };
        Teban = PlayerNum.N1;
    }

    public static string[] Scene_to_name = new[] { "Title", "Select", "Fight", "Result" };

    public static Result Result { get; set; }
    /// <summary>
    /// 人間か、コンピューターか。
    /// </summary>
    public static Dictionary<PlayerNum, bool> computerFlags { get; set; }
    /// <summary>
    /// [Player] プレイヤーの使用キャラクター。
    /// </summary>
    public static Dictionary<PlayerNum, CharacterIndex> UseCharacters { get; set; }
    public static PlayerNum Teban { get; set; }
    public static PlayerNum ReverseTeban(PlayerNum player)
    {
        switch (player)
        {
            case PlayerNum.N1: return PlayerNum.N2;
            case PlayerNum.N2: return PlayerNum.N1;
            default: Debug.LogError("未定義のプレイヤー☆"); return player;
        }
    }

    /// <summary>
    /// セレクト画面と、リザルト画面で使う、顔☆
    /// </summary>
    public static string[,] CharacterAndSlice_to_faceSprites = new string[,]{
        { "Sprites/Face0", "Face0_0", "Face0_1" },
        { "Sprites/Face1", "Face1_0", "Face1_1" },
        { "Sprites/Face2", "Face2_0", "Face2_1" },
        { "Sprites/Face3", "Face3_0", "Face3_1" },
    };


}
