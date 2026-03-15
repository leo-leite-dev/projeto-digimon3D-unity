using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance;

    private Dictionary<GameObject, Queue<GameObject>> pools = new();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!pools.ContainsKey(prefab))
            pools[prefab] = new Queue<GameObject>();

        Queue<GameObject> pool = pools[prefab];

        GameObject obj;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
            obj.transform.SetPositionAndRotation(position, rotation);
            obj.SetActive(true);
        }
        else
        {
            obj = Instantiate(prefab, position, rotation);

            PooledProjectile pooled = obj.GetComponent<PooledProjectile>();

            if (pooled == null)
                pooled = obj.AddComponent<PooledProjectile>();

            pooled.pool = this;
            pooled.SetPrefab(prefab);
        }

        return obj;
    }

    public void Return(GameObject prefab, GameObject obj)
    {
        if (obj == null)
            return;

        obj.SetActive(false);

        if (!pools.ContainsKey(prefab))
            pools[prefab] = new Queue<GameObject>();

        pools[prefab].Enqueue(obj);
    }
}
