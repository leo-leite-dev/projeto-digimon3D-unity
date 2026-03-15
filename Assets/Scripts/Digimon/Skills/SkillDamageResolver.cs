using UnityEngine;

public class SkillDamageResolver : MonoBehaviour
{
    public void Apply(DigimonSkill skill, Transform target, Digimon attacker)
    {
        if (skill == null)
            return;

        if (target == null)
            return;

        if (attacker == null)
            return;

        IDamageable damageable = target.GetComponentInParent<IDamageable>();
        Digimon defender = target.GetComponentInParent<Digimon>();

        if (damageable == null || defender == null)
            return;

        int damage = CombatCalculator.CalculateDamage(attacker, defender, skill);

        if (damage <= 0)
            return;

        damageable.TakeDamage(damage, attacker);
    }
}
