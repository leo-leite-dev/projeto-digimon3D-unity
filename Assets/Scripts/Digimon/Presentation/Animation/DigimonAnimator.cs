using System;
using UnityEngine;

public class DigimonAnimator : MonoBehaviour
{
    public event Action OnSpawnEffect;
    public event Action OnSwitchEffectToMoveToTarget;
    public event Action OnApplyHit;
    public event Action OnFinishSkill;

    private Animator animator;
    private bool isInitialized = false;

    [Header("Triggers")]
    [SerializeField]
    private string damageTrigger = "damage";

    [SerializeField]
    private string deathTrigger = "death";

    [SerializeField]
    private string defaultSkillTrigger = "skill";

    [Header("Parameters")]
    [SerializeField]
    private string speedParam = "Speed";

    public void Inject(Animator animator)
    {
        this.animator = animator;

        if (this.animator == null)
        {
            Debug.LogError("❌ DigimonAnimator → Animator não injetado", this);
            return;
        }

        isInitialized = true;
    }

    public void SetSpeed(float speed)
    {
        if (!isInitialized)
            return;
        animator.SetFloat(speedParam, speed);
    }

    public void PlaySkill(DigimonSkill skill)
    {
        if (!isInitialized)
            return;

        string trigger = defaultSkillTrigger;

        if (skill != null && !string.IsNullOrWhiteSpace(skill.animationTrigger))
            trigger = skill.animationTrigger;

        animator.SetTrigger(trigger);
    }

    public void PlayDamage()
    {
        if (!isInitialized)
            return;
        animator.SetTrigger(damageTrigger);
    }

    public void PlayDeath()
    {
        if (!isInitialized)
            return;
        animator.SetTrigger(deathTrigger);
    }

    public void AnimationEvent_SpawnEffect() => OnSpawnEffect?.Invoke();

    public void AnimationEvent_SwitchEffectToMoveToTarget() =>
        OnSwitchEffectToMoveToTarget?.Invoke();

    public void AnimationEvent_ApplyHit() => OnApplyHit?.Invoke();

    public void AnimationEvent_FinishSkill() => OnFinishSkill?.Invoke();
}
