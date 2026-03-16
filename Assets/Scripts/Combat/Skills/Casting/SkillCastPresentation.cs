using UnityEngine;

public class SkillCastPresentation
{
    private readonly DigimonMovement movement;
    private readonly DigimonSkillAnimator animator;

    public SkillCastPresentation(DigimonMovement movement, DigimonSkillAnimator animator)
    {
        this.movement = movement;
        this.animator = animator;
    }

    public void BeginCast(DigimonSkill skill, GameObject target)
    {
        if (movement != null)
        {
            movement.AddMovementLock();
            if (target != null)
                movement.RotateToTarget(target.transform);
        }
        if (animator != null)
            animator.PlaySkill(skill?.skillName);
    }

    public void EndCast()
    {
        if (movement != null)
            movement.RemoveMovementLock();
    }
}
