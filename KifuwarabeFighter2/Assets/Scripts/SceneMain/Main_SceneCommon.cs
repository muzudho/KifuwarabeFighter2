using UnityEngine;

namespace SceneMain
{
    //public enum WeightIndex
    //{
    //    Light,
    //    Medium,
    //    Hard
    //}
    public enum GameobjectIndex
    {
        Player,
        Name,
        Bar,
        Value,
        Turn,
        Time,
    }
    /// <summary>
    /// 相手に向かっていっているかどうか。動いていないときは Stay で☆
    /// </summary>
    public enum FacingOpponentMoveFwBkSt
    {
        Forward,
        Back,
        Stay
    }
    /// <summary>
    /// 相手は左か右か。めんどうなので、相手に重なっている、Equal は無しで。
    /// </summary>
    public enum FacingOpponentLR
    {
        Left,
        Right
    }

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
        public const string TRIGGER_BLOCK = "block";
        public const string TRIGGER_DEBLOCK = "deblock";
        
        public const string FLOAT_VEL_Y = "velY";

        public const string BOOL_IS_GROUNDED = "isGrounded";
        public const string BOOL_GIVEUPING = "giveuping";
        public const string BOOL_JMOVE0 = "jmove0";

        public const string INTEGER_LEVER_X_PRESSING = "leverXPressing";
        public const string INTEGER_LEVER_X_NEUTRAL = "leverXNeutral";
        public const string INTEGER_LEVER_X_IDOL = "leverXIdol";
        public const string INTEGER_LEVER_Y_PRESSING = "leverYPressing";
        public const string INTEGER_LEVER_Y_NEUTRAL = "leverYNeutral";
        public const string INTEGER_LEVER_Y_IDOL = "leverYIdol";
        public const string INTEGER_ACTIONING = "actioning";

        public const string GAMEOBJ_FIGHT0 = "Canvas/Fight0";
        public const string GAMEOBJ_FIGHT1 = "Canvas/Fight1";
        public const string GAMEOBJ_RESIGN0 = "Canvas/Resign0";

        public static string[] Character_to_nameRoma = new[]
        {
            "KifuWarabe", "Panahiko", "Roborinko", "TofuMan"
        };
        public static string[] Character_to_animationController = new[]
        {
            "AnimatorControllers/Main_Char0",
            "AnimatorControllers/Main_Char1",
            "AnimatorControllers/Main_Char2",
            "AnimatorControllers/Main_Char3",
        };

        public static string[] Player_to_tag = new[] { "Player0", "Player1" };
        public static string[,] PlayerAndGameobject_to_path = new[,]
        {
            { "Player0","Canvas/Name0","Canvas/Bar0","Canvas/Value0","Canvas/Turn0","Canvas/Time0",},
            { "Player1","Canvas/Name1","Canvas/Bar1","Canvas/Value1","Canvas/Turn1","Canvas/Time1",},
        };

        public static string[,] PlayerAndHitbox_to_path = new[,] {
            { "Hitbox0", "Weakbox0", "Strongbox0", },
            { "Hitbox1", "Weakbox1", "Strongbox1", },
        };
        public static string[,] PlayerAndHitbox_to_tag = new[,] {
            { "Hitbox0", "Weakbox0", "Strongbox0", },
            { "Hitbox1", "Weakbox1", "Strongbox1", },
        };

        /// <summary>
        /// 開始台詞
        /// </summary>
        public static string[] Character_to_fightMessage = new[]
        {
            "定刻になりましたので\nきふわらべ３３位の先手で\n始めてください",
            "定刻になりましたので\nパナ彦１位の先手で\n始めてください",
            "定刻になりましたので\nろぼりん娘２９位の先手で\n始めてください",
            "定刻になりましたので\n豆腐マン失格の先手で\n始めてください",
        };

        /// <summary>
        /// player position x for facing opponent.
        /// 相手と向かい合うために使うプレイヤーのX座標だぜ☆（＾▽＾）x位置を共有するためのものだぜ☆
        /// </summary>
        public static Transform[] Player_to_transform = new Transform[] { null, null };

        public static string Prefab_TakoyakiParticle0 = "TakoyakiParticle0";

        /// <summary>
        /// Ready message presentation time; 対局開始メッセージが表示されている時間。
        /// </summary>
        public const int READY_TIME_LENGTH = 60;
    }
}
