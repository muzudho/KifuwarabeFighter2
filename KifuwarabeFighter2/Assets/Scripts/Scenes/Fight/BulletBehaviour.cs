namespace SceneMain
{
    using Assets.Scripts.Models.Scenes.Fight;
    using Assets.Scripts.Models.Input;
    using DojinCircleGrayscale.Hitbox2DLorikeet;
    using UnityEngine;

    /// <summary>
    /// 弾の振る舞い
    /// </summary>
    public class BulletBehaviour : MonoBehaviour
    {
        #region 弾作成
        /// <summary>
        /// この弾を発射したプレイヤー☆ １プレイヤーは 0 と指定☆
        /// </summary>
        public int friend;
        int speed = 10;
        #endregion

        #region 当たり判定
        /// <summary>
        /// この弾が当たるプレイヤー☆ １プレイヤーは 0 と指定☆
        /// </summary>
        public int opponent;
        GameObject mainCamera;
        CameraBehaviour mainCameraScript;
        #endregion

        /// <summary>
        /// 生成から破棄まで
        /// </summary>
        void Start()
        {
            #region 当たり判定
            mainCamera = GameObject.Find("Main Camera");
            mainCameraScript = mainCamera.GetComponent<CameraBehaviour>();
            #endregion

            #region 弾作成
            // 味方キャラから 敵キャラに向かって弾を飛ばします
            GameObject me = GameObject.FindWithTag(ThisSceneStatus.PlayerToTag[friend]);   // 自キャラ
            Rigidbody2D bullet = GetComponent<Rigidbody2D>();                           // 弾
            bullet.velocity = new Vector2(                                              // 自キャラの向いている向きに弾を飛ばす
                speed * me.transform.localScale.x, bullet.velocity.y);
            
            // 弾の画像の向きを　味方キャラクター　に合わせる
            Vector2 temp = transform.localScale;
            temp.x = me.transform.localScale.x / Common.SCALE;
            transform.localScale = temp;

            // 5秒後に消滅
            Destroy(gameObject, 5);
            #endregion
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            // 敵キャラ　に当たったら、この弾を消すぜ☆
            if (col.gameObject.tag == ThisSceneStatus.PlayerToTag[opponent])
            {
                if (mainCameraScript != null)// なぜかヌルになっていることがあるぜ☆（＾～＾）
                {
                    var opponentKey = PlayerKeys.FromArrayIndex(opponent);

                    // 爆発の粒子を作るぜ☆
                    TakoyakiParticleScript.Add(this.transform.position.x, this.transform.position.y);

                    // ＨＰメーター
                    {
                        float damage;
                        switch (opponentKey)
                        {
                            case PlayerKey.N1: damage = -50.0f; break; // １プレイヤーにダメージの場合マイナス☆
                            case PlayerKey.N2: damage = 50.0f; break;
                            default: Debug.LogError("Bullet / HP meter / opponent"); damage = 0.0f; break;
                        }

                        // コンピューターが連射をしていると、mainCameraScript がヌルになっていることがあるようだ。
                        mainCameraScript.OffsetBar(damage);
                    }

                    // 手番
                    {
                        // 攻撃を受けた方の手番に変わるぜ☆（＾▽＾）
                        mainCameraScript.SetTeban(opponentKey);
                    }

                    // この弾を消すぜ☆
                    Destroy(gameObject);
                }
            }
        }
    }
}
