using UnityEngine;

public class SkillHitExecutor : ValidatedMonoBehaviour
{
    [SerializeField]
    private DigimonReferences references;

    public DigimonReferences References => references;

    protected override void Validate()
    {
        RefreshReferences();
    }

    protected override void Awake()
    {
        base.Awake();
        RefreshReferences();
    }

    [ContextMenu("Refresh References")]
    public void RefreshReferences()
    {
        if (references == null)
            references = GetComponent<DigimonReferences>();

        if (references == null)
            references = GetComponentInParent<DigimonReferences>();
    }

    public SkillEffect TriggerEffectHitFlow(
        DigimonSkill currentSkill,
        GameObject currentTarget,
        bool hasSpawnedEffectForCurrentSkill,
        DigimonAttack attackOwner
    )
    {
        if (currentSkill == null || currentTarget == null)
            return null;

        if (currentSkill.hitTriggerType != SkillHitTriggerType.Effect)
            return null;

        if (references == null || references.EffectSpawner == null)
            return null;

        if (hasSpawnedEffectForCurrentSkill)
            return null;

        return references.EffectSpawner.Spawn(
            currentSkill,
            currentTarget.transform,
            references.Digimon,
            attackOwner
        );
    }

    public void TriggerDirectHitFlow(DigimonSkill currentSkill, GameObject currentTarget)
    {
        if (currentSkill == null || currentTarget == null)
            return;

        if (currentSkill.hitTriggerType != SkillHitTriggerType.Direct)
            return;

        if (references == null || references.DamageResolver == null)
            return;

        references.DamageResolver.Apply(currentSkill, currentTarget.transform, references.Digimon);
    }

    public void SwitchCurrentEffectToMoveToTarget(SkillEffect currentEffect)
    {
        if (currentEffect == null)
            return;

        currentEffect.SwitchToMoveToTarget();
    }
}
