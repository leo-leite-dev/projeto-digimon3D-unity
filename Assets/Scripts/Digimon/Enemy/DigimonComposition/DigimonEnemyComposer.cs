using UnityEngine;

public static class DigimonEnemyComposer
{
    public static void Compose(GameObject digimonGO)
    {
        var references = digimonGO.GetComponent<DigimonReferences>();

        if (references == null || !references.HasCoreReferences())
        {
            Debug.LogError("❌ References inválidas", digimonGO);
            return;
        }

        if (references.ModelRoot == null)
        {
            Debug.LogError("❌ ModelRoot não definido", digimonGO);
            return;
        }

        var digimon = references.Digimon;
        var attack = digimonGO.GetComponent<DigimonAttack>();

        if (digimon == null || attack == null)
        {
            Debug.LogError("❌ Dependências do Enemy incompletas", digimonGO);
            return;
        }

        if (!SetupVisual(references, digimonGO))
            return;

        DigimonAttackComposer.Compose(attack, references, references.ProjectilePool);

        var controller = digimonGO.GetComponent<DigimonEnemyController>();

        if (controller == null)
        {
            Debug.LogError("❌ DigimonEnemyController não encontrado", digimonGO);
            return;
        }

        controller.Inject(attack);

        var enemyView = digimonGO.GetComponent<DigimonEnemyView>();
        if (enemyView != null)
            enemyView.Inject(references.DigimonAnimator);

        var hitReceiver = digimonGO.GetComponentInChildren<DigimonHitReceiver>();

        if (hitReceiver == null)
        {
            Debug.LogError("❌ DigimonHitReceiver não encontrado", digimonGO);
        }
        else
        {
            hitReceiver.Inject(digimon, references.DigimonAnimator);

            hitReceiver.OnHit += controller.EnterCombat;
        }
    }

    private static bool SetupVisual(DigimonReferences references, GameObject digimonGO)
    {
        foreach (Transform child in references.ModelRoot)
            Object.Destroy(child.gameObject);

        if (references.Digimon.Data.modelPrefab == null)
        {
            Debug.LogError("❌ ModelPrefab não definido", digimonGO);
            return false;
        }

        var modelInstance = Object.Instantiate(
            references.Digimon.Data.modelPrefab,
            references.ModelRoot
        );

        modelInstance.transform.localPosition = Vector3.zero;
        modelInstance.transform.localRotation = Quaternion.identity;

        var animator = modelInstance.GetComponentInChildren<Animator>();

        if (animator == null)
        {
            Debug.LogError("❌ Animator não encontrado no model", digimonGO);
            return false;
        }

        var firePoint = FindDeepChild(modelInstance.transform, "FirePoint");

        if (firePoint == null)
            Debug.LogWarning("⚠️ FirePoint não encontrado", digimonGO);

        var digimonAnimator =
            references.ModelRoot.GetComponent<DigimonAnimator>()
            ?? references.ModelRoot.gameObject.AddComponent<DigimonAnimator>();

        digimonAnimator.Inject(animator);

        var proxy = animator.GetComponent<DigimonAnimationEventProxy>();

        if (proxy == null)
            proxy = animator.gameObject.AddComponent<DigimonAnimationEventProxy>();

        proxy.Initialize(digimonAnimator);

        references.SetVisualReferences(references.ModelRoot, animator, firePoint, digimonAnimator);

        return true;
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
