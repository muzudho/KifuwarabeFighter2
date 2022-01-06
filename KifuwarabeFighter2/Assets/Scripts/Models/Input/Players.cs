namespace Assets.Scripts.Models.Input
{
    /// <summary>
    /// プレイヤー番号の一覧
    /// </summary>
    public static class Players
    {
        public static Player[] All = new[]
        {
            Player.N1,
            Player.N2,
        };

        /// <summary>
        /// None を詰めて 0 始まりにします。
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static int ToArrayIndex(Player player)
        {
            return ((int)player) - 1;
        }

        /// <summary>
        /// None を埋めて 1 始まりにします。
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static Player FromArrayIndex(int playerIndex)
        {
            return (Player)(playerIndex + 1);
        }
    }
}
