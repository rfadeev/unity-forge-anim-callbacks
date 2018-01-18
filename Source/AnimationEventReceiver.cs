using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityForge.AnimCallbacks
{
    public class AnimationEventReceiver : MonoBehaviour
    {
        private List<Action> animationStartCallbacks = new List<Action>();
        private List<Action> animationEndCallbacks = new List<Action>();

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

        // Unity binds animation events by method name. This means all components
        // which have method with the name from animation event will be called.
        // Such component must be attached to the object with Animator/Animation.
        // Binding by name prohibits having arbitrary callbacks easily since for
        // that case method must be generated at runtime.
        private void OnStartEventRaised()
        {
            foreach (var callback in animationStartCallbacks)
            {
                callback();
            }
        }

        private void OnEndEventRaised()
        {
            foreach (var callback in animationEndCallbacks)
            {
                callback();
            }
        }
    }
}
