using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleBehaviour : StateMachineBehaviour {

    ///// OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    AnimatorClipInfo clipInfo = animator.GetCurrentAnimatorClipInfo(0)[0];
    //    //Debug.Log("OnStateEnter アニメーションクリップ名 : " + clipInfo.clip.name);
    //    string clipname = clipInfo.clip.name;

    //    float clipLength = 0.0f;
    //    if (animator != null)
    //    {
    //        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
    //        AnimationClip clip = System.Array.Find<AnimationClip>(ac.animationClips, (AnimationClip) => AnimationClip.name.Equals(clipname));
    //        if (clip != null)
    //        {
    //            clipLength = clip.length;
    //        }
    //    }
    //    //Debug.Log("OnStateEnter clipname = " + clipname + " clipLength = " + clipLength + " normalizedTime = " + stateInfo.normalizedTime);
    //}

    ///// OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    AnimatorClipInfo clipInfo = animator.GetCurrentAnimatorClipInfo(0)[0];
    //    //Debug.Log("OnStateUpdate アニメーションクリップ名 : " + clipInfo.clip.name);
    //    string clipname = clipInfo.clip.name;

    //    float clipLength = 0.0f;
    //    if (animator != null)
    //    {
    //        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
    //        AnimationClip clip = System.Array.Find<AnimationClip>(ac.animationClips, (AnimationClip) => AnimationClip.name.Equals(clipname));
    //        if (clip != null)
    //        {
    //            clipLength = clip.length;
    //        }
    //    }
    //    //Debug.Log("OnStateUpdate layerIndex = " + layerIndex + " clipname = " + clipname + " clipLength = " + clipLength + " normalizedTime = " + stateInfo.normalizedTime);
    //}

    ///// OnStateExit is called before OnStateExit is called on any state inside this state machine
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    AnimatorClipInfo clipInfo = animator.GetCurrentAnimatorClipInfo(0)[0];
    //    //Debug.Log("OnStateExit アニメーションクリップ名 : " + clipInfo.clip.name);
    //    string clipname = clipInfo.clip.name;

    //    float clipLength = 0.0f;
    //    if (animator != null)
    //    {
    //        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
    //        AnimationClip clip = System.Array.Find<AnimationClip>(ac.animationClips, (AnimationClip) => AnimationClip.name.Equals(clipname));
    //        if (clip != null)
    //        {
    //            clipLength = clip.length;
    //        }
    //    }
    //    //Debug.Log("OnStateExit layerIndex = " + layerIndex + " clipname = " + clipname + " clipLength = " + clipLength + " normalizedTime = " + stateInfo.normalizedTime);
    //}

    ///// OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    AnimatorClipInfo clipInfo = animator.GetCurrentAnimatorClipInfo(0)[0];
    //    //Debug.Log("OnStateMove アニメーションクリップ名 : " + clipInfo.clip.name);
    //    string clipname = clipInfo.clip.name;

    //    float clipLength = 0.0f;
    //    if (animator != null)
    //    {
    //        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
    //        AnimationClip clip = System.Array.Find<AnimationClip>(ac.animationClips, (AnimationClip) => AnimationClip.name.Equals(clipname));
    //        if (clip != null)
    //        {
    //            clipLength = clip.length;
    //        }
    //    }
    //    //Debug.Log("OnStateMove layerIndex = " + layerIndex + " clipname = " + clipname + " clipLength = " + clipLength + " normalizedTime = " + stateInfo.normalizedTime);
    //}

    ///// OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    AnimatorClipInfo clipInfo = animator.GetCurrentAnimatorClipInfo(0)[0];
    //    //Debug.Log("OnStateIK アニメーションクリップ名 : " + clipInfo.clip.name);
    //    string clipname = clipInfo.clip.name;

    //    float clipLength = 0.0f;
    //    if (animator != null)
    //    {
    //        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
    //        AnimationClip clip = System.Array.Find<AnimationClip>(ac.animationClips, (AnimationClip) => AnimationClip.name.Equals(clipname));
    //        if (clip != null)
    //        {
    //            clipLength = clip.length;
    //        }
    //    }
    //    //Debug.Log("OnStateIK layerIndex = " + layerIndex + " clipname = " + clipname + " clipLength = " + clipLength + " normalizedTime = " + stateInfo.normalizedTime);
    //}

    ///// OnStateMachineEnter is called when entering a statemachine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    AnimatorClipInfo clipInfo = animator.GetCurrentAnimatorClipInfo(0)[0];
    //    //Debug.Log("OnStateMachineEnter アニメーションクリップ名 : " + clipInfo.clip.name);
    //    string clipname = clipInfo.clip.name;

    //    float clipLength = 0.0f;
    //    if (animator != null)
    //    {
    //        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
    //        AnimationClip clip = System.Array.Find<AnimationClip>(ac.animationClips, (AnimationClip) => AnimationClip.name.Equals(clipname));
    //        if (clip != null)
    //        {
    //            clipLength = clip.length;
    //        }
    //    }
    //    //Debug.Log("OnStateMachineEnter clipname = " + clipname + " stateMachinePathHash = " + stateMachinePathHash + " clipLength = " + clipLength);
    //}

    ///// OnStateMachineExit is called when exiting a statemachine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    AnimatorClipInfo clipInfo = animator.GetCurrentAnimatorClipInfo(0)[0];
    //    //Debug.Log("OnStateMachineExit アニメーションクリップ名 : " + clipInfo.clip.name);
    //    string clipname = clipInfo.clip.name;

    //    float clipLength = 0.0f;
    //    if (animator != null)
    //    {
    //        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
    //        AnimationClip clip = System.Array.Find<AnimationClip>(ac.animationClips, (AnimationClip) => AnimationClip.name.Equals(clipname));
    //        if (clip != null)
    //        {
    //            clipLength = clip.length;
    //        }
    //    }
    //    //Debug.Log("OnStateMachineExit clipname = " + clipname + " stateMachinePathHash = " + stateMachinePathHash + " clipLength = " + clipLength);
    //}
}
