using System.Collections.Generic;
using Assets.Scripts.Model.Dto.Input;

namespace SceneSelect
{
    public enum GameobjectIndex
    {
        Player,
        Name,
        Face,
        BoxBack,
        Box,
        Turn
    }

    public class SceneCommon
    {
        static SceneCommon()
        {
            TransitionTime = 0;
        }

        public const string TRIGGER_STAY = "stay";
        public const string TRIGGER_MOVE = "move";
        public const string TRIGGER_SELECT = "select";
        public const string TRIGGER_TIMEOVER = "timeover";

        public static Dictionary<PlayerIndex, string[]> GameobjectToPath = new Dictionary<PlayerIndex, string[]>()
        {
            {PlayerIndex.Player1,new string[]{ "Canvas/Player0","Canvas/Name0","Canvas/Face0","Canvas/Box0Back","Canvas/Box0","Canvas/Turn0",} },
            {PlayerIndex.Player2,new string[]{ "Canvas/Player1","Canvas/Name1","Canvas/Face1","Canvas/Box1Back","Canvas/Box1","Canvas/Turn1",} },
        };

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

        public static float[] BoxColumn_to_locationX = new[] { -150.0f, 0.0f, 150.0f };

        public static Dictionary<PlayerIndex, float> locationYs = new Dictionary<PlayerIndex, float>()
        {
            {PlayerIndex.Player1, -124.0f },
            {PlayerIndex.Player2, -224.0f },
        };

        public static int TransitionTime;
    }
}
