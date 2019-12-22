namespace Assets.Scripts.Model.Dto.Select
{
    using System.Collections.Generic;

    public class GameObjectPaths
    {
        public static Dictionary<GameObjectIndex, string> All = new Dictionary<GameObjectIndex, string>()
        {
            {GameObjectIndexes.P1Player,  "Canvas/Player0"},
            {GameObjectIndexes.P1Name,  "Canvas/Name0"},
            {GameObjectIndexes.P1Face,  "Canvas/Face0"},
            {GameObjectIndexes.P1BoxBack,  "Canvas/Box0Back"},
            {GameObjectIndexes.P1Box,  "Canvas/Box0"},
            {GameObjectIndexes.P1Turn,  "Canvas/Turn0"},

            {GameObjectIndexes.P2Player,  "Canvas/Player1"},
            {GameObjectIndexes.P2Name,  "Canvas/Name1"},
            {GameObjectIndexes.P2Face,  "Canvas/Face1"},
            {GameObjectIndexes.P2BoxBack,  "Canvas/Box1Back"},
            {GameObjectIndexes.P2Box,  "Canvas/Box1"},
            {GameObjectIndexes.P2Turn,  "Canvas/Turn1"},
        };
    }
}
