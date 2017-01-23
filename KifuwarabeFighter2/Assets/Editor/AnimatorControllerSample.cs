using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
//using SceneMain;

//[ExecuteInEditMode]
public class AnimatorControllerSample : MonoBehaviour {

    /// <summary>
    /// エディット・モードでは、ルックアップはできなさそう。
    /// </summary>
    [MenuItem("(^_^)Menu/Lookup")]
    static void Lookup()
    {
        //// アニメーター・コントローラーを取得。
        //AnimatorController ac = (AnimatorController)AssetDatabase.LoadAssetAtPath<AnimatorController>("Assets/Resources/AnimatorControllers/AniCon@Char3.controller");
        //AnimatorControllerLayer layer = ac.layers[0];
        //AnimatorState state = layer.stateMachine.states[0].state;

        // 実行中なら animator.Play(～) が利く。
        Animator animator = GameObject.Find("Player0").GetComponent<Animator>();
        animator.Play("JMove0",0);
        Debug.Log("Play2!");
    }

    [MenuItem("(^_^)Menu/Set Tag13")]
    static void SetTag()
    {
        // アニメーター・コントローラーを取得。
        AnimatorController ac = (AnimatorController)AssetDatabase.LoadAssetAtPath<AnimatorController>("Assets/Resources/AnimatorControllers/AniCon@Char3.controller");

        AnimatorState state = AinmatorControllerOperation.LookupState(ac, "Base Layer.JMove.JMove0");
        state.tag = "tamesi(^q^)5";
    }

}
