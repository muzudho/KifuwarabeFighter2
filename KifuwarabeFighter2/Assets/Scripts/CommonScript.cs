using UnityEngine;
using System.Text;

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
/// <summary>
/// キャラクターの行動、キャラクター画像とも対応。Unityで数字を直打ちしているので、数字も保つこと。
/// </summary>
public enum ActioningIndex
{
    /// <summary>
    /// [0]
    /// </summary>
    Stand,
    Jump,
    Dash,
    Crouch,
    Other,
    /// <summary>
    /// 列挙型の要素数、または未使用の値として使用。
    /// </summary>
    Num
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
//public enum WeightIndex
//{
//    Light,
//    Medium,
//    Hard
//}

// どこからでも使われるぜ☆
public class CommonScript
{

    static CommonScript()
    {
        Result = Result.None;
        Player_To_Computer = new bool[] { true, true };
        Player_To_UseCharacter = new CharacterIndex[] { CharacterIndex.Kifuwarabe, CharacterIndex.Kifuwarabe };
        Teban = PlayerIndex.Player1;

        //StringBuilder sb = new StringBuilder();
        //for (int iChar = 0; iChar < (int)CharacterIndex.Num; iChar++)
        //{
        //    for (int iAstate = 0; iAstate < (int)MotionDatabaseScript.AstateIndex.Num; iAstate++)
        //    {
        //        sb.Append("Char");
        //        sb.Append(iChar);
        //        sb.Append("@");
        //        sb.Append(MotionDatabaseScript.astate_to_record[(MotionDatabaseScript.AstateIndex)iAstate].name);
        //        CharacterAndAstate_To_Clip[iChar, iAstate] = sb.ToString();
        //        //Debug.Log("CommonScript: iChar = " + iChar + " iMotion = " + iMotion + " : " + sb.ToString());
        //        sb.Length = 0;
        //    }
        //}
    }

    public const string SCENE_TITLE = "Title";
    public const string SCENE_SELECT = "Select";
    public const string SCENE_MAIN = "Main";
    public const string SCENE_RESULT = "Result";

    /// <summary>
    /// BUTTON はコンフィグ画面でユーザーの目に触れるので、プレイヤー１はP1、プレイヤー２はP2 だぜ☆（＾～＾）
    /// </summary>
    public const string BUTTON_01_P0_HO = "Horizontal";
    public const string BUTTON_02_P0_VE = "Vertical";
    public const string BUTTON_03_P0_LP = "P1LightPunch";
    public const string BUTTON_04_P0_MP = "P1MediumPunch";
    public const string BUTTON_05_P0_HP = "P1HardPunch";
    public const string BUTTON_06_P0_LK = "P1LightKick";
    public const string BUTTON_07_P0_MK = "P1MediumKick";
    public const string BUTTON_08_P0_HK = "P1HardKick";
    public const string BUTTON_09_P0_PA = "P1Pause";
    public const string BUTTON_10_CA = "Cancel";
    public const string BUTTON_11_P1_HO = "P2Horizontal";
    public const string BUTTON_12_P1_VE = "P2Vertical";
    public const string BUTTON_13_P1_LP = "P2LightPunch";
    public const string BUTTON_14_P1_MP = "P2MediumPunch";
    public const string BUTTON_15_P1_HP = "P2HardPunch";
    public const string BUTTON_16_P1_LK = "P2LightKick";
    public const string BUTTON_17_P1_MK = "P2MediumKick";
    public const string BUTTON_18_P1_HK = "P2HardKick";
    public const string BUTTON_19_P1_PA = "P2Pause";

    public const string TRIGGER_MOVE_X = "moveX";
    public const string TRIGGER_MOVE_X_FORWARD = "moveXForward";
    public const string TRIGGER_MOVE_X_BACK = "moveXBack";
    public const string TRIGGER_JUMP = "jump";
    public const string TRIGGER_CROUCH = "crouch";
    public const string TRIGGER_ATK_LP = "atkLP";
    public const string TRIGGER_ATK_MP = "atkMP";
    public const string TRIGGER_ATK_HP = "atkHP";
    public const string TRIGGER_ATK_LK = "atkLK";
    public const string TRIGGER_ATK_MK = "atkMK";
    public const string TRIGGER_ATK_HK = "atkHK";
    public const string TRIGGER_DOWN = "down";
    public const string TRIGGER_DAMAGE_L = "damageL";
    public const string TRIGGER_DAMAGE_M = "damageM";
    public const string TRIGGER_DAMAGE_H = "damageH";
    public const string TRIGGER_GIVEUP = "giveup";

    public const string BOOL_ATTACKING = "attacking";
    public const string BOOL_BACKSTEPING = "backsteping";
    public const string BOOL_PUSHING_LP = "pushingLP";
    public const string BOOL_PUSHING_MP = "pushingMP";
    public const string BOOL_PUSHING_HP = "pushingHP";
    public const string BOOL_PUSHING_LK = "pushingLK";
    public const string BOOL_PUSHING_MK = "pushingMK";
    public const string BOOL_PUSHING_HK = "pushingHK";
    public const string BOOL_PUSHING_PA = "pushingPA";
    public const string BOOL_DOWNING = "downing";
    public const string BOOL_INVINCIBLE = "invincible";
    public const string BOOL_GIVEUPING = "giveuping";
    public const string BOOL_JMOVE0 = "jmove0";

    public const string INTEGER_LEVER_X_PRESSING = "leverXPressing";
    public const string INTEGER_LEVER_X_NEUTRAL = "leverXNeutral";
    public const string INTEGER_LEVER_X_IDOL = "leverXIdol";
    public const string INTEGER_LEVER_Y_PRESSING = "leverYPressing";
    public const string INTEGER_LEVER_Y_NEUTRAL = "leverYNeutral";
    public const string INTEGER_LEVER_Y_IDOL = "leverYIdol";
    public const string INTEGER_ACTIONING = "actioning";

    public const string SPRITE_FIGHT0 = "Canvas/Fight0";
    public const string SPRITE_FIGHT1 = "Canvas/Fight1";
    public const string SPRITE_RESIGN0 = "Canvas/Resign0";


    /// <summary>
    /// [player,button]
    /// 内部的には　プレイヤー１はP0、プレイヤー２はP1 だぜ☆（＾▽＾）
    /// </summary>
    public static string[,] PlayerAndButton_To_ButtonName = new string[2, (int)ButtonIndex.Num] {
        { BUTTON_01_P0_HO, BUTTON_02_P0_VE,BUTTON_03_P0_LP,BUTTON_04_P0_MP,BUTTON_05_P0_HP,BUTTON_06_P0_LK,BUTTON_07_P0_MK,BUTTON_08_P0_HK,BUTTON_09_P0_PA},
        { BUTTON_11_P1_HO, BUTTON_12_P1_VE,BUTTON_13_P1_LP,BUTTON_14_P1_MP,BUTTON_15_P1_HP,BUTTON_16_P1_LK,BUTTON_17_P1_MK,BUTTON_18_P1_HK,BUTTON_19_P1_PA},
    };

    public static Result Result { get; set; }
    /// <summary>
    /// 人間か、コンピューターか。
    /// </summary>
    public static bool[] Player_To_Computer { get; set; }
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
        CharacterIndex.Kifuwarabe, CharacterIndex.Roborinko, CharacterIndex.Ponahiko, CharacterIndex.TofuMan,
    };
    /// <summary>
    /// [character]
    /// </summary>
    public static string[] Character_To_Name = new string[]
    {
        "きふわらべ", "パナ彦", "ろぼりん娘", "豆腐マン"
    };
    public static string[] Character_To_NameRoma = new string[]
    {
        "KifuWarabe", "Panahiko", "Roborinko", "TofuMan"
    };
    public static string[] Character_To_AnimationController = new string[]
    {
        "AnimatorControllers/AniCon@Char0",
        "AnimatorControllers/AniCon@Char1",
        "AnimatorControllers/AniCon@Char2",
        "AnimatorControllers/AniCon@Char3",
    };
    /// <summary>
    /// [character]
    /// </summary>
    public static string Character_To_Attack(CharacterIndex character)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Sprites/Attack");
        sb.Append((int)character);
        sb.Append("a");
        return sb.ToString();
    }
    /// <summary>
    /// [character,スライス番号]
    /// </summary>
    public static string CharacterAndSlice_To_AttackSlice(CharacterIndex character, int slice){
        StringBuilder sb = new StringBuilder();
        sb.Append("Attack");
        sb.Append((int)character);
        sb.Append("a_");
        sb.Append(slice);
        return sb.ToString();
    }
    /// <summary>
    /// [character,スライス番号]
    /// </summary>
    public static string[,] CharacterAndSlice_To_FaceSprites = new string[,]{
        { "Sprites/Face0", "Face0_0", "Face0_1" },
        { "Sprites/Face1", "Face1_0", "Face1_1" },
        { "Sprites/Face2", "Face2_0", "Face2_1" },
        { "Sprites/Face3", "Face3_0", "Face3_1" },
    };
    //public static string[,] CharacterAndAstate_To_Clip = new string[(int)CharacterIndex.Num, (int)MotionDatabaseScript.AstateIndex.Num];
    public static string[] Player_To_Tag = new string[] { "Char0", "Char1" };
    public static string[] Player_To_Attacker = new string[] { "Attacker0", "Attacker1" };
    public static string[] Player_To_AttackerTag = new string[] { "Attacker0", "Attacker1" };

    /// <summary>
    /// 画像を２．５倍角にして使っているぜ☆（＾～＾）
    /// </summary>
    public static float GRAPHIC_SCALE = 2.5f;

    /// <summary>
    /// 開始台詞
    /// </summary>
    public static string[] Character_To_FightMessage = new string[]
    {
        "定刻になりましたので\nきふわらべ３３位の先手で\n始めてください",
        "定刻になりましたので\nパナ彦１位の先手で\n始めてください",
        "定刻になりましたので\nろぼりん娘２９位の先手で\n始めてください",
        "定刻になりましたので\n豆腐マン失格の先手で\n始めてください",
    };

    /// <summary>
    /// 勝利台詞
    /// </summary>
    public static string[] Character_To_WinMessage = new string[]
    {
        "公園（くに）へ帰れだぜ☆！\nお前にもお父んがいるだろう☆",
        "努力あるのみ☆\n",
        "あかごの方が　はごたえがあるのじゃ！\n辱め詰めで追い回してやろう！",
        "妥協の産物！\n",
    };

    public static string Prefab_TakoyakiParticle0 = "TakoyakiParticle0";
}
