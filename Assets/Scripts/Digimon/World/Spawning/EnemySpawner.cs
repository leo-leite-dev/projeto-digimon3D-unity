using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private WanderArea wanderArea;

    [SerializeField]
    private Transform player;

    [Header("Enemy Types")]
    [SerializeField]
    private List<DigimonData> enemyTypes;

    [Header("Enemy Base Prefab")]
    [SerializeField]
    private GameObject enemyBasePrefab;

    [Header("Spawn Settings")]
    [SerializeField]
    private int maxEnemies = 3;

    [SerializeField]
    private float respawnTime = 5f;

    public System.Action<DigimonEnemy> OnEnemySpawned;

    private readonly List<GameObject> aliveEnemies = new();

    void Awake()
    {
        if (wanderArea == null)
            wanderArea = GetComponent<WanderArea>();
    }

    void Start()
    {
        if (!ValidateSpawner())
            return;

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
        if (!CanSpawn())
            return;

        DigimonData data = GetRandomEnemyData();

        if (!SpawnPositionResolver.TryGetValidPosition(wanderArea, out Vector3 position))
        {
            Debug.LogWarning("⚠️ Não encontrou posição válida para spawn", this);
            return;
        }

        GameObject enemyGO = DigimonFactory.Create(enemyBasePrefab, data, position);

        if (enemyGO == null)
        {
            Debug.LogError("❌ Falha ao criar enemyGO", this);
            return;
        }
        var references = enemyGO.GetComponent<DigimonReferences>();

        if (references == null)
        {
            Debug.LogError("❌ DigimonReferences não encontrado", enemyGO);
            Destroy(enemyGO);
            return;
        }

        DigimonEnemyComposer.Compose(enemyGO);

        if (!references.HasCoreReferences())
        {
            Debug.LogError("❌ Enemy inválido após composição", enemyGO);
            Destroy(enemyGO);
            return;
        }

        EnemyInitializer.Initialize(enemyGO, wanderArea, player);

        aliveEnemies.Add(enemyGO);

        var enemy = enemyGO.GetComponent<DigimonEnemy>();
        if (enemy != null)
        {
            OnEnemySpawned?.Invoke(enemy);
        }

        Debug.Log("✅ Enemy spawnado com sucesso", enemyGO);
    }

    bool CanSpawn()
    {
        if (enemyTypes == null || enemyTypes.Count == 0)
        {
            Debug.LogWarning("⚠️ enemyTypes vazio", this);
            return false;
        }

        if (wanderArea == null)
        {
            Debug.LogError("❌ WanderArea não definido", this);
            return false;
        }

        if (enemyBasePrefab == null)
        {
            Debug.LogError("❌ enemyBasePrefab não definido", this);
            return false;
        }

        return true;
    }

    bool ValidateSpawner()
    {
        if (player == null)
        {
            Debug.LogError("❌ Player não atribuído no EnemySpawner", this);
            return false;
        }

        if (wanderArea == null)
        {
            Debug.LogError("❌ WanderArea não atribuído", this);
            return false;
        }

        return true;
    }

    DigimonData GetRandomEnemyData()
    {
        return enemyTypes[Random.Range(0, enemyTypes.Count)];
    }

    public void NotifyEnemyDeath(GameObject enemy)
    {
        if (aliveEnemies.Contains(enemy))
            aliveEnemies.Remove(enemy);

        Invoke(nameof(SpawnEnemy), respawnTime);
    }
}
