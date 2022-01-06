namespace Assets.Scripts.Models.Input
{
    /// <summary>
    /// ゲームパッド１人分の入力状態
    /// </summary>
    public class GamepadStatus
    {
        public GamepadStatus(
            ButtonKey horizontalKey,
            ButtonKey verticalKey,
            ButtonKey lpKey,
            ButtonKey mpKey,
            ButtonKey hpKey,
            ButtonKey lkKey,
            ButtonKey mkKey,
            ButtonKey hkKey,
            ButtonKey pauseKey,
            ButtonKey cancelMenuKey)
        {
            this.HorizontalKey = horizontalKey;
            this.HorizontalName = ButtonNames.Dictionary[this.HorizontalKey];

            this.VerticalKey = horizontalKey;
            this.VerticalName = ButtonNames.Dictionary[this.VerticalKey];

            this.Lp = new ButtonStatus(lpKey);
            this.Mp = new ButtonStatus(mpKey);
            this.Hp = new ButtonStatus(hpKey);
            this.Lk = new ButtonStatus(lkKey);
            this.Mk = new ButtonStatus(mkKey);
            this.Hk = new ButtonStatus(hkKey);
            this.Pause = new ButtonStatus(pauseKey);
            this.CancelMenu = new ButtonStatus(cancelMenuKey);
        }

        public ButtonKey HorizontalKey { get; private set; }
        public string HorizontalName { get; private set; }

        public ButtonKey VerticalKey { get; private set; }
        public string VerticalName { get; private set; }

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
