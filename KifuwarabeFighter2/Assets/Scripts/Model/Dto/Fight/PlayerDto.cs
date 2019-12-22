namespace Assets.Scripts.Model.Dto.Fight
{
    using global::SceneMain;
    using UnityEngine;
    using UnityEngine.UI;

    public class PlayerDto
    {
        public PlayerDto(
            RectTransform barTransform,
            Text playerName,
            Text value,
            GameObject playerChar,
            GameObject[] roundsResult,
            Text turn,
            Outline turnOutline,
            Text time,
            Outline timeOutline)
        {
            this.BarTransform = barTransform;
            this.PlayerName = playerName;
            this.Value = value;
            this.PlayerChar = playerChar;
            this.PlayerCharScript = this.PlayerChar.GetComponent<PlayerScript>();
            this.PlayerCharAnimetor = this.PlayerChar.GetComponent<Animator>();

            this.RoundsResult = roundsResult;
            for (int iRound = 0; iRound < 2; iRound++)
            {
                this.RoundsResult[iRound].SetActive(false);
            }

            this.Turn = turn;
            this.TurnOutline = turnOutline;
            this.Time = time;
            this.TimeOutline = timeOutline;
            // this.TimeCount = 0;
            // this.WinCount = 0;
        }

        /// <summary>
        /// life bar. 評価値メーター
        /// </summary>
        public RectTransform BarTransform { get; set; }

        /// <summary>
        /// Player name on bar. プレイヤー名。
        /// </summary>
        public Text PlayerName { get; set; }

        /// <summary>
        /// life point. 評価値。
        /// </summary>
        public Text Value { get; set; }

        /// <summary>
        /// player character. プレイヤー・キャラ。
        /// </summary>
        public GameObject PlayerChar { get; set; }

        /// <summary>
        /// player character's attached script. プレイヤー・キャラにアタッチされているスクリプト。
        /// </summary>
        public PlayerScript PlayerCharScript { get; private set; }

        /// <summary>
        /// players animator. アニメーター。
        /// </summary>
        public Animator PlayerCharAnimetor { get; set; }

        /// <summary>
        /// win,lose mark. 勝ち星
        /// </summary>
        public GameObject[] RoundsResult { get; set; }

        /// <summary>
        /// turn. ターン。
        /// </summary>
        public Text Turn { get; set; }

        public Outline TurnOutline { get; set; }

        public Text Time { get; set; }

        /// <summary>
        /// timer. 残りタイマー。
        /// </summary>
        public Outline TimeOutline { get; set; }

        // 制限時間
        public float TimeCount { get; set; }

        /// <summary>
        /// Win count. 勝ち数。２本先取か数える。
        /// </summary>
        public int WinCount { get; set; }
    }
}
