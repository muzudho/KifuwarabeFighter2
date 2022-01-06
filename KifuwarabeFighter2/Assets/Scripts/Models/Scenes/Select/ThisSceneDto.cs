namespace Assets.Scripts.Model.Dto.Select
{
    using System.Collections.Generic;
    using Assets.Scripts.Model.Dto.Input;

    public static class ThisSceneDto
    {
        static ThisSceneDto()
        {
            ThisSceneDto.PlayerDTOs = new Dictionary<PlayerNum, PlayerDto>()
            {
                {PlayerNum.N1, new PlayerDto(-124.0f)},
                {PlayerNum.N2, new PlayerDto(-224.0f)},
            };

            // セレクト画面でのキャラクターの並び順。
            // 見えるのは３列。残りの１つは見えない。
            ThisSceneDto.Table = new CellDto[]
            {
                new CellDto("きふわらべ", CharacterIndex.Kifuwarabe, -150.0f),
                new CellDto("パナ彦", CharacterIndex.Roborinko, 0.0f),
                new CellDto("ろぼりん娘",CharacterIndex.Ponahiko, 150.0f),
                new CellDto("豆腐マン",CharacterIndex.TofuMan,0.0f),
            };

            TransitionTime = 0;
        }

        public const string TriggerStay = "stay";
        public const string TriggerMove = "move";
        public const string TriggerSelect = "select";
        public const string TriggerTimeOver = "timeover";

        public static int TransitionTime;

        public static Dictionary<PlayerNum, PlayerDto> PlayerDTOs { get; set; }
        public static CellDto[] Table { get; set; }
    }
}
