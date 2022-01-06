namespace Assets.Scripts.Models.Input
{
    /// <summary>
    /// ボタン１つ分の状況
    /// </summary>
    public class ButtonStatus
    {
        /// <summary>
        /// ボタンを識別するのに使います
        /// </summary>
        public ButtonKey Key { get; private set; }

        /// <summary>
        /// ボタンの名前です
        /// </summary>
        public string Name { get; private set; }

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

        public ButtonStatus(ButtonKey key)
        {
            this.Key = key;
            this.Name = ButtonNames.Dictionary[key];
        }

        public void Set(bool pressing, bool down, bool up)
        {
            this.Pressing = pressing;
            this.Down = down;
            this.Up = up;
        }

        /// <summary>
        /// ボタン押下状態の簡易表示。
        /// </summary>
        /// <returns></returns>
        public string ToDisplay()
        {
            return $"{(this.Down ? "v" : "")}{(this.Pressing ? "v" : "")}{(this.Up ? "^" : "")}";
        }
    }
}
