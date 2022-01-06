namespace SceneMain
{
    using System.Collections.Generic;
    using Assets.Scripts.Model.Dto.Fight;
    using Assets.Scripts.Model.Dto.Input;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    /// <summary>
    /// メインシーンのメインカメラのスクリプトだぜ☆
    /// </summary>
    public class CameraBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Player dto(Data transfer object).
        /// </summary>
        Dictionary<PlayerIndex, PlayerDto> PlayerDTOs { get; set; }
        public Dictionary<PlayerIndex, PublicPlayerDto> PublicPlayerDTOs { get; set; }

        #region UI 表示物
        /// <summary>
        /// Ready message 0, 1. 対局開始テキスト。
        /// </summary>
        GameObject fight0;
        GameObject fight1;
        /// <summary>
        /// KO message. 参りましたテキスト。
        /// </summary>
        GameObject resign0;
        #endregion

        /// <summary>
        /// Ready message presentation time counter; 対局開始メッセージが表示されている時間カウンター。
        /// </summary>
        int readyingTime;
        public int ReadyingTime { get { return readyingTime; } set { readyingTime = value; } }

        #region ラウンド
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
            fight0 = GameObject.Find(ThisSceneDto.GameObjFight0);
            fight0.GetComponent<Text>().text = ThisSceneDto.CharacterToFightMessage[(int)CommonScript.UseCharacters[PlayerIndex.Player1]];
            fight1 = GameObject.Find(ThisSceneDto.GameObjFight1);
            resign0 = GameObject.Find(ThisSceneDto.GameObjResign0);
            resign0.SetActive(false);

            this.PlayerDTOs = new Dictionary<PlayerIndex, PlayerDto>()
            {
                {PlayerIndex.Player1, new PlayerDto(
                    // bar1のRectTransformコンポーネントをキャッシュ
                    GameObject.Find(ThisSceneDto.GameObjectPaths[PlayerIndex.Player1][(int)GameObjectTypeIndex.Bar]).GetComponent<RectTransform>(),
                    // 名前
                    GameObject.Find(ThisSceneDto.GameObjectPaths[PlayerIndex.Player1][(int)GameObjectTypeIndex.Name]).GetComponent<Text>(),
                    GameObject.Find(ThisSceneDto.GameObjectPaths[PlayerIndex.Player1][(int)GameObjectTypeIndex.Value]).GetComponent<Text>(),
                    GameObject.Find(ThisSceneDto.GameObjectPaths[PlayerIndex.Player1][(int)GameObjectTypeIndex.Player]),
                    new GameObject[]{ GameObject.Find("ResultP0_0"), GameObject.Find("ResultP0_1") },
                    GameObject.Find(ThisSceneDto.GameObjectPaths[PlayerIndex.Player1][(int)GameObjectTypeIndex.Turn]).GetComponent<Text>(),
                    GameObject.Find(ThisSceneDto.GameObjectPaths[PlayerIndex.Player1][(int)GameObjectTypeIndex.Turn]).GetComponent<Outline>(),
                    GameObject.Find(ThisSceneDto.GameObjectPaths[PlayerIndex.Player1][(int)GameObjectTypeIndex.Time]).GetComponent<Text>(),
                    GameObject.Find(ThisSceneDto.GameObjectPaths[PlayerIndex.Player1][(int)GameObjectTypeIndex.Time]).GetComponent<Outline>()
                    ) },
                {PlayerIndex.Player2, new PlayerDto(
                    GameObject.Find(ThisSceneDto.GameObjectPaths[PlayerIndex.Player2][(int)GameObjectTypeIndex.Bar]).GetComponent<RectTransform>(),
                    GameObject.Find(ThisSceneDto.GameObjectPaths[PlayerIndex.Player2][(int)GameObjectTypeIndex.Name]).GetComponent<Text>(),
                    GameObject.Find(ThisSceneDto.GameObjectPaths[PlayerIndex.Player2][(int)GameObjectTypeIndex.Value]).GetComponent<Text>(),
                    GameObject.Find(ThisSceneDto.GameObjectPaths[PlayerIndex.Player2][(int)GameObjectTypeIndex.Player]),
                    new GameObject[]{ GameObject.Find("ResultP1_0"), GameObject.Find("ResultP1_1") },
                    GameObject.Find(ThisSceneDto.GameObjectPaths[PlayerIndex.Player2][(int)GameObjectTypeIndex.Turn]).GetComponent<Text>(),
                    GameObject.Find(ThisSceneDto.GameObjectPaths[PlayerIndex.Player2][(int)GameObjectTypeIndex.Turn]).GetComponent<Outline>(),
                    GameObject.Find(ThisSceneDto.GameObjectPaths[PlayerIndex.Player2][(int)GameObjectTypeIndex.Time]).GetComponent<Text>(),
                    GameObject.Find(ThisSceneDto.GameObjectPaths[PlayerIndex.Player2][(int)GameObjectTypeIndex.Time]).GetComponent<Outline>()
                    ) },
            };
            this.PublicPlayerDTOs = new Dictionary<PlayerIndex, PublicPlayerDto>()
            {
                {PlayerIndex.Player1, new PublicPlayerDto() },
                {PlayerIndex.Player2, new PublicPlayerDto() },
            };

            #endregion

            #region 選択キャラクター
            foreach (var player in PlayerIndexes.All)
            {
                CharacterIndex character = CommonScript.UseCharacters[player];
                this.PlayerDTOs[player].PlayerName.text = ThisSceneDto.CharacterToNameRoma[(int)character];

                // アニメーター
                this.PlayerDTOs[player].PlayerCharAnimetor.runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load(ThisSceneDto.CharacterToAnimationController[(int)character]));
            }
            #endregion

            // 初期化。
            InitTime();
            this.PublicPlayerDTOs[PlayerIndex.Player1].AttackPower = 0.0f;
            this.PublicPlayerDTOs[PlayerIndex.Player2].AttackPower = 0.0f;

            //StateExTable.Instance.InsertAllStates();

            #region リセット（配列やスプライト等の初期設定が終わってから）
            ReadyingTime = 0;
            SetTeban(PlayerIndex.Player1);

            // コンピューターかどうか。
            foreach (var player in PlayerIndexes.All)
            {
                this.PlayerDTOs[player].PlayerCharScript.isComputer = CommonScript.computerFlags[player];
            }
            #endregion
        }

        // Update is called once per frame
        void Update()
        {
            #region 対局開始表示
            ReadyingTime++;
            if (ThisSceneDto.ReadyTimeLength == ReadyingTime)
            {
                fight0.SetActive(false);
                fight1.SetActive(false);
            }
            #endregion

            #region 投了判定
            foreach (var loser in PlayerIndexes.All)
            {
                if (this.PlayerDTOs[loser].PlayerCharScript.IsResign)
                {
                    this.PlayerDTOs[loser].PlayerCharScript.IsResign = false;

                    PlayerIndex winner = CommonScript.ReverseTeban(loser);
                    this.PlayerDTOs[winner].WinCount++;

                    if (PlayerIndex.Player1 == loser)
                    {
                        // １プレイヤーの投了
                        CommonScript.Result = Result.Player2_Win;
                    }
                    else if (PlayerIndex.Player2 == loser)
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
                    if (!this.PlayerDTOs[loser].RoundsResult[0].activeSelf)//１ラウンド目の星が表示されていないとき。
                    {
                        round = 0;
                    }
                    else if (!this.PlayerDTOs[loser].RoundsResult[1].activeSelf)//２ラウンド目の星が表示されていないとき。
                    {
                        round = 1;
                        if (1 < this.PlayerDTOs[winner].WinCount)//2本先取
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
                        this.PlayerDTOs[PlayerIndex.Player1].RoundsResult[round].SetActive(true);
                        this.PlayerDTOs[PlayerIndex.Player2].RoundsResult[round].SetActive(true);

                        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/ResultMark0");
                        if (PlayerIndex.Player1 == winner)
                        {
                            this.PlayerDTOs[PlayerIndex.Player1].RoundsResult[round].GetComponent<Image>().sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("ResultMark0_0"));
                            this.PlayerDTOs[PlayerIndex.Player2].RoundsResult[round].GetComponent<Image>().sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("ResultMark0_1"));
                        }
                        else if (PlayerIndex.Player2 == winner)
                        {
                            this.PlayerDTOs[PlayerIndex.Player1].RoundsResult[round].GetComponent<Image>().sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("ResultMark0_1"));
                            this.PlayerDTOs[PlayerIndex.Player2].RoundsResult[round].GetComponent<Image>().sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("ResultMark0_0"));
                        }

                        InitTime();
                        InitBar();
                        isRoundFinished = false;
                        IsResignCalled = false;
                        resign0.SetActive(false);
                        this.PlayerDTOs[PlayerIndex.Player1].PlayerChar.transform.position = new Vector3(2.52f, 0.0f);
                        this.PlayerDTOs[PlayerIndex.Player2].PlayerChar.transform.position = new Vector3(-2.52f, 0.0f);
                        ReadyingTime = 0;
                        fight0.SetActive(true);
                        fight1.SetActive(true);
                    }
                }
            }
            #endregion

            #region 
            // 時間制限
            if (!isRoundFinished)
            {
                // カウントダウン
                this.PlayerDTOs[CommonScript.Teban].TimeCount -= Time.deltaTime; // 前のフレームからの経過時間を引くぜ☆
                this.PlayerDTOs[CommonScript.Teban].Time.text = ((int)this.PlayerDTOs[CommonScript.Teban].TimeCount).ToString();
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
                PlayerIndex loser = PlayerIndex.None;
                if (this.PlayerDTOs[PlayerIndex.Player2].BarTransform.sizeDelta.x <= this.PlayerDTOs[PlayerIndex.Player1].BarTransform.sizeDelta.x
                    || this.PlayerDTOs[PlayerIndex.Player2].TimeCount < 1.0f)
                {
                    // １プレイヤーの勝ち
                    loser = PlayerIndex.Player2;
                }
                else if (this.PlayerDTOs[PlayerIndex.Player1].BarTransform.sizeDelta.x <= 0
                    || this.PlayerDTOs[PlayerIndex.Player1].TimeCount < 1.0f)
                {
                    // ２プレイヤーの勝ち
                    loser = PlayerIndex.Player1;
                }

                if (PlayerIndex.None != loser)
                {
                    isRoundFinished = true;
                    this.PlayerDTOs[loser].PlayerCharScript.Pull_ResignByLose();
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
                this.PlayerDTOs[teban].Turn.color = Color.white;
                this.PlayerDTOs[teban].Time.color = Color.white;

                Color outlineColor;
                if (ColorUtility.TryParseHtmlString("#776DC180", out outlineColor))
                {
                    this.PlayerDTOs[teban].TurnOutline.effectColor = outlineColor;
                    this.PlayerDTOs[teban].TimeOutline.effectColor = outlineColor;
                }
            }

            // 後手の色
            {
                Color fontColor;
                if (ColorUtility.TryParseHtmlString("#A5A9A9FF", out fontColor))
                {
                    this.PlayerDTOs[opponent].Turn.color = fontColor;
                    this.PlayerDTOs[opponent].Time.color = fontColor;
                }

                Color outlineColor;
                if (ColorUtility.TryParseHtmlString("#35298E80", out outlineColor))
                {
                    this.PlayerDTOs[opponent].TurnOutline.effectColor = outlineColor;
                    this.PlayerDTOs[opponent].TimeOutline.effectColor = outlineColor;
                }
            }
        }

        /// <summary>
        /// 初期化。
        /// </summary>
        public void InitTime()
        {
            this.PlayerDTOs[PlayerIndex.Player1].TimeCount = 60.0f;
            this.PlayerDTOs[PlayerIndex.Player2].TimeCount = 60.0f;
        }

        public void InitBar()
        {
            this.PlayerDTOs[PlayerIndex.Player1].BarTransform.sizeDelta = new Vector2(
                1791.7f,
                this.PlayerDTOs[PlayerIndex.Player1].BarTransform.sizeDelta.y
                );
            this.PlayerDTOs[PlayerIndex.Player1].Value.text = ((int)0).ToString();
            this.PlayerDTOs[PlayerIndex.Player2].Value.text = ((int)0).ToString();
        }
        public void OffsetBar(float value)
        {
            this.PlayerDTOs[PlayerIndex.Player1].BarTransform.sizeDelta = new Vector2(
                this.PlayerDTOs[PlayerIndex.Player1].BarTransform.sizeDelta.x + value,
                this.PlayerDTOs[PlayerIndex.Player1].BarTransform.sizeDelta.y
                );

            // 見えていないところも含めた、bar1 の割合 -0.5～0.5。（真ん中を０とする）
            float rate = this.PlayerDTOs[PlayerIndex.Player1].BarTransform.sizeDelta.x / this.PlayerDTOs[PlayerIndex.Player2].BarTransform.sizeDelta.x - 0.5f;
            // 正負
            float sign = 0 <= rate ? 1.0f : -1.0f;
            // bar1 の割合 0～1。（真ん中を０とする絶対値）
            float score = Mathf.Abs(rate * 2.0f)// 0～1 に直す
                * 10000.0f; // 0～10000点に変換（見えているところの端を 2000 とする）
            if (9999.0f < score)
            {
                score = 9999.0f;
            }
            this.PlayerDTOs[PlayerIndex.Player1].Value.text = ((int)score).ToString();
            this.PlayerDTOs[PlayerIndex.Player2].Value.text = ((int)score).ToString();
            // 見えているところの半分で　357px　ぐらい。これが 2000点。
            // 全体を 20000点にしたいので、全体は 3570px か。

            if (0 <= sign)
            {
                this.PlayerDTOs[PlayerIndex.Player1].Value.color = Color.white;
                this.PlayerDTOs[PlayerIndex.Player2].Value.color = Color.red;
            }
            else
            {
                this.PlayerDTOs[PlayerIndex.Player1].Value.color = Color.red;
                this.PlayerDTOs[PlayerIndex.Player2].Value.color = Color.white;
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
