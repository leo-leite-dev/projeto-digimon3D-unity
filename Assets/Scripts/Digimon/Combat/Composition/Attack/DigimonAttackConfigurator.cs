public class DigimonAttackConfigurator
{
    private readonly SkillTargetResolver targetResolver;

    public DigimonAttackConfigurator(SkillTargetResolver targetResolver)
    {
        this.targetResolver = targetResolver;
    }

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
        attack.Configure(
            references,
            executionState,
            castOrchestrator,
            effectSpawnCoordinator,
            impactCoordinator,
            finishResolver,
            targetResolver
        );
    }
}
