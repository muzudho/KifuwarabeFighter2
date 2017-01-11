using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultCameraScript : MonoBehaviour {

    public Text text;
    public Image face1;
    public Image face2;
    private Image[] faces;

    void Start()
    {
        // プレイヤー１、２の顔
        {
            //Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/顔３_らぶりんこ");
            //face1.sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("顔３_らぶりんこ_0"));

            faces = new Image[] { face1, face2 };

            for (int iPlayer = (int)PlayerIndex.Player1; iPlayer < (int)PlayerIndex.Num; iPlayer++)
            {
                int character = (int)CommonScript.Player_To_UseCharacter[iPlayer];
                Sprite[] sprites = Resources.LoadAll<Sprite>(CommonScript.CharacterAndSlice_To_FaceSprites[character, (int)PlayerCharacterSpritesIndex.All]);
                string slice;
                switch (CommonScript.Result)
                {
                    case Result.Player1_Win:
                        switch ((PlayerIndex)iPlayer)
                        {
                            case PlayerIndex.Player1: slice = CommonScript.CharacterAndSlice_To_FaceSprites[character, (int)PlayerCharacterSpritesIndex.Win]; break;
                            case PlayerIndex.Player2: slice = CommonScript.CharacterAndSlice_To_FaceSprites[character, (int)PlayerCharacterSpritesIndex.Lose]; break;
                            default: Debug.LogError("未定義のプレイヤー☆"); slice = ""; break;
                        }
                        break;
                    case Result.Player2_Win:
                        switch ((PlayerIndex)iPlayer)
                        {
                            case PlayerIndex.Player1: slice = CommonScript.CharacterAndSlice_To_FaceSprites[character, (int)PlayerCharacterSpritesIndex.Lose]; break;
                            case PlayerIndex.Player2: slice = CommonScript.CharacterAndSlice_To_FaceSprites[character, (int)PlayerCharacterSpritesIndex.Win]; break;
                            default: Debug.LogError("未定義のプレイヤー☆"); slice = ""; break;
                        }
                        break;
                    //case Result.Double_KnockOut:
                    //    break;
                    default:
                        // 開発中画面などで☆
                        slice = CommonScript.CharacterAndSlice_To_FaceSprites[character, (int)PlayerCharacterSpritesIndex.Win];
                        break;
                }
                faces[iPlayer].sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals(slice));
            }
        }

        switch (CommonScript.Result)
        {
            case Result.Player1_Win:
                text.text = CommonScript.Character_To_WinMessage[(int)CommonScript.Player_To_UseCharacter[(int)PlayerIndex.Player1]];
                break;
            case Result.Player2_Win:
                text.text = CommonScript.Character_To_WinMessage[(int)CommonScript.Player_To_UseCharacter[(int)PlayerIndex.Player2]];
                break;
            //case Result.Double_KnockOut:
            //    text.text = "ダブルＫＯ！\n";
            //    break;
            default:
                text.text = "結果は\nどうなったんだぜ☆（＾～＾）？";
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

        // 何かボタンを押したらセレクト画面へ遷移
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
            SceneManager.LoadScene("Select");
        }

    }
}
