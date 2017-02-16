using UnityEditor.Animations;
using UnityEngine;

namespace StellaQL
{
    /// <summary>
    /// There is also a shallow copy (incomplete) AconShallowcopy on the lower side.
    /// There is also AconRemoveLink which cuts off the link on the lower side.
    /// </summary>
    public class AconDeepcopy
    {
        /// <summary>
        /// I want you to deep copy the layer.
        /// </summary>
        public static AnimatorControllerLayer DeepcopyLayer(AnimatorControllerLayer old)
        {
            AnimatorControllerLayer n = new AnimatorControllerLayer();
            n.avatarMask                = old.avatarMask;
            n.blendingMode              = old.blendingMode;
            n.defaultWeight             = old.defaultWeight;
            n.iKPass                    = old.iKPass;
            n.name                      = old.name;
            n.stateMachine              = AconDeepcopy.DeepcopyStatemachine(old.stateMachine);
            n.syncedLayerAffectsTiming  = old.syncedLayerAffectsTiming;
            n.syncedLayerIndex          = old.syncedLayerIndex;
            return n;
        }

        /// <summary>
        /// I want you to deep copy the child statemachine.
        /// </summary>
        public static ChildAnimatorStateMachine[] DeepcopyChildStatemachine(ChildAnimatorStateMachine[] oldItems)
        {
            ChildAnimatorStateMachine[] nItems = new ChildAnimatorStateMachine[oldItems.Length];
            int i = 0;
            foreach (ChildAnimatorStateMachine old in oldItems)
            {
                nItems[i] = DeepcopyChildStatemachine(old);
                i++;
            }
            return nItems;
        }

        /// <summary>
        /// I want you to deep copy the child statemachine.
        /// </summary>
        public static ChildAnimatorStateMachine DeepcopyChildStatemachine(ChildAnimatorStateMachine old)
        {
            ChildAnimatorStateMachine n = new ChildAnimatorStateMachine();
            n.position      = old.position;
            n.stateMachine  = AconDeepcopy.DeepcopyStatemachine(old.stateMachine);
            return n;
        }

        /// <summary>
        /// I want you to deep copy the statemachine.
        /// </summary>
        public static AnimatorStateMachine DeepcopyStatemachine(AnimatorStateMachine old)
        {
            AnimatorStateMachine n = new AnimatorStateMachine();
            n.anyStatePosition              = old.anyStatePosition;
            n.anyStateTransitions           = old.anyStateTransitions;
            n.behaviours                    = old.behaviours;
            n.defaultState                  = old.defaultState;
            n.entryPosition                 = old.entryPosition;
            n.entryTransitions              = old.entryTransitions;
            n.exitPosition                  = old.exitPosition;
            n.hideFlags                     = old.hideFlags;
            n.name                          = old.name;
            n.parentStateMachinePosition    = old.parentStateMachinePosition;
            n.stateMachines                 = AconDeepcopy.DeepcopyChildStatemachine(old.stateMachines);
            n.states                        = old.states;
            return n;
        }

        /// <summary>
        /// I want you to deep copy the child state.
        /// </summary>
        public static ChildAnimatorState DeepcopyChildstate(ChildAnimatorState old)
        {
            ChildAnimatorState n = new ChildAnimatorState();
            n.position  = old.position;
            n.state     = AconDeepcopy.DeepcopyState(old.state);
            return n;
        }

        /// <summary>
        /// I want you to deep copy the state.
        /// </summary>
        public static AnimatorState DeepcopyState(AnimatorState old)
        {
            AnimatorState n = new AnimatorState();
            n.behaviours                    = old.behaviours;
            n.cycleOffset                   = old.cycleOffset;
            n.cycleOffsetParameter          = old.cycleOffsetParameter;
            n.cycleOffsetParameterActive    = old.cycleOffsetParameterActive;
            n.hideFlags                     = old.hideFlags;
            n.iKOnFeet                      = old.iKOnFeet;
            n.mirror                        = old.mirror;
            n.mirrorParameter               = old.mirrorParameter;
            n.mirrorParameterActive         = old.mirrorParameterActive;
            n.motion                        = old.motion;
            n.name                          = old.name;
            // n.nameHash = o.nameHash;
            n.speed                         = old.speed;
            n.speedParameter                = old.speedParameter;
            n.speedParameterActive          = old.speedParameterActive;
            n.tag                           = old.tag;
            n.transitions                   = DeepcopyTransitions( old.transitions);
            // n.uniqueName = o.uniqueName;
            // n.uniqueNameHash = o.uniqueNameHash;
            n.writeDefaultValues            = old.writeDefaultValues;
            return n;
        }

        /// <summary>
        /// I want you to deep copy the transitions.
        /// </summary>
        public static AnimatorStateTransition[] DeepcopyTransitions(AnimatorStateTransition[] oldArray)
        {
            AnimatorStateTransition[] n = new AnimatorStateTransition[oldArray.Length];
            int i = 0;
            foreach(AnimatorStateTransition old in oldArray)
            {
                n[i] = DeepcopyTransition( old);
                i++;
            }
            return n;
        }

        /// <summary>
        /// I want you to deep copy the transition.
        /// </summary>
        public static AnimatorStateTransition DeepcopyTransition(AnimatorStateTransition old)
        {
            AnimatorStateTransition n = new AnimatorStateTransition();
            n.canTransitionToSelf       = old.canTransitionToSelf;
            n.conditions                = old.conditions;
            n.destinationState          = old.destinationState;
            n.destinationStateMachine   = old.destinationStateMachine;
            n.duration                  = old.duration;
            n.exitTime                  = old.exitTime;
            n.hasExitTime               = old.hasExitTime;
            n.hasFixedDuration          = old.hasFixedDuration;
            n.hideFlags                 = old.hideFlags;
            n.interruptionSource        = old.interruptionSource;
            n.isExit                    = old.isExit;
            n.mute                      = old.mute;
            n.name                      = old.name;
            n.offset                    = old.offset;
            n.orderedInterruption       = old.orderedInterruption;
            n.solo                      = old.solo;
            return n;
        }
    }

    public class AconShallowcopy
    {
        /// <summary>
        /// I want you to shallow copy the state.
        /// </summary>
        public static AnimatorState ShallowcopyState(AnimatorState old)
        {
            AnimatorState n = new AnimatorState();
            n.behaviours                    = old.behaviours;
            n.cycleOffset                   = old.cycleOffset;
            n.cycleOffsetParameter          = old.cycleOffsetParameter;
            n.cycleOffsetParameterActive    = old.cycleOffsetParameterActive;
            n.hideFlags                     = old.hideFlags;
            n.iKOnFeet                      = old.iKOnFeet;
            n.mirror                        = old.mirror;
            n.mirrorParameter               = old.mirrorParameter;
            n.mirrorParameterActive         = old.mirrorParameterActive;
            n.motion                        = old.motion;
            n.name                          = old.name;
            // n.nameHash = o.nameHash;
            n.speed                         = old.speed;
            n.speedParameter                = old.speedParameter;
            n.speedParameterActive          = old.speedParameterActive;
            n.tag                           = old.tag;
            n.transitions                   = ShallowcopyTransitions( old.transitions);
            // n.uniqueName = o.uniqueName;
            // n.uniqueNameHash = o.uniqueNameHash;
            n.writeDefaultValues            = old.writeDefaultValues;
            return n;
        }

        /// <summary>
        /// I want you to shallow copy the transitions.
        /// Use it to the extent that it changes the contents.
        /// </summary>
        public static AnimatorStateTransition[] ShallowcopyTransitions(AnimatorStateTransition[] srcArray)
        {
            AnimatorStateTransition[] n = new AnimatorStateTransition[srcArray.Length];
            int i = 0;
            foreach (AnimatorStateTransition src in srcArray)
            {
                n[i] = ShallowcopyTransition(src);
                i++;
            }
            return n;
        }

        /// <summary>
        /// I want you to shallow copy the transition.
        /// Use it to the extent that it changes the contents.
        /// </summary>
        public static AnimatorStateTransition ShallowcopyTransition(AnimatorStateTransition src)
        {
            return ShallowcopyTransition(new AnimatorStateTransition(), src);
        }

        /// <summary>
        /// I want you to shallow copy the transition.
        /// Use it to the extent that it changes the contents.
        /// </summary>
        public static AnimatorStateTransition ShallowcopyTransition(AnimatorStateTransition dst, AnimatorStateTransition src)
        {
            ShallowcopyTransition_ExceptDestinaionState(dst,src);
            dst.destinationState = src.destinationState;
            return dst;
        }

        /// <summary>
        /// I want you to shallow copy the transition except DestinationState.
        /// Use it to the extent that it changes the contents.
        /// </summary>
        public static AnimatorStateTransition ShallowcopyTransition_ExceptDestinaionState(AnimatorStateTransition dst, AnimatorStateTransition src)
        {
            dst.canTransitionToSelf     = src.canTransitionToSelf;
            dst.conditions              = src.conditions;
            // 遷移先は除く dst.destinationState = src.destinationState;
            dst.destinationStateMachine = src.destinationStateMachine; // This is a copy.
            dst.duration                = src.duration;
            dst.exitTime                = src.exitTime;
            dst.hasExitTime             = src.hasExitTime;
            dst.hasFixedDuration        = src.hasFixedDuration;
            dst.hideFlags               = src.hideFlags;
            dst.interruptionSource      = src.interruptionSource;
            dst.isExit                  = src.isExit;
            dst.mute                    = src.mute;
            dst.name                    = src.name;
            dst.offset                  = src.offset;
            dst.orderedInterruption     = src.orderedInterruption;
            dst.solo                    = src.solo;
            return dst;
        }
    }

    public class AconRemoveLink
    {
        public static void RemoveLinkState(AnimatorState state)
        {
            state.behaviours = new StateMachineBehaviour[0];
            state.transitions = new AnimatorStateTransition[0];
        }
    }
}
