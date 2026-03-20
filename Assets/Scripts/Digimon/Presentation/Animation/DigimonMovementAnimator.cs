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

        float speed = movement.Velocity.magnitude;
        speed = Mathf.Clamp(speed, 0f, 1f);

        if (Mathf.Approximately(speed, lastSpeed))
            return;

        lastSpeed = speed;
        animator.SetSpeed(speed);
    }
}
