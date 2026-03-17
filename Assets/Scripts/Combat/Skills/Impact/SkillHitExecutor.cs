using System;
using UnityEngine;

public class SkillHitExecutor
{
    private readonly SkillDamageResolver damageResolver;
    private readonly Digimon attacker;

    public SkillHitExecutor(SkillDamageResolver damageResolver, Digimon attacker)
    {
        this.damageResolver =
            damageResolver ?? throw new ArgumentNullException(nameof(damageResolver));
        this.attacker = attacker ?? throw new ArgumentNullException(nameof(attacker));
    }

    public void ApplyHit(DigimonSkill currentSkill, GameObject currentTarget)
    {
        if (currentSkill == null || currentTarget == null)
            return;

        if (
            !damageResolver.TryBuildHitContext(
                currentSkill,
                currentTarget.transform,
                attacker,
                out HitContext context
            )
        )
            return;

        DigimonHitReceiver hitReceiver = currentTarget.GetComponentInParent<DigimonHitReceiver>();

        if (hitReceiver == null)
            return;

        hitReceiver.ReceiveHit(context);
    }
}
