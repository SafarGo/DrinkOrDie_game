using System.Collections;
using UnityEngine;
public class PlayerController : MonoBehaviour 
{
    public float Hp;
    public float ShootSpeed;
    public Transform FirePoint;
    public float Damage;

    [SerializeField] private float speed;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int numberOftilesForShoot = 8;
    [SerializeField] private float projectileDamage = 5f;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float projectileLifetime = 3f;
    [SerializeField] private float startAngle = 0f;

    private Rigidbody2D rb; 
    private Vector2 movement;
    
    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>(); 
    } 

    void Start()
    {
        StartCoroutine(Shooting());
    }   
    
    private void Update() {
        movement.x = Input.GetAxisRaw("Horizontal"); 
        movement.y = Input.GetAxisRaw("Vertical"); 
        movement = movement.normalized; 
    } 
    private void FixedUpdate() {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime); 
    } 

    public void Shoot()
    {
        float angleStep = 360f / numberOftilesForShoot;

        for (int i = 0; i < numberOftilesForShoot; i++)
        {
            float angle = startAngle + angleStep * i;
            Vector2 dir = AngleToDirection(angle);
            SpawnProjectile(dir);
        }
    }

    private void SpawnProjectile(Vector2 dir)
    {
        Vector3 spawnPos = FirePoint != null ? FirePoint.position : transform.position;

        GameObject go = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        Projectile proj = go.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.Setup(dir, projectileDamage, projectileSpeed, projectileLifetime);
        }
    }

    private Vector2 AngleToDirection(float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }
    IEnumerator Shooting()
    {
        while (true)
        {
            yield return new WaitForSeconds(ShootSpeed);
            Shoot();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(Damage);
        }
    }
}