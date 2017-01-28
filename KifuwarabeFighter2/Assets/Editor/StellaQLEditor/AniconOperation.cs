//
// Animation Controller Operation
//
using UnityEditor.Animations;
using UnityEngine;
using System.Collections.Generic;

namespace StellaQL
{

    /// <summary>
    /// ステート関連
    /// </summary>
    public abstract class AniconOpe_State
    {
        #region 検索
        /// <summary>
        /// パスを指定すると ステートを返す。
        /// </summary>
        /// <param name="path">"Base Layer.JMove.JMove0" といった文字列。</param>
        public static AnimatorState Lookup(AnimatorController ac, string path)
        {
            string[] nodes = path.Split('.');
            // [0～length-2] ステートマシン名
            // [length-1] ステート名

            if (nodes.Length < 2) { throw new UnityException("ノード数が２つ未満だったぜ☆（＾～＾） レイヤー番号か、ステート名は無いのかだぜ☆？"); }

            // 最初の名前[0]は、レイヤーを検索する。
            AnimatorStateMachine currentMachine = null;
            foreach (AnimatorControllerLayer layer in ac.layers)
            {
                if (nodes[0] == layer.name) { currentMachine = layer.stateMachine; break; }
            }
            if (null == currentMachine) { throw new UnityException("見つからないぜ☆（＾～＾）nodes=[" + string.Join("][", nodes) + "]"); }

            if (2 < nodes.Length) // ステートマシンが途中にある場合、最後のステートマシンまで降りていく。
            {
                currentMachine = GetLeafMachine(currentMachine, nodes);
                if (null == currentMachine) { throw new UnityException("無いノードが指定されたぜ☆（＾～＾）9 currentMachine.name=[" + currentMachine.name + "] nodes=[" + string.Join("][", nodes) + "]"); }
            }

            return GetChildState(currentMachine, nodes[nodes.Length - 1]); // レイヤーと葉だけの場合
        }

        /// <summary>
        /// 分かりづらいが、ノードの[1]～[length-1]を辿って、最後のステートマシンを返す。
        /// </summary>
        private static AnimatorStateMachine GetLeafMachine(AnimatorStateMachine currentMachine, string[] nodes)
        {
            for (int i = AniconOpe_Common.ROOT_NODE_IS_LAYER; i < nodes.Length + AniconOpe_Common.LEAF_NODE_IS_STATE; i++)
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

        public static void UpdateProperty(AnimatorController ac, Dictionary<string,string> properties, HashSet<AnimatorState> states)
        {
            foreach (AnimatorState state in states) // 指定されたステート全て対象
            {
                foreach (KeyValuePair<string,string> pair in properties)
                {
                    switch (pair.Key)
                    {
                        // state.behaviours (not primal)
                        case "cycleOffset": state.cycleOffset = float.Parse(pair.Value); break;
                        case "cycleOffsetParameter": state.cycleOffsetParameter = pair.Value; break;
                        case "cycleOffsetParameterActive": state.cycleOffsetParameterActive = bool.Parse(pair.Value); break;
                        // state.hideFlags (not primal)
                        case "iKOnFeet": state.iKOnFeet = bool.Parse(pair.Value); break;
                        case "mirror": state.mirror = bool.Parse(pair.Value); break;
                        case "mirrorParameter": state.mirrorParameter = pair.Value; break;
                        case "mirrorParameterActive": state.mirrorParameterActive = bool.Parse(pair.Value); break;
                        // state.motion (not primal)
                        case "name": state.name = pair.Value; break;
                        // state.nameHash (read only)
                        case "speed": state.speed = float.Parse(pair.Value); break;
                        case "speedParameter": state.speedParameter = pair.Value; break;
                        case "speedParameterActive": state.speedParameterActive = bool.Parse(pair.Value); break;
                        case "tag": state.tag = pair.Value; break;
                        // state.transitions (not primal)
                        // state.uniqueName (deprecated)
                        // state.uniqueNameHash (deprecated)
                        case "writeDefaultValues": state.writeDefaultValues = bool.Parse(pair.Value); break;
                        default: throw new UnityException("未対応のステート・プロパティー ["+ pair.Key+ "]=[" + pair.Value + "]");
                    }
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// トランジション関連
    /// </summary>
    public abstract class AniconOpe_Transition
    {
        /// <summary>
        /// ２つのステートを トランジションで結ぶ。
        /// </summary>
        /// <param name="path_src">"Base Layer.JMove.JMove0" といった文字列。</param>
        public static AnimatorStateTransition Lookup(AnimatorController ac, string path_src, string path_dst)
        {
            AnimatorState state_src = AniconOpe_State.Lookup(ac, path_src);
            AnimatorState state_dst = AniconOpe_State.Lookup(ac, path_dst);

            foreach (AnimatorStateTransition transition in state_src.transitions)
            {
                if (transition.destinationState.name == state_dst.name)
                {
                    return transition;
                }
            }
            return null;
        }

        /// <summary>
        /// ２つのステートを トランジションで結ぶ。
        /// </summary>
        /// <param name="path_src">"Base Layer.JMove.JMove0" といった文字列。</param>
        public static void Add(AnimatorController ac, string path_src, string path_dst)
        {
            AnimatorState state_src = AniconOpe_State.Lookup(ac, path_src);
            AnimatorState state_dst = AniconOpe_State.Lookup(ac, path_dst);

            state_src.AddTransition(state_dst);
        }

        /// <summary>
        /// ２つのステートを トランジションで結ぶ。ステートは複数指定でき、src→dst方向の総当たりで全部結ぶ。
        /// </summary>
        /// <param name="path_src">"Base Layer.JMove.JMove0" といった文字列。</param>
        public static void AddAll(AnimatorController ac, HashSet<AnimatorState> states_src, HashSet<AnimatorState> states_dst)
        {
            foreach (AnimatorState state_src in states_src) {
                foreach (AnimatorState state_dst in states_dst) { state_src.AddTransition(state_dst); }
            }
        }

        /// <summary>
        /// ２つのステート間の トランジションを削除する。ステートは複数指定でき、src→dst方向の総当たりで全部削除する。
        /// </summary>
        /// <param name="path_src">"Base Layer.JMove.JMove0" といった文字列。</param>
        public static void RemoveAll(AnimatorController ac, HashSet<AnimatorState> states_src, HashSet<AnimatorState> states_dst)
        {
            foreach (AnimatorState state_src in states_src)
            {
                foreach (AnimatorStateTransition transition in state_src.transitions)
                {
                    foreach (AnimatorState state_dst in states_dst)
                    {
                        if (state_dst == transition.destinationState)
                        {
                            state_src.RemoveTransition(transition);
                            // break; // 複数、同じところにトランジションを貼れるみたいなんで、全部消そう☆
                        }
                    }
                }
            }
        }

        public static void UpdateProperty(AnimatorController ac, Dictionary<string, string> properties, HashSet<AnimatorState> states_src, HashSet<AnimatorState> states_dst)
        {
            foreach (AnimatorState state_src in states_src) // 指定されたステート全て対象
            {
                foreach (AnimatorStateTransition transition in state_src.transitions)
                {
                    foreach (AnimatorState state_dst in states_dst) // 指定されたステート全て対象
                    {
                        if (state_dst == transition.destinationState)
                        {
                            foreach (KeyValuePair<string, string> pair in properties)
                            {
                                switch (pair.Key)
                                {
                                    case "canTransitionToSelf": transition.canTransitionToSelf = bool.Parse(pair.Value); break;
                                    // transition.conditions (not primal)
                                    // transition.destinationState (not primal)
                                    // transition.destinationStateMachine (not primal)
                                    case "duration": transition.duration = float.Parse(pair.Value); break;
                                    case "exitTime": transition.exitTime = float.Parse(pair.Value); break;
                                    case "hasExitTime": transition.hasExitTime = bool.Parse(pair.Value); break;
                                    case "hasFixedDuration": transition.hasFixedDuration = bool.Parse(pair.Value); break;
                                    // transition.hideFlags (not primal)
                                    // transition.interruptionSource (not primal)
                                    case "isExit": transition.isExit = bool.Parse(pair.Value); break;
                                    case "mute": transition.mute = bool.Parse(pair.Value); break;
                                    case "name": transition.name = pair.Value; break;
                                    case "offset": transition.offset = float.Parse(pair.Value); break;
                                    case "orderedInterruption": transition.orderedInterruption = bool.Parse(pair.Value); break;
                                    case "solo": transition.solo = bool.Parse(pair.Value); break;
                                    default: throw new UnityException("未対応のトランジション・プロパティー [" + pair.Key + "]=[" + pair.Value + "]");
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public abstract class AniconOpe_Common
    {
        /// <summary>
        /// ノードの最初の１つは　レイヤー番号
        /// </summary>
        public const int ROOT_NODE_IS_LAYER = 1;
        /// <summary>
        /// ノードの最後の１つは　ステート名
        /// </summary>
        public const int LEAF_NODE_IS_STATE = -1;
    }
}
