using System;
using UnityEngine;

public class DigimonHitReceiver : MonoBehaviour
{
    private Digimon digimon;
    private DigimonAnimator digimonAnimator;

    private bool initialized;

    public event Action<Transform> OnHit;

    public void Initialize(Digimon digimon, DigimonAnimator animator)
    {
        if (initialized)
        {
            Debug.LogWarning("DigimonHitReceiver já foi inicializado", this);
            return;
        }

        this.digimon = digimon ?? throw new ArgumentNullException(nameof(digimon));
        this.digimonAnimator = animator;

        initialized = true;
    }

    public void ReceiveHit(HitContext context)
    {
        if (!initialized)
        {
            Debug.LogError("❌ DigimonHitReceiver não inicializado", this);
            return;
        }

        ApplyDamage(context);
        PlayHitFeedback();
        NotifyHit(context);
    }

    private void ApplyDamage(HitContext context)
    {
        int damage = Mathf.Max(0, context.FinalDamage);

        digimon.stats.Hp -= damage;

        if (digimon.stats.Hp < 0)
            digimon.stats.Hp = 0;
    }

    private void PlayHitFeedback()
    {
        digimonAnimator?.PlayDamage();
    }

    private void NotifyHit(HitContext context)
    {
        var attacker = context.Attacker;

        if (attacker == null)
            return;

        OnHit?.Invoke(attacker.transform);
    }
}
