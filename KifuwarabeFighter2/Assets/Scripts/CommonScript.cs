using UnityEngine;

public enum SceneIndex
{
    Title,
    Select,
    Main,
    Result,
    Num
}
/// <summary>
/// 対局の結果☆
/// </summary>
public enum Result
{
    Player1_Win,
    Player2_Win,
    //Double_KnockOut,
    None
}
/// <summary>
/// プレイヤー番号。
/// </summary>
public enum PlayerIndex
{
    Player1,
    Player2,
    Num
}
/// <summary>
/// 使用キャラクター。
/// </summary>
public enum CharacterIndex
{
    Kifuwarabe,
    Ponahiko,
    Roborinko,
    TofuMan,
    /// <summary>
    /// 列挙型の要素数、または未使用の値として使用。
    /// </summary>
    Num,
}
public enum ResultFaceSpriteIndex
{
    All,
    Win,
    Lose
}
public enum InputIndex
{
    Horizontal,
    Vertical,
    LightPunch,
    MediumPunch,
    HardPunch,
    LightKick,
    MediumKick,
    HardKick,
    Pause,
    Num
}

// どこからでも使われるぜ☆
public class CommonScript
{
    static CommonScript()
    {
        Result = Result.None;
        Player_to_computer = new bool[] { true, true };
        Player_to_useCharacter = new CharacterIndex[] { CharacterIndex.Kifuwarabe, CharacterIndex.Kifuwarabe };
        Teban = PlayerIndex.Player1;
    }

    public static string[] Scene_to_name = new[] { "Title", "Select", "Main", "Result" };

    public static Result Result { get; set; }
    /// <summary>
    /// 人間か、コンピューターか。
    /// </summary>
    public static bool[] Player_to_computer { get; set; }
    /// <summary>
    /// [Player] プレイヤーの使用キャラクター。
    /// </summary>
    public static CharacterIndex[] Player_to_useCharacter { get; set; }
    public static PlayerIndex Teban { get; set; }
    public static PlayerIndex ReverseTeban(PlayerIndex player)
    {
        switch (player)
        {
            case PlayerIndex.Player1: return PlayerIndex.Player2;
            case PlayerIndex.Player2: return PlayerIndex.Player1;
            default: Debug.LogError("未定義のプレイヤー☆"); return player;
        }
    }

    /// <summary>
    /// セレクト画面と、リザルト画面で使う、顔☆
    /// </summary>
    public static string[,] CharacterAndSlice_to_faceSprites = new string[,]{
        { "Sprites/Face0", "Face0_0", "Face0_1" },
        { "Sprites/Face1", "Face1_0", "Face1_1" },
        { "Sprites/Face2", "Face2_0", "Face2_1" },
        { "Sprites/Face3", "Face3_0", "Face3_1" },
    };


}

/// <summary>
/// 共有する入力関連はこちらに。
/// </summary>
public abstract class CommonInput
{
    static CommonInput()
    {
        player_to_input = new[] { new InputStateDto(), new InputStateDto() };
    }

    /// <summary>
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
    /// </summary>
    public static string[,] PlayerAndInput_to_inputName = new string[2, (int)InputIndex.Num] {
        // Player 1.
        { "Horizontal", "Vertical","P1LightPunch","P1MediumPunch","P1HardPunch","P1LightKick","P1MediumKick","P1HardKick","P1Pause"},
        // Player 2.
        { "P2Horizontal", "P2Vertical","P2LightPunch","P2MediumPunch","P2HardPunch","P2LightKick","P2MediumKick","P2HardKick","P2Pause"},
    };
    public const string INPUT_10_CA = "Cancel";

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
    public static InputStateDto[] player_to_input;

    /// <summary>
    /// シーンの Update メソッドの中で呼び出されます。
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public static InputStateDto OnUpdate(PlayerIndex player)
    {
        InputStateDto input = player_to_input[(int)player];

        //左キー: -1、右キー: 1
        input.leverX = Input.GetAxisRaw(CommonInput.PlayerAndInput_to_inputName[(int)player, (int)InputIndex.Horizontal]);
        // 下キー: -1、上キー: 1 (Input設定でVerticalの入力にはInvertをチェックしておく）
        input.leverY = Input.GetAxisRaw(CommonInput.PlayerAndInput_to_inputName[(int)player, (int)InputIndex.Vertical]);
        input.pressingLP = Input.GetButton(CommonInput.PlayerAndInput_to_inputName[(int)player, (int)InputIndex.LightPunch]);

        // プレイヤー１の 入力テストをしたいとき。
        if (PlayerIndex.Player1 == player)
        {
            Debug.Log($"player1 input leverX={input.leverX} leverY={input.leverY} LP={input.pressingLP}");
        }

        input.pressingMP = Input.GetButton(CommonInput.PlayerAndInput_to_inputName[(int)player, (int)InputIndex.MediumPunch]);
        input.pressingHP = Input.GetButton(CommonInput.PlayerAndInput_to_inputName[(int)player, (int)InputIndex.HardPunch]);
        input.pressingLK = Input.GetButton(CommonInput.PlayerAndInput_to_inputName[(int)player, (int)InputIndex.LightKick]);
        input.pressingMK = Input.GetButton(CommonInput.PlayerAndInput_to_inputName[(int)player, (int)InputIndex.MediumKick]);
        input.pressingHK = Input.GetButton(CommonInput.PlayerAndInput_to_inputName[(int)player, (int)InputIndex.HardKick]);
        input.pressingPA = Input.GetButton(CommonInput.PlayerAndInput_to_inputName[(int)player, (int)InputIndex.Pause]);
        input.pressingCA = Input.GetButton(CommonInput.INPUT_10_CA); // FIXME:
        input.buttonDownLP = Input.GetButtonDown(CommonInput.PlayerAndInput_to_inputName[(int)player, (int)InputIndex.LightPunch]);
        input.buttonDownMP = Input.GetButtonDown(CommonInput.PlayerAndInput_to_inputName[(int)player, (int)InputIndex.MediumPunch]);
        input.buttonDownHP = Input.GetButtonDown(CommonInput.PlayerAndInput_to_inputName[(int)player, (int)InputIndex.HardPunch]);
        input.buttonDownLK = Input.GetButtonDown(CommonInput.PlayerAndInput_to_inputName[(int)player, (int)InputIndex.LightKick]);
        input.buttonDownMK = Input.GetButtonDown(CommonInput.PlayerAndInput_to_inputName[(int)player, (int)InputIndex.MediumKick]);
        input.buttonDownHK = Input.GetButtonDown(CommonInput.PlayerAndInput_to_inputName[(int)player, (int)InputIndex.HardKick]);
        input.buttonDownPA = Input.GetButtonDown(CommonInput.PlayerAndInput_to_inputName[(int)player, (int)InputIndex.Pause]);
        input.buttonUpLP = Input.GetButtonUp(CommonInput.PlayerAndInput_to_inputName[(int)player, (int)InputIndex.LightPunch]);
        input.buttonUpMP = Input.GetButtonUp(CommonInput.PlayerAndInput_to_inputName[(int)player, (int)InputIndex.MediumPunch]);
        input.buttonUpHP = Input.GetButtonUp(CommonInput.PlayerAndInput_to_inputName[(int)player, (int)InputIndex.HardPunch]);
        input.buttonUpLK = Input.GetButtonUp(CommonInput.PlayerAndInput_to_inputName[(int)player, (int)InputIndex.LightKick]);
        input.buttonUpMK = Input.GetButtonUp(CommonInput.PlayerAndInput_to_inputName[(int)player, (int)InputIndex.MediumKick]);
        input.buttonUpHK = Input.GetButtonUp(CommonInput.PlayerAndInput_to_inputName[(int)player, (int)InputIndex.HardKick]);
        input.buttonUpPA = Input.GetButtonUp(CommonInput.PlayerAndInput_to_inputName[(int)player, (int)InputIndex.Pause]);

        return input;
    }

}