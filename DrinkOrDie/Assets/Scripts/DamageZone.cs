
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    private readonly Dictionary<GameObject, Coroutine> damageCoroutines =
        new Dictionary<GameObject, Coroutine>();
    private PlayerController player;

    private void Awake()
    {
        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null)
            player = playerObject.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy"))
            return;
        GameObject enemy = other.gameObject;
        if (damageCoroutines.ContainsKey(enemy))
            return;

        Coroutine coroutine = StartCoroutine(GetDamage(enemy));
        damageCoroutines.Add(enemy, coroutine);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy"))
            return;

        GameObject enemy = other.gameObject;

        if (damageCoroutines.TryGetValue(enemy, out Coroutine coroutine))
        {
            StopCoroutine(coroutine);
            damageCoroutines.Remove(enemy);
        }
    }

    private void OnDisable()
    {
        foreach (var coroutine in damageCoroutines.Values)
        {
            StopCoroutine(coroutine);
        }
        damageCoroutines.Clear();
    }

    IEnumerator GetDamage(GameObject enemy)
    {
        if (player == null)
            yield break;
        float damage = player.Damage;
        float damageInterval = player.ShootSpeed;

        while (enemy != null)
        {
            yield return new WaitForSeconds(damageInterval);
            if (enemy == null)
                yield break;
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            if (enemyComponent != null)
                enemyComponent.TakeDamage(damage);
        }
        damageCoroutines.Remove(enemy);
    }
}