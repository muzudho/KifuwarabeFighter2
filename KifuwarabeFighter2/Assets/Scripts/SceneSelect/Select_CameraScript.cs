using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

namespace SceneSelect
{
    public class Select_CameraScript : MonoBehaviour
    {

        int transitionTime;
        #region カーソル移動
        public Text player0;
        public Text player1;
        public Image face0;
        public Image face1;
        public Text name0;
        public Text name1;
        private Text[] player_to_cursor;// [player]
        private Image[] player_to_face;// [player]
        private Text[] player_to_name;// [player]
        private bool[] player_to_cursorMoving;// [player]
        private Rigidbody2D[] player_to_rigidbody2Ds;//[プレイヤー番号]
        public AnimationCurve animCurve = AnimationCurve.Linear(0, 0, 1, 1);
        private int[] player_to_cursorColumn;
        private float[] box_to_locationX = new float[] {// [box column]
        -150.0f, 0.0f, 150.0f
    };
        private float[] player_to_locationY = new float[] // [player]
        {
        -124.0f, -224.0f
        };
        #endregion

        void Start()
        {
            transitionTime = 0;
            #region カーソル移動
            player_to_cursorColumn = new int[] { 0, 0 };
            player_to_cursor = new Text[] { player0, player1 };
            player_to_face = new Image[] { face0, face1 };
            player_to_name = new Text[] { name0, name1 };
            player_to_cursorMoving = new bool[] { false, false };
            player_to_rigidbody2Ds = new Rigidbody2D[] { player0.GetComponent<Rigidbody2D>(), player1.GetComponent<Rigidbody2D>() };
            ChangeCharacter((int)PlayerIndex.Player1);
            ChangeCharacter((int)PlayerIndex.Player2);
            #endregion

            // このシーンのデータベースを用意するぜ☆（＾▽＾）
            AstateDatabase.InsertAllStates();
        }

        // Update is called once per frame
        void Update()
        {
            for (int iPlayer = (int)PlayerIndex.Player1; iPlayer < (int)PlayerIndex.Num; iPlayer++)
            {
                //左キー: -1、右キー: 1
                float leverX = Input.GetAxisRaw(CommonScript.PlayerAndInput_To_InputName[iPlayer, (int)InputIndex.Horizontal]);
                // 下キー: -1、上キー: 1 (Input設定でVerticalの入力にはInvertをチェックしておく）
                float leverY = Input.GetAxisRaw(CommonScript.PlayerAndInput_To_InputName[iPlayer, (int)InputIndex.Vertical]);

                if (
                    !CommonScript.Player_To_Computer[iPlayer] // 人間プレイヤーの場合
                    &&
                    (
                    Input.GetButton(CommonScript.PlayerAndInput_To_InputName[iPlayer, (int)InputIndex.LightPunch]) ||
                    Input.GetButton(CommonScript.PlayerAndInput_To_InputName[iPlayer, (int)InputIndex.MediumPunch]) ||
                    Input.GetButton(CommonScript.PlayerAndInput_To_InputName[iPlayer, (int)InputIndex.HardPunch]) ||
                    Input.GetButton(CommonScript.PlayerAndInput_To_InputName[iPlayer, (int)InputIndex.LightKick]) ||
                    Input.GetButton(CommonScript.PlayerAndInput_To_InputName[iPlayer, (int)InputIndex.MediumKick]) ||
                    Input.GetButton(CommonScript.PlayerAndInput_To_InputName[iPlayer, (int)InputIndex.HardKick]) ||
                    Input.GetButton(CommonScript.PlayerAndInput_To_InputName[iPlayer, (int)InputIndex.Pause]) ||
                    Input.GetButton(CommonScript.INPUT_10_CA)
                    ))
                {
                    // 人間プレイヤーが、何かボタンを押したらメイン画面へ遷移
                    transitionTime = 1;
                }
                else if (
                    CommonScript.Player_To_Computer[iPlayer] && // コンピュータープレイヤーの場合
                    (
                    0 != leverX ||
                    0 != leverY ||
                    Input.GetButton(CommonScript.PlayerAndInput_To_InputName[iPlayer, (int)InputIndex.LightPunch]) ||
                    Input.GetButton(CommonScript.PlayerAndInput_To_InputName[iPlayer, (int)InputIndex.MediumPunch]) ||
                    Input.GetButton(CommonScript.PlayerAndInput_To_InputName[iPlayer, (int)InputIndex.HardPunch]) ||
                    Input.GetButton(CommonScript.PlayerAndInput_To_InputName[iPlayer, (int)InputIndex.LightKick]) ||
                    Input.GetButton(CommonScript.PlayerAndInput_To_InputName[iPlayer, (int)InputIndex.MediumKick]) ||
                    Input.GetButton(CommonScript.PlayerAndInput_To_InputName[iPlayer, (int)InputIndex.HardKick]) ||
                    Input.GetButton(CommonScript.PlayerAndInput_To_InputName[iPlayer, (int)InputIndex.Pause]) ||
                    Input.GetButton(CommonScript.INPUT_10_CA)
                    ))
                {
                    // コンピューター・プレイヤー側のゲームパッドで、何かボタンを押したら、人間の参入。
                    CommonScript.Player_To_Computer[iPlayer] = false;
                    // FIXME: 硬直時間を入れたい。
                }
            }

            if (0 < transitionTime)
            {
                transitionTime++;

                if (5 == transitionTime)
                {
                    SceneManager.LoadScene(CommonScript.scene_to_name[(int)SceneIndex.Main]);
                }
            }

            #region カーソル移動
            for (int iPlayer = (int)PlayerIndex.Player1; iPlayer < (int)PlayerIndex.Num; iPlayer++)
            {
                // 入力
                //左キー: -1、右キー: 1
                float leverX;
                if (CommonScript.Player_To_Computer[iPlayer])
                {
                    leverX = Random.Range(-1.0f, 1.0f);
                }
                else
                {
                    leverX = Input.GetAxisRaw(CommonScript.PlayerAndInput_To_InputName[iPlayer, (int)InputIndex.Horizontal]);
                }

                if (!player_to_cursorMoving[iPlayer])//カーソル移動中でなければ。
                {
                    if (leverX != 0.0f)//左か右を入力したら
                    {
                        player_to_cursorMoving[iPlayer] = true;
                        //Debug.Log("slide lever x = " + leverX.ToString());

                        if (leverX < 0.0f)
                        {
                            player_to_cursorColumn[iPlayer]--;
                            if (player_to_cursorColumn[iPlayer] < 0)
                            {
                                player_to_cursorColumn[iPlayer] = 2;
                            }
                        }
                        else
                        {
                            player_to_cursorColumn[iPlayer]++;
                            if (2 < player_to_cursorColumn[iPlayer])
                            {
                                player_to_cursorColumn[iPlayer] = 0;
                            }
                        }

                        //Debug.Log("slide pos = " + cursorColumn[iPlayerIndex]);

                        ChangeCharacter(iPlayer);

                        //入力方向へ移動
                        //rigidbody2Ds[iPlayerIndex].velocity = new Vector2(leverX * cursorSpeed, rigidbody2Ds[iPlayerIndex].velocity.y);
                        SlideIn((PlayerIndex)iPlayer);
                    }
                    else//左も右も入力していなかったら
                    {
                        //横移動の速度を0にしてピタッと止まるようにする
                        player_to_rigidbody2Ds[iPlayer].velocity = new Vector2(0, player_to_rigidbody2Ds[iPlayer].velocity.y);
                    }
                }
            }
            #endregion
        }

        private void ChangeCharacter(int iPlayer)
        {
            // 選択キャラクター変更
            CharacterIndex character = SceneCommon.X_To_CharacterInSelectMenu[player_to_cursorColumn[iPlayer]];
            CommonScript.Player_To_UseCharacter[iPlayer] = character;
            // 顔変更
            Sprite[] sprites = Resources.LoadAll<Sprite>(CommonScript.CharacterAndSlice_To_FaceSprites[(int)character, (int)ResultFaceSpriteIndex.All]);
            string slice = CommonScript.CharacterAndSlice_To_FaceSprites[(int)character, (int)ResultFaceSpriteIndex.Win];
            player_to_face[iPlayer].sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals(slice));
            // キャラクター名変更
            player_to_name[iPlayer].text = SceneCommon.Character_To_Name[(int)CommonScript.Player_To_UseCharacter[iPlayer]];
        }

        /// <summary>
        /// 参考： http://hoge465.seesaa.net/article/421400008.html
        /// </summary>
        private void SlideIn(PlayerIndex player)
        {
            StartCoroutine(StartSlideCoroutine(player));
        }

        /// <summary>
        /// 参考： http://hoge465.seesaa.net/article/421400008.html
        /// </summary>
        private IEnumerator StartSlideCoroutine(PlayerIndex player)
        {
            Vector3 inPosition = new Vector3(
                box_to_locationX[player_to_cursorColumn[(int)player]],
                player_to_locationY[(int)player],
                0.0f);// スライドイン後の位置
            float duration = 1.0f;// スライド時間（秒）

            float startTime = Time.time;    // 開始時間
            Vector3 startPos = player_to_cursor[(int)player].transform.localPosition;  // 開始位置
            Vector3 moveDistance;            // 移動距離および方向

            moveDistance = (inPosition - startPos);

            while ((Time.time - startTime) < duration)
            {
                player_to_cursor[(int)player].transform.localPosition = startPos + moveDistance * animCurve.Evaluate((Time.time - startTime) / duration);
                yield return 0;        // 1フレーム後、再開
            }
            player_to_cursor[(int)player].transform.localPosition = startPos + moveDistance;
            player_to_cursorMoving[(int)player] = false;
        }
    }
}
