using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneMain
{
    public class Main_WeakboxScript : MonoBehaviour
    {

        public int playerIndex;
        private Main_PlayerScript playerScript;
        #region 効果音
        private AudioSource audioSource;
        #endregion

        // Use this for initialization
        void Start()
        {
            playerScript = GameObject.Find(SceneCommon.Player_To_Tag[playerIndex]).GetComponent<Main_PlayerScript>();
            #region 効果音
            audioSource = GetComponent<AudioSource>();
            #endregion
        }

        //void OnCollisionEnter2D(Collider2D col)
        //{
        //    Debug.Log("物体だ☆！ col.tag = " + col.tag);
        //}
        /// <summary>
        /// BoxCollider2D（等のPhysics2D）, Rigidbody2D　の２つをアタッチしているものと接触したとき。
        /// </summary>
        /// <param name="col"></param>
        void OnTriggerEnter2D(Collider2D col)
        {
            #region 当たり判定
            if (col.tag == playerScript.opponentHitboxTag)// 相手の　攻撃当たり判定くん　が重なった時
            {
                Debug.Log("当たった☆！ col.tag = " + col.tag);

                playerScript.damageHitCount++;// 攻撃を受けた回数。

                // 効果音を鳴らすぜ☆
                audioSource.PlayOneShot(audioSource.clip);

                // 爆発の粒子を作るぜ☆
                TakoyakiParticleScript.Add(transform.position.x, transform.position.y);

                // ＨＰメーター
                {
                    float damage = playerScript.mainCameraScript.player_to_attackPower[(int)playerScript.opponent];

                    float value = damage * (playerIndex == (int)PlayerIndex.Player1 ? -1 : 1);
                    playerScript.mainCameraScript.OffsetBar(value);

                    if (10 <= playerScript.damageHitCount)
                    {
                        // ダウン・アニメーションの開始
                        playerScript.Pull_Down();
                    }
                    else if (100.0f <= damage)
                    {
                        // ダメージ・アニメーションの開始
                        playerScript.Pull_DamageH();
                    }
                    else if (50.0f <= damage)
                    {
                        // ダメージ・アニメーションの開始
                        playerScript.Pull_DamageM();
                    }
                    else
                    {
                        // ダメージ・アニメーションの開始
                        playerScript.Pull_DamageL();
                    }
                }

                // 手番
                {
                    // 攻撃を受けた方の手番に変わるぜ☆（＾▽＾）
                    playerScript.mainCameraScript.SetTeban(playerScript.opponent);
                }
            }
            else
            {
                //Debug.Log("相手の攻撃じゃないものに当たった☆！ col.tag = " + col.tag);
            }
            #endregion
        }
    }
}
