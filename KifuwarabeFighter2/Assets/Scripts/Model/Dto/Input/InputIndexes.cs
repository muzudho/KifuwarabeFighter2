namespace Assets.Scripts.Model.Dto.Input
{
    public static class InputIndexes
    {
        public static PlayerButtonNum P1Horizontal = new PlayerButtonNum(PlayerNum.N1, ButtonNum.Horizontal);
        public static PlayerButtonNum P1Vertical = new PlayerButtonNum(PlayerNum.N1, ButtonNum.Vertical);
        public static PlayerButtonNum P1Lp = new PlayerButtonNum(PlayerNum.N1, ButtonNum.LightPunch);
        public static PlayerButtonNum P1Mp = new PlayerButtonNum(PlayerNum.N1, ButtonNum.MediumPunch);
        public static PlayerButtonNum P1Hp = new PlayerButtonNum(PlayerNum.N1, ButtonNum.HardPunch);
        public static PlayerButtonNum P1Lk = new PlayerButtonNum(PlayerNum.N1, ButtonNum.LightKick);
        public static PlayerButtonNum P1Mk = new PlayerButtonNum(PlayerNum.N1, ButtonNum.MediumKick);
        public static PlayerButtonNum P1Hk = new PlayerButtonNum(PlayerNum.N1, ButtonNum.HardKick);
        public static PlayerButtonNum P1Pause = new PlayerButtonNum(PlayerNum.N1, ButtonNum.Pause);
        public static PlayerButtonNum P1CancelMenu = new PlayerButtonNum(PlayerNum.N1, ButtonNum.CancelMenu); // プレイヤー１のみキャンセル可能。

        public static PlayerButtonNum P2Horizontal = new PlayerButtonNum(PlayerNum.N2, ButtonNum.Horizontal);
        public static PlayerButtonNum P2Vertical = new PlayerButtonNum(PlayerNum.N2, ButtonNum.Vertical);
        public static PlayerButtonNum P2Lp = new PlayerButtonNum(PlayerNum.N2, ButtonNum.LightPunch);
        public static PlayerButtonNum P2Mp = new PlayerButtonNum(PlayerNum.N2, ButtonNum.MediumPunch);
        public static PlayerButtonNum P2Hp = new PlayerButtonNum(PlayerNum.N2, ButtonNum.HardPunch);
        public static PlayerButtonNum P2Lk = new PlayerButtonNum(PlayerNum.N2, ButtonNum.LightKick);
        public static PlayerButtonNum P2Mk = new PlayerButtonNum(PlayerNum.N2, ButtonNum.MediumKick);
        public static PlayerButtonNum P2Hk = new PlayerButtonNum(PlayerNum.N2, ButtonNum.HardKick);
        public static PlayerButtonNum P2Pause = new PlayerButtonNum(PlayerNum.N2, ButtonNum.Pause);
        public static PlayerButtonNum P2CancelMenu = new PlayerButtonNum(PlayerNum.N2, ButtonNum.CancelMenu); // プレイヤー２にこのボタンはありません。
    }
}
