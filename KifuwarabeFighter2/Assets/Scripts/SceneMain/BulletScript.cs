namespace SceneMain
{
    using UnityEngine;
    using DojinCircleGrayscale.Hitbox2DLorikeet;
    using Assets.Scripts.SceneMain;

    public class BulletScript : MonoBehaviour
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
        CameraScript mainCameraScript;
        #endregion

        void Start()
        {
            #region 当たり判定
            mainCamera = GameObject.Find("Main Camera");
            mainCameraScript = mainCamera.GetComponent<CameraScript>();
            #endregion
            #region 弾作成
            // 味方キャラクター　のオブジェクトを取得
            GameObject friendChar = GameObject.FindWithTag(ThisSceneConst.PlayerToTag[friend]);
            // 弾のrigidbody2Dコンポーネントを取得
            Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
            // 味方キャラクター　の向いている向きに弾を飛ばす
            rigidbody2D.velocity = new Vector2(speed * friendChar.transform.localScale.x, rigidbody2D.velocity.y);
            // 弾の画像の向きを　味方キャラクター　に合わせる
            Vector2 temp = transform.localScale;
            temp.x = friendChar.transform.localScale.x / Common.SCALE;
            transform.localScale = temp;
            // 5秒後に消滅
            Destroy(gameObject, 5);
            #endregion
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            // 相手キャラクター　に当たったら、この弾を消すぜ☆
            if (col.gameObject.tag == ThisSceneConst.PlayerToTag[opponent])
            {
                if (mainCameraScript != null)// なぜかヌルになっていることがあるぜ☆（＾～＾）
                {
                    // 爆発の粒子を作るぜ☆
                    TakoyakiParticleScript.Add(this.transform.position.x, this.transform.position.y);

                    // ＨＰメーター
                    {
                        float damage;
                        switch ((PlayerIndex)opponent)
                        {
                            case PlayerIndex.Player1: damage = -50.0f; break; // １プレイヤーにダメージの場合マイナス☆
                            case PlayerIndex.Player2: damage = 50.0f; break;
                            default: Debug.LogError("Bullet / HP meter / opponent"); damage = 0.0f; break;
                        }

                        // コンピューターが連射をしていると、mainCameraScript がヌルになっていることがあるようだ。
                        mainCameraScript.OffsetBar(damage);
                    }

                    // 手番
                    {
                        // 攻撃を受けた方の手番に変わるぜ☆（＾▽＾）
                        mainCameraScript.SetTeban((PlayerIndex)opponent);
                    }

                    // この弾を消すぜ☆
                    Destroy(gameObject);
                }
            }
        }
    }
}
