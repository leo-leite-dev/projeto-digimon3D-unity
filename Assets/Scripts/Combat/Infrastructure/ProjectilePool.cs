using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    private readonly Dictionary<GameObject, Queue<GameObject>> pools = new();

    public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null)
        {
            Debug.LogError("❌ ProjectilePool.Get chamado com prefab nulo", this);
            return null;
        }

        if (!pools.TryGetValue(prefab, out Queue<GameObject> pool))
        {
            pool = new Queue<GameObject>();
            pools[prefab] = pool;
        }

        GameObject obj;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();

            if (obj == null)
                return Get(prefab, position, rotation);

            obj.transform.SetPositionAndRotation(position, rotation);
            obj.SetActive(true);
        }
        else
        {
            obj = CreateNew(prefab, position, rotation);
        }

        return obj;
    }

    public void Return(GameObject prefab, GameObject obj)
    {
        if (prefab == null || obj == null)
            return;

        obj.SetActive(false);

        if (!pools.TryGetValue(prefab, out Queue<GameObject> pool))
        {
            pool = new Queue<GameObject>();
            pools[prefab] = pool;
        }

        pool.Enqueue(obj);
    }

    private GameObject CreateNew(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject obj = Instantiate(prefab, position, rotation);

        PooledProjectile pooled = obj.GetComponent<PooledProjectile>();

        if (pooled == null)
            pooled = obj.AddComponent<PooledProjectile>();

        pooled.Setup(this, prefab);

        return obj;
    }
}
