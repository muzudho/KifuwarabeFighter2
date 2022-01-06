namespace Assets.Scripts.Model.Dao.Input
{
    using Assets.Scripts.Model.Dto.Input;
    using UnityEngine;

    public static class PlayerInputDao
    {
        public static void UpdateState(PlayerNum player, InputStateDto state)
        {
            //左キー: -1、右キー: 1
            state.LeverX = Input.GetAxisRaw(state.HorizontalName);
            // 下キー: -1、上キー: 1 (Input設定でVerticalの入力にはInvertをチェックしておく）
            state.LeverY = Input.GetAxisRaw(InputNames.Dictionary[new PlayerButtonNum(player, ButtonNum.Vertical)]);
            state.Lp.set(Input.GetButton(state.LpName), Input.GetButtonDown(state.LpName), Input.GetButtonUp(state.LpName));
            state.Mp.set(Input.GetButton(state.MpName), Input.GetButtonDown(state.MpName), Input.GetButtonUp(state.MpName));
            state.Hp.set(Input.GetButton(state.HpName), Input.GetButtonDown(state.HpName), Input.GetButtonUp(state.HpName));
            state.Lk.set(Input.GetButton(state.LkName), Input.GetButtonDown(state.LkName), Input.GetButtonUp(state.LkName));
            state.Mk.set(Input.GetButton(state.MkName), Input.GetButtonDown(state.MkName), Input.GetButtonUp(state.MkName));
            state.Hk.set(Input.GetButton(state.HkName), Input.GetButtonDown(state.HkName), Input.GetButtonUp(state.HkName));
            state.Pause.set(Input.GetButton(state.PauseName), Input.GetButtonDown(state.PauseName), Input.GetButtonUp(state.PauseName));

            // プレイヤー１のみキャンセル可能
            if (player == PlayerNum.N1)
            {
                state.CancelMenu.set(Input.GetButton(state.CancelMenuName), Input.GetButtonDown(state.CancelMenuName), Input.GetButtonUp(state.CancelMenuName));
            }

            // プレイヤー１だけテストで表示します。
            if (PlayerNum.N1 == player)
            {
                Debug.Log($"player1 input {state.ToDisplay()}");
            }
        }
    }
}
