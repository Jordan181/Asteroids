using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class ScreenWrapper : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Collider2D objectCollider;
    private ScreenLimits screenLimits;

    private float halfSize;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        objectCollider = GetComponent<Collider2D>();
        screenLimits = ScreenUtils.GetScreenLimits();

        halfSize = objectCollider.bounds.size.magnitude / 2;
    }

    void FixedUpdate()
    {
        if (HeadingAwayFromCenter())
            WrapIfOffScreen();
    }

    private void WrapIfOffScreen()
    {
        var x = rigidBody.position.x;
        var y = rigidBody.position.y;

        if (x + halfSize < screenLimits.XMin)
            x = screenLimits.XMax + halfSize;
        else if (x - halfSize > screenLimits.XMax)
            x = screenLimits.XMin - halfSize;

        if (y + halfSize < screenLimits.YMin)
            y = screenLimits.YMax + halfSize;
        else if (y - halfSize > screenLimits.YMax)
            y = screenLimits.YMin - halfSize;

        rigidBody.position = new Vector2(x, y);
    }

    private bool HeadingAwayFromCenter()
    {
        var directionToCenter = -(Vector2)transform.position.normalized;
        var velocity = rigidBody.velocity.normalized;

        return Vector2.Dot(velocity, directionToCenter) < 0;
    }
}
