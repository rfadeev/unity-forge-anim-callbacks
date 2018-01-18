using System;
using UnityEngine;

namespace UnityForge.AnimCallbacks
{
    public static class AnimationClipExtensions
    {
        public static void AddEventIfNotExists(this AnimationClip clip, string methodName, float time)
        {
            var clipAnimationEvents = clip.events;
            var animationEvent = Array.Find(clipAnimationEvents,
                e => e.functionName == methodName && e.time == time);

            if (animationEvent == null)
            {
                animationEvent = new AnimationEvent();
                animationEvent.functionName = methodName;
                animationEvent.time = time;
                clip.AddEvent(animationEvent);
            }
        }
    }
}
