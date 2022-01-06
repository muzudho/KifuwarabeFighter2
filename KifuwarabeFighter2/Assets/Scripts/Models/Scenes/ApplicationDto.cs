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
            PlayerInputStateDTOs = new Dictionary<PlayerNum, GamepadStatus>()
            {
                { PlayerNum.N1, new GamepadStatus(
                    ButtonNums.P1Horizontal,
                    ButtonNums.P1Vertical,
                    ButtonNums.P1Lp,
                    ButtonNums.P1Mp,
                    ButtonNums.P1Hp,
                    ButtonNums.P1Lk,
                    ButtonNums.P1Mk,
                    ButtonNums.P1Hk,
                    ButtonNums.P1Pause,
                    ButtonNums.P1CancelMenu) },
                { PlayerNum.N2, new GamepadStatus(
                    ButtonNums.P2Horizontal,
                    ButtonNums.P2Vertical,
                    ButtonNums.P2Lp,
                    ButtonNums.P2Mp,
                    ButtonNums.P2Hp,
                    ButtonNums.P2Lk,
                    ButtonNums.P2Mk,
                    ButtonNums.P2Hk,
                    ButtonNums.P2Pause,
                    ButtonNums.P2CancelMenu) },
            };
        }

        /// <summary>
        /// キャストするのがめんどくさいので、マップを使うぜ☆（＾～＾）
        /// </summary>
        public static Dictionary<PlayerNum, GamepadStatus> PlayerInputStateDTOs { get; set; }

        /// <summary>
        /// キーの押下状態を読み取ります。
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static GamepadStatus ReadInput(PlayerNum player)
        {
            // DTOを使い回します。
            GamepadStatus state = PlayerInputStateDTOs[player];

            GamepadHelper.UpdateState(player, state);

            return state;
        }
    }
}
