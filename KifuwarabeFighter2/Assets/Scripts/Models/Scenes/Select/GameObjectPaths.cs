namespace Assets.Scripts.Model.Dto.Select
{
    using System.Collections.Generic;

    public class GameObjectPaths
    {
        public static Dictionary<GameObjectKey, string> All = new Dictionary<GameObjectKey, string>()
        {
            {GameObjectKeys.P1Player,  "Canvas/Player0"},
            {GameObjectKeys.P1Name,  "Canvas/Name0"},
            {GameObjectKeys.P1Face,  "Canvas/Face0"},
            {GameObjectKeys.P1BoxBack,  "Canvas/Box0Back"},
            {GameObjectKeys.P1Box,  "Canvas/Box0"},
            {GameObjectKeys.P1Turn,  "Canvas/Turn0"},

            {GameObjectKeys.P2Player,  "Canvas/Player1"},
            {GameObjectKeys.P2Name,  "Canvas/Name1"},
            {GameObjectKeys.P2Face,  "Canvas/Face1"},
            {GameObjectKeys.P2BoxBack,  "Canvas/Box1Back"},
            {GameObjectKeys.P2Box,  "Canvas/Box1"},
            {GameObjectKeys.P2Turn,  "Canvas/Turn1"},
        };
    }
}
