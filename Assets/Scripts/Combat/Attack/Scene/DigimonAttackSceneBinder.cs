using UnityEngine;

public class DigimonAttackSceneBinder : MonoBehaviour
{
    [SerializeField]
    private DigimonAttack attack;

    [SerializeField]
    private DigimonReferences references;

    [SerializeField]
    private SkillHitExecutor hitExecutor;

    private readonly DigimonAttackComposer composer = new DigimonAttackComposer();
    private readonly DigimonAttackDependenciesFactory factory =
        new DigimonAttackDependenciesFactory();

    private void Reset()
    {
        AutoBindLocalComponents();
    }

    private void OnValidate()
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

        if (!ValidateReferences())
            return;

        var dependencies = factory.Create(references, hitExecutor);

        composer.Compose(
            attack,
            references,
            dependencies.ExecutionState,
            dependencies.CastOrchestrator,
            dependencies.ImpactCoordinator,
            dependencies.FinishResolver
        );
    }

    private void AutoBindLocalComponents()
    {
        if (attack == null)
            attack = GetComponent<DigimonAttack>();

        if (references == null)
            references = GetComponent<DigimonReferences>();

        if (hitExecutor == null)
            hitExecutor = GetComponent<SkillHitExecutor>();
    }

    private bool ValidateReferences()
    {
        if (attack == null)
            return false;

        if (references == null)
            return false;

        if (hitExecutor == null)
            return false;

        if (references.Digimon == null)
            return false;

        return true;
    }
}
