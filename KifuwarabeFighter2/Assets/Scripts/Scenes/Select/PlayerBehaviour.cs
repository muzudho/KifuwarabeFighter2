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
            var player = PlayerKeys.FromArrayIndex(this.playerIndex);

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
            var player = PlayerKeys.FromArrayIndex(this.playerIndex);

            // 現在のアニメーター・ステートに紐づいたデータ
            AcStateRecordable astateRecord = AControl.Instance.GetCurrentAcStateRecord(animator);

            #region 入力受付と途中参加
            GamepadStatus input = ApplicationStatus.ReadInput(player);

            // 人間の途中参加受付
            if (
                AppHelper.ComputerFlags[player] && // コンピュータープレイヤーの場合
                (
                // レバーはコンピューターもいじっているので、区別できない。
                // 0 != leverX ||
                // 0 != leverY ||
                0 != input.LeverX ||
                0 != input.LeverY ||
                input.Lp.Pressing ||
                input.Mp.Pressing ||
                input.Hp.Pressing ||
                input.Lk.Pressing ||
                input.Mk.Pressing ||
                input.Hk.Pressing ||
                input.Pause.Pressing ||
                input.CancelMenu.Pressing
                ))
            {
                Debug.Log("途中参加 " + player + " プレイヤー" + " leverX = " + input.LeverX + " leverY = " + input.LeverY);
                // コンピューター・プレイヤー側のゲームパッドで、何かボタンを押したら、人間の参入。
                AppHelper.ComputerFlags[player] = false;
                // FIXME: 硬直時間を入れたい。
                return;
            }

            if (AppHelper.ComputerFlags[player])
            {
                input.LeverX = Random.Range(-1.0f, 1.0f);
                input.Lp.Pressing = false;
                input.Mp.Pressing = false;
                input.Hp.Pressing = false;
                input.Lk.Pressing = false;
                input.Mk.Pressing = false;
                input.Hk.Pressing = false;
                input.Pause.Pressing = false;
                input.CancelMenu.Pressing = false;
            }
            else
            {
                input.LeverX = Input.GetAxisRaw(ButtonNames.Dictionary[new ButtonKey(player, ButtonType.Horizontal)]);
            }
            #endregion

            if (AControl.Instance.StateHash_to_record[Animator.StringToHash(Select_Cursor_AbstractAControl.BASELAYER_STAY)].Name == astateRecord.Name)
            {
                //カーソル移動中でなければ。

                if (AppHelper.ComputerFlags[player])// コンピュータープレイヤーの場合
                {
                    if (CameraBehaviour.READY_TIME_LENGTH < mainCameraScript.ReadyingTime)
                    {
                        input.Lp.Pressing = (0.5 < Random.Range(0.0f, 1.0f)); // たまにパンチ・キーを押して決定する。
                    }
                }

                if (
                    input.Lp.Pressing ||
                    input.Mp.Pressing ||
                    input.Hp.Pressing ||
                    input.Lk.Pressing ||
                    input.Mk.Pressing ||
                    input.Hk.Pressing ||
                    input.Pause.Pressing
                    )
                {
                    // 何かボタンを押したら、キャラクター選択。
                    animator.SetTrigger(ThisSceneStatus.TriggerSelect);
                }
                else if (input.LeverX != 0.0f)//左か右を入力したら
                {
                    //Debug.Log("slide lever x = " + leverX.ToString());
                    if (input.LeverX < 0.0f)
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
                    !AppHelper.ComputerFlags[player] // 人間プレイヤーの場合
                    &&
                    (
                    input.Lk.Pressing ||
                    input.Mk.Pressing ||
                    input.Hk.Pressing ||
                    input.CancelMenu.Pressing
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
            var player = PlayerKeys.FromArrayIndex(this.playerIndex);

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
            var player = PlayerKeys.FromArrayIndex(this.playerIndex);

            // 選択キャラクター変更
            CharacterKey character = ThisSceneStatus.Table[cursorColumn].CharacterIndex;
            AppHelper.UseCharacters[player] = character;
            // 顔変更
            Sprite[] sprites = Resources.LoadAll<Sprite>(AppHelper.CharacterAndSlice_to_faceSprites[(int)character, (int)ResultFacialExpressionKey.All]);
            string slice = AppHelper.CharacterAndSlice_to_faceSprites[(int)character, (int)ResultFacialExpressionKey.Win];
            face.sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals(slice));
            // キャラクター名変更
            myName.text = ThisSceneStatus.Table[(int)AppHelper.UseCharacters[player]].Name;
        }
    }
}
