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
            PlayerInputStateDTOs = new Dictionary<PlayerNum, InputStateDto>()
            {
                { PlayerNum.N1, new InputStateDto(
                    InputIndexes.P1Horizontal,
                    InputIndexes.P1Vertical,
                    InputIndexes.P1Lp,
                    InputIndexes.P1Mp,
                    InputIndexes.P1Hp,
                    InputIndexes.P1Lk,
                    InputIndexes.P1Mk,
                    InputIndexes.P1Hk,
                    InputIndexes.P1Pause,
                    InputIndexes.P1CancelMenu) },
                { PlayerNum.N2, new InputStateDto(
                    InputIndexes.P2Horizontal,
                    InputIndexes.P2Vertical,
                    InputIndexes.P2Lp,
                    InputIndexes.P2Mp,
                    InputIndexes.P2Hp,
                    InputIndexes.P2Lk,
                    InputIndexes.P2Mk,
                    InputIndexes.P2Hk,
                    InputIndexes.P2Pause,
                    InputIndexes.P2CancelMenu) },
            };
        }

        /// <summary>
        /// キャストするのがめんどくさいので、マップを使うぜ☆（＾～＾）
        /// </summary>
        public static Dictionary<PlayerNum, InputStateDto> PlayerInputStateDTOs { get; set; }

        /// <summary>
        /// キーの押下状態を読み取ります。
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static InputStateDto ReadInput(PlayerNum player)
        {
            // DTOを使い回します。
            InputStateDto state = PlayerInputStateDTOs[player];

            PlayerInputDao.UpdateState(player, state);

            return state;
        }
    }
}
