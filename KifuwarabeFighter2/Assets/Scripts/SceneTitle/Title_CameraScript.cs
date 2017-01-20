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
                    Input.GetButton(CommonScript.PlayerAndInput_To_InputName[iPlayer, (int)InputIndex.LightPunch]) ||
                    Input.GetButton(CommonScript.PlayerAndInput_To_InputName[iPlayer, (int)InputIndex.MediumPunch]) ||
                    Input.GetButton(CommonScript.PlayerAndInput_To_InputName[iPlayer, (int)InputIndex.HardPunch]) ||
                    Input.GetButton(CommonScript.PlayerAndInput_To_InputName[iPlayer, (int)InputIndex.LightKick]) ||
                    Input.GetButton(CommonScript.PlayerAndInput_To_InputName[iPlayer, (int)InputIndex.MediumKick]) ||
                    Input.GetButton(CommonScript.PlayerAndInput_To_InputName[iPlayer, (int)InputIndex.HardKick]) ||
                    Input.GetButton(CommonScript.PlayerAndInput_To_InputName[iPlayer, (int)InputIndex.Pause]) ||
                    Input.GetButton(CommonScript.INPUT_10_CA)
                )
                {
                    CommonScript.Player_To_Computer[iPlayer] = false;
                    SceneManager.LoadScene(CommonScript.scene_to_name[(int)SceneIndex.Select]);
                }
            }

        }
    }
}
