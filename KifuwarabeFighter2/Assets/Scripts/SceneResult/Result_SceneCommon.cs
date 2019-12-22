using System.Collections.Generic;
using Assets.Scripts.Model.Dto.Input;

namespace SceneResult
{
    public enum GameobjectIndex
    {
        Face,
    }

    public class SceneCommon
    {
        public const string GAMEOBJ_TEXT = "Canvas/Text";

        public static Dictionary<PlayerIndex, string[]> GameobjectToPath = new Dictionary<PlayerIndex, string[]>
        {
            {PlayerIndex.Player1, new string[]{ "Canvas/Face0", } },
            {PlayerIndex.Player2, new string[]{ "Canvas/Face1", } },
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
    }
}
