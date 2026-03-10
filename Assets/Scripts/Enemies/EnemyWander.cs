using UnityEngine;

public class EnemyWander : MonoBehaviour
{
    [HideInInspector]
    public WanderArea wanderArea;

    [HideInInspector]
    public EnemySpawner spawner;
    public Animator animator;

    public float speed = 3f;
    public float stopTime = 2f;
    public float minDistanceFromOthers = 1.5f;

    private Vector3 targetPosition;
    private float stopTimer;

    public void Initialize(WanderArea area, EnemySpawner enemySpawner)
    {
        wanderArea = area;
        spawner = enemySpawner;

        stopTimer = Random.Range(stopTime * 0.5f, stopTime * 1.5f);

        ChooseNewTarget();
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        MoveBehaviour();
    }

    void MoveBehaviour()
    {
        if (stopTimer > 0)
        {
            stopTimer -= Time.deltaTime;

            if (animator != null)
                animator.SetFloat("Speed", 0f);

            return;
        }

        MoveToTarget();
    }

    void MoveToTarget()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                5f * Time.deltaTime
            );
        }

        Vector3 newPos = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        newPos.y = transform.position.y;

        transform.position = newPos;

        if (animator != null && animator.runtimeAnimatorController != null)
            animator.SetFloat("Speed", 1f);

        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance < 0.2f)
        {
            stopTimer = stopTime;

            if (animator != null && animator.runtimeAnimatorController != null)
                animator.SetFloat("Speed", 0f);

            ChooseNewTarget();
        }
    }

    void ChooseNewTarget()
    {
        if (wanderArea == null)
            return;

        Vector3 candidatePosition;
        int attempts = 10;

        do
        {
            candidatePosition = wanderArea.GetRandomPosition();
            attempts--;
        } while (IsTooCloseToOtherEnemy(candidatePosition) && attempts > 0);

        targetPosition = candidatePosition;
    }

    bool IsTooCloseToOtherEnemy(Vector3 position)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            if (enemy == gameObject)
                continue;

            float dist = Vector3.Distance(position, enemy.transform.position);

            if (dist < minDistanceFromOthers)
                return true;
        }

        return false;
    }
}
