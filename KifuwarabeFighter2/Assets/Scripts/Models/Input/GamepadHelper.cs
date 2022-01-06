namespace Assets.Scripts.Model.Dao.Input
{
    using Assets.Scripts.Model.Dto.Input;
    using UnityEngine;

    public static class GamepadHelper
    {
        public static void UpdateState(PlayerKey player, GamepadStatus gamepad)
        {
            //左キー: -1、右キー: 1
            gamepad.LeverX = Input.GetAxisRaw(gamepad.HorizontalName);
            // 下キー: -1、上キー: 1 (Input設定でVerticalの入力にはInvertをチェックしておく）
            gamepad.LeverY = Input.GetAxisRaw(ButtonNames.Dictionary[new ButtonKey(player, ButtonType.Vertical)]);
            gamepad.Lp.set(Input.GetButton(gamepad.LpName), Input.GetButtonDown(gamepad.LpName), Input.GetButtonUp(gamepad.LpName));
            gamepad.Mp.set(Input.GetButton(gamepad.MpName), Input.GetButtonDown(gamepad.MpName), Input.GetButtonUp(gamepad.MpName));
            gamepad.Hp.set(Input.GetButton(gamepad.HpName), Input.GetButtonDown(gamepad.HpName), Input.GetButtonUp(gamepad.HpName));
            gamepad.Lk.set(Input.GetButton(gamepad.LkName), Input.GetButtonDown(gamepad.LkName), Input.GetButtonUp(gamepad.LkName));
            gamepad.Mk.set(Input.GetButton(gamepad.MkName), Input.GetButtonDown(gamepad.MkName), Input.GetButtonUp(gamepad.MkName));
            gamepad.Hk.set(Input.GetButton(gamepad.HkName), Input.GetButtonDown(gamepad.HkName), Input.GetButtonUp(gamepad.HkName));
            gamepad.Pause.set(Input.GetButton(gamepad.PauseName), Input.GetButtonDown(gamepad.PauseName), Input.GetButtonUp(gamepad.PauseName));

            // プレイヤー１のみキャンセル可能
            if (player == PlayerKey.N1)
            {
                gamepad.CancelMenu.set(Input.GetButton(gamepad.CancelMenuName), Input.GetButtonDown(gamepad.CancelMenuName), Input.GetButtonUp(gamepad.CancelMenuName));
            }

            // プレイヤー１だけテストで表示します。
            if (PlayerKey.N1 == player)
            {
                Debug.Log($"player1 input {gamepad.ToDisplay()}");
            }
        }
    }
}
