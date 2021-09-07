using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    private const string FileName = "asteroids.dat";
    private static readonly string FilePath = Path.Combine(Application.persistentDataPath, FileName);
    private static readonly BinaryFormatter Formatter = new BinaryFormatter();

    private static int? highScore;
    
    public static void SaveHighScore(int score)
    {
        var saveData = new SaveData(score);

        using (var stream = new FileStream(FilePath, FileMode.Create))
        {
            Formatter.Serialize(stream, saveData);
        }

        highScore = score;
    }

    public static int LoadHighScore()
    {
        if (!highScore.HasValue)
        {
            if (File.Exists(FilePath))
            {
                using (var stream = new FileStream(FilePath, FileMode.Open))
                {
                    var data = (SaveData) Formatter.Deserialize(stream);
                    highScore = data.Score;
                }
            }
            else
            {
                highScore = 0;
            }
        }

        return highScore.Value;
    }
}