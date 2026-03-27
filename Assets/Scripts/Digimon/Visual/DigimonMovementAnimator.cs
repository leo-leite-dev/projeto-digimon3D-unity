using UnityEngine;

public class DigimonMovementAnimator : MonoBehaviour
{
    private DigimonMovement movement;
    private DigimonAnimator animator;

    private bool isInitialized;
    private float lastSpeed;

    public void Inject(DigimonMovement movement, DigimonAnimator animator)
    {
        this.movement = movement;
        this.animator = animator;

        if (this.movement == null)
        {
            Debug.LogError("❌ DigimonMovementAnimator → Movement null", this);
            return;
        }

        if (this.animator == null)
        {
            Debug.LogError("❌ DigimonMovementAnimator → Animator null", this);
            return;
        }

        isInitialized = true;
    }

    void Update()
    {
        if (!isInitialized)
            return;

        float speed = CalculateMovementSpeed();

        lastSpeed = speed;
        animator.SetSpeed(speed);
    }

    private float CalculateMovementSpeed()
    {
        if (movement == null)
            return 0f;

        float velocity = movement.Velocity.magnitude;

        if (velocity < 0.05f)
            return 0f;

        return 1f;
    }
}
