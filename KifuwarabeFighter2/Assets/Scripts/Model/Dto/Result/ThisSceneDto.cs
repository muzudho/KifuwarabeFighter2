namespace Assets.Scripts.Model.Dto.Result
{
    using System.Collections.Generic;
    using Assets.Scripts.Model.Dto.Input;

    public enum GameobjectIndex
    {
        Face,
    }

    public class ThisSceneDto
    {
        public const string GameObjText = "Canvas/Text";

        public static Dictionary<PlayerIndex, string[]> GameObjectPaths = new Dictionary<PlayerIndex, string[]>
        {
            {PlayerIndex.Player1, new string[]{ "Canvas/Face0", } },
            {PlayerIndex.Player2, new string[]{ "Canvas/Face1", } },
        };

        /// <summary>
        /// 勝利台詞
        /// </summary>
        public static string[] WinMessageByCharacter = new string[]
        {
            "公園（くに）へ帰れだぜ☆！\nお前にもお父んがいるだろう☆",
            "努力あるのみ☆\n",
            "あかごの方が　はごたえがあるのじゃ！\n辱め詰めで追い回してやろう！",
            "妥協の産物！\n",
        };
    }
}
