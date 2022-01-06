namespace Assets.Scripts.Model.Dto.Fight
{
    using System.Collections.Generic;
    using Assets.Scripts.Model.Dto.Input;
    using UnityEngine;

    public class ThisSceneDto
    {
        public const string TriggerMoveX = "moveX";
        public const string TriggerMoveXForward = "moveXForward";
        public const string TriggerMoveXBack = "moveXBack";
        public const string TriggerJump = "jump";
        public const string TriggerCrouch = "crouch";
        public const string TriggerAtkLp = "atkLP";
        public const string TriggerAtkMp = "atkMP";
        public const string TriggerAtkHp = "atkHP";
        public const string TriggerAtkLk = "atkLK";
        public const string TriggerAtkMk = "atkMK";
        public const string TriggerAtkHk = "atkHK";
        public const string TriggerDown = "down";
        public const string TriggerDamageL = "damageL";
        public const string TriggerDamageM = "damageM";
        public const string TriggerDamageH = "damageH";
        public const string TriggerGiveUp = "giveup";
        public const string TriggerBlock = "block";
        public const string TriggerDeblock = "deblock";

        public const string FloatVelY = "velY";

        public const string BoolIsGrounded = "isGrounded";
        public const string BoolGiveUping = "giveuping";
        public const string BoolJMove0 = "jmove0";

        public const string IntegerLeverXPressing = "leverXPressing";
        public const string IntegerLeverXNeutral = "leverXNeutral";
        public const string IntegerLeverXIdol = "leverXIdol";
        public const string IntegerLeverYPressing = "leverYPressing";
        public const string IntegerLeverYNeutral = "leverYNeutral";
        public const string IntegerLeverYIdol = "leverYIdol";
        public const string IntegerActioning = "actioning";

        public const string GameObjFight0 = "Canvas/Fight0";
        public const string GameObjFight1 = "Canvas/Fight1";
        public const string GameObjResign0 = "Canvas/Resign0";

        public static string[] CharacterToNameRoma = new[]
        {
            "KifuWarabe", "Panahiko", "Roborinko", "TofuMan"
        };
        public static string[] CharacterToAnimationController = new[]
        {
            "AnimatorControllers/Main_Char0",
            "AnimatorControllers/Main_Char1",
            "AnimatorControllers/Main_Char2",
            "AnimatorControllers/Main_Char3",
        };

        public static string[] PlayerToTag = new[] { "Player0", "Player1" };
        public static Dictionary<PlayerKey, string[]> GameObjectPaths = new Dictionary<PlayerKey, string[]>
        {
            {PlayerKey.N1, new string[]{ "Player0","Canvas/Name0","Canvas/Bar0","Canvas/Value0","Canvas/Turn0","Canvas/Time0",} },
            {PlayerKey.N2, new string[]{ "Player1","Canvas/Name1","Canvas/Bar1","Canvas/Value1","Canvas/Turn1","Canvas/Time1",} },
        };

        public static Dictionary<PlayerKey, string[]> HitboxPaths = new Dictionary<PlayerKey, string[]>
        {
            {PlayerKey.N1,new string[]{ "Hitbox0", "Weakbox0", "Strongbox0", } },
            {PlayerKey.N2,new string[]{ "Hitbox1", "Weakbox1", "Strongbox1", } },
        };
        public static Dictionary<PlayerKey, string[]> HitboxTags = new Dictionary<PlayerKey, string[]> {
            {PlayerKey.N1,new string[]{ "Hitbox0", "Weakbox0", "Strongbox0", } },
            {PlayerKey.N2,new string[]{ "Hitbox1", "Weakbox1", "Strongbox1", } },
        };

        /// <summary>
        /// 開始台詞
        /// </summary>
        public static string[] CharacterToFightMessage = new[]
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
        public static Dictionary<PlayerKey, Transform> PlayerToTransform = new Dictionary<PlayerKey, Transform>()
        {
            {PlayerKey.N1, null },
            {PlayerKey.N2, null },
        };

        public static string PrefabTakoyakiParticle0 = "TakoyakiParticle0";

        /// <summary>
        /// Ready message presentation time; 対局開始メッセージが表示されている時間。
        /// </summary>
        public const int ReadyTimeLength = 60;
    }
}
