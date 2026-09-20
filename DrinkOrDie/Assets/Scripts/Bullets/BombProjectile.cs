using UnityEngine;

public class BombProjectile : Projectile
{
    [SerializeField] private float stopAfter = 0.4f;
    [SerializeField] private float explodeAfter = 1.5f;
    [SerializeField] private float explosionRadius = 2.5f;

    private float age;
    private bool hasStopped;
    private bool hasExploded;

    

    private void Update()
    {
        if (!hasStopped && age >= stopAfter)
        {
            hasStopped = true;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        if (age >= explodeAfter)
        {
            Explode();
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {

    }

    private void Explode()
    {
        hasExploded = true;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
                hit.GetComponent<Enemy>()?.TakeDamage(damage);
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}