using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
public abstract class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject explosion;
    [SerializeField] private AudioSource audioSource;

    private SpriteRenderer spriteRenderer;
    private Collider2D projectileCollider;
    private bool destroy;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        projectileCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (destroy && audioSource.isPlaying == false)
            Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(explosion, transform.position, Quaternion.identity);

        if (spriteRenderer != null && projectileCollider != null)
        {
            spriteRenderer.enabled = false;
            projectileCollider.enabled = false;
        }

        destroy = true;
    }
}
