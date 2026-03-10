using UnityEngine;

public class DigimonFollow : Digimon
{
    [Header("References")]
    public PlayerMovement player;
    public TargetSystem targetSystem;
    public DigimonAttack attack;

    private Animator animator;

    [Header("Follow")]
    public int followDelay = 15;
    public float speed = 5f;
    public float stopDistance = 2f;
    public float rotationSpeed = 10f;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
    }

    void OnEnable()
    {
        EnemyHealth.OnEnemyKilled += HandleEnemyKilled;
    }

    void OnDisable()
    {
        EnemyHealth.OnEnemyKilled -= HandleEnemyKilled;
    }

    void Update()
    {
        if (player == null)
            return;

        if (targetSystem != null && targetSystem.currentTarget != null)
        {
            FollowTarget();
            return;
        }

        FollowPlayer();
    }

    void FollowTarget()
    {
        if (attack == null || attack.skills.Count == 0)
            return;

        DigimonSkill skill = attack.skills[0];

        Vector3 targetPosition = targetSystem.currentTarget.transform.position;

        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance <= skill.range)
            return;

        Vector3 direction = (targetPosition - transform.position).normalized;

        Vector3 stopPosition = targetPosition - direction * skill.range;

        MoveTo(stopPosition);
    }

    void FollowPlayer()
    {
        Vector3 playerPos = player.transform.position;

        float distanceFromPlayer = Vector3.Distance(transform.position, playerPos);

        bool playerMoving = player.IsMoving;

        if (playerMoving)
        {
            Vector3[] history = player.PositionHistory;

            if (history.Length <= followDelay)
                return;

            if (distanceFromPlayer <= stopDistance)
                return;

            Vector3 targetPosition = history[followDelay];

            MoveTo(targetPosition);
        }
        else
        {
            Vector3 targetPosition = playerPos - player.transform.forward * stopDistance;

            float dist = Vector3.Distance(transform.position, targetPosition);

            if (dist > 0.1f)
                MoveTo(targetPosition);
        }
    }

    void MoveTo(Vector3 target)
    {
        Vector3 direction = target - transform.position;

        if (direction.sqrMagnitude < 0.001f)
            return;

        direction.Normalize();

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );

        Rotate(direction);

        UpdateAnimation();
    }

    void Rotate(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    void UpdateAnimation()
    {
        if (animator == null)
            return;

        animator.SetFloat("Speed", speed);
    }

    public void GainExp(int amount)
    {
        level.Experience += amount;

        while (level.Experience >= ExperienceCalculator.GetExpToNextLevel(level.Level))
        {
            level.Experience -= ExperienceCalculator.GetExpToNextLevel(level.Level);

            level.LevelUp();

            ApplyLevelGrowth();

            RecalculateStats();

            Debug.Log($"{Name} subiu para o nível {level.Level}");
        }
    }

    void HandleEnemyKilled(DigimonEnemy enemy)
    {
        int xp = ExperienceCalculator.CalculateExpGain(this, enemy);

        GainExp(xp);
    }
}
