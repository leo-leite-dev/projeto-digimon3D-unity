public class SkillFinishResolver
{
    private readonly SkillExecutionState executionState;
    private readonly SkillCastOrchestrator castOrchestrator;

    public SkillFinishResolver(
        SkillExecutionState executionState,
        SkillCastOrchestrator castOrchestrator
    )
    {
        this.executionState = executionState;
        this.castOrchestrator = castOrchestrator;
    }

    public void OnEffectReachedTarget()
    {
        if (!executionState.IsCasting)
            return;

        if (executionState.CurrentSkill == null)
            return;

        if (executionState.CurrentSkill.finishMode != SkillFinishMode.EffectImpact)
            return;

        castOrchestrator.Finish();
    }
}
