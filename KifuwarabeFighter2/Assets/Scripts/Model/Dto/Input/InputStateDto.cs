namespace Assets.Scripts.Model.Dto.Input
{
    /// <summary>
    /// あるプレイヤーのキー入力状態。
    /// </summary>
    public class InputStateDto
    {
        public InputStateDto(
            InputIndex horizontalKey,
            InputIndex verticalKey,
            InputIndex lpKey,
            InputIndex mpKey,
            InputIndex hpKey,
            InputIndex lkKey,
            InputIndex mkKey,
            InputIndex hkKey,
            InputIndex pauseKey,
            InputIndex cancelMenuKey)
        {
            this.HorizontalKey = horizontalKey;
            this.HorizontalName = InputNames.Dictionary[this.HorizontalKey];

            this.VerticalKey = horizontalKey;
            this.VerticalName = InputNames.Dictionary[this.VerticalKey];

            this.LpKey = lpKey;
            this.LpName = InputNames.Dictionary[this.LpKey];

            this.MpKey = mpKey;
            this.MpName = InputNames.Dictionary[this.MpKey];

            this.HpKey = hpKey;
            this.HpName = InputNames.Dictionary[this.HpKey];

            this.LkKey = lkKey;
            this.LkName = InputNames.Dictionary[this.LkKey];

            this.MkKey = mkKey;
            this.MkName = InputNames.Dictionary[this.MkKey];

            this.HkKey = hkKey;
            this.HkName = InputNames.Dictionary[this.HkKey];

            this.PauseKey = pauseKey;
            this.PauseName = InputNames.Dictionary[this.PauseKey];

            this.CancelMenuKey = cancelMenuKey;
            this.CancelMenuName = InputNames.Dictionary[this.CancelMenuKey];

            this.Lp = new InputButtonStateDto();
            this.Mp = new InputButtonStateDto();
            this.Hp = new InputButtonStateDto();
            this.Lk = new InputButtonStateDto();
            this.Mk = new InputButtonStateDto();
            this.Hk = new InputButtonStateDto();
            this.Pause = new InputButtonStateDto();
            this.CancelMenu = new InputButtonStateDto();
        }

        public InputIndex HorizontalKey { get; private set; }
        public string HorizontalName { get; private set; }

        public InputIndex VerticalKey { get; private set; }
        public string VerticalName { get; private set; }

        public InputIndex LpKey { get; private set; }
        public string LpName { get; private set; }

        public InputIndex MpKey { get; private set; }
        public string MpName { get; private set; }

        public InputIndex HpKey { get; private set; }
        public string HpName { get; private set; }

        public InputIndex LkKey { get; private set; }
        public string LkName { get; private set; }

        public InputIndex MkKey { get; private set; }
        public string MkName { get; private set; }

        public InputIndex HkKey { get; private set; }
        public string HkName { get; private set; }

        public InputIndex PauseKey { get; private set; }
        public string PauseName { get; private set; }

        public InputIndex CancelMenuKey { get; private set; }
        public string CancelMenuName { get; private set; }

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

        /// <summary>
        /// 簡易表示☆（＾～＾）
        /// </summary>
        /// <returns></returns>
        public string ToDisplay()
        {
            return $"leverX={LeverX} Y={LeverY} Lp={Lp.ToDisplay()} Mp={Mp.ToDisplay()} Hp={Hp.ToDisplay()} Lk={Lk.ToDisplay()} Mk={Mk.ToDisplay()} Hk={Hk.ToDisplay()} Pause={Pause.ToDisplay()} CancelMenu={CancelMenu.ToDisplay()}";
        }
    }
}
