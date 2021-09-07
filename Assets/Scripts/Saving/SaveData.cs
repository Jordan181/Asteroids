using System;

[Serializable]
public class SaveData
{
    public int Score { get; }

    public SaveData(int score)
    {
        Score = score;
    }
}