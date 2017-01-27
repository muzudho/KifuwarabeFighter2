using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SceneMain
{
    /// <summary>
    /// メインシーンのメインカメラのスクリプトだぜ☆
    /// </summary>
    public class Main_CameraScript : MonoBehaviour
    {
        #region UI 表示物
        /// <summary>
        /// Player name on bar. プレイヤー名。
        /// </summary>
        Text[] player_to_name;
        /// <summary>
        /// Ready message 0, 1. 対局開始テキスト。
        /// </summary>
        GameObject fight0;
        GameObject fight1;
        /// <summary>
        /// KO message. 参りましたテキスト。
        /// </summary>
        GameObject resign0;
        /// <summary>
        /// life bar. 評価値メーター
        /// </summary>
        RectTransform[] player_to_barTransform;
        /// <summary>
        /// life point. 評価値。
        /// </summary>
        Text[] player_to_value;
        #endregion

        /// <summary>
        /// Ready message presentation time counter; 対局開始メッセージが表示されている時間カウンター。
        /// </summary>
        int readyingTime; public int ReadyingTime { get { return readyingTime; } set { readyingTime = value; } }

        #region 選択キャラクター
        /// <summary>
        /// player character. プレイヤー・キャラ。
        /// </summary>
        GameObject[] player_to_playerChar;
        /// <summary>
        /// player character's attached script. プレイヤー・キャラにアタッチされているスクリプト。
        /// </summary>
        Main_PlayerScript[] player_to_charScript;
        /// <summary>
        /// players animator. アニメーター。
        /// </summary>
        Animator[] player_to_anime;
        #endregion
        /// <summary>
        /// win,lose mark. 勝ち星
        /// </summary>
        GameObject[,] playerAndRound_to_result;
        #region 制限時間
        /// <summary>
        /// turn. ターン。
        /// </summary>
        Text[] player_to_turn;
        Outline[] player_to_turnOutline;
        /// <summary>
        /// timer. 残りタイマー。
        /// </summary>
        Text[] player_to_time;
        Outline[] player_to_timeOutline;
        float[] player_to_timeCount;
        #endregion
        #region 攻撃力
        /// <summary>
        /// point for calculate damage. ダメージ計算のための数字。
        /// </summary>
        private float[] player_to_attackPower; public float[] Player_to_attackPower { get { return player_to_attackPower; } }
        #endregion
        #region ラウンド
        /// <summary>
        /// Win count. 勝ち数。２本先取か数える。
        /// </summary>
        int[] player_to_winCount;
        /// <summary>
        /// flag of end. 終了フラグ
        /// </summary>
        bool isRoundFinished;
        /// <summary>
        /// flag of start of KO Message. 参りましたメッセージを開始したときにＯＮにするフラグ。
        /// </summary>
        public bool IsResignCalled { get; set; }
        #endregion

        void Start()
        {
            #region UI 表示物
            fight0 = GameObject.Find(SceneCommon.GAMEOBJ_FIGHT0);
            fight0.GetComponent<Text>().text = SceneCommon.Character_to_fightMessage[(int)CommonScript.Player_to_useCharacter[(int)PlayerIndex.Player1]];
            fight1 = GameObject.Find(SceneCommon.GAMEOBJ_FIGHT1);
            resign0 = GameObject.Find(SceneCommon.GAMEOBJ_RESIGN0);
            resign0.SetActive(false);
            player_to_name = new[] { GameObject.Find(SceneCommon.PlayerAndGameobject_to_path[(int)PlayerIndex.Player1,(int)GameobjectIndex.Name]).GetComponent<Text>(), GameObject.Find(SceneCommon.PlayerAndGameobject_to_path[(int)PlayerIndex.Player2, (int)GameobjectIndex.Name]).GetComponent<Text>() };
            player_to_value = new[] { GameObject.Find(SceneCommon.PlayerAndGameobject_to_path[(int)PlayerIndex.Player1, (int)GameobjectIndex.Value]).GetComponent<Text>(), GameObject.Find(SceneCommon.PlayerAndGameobject_to_path[(int)PlayerIndex.Player2, (int)GameobjectIndex.Value]).GetComponent<Text>() };
            #endregion
            #region Internal variable 内部変数
            player_to_winCount = new[] { 0, 0 };
            #endregion
            #region 選択キャラクター
            player_to_playerChar = new[] { GameObject.Find(SceneCommon.PlayerAndGameobject_to_path[(int)PlayerIndex.Player1, (int)GameobjectIndex.Player]), GameObject.Find(SceneCommon.PlayerAndGameobject_to_path[(int)PlayerIndex.Player2, (int)GameobjectIndex.Player]) };
            player_to_charScript = new[] { player_to_playerChar[(int)PlayerIndex.Player1].GetComponent<Main_PlayerScript>(), player_to_playerChar[(int)PlayerIndex.Player2].GetComponent<Main_PlayerScript>() };
            player_to_anime = new [] { player_to_playerChar[(int)PlayerIndex.Player1].GetComponent<Animator>(), player_to_playerChar[(int)PlayerIndex.Player2].GetComponent<Animator>() };
            for (int iPlayer = (int)PlayerIndex.Player1; iPlayer < (int)PlayerIndex.Num; iPlayer++)
            {
                CharacterIndex character = CommonScript.Player_to_useCharacter[iPlayer];
                // 名前
                player_to_name[iPlayer].text = SceneCommon.Character_to_nameRoma[(int)character];

                // アニメーター
                player_to_anime[iPlayer].runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load(SceneCommon.Character_to_animationController[(int)character]));
            }
            #endregion

            //bar1のRectTransformコンポーネントをキャッシュ
            player_to_barTransform = new[] { GameObject.Find(SceneCommon.PlayerAndGameobject_to_path[(int)PlayerIndex.Player1, (int)GameobjectIndex.Bar]).GetComponent<RectTransform>(), GameObject.Find(SceneCommon.PlayerAndGameobject_to_path[(int)PlayerIndex.Player2, (int)GameobjectIndex.Bar]).GetComponent<RectTransform>() };

            #region 時間制限
            player_to_turn = new [] { GameObject.Find(SceneCommon.PlayerAndGameobject_to_path[(int)PlayerIndex.Player1, (int)GameobjectIndex.Turn]).GetComponent<Text>(), GameObject.Find(SceneCommon.PlayerAndGameobject_to_path[(int)PlayerIndex.Player2, (int)GameobjectIndex.Turn]).GetComponent<Text>() };
            player_to_time = new [] { GameObject.Find(SceneCommon.PlayerAndGameobject_to_path[(int)PlayerIndex.Player1, (int)GameobjectIndex.Time]).GetComponent<Text>(), GameObject.Find(SceneCommon.PlayerAndGameobject_to_path[(int)PlayerIndex.Player2, (int)GameobjectIndex.Time]).GetComponent<Text>() };
            player_to_turnOutline = new [] { GameObject.Find(SceneCommon.PlayerAndGameobject_to_path[(int)PlayerIndex.Player1, (int)GameobjectIndex.Turn]).GetComponent<Outline>(), GameObject.Find(SceneCommon.PlayerAndGameobject_to_path[(int)PlayerIndex.Player2, (int)GameobjectIndex.Turn]).GetComponent<Outline>() };
            player_to_timeOutline = new [] { GameObject.Find(SceneCommon.PlayerAndGameobject_to_path[(int)PlayerIndex.Player1, (int)GameobjectIndex.Time]).GetComponent<Outline>(), GameObject.Find(SceneCommon.PlayerAndGameobject_to_path[(int)PlayerIndex.Player2, (int)GameobjectIndex.Time]).GetComponent<Outline>() };
            InitTime();
            #endregion

            #region 攻撃力
            player_to_attackPower = new float[] { 0.0f, 0.0f };
            #endregion

            #region ラウンド
            playerAndRound_to_result = new GameObject[,] {
            { GameObject.Find("ResultP0_0"), GameObject.Find("ResultP0_1") },
            { GameObject.Find("ResultP1_0"), GameObject.Find("ResultP1_1") },
        };
            for (int iPlayer = (int)PlayerIndex.Player1; iPlayer < (int)PlayerIndex.Num; iPlayer++)
            {
                for (int iRound = 0; iRound < 2; iRound++)
                {
                    playerAndRound_to_result[iPlayer, iRound].SetActive(false);
                }
            }
            #endregion

            StateExTable.Instance.InsertAllStates();

            #region リセット（配列やスプライト等の初期設定が終わってから）
            ReadyingTime = 0;
            SetTeban(PlayerIndex.Player1);
            // コンピューターかどうか。
            for (int iPlayer = (int)PlayerIndex.Player1; iPlayer < (int)PlayerIndex.Num; iPlayer++)
            {
                player_to_charScript[iPlayer].isComputer = CommonScript.Player_to_computer[iPlayer];
            }
            #endregion
        }

        // Update is called once per frame
        void Update()
        {
            #region 対局開始表示
            ReadyingTime++;
            if (SceneCommon.READY_TIME_LENGTH == ReadyingTime)
            {
                fight0.SetActive(false);
                fight1.SetActive(false);
            }
            #endregion

            #region 投了判定
            for (int iLoser = (int)PlayerIndex.Player1; iLoser < (int)PlayerIndex.Num; iLoser++)
            {
                if (player_to_charScript[iLoser].IsResign)
                {
                    player_to_charScript[iLoser].IsResign = false;

                    PlayerIndex winner = CommonScript.ReverseTeban((PlayerIndex)iLoser);
                    player_to_winCount[(int)winner]++;

                    if ((int)PlayerIndex.Player1 == iLoser)
                    {
                        // １プレイヤーの投了
                        CommonScript.Result = Result.Player2_Win;
                    }
                    else if ((int)PlayerIndex.Player2 == iLoser)
                    {
                        // ２プレイヤーの投了
                        CommonScript.Result = Result.Player1_Win;
                    }
                    else
                    {
                        throw new UnityException("どっちが勝ったんだぜ☆？");
                    }

                    // 現在、何ラウンドか☆
                    int round;
                    if (!playerAndRound_to_result[iLoser, 0].activeSelf)//１ラウンド目の星が表示されていないとき。
                    {
                        round = 0;
                    }
                    else if (!playerAndRound_to_result[iLoser, 1].activeSelf)//２ラウンド目の星が表示されていないとき。
                    {
                        round = 1;
                        if (1 < player_to_winCount[(int)winner])//2本先取
                        {
                            SceneManager.LoadScene(CommonScript.Scene_to_name[(int)SceneIndex.Result]);
                            return;
                        }
                    }
                    else//星が２ラウンド目まで表示されているとき☆
                    {
                        round = 2;
                        SceneManager.LoadScene(CommonScript.Scene_to_name[(int)SceneIndex.Result]);
                        return;
                    }

                    //if (round < 2)
                    {
                        playerAndRound_to_result[(int)PlayerIndex.Player1, round].SetActive(true);
                        playerAndRound_to_result[(int)PlayerIndex.Player2, round].SetActive(true);

                        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/ResultMark0");
                        if (PlayerIndex.Player1 == winner)
                        {
                            playerAndRound_to_result[(int)PlayerIndex.Player1, round].GetComponent<Image>().sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("ResultMark0_0"));
                            playerAndRound_to_result[(int)PlayerIndex.Player2, round].GetComponent<Image>().sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("ResultMark0_1"));
                        }
                        else if (PlayerIndex.Player2 == winner)
                        {
                            playerAndRound_to_result[(int)PlayerIndex.Player1, round].GetComponent<Image>().sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("ResultMark0_1"));
                            playerAndRound_to_result[(int)PlayerIndex.Player2, round].GetComponent<Image>().sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("ResultMark0_0"));
                        }

                        InitTime();
                        InitBar();
                        isRoundFinished = false;
                        IsResignCalled = false;
                        resign0.SetActive(false);
                        player_to_playerChar[(int)PlayerIndex.Player1].transform.position = new Vector3(2.52f, 0.0f);
                        player_to_playerChar[(int)PlayerIndex.Player2].transform.position = new Vector3(-2.52f, 0.0f);
                        ReadyingTime = 0;
                        fight0.SetActive(true);
                        fight1.SetActive(true);
                    }
                }
            }
            #endregion

            #region 時間制限
            if (!isRoundFinished)
            {
                // カウントダウン
                player_to_timeCount[(int)CommonScript.Teban] -= Time.deltaTime; // 前のフレームからの経過時間を引くぜ☆
                player_to_time[(int)CommonScript.Teban].text = ((int)player_to_timeCount[(int)CommonScript.Teban]).ToString();
            }
            #endregion

            #region HP、残り時間判定
            if (!isRoundFinished)
            {
                //if (bar1_rt.sizeDelta.x <= 0 && bar2_rt.sizeDelta.x <= 0)
                //{
                //    // ダブル・ノックアウト
                //}
                //else
                PlayerIndex loser = PlayerIndex.Num;
                if (player_to_barTransform[(int)PlayerIndex.Player2].sizeDelta.x <= player_to_barTransform[(int)PlayerIndex.Player1].sizeDelta.x
                    || player_to_timeCount[(int)PlayerIndex.Player2] < 1.0f)
                {
                    // １プレイヤーの勝ち
                    loser = PlayerIndex.Player2;
                }
                else if (player_to_barTransform[(int)PlayerIndex.Player1].sizeDelta.x <= 0
                    || player_to_timeCount[(int)PlayerIndex.Player1] < 1.0f)
                {
                    // ２プレイヤーの勝ち
                    loser = (int)PlayerIndex.Player1;
                }

                if (PlayerIndex.Num != loser)
                {
                    isRoundFinished = true;
                    player_to_charScript[(int)loser].Pull_ResignByLose();
                }
            }
            #endregion
        }

        /// <summary>
        /// 手番を変えるぜ☆
        /// </summary>
        /// <param name="teban"></param>
        public void SetTeban(PlayerIndex teban)
        {
            CommonScript.Teban = teban;
            PlayerIndex opponent = CommonScript.ReverseTeban(teban);
            //Debug.Log("Teban = " + teban.ToString() + " Opponent = " + opponent.ToString());

            // 先手の色
            {
                player_to_turn[(int)teban].color = Color.white;
                player_to_time[(int)teban].color = Color.white;

                Color outlineColor;
                if (ColorUtility.TryParseHtmlString("#776DC180", out outlineColor))
                {
                    player_to_turnOutline[(int)teban].effectColor = outlineColor;
                    player_to_timeOutline[(int)teban].effectColor = outlineColor;
                }
            }

            // 後手の色
            {
                Color fontColor;
                if (ColorUtility.TryParseHtmlString("#A5A9A9FF", out fontColor))
                {
                    player_to_turn[(int)opponent].color = fontColor;
                    player_to_time[(int)opponent].color = fontColor;
                }

                Color outlineColor;
                if (ColorUtility.TryParseHtmlString("#35298E80", out outlineColor))
                {
                    player_to_turnOutline[(int)opponent].effectColor = outlineColor;
                    player_to_timeOutline[(int)opponent].effectColor = outlineColor;
                }
            }
        }

        public void InitTime()
        {
            player_to_timeCount = new float[] { 60.0f, 60.0f };
        }
        public void InitBar()
        {
            player_to_barTransform[(int)PlayerIndex.Player1].sizeDelta = new Vector2(
                1791.7f,
                player_to_barTransform[(int)PlayerIndex.Player1].sizeDelta.y
                );
            player_to_value[(int)PlayerIndex.Player1].text = ((int)0).ToString();
            player_to_value[(int)PlayerIndex.Player2].text = ((int)0).ToString();
        }
        public void OffsetBar(float value)
        {
            player_to_barTransform[(int)PlayerIndex.Player1].sizeDelta = new Vector2(
                player_to_barTransform[(int)PlayerIndex.Player1].sizeDelta.x + value,
                player_to_barTransform[(int)PlayerIndex.Player1].sizeDelta.y
                );

            // 見えていないところも含めた、bar1 の割合 -0.5～0.5。（真ん中を０とする）
            float rate = player_to_barTransform[(int)PlayerIndex.Player1].sizeDelta.x / player_to_barTransform[(int)PlayerIndex.Player2].sizeDelta.x - 0.5f;
            // 正負
            float sign = 0 <= rate ? 1.0f : -1.0f;
            // bar1 の割合 0～1。（真ん中を０とする絶対値）
            float score = Mathf.Abs(rate * 2.0f)// 0～1 に直す
                * 10000.0f; // 0～10000点に変換（見えているところの端を 2000 とする）
            if (9999.0f < score)
            {
                score = 9999.0f;
            }
            player_to_value[(int)PlayerIndex.Player1].text = ((int)score).ToString();
            player_to_value[(int)PlayerIndex.Player2].text = ((int)score).ToString();
            // 見えているところの半分で　357px　ぐらい。これが 2000点。
            // 全体を 20000点にしたいので、全体は 3570px か。

            if (0 <= sign)
            {
                player_to_value[(int)PlayerIndex.Player1].color = Color.white;
                player_to_value[(int)PlayerIndex.Player2].color = Color.red;
            }
            else
            {
                player_to_value[(int)PlayerIndex.Player1].color = Color.red;
                player_to_value[(int)PlayerIndex.Player2].color = Color.white;
            }
        }
        /// <summary>
        /// 参りましたの発声。
        /// </summary>
        public void ResignCall()
        {
            IsResignCalled = true;
            resign0.SetActive(true);
        }
    }
}
