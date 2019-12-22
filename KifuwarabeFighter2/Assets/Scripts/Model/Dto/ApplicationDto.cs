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
            PlayerInputStates = new Dictionary<PlayerIndex, InputStateDto>()
            {
                { PlayerIndex.Player1, new InputStateDto() },
                { PlayerIndex.Player2, new InputStateDto() },
            };
        }

        /// <summary>
        /// キャストするのがめんどくさいので、マップを使うぜ☆（＾～＾）
        /// </summary>
        public static Dictionary<PlayerIndex, InputStateDto> PlayerInputStates { get; set; }

        /// <summary>
        /// シーンの Update メソッドの中で呼び出されます。
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static InputStateDto OnUpdate(PlayerIndex player)
        {
            InputStateDto state = PlayerInputStates[player];

            PlayerInputDao.UpdateState(player, state);

            return state;
        }
    }
}
