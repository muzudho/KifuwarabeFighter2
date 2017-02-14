using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor.Animations;
using UnityEngine;

namespace StellaQL
{
    public abstract class AconFetcher
    {
        /// <summary>
        /// パスを指定すると パラメーターを返す。
        /// </summary>
        /// <param name="parameterName">"Average body length" といった文字列。</param>
        public static AnimatorControllerParameter FetchParameter(AnimatorController ac, string parameterName)
        {
            foreach (AnimatorControllerParameter parameter in ac.parameters) { if (parameterName == parameter.name) { return parameter; } }
            throw new UnityException("レイヤーが見つからないぜ☆（＾～＾）parameterName=[" + parameterName + "]");
            //return null;
        }

        /// <summary>
        /// パスを指定すると レイヤーを返す。
        /// </summary>
        /// <param name="justLayerName_EndsWithoutDot">"Base Layer" といった文字列。</param>
        public static AnimatorControllerLayer FetchLayer_JustLayerName(AnimatorController ac, string justLayerName_EndsWithoutDot)
        {
            // 最初の名前のノード[0]は、レイヤーを検索する。
            foreach (AnimatorControllerLayer layer in ac.layers) { if (justLayerName_EndsWithoutDot == layer.name) { return layer; } }
            throw new UnityException("レイヤーが見つからないぜ☆（＾～＾）justLayerName_EndsWithoutDot=[" + justLayerName_EndsWithoutDot + "]");
            //return null;
        }

        /// <summary>
        /// パスを指定すると ステートマシンを返す。
        /// </summary>
        /// <param name="query">"Base Layer.JMove" といった文字列。</param>
        public static AnimatorStateMachine FetchStatemachine(AnimatorController ac, FullpathTokens ft, AnimatorControllerLayer layer)
        {
            AnimatorStateMachine currentMachine = layer.stateMachine;

            if (0 < ft.StatemachineNamesEndsWithoutDot.Count) // ステートマシンが途中にある場合、最後のステートマシンまで降りていく。
            {
                currentMachine = GetLeafMachine_ofStatemachine(ac, currentMachine, ft.StatemachineNamesEndsWithoutDot);
                if (null == currentMachine) { throw new UnityException("無いノードが指定されたぜ☆（＾～＾）9 currentMachine.name=[" + currentMachine.name + "] nodes=[" + string.Join("][", ft.StatemachineNamesEndsWithoutDot.ToArray()) + "] ac.name=[" + ac.name + "]"); }
            }

            return currentMachine;
        }

        /// <summary>
        /// 分かりづらいが、ノードの[1]～[length-1]を辿って、最後のステートマシンを返す。
        /// </summary>
        private static AnimatorStateMachine GetLeafMachine_ofStatemachine(AnimatorController ac, AnimatorStateMachine currentMachine, List<string> statemachineNamesEndsWithoutDot)// string[] nodes
        {
            //for (int i = Operation_Common.ROOT_NODE_IS_LAYER; i < nodes.Length + Operation_Common.LEAF_NODE_IS_STATE; i++)
            for (int i = 0; i < statemachineNamesEndsWithoutDot.Count; i++)
            {
                currentMachine = FetchChildMachine_ofStatemachine(currentMachine, statemachineNamesEndsWithoutDot[i]);
                if (null == currentMachine) { throw new UnityException("無いノードが指定されたぜ☆（＾～＾）10 i=[" + i + "] statemachineNamesEndsWithoutDot[i]=[" + statemachineNamesEndsWithoutDot[i] + "] ac.name=[" + ac.name + "]"); }
            }
            return currentMachine;
        }

        /// <summary>
        /// ノード名から、ステートマシンを取得する。
        /// </summary>
        private static AnimatorStateMachine FetchChildMachine_ofStatemachine(AnimatorStateMachine machine, string childName)
        {
            foreach (ChildAnimatorStateMachine wrapper in machine.stateMachines)
            {
                if (wrapper.stateMachine.name == childName) { return wrapper.stateMachine; }
            }
            return null;
        }

        /// <summary>
        /// パスを指定すると ステートを返す。
        /// </summary>
        /// <param name="path">"Base Layer.JMove.JMove0" といった文字列。</param>
        public static ChildAnimatorState FetchChildstate(AnimatorController ac, string path)
        {
            string[] nodes = path.Split('.'); // [0～length-2] ステートマシン名、[length-1] ステート名　（[0]はレイヤー名かも）
            if (nodes.Length < 2) { throw new UnityException("ノード数が２つ未満だったぜ☆（＾～＾） ステートマシン名か、ステート名は無いのかだぜ☆？ path=[" + path + "]"); }

            // 最初の名前[0]は、レイヤーを検索する。
            AnimatorStateMachine currentMachine = null;
            foreach (AnimatorControllerLayer layer in ac.layers) { if (nodes[0] == layer.name) { currentMachine = layer.stateMachine; break; } }
            if (null == currentMachine) { throw new UnityException("見つからないぜ☆（＾～＾）nodes=[" + string.Join("][", nodes) + "]"); }

            if (2 < nodes.Length) // ステートマシンが途中にある場合、最後のステートマシンまで降りていく。
            {
                currentMachine = FetchLeafMachine_ofChilestate(currentMachine, nodes);
                if (null == currentMachine) { throw new UnityException("無いノードが指定されたぜ☆（＾～＾）9 currentMachine.name=[" + currentMachine.name + "] nodes=[" + string.Join("][", nodes) + "]"); }
            }

            return FetchChildState_ofChilestate(currentMachine, nodes[nodes.Length - 1]); // レイヤーと葉だけの場合
        }

        /// <summary>
        /// 分かりづらいが、ノードの[1]～[length-1]を辿って、最後のステートマシンを返す。
        /// </summary>
        private static AnimatorStateMachine FetchLeafMachine_ofChilestate(AnimatorStateMachine currentMachine, string[] nodes)
        {
            for (int i = Operation_Common.ROOT_NODE_IS_LAYER; i < nodes.Length + Operation_Common.LEAF_NODE_IS_STATE; i++)
            {
                currentMachine = FetchChildMachine_ofChilestate(currentMachine, nodes[i]);
                if (null == currentMachine) { throw new UnityException("無いノードが指定されたぜ☆（＾～＾）10 i=[" + i + "] node=[" + nodes[i] + "]"); }
            }
            return currentMachine;
        }

        private static AnimatorStateMachine FetchChildMachine_ofChilestate(AnimatorStateMachine machine, string childName)
        {
            foreach (ChildAnimatorStateMachine wrapper in machine.stateMachines)
            {
                if (wrapper.stateMachine.name == childName) { return wrapper.stateMachine; }
            }
            return null;
        }

        private static ChildAnimatorState FetchChildState_ofChilestate(AnimatorStateMachine machine, string stateName)
        {
            foreach (ChildAnimatorState wrapper in machine.states)
            {
                if (wrapper.state.name == stateName) { return wrapper; }
            }
            throw new UnityException("チャイルド・A・ステートが見つからないぜ☆（＾～＾） stateName=[" + stateName + "]");
        }

        /// <summary>
        /// パス・トークンを指定すると ステートを返す。
        /// </summary>
        public static AnimatorState FetchState(AnimatorController ac, FullpathTokens ft)
        {
            // 最初の名前[0]は、レイヤーを検索する。
            AnimatorControllerLayer layer = AconFetcher.FetchLayer_JustLayerName(ac, ft.LayerNameEndsWithoutDot);
            AnimatorStateMachine currentMachine = layer.stateMachine;
            if (null == currentMachine) { throw new UnityException("見つからないぜ☆（＾～＾）nodes=[" + ft.StatemachinePath + "]"); }

            if (0 < ft.StatemachineNamesEndsWithoutDot.Count) // ステートマシンが途中にある場合、最後のステートマシンまで降りていく。
            {
                currentMachine = FetchLeafMachine_ofState(currentMachine, ft.StatemachineNamesEndsWithoutDot);
                if (null == currentMachine) { throw new UnityException("無いノードが指定されたぜ☆（＾～＾）9 currentMachine.name=[" + currentMachine.name + "] nodes=[" + ft.StatemachinePath + "]"); }
            }

            return FetchChildState_ofState(currentMachine, ft.StateName); // 葉
        }

        /// <summary>
        /// 分かりづらいが、ノードの[1]～[length-1]を辿って、最後のステートマシンを返す。
        /// </summary>
        private static AnimatorStateMachine FetchLeafMachine_ofState(AnimatorStateMachine currentMachine, List<string> statemachineNamesEndsWithoutDot)// string[] nodes
        {
            //for (int i = Operation_Common.ROOT_NODE_IS_LAYER; i < nodes.Length + Operation_Common.LEAF_NODE_IS_STATE; i++)
            for (int i = 0; i < statemachineNamesEndsWithoutDot.Count; i++)
            {
                currentMachine = FetchChildMachine_ofState(currentMachine, statemachineNamesEndsWithoutDot[i]);
                if (null == currentMachine) { throw new UnityException("無いノードが指定されたぜ☆（＾～＾）10 i=[" + i + "] node=[" + statemachineNamesEndsWithoutDot[i] + "]"); }
            }
            return currentMachine;
        }

        private static AnimatorStateMachine FetchChildMachine_ofState(AnimatorStateMachine machine, string childName)
        {
            foreach (ChildAnimatorStateMachine wrapper in machine.stateMachines)
            {
                if (wrapper.stateMachine.name == childName) { return wrapper.stateMachine; }
            }
            return null;
        }

        private static AnimatorState FetchChildState_ofState(AnimatorStateMachine machine, string stateName)
        {
            foreach (ChildAnimatorState wrapper in machine.states)
            {
                if (wrapper.state.name == stateName) { return wrapper.state; }
            }
            return null;
        }

        public static AnimatorStateTransition FetchTransition(AnimatorController ac, DataManipulationRecord request)
        {
            if (null == request.TransitionNum_ofFullpath) { throw new UnityException("トランジション番号が指定されていないぜ☆（＾～＾） トランジション番号=[" + request.TransitionNum_ofFullpath + "] ac=[" + ac.name + "]"); }
            int transitionNum_ofFullpath = int.Parse(request.TransitionNum_ofFullpath);
            return FetchTransition_ByFullpath(ac, request.Fullpath, transitionNum_ofFullpath);
        }
        public static AnimatorStateTransition FetchTransition_ByFullpath(AnimatorController ac, string fullpath, int transitionNum_ofFullpath)
        {
            int caret = 0;
            FullpathTokens ft = new FullpathTokens();
            if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(fullpath, ref caret, ref ft)) { throw new UnityException("[" + fullpath + "]パース失敗だぜ☆（＾～＾） ac=[" + ac.name + "]"); }

            AnimatorState state = AconFetcher.FetchState(ac, ft);
            if (null == state) { throw new UnityException("[" + fullpath + "]ステートは見つからなかったぜ☆（＾～＾） ac=[" + ac.name + "]"); }

            int tNum = 0;
            foreach (AnimatorStateTransition transition in state.transitions)
            {
                if (transitionNum_ofFullpath == tNum) { return transition; }
                tNum++;
            }

            return null;// TODO:
        }
        /// <summary>
        /// ２つのステートを結んでいる全てのトランジション。
        /// </summary>
        /// <param name="path_src">"Base Layer.JMove.JMove0" といった文字列。</param>
        public static List<AnimatorStateTransition> FetchTransition_SrcDst(AnimatorController ac, string path_src, string path_dst)
        {
            List<AnimatorStateTransition> hit = new List<AnimatorStateTransition>();

            AnimatorState state_src;
            {
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(path_src, ref caret, ref ft)) { throw new UnityException("[" + path_src + "]パース失敗だぜ☆（＾～＾） ac=[" + ac.name + "]"); }
                state_src = AconFetcher.FetchState(ac, ft);
            }
            AnimatorState state_dst;
            {
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(path_dst, ref caret, ref ft)) { throw new UnityException("[" + path_dst + "]パース失敗だぜ☆（＾～＾） ac=[" + ac.name + "]"); }
                state_dst = AconFetcher.FetchState(ac, ft);
            }

            // ebug.Log("Fetch state_src.transitions.Length = [" + state_src.transitions.Length + "]");
            foreach (AnimatorStateTransition transition in state_src.transitions)
            {
                //〇if (transition.destinationState.nameHash == state_dst.nameHash)//if (transition.destinationState.name == state_dst.name)
                if (transition.destinationState == state_dst)////× 
                {
                    // ebug.Log("Fetch 〇 ["+ transition.destinationState.name + "]==["+ state_dst.name + "]");
                    hit.Add(transition);//                    return  transition;
                }
                //else
                //{
                //    ebug.Log("Fetch × [" + transition.destinationState.name + "]==[" + state_dst.name + "]");
                //}
            }
            return hit;// return null;
        }

        public static ConditionRecord.AnimatorConditionWrapper FetchCondition(AnimatorController ac, DataManipulationRecord request)
        {
            AnimatorStateTransition transition = AconFetcher.FetchTransition(ac, request);
            if (null != transition) { return AconFetcher.FetchCondition(ac, request); }

            return null;// TODO:
        }

        public static ConditionRecord.AnimatorConditionWrapper FetchCondition(AnimatorController ac, AnimatorStateTransition transition, int conditionNum_ofFullpath)
        {
            // DataManipulationRecord request
            //int fullpathCondition = int.Parse(request.ConditionNum_ofFullpath);

            int cNum = 0;
            foreach (AnimatorCondition condition in transition.conditions)
            {
                if (conditionNum_ofFullpath == cNum) { return new ConditionRecord.AnimatorConditionWrapper(conditionNum_ofFullpath, transition, condition); }
                cNum++;
            }

            return new ConditionRecord.AnimatorConditionWrapper(); // 空コンストラクタで生成した場合、.IsNull( ) メソッドでヌルを返す。
        }
    }
}
