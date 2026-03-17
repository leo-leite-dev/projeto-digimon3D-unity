using UnityEngine;

public static class DigimonComposer
{
    public static void Compose(GameObject digimonGO)
    {
        if (digimonGO == null)
        {
            Debug.LogError("❌ Compose recebeu GameObject nulo");
            return;
        }

        var references = digimonGO.GetComponent<DigimonReferences>();
        var view = digimonGO.GetComponent<DigimonEnemyView>();

        if (references == null)
        {
            Debug.LogError("❌ Digimon sem References", digimonGO);
            return;
        }

        if (view == null)
        {
            Debug.LogError("❌ Digimon sem View", digimonGO);
            return;
        }

        references.RefreshCoreReferences();

        var animator = view.GetAnimator();

        if (animator == null)
        {
            Debug.LogError("❌ Animator não disponível no Compose. SpawnModel falhou.", digimonGO);
            return;
        }

        references.SetVisualInternal(references.ModelRoot, animator);

        if (!references.HasVisualReferences())
            Debug.LogError("❌ VisualReferences inválidas após Compose", digimonGO);
    }
}
