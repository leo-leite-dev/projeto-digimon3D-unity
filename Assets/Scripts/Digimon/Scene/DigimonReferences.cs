using UnityEngine;
using UnityEngine.AI;

public class DigimonReferences : ValidatedMonoBehaviour
{
    [Header("Core")]
    [SerializeField]
    private Digimon digimon;

    [SerializeField]
    private DigimonMovement movement;

    [SerializeField]
    private DigimonAttack attack;

    [SerializeField]
    private DigimonFollow follow;

    [Header("Visual")]
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Transform modelRoot;

    [SerializeField]
    private DigimonAnimator digimonAnimator;

    [SerializeField]
    private Transform firePoint;

    [Header("Navigation")]
    [SerializeField]
    private NavMeshAgent navMeshAgent;

    [Header("Combat")]
    [SerializeField]
    private SkillDamageResolver damageResolver;

    [SerializeField]
    private DigimonHitReceiver hitReceiver;

    public Digimon Digimon => digimon;
    public DigimonMovement Movement => movement;
    public DigimonAttack Attack => attack;
    public DigimonFollow Follow => follow;

    public Animator Animator => animator;
    public Transform ModelRoot => modelRoot;
    public DigimonAnimator DigimonAnimator => digimonAnimator;
    public Transform FirePoint => firePoint;

    public NavMeshAgent NavMeshAgent => navMeshAgent;

    public SkillDamageResolver DamageResolver => damageResolver;
    public DigimonHitReceiver HitReceiver => hitReceiver;

    protected override void Awake()
    {
        base.Awake();
        RefreshCoreReferences();
    }

    protected override void Validate()
    {
        RefreshCoreReferences();
    }

    [ContextMenu("Refresh Core References")]
    public void RefreshCoreReferences()
    {
        DigimonCoreReferencesResolver.Refresh(this);
    }

    [ContextMenu("Refresh Visual References")]
    public void RefreshVisualReferences()
    {
        DigimonVisualReferencesResolver.Refresh(this);

        if (animator == null && modelRoot != null)
            animator = modelRoot.GetComponentInChildren<Animator>(true);

        if (firePoint == null && modelRoot != null)
        {
            var marker = modelRoot.GetComponentInChildren<FirePoint>(true);
            if (marker != null)
                firePoint = marker.transform;
        }
    }

    public void SetCoreReferences(
        Digimon digimon,
        DigimonMovement movement,
        DigimonAttack attack,
        DigimonFollow follow,
        NavMeshAgent navMeshAgent,
        DigimonAnimator digimonAnimator,
        SkillDamageResolver damageResolver,
        DigimonHitReceiver hitReceiver
    )
    {
        this.digimon = digimon;
        this.movement = movement;
        this.attack = attack;
        this.follow = follow;
        this.navMeshAgent = navMeshAgent;
        this.digimonAnimator = digimonAnimator;
        this.damageResolver = damageResolver;
        this.hitReceiver = hitReceiver;
    }

    public void SetVisualInternal(Transform modelRoot, Animator animator)
    {
        this.modelRoot = modelRoot;
        this.animator = animator;
    }

    public void InitializeAfterModelSpawn(Transform modelRoot, Animator animator)
    {
        SetVisualInternal(modelRoot, animator);

        RefreshVisualReferences();
    }

    public bool HasVisualReferences()
    {
        return modelRoot != null && animator != null;
    }
}
