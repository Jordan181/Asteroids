using Random = UnityEngine.Random;

public static class RandomUtils
{
    public static T SelectRandomItem<T>(T[] items)
        => items[Random.Range(0, items.Length)];
}