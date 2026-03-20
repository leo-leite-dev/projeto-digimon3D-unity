using UnityEngine;

public class SkillCastOrchestrator
{
    private readonly SkillUsageValidator usageValidator;
    private readonly SkillExecutionState executionState;
    private readonly SkillCastPipeline pipeline;

    public SkillCastOrchestrator(
        SkillUsageValidator usageValidator,
        SkillExecutionState executionState,
        SkillCastPipeline pipeline
    )
    {
        this.usageValidator = usageValidator;
        this.executionState = executionState;
        this.pipeline = pipeline;
    }

    public SkillUseCheckResult Evaluate(
        Digimon digimon,
        Transform caster,
        DigimonSkill skill,
        GameObject target
    )
    {
        return usageValidator.Evaluate(digimon, caster, skill, target);
    }

    public bool TryStart(Digimon digimon, Transform caster, DigimonSkill skill, GameObject target)
    {
        var result = Evaluate(digimon, caster, skill, target);
        if (result != SkillUseCheckResult.Success)
            return false;

        executionState.Begin(skill, target);
        pipeline.OnSkillStarted(skill, target);
        return true;
    }

    public void Finish()
    {
        pipeline.OnSkillFinished(executionState.CurrentSkill, executionState.CurrentTarget);
        executionState.ClearEffect();
        executionState.Finish();
    }
}
