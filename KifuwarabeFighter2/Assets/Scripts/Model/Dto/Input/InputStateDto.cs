namespace Assets.Scripts.Model.Dto.Input
{
    /// <summary>
    /// あるプレイヤーのキー入力状態。
    /// </summary>
    public class InputStateDto
    {
        public InputStateDto()
        {
            this.Lp = new InputButtonStateDto();
            this.Mp = new InputButtonStateDto();
            this.Hp = new InputButtonStateDto();
            this.Lk = new InputButtonStateDto();
            this.Mk = new InputButtonStateDto();
            this.Hk = new InputButtonStateDto();
            this.Pause = new InputButtonStateDto();
            this.CancelMenu = new InputButtonStateDto();
        }

        /// <summary>
        /// レバーが水平方向に倒れているか。
        /// </summary>
        public float LeverX { get; set; }

        /// <summary>
        /// レバーが垂直方向に倒れているか。
        /// </summary>
        public float LeverY { get; set; }

        /// <summary>
        /// 弱パンチボタン押下状態。
        /// </summary>
        public InputButtonStateDto Lp { get; set; }

        /// <summary>
        /// 中パンチボタン押下状態。
        /// </summary>
        public InputButtonStateDto Mp { get; set; }

        /// <summary>
        /// 強パンチボタン押下状態。
        /// </summary>
        public InputButtonStateDto Hp { get; set; }

        /// <summary>
        /// 弱キックボタン押下状態。
        /// </summary>
        public InputButtonStateDto Lk { get; set; }

        /// <summary>
        /// 中キックボタン押下状態。
        /// </summary>
        public InputButtonStateDto Mk { get; set; }

        /// <summary>
        /// 強キックボタン押下状態。
        /// </summary>
        public InputButtonStateDto Hk { get; set; }

        /// <summary>
        /// ポーズ・ボタン押下状態。
        /// </summary>
        public InputButtonStateDto Pause { get; set; }

        /// <summary>
        /// キャンセル・ボタン押下状態。（１プレイヤーだけが使うことを想定したボタン）
        /// </summary>
        public InputButtonStateDto CancelMenu { get; set; }
    }
}
