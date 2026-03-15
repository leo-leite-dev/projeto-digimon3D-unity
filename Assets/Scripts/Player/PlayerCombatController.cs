using UnityEngine;

public class PlayerCombatController : ValidatedMonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private TargetSystem targetSystem;

    private DigimonAttack attack;

    public DigimonAttack CurrentAttack => attack;

    public void UseSkill(DigimonSkill skill)
    {
        if (attack == null)
            return;

        if (skill == null)
            return;

        if (targetSystem == null)
            return;

        GameObject target = targetSystem.CurrentTarget;

        if (target == null)
            return;

        attack.TryUseSkill(skill, target);
    }

    public void SetDigimon(DigimonAttack newAttack)
    {
        attack = newAttack;

        if (attack == null)
            return;
    }
}
