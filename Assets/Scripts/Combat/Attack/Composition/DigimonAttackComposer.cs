public class DigimonAttackComposer
{
    public void Compose(
        DigimonAttack attack,
        DigimonReferences references,
        SkillExecutionState executionState,
        SkillCastOrchestrator castOrchestrator,
        SkillImpactCoordinator impactCoordinator,
        SkillFinishResolver finishResolver
    )
    {
        attack.Configure(
            references,
            executionState,
            castOrchestrator,
            impactCoordinator,
            finishResolver
        );
    }
}
