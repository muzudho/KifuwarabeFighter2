using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SelectCameraScript : MonoBehaviour {

    int transitionTime;
    #region カーソル移動
    public Text cursor1;
    public Text cursor2;
    public Image face1;
    public Image face2;
    public Text name1;
    public Text name2;
    private Text[] player_to_cursor;// [player]
    private Image[] player_to_face;// [player]
    private Text[] player_to_name;// [player]
    private bool[] cursorMoving;// [player]
    private Rigidbody2D[] rigidbody2Ds;//[プレイヤー番号]
    public AnimationCurve animCurve = AnimationCurve.Linear(0, 0, 1, 1);
    private int[] player_to_cursorColumn;
    private float[] locationX = new float[] {// [column]
        -150.0f, 0.0f, 150.0f
    };
    private float[] locationY = new float[] // [player]
    {
        -124.0f, -224.0f
    };
    #endregion

    void Start()
    {
        transitionTime = 0;
        #region カーソル移動
        player_to_cursorColumn = new int[] { 0, 0 };
        player_to_cursor = new Text[] { cursor1, cursor2 };
        player_to_face = new Image[] { face1, face2 };
        player_to_name = new Text[] { name1, name2 };
        cursorMoving = new bool[] { false, false };
        rigidbody2Ds = new Rigidbody2D[] { cursor1.GetComponent<Rigidbody2D>(), cursor2.GetComponent<Rigidbody2D>() };
        ChangeCharacter((int)PlayerIndex.Player1);
        ChangeCharacter((int)PlayerIndex.Player2);
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        // 何かボタンを押したらメイン画面へ遷移
        if (Input.GetButton(CommonScript.BUTTON_03_P1_LP) ||
            Input.GetButton(CommonScript.BUTTON_04_P1_MP) ||
            Input.GetButton(CommonScript.BUTTON_05_P1_HP) ||
            Input.GetButton(CommonScript.BUTTON_06_P1_LK) ||
            Input.GetButton(CommonScript.BUTTON_07_P1_MK) ||
            Input.GetButton(CommonScript.BUTTON_08_P1_HK) ||
            Input.GetButton(CommonScript.BUTTON_09_P1_PA) ||
            Input.GetButton(CommonScript.BUTTON_10_CA) ||
            Input.GetButton(CommonScript.BUTTON_13_P2_LP) ||
            Input.GetButton(CommonScript.BUTTON_14_P2_MP) ||
            Input.GetButton(CommonScript.BUTTON_15_P2_HP) ||
            Input.GetButton(CommonScript.BUTTON_16_P2_LK) ||
            Input.GetButton(CommonScript.BUTTON_17_P2_MK) ||
            Input.GetButton(CommonScript.BUTTON_18_P2_HK) ||
            Input.GetButton(CommonScript.BUTTON_19_P2_PA)
            )
        {
            transitionTime = 1;
        }

        if (0< transitionTime)
        {
            transitionTime++;

            if (5==transitionTime)
            {
                SceneManager.LoadScene("Main");
            }
        }

    }

    void FixedUpdate()
    {
        #region カーソル移動
        for (int iPlayerIndex = (int)PlayerIndex.Player1; iPlayerIndex < (int)PlayerIndex.Num; iPlayerIndex++)
        {
            if (!cursorMoving[iPlayerIndex])
            {
                //左キー: -1、右キー: 1
                float leverX = Input.GetAxisRaw(CommonScript.PlayerAndButton_To_ButtonName[iPlayerIndex, (int)ButtonIndex.Horizontal]);

                if (leverX != 0.0f)//左か右を入力したら
                {
                    cursorMoving[iPlayerIndex] = true;
                    //Debug.Log("slide lever x = " + leverX.ToString());

                    if (leverX < 0.0f)
                    {
                        player_to_cursorColumn[iPlayerIndex]--;
                        if (player_to_cursorColumn[iPlayerIndex] < 0)
                        {
                            player_to_cursorColumn[iPlayerIndex] = 2;
                        }
                    }
                    else
                    {
                        player_to_cursorColumn[iPlayerIndex]++;
                        if (2 < player_to_cursorColumn[iPlayerIndex])
                        {
                            player_to_cursorColumn[iPlayerIndex] = 0;
                        }
                    }

                    //Debug.Log("slide pos = " + cursorColumn[iPlayerIndex]);

                    ChangeCharacter(iPlayerIndex);

                    //入力方向へ移動
                    //rigidbody2Ds[iPlayerIndex].velocity = new Vector2(leverX * cursorSpeed, rigidbody2Ds[iPlayerIndex].velocity.y);
                    SlideIn((PlayerIndex)iPlayerIndex);
                }
                else//左も右も入力していなかったら
                {
                    //横移動の速度を0にしてピタッと止まるようにする
                    rigidbody2Ds[iPlayerIndex].velocity = new Vector2(0, rigidbody2Ds[iPlayerIndex].velocity.y);
                }
            }
        }
        #endregion
    }

    private void ChangeCharacter(int iPlayerIndex)
    {
        // 選択キャラクター変更
        PlayerCharacter character = CommonScript.X_To_CharacterInSelectMenu[player_to_cursorColumn[iPlayerIndex]];
        CommonScript.Player_To_UseCharacter[iPlayerIndex] = character;
        // 顔変更
        Sprite[] sprites1 = Resources.LoadAll<Sprite>(CommonScript.CharacterAndSlice_To_FaceSprites[(int)character, (int)PlayerCharacterSpritesIndex.All]);
        string slice1 = CommonScript.CharacterAndSlice_To_FaceSprites[(int)character, (int)PlayerCharacterSpritesIndex.Win];
        player_to_face[iPlayerIndex].sprite = System.Array.Find<Sprite>(sprites1, (sprite) => sprite.name.Equals(slice1));
        // キャラクター名変更
        player_to_name[iPlayerIndex].text = CommonScript.Character_To_Name[(int)CommonScript.Player_To_UseCharacter[iPlayerIndex]];
    }

    /// <summary>
    /// 参考： http://hoge465.seesaa.net/article/421400008.html
    /// </summary>
    private void SlideIn(PlayerIndex playerIndex)
    {
        StartCoroutine( StartSlideCoroutine(playerIndex));
    }

    /// <summary>
    /// 参考： http://hoge465.seesaa.net/article/421400008.html
    /// </summary>
    private IEnumerator StartSlideCoroutine(PlayerIndex playerIndex)
    {
        Vector3 inPosition = new Vector3(
            locationX[player_to_cursorColumn[(int)playerIndex]],
            locationY[(int)playerIndex],
            0.0f);// スライドイン後の位置
        float duration = 1.0f;// スライド時間（秒）

        float startTime = Time.time;    // 開始時間
        Vector3 startPos = player_to_cursor[(int)playerIndex].transform.localPosition;  // 開始位置
        Vector3 moveDistance;            // 移動距離および方向

        moveDistance = (inPosition - startPos);

        while ((Time.time - startTime) < duration)
        {
            player_to_cursor[(int)playerIndex].transform.localPosition = startPos + moveDistance * animCurve.Evaluate((Time.time - startTime) / duration);
            yield return 0;        // 1フレーム後、再開
        }
        player_to_cursor[(int)playerIndex].transform.localPosition = startPos + moveDistance;
        cursorMoving[(int)playerIndex] = false;
    }
}
