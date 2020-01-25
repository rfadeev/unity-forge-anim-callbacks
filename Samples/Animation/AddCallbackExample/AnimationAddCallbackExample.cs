using UnityEngine;

namespace UnityForge.AnimCallbacks.Examples
{
    public class AnimationAddCallbackExample : MonoBehaviour
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
            exampleAnimation.AddClipStartCallback(clipName, () =>
            {
                Debug.LogFormat("Clip \"{0}\": started", clipName);
            });
            exampleAnimation.AddClipEndCallback(clipName, () =>
            {
                Debug.LogFormat("Clip \"{0}\": ended", clipName);
            });
            exampleAnimation.AddClipCallback(clipName, ExampleTimelinePosition, () =>
            {
                Debug.LogFormat("Clip \"{0}\": callback at {1} seconds after start", clipName, ExampleTimelinePosition);
            });
            exampleAnimation.Play(clipName);
        }
    }
}
