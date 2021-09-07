using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverCanvas : MonoBehaviour
{
    [SerializeField] private Text newHighScoreText;
    [SerializeField] private AudioSource gameOverSoundEffectSource;
    [SerializeField] private AudioSource gameOverMusicSource;

    void Start()
    {
        StartCoroutine(PlayMusicWhenSoundEffectFinished());

        if (GameCoordinator.Instance.IsNewHighScore)
        {
            newHighScoreText.gameObject.SetActive(true);
        }
    }

    private IEnumerator PlayMusicWhenSoundEffectFinished()
    {
        yield return new WaitUntil(() => gameOverSoundEffectSource.isPlaying == false);

        gameOverMusicSource.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
            SceneManager.LoadScene(SceneNames.GameScene);
        else if (Input.GetKeyDown(KeyCode.N))
            SceneManager.LoadScene(SceneNames.MenuScene);
    }
}
