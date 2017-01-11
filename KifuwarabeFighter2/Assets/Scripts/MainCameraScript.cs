using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// メインシーンのメインカメラのスクリプトだぜ☆
public class MainCameraScript : MonoBehaviour {

    int readyTime;
    public GameObject fight1;
    public GameObject fight2;
    #region 選択キャラクター
    public Text name1;
    public Text name2;
    private Text[] player_to_name;
    public GameObject char1;
    public GameObject char2;
    private GameObject[] player_to_char;//[プレイヤー番号]
    Animator anim1;
    Animator anim2;
    //public Animator anim1;
    //public Animator anim2;
    private Animator[] player_to_anime;//[プレイヤー番号]
    GameObject char1Attack;
    GameObject char2Attack;
    private GameObject[] player_to_charAttack;//[プレイヤー番号]
    private SpriteRenderer[] player_to_charAttackSpriteRenderer;//[プレイヤー番号]
    private BoxCollider2D[] player_to_charAttackBoxCollider2D;//[プレイヤー番号]
    ///// <summary>
    ///// [プレイヤー番号] 連射防止用。0 以下なら行動可能☆
    ///// </summary>
    //private int[] player_to_attacking;
    #endregion
    public GameObject bar1;
    public GameObject bar2;
    GameObject[,] result;//勝ち星
    public Text value1;
    public Text value2;
    RectTransform bar1_rt;
    RectTransform bar2_rt;
    #region 制限時間
    public Text turn1;
    public Text turn2;
    private Text[] turns;
    public Text time1;
    public Text time2;
    private Outline[] turnOutlines;
    private Outline[] timeOutlines;
    private Text[] times;
    private float[] player_to_timeCount;
    #endregion
    #region 歩行
    float speed2 = 4.0f; //歩行速度☆
    private Rigidbody2D[] rigidbody2Ds;//[プレイヤー番号]
    #endregion

    void Start()
    {
        #region 選択キャラクター
        player_to_name = new Text[] { name1, name2 };
        player_to_char = new GameObject[] { char1, char2 };
        player_to_anime = new Animator[] { char1.GetComponent<Animator>(), char2.GetComponent<Animator>() };
        //player_to_anime = new Animator[] { anim1, anim2 };
        player_to_charAttack = new GameObject[] { GameObject.Find("Char1Attack"), GameObject.Find("Char2Attack") };
        player_to_charAttackSpriteRenderer = new SpriteRenderer[] { player_to_charAttack[(int)PlayerIndex.Player1].GetComponent<SpriteRenderer>(), player_to_charAttack[(int)PlayerIndex.Player2].GetComponent<SpriteRenderer>() };
        player_to_charAttackBoxCollider2D = new BoxCollider2D[] { player_to_charAttack[(int)PlayerIndex.Player1].GetComponent<BoxCollider2D>(), player_to_charAttack[(int)PlayerIndex.Player2].GetComponent<BoxCollider2D>() };
        for (int iPlayer = (int)PlayerIndex.Player1; iPlayer<(int)PlayerIndex.Num; iPlayer++)
        {
            //player_to_attacking[iPlayer] = 0;

            CharacterIndex character = CommonScript.Player_To_UseCharacter[iPlayer];
            // 名前
            player_to_name[iPlayer].text = CommonScript.Character_To_NameRoma[(int)character];

            // アニメーター
            player_to_anime[iPlayer].runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load(CommonScript.Character_To_AnimationController[(int)character]));
        }
        #endregion

        //bar1のRectTransformコンポーネントをキャッシュ
        bar1_rt = bar1.GetComponent<RectTransform>();
        bar2_rt = bar2.GetComponent<RectTransform>();

        #region 時間制限
        turns = new Text[] { turn1, turn2 };
        times = new Text[] { time1, time2 };
        turnOutlines = new Outline[] { turn1.GetComponent<Outline>(), turn2.GetComponent<Outline>() };
        timeOutlines = new Outline[] { time1.GetComponent<Outline>(), time2.GetComponent<Outline>() };
        player_to_timeCount = new float[] { 120.0f, 120.0f };
        #endregion

        #region 歩行
        rigidbody2Ds = new Rigidbody2D[] { char1.GetComponent<Rigidbody2D>(), char2.GetComponent<Rigidbody2D>() };
        #endregion

        #region ラウンド
        result = new GameObject[,] {
            { GameObject.Find("ResultP1_1"), GameObject.Find("ResultP1_2") },
            { GameObject.Find("ResultP2_1"), GameObject.Find("ResultP2_2") },
        };
        for (int iPlayer=(int)PlayerIndex.Player1; iPlayer<(int)PlayerIndex.Num; iPlayer++)
        {
            for (int iRound = 0; iRound < 2; iRound++)
            {
                result[iPlayer, iRound].SetActive(false);
            }
        }
        #endregion

        #region リセット（配列やスプライト等の初期設定が終わってから）
        readyTime = 0;
        SetTeban(PlayerIndex.Player1);
        #endregion
    }

    private const int READY_TIME_LENGTH = 20;
    // Update is called once per frame
    void Update()
    {
        #region 対局開始表示
        readyTime++;
        if (READY_TIME_LENGTH == readyTime)
        {
            fight1.SetActive(false);
            fight2.SetActive(false);
        }
        #endregion

        #region 当たり判定
        if(READY_TIME_LENGTH < readyTime)
        {
            for (int iPlayer = (int)PlayerIndex.Player1; iPlayer < (int)PlayerIndex.Num; iPlayer++)
            {
                // 位置合わせ
                player_to_charAttack[(int)iPlayer].transform.position = player_to_char[(int)iPlayer].transform.position;

                // クリップ名取得
                Animator anime = player_to_anime[(int)iPlayer];
                AnimationClip clip = anime.GetCurrentAnimatorClipInfo(0)[0].clip;
                string clipName = clip.name;

                // 正規化時間取得
                float normalizedTime = player_to_anime[(int)iPlayer].GetCurrentAnimatorStateInfo(0).normalizedTime;

                // モーション・フレーム要素数取得
                int motionFrames = Mathf.CeilToInt(clip.length * clip.frameRate); // 2 = Ceil(0.133333 * 15)
                int currentMotionFrame = Mathf.FloorToInt( (normalizedTime % 1.0f) * motionFrames); // 1 = Floor( 0.5 * 2)


                CharacterIndex character = CommonScript.Player_To_UseCharacter[iPlayer];

                // 当たり判定くん　画像差し替え
                Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Attack1a");
                string sliceName = "";
                if (CommonScript.CharacterAndMotion_To_Slice[(int)character, (int)MotionIndex.Wait] == clipName)
                {
                    switch (currentMotionFrame)
                    {
                        case 0: sliceName = "Attack1a_0"; break;
                        case 1: sliceName = "Attack1a_1"; break;
                        case 2: sliceName = "Attack1a_2"; break;
                        case 3: sliceName = "Attack1a_3"; break;
                    }
                }
                else if (CommonScript.CharacterAndMotion_To_Slice[(int)character, (int)MotionIndex.LP] == clipName)
                {
                    switch (currentMotionFrame)
                    {
                        case 0: sliceName = "Attack1a_6"; break;
                        case 1: sliceName = "Attack1a_7"; break;
                    }
                }
                else if (CommonScript.CharacterAndMotion_To_Slice[(int)character, (int)MotionIndex.MP] == clipName)
                {
                    switch (currentMotionFrame)
                    {
                        case 0: sliceName = "Attack1a_6"; break;
                        case 1: sliceName = "Attack1a_7"; break;
                        case 2: sliceName = "Attack1a_8"; break;
                    }
                }
                else if (CommonScript.CharacterAndMotion_To_Slice[(int)character, (int)MotionIndex.HP] == clipName)
                {
                    switch (currentMotionFrame)
                    {
                        case 0: sliceName = "Attack1a_6"; break;
                        case 1: sliceName = "Attack1a_7"; break;
                        case 2: sliceName = "Attack1a_8"; break;
                        case 3: sliceName = "Attack1a_9"; break;
                        case 4: sliceName = "Attack1a_7"; break;
                    }
                }
                else if (CommonScript.CharacterAndMotion_To_Slice[(int)character, (int)MotionIndex.LK] == clipName)
                {
                    switch (currentMotionFrame)
                    {
                        case 0: sliceName = "Attack1a_13"; break;
                        case 1: sliceName = "Attack1a_14"; break;
                    }
                }
                else if (CommonScript.CharacterAndMotion_To_Slice[(int)character, (int)MotionIndex.MK] == clipName)
                {
                    switch (currentMotionFrame)
                    {
                        case 0: sliceName = "Attack1a_13"; break;
                        case 1: sliceName = "Attack1a_14"; break;
                        case 2: sliceName = "Attack1a_15"; break;
                    }
                }
                else if (CommonScript.CharacterAndMotion_To_Slice[(int)character, (int)MotionIndex.HK] == clipName)
                {
                    switch (currentMotionFrame)
                    {
                        case 0: sliceName = "Attack1a_13"; break;
                        case 1: sliceName = "Attack1a_14"; break;
                        case 2: sliceName = "Attack1a_15"; break;
                        case 3: sliceName = "Attack1a_16"; break;
                        case 4: sliceName = "Attack1a_14"; break;
                    }
                }

                if(""!= sliceName)
                {
                    player_to_charAttackSpriteRenderer[(int)iPlayer].sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals(sliceName));

                    player_to_charAttackBoxCollider2D[(int)iPlayer].offset.Set( 0.25f, 0.05f );
                    player_to_charAttackBoxCollider2D[(int)iPlayer].size.Set(0.3f, 0.3f);
                }

                Debug.Log("iPlayerIndex = " + iPlayer + " clip = " + clipName + " currentMotionFrame = " + currentMotionFrame + " motionFrames = " + motionFrames + " normalizedTime = " + normalizedTime + " length = " + clip.length + " frameRate = " + clip.frameRate + " sliceName = " + sliceName);
            }
        }
        #endregion

        #region 時間制限
        {
            // カウントダウン
            player_to_timeCount[(int)CommonScript.Teban] -= Time.deltaTime; // 前のフレームからの経過時間を引くぜ☆
            times[(int)CommonScript.Teban].text = ((int)player_to_timeCount[(int)CommonScript.Teban]).ToString();
        }
        #endregion

        #region HP、残り時間判定
        {
            //if (bar1_rt.sizeDelta.x <= 0 && bar2_rt.sizeDelta.x <= 0)
            //{
            //    // ダブル・ノックアウト
            //    CommonScript.Result = Result.Double_KnockOut;
            //    SceneManager.LoadScene("Result");
            //}
            //else
            PlayerIndex winner = PlayerIndex.Num;
            if (bar2_rt.sizeDelta.x <= bar1_rt.sizeDelta.x
                || player_to_timeCount[(int)PlayerIndex.Player2] < 1.0f)
            {
                // １プレイヤーの勝ち
                winner = PlayerIndex.Player1;
            }
            else if (bar1_rt.sizeDelta.x <= 0
                || player_to_timeCount[(int)PlayerIndex.Player1] < 1.0f)
            {
                // ２プレイヤーの勝ち
                winner = PlayerIndex.Player2;
            }

            if (PlayerIndex.Num != winner)
            {
                // 現在、何ラウンドか☆
                int round;
                if (!result[(int)PlayerIndex.Player1, 0].activeSelf)
                {
                    round = 0;
                }
                else if (!result[(int)PlayerIndex.Player1, 1].activeSelf)
                {
                    round = 1;
                }
                else
                {
                    round = 2;
                }

                if (PlayerIndex.Player1 == winner)
                {
                    if (2 == round)
                    {
                        // 決着☆
                        CommonScript.Result = Result.Player1_Win;
                        SceneManager.LoadScene("Result");
                    }
                    else
                    {
                        result[(int)PlayerIndex.Player1, round].SetActive(true);
                        result[(int)PlayerIndex.Player2, round].SetActive(true);

                        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/ResultMark1");
                        result[(int)PlayerIndex.Player1, round].GetComponent<Image>().sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("ResultMark1_0"));
                        result[(int)PlayerIndex.Player2, round].GetComponent<Image>().sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("ResultMark1_1"));

                        InitBar();
                    }
                }
                else if (PlayerIndex.Player2 == winner)
                {
                    if (2 == round)
                    {
                        // 決着☆
                        CommonScript.Result = Result.Player2_Win;
                        SceneManager.LoadScene("Result");
                    }
                    else
                    {
                        result[(int)PlayerIndex.Player1, round].SetActive(true);
                        result[(int)PlayerIndex.Player2, round].SetActive(true);

                        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/ResultMark1");
                        result[(int)PlayerIndex.Player1, round].GetComponent<Image>().sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("ResultMark1_1"));
                        result[(int)PlayerIndex.Player2, round].GetComponent<Image>().sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("ResultMark1_0"));

                        InitBar();
                    }
                }
            }
        }
        #endregion

        #region 行動
        //for (int iPlayer = (int)PlayerIndex.Player1; iPlayer < (int)PlayerIndex.Num; iPlayer++)
        //{
        //    player_to_attacking[iPlayer]--;
        //}
        //// １プレイヤー
        //if (player_to_attacking[(int)PlayerIndex.Player1] < 1)
        //{
            if (Input.GetButtonDown(CommonScript.BUTTON_03_P1_LP))
            {
                Debug.Log("button BUTTON_03_P1_LP");
                LightPunch(PlayerIndex.Player1);
            }
            else if (Input.GetButtonDown(CommonScript.BUTTON_04_P1_MP))
            {
                Debug.Log("button BUTTON_04_P1_MP");
                MediumPunch(PlayerIndex.Player1);
            }
            else if (Input.GetButtonDown(CommonScript.BUTTON_05_P1_HP))
            {
                Debug.Log("button BUTTON_05_P1_HP");
                HardPunch(PlayerIndex.Player1);
            }
            else if (Input.GetButtonDown(CommonScript.BUTTON_06_P1_LK))
            {
                Debug.Log("button BUTTON_06_P1_LK");
                LightKick(PlayerIndex.Player1);
            }
            else if (Input.GetButtonDown(CommonScript.BUTTON_07_P1_MK))
            {
                Debug.Log("button BUTTON_07_P1_MK");
                MediumKick(PlayerIndex.Player1);
            }
            else if (Input.GetButtonDown(CommonScript.BUTTON_08_P1_HK))
            {
                Debug.Log("button BUTTON_08_P1_HK");
                HardKick(PlayerIndex.Player1);
            }
            else if (Input.GetButtonDown(CommonScript.BUTTON_09_P1_PA))
            {
                Debug.Log("button BUTTON_09_P1_PA");
            }
        //}
        //// ２プレイヤー
        //if (player_to_attacking[(int)PlayerIndex.Player2] < 1)
        //{
            if (Input.GetButtonDown(CommonScript.BUTTON_13_P2_LP))
            {
                Debug.Log("button BUTTON_13_P2_LP");
                LightPunch(PlayerIndex.Player2);
            }
            else if (Input.GetButtonDown(CommonScript.BUTTON_14_P2_MP))
            {
                Debug.Log("button BUTTON_14_P2_MP");
                MediumPunch(PlayerIndex.Player2);
            }
            else if (Input.GetButtonDown(CommonScript.BUTTON_15_P2_HP))
            {
                Debug.Log("button BUTTON_15_P2_HP");
                HardPunch(PlayerIndex.Player2);
            }
            else if (Input.GetButtonDown(CommonScript.BUTTON_16_P2_LK))
            {
                Debug.Log("button BUTTON_16_P2_LK");
                LightKick(PlayerIndex.Player2);
            }
            else if (Input.GetButtonDown(CommonScript.BUTTON_17_P2_MK))
            {
                Debug.Log("button BUTTON_17_P2_MK");
                MediumKick(PlayerIndex.Player2);
            }
            else if (Input.GetButtonDown(CommonScript.BUTTON_18_P2_HK))
            {
                Debug.Log("button BUTTON_18_P2_HK");
                HardKick(PlayerIndex.Player2);
            }
            else if (Input.GetButtonDown(CommonScript.BUTTON_19_P2_PA))
            {
                Debug.Log("button BUTTON_19_P2_PA");
            }
        //}
        #endregion
    }

    void FixedUpdate()
    {
        for (int iPlayerIndex=(int)PlayerIndex.Player1; iPlayerIndex< (int)PlayerIndex.Num; iPlayerIndex++)
        {
            //左キー: -1、右キー: 1
            float leverX = Input.GetAxisRaw(CommonScript.PlayerAndButton_To_ButtonName[iPlayerIndex,(int)ButtonIndex.Horizontal]);

            if (leverX != 0)//左か右を入力したら
            {
                //Debug.Log("lever x = " + x.ToString());

                //入力方向へ移動
                rigidbody2Ds[iPlayerIndex].velocity = new Vector2(leverX * speed2, rigidbody2Ds[iPlayerIndex].velocity.y);
                //localScale.xを-1にすると画像が反転する
                Vector2 temp = player_to_char[iPlayerIndex].transform.localScale;
                temp.x = leverX * CommonScript.GRAPHIC_SCALE;
                player_to_char[iPlayerIndex].transform.localScale = temp;
                //Wait→Dash
                //anim.SetBool("Dash", true);
            }
            else//左も右も入力していなかったら
            {
                //横移動の速度を0にしてピタッと止まるようにする
                rigidbody2Ds[iPlayerIndex].velocity = new Vector2(0, rigidbody2Ds[iPlayerIndex].velocity.y);
                //Dash→Wait
                //anim.SetBool("Dash", false);
            }
        }
    }

    //private static int TEKITO_WAIT = 10;//適当に操作不能時間☆
    void LightPunch(PlayerIndex player)
    {
        // アニメーションの開始
        player_to_anime[(int)player].SetInteger("weight", (int)WeightIndex.Light);
        player_to_anime[(int)player].SetTrigger("punch");

        OffsetBar(-1.0f);
        //player_to_attacking[(int)player] = TEKITO_WAIT;
    }
    void MediumPunch(PlayerIndex player)
    {
        // アニメーションの開始
        player_to_anime[(int)player].SetInteger("weight", (int)WeightIndex.Medium);
        player_to_anime[(int)player].SetTrigger("punch");

        OffsetBar(-10.0f);
        //player_to_attacking[(int)player] = TEKITO_WAIT;
    }
    void HardPunch(PlayerIndex player)
    {
        // アニメーションの開始
        player_to_anime[(int)player].SetInteger("weight", (int)WeightIndex.Hard);
        player_to_anime[(int)player].SetTrigger("punch");

        OffsetBar(-100.0f);
        //player_to_attacking[(int)player] = TEKITO_WAIT;
    }
    void LightKick(PlayerIndex player)
    {
        // アニメーションの開始
        player_to_anime[(int)player].SetInteger("weight", (int)WeightIndex.Light);
        player_to_anime[(int)player].SetTrigger("kick");

        OffsetBar(1.0f);
        //player_to_attacking[(int)player] = TEKITO_WAIT;
    }
    void MediumKick(PlayerIndex player)
    {
        // アニメーションの開始
        player_to_anime[(int)player].SetInteger("weight", (int)WeightIndex.Medium);
        player_to_anime[(int)player].SetTrigger("kick");

        OffsetBar(10.0f);
        //player_to_attacking[(int)player] = TEKITO_WAIT;
    }
    void HardKick(PlayerIndex player)
    {
        // アニメーションの開始
        player_to_anime[(int)player].SetInteger("weight", (int)WeightIndex.Hard);
        player_to_anime[(int)player].SetTrigger("kick");

        OffsetBar(100.0f);
        //player_to_attacking[(int)player] = TEKITO_WAIT;
    }

    /// <summary>
    /// 手番を変えるぜ☆
    /// </summary>
    /// <param name="teban"></param>
    public void SetTeban(PlayerIndex teban)
    {
        CommonScript.Teban = teban;
        PlayerIndex opponent = CommonScript.ReverseTeban(teban);
        Debug.Log("Teban = " + teban.ToString() + " Opponent = " + opponent.ToString());

        // 先手の色
        {
            turns[(int)teban].color = Color.white;
            times[(int)teban].color = Color.white;

            Color outlineColor;
            if (ColorUtility.TryParseHtmlString("#776DC180", out outlineColor))
            {
                turnOutlines[(int)teban].effectColor = outlineColor;
                timeOutlines[(int)teban].effectColor = outlineColor;
            }
        }

        // 後手の色
        {
            Color fontColor;
            if (ColorUtility.TryParseHtmlString("#A5A9A9FF", out fontColor))
            {
                turns[(int)opponent].color = fontColor;
                times[(int)opponent].color = fontColor;
            }

            Color outlineColor;
            if (ColorUtility.TryParseHtmlString("#35298E80", out outlineColor))
            {
                turnOutlines[(int)opponent].effectColor = outlineColor;
                timeOutlines[(int)opponent].effectColor = outlineColor;
            }
        }
    }

    public void InitBar()
    {
        bar1_rt.sizeDelta = new Vector2(
            1791.7f,
            bar1_rt.sizeDelta.y
            );
        value1.text = ((int)0).ToString();
        value2.text = ((int)0).ToString();
    }
    public void OffsetBar(float value)
    {
        bar1_rt.sizeDelta = new Vector2(
            bar1_rt.sizeDelta.x + value,
            bar1_rt.sizeDelta.y
            );

        // 見えていないところも含めた、bar1 の割合 -0.5～0.5。（真ん中を０とする）
        float rate = bar1_rt.sizeDelta.x / bar2_rt.sizeDelta.x - 0.5f;
        // 正負
        float sign = 0 <= rate ? 1.0f : -1.0f;
        // bar1 の割合 0～1。（真ん中を０とする絶対値）
        float score = Mathf.Abs(rate * 2.0f)// 0～1 に直す
            * 10000.0f; // 0～10000点に変換（見えているところの端を 2000 とする）
        if (9999.0f < score)
        {
            score = 9999.0f;
        }
        value1.text = ((int)score).ToString();
        value2.text = ((int)score).ToString();
        // 見えているところの半分で　357px　ぐらい。これが 2000点。
        // 全体を 20000点にしたいので、全体は 3570px か。

        if (0<=sign)
        {
            value1.color = Color.white;
            value2.color = Color.red;
        }
        else
        {
            value1.color = Color.red;
            value2.color = Color.white;
        }
    }
}
