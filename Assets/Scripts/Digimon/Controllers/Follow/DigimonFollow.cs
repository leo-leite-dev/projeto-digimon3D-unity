using UnityEngine;

public class DigimonFollow : Digimon
{
    [Header("Follow Target")]
    private Transform player;
    private Transform followPoint;

    private DigimonMovement movement;
    private DigimonAnimator digimonAnimator;

    [Header("Follow Settings")]
    [SerializeField]
    private float repathDistance = 0.25f;

    private Vector3 lastFollowPosition;

    public bool IsInjected => movement != null;

    public override void Setup(DigimonData digimonData)
    {
        base.Setup(digimonData);
    }

    public void Inject(DigimonMovement movement, DigimonAnimator digimonAnimator)
    {
        if (movement == null)
        {
            Debug.LogError("❌ Movement não pode ser nulo", this);
            return;
        }

        this.movement = movement;
        this.digimonAnimator = digimonAnimator;
    }

    public void Initialize(Transform playerRef, Transform followPointRef)
    {
        if (playerRef == null || followPointRef == null)
        {
            Debug.LogError("❌ Initialize inválido no DigimonFollow", this);
            return;
        }

        player = playerRef;
        followPoint = followPointRef;

        lastFollowPosition = Vector3.positiveInfinity;
    }

    private void Update()
    {
        if (!IsInitialized || !EnsureInjected())
            return;

        if (player == null || followPoint == null)
            return;

        FollowPlayer();
        UpdateAnimation();
    }

    private bool EnsureInjected()
    {
        if (!IsInjected)
        {
            Debug.LogError("❌ DigimonFollow não foi injetado corretamente", this);
            return false;
        }

        return true;
    }

    private void FollowPlayer()
    {
        if (movement == null)
            return;

        Vector3 targetPosition = followPoint.position;

        float sqrDistance = (targetPosition - lastFollowPosition).sqrMagnitude;

        if (sqrDistance >= repathDistance * repathDistance)
        {
            movement.SetDestination(targetPosition);
            lastFollowPosition = targetPosition;
        }
    }

    private void UpdateAnimation()
    {
        if (digimonAnimator == null || movement == null)
            return;

        digimonAnimator.SetSpeed(movement.Velocity.magnitude);
    }
}
