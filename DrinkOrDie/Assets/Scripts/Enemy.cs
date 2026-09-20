
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    public int ExpDrop;
    public float MoveSpeed = 3f;

    [SerializeField] private string enemyName = "Enemy";
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private float contactDamage = 5f;
    [SerializeField] private GameObject damageNumberPrefab;
    [SerializeField] private float attackDelay;
    [SerializeField] private float attackDamage;
    

    private float currentHealth;
    private Transform target;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private Coroutine attackCoroutine;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameObject player = GameObject.Find("Player");

        if (player != null)
            SetTarget(player.transform);
    }

    public void PrepareForSpawn()
    {
        currentHealth = maxHealth;
        attackCoroutine = null;

        GameObject player = GameObject.Find("Player");

        if (player != null)
            SetTarget(player.transform);
    }

    public virtual void SetTarget(Transform player)
    {
        target = player;
    }

    protected virtual void Update()
    {
        if (target == null) return;

        Vector2 dir = ((Vector2)target.position - rb.position).normalized;
        rb.MovePosition(rb.position + dir * MoveSpeed * Time.deltaTime);
        if (dir.x > 0)
            spriteRenderer.flipX = false;
        else if (dir.x < 0)
            spriteRenderer.flipX = true;
    }

    public virtual void TakeDamage(float amount)
    {
        currentHealth -= amount;

        GameObject damageText = Instantiate(
            damageNumberPrefab,
            transform.position,
            transform.rotation
        );

        damageText.GetComponentInChildren<DamageNumber>().SetDamage(amount);

        if (currentHealth <= 0f)
            Die();
    }

    protected virtual void Die()
    {
        GameManager.Instance.AddExp(ExpDrop);
        EnemyPool.Instance.ReturnEnemy(gameObject);
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (attackCoroutine == null)
            {
                attackCoroutine = StartCoroutine(
                    GetDamage(collision.gameObject)
                );
            }
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }
    }

    IEnumerator GetDamage(GameObject player)
    {
        while (player != null)
        {
            yield return new WaitForSeconds(attackDelay);

            if (player == null)
                yield break;

            PlayerController playerController =
                player.GetComponent<PlayerController>();

            if (playerController != null)
                playerController.Hp -= attackDamage;
        }

        attackCoroutine = null;
    }
}