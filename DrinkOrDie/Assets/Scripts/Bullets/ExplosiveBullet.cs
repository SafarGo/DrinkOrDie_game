using UnityEngine;

public class ExplosiveBullet : Projectile
{
    [SerializeField] private float explosionRadius = 2f;

    protected void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Enemy")) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                return;
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}