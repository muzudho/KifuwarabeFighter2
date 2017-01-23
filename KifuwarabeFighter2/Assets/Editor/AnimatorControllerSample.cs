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

    [MenuItem("(^_^)Menu/Set Tag15")]
    static void SetTag()
    {
        // アニメーター・コントローラーを取得。
        AnimatorController ac = (AnimatorController)AssetDatabase.LoadAssetAtPath<AnimatorController>("Assets/Resources/AnimatorControllers/AniCon@Char3.controller");

        AnimatorState state = AinmatorControllerOperation.LookupState(ac, "Base Layer.JMove.JMove0");
        //AnimatorState state;
        //state = AinmatorControllerOperation.LookupState(ac, "Base Layer.JMove.TamesiMachine1.Tamesi1");
        //state.tag = "tamesi(^q^)1";
        //state = AinmatorControllerOperation.LookupState(ac, "Base Layer.JMove.TamesiMachine1.Tamesi2");
        //state.tag = "tamesi(^q^)2";
        //state = AinmatorControllerOperation.LookupState(ac, "Base Layer.JMove.TamesiMachine1.Tamesi3");
        //state.tag = "tamesi(^q^)3";
        //state = AinmatorControllerOperation.LookupState(ac, "Base Layer.JMove.TamesiMachine1.Tamesi4");
        //state.tag = "tamesi(^q^)4";
        //state = AinmatorControllerOperation.LookupState(ac, "Base Layer.JMove.TamesiMachine1.Tamesi5");
        //state.tag = "tamesi(^q^)5";
    }

    [MenuItem("(^_^)Menu/Set Transition Name 1")]
    static void SetTransitionTag()
    {
        // アニメーター・コントローラーを取得。
        AnimatorController ac = (AnimatorController)AssetDatabase.LoadAssetAtPath<AnimatorController>("Assets/Resources/AnimatorControllers/AniCon@Char3.controller");

        AnimatorStateTransition transition = AinmatorControllerOperation.LookupTransition(ac, "Base Layer.JMove.Tamesi1 1", "Base Layer.JMove.Tamesi1");
        transition.name = "tamesi(^q^)6";
    }

    [MenuItem("(^_^)Menu/Add Transition 1")]
    static void AddTransition()
    {
        // アニメーター・コントローラーを取得。
        AnimatorController ac = (AnimatorController)AssetDatabase.LoadAssetAtPath<AnimatorController>("Assets/Resources/AnimatorControllers/AniCon@Char3.controller");

        AinmatorControllerOperation.AddTransition(ac, "Base Layer.JMove.Tamesi1 0", "Base Layer.JMove.Tamesi1");
    }

    [MenuItem("(^_^)Menu/Add 5x5 Transitions 1")]
    static void AddTransitions()
    {
        // アニメーター・コントローラーを取得。
        AnimatorController ac = (AnimatorController)AssetDatabase.LoadAssetAtPath<AnimatorController>("Assets/Resources/AnimatorControllers/AniCon@Char3.controller");

        List<AnimatorState> states = new List<AnimatorState>() {
            AinmatorControllerOperation.LookupState(ac, "Base Layer.JMove.TamesiMachine1.Tamesi1"),
            AinmatorControllerOperation.LookupState(ac, "Base Layer.JMove.TamesiMachine1.Tamesi2"),
            AinmatorControllerOperation.LookupState(ac, "Base Layer.JMove.TamesiMachine1.Tamesi3"),
            AinmatorControllerOperation.LookupState(ac, "Base Layer.JMove.TamesiMachine1.Tamesi4"),
            AinmatorControllerOperation.LookupState(ac, "Base Layer.JMove.TamesiMachine1.Tamesi5"),
        };
        Debug.Log("states.Count = " + states.Count);
        AinmatorControllerOperation.AddTransitions(ac, states, states);
    }

}
