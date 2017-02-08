using UnityEditor;
using UnityEngine;

/// <summary>
/// 新しいコントローラーと　ステートマシンを作成させる [MyMenu] - [Create Controller] メニューを　メニューバーに追加するぜ☆
/// 
/// 手順
/// （１）Assets フォルダーの下のどこかに Editor というフォルダーを作成し、このスクリプト・ファイルを入れる☆
/// （２）Assets/Mecanim フォルダーを作っておくこと。
/// （３）[MyMenu] - [Create Controller] メニューをクリックする☆
/// </summary>
public class SM : MonoBehaviour
{
    /*
    /// <summary>
    /// 
    /// </summary>
    [MenuItem("MyMenu/Create Controller")]
    static void CreateController()
    {
        // （１）コントローラー（ファイル）を作る☆
        var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath("Assets/Mecanim/StateMachineTransitions.controller");

        // （２）[parameters] に４つ追加する。
        controller.AddParameter("TransitionNow", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("Reset", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("GotoB1", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("GotoC", AnimatorControllerParameterType.Trigger);

        // （３）ステートマシン（六角形のやつ）を３つ追加する。
        var rootStateMachine = controller.layers[0].stateMachine;
        var smA = rootStateMachine.AddStateMachine("smA"); // stateMachineA
        var smB = rootStateMachine.AddStateMachine("smB"); // stateMachineB
        var smC = smB.AddStateMachine("smC"); // stateMachineC

        // （４）ステート（長方形のやつ）を５つ追加する。
        var stateA1 = smA.AddState("stateA1");
        var stateB1 = smB.AddState("stateB1");
        var stateB2 = smB.AddState("stateB2");
        smC.AddState("stateC1");
        var stateC2 = smC.AddState("stateC2"); // don’t add an entry transition, should entry to state by default

        // 以下、トランジション（白い矢印の線）を追加する。

        // （５）stateA1 から Exit へのトランジション
        var exitTransition = stateA1.AddExitTransition(); // stateA1 は Exit につなげる。
        
        exitTransition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "TransitionNow"); // 条件にトリガーを追加する
        exitTransition.duration = 0; // duration は 0 に。

        // （６）（これは分からない） stateA1 と Any State がつながっていないようだが？
        var resetTransition = smA.AddAnyStateTransition(stateA1);
        resetTransition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "Reset");
        resetTransition.duration = 0;

        // （７）stateB1 は Entry につなげる。
        var transitionB1 = smB.AddEntryTransition(stateB1);
        transitionB1.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "GotoB1"); // 条件にトリガーを追加する。
        // （８）stateB2 も Entry につなげる。
        smB.AddEntryTransition(stateB2);
        // （９）stateC2 は Entry につなげる方法ではなく、デフォルトとして設定する。
        smC.defaultState = stateC2;
        // （１０）stateC2 は Exit につなげる。
        var exitTransitionC2 = stateC2.AddExitTransition();
        exitTransitionC2.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "TransitionNow"); // 条件にトリガーを追加する。
        exitTransitionC2.duration = 0;

        // （１１）smA（六角形）を smC（六角形）につなげる。
        var stateMachineTransition = rootStateMachine.AddStateMachineTransition(smA, smC);
        stateMachineTransition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "GotoC");

        // （１２）smA（六角形）から smB（六角形）につなげる。
        rootStateMachine.AddStateMachineTransition(smA, smB);
    }
    */
}