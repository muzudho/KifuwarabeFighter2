namespace Assets.Scripts.Models.Scene.Select
{
    using System.Collections.Generic;
    using Assets.Scripts.Models.Input;

    /// <summary>
    /// セレクトシーンの状況
    /// </summary>
    public static class ThisSceneStatus
    {
        static ThisSceneStatus()
        {
            ThisSceneStatus.PlayerStatusDict = new Dictionary<PlayerKey, PlayerStatus>()
            {
                {PlayerKey.N1, new PlayerStatus(-124.0f)},
                {PlayerKey.N2, new PlayerStatus(-224.0f)},
            };

            // セレクト画面でのキャラクターの並び順。
            // 見えるのは３列。残りの１つは見えない。
            ThisSceneStatus.Table = new CharacterCell[]
            {
                new CharacterCell("きふわらべ", CharacterIndex.Kifuwarabe, -150.0f),
                new CharacterCell("パナ彦", CharacterIndex.Roborinko, 0.0f),
                new CharacterCell("ろぼりん娘",CharacterIndex.Ponahiko, 150.0f),
                new CharacterCell("豆腐マン",CharacterIndex.TofuMan,0.0f),
            };

            TransitionTime = 0;
        }

        public const string TriggerStay = "stay";
        public const string TriggerMove = "move";
        public const string TriggerSelect = "select";
        public const string TriggerTimeOver = "timeover";

        public static int TransitionTime;

        public static Dictionary<PlayerKey, PlayerStatus> PlayerStatusDict { get; set; }
        public static CharacterCell[] Table { get; set; }
    }
}
