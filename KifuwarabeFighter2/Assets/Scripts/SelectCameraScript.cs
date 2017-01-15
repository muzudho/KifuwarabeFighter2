using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SelectCameraScript : MonoBehaviour {

    int transitionTime;
    #region カーソル移動
    public Text cursor0;
    public Text cursor1;
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
        player_to_cursor = new Text[] { cursor0, cursor1 };
        player_to_face = new Image[] { face0, face1 };
        player_to_name = new Text[] { name0, name1 };
        player_to_cursorMoving = new bool[] { false, false };
        player_to_rigidbody2Ds = new Rigidbody2D[] { cursor0.GetComponent<Rigidbody2D>(), cursor1.GetComponent<Rigidbody2D>() };
        ChangeCharacter((int)PlayerIndex.Player1);
        ChangeCharacter((int)PlayerIndex.Player2);
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        for (int iPlayer = (int)PlayerIndex.Player1; iPlayer < (int)PlayerIndex.Num; iPlayer++)
        {
            if (
                !CommonScript.Player_To_Computer[iPlayer] &&
                (
                Input.GetButton(CommonScript.PlayerAndButton_To_ButtonName[iPlayer, (int)ButtonIndex.LightPunch]) ||
                Input.GetButton(CommonScript.PlayerAndButton_To_ButtonName[iPlayer, (int)ButtonIndex.MediumPunch]) ||
                Input.GetButton(CommonScript.PlayerAndButton_To_ButtonName[iPlayer, (int)ButtonIndex.HardPunch]) ||
                Input.GetButton(CommonScript.PlayerAndButton_To_ButtonName[iPlayer, (int)ButtonIndex.LightKick]) ||
                Input.GetButton(CommonScript.PlayerAndButton_To_ButtonName[iPlayer, (int)ButtonIndex.MediumKick]) ||
                Input.GetButton(CommonScript.PlayerAndButton_To_ButtonName[iPlayer, (int)ButtonIndex.HardKick]) ||
                Input.GetButton(CommonScript.PlayerAndButton_To_ButtonName[iPlayer, (int)ButtonIndex.Pause]) ||
                Input.GetButton(CommonScript.BUTTON_10_CA)
                ))
            {
                // 人間プレイヤーが、何かボタンを押したらメイン画面へ遷移
                transitionTime = 1;
            }
            else if (
                CommonScript.Player_To_Computer[iPlayer] &&
                (
                Input.GetButton(CommonScript.PlayerAndButton_To_ButtonName[iPlayer, (int)ButtonIndex.LightPunch]) ||
                Input.GetButton(CommonScript.PlayerAndButton_To_ButtonName[iPlayer, (int)ButtonIndex.MediumPunch]) ||
                Input.GetButton(CommonScript.PlayerAndButton_To_ButtonName[iPlayer, (int)ButtonIndex.HardPunch]) ||
                Input.GetButton(CommonScript.PlayerAndButton_To_ButtonName[iPlayer, (int)ButtonIndex.LightKick]) ||
                Input.GetButton(CommonScript.PlayerAndButton_To_ButtonName[iPlayer, (int)ButtonIndex.MediumKick]) ||
                Input.GetButton(CommonScript.PlayerAndButton_To_ButtonName[iPlayer, (int)ButtonIndex.HardKick]) ||
                Input.GetButton(CommonScript.PlayerAndButton_To_ButtonName[iPlayer, (int)ButtonIndex.Pause]) ||
                Input.GetButton(CommonScript.BUTTON_10_CA)
                ))
            {
                // コンピューター・プレイヤー側のゲームパッドで、何かボタンを押したら、人間の参入。
                CommonScript.Player_To_Computer[iPlayer] = false;
            }
        }

        if (0< transitionTime)
        {
            transitionTime++;

            if (5==transitionTime)
            {
                SceneManager.LoadScene(CommonScript.SCENE_MAIN);
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
                leverX = Input.GetAxisRaw(CommonScript.PlayerAndButton_To_ButtonName[iPlayer, (int)ButtonIndex.Horizontal]);
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
        CharacterIndex character = CommonScript.X_To_CharacterInSelectMenu[player_to_cursorColumn[iPlayer]];
        CommonScript.Player_To_UseCharacter[iPlayer] = character;
        // 顔変更
        Sprite[] sprites = Resources.LoadAll<Sprite>(CommonScript.CharacterAndSlice_To_FaceSprites[(int)character, (int)PlayerCharacterSpritesIndex.All]);
        string slice = CommonScript.CharacterAndSlice_To_FaceSprites[(int)character, (int)PlayerCharacterSpritesIndex.Win];
        player_to_face[iPlayer].sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals(slice));
        // キャラクター名変更
        player_to_name[iPlayer].text = CommonScript.Character_To_Name[(int)CommonScript.Player_To_UseCharacter[iPlayer]];
    }

    /// <summary>
    /// 参考： http://hoge465.seesaa.net/article/421400008.html
    /// </summary>
    private void SlideIn(PlayerIndex player)
    {
        StartCoroutine( StartSlideCoroutine(player));
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
