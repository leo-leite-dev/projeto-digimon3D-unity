using UnityEngine;
using UnityEngine.AI;

public class DigimonReferences : MonoBehaviour
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
    private Transform modelRoot;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Transform firePoint;

    [SerializeField]
    private DigimonAnimator digimonAnimator;

    [Header("Navigation")]
    [SerializeField]
    private NavMeshAgent navMeshAgent;

    [Header("Combat")]
    [SerializeField]
    private SkillDamageResolver damageResolver;

    [SerializeField]
    private DigimonHitReceiver hitReceiver;

    [Header("Services")]
    [SerializeField]
    private ProjectilePool projectilePool;

    public Digimon Digimon => digimon;
    public DigimonMovement Movement => movement;
    public DigimonAttack Attack => attack;
    public DigimonFollow Follow => follow;

    public Transform ModelRoot => modelRoot;
    public Animator Animator => animator;
    public Transform FirePoint => firePoint;
    public DigimonAnimator DigimonAnimator => digimonAnimator;

    public NavMeshAgent NavMeshAgent => navMeshAgent;

    public SkillDamageResolver DamageResolver => damageResolver;
    public DigimonHitReceiver HitReceiver => hitReceiver;

    public ProjectilePool ProjectilePool => projectilePool;

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

    public void SetVisualReferences(
        Transform modelRoot,
        Animator animator,
        Transform firePoint,
        DigimonAnimator digimonAnimator
    )
    {
        this.modelRoot = modelRoot;
        this.animator = animator;
        this.firePoint = firePoint;
        this.digimonAnimator = digimonAnimator;
    }

    public void BindRuntime(GameObject modelInstance)
    {
        animator = modelInstance.GetComponentInChildren<Animator>();
        firePoint = modelInstance.transform.Find("FirePoint");
    }

    public bool HasCoreReferences()
    {
        return digimon != null && movement != null && navMeshAgent != null;
    }

    public bool HasVisualReferences()
    {
        return modelRoot != null && animator != null;
    }
}
