namespace Assets.Scripts.Model.Dto.Input
{
    /// <summary>
    /// [プレイヤー，入力ボタン]の一意のインデックス。
    /// </summary>
    public class InputIndex
    {
        #region [プレイヤー, 入力ボタン]で一意のキー
        private string _mapKey;

        public string MapKey
        {
            get { return this._mapKey; }
        }
        #endregion

        public InputIndex(PlayerIndex player, ButtonIndex button)
        {
            this.PlayerIndex = player;
            this.ButtonIndex = button;
            this._mapKey = $"{(int)this.PlayerIndex},{(int)this.ButtonIndex}";
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

            return this.PlayerIndex == item.PlayerIndex && this.ButtonIndex == item.ButtonIndex;
        }
    }
}
