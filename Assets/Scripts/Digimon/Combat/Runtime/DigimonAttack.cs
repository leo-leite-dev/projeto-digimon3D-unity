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
    private SkillTargetResolver targetResolver;

    private bool configured;

    public bool IsConfigured =>
        configured
        && references != null
        && executionState != null
        && castOrchestrator != null
        && effectSpawnCoordinator != null
        && impactCoordinator != null
        && finishResolver != null
        && targetResolver != null;

    protected override void Validate() { }

    public void Configure(
        DigimonReferences references,
        SkillExecutionState executionState,
        SkillCastOrchestrator castOrchestrator,
        SkillEffectSpawnCoordinator effectSpawnCoordinator,
        SkillImpactCoordinator impactCoordinator,
        SkillFinishResolver finishResolver,
        SkillTargetResolver targetResolver
    )
    {
        if (configured)
        {
            Debug.LogWarning($"DigimonAttack já configurado: {this}", this);
            return;
        }

        this.references = references ?? throw new System.ArgumentNullException(nameof(references));
        this.executionState =
            executionState ?? throw new System.ArgumentNullException(nameof(executionState));
        this.castOrchestrator =
            castOrchestrator ?? throw new System.ArgumentNullException(nameof(castOrchestrator));
        this.effectSpawnCoordinator =
            effectSpawnCoordinator
            ?? throw new System.ArgumentNullException(nameof(effectSpawnCoordinator));
        this.impactCoordinator =
            impactCoordinator ?? throw new System.ArgumentNullException(nameof(impactCoordinator));
        this.finishResolver =
            finishResolver ?? throw new System.ArgumentNullException(nameof(finishResolver));
        this.targetResolver =
            targetResolver ?? throw new System.ArgumentNullException(nameof(targetResolver));

        configured = true;

        Debug.Log($"CONFIGURADO ATTACK: {this}");
    }

    public SkillUseCheckResult EvaluateSkillUse(DigimonSkill skill, GameObject target)
    {
        if (!EnsureConfigured())
            return SkillUseCheckResult.InvalidSkill;

        return castOrchestrator.Evaluate(Digimon, transform, skill, target);
    }

    // public void TryUseSkill(DigimonSkill skill, GameObject target)
    // {
    //     if (!EnsureConfigured())
    //         return;

    //     Debug.Log($"[ATTACK] Tentando usar skill: {skill}");

    //     castOrchestrator.TryStart(Digimon, transform, skill, target);
    // }

    public void TryUseSkill(DigimonSkill skill, GameObject target)
    {
        Debug.Log("🟡 [FLOW] TryUseSkill chamado");

        if (!EnsureConfigured())
        {
            Debug.Log("❌ [FLOW] Attack NÃO configurado");
            return;
        }

        Debug.Log($"🟡 [FLOW] Skill: {skill} | Target: {target}");

        var result = castOrchestrator.Evaluate(Digimon, transform, skill, target);

        Debug.Log($"🟡 [FLOW] Resultado Evaluate: {result}");

        if (result != SkillUseCheckResult.Success)
        {
            Debug.Log("❌ [FLOW] Skill NÃO passou na validação");
            return;
        }

        Debug.Log("🟢 [FLOW] Chamando TryStart");

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
        {
            Debug.LogWarning("OnSpawnEffect abortado → dados inválidos", this);
            return;
        }

        var resolvedTarget = targetResolver.Resolve(target);

        if (resolvedTarget == null)
        {
            Debug.LogError("Target inválido após resolução.", this);
            return;
        }

        var effect = effectSpawnCoordinator.Spawn(
            skill,
            resolvedTarget,
            firePoint.position,
            firePoint.rotation
        );

        if (effect == null)
            return;

        if (executionState.CurrentEffect != null)
            executionState.CurrentEffect.OnImpact -= OnCurrentEffectReachedTarget;

        executionState.SetEffect(effect);

        effect.OnImpact += OnCurrentEffectReachedTarget;
    }

    public void OnSwitchEffectToMoveToTarget()
    {
        if (!EnsureConfigured())
            return;

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
        if (!IsConfigured)
        {
            Debug.LogError($"ATTACK NÃO CONFIGURADO → {this}", this);
            return false;
        }

        return true;
    }
}
