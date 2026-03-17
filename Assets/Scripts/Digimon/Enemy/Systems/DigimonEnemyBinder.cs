using UnityEngine;

public class DigimonEnemyBinder : MonoBehaviour, IDigimonComposable
{
    [SerializeField]
    private DigimonEnemy enemy;

    [SerializeField]
    private DigimonEnemyView view;

    [SerializeField]
    private DigimonReferences references;

    public void Compose()
    {
        if (enemy == null || view == null || references == null)
            return;

        if (enemy.Data == null)
            return;

        view.SpawnModel(enemy.Data);

        var animator = view.GetAnimator();

        if (animator == null)
            return;

        references.InitializeAfterModelSpawn(view.transform, animator);
    }
}
