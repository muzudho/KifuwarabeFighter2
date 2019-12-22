namespace SceneMain
{
    using UnityEngine;
    using Assets.Scripts.SceneMain;

    public class WeakboxScript : MonoBehaviour
    {

        public int playerIndex;
        PlayerScript playerScript;
        #region 効果音
        AudioSource[] audioSources;
        #endregion

        // Use this for initialization
        void Start()
        {
            playerScript = GameObject.Find(ThisSceneConst.PlayerToTag[playerIndex]).GetComponent<PlayerScript>();
            #region 効果音
            audioSources = GetComponents<AudioSource>();
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
            if (col.tag == playerScript.OpponentHitboxTag)// 相手の　攻撃当たり判定くん　が重なった時
            {

                // ブロックしているかどうか判定☆
                //CommonInput.PlayerInput input = CommonInput.player_to_input[playerIndex];
                CommonInput.InputStateDto input = CommonInput.OnUpdate((PlayerIndex)playerIndex);
                if (FacingOpponentMoveFwBkSt.Back == playerScript.GetFacingOpponentMoveFwBkSt(input.leverX))
                {
                    //if ((int)PlayerIndex.Player1==playerIndex)
                    //{
                    //    Debug.Log("ブロック☆！ col.tag = " + col.tag + " input.leverX = " + input.leverX + " Time.deltaTime = " + Time.deltaTime);
                    //}
                    audioSources[1].PlayOneShot(audioSources[1].clip);// 効果音を鳴らすぜ☆
                    playerScript.Animator.SetTrigger(ThisSceneConst.TriggerBlock);
                }
                else
                {
                    //if ((int)PlayerIndex.Player1 == playerIndex)
                    //{
                    //    Debug.Log("痛っ☆！ col.tag = " + col.tag + " input.leverX = " + input.leverX + " Time.deltaTime = " + Time.deltaTime);
                    //}
                    playerScript.DamageHitCount++;// 攻撃を受けた回数。

                    audioSources[0].PlayOneShot(audioSources[0].clip);// 効果音を鳴らすぜ☆

                    // 爆発の粒子を作るぜ☆
                    TakoyakiParticleScript.Add(transform.position.x, transform.position.y);

                    // ＨＰメーター
                    {
                        float damage = playerScript.MainCameraScript.Player_to_attackPower[(int)playerScript.Opponent];

                        float value = damage * (playerIndex == (int)PlayerIndex.Player1 ? -1 : 1);
                        playerScript.MainCameraScript.OffsetBar(value);

                        if (10 <= playerScript.DamageHitCount)
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
                        playerScript.MainCameraScript.SetTeban(playerScript.Opponent);
                    }
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
