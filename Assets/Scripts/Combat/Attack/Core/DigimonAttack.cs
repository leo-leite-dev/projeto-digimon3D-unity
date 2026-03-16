using UnityEngine;

public class DigimonAttack : ValidatedMonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private DigimonReferences references;

    public Digimon Digimon => references != null ? references.Digimon : null;
    public DigimonSkill CurrentSkill => executionState?.CurrentSkill;
    public GameObject CurrentTarget => executionState?.CurrentTarget;
    public bool IsCasting => executionState != null && executionState.IsCasting;

    private SkillExecutionState executionState;
    private SkillCastOrchestrator castOrchestrator;
    private SkillImpactCoordinator impactCoordinator;
    private SkillFinishResolver finishResolver;

    public bool IsConfigured =>
        references != null
        && executionState != null
        && castOrchestrator != null
        && impactCoordinator != null
        && finishResolver != null;

    protected override void Validate() { }

    private void Awake()
    {
        base.Awake();
        Debug.Log($"[DigimonAttack] Awake em {name}", this);
    }

    private void OnEnable()
    {
        Debug.Log("### DIGIMON ATTACK ONENABLE NOVO ###", this);
        SubscribeToAnimatorEvents();
    }

    public void Configure(
        DigimonReferences references,
        SkillExecutionState executionState,
        SkillCastOrchestrator castOrchestrator,
        SkillImpactCoordinator impactCoordinator,
        SkillFinishResolver finishResolver
    )
    {
        Debug.Log($"[DigimonAttack] Configure chamado em {name}", this);

        UnsubscribeFromAnimatorEvents();

        this.references = references;
        this.executionState = executionState;
        this.castOrchestrator = castOrchestrator;
        this.impactCoordinator = impactCoordinator;
        this.finishResolver = finishResolver;

        Debug.Log(
            $"[DigimonAttack] Configure -> references={(this.references != null ? this.references.name : "null")} | animator={(this.references != null && this.references.SkillAnimator != null ? this.references.SkillAnimator.name : "null")}",
            this
        );

        SubscribeToAnimatorEvents();
    }

    private void SubscribeToAnimatorEvents()
    {
        if (references == null)
        {
            Debug.LogWarning("[DigimonAttack] Subscribe -> references == null", this);
            return;
        }

        if (references.SkillAnimator == null)
        {
            Debug.LogWarning("[DigimonAttack] Subscribe -> references.SkillAnimator == null", this);
            return;
        }

        Debug.Log($"[DigimonAttack] Subscribe -> animator={references.SkillAnimator.name}", this);

        references.SkillAnimator.OnSpawnEffect -= HandleSpawnEffect;
        references.SkillAnimator.OnSwitchEffectToMoveToTarget -= HandleSwitchEffectToMoveToTarget;
        references.SkillAnimator.OnApplyHit -= HandleApplyHit;
        references.SkillAnimator.OnFinishSkill -= HandleFinishSkill;

        references.SkillAnimator.OnSpawnEffect += HandleSpawnEffect;
        references.SkillAnimator.OnSwitchEffectToMoveToTarget += HandleSwitchEffectToMoveToTarget;
        references.SkillAnimator.OnApplyHit += HandleApplyHit;
        references.SkillAnimator.OnFinishSkill += HandleFinishSkill;
    }

    private void UnsubscribeFromAnimatorEvents()
    {
        if (references == null || references.SkillAnimator == null)
            return;

        Debug.Log(
            $"[DigimonAttack] UnsubscribeFromAnimatorEvents -> removendo de {references.SkillAnimator.name}",
            this
        );

        references.SkillAnimator.OnSpawnEffect -= HandleSpawnEffect;
        references.SkillAnimator.OnSwitchEffectToMoveToTarget -= HandleSwitchEffectToMoveToTarget;
        references.SkillAnimator.OnApplyHit -= HandleApplyHit;
        references.SkillAnimator.OnFinishSkill -= HandleFinishSkill;
    }

    private void HandleSpawnEffect()
    {
        Debug.Log("[DigimonAttack] HandleSpawnEffect", this);
        TriggerEffectHitFlow();
    }

    private void HandleSwitchEffectToMoveToTarget()
    {
        Debug.Log("[DigimonAttack] HandleSwitchEffectToMoveToTarget", this);
        SwitchCurrentEffectToMoveToTarget();
    }

    private void HandleApplyHit()
    {
        Debug.Log("[DigimonAttack] HandleApplyHit", this);
        TriggerDirectHitFlow();
    }

    private void HandleFinishSkill()
    {
        Debug.Log("[DigimonAttack] HandleFinishSkill", this);
        FinishSkill();
    }

    public void TryUseSkill(DigimonSkill skill, GameObject target)
    {
        if (!IsConfigured)
        {
            Debug.LogError($"{nameof(DigimonAttack)} is not configured.", this);
            return;
        }

        bool started = castOrchestrator.TryStart(Digimon, transform, skill, target);
        Debug.Log(
            $"[DigimonAttack] TryUseSkill -> started={started} | skill={skill?.skillName} | target={target?.name}",
            this
        );
    }

    public bool CanUseSkill(DigimonSkill skill, GameObject target)
    {
        return EvaluateSkillUse(skill, target) == SkillUseCheckResult.Success;
    }

    public SkillUseCheckResult EvaluateSkillUse(DigimonSkill skill, GameObject target)
    {
        if (!IsConfigured)
        {
            Debug.LogError($"{nameof(DigimonAttack)} is not configured.", this);
            return SkillUseCheckResult.InvalidSkill;
        }

        return castOrchestrator.Evaluate(Digimon, transform, skill, target);
    }

    public void TriggerEffectHitFlow()
    {
        Debug.Log("[DigimonAttack] TriggerEffectHitFlow", this);

        if (!IsConfigured)
        {
            Debug.LogError($"{nameof(DigimonAttack)} is not configured.", this);
            return;
        }

        impactCoordinator.TriggerEffectHitFlow(this);
    }

    public void TriggerDirectHitFlow()
    {
        Debug.Log("[DigimonAttack] TriggerDirectHitFlow", this);

        if (!IsConfigured)
        {
            Debug.LogError($"{nameof(DigimonAttack)} is not configured.", this);
            return;
        }

        impactCoordinator.TriggerDirectHitFlow();
    }

    public void SwitchCurrentEffectToMoveToTarget()
    {
        Debug.Log("[DigimonAttack] SwitchCurrentEffectToMoveToTarget", this);

        if (!IsConfigured)
        {
            Debug.LogError($"{nameof(DigimonAttack)} is not configured.", this);
            return;
        }

        impactCoordinator.SwitchCurrentEffectToMoveToTarget();
    }

    public void OnCurrentEffectReachedTarget()
    {
        Debug.Log("[DigimonAttack] OnCurrentEffectReachedTarget", this);

        if (!IsConfigured)
        {
            Debug.LogError($"{nameof(DigimonAttack)} is not configured.", this);
            return;
        }

        finishResolver.OnEffectReachedTarget();
    }

    public void FinishSkill()
    {
        Debug.Log("[DigimonAttack] FinishSkill", this);

        if (!IsConfigured)
        {
            Debug.LogError($"{nameof(DigimonAttack)} is not configured.", this);
            return;
        }

        castOrchestrator.Finish();
    }
}
