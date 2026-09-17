using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private string enemyName = "Enemy";
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float contactDamage = 5f;
    [SerializeField] private int resourceDrop = 1;

    private float currentHealth;
    private Transform target;
    private Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        SetTarget(GameObject.Find("Player").transform);
    }

    public virtual void SetTarget(Transform player)
    {
        target = player;
    }

    protected virtual void Update()
    {
        if (target == null) return;
        Vector2 dir = ((Vector2)target.position - rb.position).normalized;
        rb.MovePosition(rb.position + dir * moveSpeed * Time.deltaTime);
    }

    public virtual void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0f) Die();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }


    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy attack");
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy attack");
        }
    }
}