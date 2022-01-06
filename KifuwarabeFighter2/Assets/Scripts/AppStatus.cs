namespace Assets.Scripts
{
    using Assets.Scripts.Models.Input;
    using System.Collections.Generic;

    /// <summary>
    /// グローバル変数のように使います
    /// </summary>
    public static class AppStatus
    {
        static AppStatus()
        {
            Result = KeyOfResult.None; // 対局の結果: なし

            IsComputer = new Dictionary<Player, bool>
            {
                { Player.N1, true }, // 1st playerはコンピューター
                { Player.N2, true }, // 2nd playerはコンピューター
            };

            UseCharacters = new Dictionary<Player, KeyOfCharacter>()
            {
                { Player.N1, KeyOfCharacter.Kifuwarabe }, // 1st playerは、きふわらべ
                { Player.N2, KeyOfCharacter.Kifuwarabe }, // 2nd playerは、きふわらべ
            };

            Teban = Player.N1; // 手番は 1st player
        }

        /// <summary>
        /// 対局の結果
        /// </summary>
        public static KeyOfResult Result { get; set; }

        /// <summary>
        /// 人間か、コンピューターか。
        /// </summary>
        public static Dictionary<Player, bool> IsComputer { get; set; }

        /// <summary>
        /// [Player] プレイヤーの使用キャラクター。
        /// </summary>
        public static Dictionary<Player, KeyOfCharacter> UseCharacters { get; set; }

        /// <summary>
        /// 手番
        /// </summary>
        public static Player Teban { get; set; }
    }
}
