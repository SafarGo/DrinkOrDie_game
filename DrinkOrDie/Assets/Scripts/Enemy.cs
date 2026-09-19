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
        rb.MovePosition(rb.position + dir * MoveSpeed * Time.deltaTime);
    }

    public virtual void TakeDamage(float amount)
    {
        currentHealth -= amount;
        GameObject text = Instantiate(damageNumberPrefab, transform.position, transform.rotation);
        text.GetComponentInChildren<DamageNumber>().SetDamage(amount);
        if (currentHealth <= 0f) Die();
    }

    protected virtual void Die()
    {
        GameManager.Instance.AddExp(ExpDrop);
        Destroy(gameObject);
    }


    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
       
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(GetDamage(collision.gameObject));
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StopCoroutine(GetDamage(collision.gameObject));
        }
    }

    IEnumerator GetDamage(GameObject player)
    {
        while (true)
        {
            yield return new WaitForSeconds(attackDelay);
            player.GetComponent<PlayerController>().Hp -= attackDamage;
        }
    }
}