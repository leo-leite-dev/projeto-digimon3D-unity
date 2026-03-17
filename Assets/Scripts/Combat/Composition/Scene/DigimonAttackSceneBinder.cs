using UnityEngine;

public class DigimonAttackSceneBinder : ValidatedMonoBehaviour
{
    [SerializeField]
    private DigimonAttack attack;

    [SerializeField]
    private DigimonReferences references;

    [SerializeField]
    private ProjectilePool projectilePool;

    private readonly DigimonAttackConfigurator configurator = new DigimonAttackConfigurator();
    private readonly DigimonAttackDependenciesFactory factory =
        new DigimonAttackDependenciesFactory();

    protected override void Validate()
    {
        AutoBindLocalComponents();
    }

    public void RefreshBindings()
    {
        AutoBindLocalComponents();
    }

    public void ComposeIfValid()
    {
        AutoBindLocalComponents();

        if (projectilePool == null)
            projectilePool = ProjectilePool.Instance ?? FindFirstObjectByType<ProjectilePool>();

        if (projectilePool == null)
        {
            Debug.LogError("ProjectilePool não encontrado na cena!");
            return;
        }

        if (!ValidateReferences())
        {
            Debug.LogWarning("[DigimonAttackSceneBinder] Referências inválidas.", this);
            return;
        }

        var hitExecutor = new SkillHitExecutor(references.DamageResolver, references.Digimon);

        var dependencies = factory.Create(references, hitExecutor, projectilePool);

        configurator.ConfigureAttack(
            attack,
            references,
            dependencies.ExecutionState,
            dependencies.CastOrchestrator,
            dependencies.EffectSpawnCoordinator,
            dependencies.ImpactCoordinator,
            dependencies.FinishResolver
        );

        BindAnimatorEvents();
    }

    private void BindAnimatorEvents()
    {
        var animatorBinder = GetComponent<DigimonAnimatorEventBinder>();

        if (animatorBinder != null)
            animatorBinder.Configure(references, attack);
    }

    private void AutoBindLocalComponents()
    {
        attack = GetRequired(attack);
        references = GetRequired(references);

        if (projectilePool == null)
            projectilePool = FindFirstObjectByType<ProjectilePool>();
    }

    private bool ValidateReferences()
    {
        return attack != null
            && references != null
            && references.Digimon != null
            && references.DamageResolver != null
            && projectilePool != null;
    }
}
