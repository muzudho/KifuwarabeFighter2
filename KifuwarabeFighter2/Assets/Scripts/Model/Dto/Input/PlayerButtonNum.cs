namespace Assets.Scripts.Model.Dto.Input
{
    /// <summary>
    /// [プレイヤー，入力ボタン]の一意のインデックス。
    /// </summary>
    public class PlayerButtonNum
    {
        #region [プレイヤー, 入力ボタン]で一意のキー
        private string _flatNum;

        /// <summary>
        /// [プレイヤー, 入力ボタン]で一意のキー
        /// </summary>
        public string FlatNum
        {
            get { return this._flatNum; }
        }
        #endregion

        public PlayerButtonNum(PlayerNum player, ButtonNum button)
        {
            this.PlayerNum = player;
            this.ButtonNum = button;
            this._flatNum = $"{(int)this.PlayerNum},{(int)this.ButtonNum}";
        }

        public PlayerNum PlayerNum { get; set; }
        public ButtonNum ButtonNum { get; set; }

        /// <summary>
        /// 同じ内容あれば同じ値を返すように変更
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.PlayerNum.GetHashCode() + this.ButtonNum.GetHashCode();
        }

        /// <summary>
        /// 同じ内容であればtrueを返すように変更
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var item = obj as PlayerButtonNum;
            if (item == null)
            {
                return false;
            }

            return this.PlayerNum == item.PlayerNum && this.ButtonNum == item.ButtonNum;
        }
    }
}
