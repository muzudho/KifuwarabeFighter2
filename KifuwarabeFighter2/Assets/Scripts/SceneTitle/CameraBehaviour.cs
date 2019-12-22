namespace SceneTitle
{
    using Assets.Scripts.Model.Dto.Input;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class CameraBehaviour : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            // キャンセル以外の 何かボタンを押したらセレクト画面へ遷移
            foreach (var player in PlayerIndexes.All)
            {
                // どのキーが押されたか、ここでテストも兼ねます。
                var Lp = Input.GetButton(InputNames.Dictionary[new InputIndex(player, ButtonIndex.LightPunch)]);
                var Mp = Input.GetButton(InputNames.Dictionary[new InputIndex(player, ButtonIndex.MediumPunch)]);
                var Hp = Input.GetButton(InputNames.Dictionary[new InputIndex(player, ButtonIndex.HardPunch)]);
                var Lk = Input.GetButton(InputNames.Dictionary[new InputIndex(player, ButtonIndex.LightKick)]);
                var Mk = Input.GetButton(InputNames.Dictionary[new InputIndex(player, ButtonIndex.MediumKick)]);
                var Hk = Input.GetButton(InputNames.Dictionary[new InputIndex(player, ButtonIndex.HardKick)]);
                var Pause = Input.GetButton(InputNames.Dictionary[new InputIndex(player, ButtonIndex.Pause)]);
                // Input.GetButton(InputNames.Dictionary[new InputIndex(PlayerIndex.Player1, ButtonIndex.CancelMenu)])

                if (Lp ||
                    Mp ||
                    Hp ||
                    Lk ||
                    Mk ||
                    Hk ||
                    Pause
                )
                {
                    Debug.Log($"Push key. human={player} Lp={Lp} Mp={Mp} Hp={Hp} Lk={Lk} Mk={Mk} Hk={Hk}");
                    CommonScript.computerFlags[player] = false;

                    // * Configure scene.
                    //     * Click main menu [File] - [Build Settings...].
                    //     * Double click [Assets] - [Scenes] - [Title] in project view.
                    //     * Click [Add Open Scenes] button.
                    //     * Double click [Assets] - [Scenes] - [Select] in project view.
                    //     * Click [Add Open Scenes] button.
                    //     * Double click [Assets] - [Scenes] - [Main] in project view.
                    //     * Click [Add Open Scenes] button.
                    //     * Double click [Assets] - [Scenes] - [Result] in project view.
                    //     * Click [Add Open Scenes] button.
                    //     * Right click `Scenes/SampleScene` from `Build Settings/Scene In Build`. and Click [Remove Selection].
                    SceneManager.LoadScene(CommonScript.Scene_to_name[(int)SceneIndex.Select]);
                }
            }
        }
    }
}
