namespace SceneResult
{
    using Assets.Scripts.Model.Dto;
    using Assets.Scripts.Model.Dto.Input;
    using Assets.Scripts.Model.Dto.Result;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class CameraBehaviour : MonoBehaviour
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
            text = GameObject.Find(ThisSceneDto.GameObjText).GetComponent<Text>();

            // プレイヤー１、２の顔
            {
                player_to_face = new Image[]
                {
                    GameObject.Find(ThisSceneDto.GameObjectPaths[PlayerNum.N1][(int)GameobjectIndex.Face]).GetComponent<Image>(),
                    GameObject.Find(ThisSceneDto.GameObjectPaths[PlayerNum.N2][(int)GameobjectIndex.Face]).GetComponent<Image>()
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
                                case PlayerNum.N1: slice = CommonScript.CharacterAndSlice_to_faceSprites[character, (int)ResultFaceSpriteIndex.Win]; break;
                                case PlayerNum.N2: slice = CommonScript.CharacterAndSlice_to_faceSprites[character, (int)ResultFaceSpriteIndex.Lose]; break;
                                default: Debug.LogError("未定義のプレイヤー☆"); slice = ""; break;
                            }
                            break;
                        case Result.Player2_Win:
                            switch (player)
                            {
                                case PlayerNum.N1: slice = CommonScript.CharacterAndSlice_to_faceSprites[character, (int)ResultFaceSpriteIndex.Lose]; break;
                                case PlayerNum.N2: slice = CommonScript.CharacterAndSlice_to_faceSprites[character, (int)ResultFaceSpriteIndex.Win]; break;
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
                    text.text = ThisSceneDto.WinMessageByCharacter[(int)CommonScript.UseCharacters[PlayerNum.N1]];
                    break;
                case Result.Player2_Win:
                    text.text = ThisSceneDto.WinMessageByCharacter[(int)CommonScript.UseCharacters[PlayerNum.N2]];
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
            foreach (var player in PlayerIndexes.All)
            {
                // プレイヤーのキー押下状態を確認。
                InputStateDto state = ApplicationDto.ReadInput(player);

                // キャンセル以外の 何かボタンを押したらセレクト画面へ遷移
                if (state.Lp.Pressing ||
                    state.Mp.Pressing ||
                    state.Hp.Pressing ||
                    state.Lk.Pressing ||
                    state.Mk.Pressing ||
                    state.Hk.Pressing ||
                    state.Pause.Pressing)
                {
                    SceneManager.LoadScene(CommonScript.Scene_to_name[(int)SceneIndex.Select]);
                }
            }
        }
    }
}
