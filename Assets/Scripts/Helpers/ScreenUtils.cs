using UnityEngine;

public static class ScreenUtils
{
    private static ScreenLimits screenLimits;

    public static ScreenLimits GetScreenLimits()
    {
        if (screenLimits != null)
            return screenLimits;

        var mainCamera = Camera.main;
        Vector2 minPoint = mainCamera.ScreenToWorldPoint(new Vector3(0, 0));
        Vector2 maxPoint = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        
        var xMin = minPoint.x;
        var xMax = maxPoint.x;
        var yMin = minPoint.y;
        var yMax = maxPoint.y;

        screenLimits = new ScreenLimits(xMin, xMax, yMin, yMax);
        return screenLimits;
    }
}