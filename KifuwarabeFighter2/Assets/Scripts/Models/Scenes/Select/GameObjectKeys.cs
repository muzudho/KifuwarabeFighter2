namespace Assets.Scripts.Models.Scene.Select
{
    using Assets.Scripts.Models.Input;

    public static class GameObjectKeys
    {
        public static GameObjectKey P1Player = new GameObjectKey(Player.N1, GameObjectType.Player);
        public static GameObjectKey P1Name = new GameObjectKey(Player.N1, GameObjectType.Name);
        public static GameObjectKey P1Face = new GameObjectKey(Player.N1, GameObjectType.Face);
        public static GameObjectKey P1BoxBack = new GameObjectKey(Player.N1, GameObjectType.BoxBack);
        public static GameObjectKey P1Box = new GameObjectKey(Player.N1, GameObjectType.Box);
        public static GameObjectKey P1Turn = new GameObjectKey(Player.N1, GameObjectType.Turn);

        public static GameObjectKey P2Player = new GameObjectKey(Player.N2, GameObjectType.Player);
        public static GameObjectKey P2Name = new GameObjectKey(Player.N2, GameObjectType.Name);
        public static GameObjectKey P2Face = new GameObjectKey(Player.N2, GameObjectType.Face);
        public static GameObjectKey P2BoxBack = new GameObjectKey(Player.N2, GameObjectType.BoxBack);
        public static GameObjectKey P2Box = new GameObjectKey(Player.N2, GameObjectType.Box);
        public static GameObjectKey P2Turn = new GameObjectKey(Player.N2, GameObjectType.Turn);
    }
}
