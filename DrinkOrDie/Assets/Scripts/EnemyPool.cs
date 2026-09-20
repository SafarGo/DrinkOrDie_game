
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance { get; private set; }

    [SerializeField] private int initialSizePerEnemy = 30;

    private readonly Dictionary<GameObject, Queue<GameObject>> pools =
        new Dictionary<GameObject, Queue<GameObject>>();

    private readonly Dictionary<GameObject, GameObject> prefabByInstance =
        new Dictionary<GameObject, GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    public void Initialize(GameObject[] prefabs)
    {
        foreach (GameObject prefab in prefabs)
        {
            if (prefab == null || pools.ContainsKey(prefab))
                continue;

            pools[prefab] = new Queue<GameObject>();

            for (int i = 0; i < initialSizePerEnemy; i++)
            {
                CreateEnemy(prefab);
            }
        }
    }


    private GameObject CreateEnemy(GameObject prefab)
    {
        GameObject enemy = Instantiate(prefab);
        enemy.SetActive(false);

        prefabByInstance[enemy] = prefab;

        return enemy;
    }

    public GameObject GetEnemy(GameObject prefab, Vector3 position)
    {
        if (prefab == null || !pools.ContainsKey(prefab))
            return null;

        Queue<GameObject> pool = pools[prefab];

        GameObject enemy = null;

        while (pool.Count > 0 && enemy == null)
        {
            enemy = pool.Dequeue();
        }

        if (enemy == null)
            enemy = CreateEnemy(prefab);

        enemy.transform.position = position;
        enemy.transform.rotation = Quaternion.identity;

        Enemy enemyComponent = enemy.GetComponent<Enemy>();

        if (enemyComponent != null)
            enemyComponent.PrepareForSpawn();

        enemy.SetActive(true);

        return enemy;
    }

    public void ReturnEnemy(GameObject enemy)
    {
        if (enemy == null || !prefabByInstance.TryGetValue(enemy, out GameObject prefab))
            return;

        enemy.SetActive(false);
        pools[prefab].Enqueue(enemy);
    }
}