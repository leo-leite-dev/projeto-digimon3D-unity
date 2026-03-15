using UnityEngine;

public class DigimonFollow : Digimon
{
    [Header("References")]
    public Transform player;
    public Transform followPoint;

    private DigimonMovement movement;
    private Animator animator;

    [SerializeField]
    private Transform modelRoot;

    private GameObject currentModel;

    protected override void Validate()
    {
        base.Validate();

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

        movement = GetComponent<DigimonMovement>();
        animator = GetComponentInChildren<Animator>();

        if (movement == null)
            Debug.LogError($"{nameof(DigimonFollow)}: DigimonMovement não encontrado.", this);
    }

    public void Initialize(Transform playerRef, Transform followPointRef)
    {
        player = playerRef;
        followPoint = followPointRef;
    }

    void Update()
    {
        if (data == null)
            return;

        if (player == null || followPoint == null)
            return;

        FollowPlayer();
        UpdateAnimation();
    }

    void FollowPlayer()
    {
        if (movement == null)
            return;

        movement.FollowTarget(followPoint);
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

        animator = currentModel.GetComponentInChildren<Animator>();

        SkillEffectSpawner spawner = GetComponent<SkillEffectSpawner>();
        if (spawner != null)
            spawner.RefreshFirePoint();

        if (animator == null)
        {
            Debug.LogWarning($"{nameof(DigimonFollow)}: Animator não encontrado no modelo.", this);
            return;
        }
    }

    void UpdateAnimation()
    {
        if (animator == null || movement == null)
            return;

        float speed = movement.Velocity.magnitude;

        animator.SetFloat("Speed", speed);
    }
}
