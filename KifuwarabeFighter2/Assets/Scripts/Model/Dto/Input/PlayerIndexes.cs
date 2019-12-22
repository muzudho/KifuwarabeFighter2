namespace Assets.Scripts.Model.Dto.Input
{
    public static class PlayerIndexes
    {
        public static PlayerIndex[] All = new[]
        {
            PlayerIndex.Player1,
            PlayerIndex.Player2,
        };

        /// <summary>
        /// None を詰めて 0 始まりにします。
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static int ToArrayIndex(PlayerIndex player)
        {
            return ((int)player) - 1;
        }

        /// <summary>
        /// None を埋めて 1 始まりにします。
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static PlayerIndex FromArrayIndex(int playerIndex)
        {
            return (PlayerIndex)(playerIndex + 1);
        }
    }
}
