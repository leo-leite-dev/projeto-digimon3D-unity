using System;
using UnityEngine;

public class DigimonAnimator : MonoBehaviour
{
    // 🎯 Eventos (bridge para gameplay)
    public event Action OnSpawnEffect;
    public event Action OnSwitchEffectToMoveToTarget;
    public event Action OnApplyHit;
    public event Action OnFinishSkill;
    public event Action OnActivateProjectile;

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
        if (!EnsureInitialized())
            return;

        animator.SetFloat(speedParam, speed);
    }

    public void PlaySkill(DigimonSkill skill)
    {
        if (!EnsureInitialized())
            return;

        string trigger = defaultSkillTrigger;

        if (skill != null && !string.IsNullOrWhiteSpace(skill.animationTrigger))
            trigger = skill.animationTrigger;

        animator.SetTrigger(trigger);
    }

    public void PlayDamage()
    {
        if (!EnsureInitialized())
            return;

        animator.SetTrigger(damageTrigger);
    }

    public void PlayDeath()
    {
        if (!EnsureInitialized())
            return;

        animator.SetTrigger(deathTrigger);
    }

    public void TriggerSpawnEffect() => OnSpawnEffect?.Invoke();

    public void TriggerSwitchEffectToMoveToTarget() => OnSwitchEffectToMoveToTarget?.Invoke();

    public void TriggerApplyHit() => OnApplyHit?.Invoke();

    public void TriggerFinishSkill() => OnFinishSkill?.Invoke();

    public void TriggerActivateProjectile() => OnActivateProjectile?.Invoke();

    private bool EnsureInitialized()
    {
        if (!isInitialized)
        {
            Debug.LogError("❌ DigimonAnimator usado antes do Inject!", this);
            return false;
        }

        return true;
    }
}
