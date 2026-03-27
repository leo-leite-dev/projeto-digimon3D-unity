using UnityEngine;

public static class DigimonEnemyOverWorldComposer
{
    public static void Compose(GameObject digimonGO)
    {
        if (digimonGO == null)
        {
            Debug.LogError("❌ DigimonGO null");
            return;
        }

        var core = digimonGO.GetComponent<DigimonCoreReferences>();

        if (core == null || !core.IsValid())
        {
            Debug.LogError("❌ CoreReferences inválido", digimonGO);
            return;
        }

        var digimon = core.Digimon;

        if (digimon == null)
        {
            Debug.LogError("❌ Digimon não encontrado", digimonGO);
            return;
        }

        if (!DigimonVisualComposer.SetupVisual(core, null, digimonGO))
            return;

        DigimonVisualComposer.BindMovementAnimator(core, digimonGO);

        Debug.Log($"👾 Enemy composto (overworld): {digimonGO.name}");
    }
}
