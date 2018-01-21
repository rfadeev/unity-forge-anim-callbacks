using UnityEngine;

namespace UnityForge.AnimCallbacks.Examples
{
    public class AnimationExample : MonoBehaviour
    {
        [SerializeField]
        private Animation exampleAnimation;
        [SerializeField]
        private string clipName;

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
            exampleAnimation.AddClipCallback(clipName, 0.5f, () =>
            {
                Debug.LogFormat("Clip \"{0}\": callback at 0.5f seconds after start", clipName);
            });
            exampleAnimation.Play(clipName);
        }
    }
}
