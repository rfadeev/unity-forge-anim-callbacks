﻿using System;
using UnityEngine;

namespace UnityForge.AnimCallbacks
{
    public static class AnimationClipExtensions
    {
        private const string OnStartEventRaisedMethodName = "OnStartEventRaised";
        private const string OnEndEventRaisedMethodName = "OnEndEventRaised";

        public static void BindStartOrEndCallback(this AnimationClip clip, GameObject animEvtReceiverObject, Action callback, bool isStart)
        {
            if (animEvtReceiverObject == null)
            {
                Debug.LogWarning("Trying to register callback for null animation event receiver game object");
                return;
            }

            if (callback == null)
            {
                Debug.LogWarning("Trying to register null callback for animation clip " + (isStart ? "start" : "end"));
                return;
            }

            var eventReceiver = animEvtReceiverObject.GetComponent<AnimationEventReceiver>();
            if (eventReceiver == null)
            {
                eventReceiver = animEvtReceiverObject.AddComponent<AnimationEventReceiver>();
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