using UnityEngine;

public class HomingMissile : Projectile
{
    private const float Speed = 3f;
    private const float RotationSpeed = 150f;

    private Rigidbody2D rigidBody;
    private Transform missileTarget;

    protected override void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        base.Start();
    }
    
    public void Track(GameObject target)
    {
        missileTarget = target.transform;
    }

    private void FixedUpdate()
    {
        if (missileTarget == null)
            return;

        var directionToTarget = (Vector2)missileTarget.position - rigidBody.position;
        var rotationAmount = -Vector3.Cross(directionToTarget.normalized, transform.up).z;

        rigidBody.angularVelocity = rotationAmount * RotationSpeed;
        rigidBody.velocity = transform.up * Speed;
    }
}