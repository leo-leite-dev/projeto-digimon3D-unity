using UnityEngine;

public class BattleSceneSpawner : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField]
    private Transform playerSpawnPoint;

    [SerializeField]
    private Transform playerDigimonSpawnPoint;

    [SerializeField]
    private Transform enemySpawnPoint;

    [Header("Base Prefabs (ROOT - lógica)")]
    [SerializeField]
    private GameObject playerBattlePrefab;

    [SerializeField]
    private GameObject playerDigimonBasePrefab;

    [SerializeField]
    private GameObject enemyDigimonBasePrefab;

    public Transform PlayerTransform { get; private set; }
    public Transform PlayerDigimonTransform { get; private set; }
    public Transform EnemyTransform { get; private set; }

    private BattleContext battleContext;

    public void Spawn()
    {
        Debug.Log("🚀 SPAWN INICIADO");

        SpawnPlayer();
        SpawnPlayerDigimon();
        SpawnEnemyDigimon();

        SetupBattleContext();

        Debug.Log("✅ SPAWN FINALIZADO");
    }

    private void SpawnPlayer()
    {
        Debug.Log("🎮 SpawnPlayer");

        if (playerBattlePrefab == null)
        {
            Debug.LogError("❌ playerBattlePrefab NÃO setado");
            return;
        }

        var playerGO = Instantiate(
            playerBattlePrefab,
            playerSpawnPoint.position,
            playerSpawnPoint.rotation
        );

        PlayerTransform = playerGO.transform;

        Debug.Log($"✅ Player criado: {playerGO.name}");
    }

    private void SpawnPlayerDigimon()
    {
        Debug.Log("🐺 SpawnPlayerDigimon");

        var data = CombatContextData.PlayerDigimon;

        if (data == null)
        {
            Debug.LogError("❌ PlayerDigimon NULL");
            return;
        }

        var go = SpawnDigimon(playerDigimonBasePrefab, playerDigimonSpawnPoint, data);

        if (go == null)
            return;

        PlayerDigimonTransform = go.transform;

        Debug.Log($"✅ Player Digimon criado: {go.name}");
    }

    private void SpawnEnemyDigimon()
    {
        Debug.Log("👾 SpawnEnemy");

        var data = CombatContextData.SelectedEnemy;

        if (data == null)
        {
            Debug.LogError("❌ Nenhum inimigo");
            return;
        }

        var go = SpawnDigimon(enemyDigimonBasePrefab, enemySpawnPoint, data);

        if (go == null)
            return;

        EnemyTransform = go.transform;

        Debug.Log($"✅ Enemy criado: {go.name}");
    }

    private GameObject SpawnDigimon(GameObject prefab, Transform spawnPoint, DigimonData data)
    {
        var go = CreateDigimon(prefab, spawnPoint, data);

        if (go == null)
            return null;

        if (!ComposeDigimon(go, data))
        {
            Destroy(go);
            return null;
        }

        return go;
    }

    private GameObject CreateDigimon(GameObject prefab, Transform spawnPoint, DigimonData data)
    {
        if (prefab == null)
        {
            Debug.LogError("❌ Prefab não setado");
            return null;
        }

        var go = DigimonFactory.Create(prefab, spawnPoint.position, spawnPoint.rotation);

        if (go == null)
        {
            Debug.LogError("❌ Falha ao criar Digimon");
            return null;
        }

        var digimon = go.GetComponent<Digimon>();

        if (digimon == null)
        {
            Debug.LogError("❌ Digimon não encontrado", go);
            Destroy(go);
            return null;
        }

        go.name = data.digimonName;

        return go;
    }

    private bool ComposeDigimon(GameObject go, DigimonData data)
    {
        var composer = go.GetComponent<DigimonBattleComposer>();

        if (composer == null)
        {
            Debug.LogError("❌ DigimonBattleComposer não encontrado", go);
            return false;
        }

        composer.Compose(go, data);

        return true;
    }

    private void SetupBattleContext()
    {
        if (PlayerDigimonTransform == null || EnemyTransform == null)
        {
            Debug.LogError("❌ Não é possível criar BattleContext");
            return;
        }

        var playerDigimon = PlayerDigimonTransform.GetComponent<Digimon>();
        var enemyDigimon = EnemyTransform.GetComponent<Digimon>();

        if (playerDigimon == null || enemyDigimon == null)
        {
            Debug.LogError("❌ Digimon não encontrado para BattleContext");
            return;
        }

        battleContext = new BattleContext(playerDigimon, enemyDigimon);

        InjectContext(PlayerDigimonTransform);
        InjectContext(EnemyTransform);

        Debug.Log("⚔️ BattleContext configurado");
    }

    private void InjectContext(Transform target)
    {
        var controller = target.GetComponent<DigimonBattleController>();

        if (controller == null)
        {
            Debug.LogError("❌ DigimonBattleController não encontrado", target);
            return;
        }

        controller.SetContext(battleContext);
    }
}
