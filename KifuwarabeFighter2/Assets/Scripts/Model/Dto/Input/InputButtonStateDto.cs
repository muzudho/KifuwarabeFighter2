namespace Assets.Scripts.Model.Dto.Input
{
    /// <summary>
    /// ボタン押下状態。
    /// </summary>
    public class InputButtonStateDto
    {
        /// <summary>
        /// ボタンが、押し下がっているか。
        /// </summary>
        public bool Pressing { get; set; }

        /// <summary>
        /// ボタンを、このフレームで押したか。
        /// </summary>
        public bool Down { get; set; }

        /// <summary>
        /// ボタンを、このフレームで離したか。
        /// </summary>
        public bool Up { get; set; }

        public void set(bool pressing, bool down, bool up)
        {
            this.Pressing = pressing;
            this.Down = down;
            this.Up = up;
        }

        /// <summary>
        /// ボタン押下状態の簡易表示。
        /// </summary>
        /// <returns></returns>
        public string toDisplay()
        {
            return $"{(this.Down ? "v" : "")}{(this.Pressing ? "v" : "")}{(this.Up ? "^" : "")}";
        }
    }
}
