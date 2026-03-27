using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionTrigger : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField]
    private string sceneToLoad;

    [Header("Combat Source")]
    [SerializeField]
    private EnemySpawner enemySpawner;

    [SerializeField]
    private PlayerStatsController playerStats;

    [SerializeField]
    private PlayerDigidex playerDigidex;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PrepareCombat();
        LoadScene();
    }

    private void PrepareCombat()
    {
        if (enemySpawner == null)
        {
            Debug.LogError("❌ EnemySpawner não definido", this);
            return;
        }

        var enemy = PickRandomEnemy();

        if (enemy == null)
        {
            Debug.LogError("❌ Nenhum inimigo disponível", this);
            return;
        }

        CombatContextData.SelectedEnemy = enemy;

        CombatContextData.TamerStats = playerStats != null ? playerStats.GetConfig() : null;
        CombatContextData.PlayerDigimon =
            playerDigidex != null ? playerDigidex.GetCurrentDigimon()?.Data : null;

        Debug.Log($"🎲 Enemy: {enemy.digimonName}");
    }

    private DigimonData PickRandomEnemy()
    {
        var list = enemySpawner.EnemyTypes;

        if (list == null || list.Count == 0)
            return null;

        int index = Random.Range(0, list.Count);
        return list[index];
    }

    private void LoadScene()
    {
        Debug.Log($"🚪 Loading scene: {sceneToLoad}");
        SceneManager.LoadScene(sceneToLoad);
    }
}
