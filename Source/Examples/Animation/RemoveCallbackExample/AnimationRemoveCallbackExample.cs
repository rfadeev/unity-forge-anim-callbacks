#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace UnityForge.AnimCallbacks.Examples
{
    public class AnimationRemoveCallbackExample : MonoBehaviour
    {
        private const float ExampleTimelinePosition = 0.5f;

#pragma warning disable 0649
        [SerializeField]
        private Animation exampleAnimation;
        [SerializeField]
        private string clipName;
#pragma warning restore 0649

        private void Start()
        {
            exampleAnimation.Play(clipName);
        }

        private void HandleClipStarted()
        {
            Debug.LogFormat("Clip \"{0}\": started", clipName);
        }

        private void HandleClipEnded()
        {
            Debug.LogFormat("Clip \"{0}\": ended", clipName);
        }

        private void HandleClipProgressed()
        {
            Debug.LogFormat("Clip \"{0}\": at {1} seconds after start", clipName, ExampleTimelinePosition);
        }

#if UNITY_EDITOR
        [ContextMenu("Add Start Callback")]
        private void AddStartCallback()
        {
            exampleAnimation.AddClipStartCallback(clipName, HandleClipStarted);
        }

        [ContextMenu("Add Start Callback", true)]
        private bool ValidateAddStartCallback()
        {
            return EditorApplication.isPlaying;
        }

        [ContextMenu("Add End Callback")]
        private void AddEndCallback()
        {
            exampleAnimation.AddClipEndCallback(clipName, HandleClipEnded);
        }

        [ContextMenu("Add End Callback", true)]
        private bool ValidateAddEndCallback()
        {
            return EditorApplication.isPlaying;
        }

        [ContextMenu("Add Progress Callback")]
        private void AddProgressCallback()
        {
            exampleAnimation.AddClipCallback(clipName, ExampleTimelinePosition, HandleClipProgressed);
        }

        [ContextMenu("Add Progress Callback", true)]
        private bool ValidateAddProgressCallback()
        {
            return EditorApplication.isPlaying;
        }

        [ContextMenu("Remove Start Callback")]
        private void RemoveStartcallback()
        {
            exampleAnimation.RemoveClipStartCallback(clipName, HandleClipStarted);
        }

        [ContextMenu("Remove Start Callback", true)]
        private bool Validate()
        {
            return EditorApplication.isPlaying;
        }

        [ContextMenu("Remove End Callback")]
        private void RemoveEndCallback()
        {
            exampleAnimation.RemoveClipEndCallback(clipName, HandleClipEnded);
        }

        [ContextMenu("Remove End Callback", true)]
        private bool ValidateRemoveEndCallback()
        {
            return EditorApplication.isPlaying;
        }

        [ContextMenu("Remove Progress Callback")]
        private void RemoveProgressCallback()
        {
            exampleAnimation.RemoveClipCallback(clipName, ExampleTimelinePosition, HandleClipProgressed);
        }

        [ContextMenu("Remove Progress Callback", true)]
        private bool ValidateRemoveProgressCallback()
        {
            return EditorApplication.isPlaying;
        }
#endif
    }
}
