public class DigimonAttackDependenciesFactory
{
    public DigimonAttackCompositionDependencies Create(
        DigimonReferences references,
        SkillHitExecutor hitExecutor
    )
    {
        var cooldownTracker = new SkillCooldownTracker();
        var executionState = new SkillExecutionState();

        var usageValidator = new SkillUsageValidator(cooldownTracker, executionState);

        var castPresentation = new SkillCastPresentation(
            references.Movement,
            references.SkillAnimator
        );

        var castPipeline = new SkillCastPipeline(castPresentation, cooldownTracker);

        var castOrchestrator = new SkillCastOrchestrator(
            usageValidator,
            executionState,
            castPipeline
        );

        var impactCoordinator = new SkillImpactCoordinator(hitExecutor, executionState);

        var finishResolver = new SkillFinishResolver(executionState, castOrchestrator);

        return new DigimonAttackCompositionDependencies(
            executionState,
            castOrchestrator,
            impactCoordinator,
            finishResolver
        );
    }
}
