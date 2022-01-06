namespace Assets.Scripts.Model.Dto.Select
{
    using Assets.Scripts.Model.Dto.Input;

    public static class GameObjectKeys
    {
        public static GameObjectKey P1Player = new GameObjectKey(PlayerKey.N1, GameObjectType.Player);
        public static GameObjectKey P1Name = new GameObjectKey(PlayerKey.N1, GameObjectType.Name);
        public static GameObjectKey P1Face = new GameObjectKey(PlayerKey.N1, GameObjectType.Face);
        public static GameObjectKey P1BoxBack = new GameObjectKey(PlayerKey.N1, GameObjectType.BoxBack);
        public static GameObjectKey P1Box = new GameObjectKey(PlayerKey.N1, GameObjectType.Box);
        public static GameObjectKey P1Turn = new GameObjectKey(PlayerKey.N1, GameObjectType.Turn);

        public static GameObjectKey P2Player = new GameObjectKey(PlayerKey.N2, GameObjectType.Player);
        public static GameObjectKey P2Name = new GameObjectKey(PlayerKey.N2, GameObjectType.Name);
        public static GameObjectKey P2Face = new GameObjectKey(PlayerKey.N2, GameObjectType.Face);
        public static GameObjectKey P2BoxBack = new GameObjectKey(PlayerKey.N2, GameObjectType.BoxBack);
        public static GameObjectKey P2Box = new GameObjectKey(PlayerKey.N2, GameObjectType.Box);
        public static GameObjectKey P2Turn = new GameObjectKey(PlayerKey.N2, GameObjectType.Turn);
    }
}
