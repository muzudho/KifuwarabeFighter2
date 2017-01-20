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
                    Input.GetButton(CommonScript.PlayerAndInput_to_inputName[iPlayer, (int)InputIndex.LightPunch]) ||
                    Input.GetButton(CommonScript.PlayerAndInput_to_inputName[iPlayer, (int)InputIndex.MediumPunch]) ||
                    Input.GetButton(CommonScript.PlayerAndInput_to_inputName[iPlayer, (int)InputIndex.HardPunch]) ||
                    Input.GetButton(CommonScript.PlayerAndInput_to_inputName[iPlayer, (int)InputIndex.LightKick]) ||
                    Input.GetButton(CommonScript.PlayerAndInput_to_inputName[iPlayer, (int)InputIndex.MediumKick]) ||
                    Input.GetButton(CommonScript.PlayerAndInput_to_inputName[iPlayer, (int)InputIndex.HardKick]) ||
                    Input.GetButton(CommonScript.PlayerAndInput_to_inputName[iPlayer, (int)InputIndex.Pause]) ||
                    Input.GetButton(CommonScript.INPUT_10_CA)
                )
                {
                    CommonScript.Player_to_computer[iPlayer] = false;
                    SceneManager.LoadScene(CommonScript.Scene_to_name[(int)SceneIndex.Select]);
                }
            }

        }
    }
}
