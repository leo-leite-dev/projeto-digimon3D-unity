using UnityEngine;

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

    public void OnAnimationFinished()
    {
        if (!executionState.IsCasting)
            return;

        var skill = executionState.CurrentSkill;

        if (skill == null || !skill.IsDirect)
            return;

        FinishInternal("AnimationFinished");
    }

    public void OnEffectImpact()
    {
        if (!executionState.IsCasting)
            return;

        var skill = executionState.CurrentSkill;

        if (skill == null || !skill.IsEffect)
            return;

        FinishInternal("EffectImpact");
    }

    public void ForceFinish(string reason)
    {
        if (!executionState.IsCasting)
            return;

        FinishInternal(reason);
    }

    private void FinishInternal(string reason)
    {
        Debug.Log($"🏁 Skill Finish → Reason: {reason}");

        castOrchestrator.Finish();
    }
}
