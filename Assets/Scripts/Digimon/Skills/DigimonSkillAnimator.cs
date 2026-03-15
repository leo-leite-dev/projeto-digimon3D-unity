using UnityEngine;

public class DigimonSkillAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private DigimonAttack digimonAttack;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (digimonAttack == null)
            digimonAttack = GetComponentInParent<DigimonAttack>();
    }

    private void Reset()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (digimonAttack == null)
            digimonAttack = GetComponentInParent<DigimonAttack>();
    }

    public void PlaySkill(DigimonSkill skill)
    {
        if (animator == null)
            return;

        if (skill == null)
            return;

        if (string.IsNullOrEmpty(skill.animationTrigger))
            return;

        animator.SetTrigger(skill.animationTrigger);
    }

    public void AnimationEvent_SpawnEffect()
    {
        if (digimonAttack == null)
            return;

        digimonAttack.TriggerEffectHitFlow();
    }

    public void AnimationEvent_SwitchEffectToMoveToTarget()
    {
        if (digimonAttack == null)
            return;

        digimonAttack.SwitchCurrentEffectToMoveToTarget();
    }

    public void AnimationEvent_ApplyHit()
    {
        if (digimonAttack == null)
            return;

        digimonAttack.TriggerDirectHitFlow();
    }

    public void AnimationEvent_FinishSkill()
    {
        if (digimonAttack == null)
            return;

        digimonAttack.FinishSkill();
    }
}
