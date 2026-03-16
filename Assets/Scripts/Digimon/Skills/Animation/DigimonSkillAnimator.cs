using System;
using UnityEngine;

public class DigimonSkillAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Animator animator;

    public event Action OnSpawnEffect;
    public event Action OnSwitchEffectToMoveToTarget;
    public event Action OnApplyHit;
    public event Action OnFinishSkill;

    public void PlaySkill(string triggerName)
    {
        if (animator == null)
        {
            Debug.LogWarning("[DigimonSkillAnimator] animator == null", this);
            return;
        }

        if (string.IsNullOrWhiteSpace(triggerName))
        {
            Debug.LogWarning("[DigimonSkillAnimator] triggerName inválido", this);
            return;
        }

        animator.SetTrigger(triggerName);
    }

    public void AnimationEvent_SpawnEffect()
    {
        OnSpawnEffect?.Invoke();
    }

    public void AnimationEvent_SwitchEffectToMoveToTarget()
    {
        OnSwitchEffectToMoveToTarget?.Invoke();
    }

    public void AnimationEvent_ApplyHit()
    {
        OnApplyHit?.Invoke();
    }

    public void AnimationEvent_FinishSkill()
    {
        OnFinishSkill?.Invoke();
    }
}
