using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] protected float lifetime = 3f;
    [SerializeField] protected float damage;

    protected float speed;
    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void Setup(Vector2 direction, float dmg, float spd, float life)
    {
        damage = dmg;
        speed = spd;
        lifetime = life;

        rb.velocity = direction * speed;
        Destroy(gameObject, lifetime);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}