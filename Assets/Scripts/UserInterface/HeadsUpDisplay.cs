using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HeadsUpDisplay : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject livesContainer;
    [SerializeField] private GameObject lifePrefab;

    private int displayedScore;
    private int displayedLives;
    private readonly List<Life> lives = new List<Life>();

    private void Start()
    {
        displayedScore = PlayerStats.Score;
        displayedLives = PlayerStats.Lives;

        lives.AddRange(livesContainer.GetComponentsInChildren<Life>());
    }

    private void OnGUI()
    {
        if (PlayerStats.Score != displayedScore)
            UpdateScore();

        if (PlayerStats.Lives != displayedLives)
            UpdateLives();
    }

    private void UpdateScore()
    {
        var newScore = PlayerStats.Score;
        scoreText.text = newScore.ToString();
        displayedScore = newScore;
    }

    private void UpdateLives()
    {
        var newLives = PlayerStats.Lives;

        if (newLives < displayedLives)
        {
            var lifeToRemove = lives.Last();
            lifeToRemove.StartRemovingLife();
            lives.Remove(lifeToRemove);

            displayedLives--;
        }
        else
        {
            var life = Instantiate(lifePrefab, livesContainer.transform).GetComponent<Life>();
            lives.Add(life);

            displayedLives++;
        }
    }
}
