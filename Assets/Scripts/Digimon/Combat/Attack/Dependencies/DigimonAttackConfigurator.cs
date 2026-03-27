using UnityEngine;

public class DigimonAttackConfigurator
{
    private readonly SkillTargetResolver targetResolver;

    public DigimonAttackConfigurator(SkillTargetResolver targetResolver)
    {
        this.targetResolver = targetResolver;
    }

    public void ConfigureAttack(
        DigimonAttack attack,
        DigimonCoreReferences core,
        DigimonBattleReferences battle,
        SkillExecutionState executionState,
        SkillCastOrchestrator castOrchestrator,
        SkillEffectSpawnCoordinator effectSpawnCoordinator,
        SkillCoordinator skillCoordinator,
        SkillFinishResolver finishResolver
    )
    {
        if (attack == null)
        {
            Debug.LogError("❌ DigimonAttack null");
            return;
        }

        if (core == null || battle == null)
        {
            Debug.LogError("❌ Core ou Battle references null");
            return;
        }

        attack.Configure(
            core,
            battle,
            executionState,
            castOrchestrator,
            effectSpawnCoordinator,
            skillCoordinator,
            finishResolver,
            targetResolver
        );
    }
}
