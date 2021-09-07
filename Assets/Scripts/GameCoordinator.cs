using System.Collections;
using UnityEngine;

public class GameCoordinator : SingletonMonoBehaviour<GameCoordinator>
{
    private const float GameOverPanelDelay = 2f;

    [SerializeField] private AsteroidSpawner asteroidSpawner;
    [SerializeField] private ShipController ship;
    [SerializeField] private GameObject gameOverCanvasPrefab;
    [SerializeField] private AudioSource mainMusicSource;

    public bool IsNewHighScore { get; private set; }
    
    public void PlayerHit()
    {
        if (PlayerStats.Lives > 0)
        {
            PlayerStats.Lives--;
            ship.TakeDamage();
        }
        else
            StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        ship.DestroyShip();
        mainMusicSource.Stop();

        if (PlayerStats.Score > SaveSystem.LoadHighScore())
        {
            IsNewHighScore = true;
            SaveSystem.SaveHighScore(PlayerStats.Score);
        }

        yield return new WaitForSeconds(GameOverPanelDelay);

        Instantiate(gameOverCanvasPrefab);
    }
}
