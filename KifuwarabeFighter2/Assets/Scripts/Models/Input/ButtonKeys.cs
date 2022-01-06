namespace Assets.Scripts.Models.Input
{
    /// <summary>
    /// [プレイヤー番号, ボタン型]の通し番号一覧
    /// </summary>
    public static class ButtonKeys
    {
        public static ButtonKey P1Horizontal = new ButtonKey(Player.N1, ButtonType.Horizontal);
        public static ButtonKey P1Vertical = new ButtonKey(Player.N1, ButtonType.Vertical);
        public static ButtonKey P1Lp = new ButtonKey(Player.N1, ButtonType.LightPunch);
        public static ButtonKey P1Mp = new ButtonKey(Player.N1, ButtonType.MediumPunch);
        public static ButtonKey P1Hp = new ButtonKey(Player.N1, ButtonType.HardPunch);
        public static ButtonKey P1Lk = new ButtonKey(Player.N1, ButtonType.LightKick);
        public static ButtonKey P1Mk = new ButtonKey(Player.N1, ButtonType.MediumKick);
        public static ButtonKey P1Hk = new ButtonKey(Player.N1, ButtonType.HardKick);
        public static ButtonKey P1Pause = new ButtonKey(Player.N1, ButtonType.Pause);
        public static ButtonKey P1CancelMenu = new ButtonKey(Player.N1, ButtonType.CancelMenu); // プレイヤー１のみキャンセル可能。

        public static ButtonKey P2Horizontal = new ButtonKey(Player.N2, ButtonType.Horizontal);
        public static ButtonKey P2Vertical = new ButtonKey(Player.N2, ButtonType.Vertical);
        public static ButtonKey P2Lp = new ButtonKey(Player.N2, ButtonType.LightPunch);
        public static ButtonKey P2Mp = new ButtonKey(Player.N2, ButtonType.MediumPunch);
        public static ButtonKey P2Hp = new ButtonKey(Player.N2, ButtonType.HardPunch);
        public static ButtonKey P2Lk = new ButtonKey(Player.N2, ButtonType.LightKick);
        public static ButtonKey P2Mk = new ButtonKey(Player.N2, ButtonType.MediumKick);
        public static ButtonKey P2Hk = new ButtonKey(Player.N2, ButtonType.HardKick);
        public static ButtonKey P2Pause = new ButtonKey(Player.N2, ButtonType.Pause);
        public static ButtonKey P2CancelMenu = new ButtonKey(Player.N2, ButtonType.CancelMenu); // プレイヤー２にこのボタンはありません。
    }
}
