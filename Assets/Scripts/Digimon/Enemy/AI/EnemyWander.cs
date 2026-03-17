using UnityEngine;
using UnityEngine.AI;

public class EnemyWander : MonoBehaviour, IContextInitializable<WanderContext>
{
    private WanderArea wanderArea;
    private EnemySpawner spawner;

    private DigimonMovement movement;
    private Animator animator;

    [SerializeField]
    private DigimonReferences references;

    [Header("Wander")]
    [SerializeField]
    private float waitTime = 2f;

    [SerializeField]
    private float samplePositionRadius = 5f;

    private float waitTimer;
    private bool initialized;

    private bool isChasing;
    private Transform chaseTarget;
    private float chaseTimer;

    public void Initialize(WanderContext context)
    {
        wanderArea = context.WanderArea;
        spawner = context.Spawner;

        if (references != null)
        {
            movement = references.Movement;
            animator = references.Animator;

            if (movement == null)
            {
                references.RefreshCoreReferences();
                movement = references.Movement;
            }
        }

        if (movement == null)
        {
            Debug.LogError("❌ EnemyWander sem Movement (mesmo após refresh)", this);
            return;
        }

        initialized = true;

        waitTimer = waitTime;
        ChooseNewTarget();
    }

    void Update()
    {
        if (!initialized)
            return;

        if (movement == null || wanderArea == null)
            return;

        if (isChasing)
        {
            UpdateChase();
            UpdateAnimator();
            return;
        }

        if (movement.PathPending)
        {
            UpdateAnimator();
            return;
        }

        if (movement.HasReachedDestination() || movement.IsIdle())
        {
            waitTimer -= Time.deltaTime;

            if (animator != null)
                animator.SetFloat("Speed", 0f);

            if (waitTimer <= 0f)
                ChooseNewTarget();

            return;
        }

        UpdateAnimator();
    }

    void ChooseNewTarget()
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
            movement.SetDestination(hit.position);

        waitTimer = waitTime;
    }

    void UpdateAnimator()
    {
        if (animator != null)
            animator.SetFloat("Speed", movement.Velocity.magnitude);
    }

    void UpdateChase()
    {
        if (chaseTarget == null || chaseTimer <= 0f)
        {
            StopChase();
            return;
        }

        chaseTimer -= Time.deltaTime;
        movement.SetDestination(chaseTarget.position);
    }

    void StopChase()
    {
        isChasing = false;
        chaseTarget = null;
        waitTimer = waitTime;
        ChooseNewTarget();
    }
}
