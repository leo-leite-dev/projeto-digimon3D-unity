using UnityEngine;

public class DigimonFollow : Digimon
{
    [Header("References")]
    public PlayerMovement player;

    private Animator animator;
    private DigimonAttack attack;

    [SerializeField]
    private Transform modelRoot;

    private GameObject currentModel;

    [Header("Leash")]
    public float leashDistance = 20f;

    [Header("Follow")]
    public int followDelay = 15;
    public float speed = 5f;
    public float stopDistance = 2f;
    public float rotationSpeed = 10f;

    [Header("Catch Up")]
    public float catchUpDistance = 8f;
    public float catchUpSpeedMultiplier = 1.8f;

    protected override void Awake()
    {
        base.Awake();

        if (player == null)
        {
            player = FindFirstObjectByType<PlayerMovement>();

            if (player == null)
                Debug.LogWarning("DigimonFollow: PlayerMovement não encontrado.");
        }

        animator = GetComponentInChildren<Animator>();
        attack = GetComponentInChildren<DigimonAttack>();
    }

    void Update()
    {
        if (data == null || player == null)
            return;

        if (attack != null && attack.IsAttacking)
        {
            CheckLeash();
            return;
        }

        FollowPlayer();
    }

    public void SpawnModel(GameObject prefab)
    {
        if (currentModel != null)
            Destroy(currentModel);

        currentModel = Instantiate(prefab, modelRoot);

        currentModel.transform.localPosition = Vector3.zero;
        currentModel.transform.localRotation = Quaternion.identity;

        animator = currentModel.GetComponentInChildren<Animator>();
    }

    void FollowPlayer()
    {
        Vector3 playerPos = player.transform.position;

        float sqrDistance = (transform.position - playerPos).sqrMagnitude;

        if (sqrDistance > catchUpDistance * catchUpDistance)
        {
            MoveTo(playerPos, speed * catchUpSpeedMultiplier);
            return;
        }

        bool playerMoving = player.IsMoving;

        if (playerMoving)
        {
            Vector3[] history = player.PositionHistory;

            if (history.Length <= followDelay)
            {
                UpdateAnimation(0f);
                return;
            }

            if (sqrDistance <= stopDistance * stopDistance)
            {
                UpdateAnimation(0f);
                return;
            }

            Vector3 targetPosition = history[followDelay];

            MoveTo(targetPosition, speed);
        }
        else
        {
            Vector3 targetPosition = playerPos - player.transform.forward * stopDistance;

            float sqrDist = (transform.position - targetPosition).sqrMagnitude;

            if (sqrDist > 0.01f)
                MoveTo(targetPosition, speed);
            else
                UpdateAnimation(0f);
        }
    }

    void MoveTo(Vector3 target, float moveSpeed)
    {
        Vector3 direction = target - transform.position;

        if (direction.sqrMagnitude < 0.001f)
            return;

        direction.Normalize();

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            moveSpeed * Time.deltaTime
        );

        Rotate(direction);
        UpdateAnimation(moveSpeed);
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

    void CheckLeash()
    {
        float sqrDistance = (transform.position - player.transform.position).sqrMagnitude;

        if (sqrDistance > leashDistance * leashDistance)
        {
            if (attack.targetSystem != null)
                attack.targetSystem.currentTarget = null;
        }
    }

    void UpdateAnimation(float moveSpeed)
    {
        if (animator == null)
            return;

        animator.SetFloat("Speed", moveSpeed);
    }
}
