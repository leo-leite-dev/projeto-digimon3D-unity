using UnityEngine;
using UnityEngine.AI;

public class EnemyWander : MonoBehaviour
{
    [HideInInspector]
    public WanderArea wanderArea;

    [HideInInspector]
    public EnemySpawner spawner;

    public Animator animator;

    public float waitTime = 2f;

    private NavMeshAgent agent;
    private float waitTimer;

    private bool isChasing;
    private Transform chaseTarget;
    private float chaseTimer;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        waitTimer = waitTime;
        ChooseNewTarget();
    }

    public void Initialize(WanderArea area, EnemySpawner enemySpawner)
    {
        wanderArea = area;
        spawner = enemySpawner;
    }

    void Update()
    {
        if (agent == null)
            return;

        if (isChasing)
        {
            UpdateChase();
            UpdateAnimatorSpeed();
            return;
        }

        if (wanderArea == null)
            return;

        if (agent.pathPending)
            return;

        if (agent.remainingDistance <= agent.stoppingDistance)
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

        if (!agent.pathPending)
            agent.SetDestination(chaseTarget.position);
    }

    void UpdateAnimatorSpeed()
    {
        if (animator != null)
        {
            float speed = agent.velocity.magnitude;
            animator.SetFloat("Speed", speed);
        }
    }

    void ChooseNewTarget()
    {
        if (wanderArea == null)
            return;

        Vector3 randomPos = wanderArea.GetRandomPosition();

        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPos, out hit, 5f, NavMesh.AllAreas))
            agent.SetDestination(hit.position);

        waitTimer = waitTime;
    }

    public void ChaseTarget(Transform target, float duration)
    {
        if (target == null || agent == null)
            return;

        isChasing = true;
        chaseTarget = target;
        chaseTimer = duration;
        waitTimer = waitTime;

        agent.isStopped = false;
        agent.SetDestination(target.position);
    }

    public void StopAndPause(float duration)
    {
        if (agent == null)
            return;

        isChasing = false;
        chaseTarget = null;
        chaseTimer = 0f;

        agent.ResetPath();
        agent.isStopped = true;

        Invoke(nameof(ResumeWander), duration);
    }

    void ResumeWander()
    {
        if (agent == null)
            return;

        agent.isStopped = false;
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
