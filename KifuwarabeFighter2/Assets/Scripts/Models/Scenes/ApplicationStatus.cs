namespace Assets.Scripts.Models
{
    using System.Collections.Generic;
    using Assets.Scripts.Models.Input;

    /// <summary>
    /// 共有する入力関連はこちらに。
    /// </summary>
    public abstract class ApplicationStatus
    {
        static ApplicationStatus()
        {
            PlayerInputStateDict = new Dictionary<Player, GamepadStatus>()
            {
                { Player.N1, new GamepadStatus(
                    ButtonKeys.P1Horizontal,
                    ButtonKeys.P1Vertical,
                    ButtonKeys.P1Lp,
                    ButtonKeys.P1Mp,
                    ButtonKeys.P1Hp,
                    ButtonKeys.P1Lk,
                    ButtonKeys.P1Mk,
                    ButtonKeys.P1Hk,
                    ButtonKeys.P1Pause,
                    ButtonKeys.P1CancelMenu) },
                { Player.N2, new GamepadStatus(
                    ButtonKeys.P2Horizontal,
                    ButtonKeys.P2Vertical,
                    ButtonKeys.P2Lp,
                    ButtonKeys.P2Mp,
                    ButtonKeys.P2Hp,
                    ButtonKeys.P2Lk,
                    ButtonKeys.P2Mk,
                    ButtonKeys.P2Hk,
                    ButtonKeys.P2Pause,
                    ButtonKeys.P2CancelMenu) },
            };
        }

        /// <summary>
        /// キャストするのがめんどくさいので、マップを使うぜ☆（＾～＾）
        /// </summary>
        public static Dictionary<Player, GamepadStatus> PlayerInputStateDict { get; set; }

        /// <summary>
        /// キーの押下状態を読み取ります。
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static GamepadStatus ReadInput(Player player)
        {
            // DTOを使い回します。
            GamepadStatus gamepad = PlayerInputStateDict[player];

            GamepadHelper.UpdateState(player, gamepad);

            return gamepad;
        }
    }
}
