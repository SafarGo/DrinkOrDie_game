
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    public float spawnInterval = 2f;
    public GameObject[] Enemies;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        EnemyPool.Instance.Initialize(Enemies);
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (Enemies == null || Enemies.Length == 0)
                continue;

            int randomIndex = Random.Range(0, Enemies.Length);

            EnemyPool.Instance.GetEnemy(
                Enemies[randomIndex],
                transform.position
            );
        }
    }
}