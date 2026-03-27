using UnityEngine;

public class PooledProjectile : MonoBehaviour
{
    private ProjectilePool pool;
    private GameObject prefab;

    public void Setup(ProjectilePool pool, GameObject prefab)
    {
        this.pool = pool;
        this.prefab = prefab;
    }

    public void ReturnToPool()
    {
        if (pool == null || prefab == null)
        {
            Debug.LogWarning("⚠️ PooledProjectile não configurado corretamente", this);
            Destroy(gameObject);
            return;
        }

        pool.Return(prefab, gameObject);
    }
}
