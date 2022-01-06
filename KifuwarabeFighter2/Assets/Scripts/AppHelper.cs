namespace Assets.Scripts
{
    using Assets.Scripts.Models.Input;
    using UnityEngine;

    /// <summary>
    /// どこからでも使われるぜ☆
    /// </summary>
    public static class AppHelper
    {
        /// <summary>
        /// 手番をひっくり返します
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static Player ReverseTeban(Player player)
        {
            switch (player)
            {
                case Player.N1: return Player.N2;
                case Player.N2: return Player.N1;
                default: Debug.LogError("未定義のプレイヤー☆"); return player;
            }
        }
    }
}