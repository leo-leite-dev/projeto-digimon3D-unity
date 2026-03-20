using UnityEngine;

public class DigimonFollowController : MonoBehaviour
{
    private DigimonAttack attack;

    private DigimonSkill pendingSkill;
    private GameObject target;
    private bool isTryingToUseSkill;

    public void Inject(DigimonAttack atk)
    {
        attack = atk;
    }

    public void RequestSkill(DigimonSkill skill, GameObject targetGO)
    {
        if (skill == null || targetGO == null)
            return;

        pendingSkill = skill;
        target = targetGO;
        isTryingToUseSkill = true;
    }

    public CombatDecision Tick()
    {
        if (attack == null)
            return CombatDecision.None;

        if (!isTryingToUseSkill)
            return CombatDecision.None;

        if (pendingSkill == null || target == null)
        {
            Clear();
            return CombatDecision.None;
        }

        var result = attack.EvaluateSkillUse(pendingSkill, target);

        switch (result)
        {
            case SkillUseCheckResult.Success:
                var decision = CombatDecision.UseSkill(pendingSkill, target);
                Clear();
                return decision;

            case SkillUseCheckResult.OutOfRange:
                return CombatDecision.MoveToTarget(target.transform, pendingSkill.range);

            case SkillUseCheckResult.OnCooldown:
            case SkillUseCheckResult.AlreadyCasting:
                return CombatDecision.Stop();

            default:
                Clear();
                return CombatDecision.None;
        }
    }

    private void Clear()
    {
        pendingSkill = null;
        target = null;
        isTryingToUseSkill = false;
    }
}
