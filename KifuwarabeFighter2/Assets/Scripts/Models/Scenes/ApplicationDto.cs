namespace Assets.Scripts.Model.Dto
{
    using System.Collections.Generic;
    using Assets.Scripts.Model.Dao.Input;
    using Assets.Scripts.Model.Dto.Input;

    /// <summary>
    /// 共有する入力関連はこちらに。
    /// </summary>
    public abstract class ApplicationDto
    {
        static ApplicationDto()
        {
            PlayerInputStateDTOs = new Dictionary<PlayerKey, GamepadStatus>()
            {
                { PlayerKey.N1, new GamepadStatus(
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
                { PlayerKey.N2, new GamepadStatus(
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
        public static Dictionary<PlayerKey, GamepadStatus> PlayerInputStateDTOs { get; set; }

        /// <summary>
        /// キーの押下状態を読み取ります。
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static GamepadStatus ReadInput(PlayerKey player)
        {
            // DTOを使い回します。
            GamepadStatus state = PlayerInputStateDTOs[player];

            GamepadHelper.UpdateState(player, state);

            return state;
        }
    }
}
