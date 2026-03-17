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

        if (!NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            return;

        GameObject enemyGO = DigimonFactory.Create(
            enemyBasePrefab,
            hit.position,
            Quaternion.identity
        );

        if (enemyGO == null)
            return;

        var digimon = enemyGO.GetComponent<Digimon>();
        if (digimon != null)
            digimon.Setup(data);
        else
        {
            return;
        }

        var view = enemyGO.GetComponent<DigimonEnemyView>();
        if (view != null)
            view.SpawnModel(data);
        else
            return;

        DigimonComposer.Compose(enemyGO);

        var wander = enemyGO.GetComponent<EnemyWander>();
        if (wander != null)
            wander.Initialize(new WanderContext(wanderArea, this));
        else
            Debug.LogWarning("⚠️ Enemy sem EnemyWander", enemyGO);

        aliveEnemies.Add(enemyGO);

        var enemy = enemyGO.GetComponent<DigimonEnemy>();
        if (enemy != null)
            OnEnemySpawned?.Invoke(enemy);
    }

    public void NotifyEnemyDeath(GameObject enemy)
    {
        if (aliveEnemies.Contains(enemy))
            aliveEnemies.Remove(enemy);

        Invoke(nameof(SpawnEnemy), respawnTime);
    }
}
