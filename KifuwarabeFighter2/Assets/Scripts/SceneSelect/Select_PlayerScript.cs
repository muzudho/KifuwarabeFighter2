using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StellaQL;

namespace SceneSelect
{
    public class Select_PlayerScript : MonoBehaviour
    {
        /// <summary>
        /// 1 player is 0. 2 player is 1. １プレイヤーは 0、２プレイヤーは 1。
        /// </summary>
        public int playerIndex;
        Animator animator;
        Select_CameraScript mainCameraScript;

        int cursorColumn;
        Rigidbody2D myRigidbody2D;
        Text playerChar;
        static AnimationCurve animCurve = AnimationCurve.Linear(0, 0, 1, 1);
        Image face;
        Text myName;


        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            mainCameraScript = GameObject.Find("Main Camera").GetComponent<Select_CameraScript>();

            cursorColumn = 0;
            myRigidbody2D = GetComponent<Rigidbody2D>();
            playerChar = GameObject.Find(SceneCommon.PlayerAndGameobject_to_path[playerIndex, (int)GameobjectIndex.Player]).GetComponent<Text>();
            face = GameObject.Find(SceneCommon.PlayerAndGameobject_to_path[playerIndex, (int)GameobjectIndex.Face]).GetComponent<Image>();
            myName = GameObject.Find(SceneCommon.PlayerAndGameobject_to_path[playerIndex, (int)GameobjectIndex.Name]).GetComponent<Text>();

            ChangeCharacter();
        }

        // Update is called once per frame
        void Update()
        {
            // 現在のアニメーター・ステートに紐づいたデータ
            UserDefindStateRecordable astateRecord = UserDefinedStateTable.Instance.GetCurrentUserDefinedStateRecord(animator);

            #region 入力受付と途中参加
            CommonInput.PlayerInput input = CommonInput.Update((PlayerIndex)playerIndex);

            // 人間の途中参加受付
            if (
                CommonScript.Player_to_computer[playerIndex] && // コンピュータープレイヤーの場合
                (
                // レバーはコンピューターもいじっているので、区別できない。
                // 0 != leverX ||
                // 0 != leverY ||
                0 != input.leverX ||
                0 != input.leverY ||
                input.pressingLP ||
                input.pressingMP ||
                input.pressingHP ||
                input.pressingLK ||
                input.pressingMK ||
                input.pressingHK ||
                input.pressingPA ||
                input.pressingCA
                ))
            {
                Debug.Log("途中参加 " + playerIndex + " プレイヤー" + " leverX = " + input.leverX + " leverY = " + input.leverY);
                // コンピューター・プレイヤー側のゲームパッドで、何かボタンを押したら、人間の参入。
                CommonScript.Player_to_computer[playerIndex] = false;
                // FIXME: 硬直時間を入れたい。
                return;
            }

            if (CommonScript.Player_to_computer[playerIndex])
            {
                input.leverX = Random.Range(-1.0f, 1.0f);
                input.pressingLP = false;
                input.pressingMP = false;
                input.pressingHP = false;
                input.pressingLK = false;
                input.pressingMK = false;
                input.pressingHK = false;
                input.pressingPA = false;
                input.pressingCA = false;
            }
            else
            {
                input.leverX = Input.GetAxisRaw(CommonInput.PlayerAndInput_to_inputName[playerIndex, (int)InputIndex.Horizontal]);
            }
            #endregion

            if (UserDefinedStateTable.Instance.StateHash_to_record[Animator.StringToHash(UserDefinedStateTable.STATE_STAY)].Name == astateRecord.Name)
            {
                //カーソル移動中でなければ。

                if (CommonScript.Player_to_computer[playerIndex])// コンピュータープレイヤーの場合
                {
                    if (Select_CameraScript.READY_TIME_LENGTH < mainCameraScript.ReadyingTime)
                    {
                        input.pressingLP = (0.5 < Random.Range(0.0f, 1.0f)); // たまにパンチ・キーを押して決定する。
                    }
                }

                if (
                    input.pressingLP ||
                    input.pressingMP ||
                    input.pressingHP ||
                    input.pressingLK ||
                    input.pressingMK ||
                    input.pressingHK ||
                    input.pressingPA
                    )
                {
                    // 何かボタンを押したら、キャラクター選択。
                    animator.SetTrigger(SceneCommon.TRIGGER_SELECT);
                }
                else if (input.leverX != 0.0f)//左か右を入力したら
                {
                    //Debug.Log("slide lever x = " + leverX.ToString());
                    if (input.leverX < 0.0f)
                    {
                        cursorColumn--;
                        if (cursorColumn < 0)
                        {
                            cursorColumn = 2;
                        }
                    }
                    else
                    {
                        cursorColumn++;
                        if (2 < cursorColumn)
                        {
                            cursorColumn = 0;
                        }
                    }

                    //Debug.Log("slide pos = " + cursorColumn[iPlayerIndex]);

                    animator.SetTrigger(SceneCommon.TRIGGER_MOVE);

                    ChangeCharacter();

                    //入力方向へ移動
                    //rigidbody2Ds[iPlayerIndex].velocity = new Vector2(leverX * cursorSpeed, rigidbody2Ds[iPlayerIndex].velocity.y);
                    SlideIn();
                }
                else // ボタン押下も、レバー左右入力もしていなかったら
                {
                    //横移動の速度を0にしてピタッと止まるようにする
                    myRigidbody2D.velocity = new Vector2(0, myRigidbody2D.velocity.y);
                }
            }
            else if (UserDefinedStateTable.Instance.StateHash_to_record[Animator.StringToHash(UserDefinedStateTable.STATE_MOVE)].Name == astateRecord.Name)
            {
            }
            else if (UserDefinedStateTable.Instance.StateHash_to_record[Animator.StringToHash(UserDefinedStateTable.STATE_READY)].Name == astateRecord.Name)
            {
                // キャラクター選択済みのとき

                if (
                    !CommonScript.Player_to_computer[playerIndex] // 人間プレイヤーの場合
                    &&
                    (
                    input.pressingLK ||
                    input.pressingMK ||
                    input.pressingHK ||
                    input.pressingCA
                    ))
                {
                    // キック・ボタンを押したら、キャンセル☆
                    animator.SetTrigger(SceneCommon.TRIGGER_STAY);
                }
            }
        }


        /// <summary>
        /// 参考： http://hoge465.seesaa.net/article/421400008.html
        /// </summary>
        private void SlideIn()
        {
            StartCoroutine(StartSlideCoroutine());
        }

        /// <summary>
        /// 参考： http://hoge465.seesaa.net/article/421400008.html
        /// </summary>
        private IEnumerator StartSlideCoroutine()
        {
            Vector3 inPosition = new Vector3(
                SceneCommon.BoxColumn_to_locationX[cursorColumn],
                SceneCommon.Player_to_locationY[playerIndex],
                0.0f);// スライドイン後の位置
            float duration = 1.0f;// スライド時間（秒）

            float startTime = Time.time;    // 開始時間
            Vector3 startPos = playerChar.transform.localPosition;  // 開始位置
            Vector3 moveDistance;            // 移動距離および方向

            moveDistance = (inPosition - startPos);

            while ((Time.time - startTime) < duration)
            {
                playerChar.transform.localPosition = startPos + moveDistance * animCurve.Evaluate((Time.time - startTime) / duration);
                yield return 0;        // 1フレーム後、再開
            }
            playerChar.transform.localPosition = startPos + moveDistance;

            animator.SetTrigger(SceneCommon.TRIGGER_STAY);
        }


        private void ChangeCharacter()
        {
            // 選択キャラクター変更
            CharacterIndex character = SceneCommon.X_To_CharacterInSelectMenu[cursorColumn];
            CommonScript.Player_to_useCharacter[playerIndex] = character;
            // 顔変更
            Sprite[] sprites = Resources.LoadAll<Sprite>(CommonScript.CharacterAndSlice_to_faceSprites[(int)character, (int)ResultFaceSpriteIndex.All]);
            string slice = CommonScript.CharacterAndSlice_to_faceSprites[(int)character, (int)ResultFaceSpriteIndex.Win];
            face.sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals(slice));
            // キャラクター名変更
            myName.text = SceneCommon.Character_To_Name[(int)CommonScript.Player_to_useCharacter[playerIndex]];
        }
    }
}
