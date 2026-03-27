using UnityEngine;

public class SkillCastPipeline
{
    private readonly SkillCastPresentation presentation;
    private readonly SkillCooldownTracker cooldownTracker;

    public SkillCastPipeline(
        SkillCastPresentation presentation,
        SkillCooldownTracker cooldownTracker
    )
    {
        this.presentation = presentation;
        this.cooldownTracker = cooldownTracker;
    }

    public void OnSkillStarted(DigimonSkill skill, Transform target)
    {
        presentation.BeginCast(skill, target);
        cooldownTracker.RegisterUse(skill);
    }

    public void OnSkillFinished(DigimonSkill skill)
    {
        if (skill == null)
            return;

        presentation.EndCast();
    }
}
