using UnityEngine;

namespace UnityForge.AnimCallbacks.Examples
{
    public class AnimatorExample : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private int layerIndex;
        [SerializeField]
        private string clipName;

        private void Start()
        {
            animator.OnClipStart(layerIndex, clipName, () => Debug.LogFormat("{0} clip started", clipName));
            animator.OnClipEnd(layerIndex, clipName, () => Debug.LogFormat("{0} clip ended", clipName));
        }
    }
}
