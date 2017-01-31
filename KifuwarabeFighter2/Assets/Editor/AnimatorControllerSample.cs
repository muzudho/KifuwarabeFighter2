using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System;
using System.Text;
using StellaQL;

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

        AnimatorState state = Operation_State.Lookup(ac, "Base Layer.JMove.JMove0");
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

        AnimatorStateTransition transition = Operation_Transition.Lookup(ac, "Base Layer.JMove.Tamesi1 1", "Base Layer.JMove.Tamesi1");
        transition.name = "tamesi(^q^)6";
    }

    [MenuItem("(^_^)Menu/Add Transition 1")]
    static void AddTransition()
    {
        // アニメーター・コントローラーを取得。
        AnimatorController ac = (AnimatorController)AssetDatabase.LoadAssetAtPath<AnimatorController>("Assets/Resources/AnimatorControllers/AniCon@Char3.controller");

        StringBuilder message = new StringBuilder();
        Operation_Transition.Insert(ac,
            new HashSet<AnimatorState>() { Operation_State.Lookup(ac, "Base Layer.JMove.Tamesi1 0") },
            new HashSet<AnimatorState>() { Operation_State.Lookup(ac, "Base Layer.JMove.Tamesi1") },
            new Dictionary<string, string>() { }, message);
    }

    [MenuItem("(^_^)Menu/Add 5x5 Transitions 1")]
    static void AddTransitions()
    {
        // アニメーター・コントローラーを取得。
        AnimatorController ac = (AnimatorController)AssetDatabase.LoadAssetAtPath<AnimatorController>("Assets/Resources/AnimatorControllers/AniCon@Char3.controller");

        HashSet<AnimatorState> states = new HashSet<AnimatorState>() {
            Operation_State.Lookup(ac, "Base Layer.JMove.TamesiMachine1.Tamesi1"),
            Operation_State.Lookup(ac, "Base Layer.JMove.TamesiMachine1.Tamesi2"),
            Operation_State.Lookup(ac, "Base Layer.JMove.TamesiMachine1.Tamesi3"),
            Operation_State.Lookup(ac, "Base Layer.JMove.TamesiMachine1.Tamesi4"),
            Operation_State.Lookup(ac, "Base Layer.JMove.TamesiMachine1.Tamesi5"),
        };
        Debug.Log("states.Count = " + states.Count);
        StringBuilder message = new StringBuilder();
        Operation_Transition.Insert(ac, states, states, new Dictionary<string, string>() { }, message);
    }

    [Flags]
    public enum Flag
    {
        A = 0x01 << 1,
        B = 0x01 << 2,
        C = 0x01 << 3,
        D = 0x01 << 4,
    }

    /// <summary>
    /// 参考：HasFlag について。 https://teratail.com/questions/46225
    /// </summary>
    [MenuItem("(^_^)Menu/HasFlag Test")]
    static void HasFlag()
    {
        Debug.Log("結果： Flag.A.HasFlag(Flag.A| Flag.B| Flag.C) = " + Flag.A.HasFlag(Flag.A| Flag.B| Flag.C));
        Debug.Log("結果： (Flag.A| Flag.B).HasFlag(Flag.A | Flag.B | Flag.C) = " + (Flag.A| Flag.B).HasFlag(Flag.A | Flag.B | Flag.C));
        Debug.Log("結果： (Flag.A| Flag.B).HasFlag(Flag.A) = " + (Flag.A| Flag.B).HasFlag(Flag.A));
        Debug.Log("結果： Flag.D.HasFlag(Flag.A | Flag.B | Flag.C) = " + Flag.D.HasFlag(Flag.A | Flag.B | Flag.C));
        Debug.Log("結果： Flag.D.HasFlag(Flag.A) = " + Flag.D.HasFlag(Flag.A));
        Debug.Log("結果： Flag.A.HasFlag(Flag.A| Flag.B) = " + Flag.A.HasFlag(Flag.A | Flag.B));
        Debug.Log("結果： Flag.A.HasFlag(Flag.A) = " + Flag.A.HasFlag(Flag.A));
        Debug.Log("結果： Flag.A.HasFlag(Flag.B) = " + Flag.A.HasFlag(Flag.B));
        Debug.Log("結果： (Flag.A | Flag.B | Flag.C).HasFlag(Flag.A | Flag.B) = " + (Flag.A | Flag.B | Flag.C).HasFlag(Flag.A | Flag.B));
        Debug.Log("結果： (Flag.A | Flag.B | Flag.C).HasFlag(Flag.A | Flag.B | Flag.C) = " + (Flag.A | Flag.B | Flag.C).HasFlag(Flag.A | Flag.B | Flag.C));
        Debug.Log("結果： (Flag.A | Flag.B).HasFlag(Flag.A | Flag.B | Flag.C) = " + (Flag.A | Flag.B).HasFlag(Flag.A | Flag.B | Flag.C));
    }

}
