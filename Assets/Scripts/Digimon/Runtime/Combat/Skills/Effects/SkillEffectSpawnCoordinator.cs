using UnityEngine;

public class SkillEffectSpawnCoordinator
{
    private readonly SkillEffectSpawner spawner;
    private readonly SkillFinishResolver finishResolver;
    private readonly SkillExecutionState executionState;

    public SkillEffectSpawnCoordinator(
        SkillEffectSpawner spawner,
        SkillFinishResolver finishResolver,
        SkillExecutionState executionState
    )
    {
        this.spawner = spawner;
        this.finishResolver = finishResolver;
        this.executionState = executionState;
    }

    public SkillEffect Spawn(
        DigimonSkill skill,
        Transform target,
        Vector3 position,
        Quaternion rotation
    )
    {
        if (skill == null || skill.effectPrefab == null || target == null)
            return null;

        var go = spawner.Spawn(skill.effectPrefab, position, rotation);

        var effect = go.GetComponent<SkillEffect>();

        if (effect == null)
            return null;

        effect.Setup(target, skill);

        BindEffectLifecycle(effect);

        return effect;
    }

    private void BindEffectLifecycle(SkillEffect effect)
    {
        bool finishedByImpact = false;
        var boundSkill = executionState.CurrentSkill;

        effect.OnImpact += _ =>
        {
            if (!executionState.IsCasting || executionState.CurrentSkill != boundSkill)
                return;

            if (finishedByImpact)
                return;

            finishedByImpact = true;

            finishResolver.OnEffectImpact();
        };

        effect.OnEffectEnded += () =>
        {
            if (finishedByImpact)
                return;

            if (!executionState.IsCasting || executionState.CurrentSkill != boundSkill)
                return;

            finishResolver.ForceFinish("Effect terminou sem impacto");
        };
    }
}
