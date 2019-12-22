using DojinCircleGrayscale.Hitbox2DLorikeet;
using DojinCircleGrayscale.StellaQL.Acons.Main_Char3;
using UnityEngine;

namespace SceneMain
{
    public class Main_PlayerScript : MonoBehaviour
    {

        #region 敵味方判定
        public int playerIndex;
        private PlayerIndex opponent; public PlayerIndex Opponent { get { return opponent; } }
        #endregion

        public bool isComputer;
        public GameObject bullet;
        Animator animator; public Animator Animator { get { return animator; } }
        Main_CameraScript mainCameraScript; public Main_CameraScript MainCameraScript { get { return mainCameraScript; } }

        #region 当たり判定
        string opponentHitboxTag; public string OpponentHitboxTag { get { return opponentHitboxTag; } }
        /// <summary>
        /// 攻撃を受けた回数。１０回溜まるとダウン☆
        /// </summary>
        int damageHitCount; public int DamageHitCount { get { return damageHitCount; } set { damageHitCount = value; } }
        SpriteRenderer[] hitboxsSpriteRenderer;
        BoxCollider2D weakboxCollider2D;
        #endregion
        /// <summary>
        /// 歩行速度☆
        /// </summary>
        float speedX = 4.0f;
        #region ジャンプ
        /// <summary>
        /// 地面のレイヤー。
        /// 備考： public LayerMask にしておくと、UnityのGUI上でレイヤー選択用のドロップダウン・リストになる。
        /// </summary>
        LayerMask groundLayer;
        bool isGrounded;
        Rigidbody2D Rigidbody2D { get; set; }
        float speedY = 7.0f; // ジャンプ速度☆
        #endregion
        #region 勝敗判定
        bool isResign; public bool IsResign { get { return isResign; } set { isResign = value; } }
        #endregion

        void Start()
        {
            mainCameraScript = GameObject.Find("Main Camera").GetComponent<Main_CameraScript>();
            #region 当たり判定
            opponent = CommonScript.ReverseTeban((PlayerIndex)playerIndex);
            opponentHitboxTag = SceneCommon.PlayerAndHitbox_to_tag[(int)this.Opponent, (int)HitboxIndex.Hitbox];

            hitboxsSpriteRenderer = new SpriteRenderer[] {
                 GameObject.Find(SceneCommon.PlayerAndHitbox_to_path[playerIndex,(int)HitboxIndex.Hitbox]).GetComponent<SpriteRenderer>(),
                 GameObject.Find(SceneCommon.PlayerAndHitbox_to_path[playerIndex,(int)HitboxIndex.Weakbox]).GetComponent<SpriteRenderer>(),
                 GameObject.Find(SceneCommon.PlayerAndHitbox_to_path[playerIndex,(int)HitboxIndex.Strongbox]).GetComponent<SpriteRenderer>(),
            };
            weakboxCollider2D = GameObject.Find(SceneCommon.PlayerAndHitbox_to_path[playerIndex, (int)HitboxIndex.Weakbox]).GetComponent<BoxCollider2D>();
            #endregion
            #region ジャンプ
            groundLayer = LayerMask.GetMask("Ground");
            Rigidbody2D = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            #endregion

            // x位置を共有できるようにするぜ☆
            SceneCommon.Player_to_transform[playerIndex] = transform;
        }


        void Update()
        {

            //if ((int)PlayerIndex.Player1 == playerIndex)
            //{
            //    //ebug.Log("Update Time.deltaTime = " + Time.deltaTime);
            //}

            // 現在のアニメーター・ステートに紐づいたデータ
            AcState astateRecord = (AcState)AControl.Instance.GetCurrentAcStateRecord(animator);

            #region 入力受付
            CommonInput.InputStateDto input = CommonInput.OnUpdate((PlayerIndex)playerIndex);
            
            if (isComputer)
            {
                if (input.buttonDownLP || input.buttonDownMP || input.buttonDownHP || input.buttonDownLK || input.buttonDownMK || input.buttonDownHK || input.buttonDownPA)
                {
                    // 人間プレイヤーの乱入☆ 次のフレームから☆
                    isComputer = false;
                    input.leverX = 0;
                    input.leverY = 0;
                }
                else
                {
                    // コンピューター・プレイヤーの場合。
                    input.leverX = Random.Range(-1.0f, 1.0f);
                    input.leverY = Random.Range(-1.0f, 1.0f);
                    if (-0.980f < input.leverX && input.leverX < 0.980f)
                    {
                        // きょろきょろするので落ち着かせるぜ☆（＾～＾）
                        input.leverX = 0.0f;
                    }

                    if (-0.995f < input.leverY && input.leverY < 0.995f)
                    {
                        // ジャンプばっかりするので落ち着かせるぜ☆（＾～＾）
                        input.leverY = 0.0f;
                    }

                    if (input.pressingLP)
                    {
                        input.buttonUpLP = (0.900f < Random.Range(0.0f, 1.0f));
                    }
                    else
                    {
                        input.buttonUpLP = false;
                        input.buttonDownLP = (0.900f < Random.Range(0.0f, 1.0f));
                    }

                    if (input.pressingMP)
                    {
                        input.buttonUpMP = (0.990f < Random.Range(0.0f, 1.0f));
                    }
                    else
                    {
                        input.buttonUpMP = false;
                        input.buttonDownMP = (0.990f < Random.Range(0.0f, 1.0f));
                    }

                    if (input.pressingHP)
                    {
                        input.buttonUpHP = (0.995f < Random.Range(0.0f, 1.0f));
                    }
                    else
                    {
                        input.buttonUpHP = false;
                        input.buttonDownHP = (0.995f < Random.Range(0.0f, 1.0f));
                    }

                    if (input.pressingLK)
                    {
                        input.buttonUpLK = (0.900f < Random.Range(0.0f, 1.0f));
                    }
                    else
                    {
                        input.buttonUpLK = false;
                        input.buttonDownLK = (0.900f < Random.Range(0.0f, 1.0f));
                    }

                    if (input.pressingMK)
                    {
                        input.buttonUpMK = (0.990f < Random.Range(0.0f, 1.0f));
                    }
                    else
                    {
                        input.buttonUpMK = false;
                        input.buttonDownMK = (0.990f < Random.Range(0.0f, 1.0f));
                    }

                    if (input.pressingHK)
                    {
                        input.buttonUpHK = (0.995f < Random.Range(0.0f, 1.0f));
                    }
                    else
                    {
                        input.buttonUpHK = false;
                        input.buttonDownHK = (0.995f < Random.Range(0.0f, 1.0f));
                    }
                    //buttonUpPA = (0.999f < Random.Range(0.0f, 1.0f));
                    //buttonDownPA = (0.999f < Random.Range(0.0f, 1.0f));
                }
            }
            #endregion

            FacingOpponentMoveFwBkSt facingOpponentMoveFwBkSt = GetFacingOpponentMoveFwBkSt(input.leverX);

            if (astateRecord.Tags.Contains((int)Animator.StringToHash(AControl.TAG_BLOCK)))
            {
                // ブロック中
                if(FacingOpponentMoveFwBkSt.Back != facingOpponentMoveFwBkSt)
                {
                    // バックを解除している場合。
                    animator.SetTrigger(SceneCommon.TRIGGER_DEBLOCK);
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
                //    //ebug.Log("B playerIndex = " + playerIndex + " isGrounded = " + isGrounded + " transform.position.y = " + transform.position.y + " Rigidbody2D.velocity.y = " + Rigidbody2D.velocity.y);
                //}

                //Animatorへパラメーターを送る
                animator.SetFloat(SceneCommon.FLOAT_VEL_Y, Rigidbody2D.velocity.y); // y方向へかかる速度単位,上へいくとプラス、下へいくとマイナス
                //ebug.Log("Jumping velY="+animator.GetFloat(SceneCommon.FLOAT_VEL_Y));
                animator.SetBool(SceneCommon.BOOL_IS_GROUNDED, isGrounded);
            }
            #endregion

            #region 弾を撃つ
            // 弾を撃つぜ☆
            if (
                (3 == animator.GetInteger(SceneCommon.INTEGER_LEVER_X_NEUTRAL) % (30)) // レバーを放して、タイミングよく攻撃ボタンを押したとき
                &&
                (
                    input.buttonDownLP ||
                    input.buttonDownMP ||
                    input.buttonDownHP ||
                    input.buttonDownLK ||
                    input.buttonDownMK ||
                    input.buttonDownHK
                )
            )
            {
                float startY;

                if (0 < input.leverY)// 上段だぜ☆
                {
                    startY = 1.2f;
                }
                else if (0 == input.leverY)// 中段だぜ☆
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

            #region レバーの押下時間の更新
            // レバー・ニュートラル時間と、レバー・プレッシング時間は、8フレームほど重複する部分がある。
            if (input.leverX != 0)//左か右を入力したら
            {
                animator.SetInteger(SceneCommon.INTEGER_LEVER_X_PRESSING, animator.GetInteger(SceneCommon.INTEGER_LEVER_X_PRESSING) + 1);
                animator.SetInteger(SceneCommon.INTEGER_LEVER_X_NEUTRAL, 0);
                animator.SetInteger(SceneCommon.INTEGER_LEVER_X_IDOL, 0);
            }
            else //左も右も入力していなかったら
            {
                // 感覚的に、左から右に隙間なく切り替えたと思っていても、
                // 入力装置的には、左から右（その逆も）に切り替える瞬間、どちらも押していない瞬間が発生する。
                if (8 < animator.GetInteger(SceneCommon.INTEGER_LEVER_X_IDOL))// レバーを放した 数フレーム目から、レバーが離れた判定をすることにする。
                {
                    animator.SetInteger(SceneCommon.INTEGER_LEVER_X_PRESSING, 0);
                    animator.SetInteger(SceneCommon.INTEGER_LEVER_X_NEUTRAL, animator.GetInteger(SceneCommon.INTEGER_LEVER_X_NEUTRAL) + 1);
                }
                else
                {
                    animator.SetInteger(SceneCommon.INTEGER_LEVER_X_IDOL, animator.GetInteger(SceneCommon.INTEGER_LEVER_X_IDOL) + 1);
                }
            }

            if (0 != input.leverY)// 上か下キーを入力していたら
            {
                animator.SetInteger(SceneCommon.INTEGER_LEVER_Y_PRESSING, animator.GetInteger(SceneCommon.INTEGER_LEVER_Y_PRESSING) + 1);
                animator.SetInteger(SceneCommon.INTEGER_LEVER_Y_NEUTRAL, 0);
                animator.SetInteger(SceneCommon.INTEGER_LEVER_Y_IDOL, 0);
            }
            else // 下も上も入力していなかったら
            {
                // 感覚的に、左から右に隙間なく切り替えたと思っていても、
                // 入力装置的には、下から上（その逆も）に切り替える瞬間、どちらも押していない瞬間が発生する。
                if (8 < animator.GetInteger(SceneCommon.INTEGER_LEVER_Y_IDOL))// レバーを放した 数フレーム目から、レバーが離れた判定をすることにする。
                {
                    animator.SetInteger(SceneCommon.INTEGER_LEVER_Y_PRESSING, 0);
                    animator.SetInteger(SceneCommon.INTEGER_LEVER_Y_NEUTRAL, animator.GetInteger(SceneCommon.INTEGER_LEVER_Y_NEUTRAL) + 1);
                }
                else
                {
                    animator.SetInteger(SceneCommon.INTEGER_LEVER_Y_IDOL, animator.GetInteger(SceneCommon.INTEGER_LEVER_Y_IDOL) + 1);
                }
            }
            #endregion

            #region レバー操作によるアクション
            //if (!anim.GetBool(CommonScript.BOOL_JMOVE0))//ジャンプ時の屈伸中ではないなら
            //{
            if (input.leverX != 0)//左か右を入力したら
            {
                if (!astateRecord.Tags.Contains(Animator.StringToHash(AControl.TAG_BUSYX)))
                {
                    //入力方向へ移動
                    Rigidbody2D.velocity = new Vector2(Mathf.Sign(input.leverX) * speedX, Rigidbody2D.velocity.y);
                }

                DoFacingOpponent(GetFacingOfOpponentLR());
                //ebug.Log("さあ、どっちだ☆ input.leverX = " + input.leverX + " facingOpponentMoveFwBkSt = " + facingOpponentMoveFwBkSt + " Time.deltaTime = " + Time.deltaTime);

                if (FacingOpponentMoveFwBkSt.Forward == facingOpponentMoveFwBkSt)// 相手に向かってレバーを倒したとき
                {
                    //if ((int)PlayerIndex.Player1 == playerIndex)
                    //{
                    //    //ebug.Log("相手に向かっていくぜ☆ input.leverX = " + input.leverX);
                    //}

                    Pull_Forward();
                    //if (isGrounded)// 接地していれば
                    //{
                    //}
                    //if ((int)ActioningIndex.Dash != anim.GetInteger(CommonScript.INTEGER_ACTIONING))
                    //{
                    //    // ダッシュ・アニメーションの開始
                    //    //if ((int)PlayerIndex.Player1 == playerIndex)
                    //    //{
                    //    //    //ebug.Log("Rigidbody2D.velocity.x = " + Rigidbody2D.velocity.x + " ダッシュ!");
                    //    //}
                    //}
                    //else
                    //{
                    //    // 既にダッシュ中なら何もしない
                    //}
                }
                else if (FacingOpponentMoveFwBkSt.Back == facingOpponentMoveFwBkSt)// 相手と反対の方向にレバーを倒したとき（バックステップ）
                {
                    //if ((int)PlayerIndex.Player1 == playerIndex)
                    //{
                    //    //ebug.Log("相手の反対側に向かっていくぜ☆ input.leverX = " + input.leverX);
                    //}

                    Pull_Back();
                }
                else // レバーを倒していない時（ここにはこない？）
                {
                    //if ((int)PlayerIndex.Player1 == playerIndex)
                    //{
                    //    //ebug.Log("止まっているぜ☆ input.leverX = " + input.leverX);
                    //}
                }
            }
            else// 左も右も入力していなかったら
            {

                if (isGrounded)// 接地していれば
                {
                    // 横移動の速度を0にしてピタッと止まるようにする
                    Rigidbody2D.velocity = new Vector2(0, Rigidbody2D.velocity.y);
                }


                // 感覚的に、左から右に隙間なく切り替えたと思っていても、
                // 入力装置的には、左から右（その逆も）に切り替える瞬間、どちらも押していない瞬間が発生する。
                if (8 < animator.GetInteger(SceneCommon.INTEGER_LEVER_X_NEUTRAL))// レバーを放した 数フレーム目から、レバーが離れた判定をすることにする。
                {
                    //if ((int)PlayerIndex.Player1 == playerIndex)
                    //{
                    //    //ebug.Log("Rigidbody2D.velocity.x = " + Rigidbody2D.velocity.x + " ストップ!");
                    //}

                    if (isGrounded)// 接地していれば
                    {
                        animator.SetInteger(SceneCommon.INTEGER_ACTIONING, (int)TilesetfileType.Stand);
                    }
                }
            }

            //ebug.Log("leverY = "+ leverY + " player_to_rigidbody2D[" + iPlayer  + "].velocity = " + player_to_rigidbody2D[iPlayer].velocity);

            if (0 != input.leverY)// 上か下キーを入力していたら
            {
                if (!astateRecord.Tags.Contains(Animator.StringToHash(AControl.TAG_BUSYY)))
                {
                    if (0 < input.leverY)// 上キーを入力したら
                    {
                        if (isGrounded)// 接地していれば
                        {
                            // ジャンプするぜ☆
                            Pull_Jump();
                        }
                    }
                    else if (input.leverY < 0)// 下キーを入力したら
                    {
                        if (isGrounded)// 接地していれば
                        {
                            // 屈むぜ☆
                            Pull_Crouch();
                        }
                    }
                }
            }
            else // 下も上も入力していなかったら
            {
            }
            //}
            #endregion

            #region 行動
            //if (buttonDownHP && buttonDownHK)
            //{
            //    //ebug.Log("投了☆！");
            //    Resign();
            //}
            //else
            if (input.buttonDownLP)
            {
                //ebug.Log("button BUTTON_03_P1_LP");
                Pull_LightPunch();
            }
            else if (input.buttonDownMP)
            {
                //ebug.Log("button BUTTON_04_P1_MP");
                Pull_MediumPunch();
            }
            else if (input.buttonDownHP)
            {
                //ebug.Log("button BUTTON_05_P1_HP");
                Pull_HardPunch();
            }
            else if (input.buttonDownLK)
            {
                //ebug.Log("button BUTTON_06_P1_LK");
                Pull_LightKick();
            }
            else if (input.buttonDownMK)
            {
                //ebug.Log("button BUTTON_07_P1_MK");
                Pull_MediumKick();
            }
            else if (input.buttonDownHK)
            {
                //ebug.Log("button BUTTON_08_P1_HK");
                Pull_HardKick();
            }
            else if (input.buttonDownPA)
            {
                //ebug.Log("button BUTTON_09_P1_PA");
            }
            #endregion

            if (SceneCommon.READY_TIME_LENGTH < mainCameraScript.ReadyingTime)
            {
                // 当たり判定くん
                Motor.Instance.Update(animator, AControl.Instance, playerIndex, transform, hitboxsSpriteRenderer, weakboxCollider2D);
            }
        }

        ///// <summary>
        ///// 現在のアニメーション・クリップに対応したデータを取得。
        ///// </summary>
        ///// <returns></returns>
        //public CliptypeExRecordable GetCurrentCliptypeExRecord(CliptypeExTable cliptypeExTable)
        //{
        //    AnimatorStateInfo animeStateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //    CliptypeIndex aclipType = (CliptypeIndex)((StateExRecord)StateExTable.Instance.index_to_exRecord[StateExTable.Instance.hash_to_index[animeStateInfo.fullPathHash]]).Cliptype;

        //    if (cliptypeExTable.index_to_exRecord.ContainsKey((int)aclipType))
        //    {
        //        return cliptypeExTable.index_to_exRecord[(int)aclipType];
        //    }

        //    throw new UnityException("aclipType = [" + aclipType + "]に対応するアニメーション・クリップのレコードが無いぜ☆");
        //}


        /// <summary>
        /// 相手に向かって進んでいるか、相手から離れているか、こっちからは動いていないかを判定する。
        /// </summary>
        /// <param name="leverX"></param>
        /// <returns></returns>
        public FacingOpponentMoveFwBkSt GetFacingOpponentMoveFwBkSt(float leverX)
        {
            if (0.0f==leverX)
            {
                return FacingOpponentMoveFwBkSt.Stay;
            }
            if (Mathf.Sign(SceneCommon.Player_to_transform[(int)CommonScript.ReverseTeban((PlayerIndex)playerIndex)].position.x - transform.position.x)
                ==
                Mathf.Sign(leverX)
                )
            {
                return FacingOpponentMoveFwBkSt.Forward;
            }
            return FacingOpponentMoveFwBkSt.Back;
        }
        FacingOpponentLR GetFacingOfOpponentLR()
        {
            // 自分と相手の位置（相手が右側にいるとき正となるようにする）
            if( 0<=SceneCommon.Player_to_transform[(int)CommonScript.ReverseTeban((PlayerIndex)playerIndex)].position.x - transform.position.x)
            {
                return FacingOpponentLR.Right;
            }
            return FacingOpponentLR.Left;
        }
        void DoFacingOpponent(FacingOpponentLR facingOpponentLR)
        {
            //localScale.xを-1にすると画像が反転する
            Vector2 temp = transform.localScale;
            switch (facingOpponentLR)
            {
                case FacingOpponentLR.Left:
                    temp.x = -1 * Common.SCALE;
                    //if ((int)PlayerIndex.Player1 == playerIndex)
                    //{
                    //    //ebug.Log("左を向くぜ☆");
                    //}
                    break;
                case FacingOpponentLR.Right:
                    temp.x = 1 * Common.SCALE;
                    //if ((int)PlayerIndex.Player1 == playerIndex)
                    //{
                    //    //ebug.Log("右を向くぜ☆");
                    //}
                    break;
                default:
                    break;
            }
            transform.localScale = temp;
        }

        #region ジャンプ
        public void JMove0Exit()
        {
            //ebug.Log("JMove0Exit");
            animator.SetBool(SceneCommon.BOOL_JMOVE0, false);
        }

        public void Jump1()
        {
            //ebug.Log("Jump1");
            float velocityX = Rigidbody2D.velocity.x;

            //左キー: -1、右キー: 1
            float leverX = Input.GetAxisRaw(CommonInput.PlayerAndInput_to_inputName[playerIndex, (int)InputIndex.Horizontal]);

            if (leverX != 0)//左か右を入力したら
            {
                //ebug.Log("lever x = " + x.ToString());

                //入力方向へ移動
                velocityX = speedX;
            }

            Rigidbody2D.velocity = new Vector2(velocityX, speedY);// 上方向へ移動
        }
        #endregion

        #region トリガーを引く
        public void Pull_DamageH()
        {
            animator.SetTrigger(SceneCommon.TRIGGER_DAMAGE_H);
        }
        public void Pull_DamageM()
        {
            animator.SetTrigger(SceneCommon.TRIGGER_DAMAGE_M);
        }
        public void Pull_DamageL()
        {
            animator.SetTrigger(SceneCommon.TRIGGER_DAMAGE_L);
        }
        public void Pull_Down()
        {
            damageHitCount = 0;
            animator.SetTrigger(SceneCommon.TRIGGER_DOWN);
        }
        void Pull_Forward()
        {
            animator.SetTrigger(SceneCommon.TRIGGER_MOVE_X);

            animator.ResetTrigger(SceneCommon.TRIGGER_MOVE_X_BACK);
            animator.SetTrigger(SceneCommon.TRIGGER_MOVE_X_FORWARD);
        }
        void Pull_Back()
        {
            animator.SetTrigger(SceneCommon.TRIGGER_MOVE_X);

            animator.ResetTrigger(SceneCommon.TRIGGER_MOVE_X_FORWARD);
            animator.SetTrigger(SceneCommon.TRIGGER_MOVE_X_BACK);
        }
        void Pull_Jump()
        {
            //ジャンプアニメーションの開始
            animator.SetTrigger(SceneCommon.TRIGGER_JUMP);
            //ebug.Log("JUMP trigger!");
        }
        void Pull_Crouch()
        {
            // 屈みアニメーションの開始
            animator.SetTrigger(SceneCommon.TRIGGER_CROUCH);
        }
        void Pull_LightPunch()
        {
            mainCameraScript.Player_to_attackPower[playerIndex] = 10.0f;

            // アニメーションの開始
            animator.SetTrigger(SceneCommon.TRIGGER_ATK_LP);
        }
        void Pull_MediumPunch()
        {
            mainCameraScript.Player_to_attackPower[playerIndex] = 50.0f;

            // アニメーションの開始
            animator.SetTrigger(SceneCommon.TRIGGER_ATK_MP);
        }
        void Pull_HardPunch()
        {
            mainCameraScript.Player_to_attackPower[playerIndex] = 100.0f;

            // アニメーションの開始
            animator.SetTrigger(SceneCommon.TRIGGER_ATK_HP);
        }
        void Pull_LightKick()
        {
            mainCameraScript.Player_to_attackPower[playerIndex] = 10.0f;

            // アニメーションの開始
            animator.SetTrigger(SceneCommon.TRIGGER_ATK_LK);
        }
        void Pull_MediumKick()
        {
            mainCameraScript.Player_to_attackPower[playerIndex] = 50.0f;

            // アニメーションの開始
            animator.SetTrigger(SceneCommon.TRIGGER_ATK_MK);
        }
        void Pull_HardKick()
        {
            mainCameraScript.Player_to_attackPower[playerIndex] = 100.0f;

            // アニメーションの開始
            animator.SetTrigger(SceneCommon.TRIGGER_ATK_HK);
        }
        /// <summary>
        /// お辞儀の開始。
        /// </summary>
        void Pull_Resign()
        {
            //ebug.Log("トリガー　投了Ａ");
            animator.SetTrigger(SceneCommon.TRIGGER_GIVEUP);
        }
        /// <summary>
        /// お辞儀の開始。
        /// </summary>
        public void Pull_ResignByLose()
        {
            //ebug.Log("トリガー　投了Ｘ");
            animator.SetTrigger(SceneCommon.TRIGGER_GIVEUP);
        }
        #endregion

        /// <summary>
        /// 参りましたの発声。
        /// </summary>
        public void ResignCall()
        {
            mainCameraScript.ResignCall();
        }
    }
}

