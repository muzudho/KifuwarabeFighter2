using UnityEngine;

public class CharacterScript : MonoBehaviour {

    public int playerIndex;
    public GameObject bullet;
    #region 当たり判定
    private GameObject mainCamera;
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
    public bool IsJump1Motion { get; set; }
    #endregion

    void Start()
    {
        #region 当たり判定
        mainCamera = GameObject.Find("Main Camera");
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
            (playerIndex == (int)PlayerIndex.Player1 &&(
                Input.GetButton(CommonScript.BUTTON_03_P0_LP) ||
                Input.GetButton(CommonScript.BUTTON_04_P0_MP) ||
                Input.GetButton(CommonScript.BUTTON_05_P0_HP) ||
                Input.GetButton(CommonScript.BUTTON_06_P0_LK) ||
                Input.GetButton(CommonScript.BUTTON_07_P0_MK) ||
                Input.GetButton(CommonScript.BUTTON_08_P0_HK)
            ))
            ||
            (playerIndex == (int)PlayerIndex.Player2 && (
                Input.GetButton(CommonScript.BUTTON_13_P1_LP) ||
                Input.GetButton(CommonScript.BUTTON_14_P1_MP) ||
                Input.GetButton(CommonScript.BUTTON_15_P1_HP) ||
                Input.GetButton(CommonScript.BUTTON_16_P1_LK) ||
                Input.GetButton(CommonScript.BUTTON_17_P1_MK) ||
                Input.GetButton(CommonScript.BUTTON_18_P1_HK)
            ))
        )
        {
            // 下キー: -1、上キー: 1
            float leverY = Input.GetAxisRaw(CommonScript.BUTTON_02_P0_VE);
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
    }

    void FixedUpdate()
    {
        #region 歩行
        if (isGrounded)// 接地していれば
        {
            if (!this.IsJump1Motion)//ジャンプ時の屈伸中ではないなら
            {
                //左キー: -1、右キー: 1
                float leverX = Input.GetAxisRaw(CommonScript.PlayerAndButton_To_ButtonName[playerIndex, (int)ButtonIndex.Horizontal]);

                if (leverX != 0)//左か右を入力したら
                {
                    //Debug.Log("lever x = " + x.ToString());

                    //入力方向へ移動
                    Rigidbody2D.velocity = new Vector2(leverX * speedX, Rigidbody2D.velocity.y);
                    //localScale.xを-1にすると画像が反転する
                    Vector2 temp = transform.localScale;
                    temp.x = leverX * CommonScript.GRAPHIC_SCALE;
                    transform.localScale = temp;

                    if (leverX < 0)
                    {
                        if (!anim.GetBool("dashing"))
                        {
                            // ダッシュ・アニメーションの開始
                            anim.SetInteger("leverNeutral", 0);
                            anim.SetBool("escaping", false);
                            anim.SetBool("dashing", true);
                            //if ((int)PlayerIndex.Player1 == playerIndex)
                            //{
                            //    Debug.Log("Rigidbody2D.velocity.x = " + Rigidbody2D.velocity.x + " ダッシュ!");
                            //}
                            anim.SetTrigger("dash");
                        }
                    }
                    else if (0 < leverX)
                    {
                        if (!anim.GetBool("escaping"))
                        {
                            // エスケープ・アニメーションの開始
                            anim.SetInteger("leverNeutral", 0);
                            anim.SetBool("dashing", false);
                            anim.SetBool("escaping", true);
                            //if ((int)PlayerIndex.Player1 == playerIndex)
                            //{
                            //    Debug.Log("Rigidbody2D.velocity.x = " + Rigidbody2D.velocity.x + " エスケープ!");
                            //}
                            anim.SetTrigger("escape");
                        }
                    }
                }
                else//左も右も入力していなかったら
                {
                    // 感覚的に、左から右に隙間なく切り替えたと思っていても、
                    // 入力装置的には、左から右（その逆も）に切り替える瞬間、どちらも押していない瞬間が発生する。
                    anim.SetInteger("leverNeutral", anim.GetInteger("leverNeutral")+1);

                    if ( 8 < anim.GetInteger("leverNeutral") )// レバーを放した 数フレーム目から、レバーが離れた判定をすることにする。
                    {
                        //横移動の速度を0にしてピタッと止まるようにする
                        Rigidbody2D.velocity = new Vector2(0, Rigidbody2D.velocity.y);
                        if ((int)PlayerIndex.Player1 == playerIndex)
                        {
                            Debug.Log("Rigidbody2D.velocity.x = " + Rigidbody2D.velocity.x + " ストップ!");
                        }

                        anim.SetBool("dashing", false);
                        anim.SetBool("escaping", false);
                    }

                }
            }
        }
        #endregion

        #region ジャンプ
        if (isGrounded)// 接地していれば
        {
            if (!this.IsJump1Motion)//ジャンプ時の屈伸中ではないなら
            {
                // 下キー: -1、上キー: 1 (Input設定でVerticalの入力にはInvertをチェックしておく）
                float leverY = Input.GetAxisRaw(CommonScript.PlayerAndButton_To_ButtonName[playerIndex, (int)ButtonIndex.Vertical]);
                //Debug.Log("leverY = "+ leverY + " player_to_rigidbody2D[" + iPlayer  + "].velocity = " + player_to_rigidbody2D[iPlayer].velocity);

                if (0 < leverY)// 上キーを入力したら
                {
                    // ジャンプするぜ☆
                    Jump1();
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
            //左キー: -1、右キー: 1
            float leverX = Input.GetAxisRaw(CommonScript.PlayerAndButton_To_ButtonName[playerIndex, (int)ButtonIndex.Horizontal]);

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
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        #region 当たり判定
        if (
            (playerIndex == (int)PlayerIndex.Player1 && col.tag == CommonScript.Player_To_AttackerTag[(int)PlayerIndex.Player2])
            ||
            (playerIndex == (int)PlayerIndex.Player2 && col.tag == CommonScript.Player_To_AttackerTag[(int)PlayerIndex.Player1])
            )
        {
            // 攻撃の当たり判定に体が入ったとき。
            PlayerIndex opponent = CommonScript.ReverseTeban((PlayerIndex)playerIndex);

            // 効果音を鳴らすぜ☆
            audioSource.PlayOneShot(audioSource.clip);

            // 爆発の粒子を作るぜ☆
            TakoyakiParticleScript.Add(transform.position.x, transform.position.y);

            MainCameraScript script = mainCamera.GetComponent<MainCameraScript>();

            // ＨＰメーター
            {
                float damage;
                switch (opponent)
                {
                    case PlayerIndex.Player1: damage = -50.0f; break; // １プレイヤーにダメージの場合マイナス☆
                    case PlayerIndex.Player2: damage = 50.0f; break;
                    default: Debug.LogError("Bullet / HP meter / opponent"); damage = 0.0f; break;
                }
                script.OffsetBar(damage);
            }

            // 手番
            {
                // 攻撃を受けた方の手番に変わるぜ☆（＾▽＾）
                script.SetTeban(opponent);
            }
        }
        #endregion
    }

    #region ジャンプ
    void Jump1()
    {
        this.IsJump1Motion = true;
        Debug.Log("this.IsJump1Motion = true");

        //ジャンプアニメーションの開始
        anim.SetTrigger("jump");
    }
    public void Jump1Exit()
    {
        this.IsJump1Motion = false;
        Debug.Log("this.IsJump1Motion = false");
    }

    public void Jump2()
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
}
