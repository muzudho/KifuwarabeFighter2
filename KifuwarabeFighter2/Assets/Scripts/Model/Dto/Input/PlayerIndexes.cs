namespace Assets.Scripts.Model.Dto.Input
{
    public static class PlayerIndexes
    {
        public static PlayerNum[] All = new[]
        {
            PlayerNum.N1,
            PlayerNum.N2,
        };

        /// <summary>
        /// None を詰めて 0 始まりにします。
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static int ToArrayIndex(PlayerNum player)
        {
            return ((int)player) - 1;
        }

        /// <summary>
        /// None を埋めて 1 始まりにします。
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static PlayerNum FromArrayIndex(int playerIndex)
        {
            return (PlayerNum)(playerIndex + 1);
        }
    }
}
