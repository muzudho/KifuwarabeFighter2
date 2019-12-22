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
            PlayerInputStateDTOs = new Dictionary<PlayerIndex, InputStateDto>()
            {
                { PlayerIndex.Player1, new InputStateDto() },
                { PlayerIndex.Player2, new InputStateDto() },
            };
        }

        /// <summary>
        /// キャストするのがめんどくさいので、マップを使うぜ☆（＾～＾）
        /// </summary>
        public static Dictionary<PlayerIndex, InputStateDto> PlayerInputStateDTOs { get; set; }

        /// <summary>
        /// キーの押下状態を読み取ります。
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static InputStateDto ReadInput(PlayerIndex player)
        {
            // DTOを使い回します。
            InputStateDto state = PlayerInputStateDTOs[player];

            PlayerInputDao.UpdateState(player, state);

            return state;
        }
    }
}
