
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCOntroller : Building
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float fireRate = 1f;
    [SerializeField] float damage;

    private readonly Dictionary<Transform, Coroutine> firingCoroutines =
        new Dictionary<Transform, Coroutine>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy"))
            return;
        Transform enemy = other.transform;

        if (firingCoroutines.ContainsKey(enemy))
            return;

        Coroutine coroutine = StartCoroutine(FireAtEnemy(enemy));
        firingCoroutines.Add(enemy, coroutine);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy"))
            return;
        Transform enemy = other.transform;
        if (firingCoroutines.TryGetValue(enemy, out Coroutine coroutine))
        {
            StopCoroutine(coroutine);
            firingCoroutines.Remove(enemy);
        }
    }

    private void OnDisable()
    {
        foreach (var coroutine in firingCoroutines.Values)
        {
            StopCoroutine(coroutine);
        }
        firingCoroutines.Clear();
    }

    IEnumerator FireAtEnemy(Transform enemyTransform)
    {
        while (enemyTransform != null)
        {
            yield return new WaitForSeconds(fireRate);
            if (enemyTransform == null)
                yield break;
            GameObject bullet = Instantiate(bulletPrefab,transform.position, Quaternion.identity);
            Vector2 direction = (enemyTransform.position - transform.position).normalized;
            bullet.GetComponent<Projectile>().Setup(direction, 10f, 3f);
        }
        firingCoroutines.Remove(enemyTransform);
    }
}