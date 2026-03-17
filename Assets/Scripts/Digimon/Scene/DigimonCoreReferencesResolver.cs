using UnityEngine;
using UnityEngine.AI;

public static class DigimonCoreReferencesResolver
{
    public static void Refresh(DigimonReferences references)
    {
        GameObject gameObject = references.gameObject;

        Digimon digimon = references.Digimon ?? gameObject.GetComponent<Digimon>();
        DigimonMovement movement =
            references.Movement ?? gameObject.GetComponent<DigimonMovement>();
        DigimonAttack attack = references.Attack ?? gameObject.GetComponent<DigimonAttack>();
        DigimonFollow follow = references.Follow ?? gameObject.GetComponent<DigimonFollow>();
        NavMeshAgent navMeshAgent =
            references.NavMeshAgent ?? gameObject.GetComponent<NavMeshAgent>();
        DigimonAnimator digimonAnimator =
            references.DigimonAnimator ?? gameObject.GetComponent<DigimonAnimator>();
        SkillDamageResolver damageResolver =
            references.DamageResolver ?? gameObject.GetComponent<SkillDamageResolver>();
        DigimonHitReceiver hitReceiver =
            references.HitReceiver ?? gameObject.GetComponent<DigimonHitReceiver>();

        references.SetCoreReferences(
            digimon,
            movement,
            attack,
            follow,
            navMeshAgent,
            digimonAnimator,
            damageResolver,
            hitReceiver
        );
    }
}
