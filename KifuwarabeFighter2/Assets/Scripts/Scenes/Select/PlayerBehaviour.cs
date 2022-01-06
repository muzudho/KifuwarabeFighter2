namespace SceneSelect
{
    using System.Collections;
    using Assets.Scripts;
    using Assets.Scripts.Models;
    using Assets.Scripts.Models.Input;
    using Assets.Scripts.Models.Scene.Select;
    using DojinCircleGrayscale.StellaQL;
    using DojinCircleGrayscale.StellaQL.Acons.Select_Cursor;
    using UnityEngine;
    using UnityEngine.UI;

    public class PlayerBehaviour : MonoBehaviour
    {
        /// <summary>
        /// 1 player is 0. 2 player is 1. １プレイヤーは 0、２プレイヤーは 1。
        /// </summary>
        public int playerIndex;

        Animator animator;
        CameraBehaviour mainCameraScript;
        int cursorColumn;
        Rigidbody2D myRigidbody2D;
        Text playerChar;
        static AnimationCurve animCurve = AnimationCurve.Linear(0, 0, 1, 1);
        Image face;
        Text myName;


        // Use this for initialization
        void Start()
        {
            var player = Players.FromArrayIndex(this.playerIndex);

            animator = GetComponent<Animator>();
            mainCameraScript = GameObject.Find("Main Camera").GetComponent<CameraBehaviour>();

            cursorColumn = 0;
            myRigidbody2D = GetComponent<Rigidbody2D>();
            playerChar = GameObject.Find(GameObjectPaths.All[new GameObjectKey(player, GameObjectType.Player)]).GetComponent<Text>();
            face = GameObject.Find(GameObjectPaths.All[new GameObjectKey(player, GameObjectType.Face)]).GetComponent<Image>();
            myName = GameObject.Find(GameObjectPaths.All[new GameObjectKey(player, GameObjectType.Name)]).GetComponent<Text>();

            ChangeCharacter();
        }

        // Update is called once per frame
        void Update()
        {
            var player = Players.FromArrayIndex(this.playerIndex);

            // 現在のアニメーター・ステートに紐づいたデータ
            AcStateRecordable astateRecord = AControl.Instance.GetCurrentAcStateRecord(animator);

            #region 入力受付と途中参加
            GamepadStatus gamepad = ApplicationStatus.ReadInput(player);

            // 人間の途中参加受付
            if (
                AppStatus.IsComputer[player] && // コンピュータープレイヤーの場合
                (
                // レバーはコンピューターもいじっているので、区別できない。
                // 0 != leverX ||
                // 0 != leverY ||
                0 != gamepad.HorizontalLever.Value ||
                0 != gamepad.VerticalLever.Value ||
                gamepad.Lp.Pressing ||
                gamepad.Mp.Pressing ||
                gamepad.Hp.Pressing ||
                gamepad.Lk.Pressing ||
                gamepad.Mk.Pressing ||
                gamepad.Hk.Pressing ||
                gamepad.Pause.Pressing ||
                gamepad.CancelMenu.Pressing
                ))
            {
                Debug.Log("途中参加 " + player + " プレイヤー" + " leverX = " + gamepad.HorizontalLever.Value + " leverY = " + gamepad.VerticalLever.Value);
                // コンピューター・プレイヤー側のゲームパッドで、何かボタンを押したら、人間の参入。
                AppStatus.IsComputer[player] = false;
                // FIXME: 硬直時間を入れたい。
                return;
            }

            if (AppStatus.IsComputer[player])
            {
                gamepad.HorizontalLever.Value = Random.Range(-1.0f, 1.0f);
                gamepad.Lp.Pressing = false;
                gamepad.Mp.Pressing = false;
                gamepad.Hp.Pressing = false;
                gamepad.Lk.Pressing = false;
                gamepad.Mk.Pressing = false;
                gamepad.Hk.Pressing = false;
                gamepad.Pause.Pressing = false;
                gamepad.CancelMenu.Pressing = false;
            }
            else
            {
                gamepad.HorizontalLever.Value = Input.GetAxisRaw(ButtonNames.Dictionary[new ButtonKey(player, ButtonType.Horizontal)]);
            }
            #endregion

            if (AControl.Instance.StateHash_to_record[Animator.StringToHash(Select_Cursor_AbstractAControl.BASELAYER_STAY)].Name == astateRecord.Name)
            {
                //カーソル移動中でなければ。

                if (AppStatus.IsComputer[player])// コンピュータープレイヤーの場合
                {
                    if (CameraBehaviour.READY_TIME_LENGTH < mainCameraScript.ReadyingTime)
                    {
                        gamepad.Lp.Pressing = (0.5 < Random.Range(0.0f, 1.0f)); // たまにパンチ・キーを押して決定する。
                    }
                }

                if (
                    gamepad.Lp.Pressing ||
                    gamepad.Mp.Pressing ||
                    gamepad.Hp.Pressing ||
                    gamepad.Lk.Pressing ||
                    gamepad.Mk.Pressing ||
                    gamepad.Hk.Pressing ||
                    gamepad.Pause.Pressing
                    )
                {
                    // 何かボタンを押したら、キャラクター選択。
                    animator.SetTrigger(ThisSceneStatus.TriggerSelect);
                }
                else if (gamepad.HorizontalLever.Value != 0.0f)//左か右を入力したら
                {
                    //Debug.Log("slide lever x = " + leverX.ToString());
                    if (gamepad.HorizontalLever.Value < 0.0f)
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

                    animator.SetTrigger(ThisSceneStatus.TriggerMove);

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
            else if (AControl.Instance.StateHash_to_record[Animator.StringToHash(Select_Cursor_AbstractAControl.BASELAYER_MOVE)].Name == astateRecord.Name)
            {
            }
            else if (AControl.Instance.StateHash_to_record[Animator.StringToHash(Select_Cursor_AbstractAControl.BASELAYER_READY)].Name == astateRecord.Name)
            {
                // キャラクター選択済みのとき

                if (
                    !AppStatus.IsComputer[player] // 人間プレイヤーの場合
                    &&
                    (
                    gamepad.Lk.Pressing ||
                    gamepad.Mk.Pressing ||
                    gamepad.Hk.Pressing ||
                    gamepad.CancelMenu.Pressing
                    ))
                {
                    // キック・ボタンを押したら、キャンセル☆
                    animator.SetTrigger(ThisSceneStatus.TriggerStay);
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
            var player = Players.FromArrayIndex(this.playerIndex);

            Vector3 inPosition = new Vector3(
                ThisSceneStatus.Table[cursorColumn].X,
                ThisSceneStatus.PlayerStatusDict[player].LocationY,
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

            animator.SetTrigger(ThisSceneStatus.TriggerStay);
        }


        private void ChangeCharacter()
        {
            var player = Players.FromArrayIndex(this.playerIndex);

            // 選択キャラクター変更
            KeyOfCharacter character = ThisSceneStatus.Table[cursorColumn].CharacterIndex;
            AppStatus.UseCharacters[player] = character;
            // 顔変更
            Sprite[] sprites = Resources.LoadAll<Sprite>(AppConstants.characterAndSliceToFaceSprites[(int)character, (int)KeyOfResultFacialExpression.All]);
            string slice = AppConstants.characterAndSliceToFaceSprites[(int)character, (int)KeyOfResultFacialExpression.Win];
            face.sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals(slice));
            // キャラクター名変更
            myName.text = ThisSceneStatus.Table[(int)AppStatus.UseCharacters[player]].Name;
        }
    }
}
