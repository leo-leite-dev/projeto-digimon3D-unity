using UnityEngine;

public class DigimonAttackConfigurator
{
    public void ConfigureAttack(
        DigimonAttack attack,
        DigimonReferences references,
        SkillExecutionState executionState,
        SkillCastOrchestrator castOrchestrator,
        SkillEffectSpawnCoordinator effectSpawnCoordinator,
        SkillImpactCoordinator impactCoordinator,
        SkillFinishResolver finishResolver
    )
    {
        // 🔹 Configura o core do sistema de ataque
        attack.Configure(
            references,
            executionState,
            castOrchestrator,
            effectSpawnCoordinator,
            impactCoordinator,
            finishResolver
        );

        // 🔹 Conecta Animation Event → Proxy → DigimonAnimator
        BindAnimationProxy(references);
    }

    private void BindAnimationProxy(DigimonReferences references)
    {
        if (references == null || references.Animator == null || references.DigimonAnimator == null)
        {
            Debug.LogWarning(
                "DigimonAttackConfigurator: Missing references for animation binding."
            );
            return;
        }

        var proxy = references.Animator.GetComponent<DigimonAnimationEventProxy>();

        if (proxy == null)
        {
            Debug.LogWarning("DigimonAnimationEventProxy not found on Animator GameObject.");
            return;
        }

        proxy.Initialize(references.DigimonAnimator);
    }
}
