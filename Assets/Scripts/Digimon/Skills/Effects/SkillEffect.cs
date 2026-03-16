using UnityEngine;

public class SkillEffect : MonoBehaviour
{
    private float currentSpeed;

    [SerializeField]
    private float hitDistance = 1.5f;

    [SerializeField]
    private SkillDamageResolver damageResolver;

    [Header("Behavior")]
    [SerializeField]
    private bool useLifetimeForStaticEffects = false;

    private Transform target;
    private DigimonSkill skill;
    private Digimon attacker;
    private DigimonAttack ownerAttack;

    private bool hasAppliedHit;
    private float spawnTime;
    private bool wasEndedByAnimation;
    private bool forceMoveToTarget;

    private bool hasAppliedDamage;
    private bool hasReachedTarget;

    private void Awake()
    {
        if (damageResolver == null)
            damageResolver = GetComponent<SkillDamageResolver>();
    }

    private void Reset()
    {
        if (damageResolver == null)
            damageResolver = GetComponent<SkillDamageResolver>();
    }

    public void Setup(
        Transform newTarget,
        DigimonSkill newSkill,
        Digimon newAttacker,
        DigimonAttack newOwnerAttack
    )
    {
        target = newTarget;
        skill = newSkill;
        attacker = newAttacker;
        ownerAttack = newOwnerAttack;

        forceMoveToTarget = false;
        hasAppliedHit = false;
        hasAppliedDamage = false;
        hasReachedTarget = false;
        wasEndedByAnimation = false;
        spawnTime = Time.time;
        currentSpeed = skill != null ? skill.projectileSpeed : 10f;
    }

    private void Update()
    {
        if (skill == null)
            return;

        if (forceMoveToTarget)
        {
            UpdateMoveToTarget();
            HandleLifetime();
            return;
        }

        switch (skill.projectileMovement)
        {
            case ProjectileMovementType.MoveToTarget:
                UpdateMoveToTarget();
                break;

            case ProjectileMovementType.Static:
                UpdateStatic();
                break;

            case ProjectileMovementType.Beam:
                UpdateBeam();
                break;
        }

        HandleLifetime();
    }

    void UpdateMoveToTarget()
    {
        if (!HasTarget())
            return;

        transform.LookAt(target);

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            currentSpeed * Time.deltaTime
        );

        float sqrDistance = (transform.position - target.position).sqrMagnitude;
        float hitDistanceSqr = hitDistance * hitDistance;
        bool hit = sqrDistance <= hitDistanceSqr;

        if (!hit || hasReachedTarget)
            return;

        hasReachedTarget = true;

        TriggerHit();

        if (ownerAttack != null)
            ownerAttack.OnCurrentEffectReachedTarget();

        EndEffect();
    }

    void UpdateStatic()
    {
        if (!HasTarget())
            return;

        if (!hasAppliedHit && Time.time >= spawnTime + skill.hitDelay)
        {
            TriggerHit();
            hasAppliedHit = true;
        }
    }

    void UpdateBeam()
    {
        if (!HasTarget())
            return;

        if (!hasAppliedHit && Time.time >= spawnTime + skill.hitDelay)
        {
            TriggerHit();
            hasAppliedHit = true;
        }
    }

    public void SwitchToMoveToTarget()
    {
        forceMoveToTarget = true;
    }

    bool HasTarget()
    {
        if (target == null)
        {
            EndEffect();
            return false;
        }

        return true;
    }

    void TriggerHit()
    {
        if (hasAppliedDamage)
            return;

        if (skill == null)
            return;

        if (skill.hitTriggerType != SkillHitTriggerType.Effect)
            return;

        hasAppliedDamage = true;
        hasAppliedHit = true;

        if (damageResolver == null)
            return;

        damageResolver.Apply(skill, target, attacker);
    }

    void HandleLifetime()
    {
        if (skill == null || wasEndedByAnimation)
            return;

        if (forceMoveToTarget || skill.projectileMovement == ProjectileMovementType.MoveToTarget)
            return;

        if (useLifetimeForStaticEffects && Time.time >= spawnTime + skill.lifeTime)
            EndEffect();
    }

    public void EndEffectFromAnimation()
    {
        wasEndedByAnimation = true;
        EndEffect();
    }

    void EndEffect()
    {
        Destroy(gameObject);
    }
}
