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
    private SkillEffectSpawnCoordinator effectSpawnCoordinator;
    private SkillImpactCoordinator impactCoordinator;
    private SkillFinishResolver finishResolver;

    public bool IsConfigured =>
        references != null
        && executionState != null
        && castOrchestrator != null
        && effectSpawnCoordinator != null
        && impactCoordinator != null
        && finishResolver != null;

    protected override void Validate() { }

    public void Configure(
        DigimonReferences references,
        SkillExecutionState executionState,
        SkillCastOrchestrator castOrchestrator,
        SkillEffectSpawnCoordinator effectSpawnCoordinator,
        SkillImpactCoordinator impactCoordinator,
        SkillFinishResolver finishResolver
    )
    {
        this.references = references;
        this.executionState = executionState;
        this.castOrchestrator = castOrchestrator;
        this.effectSpawnCoordinator = effectSpawnCoordinator;
        this.impactCoordinator = impactCoordinator;
        this.finishResolver = finishResolver;
    }

    public SkillUseCheckResult EvaluateSkillUse(DigimonSkill skill, GameObject target)
    {
        if (!EnsureConfigured())
            return SkillUseCheckResult.InvalidSkill;

        return castOrchestrator.Evaluate(Digimon, transform, skill, target);
    }

    public void TryUseSkill(DigimonSkill skill, GameObject target)
    {
        if (!EnsureConfigured())
            return;

        castOrchestrator.TryStart(Digimon, transform, skill, target);
    }

    public void OnSpawnEffect()
    {
        if (!EnsureConfigured())
            return;

        var skill = executionState.CurrentSkill;
        var target = executionState.CurrentTarget;
        var firePoint = references?.FirePoint;

        if (skill == null || target == null || firePoint == null)
            return;

        var effect = effectSpawnCoordinator.Spawn(
            skill,
            target.transform,
            firePoint.position,
            firePoint.rotation
        );

        if (effect == null)
            return;

        executionState.SetEffect(effect);

        effect.OnImpact += OnCurrentEffectReachedTarget;
    }

    public void OnSwitchEffectToMoveToTarget()
    {
        executionState.CurrentEffect?.SwitchToMoveToTarget();
    }

    public void OnApplyHit()
    {
        if (!EnsureConfigured())
            return;

        impactCoordinator.TriggerDirectHitFlow(this);
    }

    public void OnCurrentEffectReachedTarget()
    {
        if (!EnsureConfigured())
            return;

        impactCoordinator.TriggerCurrentEffectImpactFlow(this);
        finishResolver.OnEffectReachedTarget();
    }

    public void OnFinishSkill()
    {
        if (!EnsureConfigured())
            return;

        castOrchestrator.Finish();
    }

    private bool EnsureConfigured()
    {
        return IsConfigured;
    }
}
