using UnityEngine;

public static class EnemyInitializer
{
    public static void Initialize(GameObject enemyGO, WanderArea wanderArea, Transform player)
    {
        var references = enemyGO.GetComponent<DigimonReferences>();

        if (references == null)
        {
            Debug.LogError("❌ DigimonReferences não encontrado", enemyGO);
            return;
        }

        var enemy = enemyGO.GetComponent<DigimonEnemy>();

        if (enemy != null)
        {
            if (player != null)
                enemy.InjectTarget(player);
            else
                Debug.LogWarning("⚠️ Player null ao injetar target", enemyGO);
        }

        var view = enemyGO.GetComponent<DigimonEnemyView>();
        var controller = enemyGO.GetComponent<DigimonEnemyController>();

        InitializeWander(enemyGO, references, wanderArea, view, controller);
    }

    static void InitializeWander(
        GameObject enemyGO,
        DigimonReferences references,
        WanderArea wanderArea,
        DigimonEnemyView view,
        DigimonEnemyController controller
    )
    {
        var wander = enemyGO.GetComponent<EnemyWander>();

        if (wander == null)
        {
            Debug.LogWarning("⚠️ Enemy sem EnemyWander", enemyGO);
            return;
        }

        if (references.Movement == null)
        {
            Debug.LogError("❌ Movement não resolvido antes do Wander", enemyGO);
            return;
        }

        if (controller == null)
        {
            Debug.LogError("❌ DigimonEnemyController não encontrado", enemyGO);
            return;
        }

        var context = new WanderContext(
            wanderArea,
            references.Movement,
            references.DigimonAnimator,
            view,
            controller
        );

        wander.Initialize(context);
    }
}
