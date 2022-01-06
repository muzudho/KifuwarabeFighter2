﻿namespace SceneResult
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
                    GameObject.Find(ThisSceneStatus.GameObjectPaths[PlayerKey.N1][(int)GameobjectType.Face]).GetComponent<Image>(),
                    GameObject.Find(ThisSceneStatus.GameObjectPaths[PlayerKey.N2][(int)GameobjectType.Face]).GetComponent<Image>()
                };

                foreach (var player in PlayerKeys.All)
                {
                    int character = (int)AppHelper.UseCharacters[player];
                    Sprite[] sprites = Resources.LoadAll<Sprite>(AppHelper.CharacterAndSlice_to_faceSprites[character, (int)ResultFacialExpressionKey.All]);
                    string slice;
                    switch (AppHelper.Result)
                    {
                        case ResultKey.Player1_Win:
                            switch (player)
                            {
                                case PlayerKey.N1: slice = AppHelper.CharacterAndSlice_to_faceSprites[character, (int)ResultFacialExpressionKey.Win]; break;
                                case PlayerKey.N2: slice = AppHelper.CharacterAndSlice_to_faceSprites[character, (int)ResultFacialExpressionKey.Lose]; break;
                                default: Debug.LogError("未定義のプレイヤー☆"); slice = ""; break;
                            }
                            break;
                        case ResultKey.Player2_Win:
                            switch (player)
                            {
                                case PlayerKey.N1: slice = AppHelper.CharacterAndSlice_to_faceSprites[character, (int)ResultFacialExpressionKey.Lose]; break;
                                case PlayerKey.N2: slice = AppHelper.CharacterAndSlice_to_faceSprites[character, (int)ResultFacialExpressionKey.Win]; break;
                                default: Debug.LogError("未定義のプレイヤー☆"); slice = ""; break;
                            }
                            break;
                        //case Result.Double_KnockOut:
                        //    break;
                        default:
                            // 開発中画面などで☆
                            slice = AppHelper.CharacterAndSlice_to_faceSprites[character, (int)ResultFacialExpressionKey.Win];
                            break;
                    }
                    player_to_face[PlayerKeys.ToArrayIndex(player)].sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals(slice));
                }
            }

            switch (AppHelper.Result)
            {
                case ResultKey.Player1_Win:
                    text.text = ThisSceneStatus.WinMessageByCharacter[(int)AppHelper.UseCharacters[PlayerKey.N1]];
                    break;
                case ResultKey.Player2_Win:
                    text.text = ThisSceneStatus.WinMessageByCharacter[(int)AppHelper.UseCharacters[PlayerKey.N2]];
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
            foreach (var player in PlayerKeys.All)
            {
                // プレイヤーのキー押下状態を確認。
                GamepadStatus state = ApplicationStatus.ReadInput(player);

                // キャンセル以外の 何かボタンを押したらセレクト画面へ遷移
                if (state.Lp.Pressing ||
                    state.Mp.Pressing ||
                    state.Hp.Pressing ||
                    state.Lk.Pressing ||
                    state.Mk.Pressing ||
                    state.Hk.Pressing ||
                    state.Pause.Pressing)
                {
                    SceneManager.LoadScene(AppHelper.sceneToName[(int)SceneKey.Select]);
                }
            }
        }
    }
}
