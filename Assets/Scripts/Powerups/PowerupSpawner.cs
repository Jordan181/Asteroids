using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    private const float powerupTime = 10f;
    private const float startForce = 10f;

    [SerializeField] private GameObject[] powerupPrefabs;

    private float timeSinceLastPowerup;
    private ScreenLimits screenLimits;

    private void Start()
    {
        screenLimits = ScreenUtils.GetScreenLimits();
    }

    private void FixedUpdate()
    {
        timeSinceLastPowerup += Time.fixedDeltaTime;

        if (timeSinceLastPowerup > powerupTime)
        {
            SpawnRandomPowerup();
            timeSinceLastPowerup = 0;
        }
    }

    private void SpawnRandomPowerup()
    {
        var powerup = RandomUtils.SelectRandomItem(powerupPrefabs);
        var x = Random.Range(screenLimits.XMin, screenLimits.XMax);
        var y = Random.Range(screenLimits.YMin, screenLimits.YMax);
        var angle = Random.Range(0, 360);
        var direction = VectorExtensions.Rotate(Vector2.right, angle);

        Instantiate(powerup, new Vector3(x, y), Quaternion.identity)
            .GetComponent<Rigidbody2D>()
            .AddForce(direction * startForce);
    }
}
