namespace Assets.Scripts.Models.Scene.Select
{
    using Assets.Scripts.Models.Input;

    /// <summary>
    /// ゲーム・オブジェクトを識別します
    /// </summary>
    public class GameObjectKey
    {
        #region [プレイヤー番号, ゲームオブジェクト型]で一意のキー
        private string _flat;

        public string Flat
        {
            get { return this._flat; }
        }
        #endregion

        public GameObjectKey(Player player, GameObjectType objectType)
        {
            this.PlayerNum = player;
            this.GameObjectType = objectType;
            this._flat = $"{(int)this.PlayerNum},{(int)this.GameObjectType}";
        }

        public Player PlayerNum { get; set; }
        public GameObjectType GameObjectType { get; set; }

        /// <summary>
        /// 同じ内容あれば同じ値を返すように変更
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.PlayerNum.GetHashCode() + this.GameObjectType.GetHashCode();
        }

        /// <summary>
        /// 同じ内容であればtrueを返すように変更
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var item = obj as GameObjectKey;
            if (item == null)
            {
                return false;
            }

            return this.PlayerNum == item.PlayerNum && this.GameObjectType == item.GameObjectType;
        }
    }
}
