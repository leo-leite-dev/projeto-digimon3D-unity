public class DigimonAttackDependencies
{
    public SkillExecutionState ExecutionState { get; }
    public SkillCastOrchestrator CastOrchestrator { get; }
    public SkillEffectSpawnCoordinator EffectSpawnCoordinator { get; }
    public SkillImpactCoordinator ImpactCoordinator { get; }
    public SkillFinishResolver FinishResolver { get; }

    public DigimonAttackDependencies(
        SkillExecutionState executionState,
        SkillCastOrchestrator castOrchestrator,
        SkillEffectSpawnCoordinator effectSpawnCoordinator,
        SkillImpactCoordinator impactCoordinator,
        SkillFinishResolver finishResolver
    )
    {
        ExecutionState = executionState;
        CastOrchestrator = castOrchestrator;
        EffectSpawnCoordinator = effectSpawnCoordinator;
        ImpactCoordinator = impactCoordinator;
        FinishResolver = finishResolver;
    }
}
