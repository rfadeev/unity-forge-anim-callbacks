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
            exampleAnimation.OnClipStart(clipName, () => Debug.LogFormat("{0} clip started", clipName));
            exampleAnimation.OnClipEnd(clipName, () => Debug.LogFormat("{0} clip ended", clipName));
            exampleAnimation.Play(clipName);
        }
    }
}
