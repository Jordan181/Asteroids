using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
public abstract class Powerup : MonoBehaviour
{
    private const float maxTimeAvailable = 20f;

    [SerializeField] private AudioSource collectedAudioSource;

    private Collider2D powerupCollider;
    private SpriteRenderer powerupRenderer;
    private float timeSinceSpawn;
    private bool pickedUp;

    protected GameObject Ship { get; private set; }

    private void Start()
    {
        powerupCollider = GetComponent<Collider2D>();
        powerupRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        timeSinceSpawn += Time.deltaTime;

        if (timeSinceSpawn >= maxTimeAvailable && !pickedUp)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.gameObject.tag == Tags.Ship)
        {
            Ship = otherCollider.gameObject;
            
            collectedAudioSource.Play();
            HidePowerup();
            StartCoroutine(PowerupSequence());

            pickedUp = true;
        }
    }

    private void HidePowerup()
    {
        if (powerupCollider != null && powerupRenderer != null)
        {
            powerupCollider.enabled = false;
            powerupRenderer.enabled = false;
        }
    }

    protected abstract IEnumerator PowerupSequence();
}
