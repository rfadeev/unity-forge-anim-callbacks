using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityForge.AnimCallbacks
{
    public class AnimationEventReceiver : MonoBehaviour
    {
        private List<Action> animationStartCallbacks = new List<Action>();
        private List<Action> animationEndCallbacks = new List<Action>();
        private Dictionary<float, List<Action>> animationTimelineCallbacks = new Dictionary<float, List<Action>>();

        public void RegisterAnimationStartCallback(Action callback)
        {
            if (callback == null)
            {
                Debug.LogWarningFormat("Trying to register null animation start callback");
                return;
            }

            animationStartCallbacks.Add(callback);
        }

        public void RegisterAnimationEndCallback(Action callback)
        {
            if (callback == null)
            {
                Debug.LogWarningFormat("Trying to register null animation end callback");
                return;
            }

            animationEndCallbacks.Add(callback);
        }

        public void RegisterTimelineCallback(float atPosition, Action callback)
        {
            if (callback == null)
            {
                Debug.LogWarningFormat("Trying to register null animation timeline callback");
                return;
            }

            if (!animationTimelineCallbacks.ContainsKey(atPosition))
            {
                animationTimelineCallbacks.Add(atPosition, new List<Action>());
            }
            animationTimelineCallbacks[atPosition].Add(callback);
        }

        // Unity binds animation events by method name. This means all components
        // which have method with the name from animation event will be called.
        // Such component must be attached to the object with Animator/Animation.
        // Binding by name prohibits having arbitrary callbacks easily since for
        // that case method must be generated at runtime.
        private void OnStartEventRaised()
        {
            FireCallbacks(animationStartCallbacks);
        }

        private void OnEndEventRaised()
        {
            FireCallbacks(animationEndCallbacks);
        }

        private void OnTimelineEventRaised(float atPosition)
        {
            if (!animationTimelineCallbacks.ContainsKey(atPosition))
            {
                Debug.LogWarningFormat("Callbacks not registered for timeline position {0}", atPosition);
                return;
            }

            var animationPositionCallbacks = animationTimelineCallbacks[atPosition];
            FireCallbacks(animationPositionCallbacks);
        }

        // Unity cannot call static method from animation event so FireCallbacks
        // cannot be called for AnimationEventReceiver added by user.
        private static void FireCallbacks(List<Action> callbacks)
        {
            // In current implementation registered callbacks cannot be removed.
            // Store current count in local variable for the case if callback
            // adds new callback to the list. Added one will be triggered next
            // time animation event happens.
            var count = callbacks.Count;
            for (var i = 0; i < count; ++i)
            {
                var callback = callbacks[i];
                callback();
            }
        }
    }
}
