using UnityEngine;

public static class VectorExtensions
{
    public static Vector2 Rotate(this Vector2 vector, float angle)
        => Quaternion.Euler(0, 0, angle) * vector;
}
