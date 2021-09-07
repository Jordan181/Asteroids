using UnityEngine;

public class Bullet : Projectile
{
    [SerializeField] private float maxActiveTime = 1f;

    private float currentActiveTime;

    private void Update()
    {
        currentActiveTime += Time.deltaTime;

        if (currentActiveTime >= maxActiveTime)
            Destroy(gameObject);
    }
}
