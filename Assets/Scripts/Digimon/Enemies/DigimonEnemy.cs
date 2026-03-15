using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyWander))]
public class DigimonEnemy : Digimon
{
    [Header("Model")]
    public Transform modelRoot;

    [Header("Target")]
    [SerializeField]
    private Transform targetPoint;
    public Transform TargetPoint => targetPoint;

    private GameObject currentModel;

    protected override void Awake()
    {
        FindTargetPointIfNeeded();
        base.Awake();
    }

    void FindTargetPointIfNeeded()
    {
        if (targetPoint != null)
            return;

        Transform found = transform.Find("Target");

        if (found != null)
            targetPoint = found;
    }

    public override void Initialize(DigimonData data)
    {
        base.Initialize(data);

        FindTargetPointIfNeeded();
        SpawnModel();
    }

    void SpawnModel()
    {
        if (currentModel != null)
            Destroy(currentModel);

        if (data == null || data.modelPrefab == null || modelRoot == null)
            return;

        currentModel = Instantiate(data.modelPrefab, modelRoot);
        currentModel.transform.localPosition = Vector3.zero;
        currentModel.transform.localRotation = Quaternion.identity;

        Animator animator = currentModel.GetComponentInChildren<Animator>();

        EnemyWander wander = GetComponent<EnemyWander>();

        if (wander != null)
            wander.animator = animator;
    }

    public void OnSkillEffectReached(DigimonAttack ownerAttack)
    {
        Debug.Log($"{name} -> skill chegou no alvo");

        if (ownerAttack == null)
        {
            Debug.LogWarning($"{name} -> ownerAttack veio null");
            return;
        }

        ownerAttack.FinishSkill();
    }
}
