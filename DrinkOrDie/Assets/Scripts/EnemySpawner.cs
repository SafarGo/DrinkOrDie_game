using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnInterval = 2f;
    public GameObject[] Enemies;


    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            int randomIndex = Random.Range(0, Enemies.Length);
            Instantiate(Enemies[randomIndex], transform.position, Quaternion.identity);
        }
    }
}
