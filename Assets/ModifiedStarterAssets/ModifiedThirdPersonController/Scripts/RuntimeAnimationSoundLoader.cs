using UnityEngine;

public class RuntimeAnimationSoundLoader : MonoBehaviour
{
    private void OnFootstep(AnimationEvent animationEvent)
    {
        transform.GetComponentInParent<StarterAssets.ThirdPersonController>().OnFootstep(animationEvent);
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        transform.GetComponentInParent<StarterAssets.ThirdPersonController>().OnLand(animationEvent);
    }
}