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
        Debug.Log(agent.destination);
        if (agent == null || wanderArea == null)
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
        {
            agent.SetDestination(hit.position);
        }

        waitTimer = waitTime;
    }
}
