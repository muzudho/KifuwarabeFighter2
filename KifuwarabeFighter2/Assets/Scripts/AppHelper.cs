namespace Assets.Scripts
{
    using Assets.Scripts.Models.Input;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// どこからでも使われるぜ☆
    /// </summary>
    public static class AppHelper
    {
        static AppHelper()
        {
            Result = ResultKey.None;
            ComputerFlags = new Dictionary<PlayerKey, bool>
            {
                { PlayerKey.N1, true },
                { PlayerKey.N2, true },
            };

            UseCharacters = new Dictionary<PlayerKey, CharacterKey>()
            {
                { PlayerKey.N1, CharacterKey.Kifuwarabe },
                { PlayerKey.N2, CharacterKey.Kifuwarabe },
            };

            Teban = PlayerKey.N1;
        }

        public static string[] sceneToName = new[] { "Title", "Select", "Fight", "Result" };

        public static ResultKey Result { get; set; }

        /// <summary>
        /// 人間か、コンピューターか。
        /// </summary>
        public static Dictionary<PlayerKey, bool> ComputerFlags { get; set; }

        /// <summary>
        /// [Player] プレイヤーの使用キャラクター。
        /// </summary>
        public static Dictionary<PlayerKey, CharacterKey> UseCharacters { get; set; }

        public static PlayerKey Teban { get; set; }

        public static PlayerKey ReverseTeban(PlayerKey player)
        {
            switch (player)
            {
                case PlayerKey.N1: return PlayerKey.N2;
                case PlayerKey.N2: return PlayerKey.N1;
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
}