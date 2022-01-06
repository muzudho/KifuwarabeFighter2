namespace Assets.Scripts.Model.Dto.Input
{
    /// <summary>
    /// ゲームパッド１人分の入力状態
    /// </summary>
    public class GamepadStatus
    {
        public GamepadStatus(
            ButtonNum horizontalKey,
            ButtonNum verticalKey,
            ButtonNum lpKey,
            ButtonNum mpKey,
            ButtonNum hpKey,
            ButtonNum lkKey,
            ButtonNum mkKey,
            ButtonNum hkKey,
            ButtonNum pauseKey,
            ButtonNum cancelMenuKey)
        {
            this.HorizontalKey = horizontalKey;
            this.HorizontalName = ButtonNames.Dictionary[this.HorizontalKey];

            this.VerticalKey = horizontalKey;
            this.VerticalName = ButtonNames.Dictionary[this.VerticalKey];

            this.LpKey = lpKey;
            this.LpName = ButtonNames.Dictionary[this.LpKey];

            this.MpKey = mpKey;
            this.MpName = ButtonNames.Dictionary[this.MpKey];

            this.HpKey = hpKey;
            this.HpName = ButtonNames.Dictionary[this.HpKey];

            this.LkKey = lkKey;
            this.LkName = ButtonNames.Dictionary[this.LkKey];

            this.MkKey = mkKey;
            this.MkName = ButtonNames.Dictionary[this.MkKey];

            this.HkKey = hkKey;
            this.HkName = ButtonNames.Dictionary[this.HkKey];

            this.PauseKey = pauseKey;
            this.PauseName = ButtonNames.Dictionary[this.PauseKey];

            this.CancelMenuKey = cancelMenuKey;
            this.CancelMenuName = ButtonNames.Dictionary[this.CancelMenuKey];

            this.Lp = new ButtonStatus();
            this.Mp = new ButtonStatus();
            this.Hp = new ButtonStatus();
            this.Lk = new ButtonStatus();
            this.Mk = new ButtonStatus();
            this.Hk = new ButtonStatus();
            this.Pause = new ButtonStatus();
            this.CancelMenu = new ButtonStatus();
        }

        public ButtonNum HorizontalKey { get; private set; }
        public string HorizontalName { get; private set; }

        public ButtonNum VerticalKey { get; private set; }
        public string VerticalName { get; private set; }

        public ButtonNum LpKey { get; private set; }
        public string LpName { get; private set; }

        public ButtonNum MpKey { get; private set; }
        public string MpName { get; private set; }

        public ButtonNum HpKey { get; private set; }
        public string HpName { get; private set; }

        public ButtonNum LkKey { get; private set; }
        public string LkName { get; private set; }

        public ButtonNum MkKey { get; private set; }
        public string MkName { get; private set; }

        public ButtonNum HkKey { get; private set; }
        public string HkName { get; private set; }

        public ButtonNum PauseKey { get; private set; }
        public string PauseName { get; private set; }

        public ButtonNum CancelMenuKey { get; private set; }
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
        public ButtonStatus Lp { get; set; }

        /// <summary>
        /// 中パンチボタン押下状態。
        /// </summary>
        public ButtonStatus Mp { get; set; }

        /// <summary>
        /// 強パンチボタン押下状態。
        /// </summary>
        public ButtonStatus Hp { get; set; }

        /// <summary>
        /// 弱キックボタン押下状態。
        /// </summary>
        public ButtonStatus Lk { get; set; }

        /// <summary>
        /// 中キックボタン押下状態。
        /// </summary>
        public ButtonStatus Mk { get; set; }

        /// <summary>
        /// 強キックボタン押下状態。
        /// </summary>
        public ButtonStatus Hk { get; set; }

        /// <summary>
        /// ポーズ・ボタン押下状態。
        /// </summary>
        public ButtonStatus Pause { get; set; }

        /// <summary>
        /// キャンセル・ボタン押下状態。（１プレイヤーだけが使うことを想定したボタン）
        /// </summary>
        public ButtonStatus CancelMenu { get; set; }

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
