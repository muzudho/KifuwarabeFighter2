using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// メインシーンのメインカメラのスクリプトだぜ☆
public class MainCameraScript : MonoBehaviour {

    int readyTime;
    public GameObject fight0;
    public GameObject fight1;
    #region 選択キャラクター
    public Text name0;
    public Text name1;
    private Text[] player_to_name;
    public GameObject char0;
    public GameObject char1;
    private GameObject[] player_to_char;//[プレイヤー番号]
    Animator anim0;
    Animator anim1;
    private Animator[] player_to_anime;//[プレイヤー番号]
    //private GameObject[] player_to_charAttack;//[プレイヤー番号]
    private GameObject[] player_to_charAttackImg;//[プレイヤー番号]
    //private SpriteRenderer[] player_to_charAttackSpriteRenderer;//[プレイヤー番号]
    private SpriteRenderer[] player_to_charAttackImgSpriteRenderer;//[プレイヤー番号]
    //private BoxCollider2D[] player_to_charAttackBoxCollider2D;//[プレイヤー番号]
    private BoxCollider2D[] player_to_charAttackImgBoxCollider2D;//[プレイヤー番号]
    ///// <summary>
    ///// [プレイヤー番号] 連射防止用。0 以下なら行動可能☆
    ///// </summary>
    //private int[] player_to_attacking;
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
    #region 歩行
    float speed = 4.0f; //歩行速度☆
    private Rigidbody2D[] player_to_rigidbody2D;//[プレイヤー番号]
    #endregion

    void Start()
    {
        #region 選択キャラクター
        player_to_name = new Text[] { name0, name1 };
        player_to_char = new GameObject[] { char0, char1 };
        player_to_anime = new Animator[] { char0.GetComponent<Animator>(), char1.GetComponent<Animator>() };
        //player_to_charAttack = new GameObject[] { GameObject.Find(CommonScript.Player_To_CharAttack[(int)PlayerIndex.Player1]), GameObject.Find(CommonScript.Player_To_CharAttack[(int)PlayerIndex.Player2]) };
        player_to_charAttackImg = new GameObject[] { GameObject.Find(CommonScript.Player_To_Attacker[(int)PlayerIndex.Player1]), GameObject.Find(CommonScript.Player_To_Attacker[(int)PlayerIndex.Player2]) };
        //player_to_charAttackSpriteRenderer = new SpriteRenderer[] { player_to_charAttack[(int)PlayerIndex.Player1].GetComponent<SpriteRenderer>(), player_to_charAttack[(int)PlayerIndex.Player2].GetComponent<SpriteRenderer>() };
        player_to_charAttackImgSpriteRenderer = new SpriteRenderer[] { player_to_charAttackImg[(int)PlayerIndex.Player1].GetComponent<SpriteRenderer>(), player_to_charAttackImg[(int)PlayerIndex.Player2].GetComponent<SpriteRenderer>() };
        //player_to_charAttackBoxCollider2D = new BoxCollider2D[] { player_to_charAttack[(int)PlayerIndex.Player1].GetComponent<BoxCollider2D>(), player_to_charAttack[(int)PlayerIndex.Player2].GetComponent<BoxCollider2D>() };
        player_to_charAttackImgBoxCollider2D = new BoxCollider2D[] { player_to_charAttackImg[(int)PlayerIndex.Player1].GetComponent<BoxCollider2D>(), player_to_charAttackImg[(int)PlayerIndex.Player2].GetComponent<BoxCollider2D>() };
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
        bar0_rt = bar0.GetComponent<RectTransform>();
        bar1_rt = bar1.GetComponent<RectTransform>();

        #region 時間制限
        player_to_turn = new Text[] { turn0, turn1 };
        player_to_time = new Text[] { time0, time1 };
        player_to_turnOutline = new Outline[] { turn0.GetComponent<Outline>(), turn1.GetComponent<Outline>() };
        player_to_timeOutline = new Outline[] { time0.GetComponent<Outline>(), time1.GetComponent<Outline>() };
        player_to_timeCount = new float[] { 60.0f, 60.0f };
        #endregion

        #region 歩行
        player_to_rigidbody2D = new Rigidbody2D[] { char0.GetComponent<Rigidbody2D>(), char1.GetComponent<Rigidbody2D>() };
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
            fight0.SetActive(false);
            fight1.SetActive(false);
        }
        #endregion

        #region 当たり判定
        if(READY_TIME_LENGTH < readyTime)
        {
            for (int iPlayer = (int)PlayerIndex.Player1; iPlayer < (int)PlayerIndex.Num; iPlayer++)
            {
                // 位置合わせ
                //player_to_charAttack[(int)iPlayer].transform.position = player_to_char[(int)iPlayer].transform.position;

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
                Sprite[] sprites = Resources.LoadAll<Sprite>(CommonScript.Character_To_Attack(character));
                int slice = -1;
                string sliceName = "";
                if (CommonScript.CharacterAndMotion_To_Clip[(int)character, (int)MotionIndex.Wait] == clipName)
                {
                    switch (currentMotionFrame)
                    {
                        case 0: slice = 0; break;
                        case 1: slice = 1; break;
                        case 2: slice = 2; break;
                        case 3: slice = 3; break;
                    }
                    if (-1 != slice)
                    {
                        sliceName = CommonScript.CharacterAndSlice_To_AttackSlice(character, slice);
                    }
                }
                else if (CommonScript.CharacterAndMotion_To_Clip[(int)character, (int)MotionIndex.LP] == clipName)
                {
                    switch (currentMotionFrame)
                    {
                        case 0: slice = 8; break;
                        case 1: slice = 9; break;
                    }
                    if (-1 != slice)
                    {
                        sliceName = CommonScript.CharacterAndSlice_To_AttackSlice(character, slice);
                    }
                }
                else if (CommonScript.CharacterAndMotion_To_Clip[(int)character, (int)MotionIndex.MP] == clipName)
                {
                    switch (currentMotionFrame)
                    {
                        case 0: slice = 8; break;
                        case 1: slice = 9; break;
                        case 2: slice = 10; break;
                    }
                    if (-1 != slice)
                    {
                        sliceName = CommonScript.CharacterAndSlice_To_AttackSlice(character, slice);
                    }
                }
                else if (CommonScript.CharacterAndMotion_To_Clip[(int)character, (int)MotionIndex.HP] == clipName)
                {
                    switch (currentMotionFrame)
                    {
                        case 0: slice = 8; break;
                        case 1: slice = 9; break;
                        case 2: slice = 10; break;
                        case 3: slice = 11; break;
                        case 4: slice = 9; break;
                    }
                    if (-1 != slice)
                    {
                        sliceName = CommonScript.CharacterAndSlice_To_AttackSlice(character, slice);
                    }
                }
                else if (CommonScript.CharacterAndMotion_To_Clip[(int)character, (int)MotionIndex.LK] == clipName)
                {
                    switch (currentMotionFrame)
                    {
                        case 0: slice = 16; break;
                        case 1: slice = 17; break;
                    }
                    if (-1 != slice)
                    {
                        sliceName = CommonScript.CharacterAndSlice_To_AttackSlice(character, slice);
                    }
                }
                else if (CommonScript.CharacterAndMotion_To_Clip[(int)character, (int)MotionIndex.MK] == clipName)
                {
                    switch (currentMotionFrame)
                    {
                        case 0: slice = 16; break;
                        case 1: slice = 17; break;
                        case 2: slice = 18; break;
                    }
                    if (-1 != slice)
                    {
                        sliceName = CommonScript.CharacterAndSlice_To_AttackSlice(character, slice);
                    }
                }
                else if (CommonScript.CharacterAndMotion_To_Clip[(int)character, (int)MotionIndex.HK] == clipName)
                {
                    switch (currentMotionFrame)
                    {
                        case 0: slice = 16; break;
                        case 1: slice = 17; break;
                        case 2: slice = 18; break;
                        case 3: slice = 19; break;
                        case 4: slice = 17; break;
                    }
                    if (-1 != slice)
                    {
                        sliceName = CommonScript.CharacterAndSlice_To_AttackSlice(character, slice);
                    }
                }
                //Debug.Log("collider clipName = " + clipName + " currentMotionFrame = " + currentMotionFrame + " sliceName = " + sliceName);

                if (""!= sliceName)
                {
                    // 当たり判定くん 画像差し替え
                    //player_to_charAttackSpriteRenderer[iPlayer].sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals(sliceName));

                    if (-1 != slice)
                    {
                        // TODO: 当たり判定
                        //player_to_charAttackBoxCollider2D[iPlayer].offset.Set(CommonScript.CharacterAndSlice_To_OffsetX[(int)character, slice], CommonScript.CharacterAndSlice_To_OffsetY[(int)character, slice]);
                        //player_to_charAttackBoxCollider2D[iPlayer].size.Set(CommonScript.CharacterAndSlice_To_ScaleX[(int)character, slice], CommonScript.CharacterAndSlice_To_ScaleY[(int)character, slice]);

                        // 新・当たり判定くん
                        player_to_charAttackImgSpriteRenderer[iPlayer].transform.position = new Vector3(
                            player_to_char[iPlayer].transform.position.x +
                            (player_to_char[iPlayer].transform.localScale.x < 0.0f ? -1.0f : 1.0f) *
                            CommonScript.GRAPHIC_SCALE * AttackCollider2DScript.CharacterAndSlice_To_OffsetX[(int)character, slice],
                            player_to_char[iPlayer].transform.position.y + CommonScript.GRAPHIC_SCALE * AttackCollider2DScript.CharacterAndSlice_To_OffsetY[(int)character, slice]
                            );
                        //Debug.Log("player_to_char[iPlayer].transform.localScale.x = " + player_to_char[iPlayer].transform.localScale.x);
                        player_to_charAttackImgSpriteRenderer[iPlayer].transform.localScale = new Vector3(

                            CommonScript.GRAPHIC_SCALE * AttackCollider2DScript.CharacterAndSlice_To_ScaleX[(int)character, slice],
                            CommonScript.GRAPHIC_SCALE * AttackCollider2DScript.CharacterAndSlice_To_ScaleY[(int)character, slice]
                            );
                        //player_to_charAttackImgBoxCollider2D[iPlayer].offset.Set(CommonScript.CharacterAndSlice_To_OffsetX[(int)character, slice], CommonScript.CharacterAndSlice_To_OffsetY[(int)character, slice]);
                        //player_to_charAttackImgBoxCollider2D[iPlayer].size.Set(CommonScript.CharacterAndSlice_To_ScaleX[(int)character, slice], CommonScript.CharacterAndSlice_To_ScaleY[(int)character, slice]);
                    }
                }

                //Debug.Log("iPlayerIndex = " + iPlayer + " clip = " + clipName + " currentMotionFrame = " + currentMotionFrame + " motionFrames = " + motionFrames + " normalizedTime = " + normalizedTime + " length = " + clip.length + " frameRate = " + clip.frameRate + " sliceName = " + sliceName);
            }
        }
        #endregion

        #region 時間制限
        {
            // カウントダウン
            player_to_timeCount[(int)CommonScript.Teban] -= Time.deltaTime; // 前のフレームからの経過時間を引くぜ☆
            player_to_time[(int)CommonScript.Teban].text = ((int)player_to_timeCount[(int)CommonScript.Teban]).ToString();
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
            if (bar1_rt.sizeDelta.x <= bar0_rt.sizeDelta.x
                || player_to_timeCount[(int)PlayerIndex.Player2] < 1.0f)
            {
                // １プレイヤーの勝ち
                winner = PlayerIndex.Player1;
            }
            else if (bar0_rt.sizeDelta.x <= 0
                || player_to_timeCount[(int)PlayerIndex.Player1] < 1.0f)
            {
                // ２プレイヤーの勝ち
                winner = PlayerIndex.Player2;
            }

            if (PlayerIndex.Num != winner)
            {
                // 現在、何ラウンドか☆
                int round;
                if (!playerAndRound_to_result[(int)PlayerIndex.Player1, 0].activeSelf)
                {
                    round = 0;
                }
                else if (!playerAndRound_to_result[(int)PlayerIndex.Player1, 1].activeSelf)
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
                        playerAndRound_to_result[(int)PlayerIndex.Player1, round].SetActive(true);
                        playerAndRound_to_result[(int)PlayerIndex.Player2, round].SetActive(true);

                        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/ResultMark0");
                        playerAndRound_to_result[(int)PlayerIndex.Player1, round].GetComponent<Image>().sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("ResultMark0_0"));
                        playerAndRound_to_result[(int)PlayerIndex.Player2, round].GetComponent<Image>().sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("ResultMark0_1"));

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
                        playerAndRound_to_result[(int)PlayerIndex.Player1, round].SetActive(true);
                        playerAndRound_to_result[(int)PlayerIndex.Player2, round].SetActive(true);

                        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/ResultMark0");
                        playerAndRound_to_result[(int)PlayerIndex.Player1, round].GetComponent<Image>().sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("ResultMark0_1"));
                        playerAndRound_to_result[(int)PlayerIndex.Player2, round].GetComponent<Image>().sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("ResultMark0_0"));

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
        if (Input.GetButtonDown(CommonScript.BUTTON_03_P0_LP))
        {
            //Debug.Log("button BUTTON_03_P1_LP");
            LightPunch(PlayerIndex.Player1);
        }
        else if (Input.GetButtonDown(CommonScript.BUTTON_04_P0_MP))
        {
            //Debug.Log("button BUTTON_04_P1_MP");
            MediumPunch(PlayerIndex.Player1);
        }
        else if (Input.GetButtonDown(CommonScript.BUTTON_05_P0_HP))
        {
            //Debug.Log("button BUTTON_05_P1_HP");
            HardPunch(PlayerIndex.Player1);
        }
        else if (Input.GetButtonDown(CommonScript.BUTTON_06_P0_LK))
        {
            //Debug.Log("button BUTTON_06_P1_LK");
            LightKick(PlayerIndex.Player1);
        }
        else if (Input.GetButtonDown(CommonScript.BUTTON_07_P0_MK))
        {
            //Debug.Log("button BUTTON_07_P1_MK");
            MediumKick(PlayerIndex.Player1);
        }
        else if (Input.GetButtonDown(CommonScript.BUTTON_08_P0_HK))
        {
            //Debug.Log("button BUTTON_08_P1_HK");
            HardKick(PlayerIndex.Player1);
        }
        else if (Input.GetButtonDown(CommonScript.BUTTON_09_P0_PA))
        {
            //Debug.Log("button BUTTON_09_P1_PA");
        }
        //}
        //// ２プレイヤー
        //if (player_to_attacking[(int)PlayerIndex.Player2] < 1)
        //{
        if (Input.GetButtonDown(CommonScript.BUTTON_13_P1_LP))
        {
            //Debug.Log("button BUTTON_13_P2_LP");
            LightPunch(PlayerIndex.Player2);
        }
        else if (Input.GetButtonDown(CommonScript.BUTTON_14_P1_MP))
        {
            //Debug.Log("button BUTTON_14_P2_MP");
            MediumPunch(PlayerIndex.Player2);
        }
        else if (Input.GetButtonDown(CommonScript.BUTTON_15_P1_HP))
        {
            //Debug.Log("button BUTTON_15_P2_HP");
            HardPunch(PlayerIndex.Player2);
        }
        else if (Input.GetButtonDown(CommonScript.BUTTON_16_P1_LK))
        {
            //Debug.Log("button BUTTON_16_P2_LK");
            LightKick(PlayerIndex.Player2);
        }
        else if (Input.GetButtonDown(CommonScript.BUTTON_17_P1_MK))
        {
            //Debug.Log("button BUTTON_17_P2_MK");
            MediumKick(PlayerIndex.Player2);
        }
        else if (Input.GetButtonDown(CommonScript.BUTTON_18_P1_HK))
        {
            //Debug.Log("button BUTTON_18_P2_HK");
            HardKick(PlayerIndex.Player2);
        }
        else if (Input.GetButtonDown(CommonScript.BUTTON_19_P1_PA))
        {
            //Debug.Log("button BUTTON_19_P2_PA");
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
                player_to_rigidbody2D[iPlayerIndex].velocity = new Vector2(leverX * speed, player_to_rigidbody2D[iPlayerIndex].velocity.y);
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
                player_to_rigidbody2D[iPlayerIndex].velocity = new Vector2(0, player_to_rigidbody2D[iPlayerIndex].velocity.y);
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
}
