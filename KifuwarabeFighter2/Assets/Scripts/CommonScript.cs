using UnityEngine;

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
    Roborinko
}
public enum PlayerCharacterSpritesIndex
{
    All,
    Win,
    Lose
}
public enum ButtonIndex
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
public enum WeightIndex
{
    Light,
    Medium,
    Hard
}
public enum MotionIndex
{
    Wait,
    LP,
    MP,
    HP,
    LK,
    MK,
    HK,
}

// どこからでも使われるぜ☆
public class CommonScript
{ //: MonoBehaviour

    static CommonScript()
    {
        Result = Result.None;
        Player_To_UseCharacter = new CharacterIndex[] { CharacterIndex.Kifuwarabe, CharacterIndex.Kifuwarabe };
        Teban = PlayerIndex.Player1;
    }

    public const string BUTTON_01_P1_HO = "Horizontal";
    public const string BUTTON_02_P1_VE = "Vertical";
    public const string BUTTON_03_P1_LP = "P1LightPunch";
    public const string BUTTON_04_P1_MP = "P1MediumPunch";
    public const string BUTTON_05_P1_HP = "P1HardPunch";
    public const string BUTTON_06_P1_LK = "P1LightKick";
    public const string BUTTON_07_P1_MK = "P1MediumKick";
    public const string BUTTON_08_P1_HK = "P1HardKick";
    public const string BUTTON_09_P1_PA = "P1Pause";
    public const string BUTTON_10_CA = "Cancel";
    public const string BUTTON_11_P2_HO = "P2Horizontal";
    public const string BUTTON_12_P2_VE = "P2Vertical";
    public const string BUTTON_13_P2_LP = "P2LightPunch";
    public const string BUTTON_14_P2_MP = "P2MediumPunch";
    public const string BUTTON_15_P2_HP = "P2HardPunch";
    public const string BUTTON_16_P2_LK = "P2LightKick";
    public const string BUTTON_17_P2_MK = "P2MediumKick";
    public const string BUTTON_18_P2_HK = "P2HardKick";
    public const string BUTTON_19_P2_PA = "P2Pause";
    /// <summary>
    /// [player,button]
    /// </summary>
    public static string[,] PlayerAndButton_To_ButtonName = new string[2, (int)ButtonIndex.Num] {
        { BUTTON_01_P1_HO, BUTTON_02_P1_VE,BUTTON_03_P1_LP,BUTTON_04_P1_MP,BUTTON_05_P1_HP,BUTTON_06_P1_LK,BUTTON_07_P1_MK,BUTTON_08_P1_HK,BUTTON_09_P1_PA},
        { BUTTON_11_P2_HO, BUTTON_12_P2_VE,BUTTON_13_P2_LP,BUTTON_14_P2_MP,BUTTON_15_P2_HP,BUTTON_16_P2_LK,BUTTON_17_P2_MK,BUTTON_18_P2_HK,BUTTON_19_P2_PA},
    };

    public static Result Result { get; set; }
    /// <summary>
    /// [Player] プレイヤーの使用キャラクター。
    /// </summary>
    public static CharacterIndex[] Player_To_UseCharacter { get; set; }
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
    /// [x] キャラクター選択画面での並び順 
    /// </summary>
    public static CharacterIndex[] X_To_CharacterInSelectMenu = new CharacterIndex[]
    {
        CharacterIndex.Kifuwarabe, CharacterIndex.Roborinko, CharacterIndex.Ponahiko
    };
    /// <summary>
    /// [character]
    /// </summary>
    public static string[] Character_To_Name = new string[]
    {
        "きふわらべ", "パナ彦", "ろぼりん娘"
    };
    public static string[] Character_To_NameRoma = new string[]
    {
        "KifuWarabe", "Panahiko", "Roborinko"
    };
    public static string[] Character_To_AnimationController = new string[]
    {
        "AnimatorControllers/Middle@Char1",
        "AnimatorControllers/Middle@Char2",
        "AnimatorControllers/Middle@Char3"
    };
    /// <summary>
    /// [character,スライス番号]
    /// </summary>
    public static string[,] CharacterAndSlice_To_FaceSprites = new string[3,3]{
        { "Sprites/Face1", "Face1_0", "Face1_1" },
        { "Sprites/Face2", "Face2_0", "Face2_1" },
        { "Sprites/Face3", "Face3_0", "Face3_1" },
    };
    public static string[,] CharacterAndMotion_To_Slice = new string[,]
    {
        { "Char1_Wait", "Char1_LP", "Char1_MP", "Char1_HP", "Char1_LK", "Char1_MK", "Char1_HK", },
        { "Char2_Wait", "Char2_LP", "Char2_MP", "Char2_HP", "Char2_LK", "Char2_MK", "Char2_HK", },
        { "Char3_Wait", "Char3_LP", "Char3_MP", "Char3_HP", "Char3_LK", "Char3_MK", "Char3_HK", },
    };
    public static string[] PlayerTags = new string[] { "Char1", "Char2" };

    /// <summary>
    /// 画像を２倍角にして使っているぜ☆（＾～＾）
    /// </summary>
    public static float GRAPHIC_SCALE = 2.0f;

    /// <summary>
    /// [character]
    /// </summary>
    public static string[] Character_To_WinMessage = new string[]
    {
        "公園（くに）へ帰れだぜ☆！\nお前にもお父んがいるだろう☆",
        "努力あるのみ☆\n",
        "あかごの方が　はごたえがあるのじゃ！\n辱め詰めで追い回してやろう！"
    };
}
