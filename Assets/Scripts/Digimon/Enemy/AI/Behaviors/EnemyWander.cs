using UnityEngine;
using UnityEngine.AI;

public class EnemyWander : MonoBehaviour, IEnemyBehaviour, IContextInitializable<WanderContext>
{
    private WanderArea wanderArea;
    private DigimonMovement movement;
    private DigimonEnemyView view;
    private DigimonEnemyController controller;

    [Header("Wander")]
    [SerializeField]
    private float waitTime = 2f;

    [SerializeField]
    private float samplePositionRadius = 5f;

    private float waitTimer;
    private bool active;
    private bool isWalking;

    private bool wasBlockedLastFrame;

    public void Initialize(WanderContext context)
    {
        if (context.Movement == null || context.WanderArea == null)
        {
            Debug.LogError("❌ WanderContext inválido", this);
            return;
        }

        wanderArea = context.WanderArea;
        movement = context.Movement;
        view = context.View;
        controller = context.Controller;

        active = false;
        isWalking = false;
        wasBlockedLastFrame = false;
    }

    public void Activate()
    {
        if (movement == null || wanderArea == null)
        {
            Debug.LogWarning("⚠️ EnemyWander não inicializado", this);
            return;
        }

        active = true;
        waitTimer = waitTime;

        ChooseNewTarget();
    }

    public void Deactivate()
    {
        active = false;
    }

    void Update()
    {
        if (!active)
            return;

        if (IsWanderBlocked())
            return;

        if (movement.PathPending)
            return;

        bool isIdle = movement.HasReachedDestination() || movement.IsIdle();

        if (isIdle)
            HandleIdle();
        else
            HandleWalking();
    }

    private bool IsWanderBlocked()
    {
        if (controller == null)
            return false;

        bool blocked = controller.IsInCombat;

        if (blocked && !wasBlockedLastFrame)
        {
            Debug.Log("⛔ Wander pausado (InCombat)");

            movement?.StopMovement();
            view?.PlayIdle();
            isWalking = false;
        }

        wasBlockedLastFrame = blocked;

        return blocked;
    }

    private void HandleIdle()
    {
        if (isWalking)
        {
            view?.PlayIdle();
            isWalking = false;
        }

        waitTimer -= Time.deltaTime;

        if (waitTimer <= 0f)
            ChooseNewTarget();
    }

    private void HandleWalking()
    {
        if (!isWalking)
        {
            view?.PlayWalk();
            isWalking = true;
        }
    }

    private void ChooseNewTarget()
    {
        Vector3 randomPos = wanderArea.GetRandomPosition();

        if (
            NavMesh.SamplePosition(
                randomPos,
                out NavMeshHit hit,
                samplePositionRadius,
                NavMesh.AllAreas
            )
        )
        {
            movement.SetDestination(hit.position);
        }

        waitTimer = waitTime;
    }
}
