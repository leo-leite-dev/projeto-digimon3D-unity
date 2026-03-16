using UnityEngine;

public class SkillImpactCoordinator
{
    private readonly SkillHitExecutor hitExecutor;
    private readonly SkillExecutionState executionState;

    public SkillImpactCoordinator(SkillHitExecutor hitExecutor, SkillExecutionState executionState)
    {
        this.hitExecutor = hitExecutor;
        this.executionState = executionState;
    }

    public void TriggerEffectHitFlow(DigimonAttack owner)
    {
        if (hitExecutor == null)
            return;

        SkillEffect effect = hitExecutor.TriggerEffectHitFlow(
            executionState.CurrentSkill,
            executionState.CurrentTarget,
            executionState.HasSpawnedEffectForCurrentSkill,
            owner
        );

        if (effect != null)
            executionState.SetEffect(effect);
    }

    public void TriggerDirectHitFlow()
    {
        if (hitExecutor == null)
            return;

        hitExecutor.TriggerDirectHitFlow(executionState.CurrentSkill, executionState.CurrentTarget);
    }

    public void SwitchCurrentEffectToMoveToTarget()
    {
        if (hitExecutor == null)
            return;

        hitExecutor.SwitchCurrentEffectToMoveToTarget(executionState.CurrentEffect);
    }
}
