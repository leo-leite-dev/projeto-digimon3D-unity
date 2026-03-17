using UnityEngine;

public class DigimonAnimatorEventBinder : ValidatedMonoBehaviour
{
    [SerializeField]
    private DigimonReferences references;

    private DigimonAttack digimonAttack;

    protected override void Validate()
    {
        references = GetRequired(references);
    }

    public void Configure(DigimonReferences references, DigimonAttack digimonAttack)
    {
        Unsubscribe();

        this.references = references;
        this.digimonAttack = digimonAttack;

        Subscribe();
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        if (references == null || references.DigimonAnimator == null || digimonAttack == null)
            return;

        references.DigimonAnimator.OnSpawnEffect -= digimonAttack.OnSpawnEffect;
        references.DigimonAnimator.OnSwitchEffectToMoveToTarget -=
            digimonAttack.OnSwitchEffectToMoveToTarget;
        references.DigimonAnimator.OnApplyHit -= digimonAttack.OnApplyHit;
        references.DigimonAnimator.OnFinishSkill -= digimonAttack.OnFinishSkill;

        references.DigimonAnimator.OnSpawnEffect += digimonAttack.OnSpawnEffect;
        references.DigimonAnimator.OnSwitchEffectToMoveToTarget +=
            digimonAttack.OnSwitchEffectToMoveToTarget;
        references.DigimonAnimator.OnApplyHit += digimonAttack.OnApplyHit;
        references.DigimonAnimator.OnFinishSkill += digimonAttack.OnFinishSkill;
    }

    private void Unsubscribe()
    {
        if (references == null || references.DigimonAnimator == null || digimonAttack == null)
            return;

        references.DigimonAnimator.OnSpawnEffect -= digimonAttack.OnSpawnEffect;
        references.DigimonAnimator.OnSwitchEffectToMoveToTarget -=
            digimonAttack.OnSwitchEffectToMoveToTarget;
        references.DigimonAnimator.OnApplyHit -= digimonAttack.OnApplyHit;
        references.DigimonAnimator.OnFinishSkill -= digimonAttack.OnFinishSkill;
    }
}
