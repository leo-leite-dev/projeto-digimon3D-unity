using UnityEngine;

public static class DigimonFollowComposer
{
    public static void Compose(GameObject digimonGO)
    {
        var references = digimonGO.GetComponent<DigimonReferences>();

        if (references == null || !references.HasCoreReferences())
        {
            Debug.LogError("❌ References inválidas", digimonGO);
            return;
        }

        var digimon = references.Digimon;
        var attack = digimonGO.GetComponent<DigimonAttack>();
        var follow = references.Follow;

        if (digimon == null || attack == null || follow == null)
        {
            Debug.LogError("❌ Dependências do Follow incompletas", digimonGO);
            return;
        }

        if (!SetupVisual(references, digimonGO))
            return;

        DigimonAttackComposer.Compose(attack, references, references.ProjectilePool);

        var combat = digimonGO.GetComponent<DigimonFollowController>();
        if (combat == null)
            combat = digimonGO.AddComponent<DigimonFollowController>();

        combat.Inject(attack);

        follow.Inject(references, combat);

        BindAnimationEvents(references, attack);
        BindMovementAnimator(references, digimonGO);
    }

    private static bool SetupVisual(DigimonReferences references, GameObject digimonGO)
    {
        foreach (Transform child in references.ModelRoot)
            Object.Destroy(child.gameObject);

        var modelInstance = Object.Instantiate(
            references.Digimon.Data.modelPrefab,
            references.ModelRoot
        );

        modelInstance.transform.localPosition = Vector3.zero;
        modelInstance.transform.localRotation = Quaternion.identity;
        modelInstance.transform.localScale = Vector3.one;

        var animator = modelInstance.GetComponentInChildren<Animator>();

        if (animator == null)
        {
            Debug.LogError("❌ Animator não encontrado", digimonGO);
            return false;
        }

        var firePoint = FindDeepChild(modelInstance.transform, "FirePoint");

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

    private static void BindAnimationEvents(DigimonReferences references, DigimonAttack attack)
    {
        var animator = references.DigimonAnimator;

        if (animator == null)
        {
            Debug.LogError("❌ DigimonAnimator não encontrado para binding");
            return;
        }

        animator.OnSpawnEffect += attack.OnSpawnEffect;
        animator.OnSwitchEffectToMoveToTarget += attack.OnSwitchEffectToMoveToTarget;
        animator.OnApplyHit += attack.OnApplyHit;
        animator.OnFinishSkill += attack.OnFinishSkill;
    }

    private static void BindMovementAnimator(DigimonReferences references, GameObject go)
    {
        var movementAnimator = go.GetComponent<DigimonMovementAnimator>();

        if (movementAnimator != null && references.Movement != null)
            movementAnimator.Inject(references.Movement, references.DigimonAnimator);
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
