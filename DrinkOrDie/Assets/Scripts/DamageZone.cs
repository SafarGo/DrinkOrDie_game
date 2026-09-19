using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            StartCoroutine(GetDamage());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            StopCoroutine(GetDamage());
        }
    }

    IEnumerator GetDamage()
    {
        float damage = GameObject.Find("Player").GetComponent<PlayerController>().Damage;
        float damageInterval = GameObject.Find("Player").GetComponent<PlayerController>().ShootSpeed;
        while (true)
        {
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
