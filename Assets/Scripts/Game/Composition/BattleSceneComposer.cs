using UnityEngine;

public class BattleSceneComposer : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private BattleSceneSpawner spawner;

    [SerializeField]
    private CombatCameraController cameraController;

    [SerializeField]
    private BattleContext battleContext;

    private void Start()
    {
        Debug.Log("🚀 BattleSceneComposer START");

        if (spawner == null)
        {
            Debug.LogError("❌ Spawner NÃO está setado no Inspector");
            return;
        }

        if (cameraController == null)
        {
            Debug.LogError("❌ CameraController NÃO está setado no Inspector");
            return;
        }

        ComposeBattle();
    }

    private void ComposeBattle()
    {
        Debug.Log("⚙️ ComposeBattle INICIADO");

        // 🔍 CHECK CONTEXT DATA
        Debug.Log($"[CTX] Tamer: {CombatContextData.TamerStats}");
        Debug.Log($"[CTX] PlayerDigimon: {CombatContextData.PlayerDigimon}");
        Debug.Log($"[CTX] Enemy: {CombatContextData.SelectedEnemy}");

        if (CombatContextData.TamerStats == null)
            Debug.LogError("❌ TamerStats NULL");

        if (CombatContextData.PlayerDigimon == null)
            Debug.LogError("❌ PlayerDigimon NULL");

        if (CombatContextData.SelectedEnemy == null)
            Debug.LogError("❌ SelectedEnemy NULL");

        // 🔥 SPAWN
        Debug.Log("🧪 Chamando Spawn()");
        spawner.Spawn();

        // 🔍 CHECK TRANSFORMS
        Debug.Log($"[SPAWN] PlayerTransform: {spawner.PlayerTransform}");
        Debug.Log($"[SPAWN] PlayerDigimonTransform: {spawner.PlayerDigimonTransform}");
        Debug.Log($"[SPAWN] EnemyTransform: {spawner.EnemyTransform}");

        if (spawner.PlayerDigimonTransform == null || spawner.EnemyTransform == null)
        {
            Debug.LogError("❌ Spawn falhou - transforms NULL");
            return;
        }

        // 🔍 GET DIGIMONS
        var playerDigimon = spawner.PlayerDigimonTransform.GetComponent<Digimon>();
        var enemyDigimon = spawner.EnemyTransform.GetComponent<Digimon>();

        Debug.Log($"[DIGIMON] PlayerDigimon Component: {playerDigimon}");
        Debug.Log($"[DIGIMON] EnemyDigimon Component: {enemyDigimon}");

        if (playerDigimon == null || enemyDigimon == null)
        {
            Debug.LogError("❌ Falha ao obter Digimons da cena");
            return;
        }

        // 🧠 CONTEXT
        battleContext = new BattleContext(playerDigimon, enemyDigimon);
        Debug.Log("🧠 BattleContext criado");

        // 🎮 PLAYER CONTROLLER
        var playerController =
            spawner.PlayerDigimonTransform.GetComponent<DigimonBattleController>();

        if (playerController != null)
        {
            playerController.SetContext(battleContext);
            Debug.Log("🎮 PlayerBattleController configurado");
        }
        else
        {
            Debug.LogError("❌ PlayerBattleController NÃO encontrado");
        }

        // 👾 ENEMY CONTROLLER
        var enemyController = spawner.EnemyTransform.GetComponent<EnemyCombatBehaviour>();

        if (enemyController != null)
        {
            enemyController.SetContext(battleContext);
            Debug.Log("👾 EnemyCombatBehaviour configurado");
        }
        else
        {
            Debug.LogWarning("⚠️ EnemyCombatBehaviour NÃO encontrado");
        }

        // 🎥 CAMERA (AGORA CORRETA)
        cameraController.Initialize(spawner.PlayerDigimonTransform, spawner.EnemyTransform);

        Debug.Log("🎥 Camera inicializada com BattleBounds");

        // 🧹 CLEAR CONTEXT
        CombatContextData.Clear();
        Debug.Log("🧹 CombatContextData limpo");

        Debug.Log("⚔️ BATALHA INICIALIZADA COM SUCESSO");
    }
}
