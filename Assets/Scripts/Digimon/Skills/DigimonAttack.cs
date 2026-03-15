using System.Collections.Generic;
using UnityEngine;

public class DigimonAttack : ValidatedMonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Digimon digimon;

    [SerializeField]
    private DigimonSkillAnimator skillAnimator;

    [SerializeField]
    private SkillEffectSpawner effectSpawner;

    [SerializeField]
    private SkillDamageResolver damageResolver;

    [SerializeField]
    private DigimonMovement movement;

    public Digimon Digimon => digimon;
    public DigimonSkill CurrentSkill => currentSkill;
    public GameObject CurrentTarget => currentTarget;
    public bool IsCasting => isCasting;

    private readonly Dictionary<DigimonSkill, float> cooldowns = new();

    private DigimonSkill currentSkill;
    private GameObject currentTarget;
    private SkillEffect currentEffect;

    private bool isCasting;
    private bool hasSpawnedEffectForCurrentSkill;

    protected override void Validate()
    {
        RefreshReferences();
    }

    protected override void Awake()
    {
        base.Awake();
        RefreshReferences();
    }

    public void RefreshReferences()
    {
        if (movement == null)
            movement = GetComponent<DigimonMovement>();

        if (digimon == null)
            digimon = GetComponent<Digimon>();

        if (digimon == null)
            digimon = GetComponentInParent<Digimon>();

        if (skillAnimator == null)
            skillAnimator = GetComponentInChildren<DigimonSkillAnimator>(true);

        if (effectSpawner == null)
            effectSpawner = GetComponentInChildren<SkillEffectSpawner>(true);

        if (damageResolver == null)
            damageResolver = GetComponentInChildren<SkillDamageResolver>(true);
    }

    public void TryUseSkill(DigimonSkill skill, GameObject target)
    {
        RefreshReferences();

        if (!gameObject.activeInHierarchy)
            return;

        if (isCasting)
            return;

        if (target == null)
            return;

        if (digimon == null || digimon.data == null)
            return;

        if (skill == null)
            return;

        if (!CanUseSkill(skill, target))
            return;

        StartSkill(skill, target);
    }

    bool CanUseSkill(DigimonSkill skill, GameObject target)
    {
        if (
            cooldowns.TryGetValue(skill, out float lastTime)
            && Time.time < lastTime + skill.cooldown
        )
        {
            return false;
        }

        float sqrDistance = (target.transform.position - transform.position).sqrMagnitude;
        float rangeSqr = skill.range * skill.range;

        return sqrDistance <= rangeSqr;
    }

    void StartSkill(DigimonSkill skill, GameObject target)
    {
        currentSkill = skill;
        currentTarget = target;
        currentEffect = null;
        hasSpawnedEffectForCurrentSkill = false;
        isCasting = true;

        if (movement != null)
            movement.AddMovementLock();

        RotateToTarget(target);

        Debug.Log($"{digimon.Name} usou {skill.skillName}");

        if (skillAnimator != null)
            skillAnimator.PlaySkill(skill);

        cooldowns[skill] = Time.time;
    }

    void RotateToTarget(GameObject target)
    {
        Vector3 direction = target.transform.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = targetRotation;
    }

    public void TriggerEffectHitFlow()
    {
        RefreshReferences();

        if (currentSkill == null || currentTarget == null)
            return;

        if (currentSkill.hitTriggerType != SkillHitTriggerType.Effect)
            return;

        if (effectSpawner == null)
            return;

        if (hasSpawnedEffectForCurrentSkill)
            return;

        currentEffect = effectSpawner.Spawn(currentSkill, currentTarget.transform, digimon, this);

        if (currentEffect != null)
            hasSpawnedEffectForCurrentSkill = true;
    }

    public void TriggerDirectHitFlow()
    {
        RefreshReferences();

        if (currentSkill == null || currentTarget == null)
            return;

        if (currentSkill.hitTriggerType != SkillHitTriggerType.Direct)
            return;

        if (damageResolver == null)
            return;

        damageResolver.Apply(currentSkill, currentTarget.transform, digimon);
    }

    public void SwitchCurrentEffectToMoveToTarget()
    {
        if (currentEffect == null)
            return;

        currentEffect.SwitchToMoveToTarget();
    }

    public void OnCurrentEffectReachedTarget()
    {
        if (!isCasting)
            return;

        if (currentSkill == null)
            return;

        if (currentSkill.finishMode != SkillFinishMode.EffectImpact)
            return;

        currentEffect = null;
        FinishSkill();
    }

    // public void EndCurrentEffect()
    // {
    //     if (currentEffect == null)
    //         return;

    //     currentEffect.EndEffectFromAnimation();
    //     currentEffect = null;
    // }

    public void FinishSkill()
    {
        if (movement != null)
            movement.RemoveMovementLock();

        currentSkill = null;
        currentTarget = null;
        currentEffect = null;
        hasSpawnedEffectForCurrentSkill = false;
        isCasting = false;
    }

    public void ResetCooldowns()
    {
        cooldowns.Clear();
    }
}
