using UnityEngine;

public class AttackCapability : MonoBehaviour, IAttackCapability
{
    [SerializeField]
    private DigimonAttack attack;

    public void UseSkill(DigimonSkill skill, GameObject target)
    {
        attack.TryUseSkill(skill, target);
    }

    public bool CanUseSkill(DigimonSkill skill, GameObject target)
    {
        return attack.EvaluateSkillUse(skill, target) == SkillUseCheckResult.Success;
    }
}
