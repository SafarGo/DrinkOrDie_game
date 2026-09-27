using System.Collections;
using UnityEngine;

public class Boss : Enemy
{
    [Header("Движение")]
    [SerializeField] private float bossMoveSpeed = 2f;
    [SerializeField] private float stopDistance = 2f;

    [Header("Атаки")]
    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private float attackRange = 8f;

    [Header("Атака 1: круговой залп")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int projectilesPerVolley = 12;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private float projectileLifetime = 5f;

    [Header("Атака 2: дэш")]
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.4f;
    [SerializeField] private float dashDamage = 20f;

    [Header("Атака 3: удар по земле")]
    [SerializeField] private float slamRadius = 3f;
    [SerializeField] private float slamDamage = 15f;

    [Header("Анимации")]
    [SerializeField] private Animator animator;

    private bool isAttacking = false;
    private bool isDead = false;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        StartCoroutine(AI());
    }

    protected override void Update()
    {
        if (isDead) return;
        if (isAttacking) return;
        if (target == null) return;

        float dx = target.position.x - transform.position.x;
        if (dx > 0.1f) spriteRenderer.flipX = false;
        else if (dx < -0.1f) spriteRenderer.flipX = true;
    }

    protected override void OnCollisionEnter2D(Collision2D collision) { }
    protected override void OnCollisionStay2D(Collision2D collision) { }
    protected override void OnCollisionExit2D(Collision2D collision) { }

    private IEnumerator AI()
    {
        while (!isDead)
        {
            if (target == null) yield break;

            while (Vector2.Distance(transform.position, target.position) > stopDistance && !isDead)
            {
                if (isAttacking) { yield return null; continue; }

                Vector2 dir = ((Vector2)target.position - (Vector2)transform.position).normalized;
                transform.position += (Vector3)(dir * bossMoveSpeed * Time.deltaTime);

                if (animator != null) animator.SetFloat("Speed", 1);
                yield return null;
            }

            if (isDead) yield break;

            if (animator != null) animator.SetFloat("Speed", 0);

            yield return StartCoroutine(ChooseAndAttack());

            yield return new WaitForSeconds(attackCooldown);
        }
    }

    private IEnumerator ChooseAndAttack()
    {
        int attack = Random.Range(0, 3);

        switch (attack)
        {
            case 0: yield return StartCoroutine(CircularVolley()); break;
            case 1: yield return StartCoroutine(DashAttack()); break;
            case 2: yield return StartCoroutine(GroundSlam()); break;
        }
    }

    private IEnumerator CircularVolley()
    {
        isAttacking = true;

        yield return new WaitForSeconds(0.5f);

        float angleStep = 360f / projectilesPerVolley;
        for (int i = 0; i < projectilesPerVolley; i++)
        {
            float angle = angleStep * i;
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Projectile p = proj.GetComponent<Projectile>();
            if (p != null) p.Setup(dir, projectileSpeed, projectileLifetime);
        }
        Debug.Log("Шмаляю");
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
    }

    private IEnumerator DashAttack()
    {
        isAttacking = true;
        animator.SetTrigger("Dash");
        yield return new WaitForSeconds(0.4f);

        if (target == null || isDead) { isAttacking = false; yield break; }

        Vector2 dashDir = ((Vector2)target.position - (Vector2)transform.position).normalized;

        float elapsed = 0f;
        while (elapsed < dashDuration && !isDead)
        {
            transform.position += (Vector3)(dashDir * dashSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (target != null && !isDead &&
            Vector2.Distance(transform.position, target.position) < 1f)
        {
            var pc = target.GetComponent<PlayerController>();
            if (pc != null) pc.Hp -= dashDamage;
        }

        yield return new WaitForSeconds(0.3f);
        isAttacking = false;
    }

    private IEnumerator GroundSlam()
    {
        isAttacking = true;

        yield return new WaitForSeconds(0.6f);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, slamRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                var pc = hit.GetComponent<PlayerController>();
                if (pc != null) pc.Hp -= slamDamage;
            }
        }

        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
    }

    public override void TakeDamage(float amount)
    {
        if (isDead) return;

        base.TakeDamage(amount);

        if (animator != null) animator.SetTrigger("Hit");
    }

    protected override void Die()
    {
        if (isDead) return;
        isDead = true;

        if (animator != null) animator.SetBool("Die", true);

        GameManager.Instance.AddExp(ExpDrop);
        Destroy(gameObject, 1.5f);
        StartCoroutine(DieBossAnim());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, slamRadius);
    }

    IEnumerator DieBossAnim()
    {
        yield return new WaitForSeconds(1);
        UnityEngine.SceneManagement.SceneManager.LoadScene("WinScene");
    }
}