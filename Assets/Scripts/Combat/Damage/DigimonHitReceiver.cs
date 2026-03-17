using UnityEngine;

public class DigimonHitReceiver : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Digimon digimon;

    [SerializeField]
    private DigimonAnimator digimonAnimator;

    private void Reset()
    {
        if (digimon == null)
            digimon = GetComponent<Digimon>();

        if (digimon == null)
            digimon = GetComponentInParent<Digimon>();

        if (digimonAnimator == null)
            digimonAnimator = GetComponent<DigimonAnimator>();

        if (digimonAnimator == null)
            digimonAnimator = GetComponentInParent<DigimonAnimator>();
    }

    private void OnValidate()
    {
        if (digimon == null)
            digimon = GetComponent<Digimon>();

        if (digimon == null)
            digimon = GetComponentInParent<Digimon>();

        if (digimonAnimator == null)
            digimonAnimator = GetComponent<DigimonAnimator>();

        if (digimonAnimator == null)
            digimonAnimator = GetComponentInParent<DigimonAnimator>();
    }

    public void ReceiveHit(HitContext context)
    {
        if (digimon == null)
            return;

        ApplyDamage(context);
        digimonAnimator?.PlayDamage();
    }

    private void ApplyDamage(HitContext context)
    {
        int finalDamage = Mathf.Max(0, context.FinalDamage);

        digimon.stats.Hp -= finalDamage;

        if (digimon.stats.Hp < 0)
            digimon.stats.Hp = 0;
    }
}
