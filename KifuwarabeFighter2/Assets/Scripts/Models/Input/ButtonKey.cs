namespace Assets.Scripts.Models.Input
{
    /// <summary>
    /// [プレイヤー, ボタン型]で一意
    /// </summary>
    public class ButtonKey
    {
        #region [プレイヤー, 入力ボタン]で一意のキー
        private string _flat;

        /// <summary>
        /// [プレイヤー, 入力ボタン]で一意のキー
        /// </summary>
        public string Flat
        {
            get { return this._flat; }
        }
        #endregion

        public ButtonKey(PlayerKey player, ButtonType button)
        {
            this.PlayerNum = player;
            this.ButtonType = button;
            this._flat = $"{(int)this.PlayerNum},{(int)this.ButtonType}";
        }

        public PlayerKey PlayerNum { get; set; }
        public ButtonType ButtonType { get; set; }

        /// <summary>
        /// 同じ内容あれば同じ値を返すように変更
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.PlayerNum.GetHashCode() + this.ButtonType.GetHashCode();
        }

        /// <summary>
        /// 同じ内容であればtrueを返すように変更
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var item = obj as ButtonKey;
            if (item == null)
            {
                return false;
            }

            return this.PlayerNum == item.PlayerNum && this.ButtonType == item.ButtonType;
        }
    }
}
