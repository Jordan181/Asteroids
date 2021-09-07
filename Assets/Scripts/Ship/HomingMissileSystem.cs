using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HomingMissileSystem : MonoBehaviour
{
    [SerializeField] private Text launchMissileText;
    [SerializeField] private GameObject missilePrefab;

    public void Launch(IReadOnlyList<Transform> missileSpawnPoints)
    {
        launchMissileText.enabled = false;

        var targets = FindTargets(missileSpawnPoints.Count);

        for (var i = 0; i < missileSpawnPoints.Count; i++)
        {
            FireMissile(missileSpawnPoints[i], targets[i]);
        }
        
        Destroy(gameObject);
    }

    private static List<GameObject> FindTargets(int requiredTargets)
    {
        var asteroids = FindObjectsOfType<Asteroid>()
            .OrderByDescending(asteroid => asteroid.AsteroidSize)
            .Select(asteroid => asteroid.gameObject)
            .ToList();

        var targets = new List<GameObject>(requiredTargets);

        if (asteroids.Count < requiredTargets)
        {
            var duplicateTargets = requiredTargets - asteroids.Count;
            targets.AddRange(asteroids);
            targets.AddRange(Enumerable.Repeat(asteroids.Last(), duplicateTargets));
        }
        else
        {
            targets.AddRange(asteroids.Take(requiredTargets));
        }

        return targets;
    }

    private void FireMissile(Transform spawnPoint, GameObject target)
    {
        Instantiate(missilePrefab, spawnPoint.position, spawnPoint.rotation)
            .GetComponent<HomingMissile>()
            .Track(target);
    }
}