using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public WanderArea wanderArea;

    public List<GameObject> enemyPrefabs;

    public int maxEnemies = 3;
    public float respawnTime = 5f;

    private List<GameObject> aliveEnemies = new List<GameObject>();

    public System.Action<DigimonEnemy> OnEnemySpawned;

    void Awake()
    {
        wanderArea = GetComponent<WanderArea>();
    }

    void Start()
    {
        SpawnInitialEnemies();
    }

    void SpawnInitialEnemies()
    {
        for (int i = 0; i < maxEnemies; i++)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

        Vector3 spawnPos = wanderArea.GetRandomPosition();

        GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);

        aliveEnemies.Add(enemy);

        DigimonEnemy digimonEnemy = enemy.GetComponent<DigimonEnemy>();

        if (digimonEnemy != null)
            OnEnemySpawned?.Invoke(digimonEnemy);

        EnemyWander wander = enemy.GetComponent<EnemyWander>();

        if (wander != null)
            wander.Initialize(wanderArea, this);
    }

    public void NotifyEnemyDeath(GameObject enemy)
    {
        aliveEnemies.Remove(enemy);

        Invoke(nameof(SpawnEnemy), respawnTime);
    }
}
