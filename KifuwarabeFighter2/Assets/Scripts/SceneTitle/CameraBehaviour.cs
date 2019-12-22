namespace SceneTitle
{
    using Assets.Scripts.Model.Dto.Input;
    using Assets.Scripts.Model.Dto.Scene.Common;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class CameraBehaviour : MonoBehaviour
    {

        // Update is called once per frame
        void Update()
        {

            // 何かボタンを押したらセレクト画面へ遷移
            foreach (var playerIndex in PlayerIndexes.All)
            {
                if (
                    Input.GetButton(CommonInput.InputNameDictionary[new InputIndex(playerIndex, ButtonIndex.LightPunch)]) ||
                    Input.GetButton(CommonInput.InputNameDictionary[new InputIndex(playerIndex, ButtonIndex.MediumPunch)]) ||
                    Input.GetButton(CommonInput.InputNameDictionary[new InputIndex(playerIndex, ButtonIndex.HardPunch)]) ||
                    Input.GetButton(CommonInput.InputNameDictionary[new InputIndex(playerIndex, ButtonIndex.LightKick)]) ||
                    Input.GetButton(CommonInput.InputNameDictionary[new InputIndex(playerIndex, ButtonIndex.MediumKick)]) ||
                    Input.GetButton(CommonInput.InputNameDictionary[new InputIndex(playerIndex, ButtonIndex.HardKick)]) ||
                    Input.GetButton(CommonInput.InputNameDictionary[new InputIndex(playerIndex, ButtonIndex.Pause)]) ||
                    Input.GetButton(CommonInput.Input10Ca)
                )
                {
                    CommonScript.Player_to_computer[(int)playerIndex] = false;

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
