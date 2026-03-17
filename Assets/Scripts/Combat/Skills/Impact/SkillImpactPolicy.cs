public class SkillImpactPolicy
{
    public bool CanSpawnSkillEffect(SkillExecutionState executionState)
    {
        if (!HasValidContext(executionState))
            return false;

        if (executionState.CurrentSkill.hitTriggerType != SkillHitTriggerType.Effect)
            return false;

        if (executionState.HasSpawnedEffectForCurrentSkill)
            return false;

        return true;
    }

    public bool CanTriggerEffectHit(SkillExecutionState executionState)
    {
        if (!HasValidContext(executionState))
            return false;

        if (executionState.CurrentSkill.hitTriggerType != SkillHitTriggerType.Effect)
            return false;

        if (executionState.HasSpawnedEffectForCurrentSkill)
            return false;

        return true;
    }

    public bool CanTriggerDirectHit(SkillExecutionState executionState)
    {
        if (!HasValidContext(executionState))
            return false;

        return executionState.CurrentSkill.hitTriggerType == SkillHitTriggerType.Direct;
    }

    public bool CanTriggerCurrentEffectImpact(SkillExecutionState executionState)
    {
        if (!HasValidContext(executionState))
            return false;

        return executionState.CurrentSkill.hitTriggerType == SkillHitTriggerType.Effect;
    }

    private bool HasValidContext(SkillExecutionState executionState)
    {
        if (executionState == null)
            return false;

        return executionState.CurrentSkill != null && executionState.CurrentTarget != null;
    }
}
