using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace DojinCircleGrayscale.StellaQL
{
    public abstract class AconFetcher
    {
        /// <summary>
        /// パスを指定すると パラメーターを返す。
        /// </summary>
        /// <param name="parameterName">"Average body length" といった文字列</param>
        public static AnimatorControllerParameter FetchParameter(AnimatorController ac, string parameterName)
        {
            foreach (AnimatorControllerParameter parameter in ac.parameters) { if (parameterName == parameter.name) { return parameter; } }
            throw new UnityException("Not found parameter. parameterName=[" + parameterName + "]");
        }

        /// <summary>
        /// パスを指定すると レイヤーを返す。
        /// </summary>
        /// <param name="justLayerName_EndsWithoutDot">"Base Layer" といった文字列</param>
        public static AnimatorControllerLayer FetchLayer_JustLayerName(AnimatorController ac, string justLayerName_EndsWithoutDot)
        {
            foreach (AnimatorControllerLayer layer in ac.layers) { if (justLayerName_EndsWithoutDot == layer.name) { return layer; } }
            throw new UnityException("Not found layer. justLayerName_EndsWithoutDot=[" + justLayerName_EndsWithoutDot + "]");
        }

        /// <summary>
        /// パスを指定すると ステートマシンを返す。
        /// </summary>
        /// <param name="query">"Base Layer.JMove" といった文字列</param>
        public static AnimatorStateMachine FetchStatemachine(AnimatorController ac, List<string> statemachineNamesEndsWithoutDot, AnimatorControllerLayer layer)
        {
            AnimatorStateMachine currentMachine = layer.stateMachine;

            // ステートマシンが途中にある場合、最後のステートマシンまで降りていく。
            if (0 < statemachineNamesEndsWithoutDot.Count)
            {
                currentMachine = GetLeafMachine_ofStatemachine(ac, currentMachine, statemachineNamesEndsWithoutDot);
                if (null == currentMachine) { throw new UnityException("Not found node. currentMachine.name=[" + currentMachine.name + "] nodes=[" + string.Join("][", statemachineNamesEndsWithoutDot.ToArray()) + "] ac.name=[" + ac.name + "]"); }
            }

            return currentMachine;
        }

        /// <summary>
        /// 最後のステートマシンを返す。
        /// </summary>
        private static AnimatorStateMachine GetLeafMachine_ofStatemachine(AnimatorController ac, AnimatorStateMachine currentMachine, List<string> statemachineNamesEndsWithoutDot)
        {
            for (int i = 0; i < statemachineNamesEndsWithoutDot.Count; i++)
            {
                currentMachine = FetchChildMachine_ofStatemachine(currentMachine, statemachineNamesEndsWithoutDot[i]);
                if (null == currentMachine) { throw new UnityException("Not found node. i=[" + i + "] statemachineNamesEndsWithoutDot[i]=[" + statemachineNamesEndsWithoutDot[i] + "] ac.name=[" + ac.name + "]"); }
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
        /// <param name="path">"Base Layer.JMove.JMove0" といった文字列</param>
        public static ChildAnimatorState FetchChildstate(AnimatorController ac, string path, out AnimatorStateMachine parentStatemachine)
        {
            // レイヤー名.ステートマシン名(いくつか).ステート名
            string[] nodes = path.Split('.');
            if (nodes.Length < 2) { throw new UnityException("Less than 2 nodes. path=[" + path + "]"); }

            AnimatorStateMachine currentMachine = null;
            foreach (AnimatorControllerLayer layer in ac.layers) { if (nodes[0] == layer.name) { currentMachine = layer.stateMachine; break; } }
            if (null == currentMachine) { throw new UnityException("Not found layer statemachine. nodes=[" + string.Join("][", nodes) + "]"); }

            // ステートマシンが途中にある場合、最後のステートマシンまで降りていく。
            if (2 < nodes.Length)
            {
                currentMachine = FetchLeafMachine_ofChilestate(currentMachine, nodes);
                if (null == currentMachine) { throw new UnityException("Not found node. currentMachine.name=[" + currentMachine.name + "] nodes=[" + string.Join("][", nodes) + "]"); }
            }

            parentStatemachine = currentMachine;
            return FetchChildState_ofChilestate(currentMachine, nodes[nodes.Length - 1]);
        }

        /// <summary>
        /// 最後のステートマシンを返す。
        /// </summary>
        private static AnimatorStateMachine FetchLeafMachine_ofChilestate(AnimatorStateMachine currentMachine, string[] nodes)
        {
            for (int i = Operation_Common.ROOT_NODE_IS_LAYER; i < nodes.Length + Operation_Common.LEAF_NODE_IS_STATE; i++)
            {
                currentMachine = FetchChildMachine_ofChilestate(currentMachine, nodes[i]);
                if (null == currentMachine) { throw new UnityException("Not found node. i=[" + i + "] node=[" + nodes[i] + "]"); }
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

        private static ChildAnimatorState FetchChildState_ofChilestate(AnimatorStateMachine parentStatemachine, string stateName)
        {
            foreach (ChildAnimatorState caState in parentStatemachine.states)
            {
                if (caState.state.name == stateName) { return caState; }
            }
            throw new UnityException("Not found child state. stateName=[" + stateName + "]");
        }

        /// <summary>
        /// パス・トークンを指定すると ステートを返す。
        /// </summary>
        public static AnimatorState FetchState(AnimatorController ac, FullpathTokens ft)
        {
            AnimatorControllerLayer layer = AconFetcher.FetchLayer_JustLayerName(ac, ft.LayerNameEndsWithoutDot);
            AnimatorStateMachine currentMachine = layer.stateMachine;
            if (null == currentMachine) { throw new UnityException("Not found layer. nodes=[" + ft.StatemachinePath + "]"); }

            // ステートマシンが途中にある場合、最後のステートマシンまで降りていく。
            if (0 < ft.StatemachineNamesEndsWithoutDot.Count)
            {
                currentMachine = FetchLeafMachine_ofState(currentMachine, ft.StatemachineNamesEndsWithoutDot);
                if (null == currentMachine) { throw new UnityException("Not found node. currentMachine.name=[" + currentMachine.name + "] nodes=[" + ft.StatemachinePath + "]"); }
            }

            return FetchChildState_ofState(currentMachine, ft.StateName);
        }

        /// <summary>
        /// 最後のステートマシンを返す。
        /// </summary>
        private static AnimatorStateMachine FetchLeafMachine_ofState(AnimatorStateMachine currentMachine, List<string> statemachineNamesEndsWithoutDot)
        {
            for (int i = 0; i < statemachineNamesEndsWithoutDot.Count; i++)
            {
                currentMachine = FetchChildMachine_ofState(currentMachine, statemachineNamesEndsWithoutDot[i]);
                if (null == currentMachine) { throw new UnityException("Not found node. i=[" + i + "] node=[" + statemachineNamesEndsWithoutDot[i] + "]"); }
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
            if (null == request.TransitionNum_ofFullpath) { throw new UnityException("Transition number is null. number=[" + request.TransitionNum_ofFullpath + "] ac=[" + ac.name + "]"); }
            int transitionNum_ofFullpath = int.Parse(request.TransitionNum_ofFullpath);
            return FetchTransition_ByFullpath(ac, request.Fullpath, transitionNum_ofFullpath);
        }
        public static AnimatorStateTransition FetchTransition_ByFullpath(AnimatorController ac, string fullpath, int transitionNum_ofFullpath)
        {
            int caret = 0;
            FullpathTokens ft = new FullpathTokens();
            if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(fullpath, ref caret, ref ft)) { throw new UnityException("Parse failure. [" + fullpath + "] ac=[" + ac.name + "]"); }

            AnimatorState state = AconFetcher.FetchState(ac, ft);
            if (null == state) { throw new UnityException("Not found state. [" + fullpath + "] ac=[" + ac.name + "]"); }

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
        /// <param name="path_src">"Base Layer.JMove.JMove0" といった文字列</param>
        public static List<AnimatorStateTransition> FetchTransition_SrcDst(AnimatorController ac, string path_src, string path_dst)
        {
            List<AnimatorStateTransition> hit = new List<AnimatorStateTransition>();

            AnimatorState state_src;
            {
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(path_src, ref caret, ref ft)) { throw new UnityException("Parse failure. [" + path_src + "] ac=[" + ac.name + "]"); }
                state_src = AconFetcher.FetchState(ac, ft);
            }
            AnimatorState state_dst;
            {
                int caret = 0;
                FullpathTokens ft = new FullpathTokens();
                if (!FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(path_dst, ref caret, ref ft)) { throw new UnityException("Parse failure. [" + path_dst + "] ac=[" + ac.name + "]"); }
                state_dst = AconFetcher.FetchState(ac, ft);
            }

            foreach (AnimatorStateTransition transition in state_src.transitions)
            {
                if (transition.destinationState == state_dst)
                {
                    hit.Add(transition);
                }
            }
            return hit;
        }

        public static ConditionRecord.AnimatorConditionWrapper FetchCondition(AnimatorController ac, DataManipulationRecord request)
        {
            AnimatorStateTransition transition = AconFetcher.FetchTransition(ac, request);
            if (null != transition) { return AconFetcher.FetchCondition(ac, request); }

            return null;
        }

        public static ConditionRecord.AnimatorConditionWrapper FetchCondition(AnimatorController ac, AnimatorStateTransition transition, int conditionNum_ofFullpath)
        {
            int cNum = 0;
            foreach (AnimatorCondition condition in transition.conditions)
            {
                if (conditionNum_ofFullpath == cNum) { return new ConditionRecord.AnimatorConditionWrapper(conditionNum_ofFullpath, transition, condition); }
                cNum++;
            }

            // 空コンストラクタで生成した場合、.IsNull( ) メソッドでヌルを返す。
            return new ConditionRecord.AnimatorConditionWrapper();
        }

        public static PositionRecord.PositionWrapper FetchPosition(AnimatorController ac, string layerNameEndsWithoutDot, List<string> statemachineNamesEndsWithoutDot, string propertyname_ofFullpath)
        {
            AnimatorControllerLayer layer = AconFetcher.FetchLayer_JustLayerName(ac, layerNameEndsWithoutDot);

            return FetchPosition(ac, layer, statemachineNamesEndsWithoutDot, propertyname_ofFullpath);
        }
        public static PositionRecord.PositionWrapper FetchPosition(AnimatorController ac, AnimatorControllerLayer layer, List<string> statemachineNamesEndsWithoutDot, string propertyname_ofFullpath)
        {
            AnimatorStateMachine statemachine = AconFetcher.FetchStatemachine(ac, statemachineNamesEndsWithoutDot, layer);
            if (null == statemachine)
            {
                throw new UnityException("Not found statemachine. [" + string.Join("][", statemachineNamesEndsWithoutDot.ToArray()) + "] ac=[" + ac.name + "]");
            }
            return AconFetcher.FetchPosition(statemachine, propertyname_ofFullpath);
        }
        public static PositionRecord.PositionWrapper FetchPosition(AnimatorStateMachine statemachine, string propertyname_ofFullpath)
        {
            return new PositionRecord.PositionWrapper(statemachine, propertyname_ofFullpath);
        }

        public static PositionRecord.PositionWrapper FetchPosition_OfState(AnimatorController ac, string fullpath, string propertyname_ofFullpath)
        {
            AnimatorStateMachine parentStatemachine;

            // 構造体
            ChildAnimatorState caState = AconFetcher.FetchChildstate(ac, fullpath, out parentStatemachine);

            return AconFetcher.FetchPosition_OfState(parentStatemachine, caState, propertyname_ofFullpath);
        }
        public static PositionRecord.PositionWrapper FetchPosition_OfState(AnimatorStateMachine parentStatemachine, ChildAnimatorState caState, string propertyname_ofFullpath)
        {
            return new PositionRecord.PositionWrapper(parentStatemachine, caState, propertyname_ofFullpath);
        }
    }
}
