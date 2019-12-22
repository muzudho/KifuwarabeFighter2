namespace SceneResult
{
    using Assets.Scripts.Model.Dto.Input;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class Result_CameraScript : MonoBehaviour
    {
        /// <summary>
        /// message. 勝利後メッセージ。
        /// </summary>
        Text text;
        /// <summary>
        /// player's face(win or lose). プレイヤーの顔。
        /// </summary>
        Image[] player_to_face;

        void Start()
        {
            text = GameObject.Find(SceneCommon.GAMEOBJ_TEXT).GetComponent<Text>();

            // プレイヤー１、２の顔
            {
                player_to_face = new Image[]
                {
                    GameObject.Find(SceneCommon.GameobjectToPath[PlayerIndex.Player1][(int)GameobjectIndex.Face]).GetComponent<Image>(),
                    GameObject.Find(SceneCommon.GameobjectToPath[PlayerIndex.Player2][(int)GameobjectIndex.Face]).GetComponent<Image>()
                };

                foreach (var player in PlayerIndexes.All)
                {
                    int character = (int)CommonScript.UseCharacters[player];
                    Sprite[] sprites = Resources.LoadAll<Sprite>(CommonScript.CharacterAndSlice_to_faceSprites[character, (int)ResultFaceSpriteIndex.All]);
                    string slice;
                    switch (CommonScript.Result)
                    {
                        case Result.Player1_Win:
                            switch (player)
                            {
                                case PlayerIndex.Player1: slice = CommonScript.CharacterAndSlice_to_faceSprites[character, (int)ResultFaceSpriteIndex.Win]; break;
                                case PlayerIndex.Player2: slice = CommonScript.CharacterAndSlice_to_faceSprites[character, (int)ResultFaceSpriteIndex.Lose]; break;
                                default: Debug.LogError("未定義のプレイヤー☆"); slice = ""; break;
                            }
                            break;
                        case Result.Player2_Win:
                            switch (player)
                            {
                                case PlayerIndex.Player1: slice = CommonScript.CharacterAndSlice_to_faceSprites[character, (int)ResultFaceSpriteIndex.Lose]; break;
                                case PlayerIndex.Player2: slice = CommonScript.CharacterAndSlice_to_faceSprites[character, (int)ResultFaceSpriteIndex.Win]; break;
                                default: Debug.LogError("未定義のプレイヤー☆"); slice = ""; break;
                            }
                            break;
                        //case Result.Double_KnockOut:
                        //    break;
                        default:
                            // 開発中画面などで☆
                            slice = CommonScript.CharacterAndSlice_to_faceSprites[character, (int)ResultFaceSpriteIndex.Win];
                            break;
                    }
                    player_to_face[PlayerIndexes.ToArrayIndex(player)].sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals(slice));
                }
            }

            switch (CommonScript.Result)
            {
                case Result.Player1_Win:
                    text.text = SceneCommon.Character_To_WinMessage[(int)CommonScript.UseCharacters[PlayerIndex.Player1]];
                    break;
                case Result.Player2_Win:
                    text.text = SceneCommon.Character_To_WinMessage[(int)CommonScript.UseCharacters[PlayerIndex.Player2]];
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
            // キャンセル以外の 何かボタンを押したらセレクト画面へ遷移
            if (Input.GetButton(InputNames.Dictionary[InputIndexes.P1Lp]) ||
                Input.GetButton(InputNames.Dictionary[InputIndexes.P1Mp]) ||
                Input.GetButton(InputNames.Dictionary[InputIndexes.P1Hp]) ||
                Input.GetButton(InputNames.Dictionary[InputIndexes.P1Lk]) ||
                Input.GetButton(InputNames.Dictionary[InputIndexes.P1Mk]) ||
                Input.GetButton(InputNames.Dictionary[InputIndexes.P1Hk]) ||
                Input.GetButton(InputNames.Dictionary[InputIndexes.P1Pause]) ||
                // Input.GetButton(InputNames.Dictionary[InputIndexes.P1CancelMenu]) ||
                Input.GetButton(InputNames.Dictionary[InputIndexes.P2Lp]) ||
                Input.GetButton(InputNames.Dictionary[InputIndexes.P2Mp]) ||
                Input.GetButton(InputNames.Dictionary[InputIndexes.P2Hp]) ||
                Input.GetButton(InputNames.Dictionary[InputIndexes.P2Lk]) ||
                Input.GetButton(InputNames.Dictionary[InputIndexes.P2Mk]) ||
                Input.GetButton(InputNames.Dictionary[InputIndexes.P2Hp]) ||
                Input.GetButton(InputNames.Dictionary[InputIndexes.P2Pause])
            )
            {
                SceneManager.LoadScene(CommonScript.Scene_to_name[(int)SceneIndex.Select]);
            }

        }
    }
}
