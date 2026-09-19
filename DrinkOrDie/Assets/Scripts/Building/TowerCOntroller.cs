using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCOntroller : Building
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float fireRate = 1f;
    [SerializeField] float damage;

    private void OnTriggerEnter2D(Collider2D other)
    { 
        if (other.CompareTag("Enemy"))
        {
            StartCoroutine(FireAtEnemy(other.gameObject.transform));
        }
    }


    IEnumerator FireAtEnemy(Transform enemyTransform)
    {
        while (true)
        {
            if (enemyTransform != null)
            {
                Debug.Log("Firing at enemy");
                yield return new WaitForSeconds(fireRate);
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                Vector2 direction = (enemyTransform.position - transform.position).normalized;
                bullet.GetComponent<Projectile>().Setup(direction, damage, 10f, 3f);
            }
        }
    }
}
