namespace Assets.Scripts.Model.Dto.Input
{
    /// <summary>
    /// 入力キーのインデックス。
    /// </summary>
    public class InputIndex
    {
        private string _mapKey;

        public InputIndex(PlayerIndex playerIndex, ButtonIndex buttonIndex)
        {
            this.PlayerIndex = (PlayerIndex)playerIndex;
            this.ButtonIndex = (ButtonIndex)buttonIndex;
            this._mapKey = $"{(int)this.PlayerIndex},{(int)this.ButtonIndex}";
        }

        public string MapKey
        {
            get { return this._mapKey; }
        }

        public PlayerIndex PlayerIndex { get; set; }
        public ButtonIndex ButtonIndex { get; set; }

        /// <summary>
        /// 同じ内容あれば同じ値を返すように変更
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.PlayerIndex.GetHashCode() + this.ButtonIndex.GetHashCode();
        }

        /// <summary>
        /// 同じ内容であればtrueを返すように変更
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var item = obj as InputIndex;
            if (item == null)
            {
                return false;
            }

            return this.PlayerIndex == item.PlayerIndex && item.ButtonIndex == item.ButtonIndex;
        }
    }
}
