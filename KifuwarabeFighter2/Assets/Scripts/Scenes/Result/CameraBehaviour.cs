namespace SceneResult
{
    using Assets.Scripts;
    using Assets.Scripts.Models;
    using Assets.Scripts.Models.Input;
    using Assets.Scripts.Models.Scenes.Result;
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
            text = GameObject.Find(ThisSceneStatus.GameObjText).GetComponent<Text>();

            // プレイヤー１、２の顔
            {
                player_to_face = new Image[]
                {
                    GameObject.Find(ThisSceneStatus.GameObjectPaths[Player.N1][(int)GameobjectType.Face]).GetComponent<Image>(),
                    GameObject.Find(ThisSceneStatus.GameObjectPaths[Player.N2][(int)GameobjectType.Face]).GetComponent<Image>()
                };

                foreach (var player in Players.All)
                {
                    int character = (int)AppStatus.UseCharacters[player];
                    Sprite[] sprites = Resources.LoadAll<Sprite>(AppConstants.characterAndSliceToFaceSprites[character, (int)KeyOfResultFacialExpression.All]);
                    string slice;
                    switch (AppStatus.Result)
                    {
                        case KeyOfResult.Player1_Win:
                            switch (player)
                            {
                                case Player.N1: slice = AppConstants.characterAndSliceToFaceSprites[character, (int)KeyOfResultFacialExpression.Win]; break;
                                case Player.N2: slice = AppConstants.characterAndSliceToFaceSprites[character, (int)KeyOfResultFacialExpression.Lose]; break;
                                default: Debug.LogError("未定義のプレイヤー☆"); slice = ""; break;
                            }
                            break;
                        case KeyOfResult.Player2_Win:
                            switch (player)
                            {
                                case Player.N1: slice = AppConstants.characterAndSliceToFaceSprites[character, (int)KeyOfResultFacialExpression.Lose]; break;
                                case Player.N2: slice = AppConstants.characterAndSliceToFaceSprites[character, (int)KeyOfResultFacialExpression.Win]; break;
                                default: Debug.LogError("未定義のプレイヤー☆"); slice = ""; break;
                            }
                            break;
                        //case Result.Double_KnockOut:
                        //    break;
                        default:
                            // 開発中画面などで☆
                            slice = AppConstants.characterAndSliceToFaceSprites[character, (int)KeyOfResultFacialExpression.Win];
                            break;
                    }
                    player_to_face[Players.ToArrayIndex(player)].sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals(slice));
                }
            }

            switch (AppStatus.Result)
            {
                case KeyOfResult.Player1_Win:
                    text.text = ThisSceneStatus.WinMessageByCharacter[(int)AppStatus.UseCharacters[Player.N1]];
                    break;
                case KeyOfResult.Player2_Win:
                    text.text = ThisSceneStatus.WinMessageByCharacter[(int)AppStatus.UseCharacters[Player.N2]];
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
            foreach (var player in Players.All)
            {
                // プレイヤーのキー押下状態を確認。
                GamepadStatus gamepad = ApplicationStatus.ReadInput(player);

                // キャンセル以外の 何かボタンを押したらセレクト画面へ遷移
                if (gamepad.Lp.Pressing ||
                    gamepad.Mp.Pressing ||
                    gamepad.Hp.Pressing ||
                    gamepad.Lk.Pressing ||
                    gamepad.Mk.Pressing ||
                    gamepad.Hk.Pressing ||
                    gamepad.Pause.Pressing)
                {
                    SceneManager.LoadScene(AppConstants.sceneToName[(int)KeyOfScene.Select]);
                }
            }
        }
    }
}
