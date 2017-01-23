using UnityEditor.Animations;
using UnityEngine;

public abstract class AinmatorControllerOperation
{
    /// <summary>
    /// ノードの最初の１つは　レイヤー番号
    /// </summary>
    public const int ROOT_NODE_IS_LAYER = 1;
    /// <summary>
    /// ノードの最後の１つは　ステート名
    /// </summary>
    public const int LEAF_NODE_IS_STATE = -1;

    #region ステート検索
    /// <summary>
    /// パスを指定すると ステートを返す。
    /// </summary>
    /// <param name="path">"Base Layer.JMove.JMove0" といった文字列。</param>
    public static AnimatorState LookupState(AnimatorController ac, string path)
    {
        string[] nodes = path.Split('.');
        // [0～length-2] ステートマシン名
        // [length-1] ステート名

        if ( nodes.Length < 2){ throw new UnityException("ノード数が２つ未満だったぜ☆（＾～＾） レイヤー番号か、ステート名は無いのかだぜ☆？"); }

        // 最初の名前[0]は、レイヤーを検索する。
        AnimatorStateMachine currentMachine = null;
        foreach (AnimatorControllerLayer layer in ac.layers)
        {
            if (nodes[0] == layer.name) { currentMachine = layer.stateMachine; break; }
        }
        if (null == currentMachine) { throw new UnityException("見つからないぜ☆（＾～＾）nodes=[" + string.Join("][", nodes) + "]"); }

        if (2<nodes.Length) // ステートマシンが途中にある場合、最後のステートマシンまで降りていく。
        {
            currentMachine = GetLeafMachine(currentMachine, nodes);
            if (null == currentMachine) { throw new UnityException("無いノードが指定されたぜ☆（＾～＾）9 currentMachine.name=[" + currentMachine.name + "] nodes=[" + string.Join("][", nodes)+"]"); }
        }

        return GetChildState(currentMachine, nodes[nodes.Length - 1]); // レイヤーと葉だけの場合
    }

    /// <summary>
    /// 分かりづらいが、ノードの[1]～[length-1]を辿って、最後のステートマシンを返す。
    /// </summary>
    private static AnimatorStateMachine GetLeafMachine(AnimatorStateMachine currentMachine, string[] nodes)
    {
        for (int i = ROOT_NODE_IS_LAYER; i < nodes.Length + LEAF_NODE_IS_STATE; i++)
        {
            currentMachine = GetChildMachine(currentMachine, nodes[i]);
            if (null == currentMachine) { throw new UnityException("無いノードが指定されたぜ☆（＾～＾）10 i=[" + i + "] node=[" + nodes[i] + "]"); }
        }
        return currentMachine;
    }

    private static AnimatorStateMachine GetChildMachine(AnimatorStateMachine machine, string childName)
    {
        foreach (ChildAnimatorStateMachine wrapper in machine.stateMachines)
        {
            if (wrapper.stateMachine.name == childName) { return wrapper.stateMachine; }
        }
        return null;
    }

    private static AnimatorState GetChildState(AnimatorStateMachine machine, string stateName)
    {
        foreach (ChildAnimatorState wrapper in machine.states)
        {
            if (wrapper.state.name == stateName) { return wrapper.state; }
        }
        return null;
    }
    #endregion

    #region トランジション
    /// <summary>
    /// パスを指定すると トランジションを返す。
    /// </summary>
    /// <param name="path">"Base Layer.JMove.JMove0" といった文字列。</param>
    public static AnimatorStateTransition LookupTransition(AnimatorController ac, string path_src, string path_dst )
    {
        AnimatorState state_src = LookupState(ac, path_src);
        AnimatorState state_dst = LookupState(ac, path_dst);

        foreach (AnimatorStateTransition transition in state_src.transitions)
        {
            if(transition.destinationState.name == state_dst.name)
            {
                return transition;
            }
        }
        return null;
    }

    /// <summary>
    /// パスを指定すると トランジションを返す。
    /// </summary>
    /// <param name="path">"Base Layer.JMove.JMove0" といった文字列。</param>
    public static void AddTransition(AnimatorController ac, string path_src, string path_dst)
    {
        AnimatorState state_src = LookupState(ac, path_src);
        AnimatorState state_dst = LookupState(ac, path_dst);

        state_src.AddTransition(state_dst);
    }
    #endregion

}
