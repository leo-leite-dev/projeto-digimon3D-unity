public class DigimonAttackDependenciesFactory
{
    public DigimonAttackDependencies Create(
        DigimonReferences references,
        SkillHitExecutor hitExecutor,
        ProjectilePool projectilePool
    )
    {
        var cooldownTracker = new SkillCooldownTracker();
        var executionState = new SkillExecutionState();
        var impactPolicy = new SkillImpactPolicy();

        var usageValidator = new SkillUsageValidator(cooldownTracker, executionState);

        var castPresentation = new SkillCastPresentation(
            references.Movement,
            references.DigimonAnimator
        );

        var castPipeline = new SkillCastPipeline(castPresentation, cooldownTracker);

        var castOrchestrator = new SkillCastOrchestrator(
            usageValidator,
            executionState,
            castPipeline
        );

        var spawner = new SkillEffectSpawner(projectilePool);

        var effectSpawnCoordinator = new SkillEffectSpawnCoordinator(spawner);

        var impactCoordinator = new SkillImpactCoordinator(
            hitExecutor,
            executionState,
            impactPolicy
        );

        var finishResolver = new SkillFinishResolver(executionState, castOrchestrator);

        return new DigimonAttackDependencies(
            executionState,
            castOrchestrator,
            effectSpawnCoordinator,
            impactCoordinator,
            finishResolver
        );
    }
}
