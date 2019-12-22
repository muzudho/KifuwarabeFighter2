using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneTitle
{
    public class Title_CameraScript : MonoBehaviour
    {

        // Update is called once per frame
        void Update()
        {

            // 何かボタンを押したらセレクト画面へ遷移
            for (int iPlayer = (int)PlayerIndex.Player1; iPlayer < (int)PlayerIndex.Num; iPlayer++)
            {
                if (
                    Input.GetButton(CommonInput.PlayerAndInput_to_inputName[iPlayer, (int)InputIndex.LightPunch]) ||
                    Input.GetButton(CommonInput.PlayerAndInput_to_inputName[iPlayer, (int)InputIndex.MediumPunch]) ||
                    Input.GetButton(CommonInput.PlayerAndInput_to_inputName[iPlayer, (int)InputIndex.HardPunch]) ||
                    Input.GetButton(CommonInput.PlayerAndInput_to_inputName[iPlayer, (int)InputIndex.LightKick]) ||
                    Input.GetButton(CommonInput.PlayerAndInput_to_inputName[iPlayer, (int)InputIndex.MediumKick]) ||
                    Input.GetButton(CommonInput.PlayerAndInput_to_inputName[iPlayer, (int)InputIndex.HardKick]) ||
                    Input.GetButton(CommonInput.PlayerAndInput_to_inputName[iPlayer, (int)InputIndex.Pause]) ||
                    Input.GetButton(CommonInput.INPUT_10_CA)
                )
                {
                    CommonScript.Player_to_computer[iPlayer] = false;

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
