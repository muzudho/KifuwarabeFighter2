using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleCameraScript : MonoBehaviour {

	// Update is called once per frame
	void Update () {

        // 何かボタンを押したらセレクト画面へ遷移
        if (Input.GetButton(CommonScript.BUTTON_03_P0_LP) ||
            Input.GetButton(CommonScript.BUTTON_04_P0_MP) ||
            Input.GetButton(CommonScript.BUTTON_05_P0_HP) ||
            Input.GetButton(CommonScript.BUTTON_06_P0_LK) ||
            Input.GetButton(CommonScript.BUTTON_07_P0_MK) ||
            Input.GetButton(CommonScript.BUTTON_08_P0_HK) ||
            Input.GetButton(CommonScript.BUTTON_09_P0_PA) ||
            Input.GetButton(CommonScript.BUTTON_10_CA) ||
            Input.GetButton(CommonScript.BUTTON_13_P1_LP) ||
            Input.GetButton(CommonScript.BUTTON_14_P1_MP) ||
            Input.GetButton(CommonScript.BUTTON_15_P1_HP) ||
            Input.GetButton(CommonScript.BUTTON_16_P1_LK) ||
            Input.GetButton(CommonScript.BUTTON_17_P1_MK) ||
            Input.GetButton(CommonScript.BUTTON_18_P1_HK) ||
            Input.GetButton(CommonScript.BUTTON_19_P1_PA)
        )
            {
            SceneManager.LoadScene("Select");
        }
		
	}
}
