using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int startScore = 0;
    [SerializeField] private int startLives = 3;

    public static int Score { get; set; }
    public static int Lives { get; set; }

    void Awake()
    {
        Score = startScore;
        Lives = startLives;
    }
}
