using System;
using System.Linq;
using UnityEngine;

namespace UnityForge.AnimCallbacks
{
    public static class AnimationClipExtensions
    {
        private const string OnTimelineEventRaisedMethodName = "OnTimelineEventRaised";

        public static void BindCallback(this AnimationClip clip, GameObject animEvtReceiverObject, float atPosition, Action callback)
        {
            clip.BindOrUnbindCallback(animEvtReceiverObject, atPosition, callback, true);
        }

        public static void UnbindCallback(this AnimationClip clip, GameObject animEvtReceiverObject, float atPosition, Action callback)
        {
            clip.BindOrUnbindCallback(animEvtReceiverObject, atPosition, callback, false);
        }

        private static void BindOrUnbindCallback(this AnimationClip clip, GameObject animEvtReceiverObject, float atPosition, Action callback, bool bind)
        {
            var actionWord = bind ? "register" : "unregister";
            if (animEvtReceiverObject == null)
            {
                Debug.LogWarningFormat("Trying to {0} callback for null animation event receiver game object", actionWord);
                return;
            }

            if (callback == null)
            {
                Debug.LogWarningFormat("Trying to {0} null callback for animation clip", actionWord);
                return;
            }

            if (atPosition < 0.0f || atPosition > clip.length)
            {
                Debug.LogWarningFormat("Trying to {0} callback for position outside of clip timeline", actionWord);
                return;
            }

            var eventReceiver = animEvtReceiverObject.GetComponent<AnimationEventReceiver>();
            if (bind)
            {
                if (eventReceiver == null)
                {
                    eventReceiver = animEvtReceiverObject.AddComponent<AnimationEventReceiver>();
                }

                // Use callback timeline position as unique identifier of the list of callbacks
                // called at that timeline position. Set timeline position as animation event parameter
                // to send it to AnimationEventReceiver to get list of callbacks at that position.
                eventReceiver.RegisterTimelineCallback(atPosition, callback);
                clip.AddEventIfNotExists(OnTimelineEventRaisedMethodName, atPosition, atPosition);
            }
            else
            {
                if (eventReceiver == null)
                {
                    Debug.LogWarningFormat("Trying to unregister callback for game object without AnimationEventReceiver component");
                    return;
                }

                // Currently AnimationEventReceiver component is not removed when all callback for all positions
                // are removed. This is due to component being public which means it could be used by user code.
                var lastCallbackForPositionRemoved = eventReceiver.UnregisterTimelineCallback(atPosition, callback);
                if (lastCallbackForPositionRemoved)
                {
                    clip.RemoveEvent(OnTimelineEventRaisedMethodName, atPosition, atPosition);
                }
            }
        }

        private static void AddEventIfNotExists(this AnimationClip clip, string methodName, float floatParameter, float time)
        {
            var clipAnimationEvents = clip.events;
            var animationEvent = Array.Find(clipAnimationEvents,
                e => e.functionName == methodName && e.floatParameter == floatParameter && e.time == time);

            if (animationEvent == null)
            {
                animationEvent = new AnimationEvent();
                animationEvent.functionName = methodName;
                animationEvent.floatParameter = floatParameter;
                animationEvent.time = time;
                clip.AddEvent(animationEvent);
            }
        }

        private static void RemoveEvent(this AnimationClip clip, string methodName, float floatParameter, float time)
        {
            var clipAnimationEvents = clip.events;
            var animationEventIndex = Array.FindIndex(clipAnimationEvents,
                e => e.functionName == methodName && e.floatParameter == floatParameter && e.time == time);

            if (animationEventIndex != -1)
            {
                clipAnimationEvents = clipAnimationEvents.Where((val, idx) => idx != animationEventIndex).ToArray();
                clip.events = clipAnimationEvents;
            }
            else
            {
                Debug.LogWarningFormat("Failed to remove animation event for clip {0}", clip.name);
            }
        }
    }
}
