using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleCameraScript : MonoBehaviour {

	// Update is called once per frame
	void Update () {

        // 何かボタンを押したらセレクト画面へ遷移
        for (int iPlayer = (int)PlayerIndex.Player1; iPlayer < (int)PlayerIndex.Num; iPlayer++)
        {
            if (
                Input.GetButton(CommonScript.PlayerAndButton_To_ButtonName[iPlayer, (int)ButtonIndex.LightPunch]) ||
                Input.GetButton(CommonScript.PlayerAndButton_To_ButtonName[iPlayer, (int)ButtonIndex.MediumPunch]) ||
                Input.GetButton(CommonScript.PlayerAndButton_To_ButtonName[iPlayer, (int)ButtonIndex.HardPunch]) ||
                Input.GetButton(CommonScript.PlayerAndButton_To_ButtonName[iPlayer, (int)ButtonIndex.LightKick]) ||
                Input.GetButton(CommonScript.PlayerAndButton_To_ButtonName[iPlayer, (int)ButtonIndex.MediumKick]) ||
                Input.GetButton(CommonScript.PlayerAndButton_To_ButtonName[iPlayer, (int)ButtonIndex.HardKick]) ||
                Input.GetButton(CommonScript.PlayerAndButton_To_ButtonName[iPlayer, (int)ButtonIndex.Pause]) ||
                Input.GetButton(CommonScript.BUTTON_10_CA)
            )
            {
                CommonScript.Player_To_Computer[iPlayer] = false;
                SceneManager.LoadScene(CommonScript.SCENE_SELECT);
            }
        }

    }
}
