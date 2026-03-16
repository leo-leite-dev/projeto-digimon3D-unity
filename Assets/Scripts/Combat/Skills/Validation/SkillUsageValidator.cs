using UnityEngine;

public class SkillUsageValidator
{
    private readonly SkillCooldownTracker cooldownTracker;
    private readonly SkillExecutionState executionState;

    public SkillUsageValidator(
        SkillCooldownTracker cooldownTracker,
        SkillExecutionState executionState
    )
    {
        this.cooldownTracker = cooldownTracker;
        this.executionState = executionState;
    }

    public SkillUseCheckResult Evaluate(
        Digimon digimon,
        Transform caster,
        DigimonSkill skill,
        GameObject target
    )
    {
        if (skill == null)
            return SkillUseCheckResult.InvalidSkill;

        if (target == null || !target.activeInHierarchy)
            return SkillUseCheckResult.InvalidTarget;

        if (executionState.IsCasting)
            return SkillUseCheckResult.AlreadyCasting;

        if (digimon == null || digimon.data == null)
            return SkillUseCheckResult.MissingDigimonData;

        if (cooldownTracker.IsOnCooldown(skill))
            return SkillUseCheckResult.OnCooldown;

        float allowedRange = skill.range + SkillGameplaySettings.CastTolerance;
        float sqrDistance = (target.transform.position - caster.position).sqrMagnitude;
        float rangeSqr = allowedRange * allowedRange;

        if (sqrDistance > rangeSqr)
            return SkillUseCheckResult.OutOfRange;

        return SkillUseCheckResult.Success;
    }
}
