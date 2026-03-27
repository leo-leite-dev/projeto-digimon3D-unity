using UnityEngine;

public class SkillExecutionState
{
    public DigimonSkill CurrentSkill { get; private set; }
    public Transform CurrentTarget;
    public SkillEffect CurrentProjectile { get; private set; }

    public Digimon Caster { get; private set; }

    public bool IsCasting { get; private set; }
    public bool IsResolvingHit { get; private set; }

    public void Begin(Digimon caster, DigimonSkill skill, Transform target)
    {
        Caster = caster;
        CurrentSkill = skill;
        CurrentTarget = target;

        IsCasting = true;
        IsResolvingHit = false;
    }

    public void SetProjectile(SkillEffect projectile)
    {
        CurrentProjectile = projectile;
    }

    public void ClearProjectile()
    {
        CurrentProjectile = null;
    }

    public void BeginHitResolution()
    {
        IsResolvingHit = true;
    }

    public void EndHitResolution()
    {
        IsResolvingHit = false;
    }

    public void Finish()
    {
        CurrentSkill = null;
        CurrentTarget = null;
        Caster = null;

        IsCasting = false;
        IsResolvingHit = false;
    }
}
