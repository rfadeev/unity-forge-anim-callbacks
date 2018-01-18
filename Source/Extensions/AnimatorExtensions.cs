using System;
using UnityEngine;

namespace UnityForge.AnimCallbacks
{
    public static class AnimatorExtensions
    {
        private const string OnStartEventRaisedMethodName = "OnStartEventRaised";
        private const string OnEndEventRaisedMethodName = "OnEndEventRaised";

        public static AnimationClip GetAnimationClip(this Animator animator, int layerIndex, string clipName)
        {
            var clipsInfo = animator.GetCurrentAnimatorClipInfo(layerIndex);
            var index = Array.FindIndex(clipsInfo, x => x.clip.name == clipName);
            if (index == -1)
            {
                Debug.LogWarningFormat("Clip with name {0} not found in layer with index {1}", clipName, layerIndex);
                return null;
            }
            var clipInfo = clipsInfo[index];
            return clipInfo.clip;
        }

        public static void OnClipStart(this Animator animator, int layerIndex, string clipName, Action callback)
        {
            animator.OnClipStartOrEnd(layerIndex, clipName, callback, true);
        }

        public static void OnClipEnd(this Animator animator, int layerIndex, string clipName, Action callback)
        {
            animator.OnClipStartOrEnd(layerIndex, clipName, callback, false);
        }

        private static void OnClipStartOrEnd(this Animator animator, int layerIndex, string clipName, Action callback, bool isStart)
        {
            if (callback == null)
            {
                Debug.LogWarning("Trying to register null callback for animator clip " + (isStart ? "start" : "end"));
                return;
            }

            var clip = animator.GetAnimationClip(layerIndex, clipName);
            if (clip == null)
            {
                Debug.LogWarning("Failed to get animation clip");
                return;
            }

            var eventReceiver = animator.gameObject.GetComponent<AnimationEventReceiver>();
            if (eventReceiver == null)
            {
                eventReceiver = animator.gameObject.AddComponent<AnimationEventReceiver>();
            }

            if (isStart)
            {
                eventReceiver.RegisterAnimationStartCallback(callback);
                clip.AddEventIfNotExists(OnStartEventRaisedMethodName, 0.0f);
            }
            else
            {
                eventReceiver.RegisterAnimationEndCallback(callback);
                clip.AddEventIfNotExists(OnEndEventRaisedMethodName, clip.length);
            }
        }
    }
}
