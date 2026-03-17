using UnityEngine;

public class DigimonAnimationEventProxy : MonoBehaviour
{
    private DigimonAnimator target;

    public void Initialize(DigimonAnimator target)
    {
        this.target = target;
    }

    public void AnimationEvent_SpawnEffect()
    {
        target?.AnimationEvent_SpawnEffect();
    }

    public void AnimationEvent_SwitchEffectToMoveToTarget()
    {
        target?.AnimationEvent_SwitchEffectToMoveToTarget();
    }

    public void AnimationEvent_ApplyHit()
    {
        target?.AnimationEvent_ApplyHit();
    }

    public void AnimationEvent_FinishSkill()
    {
        target?.AnimationEvent_FinishSkill();
    }
}
