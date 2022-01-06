namespace Assets.Scripts.Models.Scenes.Result
{
    using System.Collections.Generic;
    using Assets.Scripts.Models.Input;

    public enum GameobjectType
    {
        Face,
    }

    public class ThisSceneStatus
    {
        public const string GameObjText = "Canvas/Text";

        public static Dictionary<Player, string[]> GameObjectPaths = new Dictionary<Player, string[]>
        {
            {Player.N1, new string[]{ "Canvas/Face0", } },
            {Player.N2, new string[]{ "Canvas/Face1", } },
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
