using UnityEngine;

public enum SceneIndex
{
    Title,
    Select,
    Main,
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
/// プレイヤー番号。
/// </summary>
public enum PlayerIndex
{
    Player1,
    Player2,
    Num
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
public enum InputIndex
{
    Horizontal,
    Vertical,
    LightPunch,
    MediumPunch,
    HardPunch,
    LightKick,
    MediumKick,
    HardKick,
    Pause,
    Num
}

// どこからでも使われるぜ☆
public class CommonScript
{
    static CommonScript()
    {
        Result = Result.None;
        Player_to_computer = new bool[] { true, true };
        Player_to_useCharacter = new CharacterIndex[] { CharacterIndex.Kifuwarabe, CharacterIndex.Kifuwarabe };
        Teban = PlayerIndex.Player1;
    }

    public static string[] Scene_to_name = new [] { "Title", "Select", "Main", "Result" };

    /// <summary>
    /// [player,button]
    /// 内部的には　プレイヤー１はP0、プレイヤー２はP1 だぜ☆（＾▽＾）
    /// 入力類は、コンフィグ画面でユーザーの目に触れる☆（＾～＾）
    /// ユーザーの目に見えるところでは 1スタート、内部的には 0スタートだぜ☆（＾▽＾）
    /// </summary>
    public static string[,] PlayerAndInput_to_inputName = new string[2, (int)InputIndex.Num] {
        { "Horizontal", "Vertical","P1LightPunch","P1MediumPunch","P1HardPunch","P1LightKick","P1MediumKick","P1HardKick","P1Pause"},
        { "P2Horizontal", "P2Vertical","P2LightPunch","P2MediumPunch","P2HardPunch","P2LightKick","P2MediumKick","P2HardKick","P2Pause"},
    };
    public const string INPUT_10_CA = "Cancel";

    public static Result Result { get; set; }
    /// <summary>
    /// 人間か、コンピューターか。
    /// </summary>
    public static bool[] Player_to_computer { get; set; }
    /// <summary>
    /// [Player] プレイヤーの使用キャラクター。
    /// </summary>
    public static CharacterIndex[] Player_to_useCharacter { get; set; }
    public static PlayerIndex Teban { get; set; }
    public static PlayerIndex ReverseTeban(PlayerIndex player)
    {
        switch (player)
        {
            case PlayerIndex.Player1: return PlayerIndex.Player2;
            case PlayerIndex.Player2: return PlayerIndex.Player1;
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
