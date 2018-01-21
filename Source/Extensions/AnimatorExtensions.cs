using System;
using UnityEngine;

namespace UnityForge.AnimCallbacks
{
    public static class AnimatorExtensions
    {
        public static void OnClipStart(this Animator animator, int layerIndex, string clipName, Action callback)
        {
            animator.OnClipStartOrEnd(layerIndex, clipName, callback, true);
        }

        public static void OnClipEnd(this Animator animator, int layerIndex, string clipName, Action callback)
        {
            animator.OnClipStartOrEnd(layerIndex, clipName, callback, false);
        }

        public static void AddClipCallback(this Animator animator, int layerIndex, string clipName, float atPosition, Action callback)
        {
            var clip = animator.GetAnimationClip(layerIndex, clipName);
            if (clip == null)
            {
                Debug.LogWarning("Failed to get animation clip for Animator component");
                return;
            }

            clip.BindCallback(animator.gameObject, atPosition, callback);
        }

        private static void OnClipStartOrEnd(this Animator animator, int layerIndex, string clipName, Action callback, bool isStart)
        {
            var clip = animator.GetAnimationClip(layerIndex, clipName);
            if (clip == null)
            {
                Debug.LogWarning("Failed to get animation clip for Animator component");
                return;
            }

            clip.BindStartOrEndCallback(animator.gameObject, callback, isStart);
        }

        private static AnimationClip GetAnimationClip(this Animator animator, int layerIndex, string clipName)
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
    }
}
