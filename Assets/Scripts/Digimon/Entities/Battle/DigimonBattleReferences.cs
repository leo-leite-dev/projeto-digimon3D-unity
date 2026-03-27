using UnityEngine;

public class DigimonBattleReferences : MonoBehaviour
{
    [Header("=== Combat ===")]
    [SerializeField]
    private DigimonAttack attack;

    [SerializeField]
    private DigimonHitReceiver hitReceiver;

    [SerializeField]
    private SkillDamageResolver damageResolver;

    [Header("=== Visual (Runtime) ===")]
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Transform firePoint;

    [SerializeField]
    private DigimonAnimator digimonAnimator;

    [Header("=== Services ===")]
    [SerializeField]
    private ProjectilePool projectilePool;

    public DigimonAttack Attack => attack;
    public DigimonHitReceiver HitReceiver => hitReceiver;
    public SkillDamageResolver DamageResolver => damageResolver;

    public Animator Animator => animator;
    public Transform FirePoint => firePoint;
    public DigimonAnimator DigimonAnimator => digimonAnimator;

    public ProjectilePool ProjectilePool => projectilePool;

    public void InjectVisual(
        Animator animator,
        Transform firePoint,
        DigimonAnimator digimonAnimator
    )
    {
        this.animator = animator;
        this.firePoint = firePoint;
        this.digimonAnimator = digimonAnimator;

        Debug.Log("🎨 [BattleReferences] Visual injetado com sucesso", this);
    }

    public bool IsValid()
    {
        bool valid = true;

        void Check(Object obj, string name)
        {
            if (obj == null)
            {
                Debug.LogError($"❌ BattleReferences: {name} missing", this);
                valid = false;
            }
        }

        Check(attack, nameof(attack));
        Check(hitReceiver, nameof(hitReceiver));
        Check(damageResolver, nameof(damageResolver));

        return valid;
    }
}
