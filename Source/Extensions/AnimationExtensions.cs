using System;
using UnityEngine;

namespace UnityForge.AnimCallbacks
{
    public static class AnimationExtensions
    {
        public static void OnClipStart(this Animation animation, string clipName, Action callback)
        {
            animation.OnClipStartOrEnd(clipName, callback, true);
        }

        public static void OnClipEnd(this Animation animation, string clipName, Action callback)
        {
            animation.OnClipStartOrEnd(clipName, callback, false);
        }

        private static void OnClipStartOrEnd(this Animation animation, string clipName, Action callback, bool isStart)
        {
            var clip = animation.GetClip(clipName);
            if (clip == null)
            {
                Debug.LogWarning("Failed to get animation clip for Animation component");
                return;
            }

            clip.BindStartOrEndCallback(animation.gameObject, callback, isStart);
        }
    }
}
