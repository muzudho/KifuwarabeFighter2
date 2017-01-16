using UnityEngine;

public class CharacterScript : MonoBehaviour {

    #region 敵味方判定
    public int playerIndex;
    PlayerIndex opponent;
    #endregion

    public bool isComputer;
    public GameObject bullet;
    #region 当たり判定
    private GameObject mainCamera;
    private string opponentAttackerTag;
    /// <summary>
    /// 攻撃を受けた回数。１０回溜まるとダウン☆
    /// </summary>
    private int damageHitCount;
    //public bool invincible;// 攻撃を受けない状態なら真。
    #endregion
    #region 効果音
    private AudioSource audioSource;
    #endregion
    #region 歩行
    float speedX = 4.0f; // 歩行速度☆
    #endregion
    #region ジャンプ
    /// <summary>
    /// 地面のレイヤー。
    /// 備考： public LayerMask にしておくと、UnityのGUI上でレイヤー選択用のドロップダウン・リストになる。
    /// </summary>
    LayerMask groundLayer;
    private bool isGrounded;
    private Rigidbody2D Rigidbody2D { get; set; }
    float speedY = 7.0f; // ジャンプ速度☆
    private Animator anim;
    /// <summary>
    /// ジャンプの屈伸モーション中なら真。
    /// </summary>
    public bool IsJump0Motion { get; set; }
    #endregion
    public MainCameraScript mainCameraScript;
    #region 勝敗判定
    public bool isResign;
    #endregion

    void Start()
    {
        #region 当たり判定
        mainCamera = GameObject.Find("Main Camera");
        mainCameraScript = mainCamera.GetComponent<MainCameraScript>();
        opponent = CommonScript.ReverseTeban((PlayerIndex)playerIndex);
        opponentAttackerTag = CommonScript.Player_To_AttackerTag[(int)opponent];
        #endregion
        #region 効果音
        audioSource = GetComponent<AudioSource>();
        #endregion
        #region ジャンプ
        groundLayer = LayerMask.GetMask("Ground");
        Rigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        #endregion
    }


    void Update()
    {
        // 入力受付
        float leverX;
        float leverY;
        bool buttonDownLP = Input.GetButtonDown(CommonScript.PlayerAndButton_To_ButtonName[playerIndex, (int)ButtonIndex.LightPunch]);
        bool buttonDownMP = Input.GetButtonDown(CommonScript.PlayerAndButton_To_ButtonName[playerIndex, (int)ButtonIndex.MediumPunch]);
        bool buttonDownHP = Input.GetButtonDown(CommonScript.PlayerAndButton_To_ButtonName[playerIndex, (int)ButtonIndex.HardPunch]);
        bool buttonDownLK = Input.GetButtonDown(CommonScript.PlayerAndButton_To_ButtonName[playerIndex, (int)ButtonIndex.LightKick]);
        bool buttonDownMK = Input.GetButtonDown(CommonScript.PlayerAndButton_To_ButtonName[playerIndex, (int)ButtonIndex.MediumKick]);
        bool buttonDownHK = Input.GetButtonDown(CommonScript.PlayerAndButton_To_ButtonName[playerIndex, (int)ButtonIndex.HardKick]);
        bool buttonDownPA = Input.GetButtonDown(CommonScript.PlayerAndButton_To_ButtonName[playerIndex, (int)ButtonIndex.Pause]);
        bool buttonUpLP = Input.GetButtonUp(CommonScript.PlayerAndButton_To_ButtonName[playerIndex, (int)ButtonIndex.LightPunch]);
        bool buttonUpMP = Input.GetButtonUp(CommonScript.PlayerAndButton_To_ButtonName[playerIndex, (int)ButtonIndex.MediumPunch]);
        bool buttonUpHP = Input.GetButtonUp(CommonScript.PlayerAndButton_To_ButtonName[playerIndex, (int)ButtonIndex.HardPunch]);
        bool buttonUpLK = Input.GetButtonUp(CommonScript.PlayerAndButton_To_ButtonName[playerIndex, (int)ButtonIndex.LightKick]);
        bool buttonUpMK = Input.GetButtonUp(CommonScript.PlayerAndButton_To_ButtonName[playerIndex, (int)ButtonIndex.MediumKick]);
        bool buttonUpHK = Input.GetButtonUp(CommonScript.PlayerAndButton_To_ButtonName[playerIndex, (int)ButtonIndex.HardKick]);
        bool buttonUpPA = Input.GetButtonUp(CommonScript.PlayerAndButton_To_ButtonName[playerIndex, (int)ButtonIndex.Pause]);
        if (isComputer)
        {
            if (buttonDownLP || buttonDownMP || buttonDownHP || buttonDownLK || buttonDownMK || buttonDownHK || buttonDownPA)
            {
                // 人間プレイヤーの乱入☆ 次のフレームから☆
                isComputer = false;
                leverX = 0;
                leverY = 0;
                anim.SetBool(CommonScript.BOOL_PUSHING_LP, false);
                anim.SetBool(CommonScript.BOOL_PUSHING_MP, false);
                anim.SetBool(CommonScript.BOOL_PUSHING_HP, false);
                anim.SetBool(CommonScript.BOOL_PUSHING_LK, false);
                anim.SetBool(CommonScript.BOOL_PUSHING_MK, false);
                anim.SetBool(CommonScript.BOOL_PUSHING_HK, false);
                anim.SetBool(CommonScript.BOOL_PUSHING_PA, false);
            }
            else
            {
                // コンピューター・プレイヤーの場合。
                leverX = Random.Range(-1.0f, 1.0f);
                leverY = Random.Range(-1.0f, 1.0f);
                if (-0.980f < leverX && leverX < 0.980f)
                {
                    // きょろきょろするので落ち着かせるぜ☆（＾～＾）
                    leverX = 0.0f;
                }

                if (-0.995f < leverY && leverY < 0.995f)
                {
                    // ジャンプばっかりするので落ち着かせるぜ☆（＾～＾）
                    leverY = 0.0f;
                }

                if (anim.GetBool(CommonScript.BOOL_PUSHING_LP))
                {
                    buttonUpLP = (0.900f < Random.Range(0.0f, 1.0f));
                }
                else
                {
                    buttonUpLP = false;
                    buttonDownLP = (0.900f < Random.Range(0.0f, 1.0f));
                }

                if (anim.GetBool(CommonScript.BOOL_PUSHING_MP))
                {
                    buttonUpMP = (0.990f < Random.Range(0.0f, 1.0f));
                }
                else
                {
                    buttonUpMP = false;
                    buttonDownMP = (0.990f < Random.Range(0.0f, 1.0f));
                }

                if (anim.GetBool(CommonScript.BOOL_PUSHING_HP))
                {
                    buttonUpHP = (0.995f < Random.Range(0.0f, 1.0f));
                }
                else
                {
                    buttonUpHP = false;
                    buttonDownHP = (0.995f < Random.Range(0.0f, 1.0f));
                }

                if (anim.GetBool(CommonScript.BOOL_PUSHING_LK))
                {
                    buttonUpLK = (0.900f < Random.Range(0.0f, 1.0f));
                }
                else
                {
                    buttonUpLK = false;
                    buttonDownLK = (0.900f < Random.Range(0.0f, 1.0f));
                }

                if (anim.GetBool(CommonScript.BOOL_PUSHING_MK))
                {
                    buttonUpMK = (0.990f < Random.Range(0.0f, 1.0f));
                }
                else
                {
                    buttonUpMK = false;
                    buttonDownMK = (0.990f < Random.Range(0.0f, 1.0f));
                }

                if (anim.GetBool(CommonScript.BOOL_PUSHING_HK))
                {
                    buttonUpHK = (0.995f < Random.Range(0.0f, 1.0f));
                }
                else
                {
                    buttonUpHK = false;
                    buttonDownHK = (0.995f < Random.Range(0.0f, 1.0f));
                }
                //buttonUpPA = (0.999f < Random.Range(0.0f, 1.0f));
                //buttonDownPA = (0.999f < Random.Range(0.0f, 1.0f));
            }
        }
        else
        {
            //左キー: -1、右キー: 1
            leverX = Input.GetAxisRaw(CommonScript.PlayerAndButton_To_ButtonName[playerIndex, (int)ButtonIndex.Horizontal]);
            // 下キー: -1、上キー: 1 (Input設定でVerticalの入力にはInvertをチェックしておく）
            leverY = Input.GetAxisRaw(CommonScript.PlayerAndButton_To_ButtonName[playerIndex, (int)ButtonIndex.Vertical]);
        }

        // 連打防止のフラグ解除
        {
            if (buttonUpLP)
            {
                anim.SetBool(CommonScript.BOOL_PUSHING_LP, false);
            }
            if (buttonUpMP)
            {
                anim.SetBool(CommonScript.BOOL_PUSHING_MP, false);
            }
            if (buttonUpHP)
            {
                anim.SetBool(CommonScript.BOOL_PUSHING_HP, false);
            }
            if (buttonUpLK)
            {
                anim.SetBool(CommonScript.BOOL_PUSHING_LK, false);
            }
            if (buttonUpMK)
            {
                anim.SetBool(CommonScript.BOOL_PUSHING_MK, false);
            }
            if (buttonUpHK)
            {
                anim.SetBool(CommonScript.BOOL_PUSHING_HK, false);
            }
        }

        #region ジャンプ
        {
            // キャラクターの下半身に、接地判定用の垂直線を引く
            // transform.up が -1 のとき、方眼紙の１マス分ぐらい下に相当？
            isGrounded = Physics2D.Linecast(
                transform.position + transform.up * 0, // スプライトの中央
                transform.position - transform.up * 1.1f, // 足元を少しはみ出すぐらい
                groundLayer // Linecastが判定するレイヤー // LayerMask.GetMask("Water")// 
                );
            //if ((int)PlayerIndex.Player1 == playerIndex)
            //{
            //    Debug.Log("B playerIndex = " + playerIndex + " isGrounded = " + isGrounded + " transform.position.y = " + transform.position.y + " Rigidbody2D.velocity.y = " + Rigidbody2D.velocity.y);
            //}
        }
        UpdateAnim();
        #endregion

        #region 弾を撃つ
        // 弾を撃つぜ☆
        if (
            (3 == anim.GetInteger(CommonScript.INTEGER_LEVER_NEUTRAL) % (30)) // レバーを放して、タイミングよく攻撃ボタンを押したとき
            &&
            (
                buttonDownLP ||
                buttonDownMP ||
                buttonDownHP ||
                buttonDownLK ||
                buttonDownMK ||
                buttonDownHK
            )
        )
        {
            float startY;

            if (0 < leverY)// 上段だぜ☆
            {
                startY = 1.2f;
            }
            else if (0 == leverY)// 中段だぜ☆
            {
                startY = 0.6f;
            }
            else // 下段だぜ☆
            {
                startY = 0.0f;
            }

            // 弾の作成☆
            GameObject newBullet = Instantiate(bullet, transform.position + new Vector3(0f, startY, 0f), transform.rotation);
            SpriteRenderer newBulletSpriteRenderer = newBullet.GetComponent<SpriteRenderer>();

            // 弾の画像を差し替えたいぜ☆（＾～＾）
            {
                int r = Random.Range(0, 14);
                Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Bullet0");
                Sprite sprite2;
                switch (r)
                {
                    case 0: sprite2 = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("Bullet0_0")); break;//歩
                    case 1: sprite2 = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("Bullet0_1")); break;//香
                    case 2: sprite2 = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("Bullet0_2")); break;//桂
                    case 3: sprite2 = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("Bullet0_3")); break;//銀
                    case 4: sprite2 = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("Bullet0_4")); break;//金
                    case 5: sprite2 = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("Bullet0_5")); break;//角
                    case 6: sprite2 = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("Bullet0_6")); break;//飛
                    case 7: sprite2 = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("Bullet0_7")); break;//玉
                    case 8: sprite2 = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("Bullet0_8")); break;//と
                    case 9: sprite2 = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("Bullet0_9")); break;//杏
                    case 10: sprite2 = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("Bullet0_10")); break;//圭
                    case 11: sprite2 = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("Bullet0_11")); break;//全
                    case 12: sprite2 = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("Bullet0_12")); break;//馬
                    default: sprite2 = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("Bullet0_13")); break;//竜
                }
                newBulletSpriteRenderer.sprite = sprite2;
            }
        }
        #endregion

        #region 歩行
        if (isGrounded)// 接地していれば
        {
            if (!this.IsJump0Motion)//ジャンプ時の屈伸中ではないなら
            {
                if (leverX != 0)//左か右を入力したら
                {
                    //Debug.Log("lever x = " + x.ToString());

                    //入力方向へ移動
                    Rigidbody2D.velocity = new Vector2(leverX * speedX, Rigidbody2D.velocity.y);

                    //localScale.xを-1にすると画像が反転する
                    Vector2 temp = transform.localScale;
                    temp.x = Mathf.Sign(leverX) * CommonScript.GRAPHIC_SCALE;
                    transform.localScale = temp;

                    if (leverX < 0)
                    {
                        if (!anim.GetBool(CommonScript.BOOL_DASHING))
                        {
                            // ダッシュ・アニメーションの開始
                            anim.SetInteger(CommonScript.INTEGER_LEVER_NEUTRAL, 0);
                            anim.SetBool(CommonScript.BOOL_ESCAPING, false);
                            anim.SetBool(CommonScript.BOOL_DASHING, true);
                            //if ((int)PlayerIndex.Player1 == playerIndex)
                            //{
                            //    Debug.Log("Rigidbody2D.velocity.x = " + Rigidbody2D.velocity.x + " ダッシュ!");
                            //}
                            anim.SetTrigger(CommonScript.TRIGGER_DASH);
                        }
                    }
                    else if (0 < leverX)
                    {
                        if (!anim.GetBool(CommonScript.BOOL_ESCAPING))
                        {
                            // エスケープ・アニメーションの開始
                            anim.SetInteger(CommonScript.INTEGER_LEVER_NEUTRAL, 0);
                            anim.SetBool(CommonScript.BOOL_DASHING, false);
                            anim.SetBool(CommonScript.BOOL_ESCAPING, true);
                            //if ((int)PlayerIndex.Player1 == playerIndex)
                            //{
                            //    Debug.Log("Rigidbody2D.velocity.x = " + Rigidbody2D.velocity.x + " エスケープ!");
                            //}
                            anim.SetTrigger(CommonScript.TRIGGER_ESCAPE);
                        }
                    }
                }
                else//左も右も入力していなかったら
                {
                    // 感覚的に、左から右に隙間なく切り替えたと思っていても、
                    // 入力装置的には、左から右（その逆も）に切り替える瞬間、どちらも押していない瞬間が発生する。
                    anim.SetInteger(CommonScript.INTEGER_LEVER_NEUTRAL, anim.GetInteger(CommonScript.INTEGER_LEVER_NEUTRAL) +1);

                    if ( 8 < anim.GetInteger(CommonScript.INTEGER_LEVER_NEUTRAL) )// レバーを放した 数フレーム目から、レバーが離れた判定をすることにする。
                    {
                        //横移動の速度を0にしてピタッと止まるようにする
                        Rigidbody2D.velocity = new Vector2(0, Rigidbody2D.velocity.y);
                        //if ((int)PlayerIndex.Player1 == playerIndex)
                        //{
                        //    Debug.Log("Rigidbody2D.velocity.x = " + Rigidbody2D.velocity.x + " ストップ!");
                        //}

                        anim.SetBool(CommonScript.BOOL_DASHING, false);
                        anim.SetBool(CommonScript.BOOL_ESCAPING, false);
                    }

                }
            }
        }
        #endregion

        #region ジャンプ
        if (isGrounded)// 接地していれば
        {
            if (!this.IsJump0Motion)//ジャンプ時の屈伸中ではないなら
            {
                //Debug.Log("leverY = "+ leverY + " player_to_rigidbody2D[" + iPlayer  + "].velocity = " + player_to_rigidbody2D[iPlayer].velocity);

                if (0 < leverY)// 上キーを入力したら
                {
                    // ジャンプするぜ☆
                    Jump0();
                }
                else if (leverY < 0)// 下キーを入力したら
                {
                }
                else // 下も上も入力していなかったら
                {
                }
            }
        }
        else // 空中なら
        {
            if (0 < leverX) // 右を入力したら
            {
                //Debug.Log("lever x = " + x.ToString());

                if (Rigidbody2D.velocity.x < 0)//左方向のベロシティーだったのなら
                {
                    // ベロシティーを少し減らして、右方向へ
                    Rigidbody2D.velocity = new Vector2(Mathf.Abs(Rigidbody2D.velocity.x * 0.8f), Rigidbody2D.velocity.y);
                }
            }
            else if (leverX < 0) // 左を入力したら
            {
                if (0 < Rigidbody2D.velocity.x)//右方向のベロシティーだったのなら
                {
                    // ベロシティーを少し減らして、左方向へ
                    Rigidbody2D.velocity = new Vector2(-Mathf.Abs(Rigidbody2D.velocity.x * 0.8f), Rigidbody2D.velocity.y);
                }
            }
            else//左も右も入力していなかったら
            {
            }
        }
        #endregion

        #region 行動
        //if (buttonDownHP && buttonDownHK)
        //{
        //    Debug.Log("投了☆！");
        //    Resign();
        //}
        //else
        if (buttonDownLP)
        {
            //Debug.Log("button BUTTON_03_P1_LP");
            LightPunch();
        }
        else if (buttonDownMP)
        {
            //Debug.Log("button BUTTON_04_P1_MP");
            MediumPunch();
        }
        else if (buttonDownHP)
        {
            //Debug.Log("button BUTTON_05_P1_HP");
            HardPunch();
        }
        else if (buttonDownLK)
        {
            //Debug.Log("button BUTTON_06_P1_LK");
            LightKick();
        }
        else if (buttonDownMK)
        {
            //Debug.Log("button BUTTON_07_P1_MK");
            MediumKick();
        }
        else if (buttonDownHK)
        {
            //Debug.Log("button BUTTON_08_P1_HK");
            HardKick();
        }
        else if (buttonDownPA)
        {
            //Debug.Log("button BUTTON_09_P1_PA");
        }
        #endregion
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        #region 当たり判定
        if (!anim.GetBool(CommonScript.BOOL_INVINCIBLE) // 攻撃が当たらない状態ではなく。
            &&
            col.tag == opponentAttackerTag)// 相手の　攻撃当たり判定くん　が重なった時
        {
            this.damageHitCount++;// 攻撃を受けた回数。

            // 効果音を鳴らすぜ☆
            audioSource.PlayOneShot(audioSource.clip);

            // 爆発の粒子を作るぜ☆
            TakoyakiParticleScript.Add(transform.position.x, transform.position.y);

            // ＨＰメーター
            {
                float damage = mainCameraScript.player_to_attackPower[(int)opponent];

                float value = damage * (playerIndex == (int)PlayerIndex.Player1 ? -1 : 1);
                mainCameraScript.OffsetBar(value);

                if (10<=damageHitCount)
                {
                    // ダメージ・アニメーションの開始
                    anim.SetTrigger(CommonScript.TRIGGER_DOWN);
                    damageHitCount = 0;
                }
                else if (100.0f <= damage)
                {
                    // ダメージ・アニメーションの開始
                    anim.SetTrigger(CommonScript.TRIGGER_DAMAGE_H);
                }
                else if (50.0f <= damage)
                {
                    // ダメージ・アニメーションの開始
                    anim.SetTrigger(CommonScript.TRIGGER_DAMAGE_M);
                }
                else
                {
                    // ダメージ・アニメーションの開始
                    anim.SetTrigger(CommonScript.TRIGGER_DAMAGE_L);
                }
            }

            // 手番
            {
                // 攻撃を受けた方の手番に変わるぜ☆（＾▽＾）
                mainCameraScript.SetTeban(opponent);
            }
        }
        #endregion
    }

    #region ジャンプ
    void Jump0()
    {
        this.IsJump0Motion = true;
        //Debug.Log("this.IsJump0Motion = true");

        //ジャンプアニメーションの開始
        anim.SetTrigger(CommonScript.TRIGGER_JUMP);
    }
    public void Jump0Exit()
    {
        this.IsJump0Motion = false;
        //Debug.Log("this.IsJump0Motion = false");
    }

    public void Jump1()
    {
        float velocityX = Rigidbody2D.velocity.x;
        float velocityY = Rigidbody2D.velocity.y;
        // 上方向へ移動
        velocityY = speedY;

        //左キー: -1、右キー: 1
        float leverX = Input.GetAxisRaw(CommonScript.PlayerAndButton_To_ButtonName[playerIndex, (int)ButtonIndex.Horizontal]);

        if (leverX != 0)//左か右を入力したら
        {
            //Debug.Log("lever x = " + x.ToString());

            //入力方向へ移動
            velocityX = speedX;

            ////localScale.xを-1にすると画像が反転する
            //Vector2 temp = transform.localScale;
            //temp.x = leverX * CommonScript.GRAPHIC_SCALE;
            //transform.localScale = temp;
        }

        Rigidbody2D.velocity = new Vector2(velocityX, velocityY);
    }

    void UpdateAnim()
    {
        //Animatorへパラメーターを送る
        anim.SetFloat("velY", Rigidbody2D.velocity.y); // y方向へかかる速度単位,上へいくとプラス、下へいくとマイナス
        anim.SetBool("isGrounded", isGrounded);
    }
    #endregion

    void LightPunch()
    {
        mainCameraScript.player_to_attackPower[playerIndex] = 10.0f;

        // アニメーションの開始
        anim.SetInteger("weight", (int)WeightIndex.Light);
        anim.SetTrigger(CommonScript.TRIGGER_PUNCH);
    }
    void MediumPunch()
    {
        mainCameraScript.player_to_attackPower[playerIndex] = 50.0f;

        // アニメーションの開始
        anim.SetInteger("weight", (int)WeightIndex.Medium);
        anim.SetTrigger(CommonScript.TRIGGER_PUNCH);
    }
    void HardPunch()
    {
        mainCameraScript.player_to_attackPower[playerIndex] = 100.0f;

        // アニメーションの開始
        anim.SetInteger("weight", (int)WeightIndex.Hard);
        anim.SetTrigger(CommonScript.TRIGGER_PUNCH);
    }
    void LightKick()
    {
        mainCameraScript.player_to_attackPower[playerIndex] = 10.0f;

        // アニメーションの開始
        anim.SetInteger("weight", (int)WeightIndex.Light);
        anim.SetTrigger(CommonScript.TRIGGER_KICK);
    }
    void MediumKick()
    {
        mainCameraScript.player_to_attackPower[playerIndex] = 50.0f;

        // アニメーションの開始
        anim.SetInteger("weight", (int)WeightIndex.Medium);
        anim.SetTrigger(CommonScript.TRIGGER_KICK);
    }
    void HardKick()
    {
        mainCameraScript.player_to_attackPower[playerIndex] = 100.0f;

        // アニメーションの開始
        anim.SetInteger("weight", (int)WeightIndex.Hard);
        anim.SetTrigger(CommonScript.TRIGGER_KICK);
    }
    /// <summary>
    /// お辞儀の開始。
    /// </summary>
    void Resign()
    {
        //Debug.Log("トリガー　投了Ａ");
        anim.SetTrigger(CommonScript.TRIGGER_GIVEUP);
    }
    /// <summary>
    /// 参りましたの発声。
    /// </summary>
    public void ResignCall()
    {
        mainCameraScript.ResignCall();
    }
}
