using UnityEngine;

public static class ColorExtensions
{
    public static Color ChangeAlpha(this Color color, float a)
        => new Color(color.r, color.g, color.b, a);
}