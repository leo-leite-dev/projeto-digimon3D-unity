using UnityEngine;

public class DigimonFollow : Digimon
{
    [Header("References")]
    public Transform player;
    public Transform followPoint;

    [SerializeField]
    private DigimonMovement movement;

    [SerializeField]
    private DigimonAttack attack;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Transform modelRoot;

    [Header("Follow")]
    [SerializeField]
    private float repathDistance = 0.25f;

    private GameObject currentModel;
    private Vector3 lastFollowPosition;

    private DigimonSkill pendingSkill;
    private GameObject requestedSkillTarget;
    private bool isTryingToUseSkill;

    protected override void Validate()
    {
        base.Validate();

        if (movement == null)
            movement = GetComponent<DigimonMovement>();

        if (attack == null)
            attack = GetComponent<DigimonAttack>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>(true);

        if (modelRoot == null)
        {
            Transform root = transform.Find("ModelRoot");

            if (root != null)
                modelRoot = root;
            else
                Debug.LogError(
                    $"{nameof(DigimonFollow)}: ModelRoot não encontrado no prefab.",
                    this
                );
        }
    }

    protected override void Awake()
    {
        base.Awake();

        if (movement == null)
            movement = GetComponent<DigimonMovement>();

        if (attack == null)
            attack = GetComponent<DigimonAttack>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>(true);

        if (movement == null)
            Debug.LogError($"{nameof(DigimonFollow)}: DigimonMovement não encontrado.", this);

        if (attack == null)
            Debug.LogError($"{nameof(DigimonFollow)}: DigimonAttack não encontrado.", this);
    }

    public void Initialize(Transform playerRef, Transform followPointRef)
    {
        player = playerRef;
        followPoint = followPointRef;

        if (followPoint != null)
            lastFollowPosition = followPoint.position;
    }

    private void Update()
    {
        if (data == null)
            return;

        if (player == null || followPoint == null)
            return;

        if (isTryingToUseSkill)
            HandlePendingSkill();
        else
            FollowPlayer();

        UpdateAnimation();
    }

    public void RequestSkillUse(DigimonSkill skill, GameObject target)
    {
        if (skill == null || target == null)
            return;

        pendingSkill = skill;
        requestedSkillTarget = target;
        isTryingToUseSkill = true;
    }

    private void HandlePendingSkill()
    {
        if (pendingSkill == null || requestedSkillTarget == null)
        {
            Debug.Log("[DigimonFollow] Pending skill ou target está null. Limpando requisição.");
            ClearPendingSkill();
            return;
        }

        if (attack == null || movement == null)
        {
            Debug.Log("[DigimonFollow] Attack ou Movement está null. Limpando requisição.");
            ClearPendingSkill();
            return;
        }

        if (!IsPendingTargetStillValid())
        {
            Debug.Log("[DigimonFollow] Target pendente não é mais válido. Limpando requisição.");
            ClearPendingSkill();
            return;
        }

        float distance = Vector3.Distance(
            transform.position,
            requestedSkillTarget.transform.position
        );

        SkillUseCheckResult result = attack.EvaluateSkillUse(pendingSkill, requestedSkillTarget);

        Debug.Log(
            $"[DigimonFollow] Skill={pendingSkill.skillName} | "
                + $"Target={requestedSkillTarget.name} | "
                + $"Dist={distance:F2} | "
                + $"Range={pendingSkill.range:F2} | "
                + $"Result={result}"
        );

        switch (result)
        {
            case SkillUseCheckResult.Success:
                Debug.Log(
                    "[DigimonFollow] Dentro do range. Parando movimento e tentando usar skill."
                );
                movement.StopMovement();
                attack.TryUseSkill(pendingSkill, requestedSkillTarget);
                ClearPendingSkill();
                break;

            case SkillUseCheckResult.OutOfRange:
                Debug.Log("[DigimonFollow] Fora do range. Movendo até a distância da skill.");
                movement.MoveToSkillRange(requestedSkillTarget.transform, pendingSkill.range);
                break;

            case SkillUseCheckResult.OnCooldown:
                Debug.Log("[DigimonFollow] Skill em cooldown. Parando movimento.");
                movement.StopMovement();
                break;

            case SkillUseCheckResult.AlreadyCasting:
                Debug.Log("[DigimonFollow] Digimon já está castando. Parando movimento.");
                movement.StopMovement();
                break;

            case SkillUseCheckResult.InvalidSkill:
                Debug.Log("[DigimonFollow] Skill inválida. Limpando requisição.");
                ClearPendingSkill();
                break;

            case SkillUseCheckResult.InvalidTarget:
                Debug.Log("[DigimonFollow] Target inválido. Limpando requisição.");
                ClearPendingSkill();
                break;

            case SkillUseCheckResult.MissingDigimonData:
                Debug.Log("[DigimonFollow] Digimon sem data. Limpando requisição.");
                ClearPendingSkill();
                break;

            default:
                Debug.Log("[DigimonFollow] Resultado inesperado. Limpando requisição.");
                ClearPendingSkill();
                break;
        }
    }

    private bool IsPendingTargetStillValid()
    {
        if (requestedSkillTarget == null)
            return false;

        if (!requestedSkillTarget.activeInHierarchy)
            return false;

        return true;
    }

    private void ClearPendingSkill()
    {
        pendingSkill = null;
        requestedSkillTarget = null;
        isTryingToUseSkill = false;

        if (followPoint != null)
            lastFollowPosition = followPoint.position;
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

    public void SpawnModel(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError($"{nameof(DigimonFollow)}: prefab do modelo é null.", this);
            return;
        }

        if (modelRoot == null)
        {
            Debug.LogError($"{nameof(DigimonFollow)}: ModelRoot não definido.", this);
            return;
        }

        if (currentModel != null)
            Destroy(currentModel);

        currentModel = Instantiate(prefab, modelRoot);
        currentModel.transform.localPosition = Vector3.zero;
        currentModel.transform.localRotation = Quaternion.identity;

        animator = currentModel.GetComponentInChildren<Animator>(true);

        SkillEffectSpawner spawner = GetComponent<SkillEffectSpawner>();
        if (spawner != null)
            spawner.RefreshFirePoint();

        if (animator == null)
            Debug.LogWarning($"{nameof(DigimonFollow)}: Animator não encontrado no modelo.", this);

        DigimonReferences references = GetComponent<DigimonReferences>();
        if (references != null)
        {
            references.RefreshReferences();
            Debug.Log("[DigimonFollow] References atualizadas após SpawnModel.", this);
        }
        else
        {
            Debug.LogWarning(
                "[DigimonFollow] DigimonReferences não encontrado após SpawnModel.",
                this
            );
        }

        DigimonAttackSceneBinder attackSceneBinder = GetComponent<DigimonAttackSceneBinder>();

        if (attackSceneBinder == null)
            attackSceneBinder = GetComponentInParent<DigimonAttackSceneBinder>();

        if (attackSceneBinder == null)
            attackSceneBinder = GetComponentInChildren<DigimonAttackSceneBinder>(true);

        if (attackSceneBinder != null)
        {
            attackSceneBinder.ComposeIfValid();
            Debug.Log("[DigimonFollow] DigimonAttackSceneBinder recomposto após SpawnModel.", this);
        }
        else
        {
            Debug.LogError(
                "[DigimonFollow] DigimonAttackSceneBinder não encontrado após SpawnModel.",
                this
            );
        }
    }

    private void UpdateAnimation()
    {
        if (animator == null || movement == null)
            return;

        animator.SetFloat("Speed", movement.Velocity.magnitude);
    }
}
