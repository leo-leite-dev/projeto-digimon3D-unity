using UnityEngine;

public static class DigimonVisualComposer
{
    public static DigimonAnimator SetupVisual(
        DigimonCoreReferences core,
        DigimonBattleReferences battle,
        GameObject digimonGO
    )
    {
        if (core == null)
        {
            Debug.LogError("❌ CoreReferences null", digimonGO);
            return null;
        }

        if (core.ModelRoot == null)
        {
            Debug.LogError("❌ ModelRoot null", digimonGO);
            return null;
        }

        var digimon = core.Digimon;

        if (digimon == null)
        {
            Debug.LogError("❌ Digimon não setado", digimonGO);
            return null;
        }

        var modelPrefab = digimon.ModelPrefab;

        if (modelPrefab == null)
        {
            Debug.LogError("❌ ModelPrefab não definido no Digimon", digimonGO);
            return null;
        }

        for (int i = core.ModelRoot.childCount - 1; i >= 0; i--)
        {
            Object.Destroy(core.ModelRoot.GetChild(i).gameObject);
        }

        Debug.Log($"[VISUAL] Instanciando model: {modelPrefab.name}");

        var modelInstance = Object.Instantiate(modelPrefab, core.ModelRoot);

        modelInstance.transform.localPosition = Vector3.zero;
        modelInstance.transform.localRotation = Quaternion.identity;
        modelInstance.transform.localScale = Vector3.one;

        var animator = modelInstance.GetComponentInChildren<Animator>();

        if (animator == null)
        {
            Debug.LogError("❌ Animator não encontrado no model", digimonGO);
            return null;
        }

        var firePoint = FindDeepChild(modelInstance.transform, "FirePoint");

        if (firePoint == null)
            Debug.LogWarning("⚠️ FirePoint não encontrado no model", digimonGO);

        var digimonAnimator = core.ModelRoot.GetComponent<DigimonAnimator>();

        if (digimonAnimator == null)
            digimonAnimator = core.ModelRoot.gameObject.AddComponent<DigimonAnimator>();

        digimonAnimator.Inject(animator);

        var proxy =
            animator.GetComponent<DigimonAnimationEventProxy>()
            ?? animator.gameObject.AddComponent<DigimonAnimationEventProxy>();

        proxy.Initialize(digimonAnimator);

        if (battle != null)
            battle.InjectVisual(animator, firePoint, digimonAnimator);

        Debug.Log("[VISUAL] ✅ Setup completo");

        return digimonAnimator;
    }

    public static void BindMovementAnimator(DigimonCoreReferences core, GameObject go)
    {
        var movementAnimator = go.GetComponent<DigimonMovementAnimator>();

        if (movementAnimator == null)
            return;

        if (core.Movement == null)
        {
            Debug.LogWarning("⚠️ Movement faltando para binding", go);
            return;
        }

        var digimonAnimator = go.GetComponentInChildren<DigimonAnimator>();

        if (digimonAnimator == null)
        {
            Debug.LogWarning("⚠️ DigimonAnimator não encontrado", go);
            return;
        }

        movementAnimator.Inject(core.Movement, digimonAnimator);
    }

    private static Transform FindDeepChild(Transform parent, string name)
    {
        foreach (var child in parent.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == name)
                return child;
        }

        return null;
    }
}
