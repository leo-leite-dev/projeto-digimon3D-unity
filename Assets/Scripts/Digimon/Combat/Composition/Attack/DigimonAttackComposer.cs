using UnityEngine;

public static class DigimonAttackComposer
{
    public static void Compose(
        DigimonAttack attack,
        DigimonReferences references,
        ProjectilePool projectilePool
    )
    {
        if (attack == null)
        {
            Debug.LogError("❌ DigimonAttack não encontrado");
            return;
        }

        if (references == null)
        {
            Debug.LogError("❌ DigimonReferences não encontrado");
            return;
        }

        var attacker = references.Digimon;

        if (attacker == null)
        {
            Debug.LogError("❌ Attacker (Digimon) não encontrado nas referências");
            return;
        }

        var damageResolver = new SkillDamageResolver();
        var hitExecutor = new SkillHitExecutor(damageResolver, attacker);

        var factory = new DigimonAttackDependenciesFactory();

        var dependencies = factory.Create(references, hitExecutor, projectilePool);

        var targetResolver = new SkillTargetResolver();
        var configurator = new DigimonAttackConfigurator(targetResolver);

        configurator.ConfigureAttack(
            attack,
            references,
            dependencies.ExecutionState,
            dependencies.CastOrchestrator,
            dependencies.EffectSpawnCoordinator,
            dependencies.ImpactCoordinator,
            dependencies.FinishResolver
        );
    }
}
