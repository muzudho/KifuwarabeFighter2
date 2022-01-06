namespace Assets.Scripts.Models.Input
{
    using UnityEngine;

    public static class GamepadHelper
    {
        public static void UpdateState(Player player, GamepadStatus gamepad)
        {
            //左キー: -1、右キー: 1
            gamepad.HorizontalLever.Value = Input.GetAxisRaw(gamepad.HorizontalLever.Name);
            // 下キー: -1、上キー: 1 (Input設定でVerticalの入力にはInvertをチェックしておく）
            gamepad.VerticalLever.Value = Input.GetAxisRaw(ButtonNames.Dictionary[new ButtonKey(player, ButtonType.Vertical)]);
            gamepad.Lp.Set(Input.GetButton(gamepad.Lp.Name), Input.GetButtonDown(gamepad.Lp.Name), Input.GetButtonUp(gamepad.Lp.Name));
            gamepad.Mp.Set(Input.GetButton(gamepad.Mp.Name), Input.GetButtonDown(gamepad.Mp.Name), Input.GetButtonUp(gamepad.Mp.Name));
            gamepad.Hp.Set(Input.GetButton(gamepad.Hp.Name), Input.GetButtonDown(gamepad.Hp.Name), Input.GetButtonUp(gamepad.Hp.Name));
            gamepad.Lk.Set(Input.GetButton(gamepad.Lk.Name), Input.GetButtonDown(gamepad.Lk.Name), Input.GetButtonUp(gamepad.Lk.Name));
            gamepad.Mk.Set(Input.GetButton(gamepad.Mk.Name), Input.GetButtonDown(gamepad.Mk.Name), Input.GetButtonUp(gamepad.Mk.Name));
            gamepad.Hk.Set(Input.GetButton(gamepad.Hk.Name), Input.GetButtonDown(gamepad.Hk.Name), Input.GetButtonUp(gamepad.Hk.Name));
            gamepad.Pause.Set(Input.GetButton(gamepad.Pause.Name), Input.GetButtonDown(gamepad.Pause.Name), Input.GetButtonUp(gamepad.Pause.Name));

            // プレイヤー１のみキャンセル可能
            if (player == Player.N1)
            {
                gamepad.CancelMenu.Set(Input.GetButton(gamepad.CancelMenu.Name), Input.GetButtonDown(gamepad.CancelMenu.Name), Input.GetButtonUp(gamepad.CancelMenu.Name));
            }

            // プレイヤー１だけテストで表示します。
            if (Player.N1 == player)
            {
                Debug.Log($"player1 input {gamepad.ToDisplay()}");
            }
        }
    }
}
