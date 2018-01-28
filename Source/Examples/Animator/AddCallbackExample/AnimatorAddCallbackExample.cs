using UnityEngine;

namespace UnityForge.AnimCallbacks.Examples
{
    public class AnimatorAddCallbackExample : MonoBehaviour
    {
        private const float ExampleTimelinePosition = 0.5f;

        [SerializeField]
        private Animator animator;
        [SerializeField]
        private int layerIndex;
        [SerializeField]
        private string clipName;

        private void Start()
        {
            animator.AddClipStartCallback(layerIndex, clipName, () =>
            {
                Debug.LogFormat("Clip \"{0}\": started", clipName);
            });
            animator.AddClipEndCallback(layerIndex, clipName, () =>
            {
                Debug.LogFormat("Clip \"{0}\": ended", clipName);
            });
            animator.AddClipCallback(layerIndex, clipName, ExampleTimelinePosition, () =>
            {
                Debug.LogFormat("Clip \"{0}\": callback at {1} seconds after start", clipName, ExampleTimelinePosition);
            });
        }
    }
}
