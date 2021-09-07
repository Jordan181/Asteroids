using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour
{
    #region Score Values

    private static readonly IReadOnlyDictionary<AsteroidSize, int> AsteroidScoreValues = new Dictionary<AsteroidSize, int>
    {
        {AsteroidSize.Small, 50},
        {AsteroidSize.Medium, 20},
        {AsteroidSize.Large, 10}
    }; 

    private static readonly IReadOnlyDictionary<AsteroidSize, int> TotalAsteroidScoreValues = new Dictionary<AsteroidSize, int>
    {
        {AsteroidSize.Small, 80},
        {AsteroidSize.Medium, 30},
        {AsteroidSize.Large, 10}
    };

    #endregion

    [SerializeField] private GameObject dustExplosionPrefab;
    [SerializeField] private AsteroidSize asteroidSize;

    private const float AsteroidCollisionsDelay = 1f;
    private bool asteroidCollisionsEnabled;
    private GameCoordinator gameCoordinator;
    
    public AsteroidSize AsteroidSize => asteroidSize;
    public Rigidbody2D Rigidbody { get; private set; }
    public bool HasBeenShot { get; private set; }
    public bool HasBeenShotByMissile { get; private set; }

    private void Start()
    {
        gameCoordinator = GameCoordinator.Instance;
        Rigidbody = GetComponent<Rigidbody2D>();

        // Prevents multiple explosions instantiated if asteroids spawned in same location
        StartCoroutine(EnableAsteroidCollisionsAfterDelay());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var collidingTag = collision.gameObject.tag;

        switch (collidingTag)
        {
            case Tags.Bullet:
                HasBeenShot = true;
                PlayerStats.Score += AsteroidScoreValues[asteroidSize];
                break;
            case Tags.Missile:
                HasBeenShotByMissile = true;
                PlayerStats.Score += TotalAsteroidScoreValues[asteroidSize];
                break;
            case Tags.Ship:
                gameCoordinator.PlayerHit();
                break;
            case Tags.Asteroid:
                if (asteroidCollisionsEnabled)
                    Instantiate(dustExplosionPrefab, transform.position, Quaternion.identity);
                break;
        }
    }

    private IEnumerator EnableAsteroidCollisionsAfterDelay()
    {
        yield return new WaitForSeconds(AsteroidCollisionsDelay);

        asteroidCollisionsEnabled = true;
    }
}