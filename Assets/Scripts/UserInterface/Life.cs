using UnityEngine;

public class Life : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void StartRemovingLife()
    {
        animator.SetBool(AnimatorParameters.LostLife, true);
    }

    public void OnLifeFlashingAnimationComplete()
    {
        Destroy(gameObject);
    }
}
