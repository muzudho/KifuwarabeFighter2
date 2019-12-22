namespace Assets.Scripts.Model.Dto.Scene.Common
{
    using System.Collections.Generic;
    using Assets.Scripts.Model.Dto.Input;
    using UnityEngine;

    /// <summary>
    /// 共有する入力関連はこちらに。
    /// </summary>
    public abstract class CommonInput
    {
        static CommonInput()
        {
            playerToInput = new[] { new InputStateDto(), new InputStateDto() };
        }

        /// * Main menu [Edit] - [Project Settings...] - [Input].
        /// * Right click `Fire1` and [Delete Array Eelement].
        /// * Same `Fire2`, `Fire3`, `Jump`, `Mouse X`, `Mouse Y`, `Mouse ScrollWheel`, `Horizontal`, `Vertical`, `Fire1`, `Fire2`, `Fire3`, `Jump`.
        /// * Change `Size` 5 to 21.
        /// * Click new items and input `name` text box. Rename to `P1LightPunch`, `P1MediumPunch`, `P1HardPunch`, `P1LightKick`, `P1MediumKick`, `P1HardKick`, `P1Pause`, `P2Horizontal`, `P2Vertical`, `P2LightPunch`, `P2MediumPunch`, `P2HardPunch`, `P2LightKick`, `P2MediumKick`, `P2HardKick`, `P2Pause`,
        /// * Configure input.
        ///     * `Horizontal`      Negative Button = Left, Positive Button = Right                 Alt Negative Button = h, Alt Positive Button = i, Gravity = 0, Type = Joystick Axis, Joy Num = Joystick 1.
        ///     * `Vertical`        Negative Button = Down, Positive Button = Up                    Alt Negative Button = j, Alt Positive Button = k, Gravity = 0, Type = Joystick Axis, Joy Num = Joystick 1.
        ///     * `P1LightPunch`                            Positive Button = joystick button 3, Alt Positive Button = a, Gravity = 0, Dead = 0.2, Sensitivity = 1, Type = Key or Mouse Button, Joy Num = Joystick 1.
        ///     * `P1MediumPunch`                           Positive Button = joystick button 2, Alt Positive Button = b, Gravity = 0, Dead = 0.2, Sensitivity = 1, Type = Key or Mouse Button, Joy Num = Joystick 1.
        ///     * `P1HardPunch`                             Positive Button = joystick button 4, Alt Positive Button = c, Gravity = 0, Dead = 0.2, Sensitivity = 1, Type = Key or Mouse Button, Joy Num = Joystick 1.
        ///     * `P1LightKick`                             Positive Button = joystick button 1, Alt Positive Button = d, Gravity = 0, Dead = 0.2, Sensitivity = 1, Type = Key or Mouse Button, Joy Num = Joystick 1.
        ///     * `P1MediumKick`                            Positive Button = joystick button 0, Alt Positive Button = e, Gravity = 0, Dead = 0.2, Sensitivity = 1, Type = Key or Mouse Button, Joy Num = Joystick 1.
        ///     * `P1HardKick`                              Positive Button = joystick button 5, Alt Positive Button = f, Gravity = 0, Dead = 0.2, Sensitivity = 1, Type = Key or Mouse Button, Joy Num = Joystick 1.
        ///     * `P1Pause`                                 Positive Button = joystick button 7, Alt Positive Button = g, Gravity = 0, Dead = 0.2, Sensitivity = 1, Type = Key or Mouse Button, Joy Num = Joystick 1.
        ///     * `P2Horizontal`                            Positive Button = '', Alt Positive Button = '', Gravity = 0, Type = Joystick Axis, Joy Num = Joystick 2.
        ///     * `P2Vertical`                              Positive Button = '', Alt Positive Button = '', Gravity = 0, Type = Joystick Axis, Joy Num = Joystick 2.
        ///     * `P2LightPunch`                            Positive Button = joystick button 3, Alt Positive Button = '', Gravity = 0, Dead = 0.2, Sensitivity = 1, Type = Key or Mouse Button, Joy Num = Joystick 2.
        ///     * `P2MediumPunch`                           Positive Button = joystick button 2, Alt Positive Button = '', Gravity = 0, Dead = 0.2, Sensitivity = 1, Type = Key or Mouse Button, Joy Num = Joystick 2.
        ///     * `P2HardPunch`                             Positive Button = joystick button 4, Alt Positive Button = '', Gravity = 0, Dead = 0.2, Sensitivity = 1, Type = Key or Mouse Button, Joy Num = Joystick 2.
        ///     * `P2LightKick`                             Positive Button = joystick button 1, Alt Positive Button = '', Gravity = 0, Dead = 0.2, Sensitivity = 1, Type = Key or Mouse Button, Joy Num = Joystick 2.
        ///     * `P2MediumKick`                            Positive Button = joystick button 0, Alt Positive Button = '', Gravity = 0, Dead = 0.2, Sensitivity = 1, Type = Key or Mouse Button, Joy Num = Joystick 2.
        ///     * `P2HardKick`                              Positive Button = joystick button 5, Alt Positive Button = '', Gravity = 0, Dead = 0.2, Sensitivity = 1, Type = Key or Mouse Button, Joy Num = Joystick 2.
        ///     * `P2Pause`                                 Positive Button = joystick button 7, Alt Positive Button = '', Gravity = 0, Dead = 0.2, Sensitivity = 1, Type = Key or Mouse Button, Joy Num = Joystick 2.
        ///     
        /// [player,button]
        /// 内部的には　プレイヤー１はP0、プレイヤー２はP1 だぜ☆（＾▽＾）
        /// 入力類は、コンフィグ画面でユーザーの目に触れる☆（＾～＾）
        /// ユーザーの目に見えるところでは 1スタート、内部的には 0スタートだぜ☆（＾▽＾）
        public static Dictionary<InputIndex,string> InputNameDictionary = new Dictionary<InputIndex, string>() {
            // Player 1.
            { new InputIndex(PlayerIndex.Player1, ButtonIndex.Horizontal), "Horizontal" },
            { new InputIndex(PlayerIndex.Player1, ButtonIndex.Vertical), "Vertical"},
            { new InputIndex(PlayerIndex.Player1, ButtonIndex.LightPunch) ,"P1LightPunch"},
            { new InputIndex(PlayerIndex.Player1, ButtonIndex.MediumPunch),"P1MediumPunch" },
            { new InputIndex(PlayerIndex.Player1, ButtonIndex.HardPunch),"P1HardPunch" },
            { new InputIndex(PlayerIndex.Player1, ButtonIndex.LightKick),"P1LightKick" },
            { new InputIndex(PlayerIndex.Player1, ButtonIndex.MediumKick),"P1MediumKick" },
            { new InputIndex(PlayerIndex.Player1, ButtonIndex.HardKick) ,"P1HardKick"},
            { new InputIndex(PlayerIndex.Player1, ButtonIndex.Pause),"P1Pause" },
            // Player 2.
            { new InputIndex(PlayerIndex.Player2, ButtonIndex.Horizontal),"P2Horizontal" },
            { new InputIndex(PlayerIndex.Player2, ButtonIndex.Vertical), "P2Vertical" },
            { new InputIndex(PlayerIndex.Player2, ButtonIndex.LightPunch),"P2LightPunch" },
            { new InputIndex(PlayerIndex.Player2, ButtonIndex.MediumPunch),"P2MediumPunch" },
            { new InputIndex(PlayerIndex.Player2, ButtonIndex.HardPunch),"P2HardPunch" },
            { new InputIndex(PlayerIndex.Player2, ButtonIndex.LightKick),"P2LightKick" },
            { new InputIndex(PlayerIndex.Player2, ButtonIndex.MediumKick),"P2MediumKick" },
            { new InputIndex(PlayerIndex.Player2, ButtonIndex.HardKick),"P2HardKick" },
            { new InputIndex(PlayerIndex.Player2, ButtonIndex.Pause),"P2Pause" },
        };
        public const string Input10Ca = "Cancel";

        /// <summary>
        /// あるプレイヤーのキー入力状態。
        /// </summary>
        public struct InputStateDto
        {
            public float leverX;
            public float leverY;
            public bool pressingLP;
            public bool pressingMP;
            public bool pressingHP;
            public bool pressingLK;
            public bool pressingMK;
            public bool pressingHK;
            public bool pressingPA;
            public bool pressingCA;
            public bool buttonDownLP;
            public bool buttonDownMP;
            public bool buttonDownHP;
            public bool buttonDownLK;
            public bool buttonDownMK;
            public bool buttonDownHK;
            public bool buttonDownPA;
            public bool buttonUpLP;
            public bool buttonUpMP;
            public bool buttonUpHP;
            public bool buttonUpLK;
            public bool buttonUpMK;
            public bool buttonUpHK;
            public bool buttonUpPA;

            //public PlayerInput(
            //    float leverX,
            //    float leverY,
            //    bool pressingLP,
            //    bool pressingMP,
            //    bool pressingHP,
            //    bool pressingLK,
            //    bool pressingMK,
            //    bool pressingHK,
            //    bool pressingPA,
            //    bool pressingCA,
            //    bool buttonDownLP,
            //    bool buttonDownMP,
            //    bool buttonDownHP,
            //    bool buttonDownLK,
            //    bool buttonDownMK,
            //    bool buttonDownHK,
            //    bool buttonDownPA,
            //    bool buttonUpLP,
            //    bool buttonUpMP,
            //    bool buttonUpHP,
            //    bool buttonUpLK,
            //    bool buttonUpMK,
            //    bool buttonUpHK,
            //    bool buttonUpPA
            //    )
            //{
            //    this.leverX = leverX;
            //    this.leverY = leverY;
            //    this.pressingLP = pressingLP;
            //    this.pressingMP = pressingMP;
            //    this.pressingHP = pressingHP;
            //    this.pressingLK = pressingLK;
            //    this.pressingMK = pressingMK;
            //    this.pressingHK = pressingHK;
            //    this.pressingPA = pressingPA;
            //    this.pressingCA = pressingCA;
            //    this.buttonDownLP = buttonDownLP;
            //    this.buttonDownMP = buttonDownMP;
            //    this.buttonDownHP = buttonDownHP;
            //    this.buttonDownLK = buttonDownLK;
            //    this.buttonDownMK = buttonDownMK;
            //    this.buttonDownHK = buttonDownHK;
            //    this.buttonDownPA = buttonDownPA;
            //    this.buttonUpLP = buttonUpLP;
            //    this.buttonUpMP = buttonUpMP;
            //    this.buttonUpHP = buttonUpHP;
            //    this.buttonUpLK = buttonUpLK;
            //    this.buttonUpMK = buttonUpMK;
            //    this.buttonUpHK = buttonUpHK;
            //    this.buttonUpPA = buttonUpPA;
            //}
        }
        public static InputStateDto[] playerToInput;

        /// <summary>
        /// シーンの Update メソッドの中で呼び出されます。
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static InputStateDto OnUpdate(PlayerIndex player)
        {
            InputStateDto input = playerToInput[(int)player];

            //左キー: -1、右キー: 1
            input.leverX = Input.GetAxisRaw(CommonInput.InputNameDictionary[new InputIndex(player, ButtonIndex.Horizontal)]);
            // 下キー: -1、上キー: 1 (Input設定でVerticalの入力にはInvertをチェックしておく）
            input.leverY = Input.GetAxisRaw(CommonInput.InputNameDictionary[new InputIndex(player, ButtonIndex.Vertical)]);
            input.pressingLP = Input.GetButton(CommonInput.InputNameDictionary[new InputIndex(player, ButtonIndex.LightPunch)]);

            // プレイヤー１の 入力テストをしたいとき。
            if (PlayerIndex.Player1 == player)
            {
                Debug.Log($"player1 input leverX={input.leverX} leverY={input.leverY} LP={input.pressingLP}");
            }

            input.pressingMP = Input.GetButton(CommonInput.InputNameDictionary[new InputIndex(player, ButtonIndex.MediumPunch)]);
            input.pressingHP = Input.GetButton(CommonInput.InputNameDictionary[new InputIndex(player, ButtonIndex.HardPunch)]);
            input.pressingLK = Input.GetButton(CommonInput.InputNameDictionary[new InputIndex(player, ButtonIndex.LightKick)]);
            input.pressingMK = Input.GetButton(CommonInput.InputNameDictionary[new InputIndex(player, ButtonIndex.MediumKick)]);
            input.pressingHK = Input.GetButton(CommonInput.InputNameDictionary[new InputIndex(player, ButtonIndex.HardKick)]);
            input.pressingPA = Input.GetButton(CommonInput.InputNameDictionary[new InputIndex(player, ButtonIndex.Pause)]);
            input.pressingCA = Input.GetButton(CommonInput.Input10Ca); // FIXME:
            input.buttonDownLP = Input.GetButtonDown(CommonInput.InputNameDictionary[new InputIndex(player, ButtonIndex.LightPunch)]);
            input.buttonDownMP = Input.GetButtonDown(CommonInput.InputNameDictionary[new InputIndex(player, ButtonIndex.MediumPunch)]);
            input.buttonDownHP = Input.GetButtonDown(CommonInput.InputNameDictionary[new InputIndex(player, ButtonIndex.HardPunch)]);
            input.buttonDownLK = Input.GetButtonDown(CommonInput.InputNameDictionary[new InputIndex(player, ButtonIndex.LightKick)]);
            input.buttonDownMK = Input.GetButtonDown(CommonInput.InputNameDictionary[new InputIndex(player, ButtonIndex.MediumKick)]);
            input.buttonDownHK = Input.GetButtonDown(CommonInput.InputNameDictionary[new InputIndex(player, ButtonIndex.HardKick)]);
            input.buttonDownPA = Input.GetButtonDown(CommonInput.InputNameDictionary[new InputIndex(player, ButtonIndex.Pause)]);
            input.buttonUpLP = Input.GetButtonUp(CommonInput.InputNameDictionary[new InputIndex(player, ButtonIndex.LightPunch)]);
            input.buttonUpMP = Input.GetButtonUp(CommonInput.InputNameDictionary[new InputIndex(player, ButtonIndex.MediumPunch)]);
            input.buttonUpHP = Input.GetButtonUp(CommonInput.InputNameDictionary[new InputIndex(player, ButtonIndex.HardPunch)]);
            input.buttonUpLK = Input.GetButtonUp(CommonInput.InputNameDictionary[new InputIndex(player, ButtonIndex.LightKick)]);
            input.buttonUpMK = Input.GetButtonUp(CommonInput.InputNameDictionary[new InputIndex(player, ButtonIndex.MediumKick)]);
            input.buttonUpHK = Input.GetButtonUp(CommonInput.InputNameDictionary[new InputIndex(player, ButtonIndex.HardKick)]);
            input.buttonUpPA = Input.GetButtonUp(CommonInput.InputNameDictionary[new InputIndex(player, ButtonIndex.Pause)]);

            return input;
        }
    }
}
