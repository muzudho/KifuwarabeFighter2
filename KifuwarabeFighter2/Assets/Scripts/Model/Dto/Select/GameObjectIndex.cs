namespace Assets.Scripts.Model.Dto.Select
{
    using Assets.Scripts.Model.Dto.Input;

    /// <summary>
    /// ゲーム・オブジェクトのインデックス。
    /// </summary>
    public class GameObjectIndex
    {
        private string _mapKey;

        public GameObjectIndex(PlayerIndex player, GameObjectTypeIndex objectType)
        {
            this.PlayerIndex = player;
            this.GameObjectTypeIndex = objectType;
            this._mapKey = $"{(int)this.PlayerIndex},{(int)this.GameObjectTypeIndex}";
        }

        public string MapKey
        {
            get { return this._mapKey; }
        }

        public PlayerIndex PlayerIndex { get; set; }
        public GameObjectTypeIndex GameObjectTypeIndex { get; set; }

        /// <summary>
        /// 同じ内容あれば同じ値を返すように変更
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.PlayerIndex.GetHashCode() + this.GameObjectTypeIndex.GetHashCode();
        }

        /// <summary>
        /// 同じ内容であればtrueを返すように変更
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var item = obj as GameObjectIndex;
            if (item == null)
            {
                return false;
            }

            return this.PlayerIndex == item.PlayerIndex && this.GameObjectTypeIndex == item.GameObjectTypeIndex;
        }
    }
}
