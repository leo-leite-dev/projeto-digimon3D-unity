using System;
using System.Collections;
using UnityEngine;

public class SkillCoordinator
{
    private readonly SkillHitExecutor hitExecutor;
    private readonly SkillExecutionState executionState;

    public SkillCoordinator(SkillHitExecutor hitExecutor, SkillExecutionState executionState)
    {
        this.hitExecutor = hitExecutor ?? throw new ArgumentNullException(nameof(hitExecutor));
        this.executionState =
            executionState ?? throw new ArgumentNullException(nameof(executionState));
    }

    public void TriggerHit(MonoBehaviour runner)
    {
        if (!executionState.IsCasting)
            return;

        var skill = executionState.CurrentSkill;
        var target = executionState.CurrentTarget;

        if (skill == null || target == null)
            return;

        executionState.BeginHitResolution();

        runner.StartCoroutine(ApplyHitWithDelay(skill, target));
    }

    public void TriggerHitFromProjectile(DigimonSkill skill, Transform target)
    {
        if (skill == null || target == null)
            return;

        hitExecutor.ApplyHit(skill, target);
    }

    private IEnumerator ApplyHitWithDelay(DigimonSkill skill, Transform target)
    {
        if (skill.damageDelay > 0f)
            yield return new WaitForSeconds(skill.damageDelay);

        if (!executionState.IsCasting)
            yield break;

        hitExecutor.ApplyHit(skill, target);

        executionState.EndHitResolution();
    }
}
