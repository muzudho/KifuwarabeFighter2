namespace SceneMain
{
    using System.Collections.Generic;
    using Assets.Scripts.Model.Dto.Input;
    using Assets.Scripts.SceneMain;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    /// <summary>
    /// メインシーンのメインカメラのスクリプトだぜ☆
    /// </summary>
    public class CameraScript : MonoBehaviour
    {
        #region UI 表示物
        /// <summary>
        /// Player name on bar. プレイヤー名。
        /// </summary>
        Dictionary<PlayerIndex, Text> playerNnames;
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
        Dictionary<PlayerIndex, RectTransform> barTransforms;
        /// <summary>
        /// life point. 評価値。
        /// </summary>
        Dictionary<PlayerIndex, Text> playerValues;
        #endregion

        /// <summary>
        /// Ready message presentation time counter; 対局開始メッセージが表示されている時間カウンター。
        /// </summary>
        int readyingTime;
        public int ReadyingTime { get { return readyingTime; } set { readyingTime = value; } }

        #region 選択キャラクター
        /// <summary>
        /// player character. プレイヤー・キャラ。
        /// </summary>
        Dictionary<PlayerIndex,GameObject> playerChars;
        /// <summary>
        /// player character's attached script. プレイヤー・キャラにアタッチされているスクリプト。
        /// </summary>
        Dictionary<PlayerIndex, PlayerScript> charScripts;
        /// <summary>
        /// players animator. アニメーター。
        /// </summary>
        Dictionary<PlayerIndex, Animator> animetors;
        #endregion
        /// <summary>
        /// win,lose mark. 勝ち星
        /// </summary>
        Dictionary<PlayerIndex, GameObject[]> roundToResult;
        #region 制限時間
        /// <summary>
        /// turn. ターン。
        /// </summary>
        Dictionary<PlayerIndex, Text> turns;
        Dictionary<PlayerIndex, Outline> turnOutlines;
        /// <summary>
        /// timer. 残りタイマー。
        /// </summary>
        Dictionary<PlayerIndex, Text> times;
        Dictionary<PlayerIndex, Outline> timeOutlines;
        Dictionary<PlayerIndex, float> timeCounts;
        #endregion
        #region 攻撃力
        /// <summary>
        /// point for calculate damage. ダメージ計算のための数字。
        /// </summary>
        private Dictionary<PlayerIndex, float> attackPowers;
        public Dictionary<PlayerIndex, float> AttackPowers { get { return attackPowers; } }
        #endregion
        #region ラウンド
        /// <summary>
        /// Win count. 勝ち数。２本先取か数える。
        /// </summary>
        Dictionary<PlayerIndex, int> winCounts;
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
            fight0 = GameObject.Find(ThisSceneConst.GameObjFight0);
            fight0.GetComponent<Text>().text = ThisSceneConst.CharacterToFightMessage[(int)CommonScript.UseCharacters[PlayerIndex.Player1]];
            fight1 = GameObject.Find(ThisSceneConst.GameObjFight1);
            resign0 = GameObject.Find(ThisSceneConst.GameObjResign0);
            resign0.SetActive(false);
            playerNnames = new Dictionary<PlayerIndex, Text>()
            {
                {PlayerIndex.Player1, GameObject.Find(ThisSceneConst.GameObjectPaths[PlayerIndex.Player1][(int)GameobjectIndex.Name]).GetComponent<Text>() },
                {PlayerIndex.Player2, GameObject.Find(ThisSceneConst.GameObjectPaths[PlayerIndex.Player2][(int)GameobjectIndex.Name]).GetComponent<Text>() },
            };
            playerValues = new Dictionary<PlayerIndex, Text>()
            {
                {PlayerIndex.Player1, GameObject.Find(ThisSceneConst.GameObjectPaths[PlayerIndex.Player1][(int)GameobjectIndex.Value]).GetComponent<Text>() },
                {PlayerIndex.Player2, GameObject.Find(ThisSceneConst.GameObjectPaths[PlayerIndex.Player2][(int)GameobjectIndex.Value]).GetComponent<Text>() },
            };
            #endregion
            #region Internal variable 内部変数
            winCounts = new Dictionary<PlayerIndex, int>()
            {
                {PlayerIndex.Player1, 0 },
                {PlayerIndex.Player2, 0 },
            };
            #endregion
            #region 選択キャラクター
            playerChars = new Dictionary<PlayerIndex, GameObject>()
            {
                {PlayerIndex.Player1, GameObject.Find(ThisSceneConst.GameObjectPaths[PlayerIndex.Player1][(int)GameobjectIndex.Player])},
                {PlayerIndex.Player2, GameObject.Find(ThisSceneConst.GameObjectPaths[PlayerIndex.Player2][(int)GameobjectIndex.Player])},
            };
            charScripts = new Dictionary<PlayerIndex, PlayerScript>()
            {
                {PlayerIndex.Player1, playerChars[PlayerIndex.Player1].GetComponent<PlayerScript>() },
                {PlayerIndex.Player2, playerChars[PlayerIndex.Player2].GetComponent<PlayerScript>() },
            };
            animetors = new Dictionary<PlayerIndex, Animator>()
            {
                {PlayerIndex.Player1, playerChars[PlayerIndex.Player1].GetComponent<Animator>() },
                {PlayerIndex.Player2, playerChars[PlayerIndex.Player2].GetComponent<Animator>() },
            };

            foreach (var player in PlayerIndexes.All)
            {
                CharacterIndex character = CommonScript.UseCharacters[player];
                // 名前
                playerNnames[player].text = ThisSceneConst.CharacterToNameRoma[(int)character];

                // アニメーター
                animetors[player].runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load(ThisSceneConst.CharacterToAnimationController[(int)character]));
            }
            #endregion

            //bar1のRectTransformコンポーネントをキャッシュ
            barTransforms = new Dictionary<PlayerIndex, RectTransform>()
            {
                {PlayerIndex.Player1, GameObject.Find(ThisSceneConst.GameObjectPaths[PlayerIndex.Player1][(int)GameobjectIndex.Bar]).GetComponent<RectTransform>() },
                {PlayerIndex.Player2, GameObject.Find(ThisSceneConst.GameObjectPaths[PlayerIndex.Player2][(int)GameobjectIndex.Bar]).GetComponent<RectTransform>() },
            };

            #region 時間制限
            turns = new Dictionary<PlayerIndex, Text>()
            {
                {PlayerIndex.Player1, GameObject.Find(ThisSceneConst.GameObjectPaths[PlayerIndex.Player1][(int)GameobjectIndex.Turn]).GetComponent<Text>() },
                {PlayerIndex.Player2, GameObject.Find(ThisSceneConst.GameObjectPaths[PlayerIndex.Player2][(int)GameobjectIndex.Turn]).GetComponent<Text>() },
            };
            times = new Dictionary<PlayerIndex, Text>()
            {
                {PlayerIndex.Player1, GameObject.Find(ThisSceneConst.GameObjectPaths[PlayerIndex.Player1][(int)GameobjectIndex.Time]).GetComponent<Text>() },
                {PlayerIndex.Player2, GameObject.Find(ThisSceneConst.GameObjectPaths[PlayerIndex.Player2][(int)GameobjectIndex.Time]).GetComponent<Text>() },
            };
            turnOutlines = new Dictionary<PlayerIndex, Outline>()
            {
                {PlayerIndex.Player1, GameObject.Find(ThisSceneConst.GameObjectPaths[PlayerIndex.Player1][(int)GameobjectIndex.Turn]).GetComponent<Outline>() },
                {PlayerIndex.Player2, GameObject.Find(ThisSceneConst.GameObjectPaths[PlayerIndex.Player2][(int)GameobjectIndex.Turn]).GetComponent<Outline>() },
            };
            timeOutlines = new Dictionary<PlayerIndex, Outline>()
            {
                {PlayerIndex.Player1, GameObject.Find(ThisSceneConst.GameObjectPaths[PlayerIndex.Player1][(int)GameobjectIndex.Time]).GetComponent<Outline>() },
                {PlayerIndex.Player2, GameObject.Find(ThisSceneConst.GameObjectPaths[PlayerIndex.Player2][(int)GameobjectIndex.Time]).GetComponent<Outline>() },
            };
            InitTime();
            #endregion

            #region 攻撃力
            attackPowers = new Dictionary<PlayerIndex, float>()
            {
                {PlayerIndex.Player1, 0.0f },
                {PlayerIndex.Player2, 0.0f },
            };
            #endregion

            #region ラウンド
            roundToResult = new Dictionary<PlayerIndex, GameObject[]>() {
                {PlayerIndex.Player1, new GameObject[]{ GameObject.Find("ResultP0_0"), GameObject.Find("ResultP0_1") } },
                { PlayerIndex.Player2, new GameObject[]{ GameObject.Find("ResultP1_0"), GameObject.Find("ResultP1_1") } },
            };

            foreach (var player in PlayerIndexes.All)
            {
                for (int iRound = 0; iRound < 2; iRound++)
                {
                    roundToResult[player][iRound].SetActive(false);
                }
            }
            #endregion

            //StateExTable.Instance.InsertAllStates();

            #region リセット（配列やスプライト等の初期設定が終わってから）
            ReadyingTime = 0;
            SetTeban(PlayerIndex.Player1);

            // コンピューターかどうか。
            foreach (var player in PlayerIndexes.All)
            {
                charScripts[player].isComputer = CommonScript.computerFlags[player];
            }
            #endregion
        }

        // Update is called once per frame
        void Update()
        {
            #region 対局開始表示
            ReadyingTime++;
            if (ThisSceneConst.ReadyTimeLength == ReadyingTime)
            {
                fight0.SetActive(false);
                fight1.SetActive(false);
            }
            #endregion

            #region 投了判定
            foreach (var loser in PlayerIndexes.All)
            {
                if (charScripts[loser].IsResign)
                {
                    charScripts[loser].IsResign = false;

                    PlayerIndex winner = CommonScript.ReverseTeban(loser);
                    winCounts[winner]++;

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
                    if (!roundToResult[loser][0].activeSelf)//１ラウンド目の星が表示されていないとき。
                    {
                        round = 0;
                    }
                    else if (!roundToResult[loser][1].activeSelf)//２ラウンド目の星が表示されていないとき。
                    {
                        round = 1;
                        if (1 < winCounts[winner])//2本先取
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
                        roundToResult[PlayerIndex.Player1][round].SetActive(true);
                        roundToResult[PlayerIndex.Player2][round].SetActive(true);

                        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/ResultMark0");
                        if (PlayerIndex.Player1 == winner)
                        {
                            roundToResult[PlayerIndex.Player1][round].GetComponent<Image>().sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("ResultMark0_0"));
                            roundToResult[PlayerIndex.Player2][round].GetComponent<Image>().sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("ResultMark0_1"));
                        }
                        else if (PlayerIndex.Player2 == winner)
                        {
                            roundToResult[PlayerIndex.Player1][round].GetComponent<Image>().sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("ResultMark0_1"));
                            roundToResult[PlayerIndex.Player2][round].GetComponent<Image>().sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("ResultMark0_0"));
                        }

                        InitTime();
                        InitBar();
                        isRoundFinished = false;
                        IsResignCalled = false;
                        resign0.SetActive(false);
                        playerChars[PlayerIndex.Player1].transform.position = new Vector3(2.52f, 0.0f);
                        playerChars[PlayerIndex.Player2].transform.position = new Vector3(-2.52f, 0.0f);
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
                timeCounts[CommonScript.Teban] -= Time.deltaTime; // 前のフレームからの経過時間を引くぜ☆
                times[CommonScript.Teban].text = ((int)timeCounts[CommonScript.Teban]).ToString();
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
                if (barTransforms[PlayerIndex.Player2].sizeDelta.x <= barTransforms[PlayerIndex.Player1].sizeDelta.x
                    || timeCounts[PlayerIndex.Player2] < 1.0f)
                {
                    // １プレイヤーの勝ち
                    loser = PlayerIndex.Player2;
                }
                else if (barTransforms[PlayerIndex.Player1].sizeDelta.x <= 0
                    || timeCounts[PlayerIndex.Player1] < 1.0f)
                {
                    // ２プレイヤーの勝ち
                    loser = PlayerIndex.Player1;
                }

                if (PlayerIndex.None != loser)
                {
                    isRoundFinished = true;
                    charScripts[loser].Pull_ResignByLose();
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
                turns[teban].color = Color.white;
                times[teban].color = Color.white;

                Color outlineColor;
                if (ColorUtility.TryParseHtmlString("#776DC180", out outlineColor))
                {
                    turnOutlines[teban].effectColor = outlineColor;
                    timeOutlines[teban].effectColor = outlineColor;
                }
            }

            // 後手の色
            {
                Color fontColor;
                if (ColorUtility.TryParseHtmlString("#A5A9A9FF", out fontColor))
                {
                    turns[opponent].color = fontColor;
                    times[opponent].color = fontColor;
                }

                Color outlineColor;
                if (ColorUtility.TryParseHtmlString("#35298E80", out outlineColor))
                {
                    turnOutlines[opponent].effectColor = outlineColor;
                    timeOutlines[opponent].effectColor = outlineColor;
                }
            }
        }

        public void InitTime()
        {
            timeCounts = new Dictionary<PlayerIndex, float>()
            {
                {PlayerIndex.Player1, 60.0f },
                {PlayerIndex.Player2, 60.0f },
            };
        }
        public void InitBar()
        {
            barTransforms[PlayerIndex.Player1].sizeDelta = new Vector2(
                1791.7f,
                barTransforms[PlayerIndex.Player1].sizeDelta.y
                );
            playerValues[PlayerIndex.Player1].text = ((int)0).ToString();
            playerValues[PlayerIndex.Player2].text = ((int)0).ToString();
        }
        public void OffsetBar(float value)
        {
            barTransforms[PlayerIndex.Player1].sizeDelta = new Vector2(
                barTransforms[PlayerIndex.Player1].sizeDelta.x + value,
                barTransforms[PlayerIndex.Player1].sizeDelta.y
                );

            // 見えていないところも含めた、bar1 の割合 -0.5～0.5。（真ん中を０とする）
            float rate = barTransforms[PlayerIndex.Player1].sizeDelta.x / barTransforms[PlayerIndex.Player2].sizeDelta.x - 0.5f;
            // 正負
            float sign = 0 <= rate ? 1.0f : -1.0f;
            // bar1 の割合 0～1。（真ん中を０とする絶対値）
            float score = Mathf.Abs(rate * 2.0f)// 0～1 に直す
                * 10000.0f; // 0～10000点に変換（見えているところの端を 2000 とする）
            if (9999.0f < score)
            {
                score = 9999.0f;
            }
            playerValues[PlayerIndex.Player1].text = ((int)score).ToString();
            playerValues[PlayerIndex.Player2].text = ((int)score).ToString();
            // 見えているところの半分で　357px　ぐらい。これが 2000点。
            // 全体を 20000点にしたいので、全体は 3570px か。

            if (0 <= sign)
            {
                playerValues[PlayerIndex.Player1].color = Color.white;
                playerValues[PlayerIndex.Player2].color = Color.red;
            }
            else
            {
                playerValues[PlayerIndex.Player1].color = Color.red;
                playerValues[PlayerIndex.Player2].color = Color.white;
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
