using UnityEngine;

public struct CombatDecision
{
    public CombatAction Action;
    public Transform Target;
    public float Range;
    public DigimonSkill Skill;
    public GameObject SkillTarget;

    public static CombatDecision None => new CombatDecision { Action = CombatAction.None };

    public static CombatDecision Stop()
    {
        return new CombatDecision { Action = CombatAction.Stop };
    }

    public static CombatDecision MoveToTarget(Transform target, float range)
    {
        return new CombatDecision
        {
            Action = CombatAction.MoveToTarget,
            Target = target,
            Range = range,
        };
    }

    public static CombatDecision UseSkill(DigimonSkill skill, GameObject target)
    {
        return new CombatDecision
        {
            Action = CombatAction.UseSkill,
            Skill = skill,
            SkillTarget = target,
        };
    }
}
