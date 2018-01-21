using System;
using UnityEngine;

namespace UnityForge.AnimCallbacks
{
    public static class AnimationExtensions
    {
        public static void AddClipStartCallback(this Animation animation, string clipName, Action callback)
        {
            animation.AddClipCallback(clipName, 0.0f, callback);
        }

        public static void AddClipEndCallback(this Animation animation, string clipName, Action callback)
        {
            var clip = animation.GetClip(clipName);
            if (clip == null)
            {
                Debug.LogWarning("Failed to get animation clip for Animation component");
                return;
            }

            clip.BindCallback(animation.gameObject, clip.length, callback);
        }

        public static void AddClipCallback(this Animation animation, string clipName, float atPosition, Action callback)
        {
            var clip = animation.GetClip(clipName);
            if (clip == null)
            {
                Debug.LogWarning("Failed to get animation clip for Animation component");
                return;
            }

            clip.BindCallback(animation.gameObject, atPosition, callback);
        }
    }
}
