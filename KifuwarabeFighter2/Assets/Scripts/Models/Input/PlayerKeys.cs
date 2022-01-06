namespace Assets.Scripts.Model.Dto.Input
{
    /// <summary>
    /// プレイヤー番号の一覧
    /// </summary>
    public static class PlayerKeys
    {
        public static PlayerKey[] All = new[]
        {
            PlayerKey.N1,
            PlayerKey.N2,
        };

        /// <summary>
        /// None を詰めて 0 始まりにします。
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static int ToArrayIndex(PlayerKey player)
        {
            return ((int)player) - 1;
        }

        /// <summary>
        /// None を埋めて 1 始まりにします。
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static PlayerKey FromArrayIndex(int playerIndex)
        {
            return (PlayerKey)(playerIndex + 1);
        }
    }
}
