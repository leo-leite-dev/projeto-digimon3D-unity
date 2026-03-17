using System;
using UnityEngine;

public class DigimonAnimator : MonoBehaviour
{
    public event Action OnSpawnEffect;
    public event Action OnSwitchEffectToMoveToTarget;
    public event Action OnApplyHit;
    public event Action OnFinishSkill;

    [SerializeField]
    private DigimonReferences references;

    [SerializeField]
    private string damageTrigger = "damage";

    [SerializeField]
    private string deathTrigger = "death";

    [SerializeField]
    private string defaultSkillTrigger = "skill";

    private Animator Animator => references != null ? references.Animator : null;

    private void Reset()
    {
        AutoBind();
    }

    private void OnValidate()
    {
        AutoBind();
    }

    private void Awake()
    {
        AutoBind();
    }

    private void AutoBind()
    {
        if (references == null)
            references = GetComponent<DigimonReferences>();

        if (references == null)
            references = GetComponentInParent<DigimonReferences>();
    }

    public void PlaySkill(DigimonSkill skill)
    {
        if (Animator == null)
            return;

        string trigger = defaultSkillTrigger;

        if (skill != null && !string.IsNullOrWhiteSpace(skill.animationTrigger))
            trigger = skill.animationTrigger;

        if (string.IsNullOrWhiteSpace(trigger))
            return;

        Animator.SetTrigger(trigger);
    }

    public void PlayDamage()
    {
        if (Animator == null || string.IsNullOrWhiteSpace(damageTrigger))
            return;

        Animator.SetTrigger(damageTrigger);
    }

    public void PlayDeath()
    {
        if (Animator == null || string.IsNullOrWhiteSpace(deathTrigger))
            return;

        Animator.SetTrigger(deathTrigger);
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
