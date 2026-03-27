using UnityEngine;

public class DigimonAnimationEventProxy : MonoBehaviour
{
    private DigimonAnimator digimonAnimator;

    public void Initialize(DigimonAnimator digimonAnimator)
    {
        this.digimonAnimator = digimonAnimator;
    }

    public void AnimationEvent_SpawnEffect()
    {
        digimonAnimator?.TriggerSpawnEffect();
    }

    public void AnimationEvent_ApplyHit()
    {
        digimonAnimator?.TriggerApplyHit();
    }

    public void AnimationEvent_FinishSkill()
    {
        digimonAnimator?.TriggerFinishSkill();
    }

    public void AnimationEvent_OnActivateProjectile()
    {
        digimonAnimator?.TriggerActivateProjectile();
    }

    public void AnimationEvent_SwitchEffectToMoveToTarget()
    {
        digimonAnimator?.TriggerSwitchEffectToMoveToTarget();
    }
}
