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
        Player_To_Computer = new bool[] { true, true };
        Player_To_UseCharacter = new CharacterIndex[] { CharacterIndex.Kifuwarabe, CharacterIndex.Kifuwarabe };
        Teban = PlayerIndex.Player1;
    }

    public static string[] scene_to_name = new [] { "Title", "Select", "Main", "Result" };

    /// <summary>
    /// [player,button]
    /// 内部的には　プレイヤー１はP0、プレイヤー２はP1 だぜ☆（＾▽＾）
    /// 入力類は、コンフィグ画面でユーザーの目に触れる☆（＾～＾）
    /// ユーザーの目に見えるところでは 1スタート、内部的には 0スタートだぜ☆（＾▽＾）
    /// </summary>
    public static string[,] PlayerAndInput_To_InputName = new string[2, (int)InputIndex.Num] {
        { "Horizontal", "Vertical","P1LightPunch","P1MediumPunch","P1HardPunch","P1LightKick","P1MediumKick","P1HardKick","P1Pause"},
        { "P2Horizontal", "P2Vertical","P2LightPunch","P2MediumPunch","P2HardPunch","P2LightKick","P2MediumKick","P2HardKick","P2Pause"},
    };
    public const string INPUT_10_CA = "Cancel";

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
    /// セレクト画面と、リザルト画面で使う、顔☆
    /// </summary>
    public static string[,] CharacterAndSlice_To_FaceSprites = new string[,]{
        { "Sprites/Face0", "Face0_0", "Face0_1" },
        { "Sprites/Face1", "Face1_0", "Face1_1" },
        { "Sprites/Face2", "Face2_0", "Face2_1" },
        { "Sprites/Face3", "Face3_0", "Face3_1" },
    };
}

namespace SceneSelect
{
    public class SceneCommon
    {
        /// <summary>
        /// [x] セレクト画面でのキャラクターの並び順 
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
    }
}

namespace SceneMain
{
    /// <summary>
    /// キャラクターの行動、キャラクター画像とも対応。Unityで数字を直打ちしているので、対応する数字も保つこと。
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
    public enum HitboxIndex
    {
        Hitbox,
        Weakbox,
        Strongbox,
        Num
    }
    //public enum WeightIndex
    //{
    //    Light,
    //    Medium,
    //    Hard
    //}

    public class SceneCommon
    {
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

        public const string BOOL_PUSHING_LP = "pushingLP";
        public const string BOOL_PUSHING_MP = "pushingMP";
        public const string BOOL_PUSHING_HP = "pushingHP";
        public const string BOOL_PUSHING_LK = "pushingLK";
        public const string BOOL_PUSHING_MK = "pushingMK";
        public const string BOOL_PUSHING_HK = "pushingHK";
        public const string BOOL_PUSHING_PA = "pushingPA";
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

        public static string[] Player_To_Tag = new string[] { "Player0", "Player1" };
        public static string[,] PlayerAndHitbox_To_Sprite = new string[,] {
            { "Hitbox0", "Weakbox0", "Strongbox0", },
            { "Hitbox1", "Weakbox1", "Strongbox1", },
        };
        public static string[,] PlayerAndHitbox_To_Tag = new string[,] {
            { "Hitbox0", "Weakbox0", "Strongbox0", },
            { "Hitbox1", "Weakbox1", "Strongbox1", },
        };

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
        /// 画像を２．５倍角にして使っているぜ☆（＾～＾）
        /// </summary>
        public static float GRAPHIC_SCALE = 2.5f;

        public static string Prefab_TakoyakiParticle0 = "TakoyakiParticle0";
    }
}


namespace SceneResult
{
    public class SceneCommon
    {
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
    }
}
