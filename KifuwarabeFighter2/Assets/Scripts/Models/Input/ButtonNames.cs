namespace Assets.Scripts.Model.Dto.Input
{
    using System.Collections.Generic;

    /// <summary>
    /// [プレイヤー番号, ボタン型]の組みに紐づく名前
    /// </summary>
    public static class ButtonNames
    {
        static ButtonNames()
        {
            Dictionary = new Dictionary<ButtonKey, string>() {
                // Player 1.
                { ButtonKeys.P1Horizontal, "Horizontal" },
                { ButtonKeys.P1Vertical, "Vertical"},
                { ButtonKeys.P1Lp,"P1LightPunch"},
                { ButtonKeys.P1Mp,"P1MediumPunch" },
                { ButtonKeys.P1Hp,"P1HardPunch" },
                { ButtonKeys.P1Lk,"P1LightKick" },
                { ButtonKeys.P1Mk,"P1MediumKick" },
                { ButtonKeys.P1Hk,"P1HardKick"},
                { ButtonKeys.P1Pause,"P1Pause" },
                { ButtonKeys.P1CancelMenu,"Cancel" }, // プレイヤー１のみキャンセル可能☆（＾～＾）
                // Player 2.
                { ButtonKeys.P2Horizontal,"P2Horizontal" },
                { ButtonKeys.P2Vertical, "P2Vertical" },
                { ButtonKeys.P2Lp,"P2LightPunch" },
                { ButtonKeys.P2Mp,"P2MediumPunch" },
                { ButtonKeys.P2Hp,"P2HardPunch" },
                { ButtonKeys.P2Lk,"P2LightKick" },
                { ButtonKeys.P2Mk,"P2MediumKick" },
                { ButtonKeys.P2Hk,"P2HardKick" },
                { ButtonKeys.P2Pause,"P2Pause" },
                { ButtonKeys.P2CancelMenu,"Cancel" }, // これに該当するボタンはありません。
            };
        }

        /// * Main menu [Edit] - [Project Settings...] - [Input].
        /// * Right click `Fire1` and [Delete Array Eelement].
        /// * Same `Fire2`, `Fire3`, `Jump`, `Mouse X`, `Mouse Y`, `Mouse ScrollWheel`, `Horizontal`, `Vertical`, `Fire1`, `Fire2`, `Fire3`, `Jump`.
        /// * Change `Size` 5 to 21.
        /// * Click new items and input `name` text box. Rename to `P1LightPunch`, `P1MediumPunch`, `P1HardPunch`, `P1LightKick`, `P1MediumKick`, `P1HardKick`, `P1Pause`, `P2Horizontal`, `P2Vertical`, `P2LightPunch`, `P2MediumPunch`, `P2HardPunch`, `P2LightKick`, `P2MediumKick`, `P2HardKick`, `P2Pause`,
        /// * Configure input.
        ///     * See: [Conventional Game Input](https://docs.unity3d.com/Manual/ConventionalGameInput.html)
        ///     * `Horizontal`      Negative Button = Left, Positive Button = Right                 Alt Negative Button = h, Alt Positive Button = i, Gravity = 0, Snap = [v], Invert = [ ], Type = Joystick Axis, Axis = X axis, Joy Num = Joystick 1.
        ///     * `Vertical`        Negative Button = Down, Positive Button = Up                    Alt Negative Button = j, Alt Positive Button = k, Gravity = 0, Snap = [v], Invert = [v], Type = Joystick Axis, Axis = Y axis, Joy Num = Joystick 1.
        ///     * `P1LightPunch`                            Positive Button = joystick 1 button 3, Alt Positive Button = a, Gravity = 0, Dead = 0.2, Sensitivity = 1, Type = Key or Mouse Button, Joy Num = Joystick 1.
        ///     * `P1MediumPunch`                           Positive Button = joystick 1 button 2, Alt Positive Button = b, Gravity = 0, Dead = 0.2, Sensitivity = 1, Type = Key or Mouse Button, Joy Num = Joystick 1.
        ///     * `P1HardPunch`                             Positive Button = joystick 1 button 4, Alt Positive Button = c, Gravity = 0, Dead = 0.2, Sensitivity = 1, Type = Key or Mouse Button, Joy Num = Joystick 1.
        ///     * `P1LightKick`                             Positive Button = joystick 1 button 1, Alt Positive Button = d, Gravity = 0, Dead = 0.2, Sensitivity = 1, Type = Key or Mouse Button, Joy Num = Joystick 1.
        ///     * `P1MediumKick`                            Positive Button = joystick 1 button 0, Alt Positive Button = e, Gravity = 0, Dead = 0.2, Sensitivity = 1, Type = Key or Mouse Button, Joy Num = Joystick 1.
        ///     * `P1HardKick`                              Positive Button = joystick 1 button 5, Alt Positive Button = f, Gravity = 0, Dead = 0.2, Sensitivity = 1, Type = Key or Mouse Button, Joy Num = Joystick 1.
        ///     * `P1Pause`                                 Positive Button = joystick 1 button 7, Alt Positive Button = g, Gravity = 0, Dead = 0.2, Sensitivity = 1, Type = Key or Mouse Button, Joy Num = Joystick 1.
        ///     * `P2Horizontal`                            Positive Button = '', Alt Positive Button = '', Gravity = 0, Snap = [v], Invert = [ ], Type = Joystick Axis, Axis = X axis, Joy Num = Joystick 2.
        ///     * `P2Vertical`                              Positive Button = '', Alt Positive Button = '', Gravity = 0, Snap = [v], Invert = [v], Type = Joystick Axis, Axis = Y axis, Joy Num = Joystick 2.
        ///     * `P2LightPunch`                            Positive Button = joystick 2 button 3, Alt Positive Button = '', Gravity = 0, Dead = 0.2, Sensitivity = 1, Type = Key or Mouse Button, Joy Num = Joystick 2.
        ///     * `P2MediumPunch`                           Positive Button = joystick 2 button 2, Alt Positive Button = '', Gravity = 0, Dead = 0.2, Sensitivity = 1, Type = Key or Mouse Button, Joy Num = Joystick 2.
        ///     * `P2HardPunch`                             Positive Button = joystick 2 button 4, Alt Positive Button = '', Gravity = 0, Dead = 0.2, Sensitivity = 1, Type = Key or Mouse Button, Joy Num = Joystick 2.
        ///     * `P2LightKick`                             Positive Button = joystick 2 button 1, Alt Positive Button = '', Gravity = 0, Dead = 0.2, Sensitivity = 1, Type = Key or Mouse Button, Joy Num = Joystick 2.
        ///     * `P2MediumKick`                            Positive Button = joystick 2 button 0, Alt Positive Button = '', Gravity = 0, Dead = 0.2, Sensitivity = 1, Type = Key or Mouse Button, Joy Num = Joystick 2.
        ///     * `P2HardKick`                              Positive Button = joystick 2 button 5, Alt Positive Button = '', Gravity = 0, Dead = 0.2, Sensitivity = 1, Type = Key or Mouse Button, Joy Num = Joystick 2.
        ///     * `P2Pause`                                 Positive Button = joystick 2 button 7, Alt Positive Button = '', Gravity = 0, Dead = 0.2, Sensitivity = 1, Type = Key or Mouse Button, Joy Num = Joystick 2.
        ///     
        /// [player,button]
        /// 内部的には　プレイヤー１はP0、プレイヤー２はP1 だぜ☆（＾▽＾）
        /// 入力類は、コンフィグ画面でユーザーの目に触れる☆（＾～＾）
        /// ユーザーの目に見えるところでは 1スタート、内部的には 0スタートだぜ☆（＾▽＾）
        public static Dictionary<ButtonKey, string> Dictionary;
    }
}
