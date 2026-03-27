using UnityEngine;

public class DigimonBattleComposer : MonoBehaviour
{
    public void Compose(GameObject digimonGO, DigimonData data)
    {
        Debug.Log("⚔️ [Composer] START", digimonGO);

        if (
            !ValidateAll(
                digimonGO,
                out var core,
                out var battle,
                out var attack,
                out var controller
            )
        )
        {
            Debug.LogError("❌ [Composer] Validação falhou", digimonGO);
            return;
        }

        Debug.Log("✅ [Composer] Validação OK", digimonGO);

        if (!SetupDigimonData(core, data, digimonGO))
        {
            Debug.LogError("❌ [Composer] SetupDigimonData falhou", digimonGO);
            return;
        }

        Debug.Log("✅ [Composer] Data configurada", digimonGO);

        if (!SetupVisual(core, battle, digimonGO))
        {
            Debug.LogError("❌ [Composer] SetupVisual falhou", digimonGO);
            return;
        }

        Debug.Log("✅ [Composer] Visual configurado", digimonGO);

        SetupCombat(core, battle, attack);
        Debug.Log("✅ [Composer] Combat configurado", digimonGO);

        BindAnimator(battle, attack);
        Debug.Log("✅ [Composer] Animator bindado", digimonGO);

        InitializeController(controller, attack);
        Debug.Log("✅ [Composer] Controller inicializado", digimonGO);

        Debug.Log("🔥 [DigimonBattleComposer] COMPLETED", digimonGO);
    }

    private bool SetupDigimonData(DigimonCoreReferences core, DigimonData data, GameObject go)
    {
        if (data == null)
        {
            Debug.LogError("❌ DigimonData NULL", go);
            return false;
        }

        if (core.Digimon == null)
        {
            Debug.LogError("❌ Digimon component NULL", go);
            return false;
        }

        core.Digimon.Setup(data);

        Debug.Log($"[Composer] Digimon configurado: {data.digimonName}", go);

        return true;
    }

    private bool SetupVisual(
        DigimonCoreReferences core,
        DigimonBattleReferences battle,
        GameObject go
    )
    {
        Debug.Log("🎨 [Composer] Chamando SetupVisual...", go);

        var digimonAnimator = DigimonVisualComposer.SetupVisual(core, battle, go);

        if (digimonAnimator == null)
        {
            Debug.LogError("❌ SetupVisual retornou NULL", go);
            return false;
        }

        Debug.Log("🎨 [Composer] Animator criado com sucesso", go);

        DigimonVisualComposer.BindMovementAnimator(core, go);

        return true;
    }

    private void SetupCombat(
        DigimonCoreReferences core,
        DigimonBattleReferences battle,
        DigimonAttack attack
    )
    {
        Debug.Log("⚔️ [Composer] SetupCombat", this);

        SetupHitReceiver(core, battle);
        SetupAttack(core, battle, attack);
    }

    private void SetupHitReceiver(DigimonCoreReferences core, DigimonBattleReferences battle)
    {
        var receiver = battle.HitReceiver;

        if (receiver == null)
        {
            Debug.LogWarning("⚠️ HitReceiver não presente", this);
            return;
        }

        receiver.Initialize(core.Digimon, battle.DigimonAnimator);
    }

    private void SetupAttack(
        DigimonCoreReferences core,
        DigimonBattleReferences battle,
        DigimonAttack attack
    )
    {
        Debug.Log("🔥 [Composer] SetupAttack", this);

        if (battle.DamageResolver == null)
        {
            Debug.LogError("❌ DamageResolver NULL", this);
            return;
        }

        var factory = new DigimonAttackDependenciesFactory();

        var hitExecutor = new SkillHitExecutor(battle.DamageResolver, core.Digimon);

        var deps = factory.Create(core, battle, hitExecutor);

        deps.CastOrchestrator.SetRunner(attack);

        var targetResolver = new SkillTargetResolver();

        var configurator = new DigimonAttackConfigurator(targetResolver);

        configurator.ConfigureAttack(
            attack,
            core,
            battle,
            deps.ExecutionState,
            deps.CastOrchestrator,
            deps.EffectSpawnCoordinator,
            deps.ImpactCoordinator,
            deps.FinishResolver
        );
    }

    private void BindAnimator(DigimonBattleReferences battle, DigimonAttack attack)
    {
        var animator = battle.DigimonAnimator;

        if (animator == null)
        {
            Debug.LogError("❌ DigimonAnimator NULL no battle", this);
            return;
        }

        animator.OnSpawnEffect += attack.OnSpawnEffect;
        animator.OnApplyHit += attack.OnApplyHit;
        animator.OnFinishSkill += attack.OnFinishSkill;
        animator.OnActivateProjectile += attack.OnActivateProjectile;
    }

    private void InitializeController(DigimonBattleController controller, DigimonAttack attack)
    {
        if (controller == null)
        {
            Debug.LogError("❌ Controller NULL", this);
            return;
        }

        controller.Initialize(attack);
    }

    private bool ValidateAll(
        GameObject go,
        out DigimonCoreReferences core,
        out DigimonBattleReferences battle,
        out DigimonAttack attack,
        out DigimonBattleController controller
    )
    {
        core = go.GetComponent<DigimonCoreReferences>();
        battle = go.GetComponent<DigimonBattleReferences>();
        attack = go.GetComponent<DigimonAttack>();
        controller = go.GetComponent<DigimonBattleController>();

        bool valid = true;

        if (core == null || !core.IsValid())
        {
            Debug.LogError("❌ Core inválido", go);
            valid = false;
        }

        if (battle == null || !battle.IsValid())
        {
            Debug.LogError("❌ Battle inválido", go);
            valid = false;
        }

        if (attack == null)
        {
            Debug.LogError("❌ Attack faltando", go);
            valid = false;
        }

        if (controller == null)
        {
            Debug.LogError("❌ Controller faltando", go);
            valid = false;
        }

        return valid;
    }
}
