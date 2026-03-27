using UnityEngine;

public class DigimonAttack : ValidatedMonoBehaviour
{
    private DigimonCoreReferences core;
    private DigimonBattleReferences battle;

    public Digimon Digimon => core != null ? core.Digimon : null;

    public DigimonSkill CurrentSkill => executionState?.CurrentSkill;
    public Transform CurrentTarget => executionState?.CurrentTarget;

    public bool IsCasting => executionState != null && executionState.IsCasting;

    private SkillExecutionState executionState;
    private SkillCastOrchestrator castOrchestrator;
    private SkillEffectSpawnCoordinator effectSpawner;
    private SkillCoordinator skillCoordinator;
    private SkillFinishResolver finishResolver;
    private SkillTargetResolver targetResolver;

    private bool configured;

    public bool IsConfigured =>
        configured
        && core != null
        && battle != null
        && executionState != null
        && castOrchestrator != null
        && effectSpawner != null
        && skillCoordinator != null
        && finishResolver != null
        && targetResolver != null;

    protected override void Validate() { }

    public void Configure(
        DigimonCoreReferences core,
        DigimonBattleReferences battle,
        SkillExecutionState executionState,
        SkillCastOrchestrator castOrchestrator,
        SkillEffectSpawnCoordinator effectSpawner,
        SkillCoordinator skillCoordinator,
        SkillFinishResolver finishResolver,
        SkillTargetResolver targetResolver
    )
    {
        if (configured)
        {
            Debug.LogWarning($"DigimonAttack já configurado: {this}", this);
            return;
        }

        this.core = core;
        this.battle = battle;
        this.executionState = executionState;
        this.castOrchestrator = castOrchestrator;
        this.effectSpawner = effectSpawner;
        this.skillCoordinator = skillCoordinator;
        this.finishResolver = finishResolver;
        this.targetResolver = targetResolver;

        configured = true;
    }

    private bool EnsureConfigured()
    {
        if (!IsConfigured)
        {
            Debug.LogError($"❌ ATTACK NÃO CONFIGURADO → {this}", this);
            return false;
        }

        return true;
    }

    public void TryUseSkill(DigimonSkill skill, BattleContext context)
    {
        if (!EnsureConfigured())
            return;

        if (IsCasting)
            return;

        if (context == null || context.IsFinished)
            return;

        var target = ResolveTarget(context);

        if (target == null)
        {
            Debug.LogWarning("⚠️ Sem target — ignorando execução");
            return;
        }

        var result = castOrchestrator.Evaluate(Digimon, transform, skill, target);

        if (
            result == SkillUseCheckResult.OnCooldown
            || result == SkillUseCheckResult.AlreadyCasting
        )
        {
            return;
        }

        castOrchestrator.TryStart(Digimon, transform, skill, target);
    }

    public SkillUseCheckResult EvaluateSkillUse(DigimonSkill skill, BattleContext context)
    {
        if (!EnsureConfigured())
            return SkillUseCheckResult.InvalidSkill;

        if (context == null || context.IsFinished)
            return SkillUseCheckResult.InvalidTarget;

        var target = ResolveTarget(context);

        if (target == null)
            return SkillUseCheckResult.InvalidTarget;

        return castOrchestrator.Evaluate(Digimon, transform, skill, target);
    }

    private Transform ResolveTarget(BattleContext context)
    {
        if (context == null)
        {
            Debug.LogError("❌ BattleContext nulo");
            return null;
        }

        var self = Digimon;

        if (self == null)
        {
            Debug.LogError("❌ Digimon não encontrado no Attack");
            return null;
        }

        var opponent = context.GetOpponent(self);

        if (opponent == null)
        {
            Debug.LogWarning("⚠️ Oponente não encontrado");
            return null;
        }

        return opponent.transform;
    }

    public void OnApplyHit()
    {
        if (!EnsureConfigured())
            return;

        skillCoordinator.TriggerHit(this);
    }

    public void OnSpawnEffect()
    {
        if (!EnsureConfigured())
            return;

        var skill = executionState.CurrentSkill;
        var target = executionState.CurrentTarget;

        if (skill == null || target == null || !skill.IsEffect)
            return;

        var resolvedTarget = targetResolver.Resolve(target);
        if (resolvedTarget == null)
            return;

        var firePoint = battle.FirePoint != null ? battle.FirePoint : transform;

        var effect = effectSpawner.Spawn(
            skill,
            resolvedTarget,
            firePoint.position,
            firePoint.rotation
        );

        if (effect == null)
            return;

        executionState.SetProjectile(effect);

        var movement = skill.useDelayedMovement ? skill.initialMovement : skill.projectileMovement;

        effect.SetMovementType(movement);

        effect.OnImpact += HandleImpact;

        void HandleImpact(GameObject hitTarget)
        {
            skillCoordinator.TriggerHitFromProjectile(skill, hitTarget.transform);

            executionState.ClearProjectile();

            finishResolver.OnEffectImpact();

            effect.OnImpact -= HandleImpact;
        }
    }

    public void OnActivateProjectile()
    {
        if (!EnsureConfigured())
            return;

        var skill = executionState.CurrentSkill;
        var projectile = executionState.CurrentProjectile;

        if (skill == null || projectile == null || !projectile.gameObject.activeInHierarchy)
            return;

        if (!skill.useDelayedMovement)
            return;

        projectile.SetMovementType(skill.delayedMovement);
    }

    public void OnFinishSkill()
    {
        if (!EnsureConfigured())
            return;

        executionState.ClearProjectile();

        finishResolver.OnAnimationFinished();
    }
}
