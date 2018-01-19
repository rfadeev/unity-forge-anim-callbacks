using System;
using UnityEngine;

namespace UnityForge.AnimCallbacks
{
    public static class AnimationExtensions
    {
        private const string OnStartEventRaisedMethodName = "OnStartEventRaised";
        private const string OnEndEventRaisedMethodName = "OnEndEventRaised";

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
            if (callback == null)
            {
                Debug.LogWarning("Trying to register null callback for animation's clip " + (isStart ? "start" : "end"));
                return;
            }

            var clip = animation.GetClip(clipName);
            if (clip == null)
            {
                Debug.LogWarning("Failed to get animation clip");
                return;
            }

            var eventReceiver = animation.gameObject.GetComponent<AnimationEventReceiver>();
            if (eventReceiver == null)
            {
                eventReceiver = animation.gameObject.AddComponent<AnimationEventReceiver>();
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
