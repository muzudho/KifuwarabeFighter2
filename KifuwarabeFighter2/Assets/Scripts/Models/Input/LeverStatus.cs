namespace Assets.Scripts.Models.Input
{
    /// <summary>
    /// レバーの状況（軸別）
    /// </summary>
    public class LeverStatus
    {
        /// <summary>
        /// レバーを識別するのに使います
        /// </summary>
        public ButtonKey Key { get; private set; }

        /// <summary>
        /// レバーの名前です
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// レバーがどれほど倒れているか。
        /// </summary>
        public float Value { get; set; }

        public LeverStatus(ButtonKey key)
        {
            this.Key = key;
            this.Name = ButtonNames.Dictionary[key];
        }
    }
}
