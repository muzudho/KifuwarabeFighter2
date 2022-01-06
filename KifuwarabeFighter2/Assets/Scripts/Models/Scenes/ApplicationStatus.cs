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
                    Buttons.P1Horizontal,
                    Buttons.P1Vertical,
                    Buttons.P1Lp,
                    Buttons.P1Mp,
                    Buttons.P1Hp,
                    Buttons.P1Lk,
                    Buttons.P1Mk,
                    Buttons.P1Hk,
                    Buttons.P1Pause,
                    Buttons.P1CancelMenu) },
                { Player.N2, new GamepadStatus(
                    Buttons.P2Horizontal,
                    Buttons.P2Vertical,
                    Buttons.P2Lp,
                    Buttons.P2Mp,
                    Buttons.P2Hp,
                    Buttons.P2Lk,
                    Buttons.P2Mk,
                    Buttons.P2Hk,
                    Buttons.P2Pause,
                    Buttons.P2CancelMenu) },
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
