using UnityEngine;

public class SkillEffectSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform firePoint;

    public void RefreshFirePoint()
    {
        FirePoint marker = GetComponentInChildren<FirePoint>(true);

        if (marker != null)
            firePoint = marker.transform;
        else
            firePoint = null;
    }

    private Transform GetFirePoint()
    {
        if (firePoint == null)
            RefreshFirePoint();

        return firePoint;
    }

    private Transform ResolveEffectTarget(Transform originalTarget)
    {
        if (originalTarget == null)
            return null;

        DigimonEnemy enemy = originalTarget.GetComponentInParent<DigimonEnemy>();

        if (enemy != null && enemy.TargetPoint != null)
            return enemy.TargetPoint;

        return originalTarget;
    }

    public SkillEffect Spawn(
        DigimonSkill skill,
        Transform target,
        Digimon attacker,
        DigimonAttack ownerAttack
    )
    {
        if (skill == null || target == null || attacker == null || ownerAttack == null)
            return null;

        if (skill.projectilePrefab == null)
            return null;

        Transform origin = GetFirePoint();

        if (origin == null)
            return null;

        GameObject effectObject = ProjectilePool.Instance.Get(
            skill.projectilePrefab,
            origin.position,
            origin.rotation
        );

        if (effectObject == null)
            return null;

        SkillEffect[] effects = effectObject.GetComponentsInChildren<SkillEffect>(true);

        for (int i = 0; i < effects.Length; i++)
        {
            Debug.Log(
                $"[Spawn] effect[{i}] obj={effects[i].gameObject.name} id={effects[i].gameObject.GetInstanceID()}",
                effects[i].gameObject
            );
        }

        SkillEffect effect = effectObject.GetComponent<SkillEffect>();

        if (effect == null)
            return null;

        Transform resolvedTarget = ResolveEffectTarget(target);

        effect.Setup(resolvedTarget, skill, attacker, ownerAttack);

        return effect;
    }
}
