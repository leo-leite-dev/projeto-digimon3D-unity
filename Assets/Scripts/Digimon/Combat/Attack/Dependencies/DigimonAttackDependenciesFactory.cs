using UnityEngine;

public class DigimonAttackDependenciesFactory
{
    public DigimonAttackDependencies Create(
        DigimonCoreReferences core,
        DigimonBattleReferences battle,
        SkillHitExecutor hitExecutor
    )
    {
        if (core == null || battle == null)
        {
            Debug.LogError("❌ Core ou Battle references null");
            return null;
        }

        var cooldownTracker = new SkillCooldownTracker();
        var executionState = new SkillExecutionState();

        var usageValidator = new SkillUsageValidator(cooldownTracker, executionState);

        var castPresentation = new SkillCastPresentation(core.Movement, battle.DigimonAnimator);

        var castPipeline = new SkillCastPipeline(castPresentation, cooldownTracker);

        var castOrchestrator = new SkillCastOrchestrator(
            usageValidator,
            executionState,
            castPipeline
        );

        var finishResolver = new SkillFinishResolver(executionState, castOrchestrator);

        var spawner = new SkillEffectSpawner(battle.ProjectilePool);

        var effectSpawnCoordinator = new SkillEffectSpawnCoordinator(
            spawner,
            finishResolver,
            executionState
        );

        var impactCoordinator = new SkillCoordinator(hitExecutor, executionState);

        return new DigimonAttackDependencies(
            executionState,
            castOrchestrator,
            effectSpawnCoordinator,
            impactCoordinator,
            finishResolver
        );
    }
}
