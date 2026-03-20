using UnityEngine;

public class DigimonEnemyView : MonoBehaviour
{
    private DigimonAnimator animator;

    public void Inject(DigimonAnimator animator)
    {
        this.animator = animator;

        if (this.animator == null)
            Debug.LogError("❌ DigimonEnemyView → Animator não injetado", this);
    }

    public void PlayWalk()
    {
        animator?.SetSpeed(1f);
    }

    public void PlayIdle()
    {
        animator?.SetSpeed(0f);
    }

    public void PlayAttack()
    {
        animator?.PlaySkill(null);
    }

    public void PlayHit()
    {
        animator?.PlayDamage();
    }
}
