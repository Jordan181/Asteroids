using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    private const float AsteroidStartForce = 1000f;
    private const float AsteroidRotationTorque = 1500f;
    private const float DirectionVariation = 25f;

    [SerializeField] private int startNumberOfAsteroids;
    [SerializeField] private GameObject[] largeAsteroidPrefabs;
    [SerializeField] private GameObject[] mediumAsteroidPrefabs;
    [SerializeField] private GameObject[] smallAsteroidPrefabs;
    [SerializeField] private Transform[] spawnPoints;

    private IReadOnlyDictionary<AsteroidSize, GameObject[]> splitAsteroidPrefabs;
    
    private List<Asteroid> asteroids;
    private Vector2[] spawnPointDirections;
    private int asteroidsToSpawn;

    void Start()
    {
        asteroids = new List<Asteroid>();
        asteroidsToSpawn = startNumberOfAsteroids;
        spawnPointDirections = CacheSpawnPointDirections();

        splitAsteroidPrefabs = new Dictionary<AsteroidSize, GameObject[]>
        {
            {AsteroidSize.Large, mediumAsteroidPrefabs},
            {AsteroidSize.Medium, smallAsteroidPrefabs}
        };
    }

    private Vector2[] CacheSpawnPointDirections()
    {
        var directions = new Vector2[spawnPoints.Length];

        for (var i = 0; i < directions.Length; i++)
        {
            directions[i] = -spawnPoints[i].position.normalized;
        }

        return directions;
    }

    void FixedUpdate()
    {
        SplitOrDestroyAsteroids();

        if (asteroids.Count == 0)
        {
            SpawnNewAsteroids();
            asteroidsToSpawn++;
        }
    }

    private void SpawnNewAsteroids()
    {
        for (var i = 0; i < asteroidsToSpawn; i++)
        {
            var spawnIndex = Random.Range(0, spawnPoints.Length);
            var spawnPoint = spawnPoints[spawnIndex];
            var asteroidToSpawn = RandomUtils.SelectRandomItem(largeAsteroidPrefabs);
            var directionVariation = Random.Range(-DirectionVariation, DirectionVariation);
            var direction = spawnPointDirections[spawnIndex].Rotate(directionVariation);

            InstantiateAndPush(asteroidToSpawn, spawnPoint.position, direction);
        }
    }

    private void SplitOrDestroyAsteroids()
    {
        for (var i = asteroids.Count - 1; i > -1; i--)
        {
            var asteroid = asteroids[i];

            if (asteroid.HasBeenShot && asteroid.AsteroidSize == AsteroidSize.Small || asteroid.HasBeenShotByMissile)
            {
                DestroyAsteroid(asteroid);
            }
            else if (asteroid.HasBeenShot)
            {
                SplitAsteroid(asteroid);
            }
        }
    }

    private void SplitAsteroid(Asteroid asteroid)
    {
        var smallerAsteroids = splitAsteroidPrefabs[asteroid.AsteroidSize];
        
        var largerAsteroidDirection = asteroid.Rigidbody.velocity.normalized;
        var largerAsteroidPosition = asteroid.transform.position;
        var directionChangeAngle = Random.Range(20, 70);

        DestroyAsteroid(asteroid);
        
        for (var i = 0; i < 2; i++)
        {
            var asteroidToSpawn = RandomUtils.SelectRandomItem(smallerAsteroids);
            var direction = largerAsteroidDirection.Rotate(directionChangeAngle);
            var position = largerAsteroidPosition + new Vector3(direction.x, direction.y);
            
            InstantiateAndPush(asteroidToSpawn, position, direction);

            directionChangeAngle = 360 - directionChangeAngle;
        }
    }

    private void DestroyAsteroid(Asteroid asteroid)
    {
        asteroids.Remove(asteroid);
        Destroy(asteroid.gameObject);
    }

    private void InstantiateAndPush(GameObject asteroid, Vector3 position, Vector2 direction)
    {
        var asteroidObject = Instantiate(asteroid, position, Quaternion.identity);
        var rigidBody = asteroidObject.GetComponent<Rigidbody2D>();

        rigidBody.AddForce(direction * AsteroidStartForce, ForceMode2D.Impulse);
        rigidBody.AddTorque(Random.Range(-AsteroidRotationTorque, AsteroidRotationTorque));
        
        asteroids.Add(asteroidObject.GetComponent<Asteroid>());
    }
}
