using System;
using UnityEngine;

public class DigimonHitReceiver : MonoBehaviour
{
    private Digimon digimon;
    private DigimonAnimator digimonAnimator;

    private bool initialized;

    public Action OnHit;

    public void Inject(Digimon digimon, DigimonAnimator animator)
    {
        if (initialized)
        {
            Debug.LogWarning("DigimonHitReceiver já foi injetado", this);
            return;
        }

        this.digimon = digimon ?? throw new System.ArgumentNullException(nameof(digimon));
        digimonAnimator = animator;

        initialized = true;
    }

    public void ReceiveHit(HitContext context)
    {
        if (!initialized)
        {
            Debug.LogError("DigimonHitReceiver não foi inicializado", this);
            return;
        }

        ApplyDamage(context);
        digimonAnimator?.PlayDamage();

        OnHit?.Invoke();
    }

    private void ApplyDamage(HitContext context)
    {
        int finalDamage = Mathf.Max(0, context.FinalDamage);

        digimon.stats.Hp -= finalDamage;

        if (digimon.stats.Hp < 0)
            digimon.stats.Hp = 0;
    }
}
