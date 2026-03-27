using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private WanderArea wanderArea;

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
    public List<DigimonData> EnemyTypes => enemyTypes;

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

        GameObject enemyGO = DigimonFactory.Create(enemyBasePrefab, position, Quaternion.identity);

        if (enemyGO == null)
        {
            Debug.LogError("❌ Falha ao criar enemyGO", this);
            return;
        }

        if (!SetupEnemy(enemyGO, data))
            return;

        DigimonEnemyOverWorldComposer.Compose(enemyGO);

        if (!ValidateComposedEnemy(enemyGO))
            return;

        aliveEnemies.Add(enemyGO);

        var enemy = enemyGO.GetComponent<DigimonEnemy>();
        OnEnemySpawned?.Invoke(enemy);

        Debug.Log("✅ Enemy spawnado com sucesso", enemyGO);
    }

    bool SetupEnemy(GameObject enemyGO, DigimonData data)
    {
        var enemy = enemyGO.GetComponent<DigimonEnemy>();

        if (enemy == null)
        {
            Debug.LogError("❌ DigimonEnemy não encontrado", enemyGO);
            Destroy(enemyGO);
            return false;
        }

        enemy.Setup(data);
        enemyGO.name = data.digimonName;

        Debug.Log($"[SETUP] Enemy {data.digimonName} configurado");

        return true;
    }

    bool ValidateComposedEnemy(GameObject enemyGO)
    {
        var core = enemyGO.GetComponent<DigimonCoreReferences>();

        if (core == null || !core.IsValid())
        {
            Debug.LogError("❌ Enemy inválido após composição", enemyGO);
            Destroy(enemyGO);
            return false;
        }

        if (core.ModelRoot == null || core.ModelRoot.childCount == 0)
        {
            Debug.LogError("❌ Enemy sem model após composição", enemyGO);
            Destroy(enemyGO);
            return false;
        }

        return true;
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
