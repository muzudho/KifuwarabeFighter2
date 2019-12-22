namespace Assets.Scripts.Model.Dao.Input
{
    using Assets.Scripts.Model.Dto.Input;
    using UnityEngine;

    public static class PlayerInputDao
    {
        public static void UpdateState(PlayerIndex player, InputStateDto state)
        {
            //左キー: -1、右キー: 1
            state.LeverX = Input.GetAxisRaw(InputNames.Dictionary[new InputIndex(player, ButtonIndex.Horizontal)]);
            // 下キー: -1、上キー: 1 (Input設定でVerticalの入力にはInvertをチェックしておく）
            state.LeverY = Input.GetAxisRaw(InputNames.Dictionary[new InputIndex(player, ButtonIndex.Vertical)]);
            state.Lp.set(
                Input.GetButton(InputNames.Dictionary[new InputIndex(player, ButtonIndex.LightPunch)]),
                Input.GetButtonDown(InputNames.Dictionary[new InputIndex(player, ButtonIndex.LightPunch)]),
                Input.GetButtonUp(InputNames.Dictionary[new InputIndex(player, ButtonIndex.LightPunch)]));

            state.Mp.set(
                Input.GetButton(InputNames.Dictionary[new InputIndex(player, ButtonIndex.MediumPunch)]),
                Input.GetButtonDown(InputNames.Dictionary[new InputIndex(player, ButtonIndex.MediumPunch)]),
                Input.GetButtonUp(InputNames.Dictionary[new InputIndex(player, ButtonIndex.MediumPunch)]));

            state.Hp.set(
                Input.GetButton(InputNames.Dictionary[new InputIndex(player, ButtonIndex.HardPunch)]),
                Input.GetButtonDown(InputNames.Dictionary[new InputIndex(player, ButtonIndex.HardPunch)]),
                Input.GetButtonUp(InputNames.Dictionary[new InputIndex(player, ButtonIndex.HardPunch)]));

            state.Lk.set(
                Input.GetButton(InputNames.Dictionary[new InputIndex(player, ButtonIndex.LightKick)]),
                Input.GetButtonDown(InputNames.Dictionary[new InputIndex(player, ButtonIndex.LightKick)]),
                Input.GetButtonUp(InputNames.Dictionary[new InputIndex(player, ButtonIndex.LightKick)]));

            state.Mk.set(
                Input.GetButton(InputNames.Dictionary[new InputIndex(player, ButtonIndex.MediumKick)]),
                Input.GetButtonDown(InputNames.Dictionary[new InputIndex(player, ButtonIndex.MediumKick)]),
                Input.GetButtonUp(InputNames.Dictionary[new InputIndex(player, ButtonIndex.MediumKick)]));

            state.Hk.set(
                Input.GetButton(InputNames.Dictionary[new InputIndex(player, ButtonIndex.HardKick)]),
                Input.GetButtonDown(InputNames.Dictionary[new InputIndex(player, ButtonIndex.HardKick)]),
                Input.GetButtonUp(InputNames.Dictionary[new InputIndex(player, ButtonIndex.HardKick)]));

            state.Pause.set(
                Input.GetButton(InputNames.Dictionary[new InputIndex(player, ButtonIndex.Pause)]),
                Input.GetButtonDown(InputNames.Dictionary[new InputIndex(player, ButtonIndex.Pause)]),
                Input.GetButtonUp(InputNames.Dictionary[new InputIndex(player, ButtonIndex.Pause)]));

            // プレイヤー１のみキャンセル可能
            if (player == PlayerIndex.Player1)
            {
                state.CancelMenu.set(
                    Input.GetButton(InputNames.Dictionary[InputIndexes.P1CancelMenu]),
                    Input.GetButtonDown(InputNames.Dictionary[InputIndexes.P1CancelMenu]),
                    Input.GetButtonUp(InputNames.Dictionary[InputIndexes.P1CancelMenu]));
            }

            // プレイヤー１だけテストで表示します。
            if (PlayerIndex.Player1 == player)
            {
                Debug.Log($"player1 input leverX={state.LeverX} Y={state.LeverY} Lp={state.Lp.toDisplay()} Mp={state.Mp.toDisplay()} Hp={state.Hp.toDisplay()} Lk={state.Lk.toDisplay()} Mk={state.Mk.toDisplay()} Hk={state.Hk.toDisplay()} Pause={state.Pause.toDisplay()} CancelMenu={state.CancelMenu.toDisplay()}");
            }
        }
    }
}
