using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
public class ShipController : MonoBehaviour
{
    private const float GhostDuration = 3f;
    private const float ThrustForce = 5f;
    private const float MaxSpeed = 5f;

    [SerializeField] private GameObject forwardThrustFlame;
    [SerializeField] private GameObject[] reverseThrustFlames;
    [SerializeField] private GameObject damageExplosion;
    [SerializeField] private AudioSource thrustersAudioSource;
    [SerializeField] private AudioSource destructionAudioSource;

    private Rigidbody2D rigidBody;
    private Collider2D shipCollider;
    private List<SpriteRenderer> shipRenderers;
    private Animator animator;
    private Vector2 mousePosition;
    private float thrust;
    private bool destructionAnimationEnded;

    public bool Destroyed { get; private set; }
    
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        shipCollider = GetComponent<Collider2D>();

        shipRenderers = new List<SpriteRenderer>
        {
            GetComponent<SpriteRenderer>(),
            forwardThrustFlame.GetComponent<SpriteRenderer>()
        };
        shipRenderers.AddRange(reverseThrustFlames.Select(o => o.GetComponent<SpriteRenderer>()));
        
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        thrust = Destroyed ? 0 : Input.GetAxisRaw("Vertical");

        ShowOrHideFlames();
        PlayOrStopThrusterSound();

        if (Destroyed && destructionAnimationEnded && destructionAudioSource.isPlaying == false)
            Destroy(gameObject);
    }

    void FixedUpdate()
    {
        MoveShip();
        RotateShip();
    }

    public void TakeDamage()
    {
        Instantiate(damageExplosion, transform.position, Quaternion.identity);
        StartCoroutine(Coroutines.Ghost(shipCollider, shipRenderers, GhostDuration));
    }

    public void DestroyShip()
    {
        Destroyed = true;
        shipCollider.enabled = false;
        animator.SetBool(AnimatorParameters.Destroyed, true);
        destructionAudioSource.Play();
    }

    public void OnDestroyShipAnimationComplete()
        => destructionAnimationEnded = true;

    private void MoveShip()
    {
        rigidBody.AddForce(transform.up * thrust * ThrustForce);
        rigidBody.velocity = Vector2.ClampMagnitude(rigidBody.velocity, MaxSpeed);
    }

    private void RotateShip()
    {
        var direction = mousePosition - rigidBody.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        rigidBody.rotation = angle;
    }

    private void ShowOrHideFlames()
    {
        forwardThrustFlame.SetActive(thrust > 0);

        foreach (var reverseFlame in reverseThrustFlames)
            reverseFlame.SetActive(thrust < 0);
    }

    private void PlayOrStopThrusterSound()
    {
        var playThrusterSound = thrust > 0 || thrust < 0;
        if (playThrusterSound)
        {
            if (!thrustersAudioSource.isPlaying)
                thrustersAudioSource.Play();
        }
        else
        {
            thrustersAudioSource.Stop();
        }
    }
}
