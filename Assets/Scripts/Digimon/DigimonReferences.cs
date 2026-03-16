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

    [Header("Navigation")]
    [SerializeField]
    private NavMeshAgent navMeshAgent;

    [Header("Attack")]
    [SerializeField]
    private DigimonSkillAnimator skillAnimator;

    [SerializeField]
    private SkillEffectSpawner effectSpawner;

    [SerializeField]
    private SkillDamageResolver damageResolver;

    [SerializeField]
    private SkillHitExecutor hitExecutor;

    public Digimon Digimon => digimon;
    public DigimonMovement Movement => movement;
    public DigimonAttack Attack => attack;
    public DigimonFollow Follow => follow;

    public Animator Animator => animator;
    public Transform ModelRoot => modelRoot;

    public NavMeshAgent NavMeshAgent => navMeshAgent;

    public DigimonSkillAnimator SkillAnimator => skillAnimator;
    public SkillEffectSpawner EffectSpawner => effectSpawner;
    public SkillDamageResolver DamageResolver => damageResolver;
    public SkillHitExecutor HitExecutor => hitExecutor;

    protected override void Validate()
    {
        RefreshReferences();
    }

    protected override void Awake()
    {
        base.Awake();
        RefreshReferences();
    }

    [ContextMenu("Refresh References")]
    public void RefreshReferences()
    {
        RefreshCoreReferences();
        RefreshVisualReferences();
        ValidateVisualReferences();

        Debug.Log(
            $"[DigimonReferences] RefreshReferences -> modelRoot={modelRoot?.name} | animator={animator?.name} | skillAnimator={skillAnimator?.name}",
            this
        );
    }

    private void RefreshCoreReferences()
    {
        if (digimon == null)
            digimon = GetComponent<Digimon>();

        if (movement == null)
            movement = GetComponent<DigimonMovement>();

        if (attack == null)
            attack = GetComponent<DigimonAttack>();

        if (follow == null)
            follow = GetComponent<DigimonFollow>();

        if (navMeshAgent == null)
            navMeshAgent = GetComponent<NavMeshAgent>();

        if (effectSpawner == null)
            effectSpawner = GetComponent<SkillEffectSpawner>();

        if (damageResolver == null)
            damageResolver = GetComponent<SkillDamageResolver>();

        if (hitExecutor == null)
            hitExecutor = GetComponent<SkillHitExecutor>();
    }

    private void RefreshVisualReferences()
    {
        modelRoot = FindChildRecursive(transform, "ModelRoot");

        Transform searchRoot = modelRoot != null ? modelRoot : transform;

        animator = searchRoot.GetComponentInChildren<Animator>(true);
        skillAnimator = searchRoot.GetComponentInChildren<DigimonSkillAnimator>(true);
    }

    private void ValidateVisualReferences()
    {
        if (skillAnimator == null)
        {
            Debug.LogWarning("[DigimonReferences] DigimonSkillAnimator não encontrado.", this);
        }

        if (animator == null)
        {
            Debug.LogWarning("[DigimonReferences] Animator não encontrado.", this);
        }
    }

    private Transform FindChildRecursive(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
                return child;

            Transform found = FindChildRecursive(child, childName);
            if (found != null)
                return found;
        }

        return null;
    }
}
