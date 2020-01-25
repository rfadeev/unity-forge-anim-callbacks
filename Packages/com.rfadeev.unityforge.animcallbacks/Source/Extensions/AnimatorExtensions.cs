using System;
using UnityEngine;

namespace UnityForge.AnimCallbacks
{
    public static class AnimatorExtensions
    {
        public static void AddClipStartCallback(this Animator animator, int layerIndex, string clipName, Action callback)
        {
            animator.AddClipCallback(layerIndex, clipName, 0.0f, callback);
        }

        public static void AddClipEndCallback(this Animator animator, int layerIndex, string clipName, Action callback)
        {
            var clip = animator.GetAnimationClip(layerIndex, clipName);
            if (clip == null)
            {
                Debug.LogWarning("Failed to get animation clip for Animator component");
                return;
            }

            clip.BindCallback(animator.gameObject, clip.length, callback);
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

        public static void RemoveClipStartCallback(this Animator animator, int layerIndex, string clipName, Action callback)
        {
            animator.RemoveClipCallback(layerIndex, clipName, 0.0f, callback);
        }

        public static void RemoveClipEndCallback(this Animator animator, int layerIndex, string clipName, Action callback)
        {
            var clip = animator.GetAnimationClip(layerIndex, clipName);
            if (clip == null)
            {
                Debug.LogWarning("Failed to get animation clip for Animator component");
                return;
            }

            clip.UnbindCallback(animator.gameObject, clip.length, callback);
        }

        public static void RemoveClipCallback(this Animator animator, int layerIndex, string clipName, float atPosition, Action callback)
        {
            var clip = animator.GetAnimationClip(layerIndex, clipName);
            if (clip == null)
            {
                Debug.LogWarning("Failed to get animation clip for Animator component");
                return;
            }

            clip.UnbindCallback(animator.gameObject, atPosition, callback);
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
