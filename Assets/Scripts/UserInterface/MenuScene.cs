using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScene : MonoBehaviour
{
    [SerializeField] private Text highScoreText;

    private void Start()
    {
        highScoreText.text = SaveSystem.LoadHighScore().ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene(SceneNames.GameScene);
    }
}
