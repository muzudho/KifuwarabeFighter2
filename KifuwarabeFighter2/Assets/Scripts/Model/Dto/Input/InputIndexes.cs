namespace Assets.Scripts.Model.Dto.Input
{
    public static class InputIndexes
    {
        public static InputIndex P1Horizontal = new InputIndex(PlayerIndex.Player1, ButtonIndex.Horizontal);
        public static InputIndex P1Vertical = new InputIndex(PlayerIndex.Player1, ButtonIndex.Vertical);
        public static InputIndex P1Lp = new InputIndex(PlayerIndex.Player1, ButtonIndex.LightPunch);
        public static InputIndex P1Mp = new InputIndex(PlayerIndex.Player1, ButtonIndex.MediumPunch);
        public static InputIndex P1Hp = new InputIndex(PlayerIndex.Player1, ButtonIndex.HardPunch);
        public static InputIndex P1Lk = new InputIndex(PlayerIndex.Player1, ButtonIndex.LightKick);
        public static InputIndex P1Mk = new InputIndex(PlayerIndex.Player1, ButtonIndex.MediumKick);
        public static InputIndex P1Hk = new InputIndex(PlayerIndex.Player1, ButtonIndex.HardKick);
        public static InputIndex P1Pause = new InputIndex(PlayerIndex.Player1, ButtonIndex.Pause);
        public static InputIndex P1CancelMenu = new InputIndex(PlayerIndex.Player1, ButtonIndex.CancelMenu); // プレイヤー１のみキャンセル可能。

        public static InputIndex P2Horizontal = new InputIndex(PlayerIndex.Player2, ButtonIndex.Horizontal);
        public static InputIndex P2Vertical = new InputIndex(PlayerIndex.Player2, ButtonIndex.Vertical);
        public static InputIndex P2Lp = new InputIndex(PlayerIndex.Player2, ButtonIndex.LightPunch);
        public static InputIndex P2Mp = new InputIndex(PlayerIndex.Player2, ButtonIndex.MediumPunch);
        public static InputIndex P2Hp = new InputIndex(PlayerIndex.Player2, ButtonIndex.HardPunch);
        public static InputIndex P2Lk = new InputIndex(PlayerIndex.Player2, ButtonIndex.LightKick);
        public static InputIndex P2Mk = new InputIndex(PlayerIndex.Player2, ButtonIndex.MediumKick);
        public static InputIndex P2Hk = new InputIndex(PlayerIndex.Player2, ButtonIndex.HardKick);
        public static InputIndex P2Pause = new InputIndex(PlayerIndex.Player2, ButtonIndex.Pause);
        public static InputIndex P2CancelMenu = new InputIndex(PlayerIndex.Player2, ButtonIndex.CancelMenu); // プレイヤー２にこのボタンはありません。
    }
}
