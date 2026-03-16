public class DigimonAttackCompositionDependencies
{
    public SkillExecutionState ExecutionState { get; }
    public SkillCastOrchestrator CastOrchestrator { get; }
    public SkillImpactCoordinator ImpactCoordinator { get; }
    public SkillFinishResolver FinishResolver { get; }

    public DigimonAttackCompositionDependencies(
        SkillExecutionState executionState,
        SkillCastOrchestrator castOrchestrator,
        SkillImpactCoordinator impactCoordinator,
        SkillFinishResolver finishResolver
    )
    {
        ExecutionState = executionState;
        CastOrchestrator = castOrchestrator;
        ImpactCoordinator = impactCoordinator;
        FinishResolver = finishResolver;
    }
}
