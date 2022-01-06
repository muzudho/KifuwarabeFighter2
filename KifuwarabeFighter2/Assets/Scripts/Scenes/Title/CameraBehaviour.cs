﻿namespace SceneTitle
{
    using Assets.Scripts;
    using Assets.Scripts.Models;
    using Assets.Scripts.Models.Input;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// [Title]シーンのカメラの振る舞い
    /// </summary>
    public class CameraBehaviour : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            // 各プレイヤーについて、キャンセル以外の 何かボタンを押したらセレクト画面へ遷移
            foreach (var player in PlayerKeys.All)
            {
                // プレイヤーのキー押下状態を確認。
                GamepadStatus state = ApplicationStatus.ReadInput(player);

                if (state.Lp.Pressing ||
                    state.Mp.Pressing ||
                    state.Hp.Pressing ||
                    state.Lk.Pressing ||
                    state.Mk.Pressing ||
                    state.Hk.Pressing ||
                    state.Pause.Pressing
                )
                {
                    Debug.Log($"Push key. human={player} input {state.ToDisplay()}");
                    AppHelper.ComputerFlags[player] = false;

                    // * Configure scene.
                    //     * Click main menu [File] - [Build Settings...].
                    //     * Double click [Assets] - [Scenes] - [Title] in project view.
                    //     * Click [Add Open Scenes] button.
                    //     * Double click [Assets] - [Scenes] - [Select] in project view.
                    //     * Click [Add Open Scenes] button.
                    //     * Double click [Assets] - [Scenes] - [Fight] in project view.
                    //     * Click [Add Open Scenes] button.
                    //     * Double click [Assets] - [Scenes] - [Result] in project view.
                    //     * Click [Add Open Scenes] button.
                    //     * Right click `Scenes/SampleScene` from `Build Settings/Scene In Build`. and Click [Remove Selection].
                    SceneManager.LoadScene(AppHelper.sceneToName[(int)SceneKey.Select]);
                }
            }
        }
    }
}
