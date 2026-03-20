using UnityEngine;

public class SkillEffectSpawner
{
    private readonly ProjectilePool projectilePool;

    public SkillEffectSpawner(ProjectilePool projectilePool)
    {
        this.projectilePool = projectilePool;
    }

    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab não definido.");
            return null;
        }

        var obj = projectilePool.Get(prefab, position, rotation);

        if (obj == null)
        {
            Debug.LogError("Falha ao obter objeto do pool.");
            return null;
        }

        return obj;
    }
}
