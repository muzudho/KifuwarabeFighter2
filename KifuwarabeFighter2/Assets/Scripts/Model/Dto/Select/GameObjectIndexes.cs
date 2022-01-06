namespace Assets.Scripts.Model.Dto.Select
{
    using Assets.Scripts.Model.Dto.Input;

    public static class GameObjectIndexes
    {
        public static GameObjectIndex P1Player = new GameObjectIndex(PlayerNum.N1, GameObjectTypeIndex.Player);
        public static GameObjectIndex P1Name = new GameObjectIndex(PlayerNum.N1, GameObjectTypeIndex.Name);
        public static GameObjectIndex P1Face = new GameObjectIndex(PlayerNum.N1, GameObjectTypeIndex.Face);
        public static GameObjectIndex P1BoxBack = new GameObjectIndex(PlayerNum.N1, GameObjectTypeIndex.BoxBack);
        public static GameObjectIndex P1Box = new GameObjectIndex(PlayerNum.N1, GameObjectTypeIndex.Box);
        public static GameObjectIndex P1Turn = new GameObjectIndex(PlayerNum.N1, GameObjectTypeIndex.Turn);

        public static GameObjectIndex P2Player = new GameObjectIndex(PlayerNum.N2, GameObjectTypeIndex.Player);
        public static GameObjectIndex P2Name = new GameObjectIndex(PlayerNum.N2, GameObjectTypeIndex.Name);
        public static GameObjectIndex P2Face = new GameObjectIndex(PlayerNum.N2, GameObjectTypeIndex.Face);
        public static GameObjectIndex P2BoxBack = new GameObjectIndex(PlayerNum.N2, GameObjectTypeIndex.BoxBack);
        public static GameObjectIndex P2Box = new GameObjectIndex(PlayerNum.N2, GameObjectTypeIndex.Box);
        public static GameObjectIndex P2Turn = new GameObjectIndex(PlayerNum.N2, GameObjectTypeIndex.Turn);
    }
}
