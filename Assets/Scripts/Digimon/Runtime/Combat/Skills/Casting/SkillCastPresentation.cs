using UnityEngine;

public class SkillCastPresentation
{
    private readonly DigimonMovement movement;
    private readonly DigimonAnimator animator;

    public SkillCastPresentation(DigimonMovement movement, DigimonAnimator animator)
    {
        this.movement = movement;
        this.animator = animator;
    }

    public void BeginCast(DigimonSkill skill, Transform target)
    {
        if (movement != null)
        {
            movement.AddMovementLock();

            if (target != null)
                movement.RotateToTarget(target.transform);
        }

        if (animator != null)
            animator.PlaySkill(skill);
    }

    public void EndCast()
    {
        if (movement != null)
            movement.RemoveMovementLock();
    }
}
