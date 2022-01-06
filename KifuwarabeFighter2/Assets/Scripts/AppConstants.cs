namespace Assets.Scripts
{
    /// <summary>
    /// 変わらない値
    /// </summary>
    public static class AppConstants
    {
        /// <summary>
        /// シーンの名前
        /// </summary>
        public readonly static string[] sceneToName = new[] { "Title", "Select", "Fight", "Result" };

        /// <summary>
        /// セレクト画面と、リザルト画面で使う、顔☆
        /// </summary>
        public readonly static string[,] characterAndSliceToFaceSprites = new string[,]{
            { "Sprites/Face0", "Face0_0", "Face0_1" },
            { "Sprites/Face1", "Face1_0", "Face1_1" },
            { "Sprites/Face2", "Face2_0", "Face2_1" },
            { "Sprites/Face3", "Face3_0", "Face3_1" },
        };
    }
}
