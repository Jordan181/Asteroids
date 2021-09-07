using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private bool animationEnded;

    private void Update()
    {
        if (animationEnded && audioSource.isPlaying == false)
            Destroy(gameObject);
    }
    
    public void OnExplosionAnimationEnd()
        => animationEnded = true;
}
