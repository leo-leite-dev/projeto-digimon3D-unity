using UnityEngine;

public class SkillExecutionState
{
    public DigimonSkill CurrentSkill { get; private set; }
    public GameObject CurrentTarget { get; private set; }
    public SkillEffect CurrentEffect { get; private set; }
    public bool IsCasting { get; private set; }
    public bool HasSpawnedEffectForCurrentSkill { get; private set; }

    public void Begin(DigimonSkill skill, GameObject target)
    {
        CurrentSkill = skill;
        CurrentTarget = target;
        CurrentEffect = null;
        HasSpawnedEffectForCurrentSkill = false;
        IsCasting = true;
    }

    public void SetEffect(SkillEffect effect)
    {
        CurrentEffect = effect;
        if (effect != null)
            HasSpawnedEffectForCurrentSkill = true;
    }

    public void ClearEffect()
    {
        CurrentEffect = null;
    }

    public void Finish()
    {
        CurrentSkill = null;
        CurrentTarget = null;
        CurrentEffect = null;
        HasSpawnedEffectForCurrentSkill = false;
        IsCasting = false;
    }
}
