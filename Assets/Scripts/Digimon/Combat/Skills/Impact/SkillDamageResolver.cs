using UnityEngine;

public class SkillDamageResolver
{
    public bool TryBuildHitContext(
        DigimonSkill skill,
        Transform target,
        Digimon attacker,
        out HitContext context
    )
    {
        context = default;

        if (skill == null || target == null || attacker == null)
            return false;

        var defender = target.GetComponentInParent<Digimon>();

        if (defender == null)
            return false;

        int damage = CombatCalculator.CalculateDamage(attacker, defender, skill);

        if (damage <= 0)
            return false;

        context = new HitContext
        {
            FinalDamage = damage,
            IsCritical = false,
            Attacker = attacker,
            Defender = defender,
        };

        return true;
    }
}
