using UnityEngine;

public class OrbitingProjectile : Projectile
{
    private Transform owner;
    private float orbitRadius;
    private float orbitSpeed;
    private float currentAngle;

    public override void Setup(Vector2 direction, float spd, float life)
    {
        speed = spd;
        lifetime = life;
        currentAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Destroy(gameObject, lifetime);
    }

    public void SetOrbit(Transform player, float radius, float speedDegPerSec)
    {
        owner = player;
        orbitRadius = radius;
        orbitSpeed = speedDegPerSec;
        UpdatePosition();
    }

    private void Update()
    {
        if (owner == null) return;

        currentAngle += orbitSpeed * Time.deltaTime;
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        float rad = currentAngle * Mathf.Deg2Rad;
        Vector2 offset = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * orbitRadius;
        transform.position = (Vector2)owner.position + offset;
    }
}