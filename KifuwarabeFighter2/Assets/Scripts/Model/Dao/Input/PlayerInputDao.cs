namespace Assets.Scripts.Model.Dao.Input
{
    using Assets.Scripts.Model.Dto.Input;
    using UnityEngine;

    public static class PlayerInputDao
    {
        public static void UpdateState(PlayerIndex player, InputStateDto state)
        {
            //左キー: -1、右キー: 1
            state.leverX = Input.GetAxisRaw(InputNames.Dictionary[new InputIndex(player, ButtonIndex.Horizontal)]);
            // 下キー: -1、上キー: 1 (Input設定でVerticalの入力にはInvertをチェックしておく）
            state.leverY = Input.GetAxisRaw(InputNames.Dictionary[new InputIndex(player, ButtonIndex.Vertical)]);
            state.pressingLP = Input.GetButton(InputNames.Dictionary[new InputIndex(player, ButtonIndex.LightPunch)]);

            // プレイヤー１の 入力テストをしたいとき。
            if (PlayerIndex.Player1 == player)
            {
                Debug.Log($"player1 input leverX={state.leverX} leverY={state.leverY} LP={state.pressingLP}");
            }

            state.pressingMP = Input.GetButton(InputNames.Dictionary[new InputIndex(player, ButtonIndex.MediumPunch)]);
            state.pressingHP = Input.GetButton(InputNames.Dictionary[new InputIndex(player, ButtonIndex.HardPunch)]);
            state.pressingLK = Input.GetButton(InputNames.Dictionary[new InputIndex(player, ButtonIndex.LightKick)]);
            state.pressingMK = Input.GetButton(InputNames.Dictionary[new InputIndex(player, ButtonIndex.MediumKick)]);
            state.pressingHK = Input.GetButton(InputNames.Dictionary[new InputIndex(player, ButtonIndex.HardKick)]);
            state.pressingPA = Input.GetButton(InputNames.Dictionary[new InputIndex(player, ButtonIndex.Pause)]);

            if (player == PlayerIndex.Player1)
            {
                state.pressingCancelMenu = Input.GetButton(InputNames.Dictionary[InputIndexes.P1CancelMenu]); // プレイヤー１のみキャンセル可能
            }

            state.buttonDownLP = Input.GetButtonDown(InputNames.Dictionary[new InputIndex(player, ButtonIndex.LightPunch)]);
            state.buttonDownMP = Input.GetButtonDown(InputNames.Dictionary[new InputIndex(player, ButtonIndex.MediumPunch)]);
            state.buttonDownHP = Input.GetButtonDown(InputNames.Dictionary[new InputIndex(player, ButtonIndex.HardPunch)]);
            state.buttonDownLK = Input.GetButtonDown(InputNames.Dictionary[new InputIndex(player, ButtonIndex.LightKick)]);
            state.buttonDownMK = Input.GetButtonDown(InputNames.Dictionary[new InputIndex(player, ButtonIndex.MediumKick)]);
            state.buttonDownHK = Input.GetButtonDown(InputNames.Dictionary[new InputIndex(player, ButtonIndex.HardKick)]);
            state.buttonDownPA = Input.GetButtonDown(InputNames.Dictionary[new InputIndex(player, ButtonIndex.Pause)]);
            state.buttonUpLP = Input.GetButtonUp(InputNames.Dictionary[new InputIndex(player, ButtonIndex.LightPunch)]);
            state.buttonUpMP = Input.GetButtonUp(InputNames.Dictionary[new InputIndex(player, ButtonIndex.MediumPunch)]);
            state.buttonUpHP = Input.GetButtonUp(InputNames.Dictionary[new InputIndex(player, ButtonIndex.HardPunch)]);
            state.buttonUpLK = Input.GetButtonUp(InputNames.Dictionary[new InputIndex(player, ButtonIndex.LightKick)]);
            state.buttonUpMK = Input.GetButtonUp(InputNames.Dictionary[new InputIndex(player, ButtonIndex.MediumKick)]);
            state.buttonUpHK = Input.GetButtonUp(InputNames.Dictionary[new InputIndex(player, ButtonIndex.HardKick)]);
            state.buttonUpPA = Input.GetButtonUp(InputNames.Dictionary[new InputIndex(player, ButtonIndex.Pause)]);
        }
    }
}
