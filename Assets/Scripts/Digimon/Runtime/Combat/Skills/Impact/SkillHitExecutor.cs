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

    public void ApplyHit(DigimonSkill skill, Transform target)
    {
        if (skill == null || target == null)
            return;

        if (!TryBuildContext(skill, target, out var context))
            return;

        if (!TryGetHitReceiver(target, out var receiver))
            return;

        receiver.ReceiveHit(context);
    }

    private bool TryBuildContext(DigimonSkill skill, Transform target, out HitContext context)
    {
        return damageResolver.TryBuildHitContext(skill, target, attacker, out context);
    }

    private bool TryGetHitReceiver(Transform target, out DigimonHitReceiver receiver)
    {
        receiver = target.GetComponentInParent<DigimonHitReceiver>();
        return receiver != null;
    }
}
