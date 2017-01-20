using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SceneResult
{
    public class Result_CameraScript : MonoBehaviour
    {
        public Text text;
        public Image face0;
        public Image face1;
        private Image[] player_to_face;

        void Start()
        {
            // プレイヤー１、２の顔
            {
                //Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/顔３_らぶりんこ");
                //face1.sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("顔３_らぶりんこ_0"));

                player_to_face = new Image[] { face0, face1 };

                for (int iPlayer = (int)PlayerIndex.Player1; iPlayer < (int)PlayerIndex.Num; iPlayer++)
                {
                    int character = (int)CommonScript.Player_To_UseCharacter[iPlayer];
                    Sprite[] sprites = Resources.LoadAll<Sprite>(CommonScript.CharacterAndSlice_To_FaceSprites[character, (int)ResultFaceSpriteIndex.All]);
                    string slice;
                    switch (CommonScript.Result)
                    {
                        case Result.Player1_Win:
                            switch ((PlayerIndex)iPlayer)
                            {
                                case PlayerIndex.Player1: slice = CommonScript.CharacterAndSlice_To_FaceSprites[character, (int)ResultFaceSpriteIndex.Win]; break;
                                case PlayerIndex.Player2: slice = CommonScript.CharacterAndSlice_To_FaceSprites[character, (int)ResultFaceSpriteIndex.Lose]; break;
                                default: Debug.LogError("未定義のプレイヤー☆"); slice = ""; break;
                            }
                            break;
                        case Result.Player2_Win:
                            switch ((PlayerIndex)iPlayer)
                            {
                                case PlayerIndex.Player1: slice = CommonScript.CharacterAndSlice_To_FaceSprites[character, (int)ResultFaceSpriteIndex.Lose]; break;
                                case PlayerIndex.Player2: slice = CommonScript.CharacterAndSlice_To_FaceSprites[character, (int)ResultFaceSpriteIndex.Win]; break;
                                default: Debug.LogError("未定義のプレイヤー☆"); slice = ""; break;
                            }
                            break;
                        //case Result.Double_KnockOut:
                        //    break;
                        default:
                            // 開発中画面などで☆
                            slice = CommonScript.CharacterAndSlice_To_FaceSprites[character, (int)ResultFaceSpriteIndex.Win];
                            break;
                    }
                    player_to_face[iPlayer].sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals(slice));
                }
            }

            switch (CommonScript.Result)
            {
                case Result.Player1_Win:
                    text.text = SceneCommon.Character_To_WinMessage[(int)CommonScript.Player_To_UseCharacter[(int)PlayerIndex.Player1]];
                    break;
                case Result.Player2_Win:
                    text.text = SceneCommon.Character_To_WinMessage[(int)CommonScript.Player_To_UseCharacter[(int)PlayerIndex.Player2]];
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
            if (Input.GetButton(CommonScript.PlayerAndInput_To_InputName[(int)PlayerIndex.Player1, (int)InputIndex.LightPunch]) ||
                Input.GetButton(CommonScript.PlayerAndInput_To_InputName[(int)PlayerIndex.Player1, (int)InputIndex.MediumPunch]) ||
                Input.GetButton(CommonScript.PlayerAndInput_To_InputName[(int)PlayerIndex.Player1, (int)InputIndex.HardPunch]) ||
                Input.GetButton(CommonScript.PlayerAndInput_To_InputName[(int)PlayerIndex.Player1, (int)InputIndex.LightKick]) ||
                Input.GetButton(CommonScript.PlayerAndInput_To_InputName[(int)PlayerIndex.Player1, (int)InputIndex.MediumKick]) ||
                Input.GetButton(CommonScript.PlayerAndInput_To_InputName[(int)PlayerIndex.Player1, (int)InputIndex.HardKick]) ||
                Input.GetButton(CommonScript.PlayerAndInput_To_InputName[(int)PlayerIndex.Player1, (int)InputIndex.Pause]) ||
                Input.GetButton(CommonScript.INPUT_10_CA) ||
                Input.GetButton(CommonScript.PlayerAndInput_To_InputName[(int)PlayerIndex.Player2, (int)InputIndex.LightPunch]) ||
                Input.GetButton(CommonScript.PlayerAndInput_To_InputName[(int)PlayerIndex.Player2, (int)InputIndex.MediumPunch]) ||
                Input.GetButton(CommonScript.PlayerAndInput_To_InputName[(int)PlayerIndex.Player2, (int)InputIndex.HardPunch]) ||
                Input.GetButton(CommonScript.PlayerAndInput_To_InputName[(int)PlayerIndex.Player2, (int)InputIndex.LightKick]) ||
                Input.GetButton(CommonScript.PlayerAndInput_To_InputName[(int)PlayerIndex.Player2, (int)InputIndex.MediumKick]) ||
                Input.GetButton(CommonScript.PlayerAndInput_To_InputName[(int)PlayerIndex.Player2, (int)InputIndex.HardPunch]) ||
                Input.GetButton(CommonScript.PlayerAndInput_To_InputName[(int)PlayerIndex.Player2, (int)InputIndex.Pause])
                )
            {
                SceneManager.LoadScene(CommonScript.scene_to_name[(int)SceneIndex.Select]);
            }

        }
    }
}
