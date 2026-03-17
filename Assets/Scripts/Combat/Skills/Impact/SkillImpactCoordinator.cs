using System;
using System.Collections;
using UnityEngine;

public class SkillImpactCoordinator
{
    private readonly SkillHitExecutor hitExecutor;
    private readonly SkillExecutionState executionState;
    private readonly SkillImpactPolicy impactPolicy;

    public SkillImpactCoordinator(
        SkillHitExecutor hitExecutor,
        SkillExecutionState executionState,
        SkillImpactPolicy impactPolicy
    )
    {
        this.hitExecutor = hitExecutor ?? throw new ArgumentNullException(nameof(hitExecutor));
        this.executionState =
            executionState ?? throw new ArgumentNullException(nameof(executionState));
        this.impactPolicy = impactPolicy ?? throw new ArgumentNullException(nameof(impactPolicy));
    }

    public void TriggerDirectHitFlow(MonoBehaviour runner)
    {
        if (!impactPolicy.CanTriggerDirectHit(executionState))
            return;

        var skill = executionState.CurrentSkill;
        var target = executionState.CurrentTarget;

        if (skill == null || target == null)
            return;

        runner.StartCoroutine(ApplyHitWithDelay(skill, target));
    }

    public void TriggerCurrentEffectImpactFlow(MonoBehaviour runner)
    {
        if (!impactPolicy.CanTriggerCurrentEffectImpact(executionState))
            return;

        var skill = executionState.CurrentSkill;
        var target = executionState.CurrentTarget;

        if (skill == null || target == null)
            return;

        runner.StartCoroutine(ApplyHitWithDelay(skill, target));
    }

    private IEnumerator ApplyHitWithDelay(DigimonSkill skill, GameObject target)
    {
        if (skill.damageDelay > 0f)
        {
            Debug.Log($"⏳ Aguardando delay: {skill.damageDelay}s");
            yield return new WaitForSeconds(skill.damageDelay);
        }

        if (!impactPolicy.CanTriggerCurrentEffectImpact(executionState))
        {
            Debug.Log("❌ Impacto cancelado após delay");
            yield break;
        }

        if (skill == null || target == null)
        {
            Debug.Log("❌ Skill ou Target inválido após delay");
            yield break;
        }

        Debug.Log("💥 Aplicando dano");

        hitExecutor.ApplyHit(skill, target);
    }
}
