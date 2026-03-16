using UnityEngine;
using UnityEngine.AI;

public class EnemyWander : MonoBehaviour
{
    [HideInInspector]
    public WanderArea wanderArea;

    [HideInInspector]
    public EnemySpawner spawner;

    [Header("References")]
    [SerializeField]
    private DigimonMovement movement;

    [SerializeField]
    private Animator animator;
    public Animator Animator => animator;

    [Header("Wander")]
    [SerializeField]
    private float waitTime = 2f;

    [SerializeField]
    private float samplePositionRadius = 5f;

    private float waitTimer;

    private bool isChasing;
    private Transform chaseTarget;
    private float chaseTimer;

    void Awake()
    {
        if (movement == null)
            movement = GetComponent<DigimonMovement>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        waitTimer = waitTime;
        ChooseNewTarget();
    }

    public void AssignAnimator(Animator value)
    {
        animator = value;
    }

    public void Initialize(WanderArea area, EnemySpawner enemySpawner)
    {
        wanderArea = area;
        spawner = enemySpawner;
    }

    void Update()
    {
        if (movement == null)
            return;

        if (isChasing)
        {
            UpdateChase();
            UpdateAnimatorSpeed();
            return;
        }

        if (wanderArea == null)
            return;

        if (movement.PathPending)
        {
            UpdateAnimatorSpeed();
            return;
        }

        bool isWaitingAtDestination = movement.HasReachedDestination() || movement.IsIdle();

        if (isWaitingAtDestination)
        {
            waitTimer -= Time.deltaTime;

            if (animator != null)
                animator.SetFloat("Speed", 0f);

            if (waitTimer <= 0f)
                ChooseNewTarget();

            return;
        }

        UpdateAnimatorSpeed();
    }

    void UpdateChase()
    {
        if (chaseTarget == null)
        {
            StopChase();
            return;
        }

        chaseTimer -= Time.deltaTime;

        if (chaseTimer <= 0f)
        {
            StopChase();
            return;
        }

        movement.SetDestination(chaseTarget.position);
    }

    void UpdateAnimatorSpeed()
    {
        if (animator == null || movement == null)
            return;

        animator.SetFloat("Speed", movement.Velocity.magnitude);
    }

    void ChooseNewTarget()
    {
        if (wanderArea == null || movement == null)
            return;

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

    public void ChaseTarget(Transform target, float duration)
    {
        if (target == null || movement == null)
            return;

        CancelInvoke(nameof(ResumeWander));

        isChasing = true;
        chaseTarget = target;
        chaseTimer = duration;
        waitTimer = waitTime;

        movement.ResumeMovement();
        movement.SetDestination(target.position);
    }

    public void StopAndPause(float duration)
    {
        if (movement == null)
            return;

        CancelInvoke(nameof(ResumeWander));

        isChasing = false;
        chaseTarget = null;
        chaseTimer = 0f;

        movement.StopMovement();
        Invoke(nameof(ResumeWander), duration);
    }

    void ResumeWander()
    {
        if (movement == null)
            return;

        movement.ResumeMovement();
        waitTimer = waitTime;
        ChooseNewTarget();
    }

    void StopChase()
    {
        isChasing = false;
        chaseTarget = null;
        chaseTimer = 0f;
        waitTimer = waitTime;
        ChooseNewTarget();
    }
}
