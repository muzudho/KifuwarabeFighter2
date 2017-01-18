using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEditor;

// メインシーンのメインカメラのスクリプトだぜ☆
public class MainCameraScript : MonoBehaviour {

    public int readyingTime;
    GameObject fight0;
    GameObject fight1;
    GameObject resign0;
    #region 選択キャラクター
    public Text name0;
    public Text name1;
    private Text[] player_to_name;
    public GameObject char0;
    public GameObject char1;
    private GameObject[] player_to_char;
    private CharacterScript[] player_to_charScript;
    Animator anim0;
    Animator anim1;
    private Animator[] player_to_anime;//[プレイヤー番号]
    //private BoxCollider2D[] player_to_charAttackImgBoxCollider2D;//[プレイヤー番号]
    /// <summary>
    /// 相手と向かい合うために使うぜ☆（＾▽＾）
    /// </summary>
    public float[] player_to_x;
    #endregion
    public GameObject bar0;
    public GameObject bar1;
    GameObject[,] playerAndRound_to_result;//勝ち星
    public Text value0;
    public Text value1;
    RectTransform bar0_rt;
    RectTransform bar1_rt;
    #region 制限時間
    public Text turn0;
    public Text turn1;
    private Text[] player_to_turn;
    public Text time0;
    public Text time1;
    private Text[] player_to_time;
    private Outline[] player_to_turnOutline;
    private Outline[] player_to_timeOutline;
    private float[] player_to_timeCount;
    #endregion
    #region 攻撃力
    public float[] player_to_attackPower;
    #endregion
    #region ラウンド
    int[] player_to_winCount;
    public const int READY_TIME_LENGTH = 60;
    bool isRoundFinished;
    public bool IsResignCalled { get; set; }
    #endregion

    void Start()
    {
        fight0 = GameObject.Find(CommonScript.SPRITE_FIGHT0);
        fight0.GetComponent<Text>().text = CommonScript.Character_To_FightMessage[(int)CommonScript.Player_To_UseCharacter[(int)PlayerIndex.Player1]];
        fight1 = GameObject.Find(CommonScript.SPRITE_FIGHT1);
        resign0 = GameObject.Find(CommonScript.SPRITE_RESIGN0);
        resign0.SetActive(false);
        #region 選択キャラクター
        player_to_name = new Text[] { name0, name1 };
        player_to_char = new GameObject[] { char0, char1 };
        player_to_charScript = new CharacterScript[] { char0.GetComponent<CharacterScript>(), char1.GetComponent<CharacterScript>() };
        player_to_anime = new Animator[] { char0.GetComponent<Animator>(), char1.GetComponent<Animator>() };
        //player_to_charAttackImgBoxCollider2D = new BoxCollider2D[] { player_to_charAttackImg[(int)PlayerIndex.Player1].GetComponent<BoxCollider2D>(), player_to_charAttackImg[(int)PlayerIndex.Player2].GetComponent<BoxCollider2D>() };
        player_to_winCount = new int[] { 0, 0 };
        for (int iPlayer = (int)PlayerIndex.Player1; iPlayer<(int)PlayerIndex.Num; iPlayer++)
        {
            CharacterIndex character = CommonScript.Player_To_UseCharacter[iPlayer];
            // 名前
            player_to_name[iPlayer].text = CommonScript.Character_To_NameRoma[(int)character];

            // アニメーター
            player_to_anime[iPlayer].runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load(CommonScript.Character_To_AnimationController[(int)character]));
        }
        player_to_x = new float[] { 0.0f, 0.0f };
        #endregion

        //bar1のRectTransformコンポーネントをキャッシュ
        bar0_rt = bar0.GetComponent<RectTransform>();
        bar1_rt = bar1.GetComponent<RectTransform>();

        #region 時間制限
        player_to_turn = new Text[] { turn0, turn1 };
        player_to_time = new Text[] { time0, time1 };
        player_to_turnOutline = new Outline[] { turn0.GetComponent<Outline>(), turn1.GetComponent<Outline>() };
        player_to_timeOutline = new Outline[] { time0.GetComponent<Outline>(), time1.GetComponent<Outline>() };
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
        for (int iPlayer=(int)PlayerIndex.Player1; iPlayer<(int)PlayerIndex.Num; iPlayer++)
        {
            for (int iRound = 0; iRound < 2; iRound++)
            {
                playerAndRound_to_result[iPlayer, iRound].SetActive(false);
            }
        }
        #endregion

        MotionDatabaseScript.ReadAstateHashs();

        #region リセット（配列やスプライト等の初期設定が終わってから）
        readyingTime = 0;
        SetTeban(PlayerIndex.Player1);
        // コンピューターかどうか。
        for (int iPlayer = (int)PlayerIndex.Player1; iPlayer < (int)PlayerIndex.Num; iPlayer++)
        {
            player_to_charScript[iPlayer].isComputer = CommonScript.Player_To_Computer[iPlayer];
        }
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        #region 対局開始表示
        readyingTime++;
        if (READY_TIME_LENGTH == readyingTime)
        {
            fight0.SetActive(false);
            fight1.SetActive(false);
        }
        #endregion

        #region 投了判定
        for (int iLoser = (int)PlayerIndex.Player1; iLoser < (int)PlayerIndex.Num; iLoser++)
        {
            if (player_to_charScript[iLoser].isResign)
            {
                player_to_charScript[iLoser].isResign = false;

                PlayerIndex winner = CommonScript.ReverseTeban((PlayerIndex)iLoser);
                player_to_winCount[(int)winner]++;

                if ((int)PlayerIndex.Player1==iLoser)
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
                    if (1<player_to_winCount[(int)winner])//2本先取
                    {
                        SceneManager.LoadScene(CommonScript.SCENE_RESULT);
                        return;
                    }
                }
                else//星が２ラウンド目まで表示されているとき☆
                {
                    round = 2;
                    SceneManager.LoadScene(CommonScript.SCENE_RESULT);
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
                    player_to_char[(int)PlayerIndex.Player1].transform.position = new Vector3( 2.52f, 0.0f);
                    player_to_char[(int)PlayerIndex.Player2].transform.position = new Vector3( -2.52f, 0.0f);
                    readyingTime = 0;
                    fight0.SetActive(true);
                    fight1.SetActive(true);
                }
            }
        }
        #endregion

        //// 当たり判定くん
        //for (int iPlayer = (int)PlayerIndex.Player1; iPlayer < (int)PlayerIndex.Num; iPlayer++)
        //{
        //    UpdateHitbox2D((PlayerIndex)iPlayer);
        //}

        #region 時間制限
        if (!isRoundFinished)
        {
            // カウントダウン
            player_to_timeCount[(int)CommonScript.Teban] -= Time.deltaTime; // 前のフレームからの経過時間を引くぜ☆
            player_to_time[(int)CommonScript.Teban].text = ((int)player_to_timeCount[(int)CommonScript.Teban]).ToString();
        }
        #endregion

        #region HP、残り時間判定
        if(!isRoundFinished)
        {
            //if (bar1_rt.sizeDelta.x <= 0 && bar2_rt.sizeDelta.x <= 0)
            //{
            //    // ダブル・ノックアウト
            //}
            //else
            PlayerIndex loser = PlayerIndex.Num;
            if (bar1_rt.sizeDelta.x <= bar0_rt.sizeDelta.x
                || player_to_timeCount[(int)PlayerIndex.Player2] < 1.0f)
            {
                // １プレイヤーの勝ち
                loser = PlayerIndex.Player2;
            }
            else if (bar0_rt.sizeDelta.x <= 0
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
        bar0_rt.sizeDelta = new Vector2(
            1791.7f,
            bar0_rt.sizeDelta.y
            );
        value0.text = ((int)0).ToString();
        value1.text = ((int)0).ToString();
    }
    public void OffsetBar(float value)
    {
        bar0_rt.sizeDelta = new Vector2(
            bar0_rt.sizeDelta.x + value,
            bar0_rt.sizeDelta.y
            );

        // 見えていないところも含めた、bar1 の割合 -0.5～0.5。（真ん中を０とする）
        float rate = bar0_rt.sizeDelta.x / bar1_rt.sizeDelta.x - 0.5f;
        // 正負
        float sign = 0 <= rate ? 1.0f : -1.0f;
        // bar1 の割合 0～1。（真ん中を０とする絶対値）
        float score = Mathf.Abs(rate * 2.0f)// 0～1 に直す
            * 10000.0f; // 0～10000点に変換（見えているところの端を 2000 とする）
        if (9999.0f < score)
        {
            score = 9999.0f;
        }
        value0.text = ((int)score).ToString();
        value1.text = ((int)score).ToString();
        // 見えているところの半分で　357px　ぐらい。これが 2000点。
        // 全体を 20000点にしたいので、全体は 3570px か。

        if (0<=sign)
        {
            value0.color = Color.white;
            value1.color = Color.red;
        }
        else
        {
            value0.color = Color.red;
            value1.color = Color.white;
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
