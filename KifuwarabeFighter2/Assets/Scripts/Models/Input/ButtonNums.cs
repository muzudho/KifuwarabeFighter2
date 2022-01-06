namespace Assets.Scripts.Model.Dto.Input
{
    /// <summary>
    /// [プレイヤー番号, ボタン型]の通し番号一覧
    /// </summary>
    public static class ButtonNums
    {
        public static ButtonNum P1Horizontal = new ButtonNum(PlayerNum.N1, ButtonType.Horizontal);
        public static ButtonNum P1Vertical = new ButtonNum(PlayerNum.N1, ButtonType.Vertical);
        public static ButtonNum P1Lp = new ButtonNum(PlayerNum.N1, ButtonType.LightPunch);
        public static ButtonNum P1Mp = new ButtonNum(PlayerNum.N1, ButtonType.MediumPunch);
        public static ButtonNum P1Hp = new ButtonNum(PlayerNum.N1, ButtonType.HardPunch);
        public static ButtonNum P1Lk = new ButtonNum(PlayerNum.N1, ButtonType.LightKick);
        public static ButtonNum P1Mk = new ButtonNum(PlayerNum.N1, ButtonType.MediumKick);
        public static ButtonNum P1Hk = new ButtonNum(PlayerNum.N1, ButtonType.HardKick);
        public static ButtonNum P1Pause = new ButtonNum(PlayerNum.N1, ButtonType.Pause);
        public static ButtonNum P1CancelMenu = new ButtonNum(PlayerNum.N1, ButtonType.CancelMenu); // プレイヤー１のみキャンセル可能。

        public static ButtonNum P2Horizontal = new ButtonNum(PlayerNum.N2, ButtonType.Horizontal);
        public static ButtonNum P2Vertical = new ButtonNum(PlayerNum.N2, ButtonType.Vertical);
        public static ButtonNum P2Lp = new ButtonNum(PlayerNum.N2, ButtonType.LightPunch);
        public static ButtonNum P2Mp = new ButtonNum(PlayerNum.N2, ButtonType.MediumPunch);
        public static ButtonNum P2Hp = new ButtonNum(PlayerNum.N2, ButtonType.HardPunch);
        public static ButtonNum P2Lk = new ButtonNum(PlayerNum.N2, ButtonType.LightKick);
        public static ButtonNum P2Mk = new ButtonNum(PlayerNum.N2, ButtonType.MediumKick);
        public static ButtonNum P2Hk = new ButtonNum(PlayerNum.N2, ButtonType.HardKick);
        public static ButtonNum P2Pause = new ButtonNum(PlayerNum.N2, ButtonType.Pause);
        public static ButtonNum P2CancelMenu = new ButtonNum(PlayerNum.N2, ButtonType.CancelMenu); // プレイヤー２にこのボタンはありません。
    }
}
