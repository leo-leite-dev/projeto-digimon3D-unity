using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public WanderArea wanderArea;

    [Header("Enemy Types")]
    public List<DigimonData> enemyTypes;

    [Header("Enemy Base Prefab")]
    public GameObject enemyBasePrefab;

    public int maxEnemies = 3;
    public float respawnTime = 5f;

    public System.Action<DigimonEnemy> OnEnemySpawned;

    private List<GameObject> aliveEnemies = new List<GameObject>();

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
        if (enemyTypes == null || enemyTypes.Count == 0 || wanderArea == null)
            return;

        DigimonData data = enemyTypes[Random.Range(0, enemyTypes.Count)];

        Vector3 randomPos = wanderArea.GetRandomPosition();

        NavMeshHit hit;

        if (!NavMesh.SamplePosition(randomPos, out hit, 10f, NavMesh.AllAreas))
            return;

        Vector3 spawnPos = hit.position;

        GameObject enemy = Instantiate(enemyBasePrefab, spawnPos, Quaternion.identity);

        aliveEnemies.Add(enemy);

        DigimonEnemy digimonEnemy = enemy.GetComponent<DigimonEnemy>();

        if (digimonEnemy != null)
        {
            digimonEnemy.Initialize(data);
            OnEnemySpawned?.Invoke(digimonEnemy);
        }

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
