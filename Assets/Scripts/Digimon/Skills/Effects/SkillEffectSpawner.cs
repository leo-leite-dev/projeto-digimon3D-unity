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
        {
            Debug.LogWarning(
                "[SkillEffectSpawner] Spawn abortado: skill/target/attacker/ownerAttack null."
            );
            return null;
        }

        if (skill.projectilePrefab == null)
        {
            Debug.LogWarning("[SkillEffectSpawner] Spawn abortado: projectilePrefab null.");
            return null;
        }

        Transform origin = GetFirePoint();

        if (origin == null)
        {
            Debug.LogWarning("[SkillEffectSpawner] Spawn abortado: firePoint null.");
            return null;
        }

        GameObject effectObject = ProjectilePool.Instance.Get(
            skill.projectilePrefab,
            origin.position,
            origin.rotation
        );

        if (effectObject == null)
        {
            Debug.LogWarning("[SkillEffectSpawner] Spawn abortado: pool retornou null.");
            return null;
        }

        SkillEffect effect = effectObject.GetComponentInChildren<SkillEffect>(true);

        if (effect == null)
        {
            Debug.LogWarning(
                $"[SkillEffectSpawner] Spawn abortado: nenhum SkillEffect encontrado em {effectObject.name}.",
                effectObject
            );
            return null;
        }

        Transform resolvedTarget = ResolveEffectTarget(target);

        effect.Setup(resolvedTarget, skill, attacker, ownerAttack);

        Debug.Log($"[SkillEffectSpawner] Efeito spawnado: {effect.name}", effect.gameObject);

        return effect;
    }
}
