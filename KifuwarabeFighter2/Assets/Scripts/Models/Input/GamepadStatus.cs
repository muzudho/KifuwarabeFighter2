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
            this.HorizontalLever = new LeverStatus(horizontalKey);
            this.VerticalLever = new LeverStatus(verticalKey);
            this.Lp = new ButtonStatus(lpKey);
            this.Mp = new ButtonStatus(mpKey);
            this.Hp = new ButtonStatus(hpKey);
            this.Lk = new ButtonStatus(lkKey);
            this.Mk = new ButtonStatus(mkKey);
            this.Hk = new ButtonStatus(hkKey);
            this.Pause = new ButtonStatus(pauseKey);
            this.CancelMenu = new ButtonStatus(cancelMenuKey);
        }

        /// <summary>
        /// レバー（水平方向）
        /// </summary>
        public LeverStatus HorizontalLever { get; set; }

        /// <summary>
        /// レバー（垂直方向）
        /// </summary>
        public LeverStatus VerticalLever { get; set; }

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
            return $"leverX={HorizontalLever.Value} Y={VerticalLever.Value} Lp={Lp.ToDisplay()} Mp={Mp.ToDisplay()} Hp={Hp.ToDisplay()} Lk={Lk.ToDisplay()} Mk={Mk.ToDisplay()} Hk={Hk.ToDisplay()} Pause={Pause.ToDisplay()} CancelMenu={CancelMenu.ToDisplay()}";
        }
    }
}
