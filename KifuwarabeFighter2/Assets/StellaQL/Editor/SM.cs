using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using StellaQL;

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
    [MenuItem("MyMenu/SetParameterName")]
    static void SetAvatarMask()
    {
        AnimatorController ac = AssetDatabase.LoadAssetAtPath<AnimatorController>(FileUtility_Engine.PATH_ANIMATOR_CONTROLLER_FOR_DEMO_TEST);
        AnimatorControllerParameter oldP = ac.parameters[0];
        AnimatorControllerParameter newP = new AnimatorControllerParameter();

        // ディープ・コピー
        newP.defaultBool = oldP.defaultBool;
        newP.defaultFloat = oldP.defaultFloat;
        newP.defaultInt = oldP.defaultInt;
        newP.name = oldP.name;
        //newP.nameHash = oldP.nameHash;
        newP.type = oldP.type;

        // プロパティー更新
        newP.name = "Average body length";

        ac.RemoveParameter(oldP);
        ac.AddParameter(newP);
        Debug.Log("プロパティー名は変更されただろうか☆（＾～＾）？");




        //AnimatorController ac = AssetDatabase.LoadAssetAtPath<AnimatorController>(FileUtility_Engine.PATH_ANIMATOR_CONTROLLER_FOR_DEMO_TEST);
        //AnimatorControllerParameter p = new AnimatorControllerParameter();
        //p.name = "Average body length";
        //ac.parameters[0] = p;
        //Debug.Log("プロパティー名は変更されただろうか☆（＾～＾）？");

        //// アニメーター・コントローラーを取得。
        //AnimatorController ac = AssetDatabase.LoadAssetAtPath<AnimatorController>(FileUtility_Engine.PATH_ANIMATOR_CONTROLLER_FOR_DEMO_TEST);
        //ac.parameters[0].name = "Average body length"; // 反映されない。
        //Debug.Log("プロパティー名は変更されただろうか☆（＾～＾）？");
    }

    /*
    [MenuItem("MyMenu/SetAvatarMask")]
    static void SetAvatarMask()
    {
        // アニメーター・コントローラーを取得。
        AnimatorController ac = AssetDatabase.LoadAssetAtPath<AnimatorController>(FileUtility_Engine.PATH_ANIMATOR_CONTROLLER_FOR_DEMO_TEST);
        AvatarMask value = AssetDatabase.LoadAssetAtPath<AvatarMask>("Assets/Resources/AvatarMasks/Head.mask");

        //ac.layers[1].avatarMask = value; // アバターマスクが変わらない
        //ac.layers[1].blendingMode = AnimatorLayerBlendingMode.Override;
        //ac.layers[1].iKPass = true;
        //ac.layers[1].defaultWeight = 59.63f;
        //Debug.Log("アバターマスクのプロパティーをセットしようぜ☆（＾～＾）？");

        //AnimatorControllerLayer layer = new AnimatorControllerLayer();
        //layer.avatarMask = value;
        //layer.blendingMode = AnimatorLayerBlendingMode.Override;
        //layer.iKPass = true;
        //layer.defaultWeight = 59.63f;
        //ac.layers[1] = layer; // 変わりなし？上書きされない？ layers配列が読取専用、またはコピーの可能性は？
        //Debug.Log("新しいレイヤーで上書きしようぜ☆（＾～＾）？");

        // レイヤーを複製できるだろうか？
        int layerIndex = 1;
        AnimatorControllerLayer layer = ac.layers[layerIndex]; // 既存レイヤーを取得
        layer.avatarMask = value;
        layer.blendingMode = AnimatorLayerBlendingMode.Override;
        layer.iKPass = true;
        layer.defaultWeight = 59.63f;
        ac.RemoveLayer(layerIndex); // 既存のレイヤーを削除（TODO:配列の後ろから）
        ac.AddLayer(layer); // プロパティーを変更した既存レイヤーを再追加。
        Debug.Log("プロパティーを変更したレイヤーを再追加してどうか☆（＾～＾）？");

        //ac.AddLayer(layer); // いける。
        //Debug.Log("レイヤーを作り直してどうか☆（＾～＾）？");
    }
    */

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