using UnityEngine;

public class PooledProjectile : MonoBehaviour
{
    public ProjectilePool pool;

    private GameObject prefab;

    public void SetPrefab(GameObject prefabRef)
    {
        prefab = prefabRef;
    }

    public void ReturnToPool()
    {
        if (pool != null && prefab != null)
            pool.Return(prefab, gameObject);
        else
            Destroy(gameObject);
    }
}
