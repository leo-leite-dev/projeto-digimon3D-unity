using UnityEngine;

public class ChunkSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Transform referencePoint;  

    [Header("Settings")]
    [SerializeField]
    private float maxDistance = 300f;

    void Start()
    {
        if (referencePoint == null)
        {
            Debug.LogError("[ChunkSystem] ReferencePoint não definido.", this);
            return;
        }

        DisableFarObjects();
    }

    void DisableFarObjects()
    {
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Chunkable");

        Vector3 refPos = referencePoint.position;
        float sqrMaxDistance = maxDistance * maxDistance;

        foreach (var obj in allObjects)
        {
            if (obj == null)
                continue;

            float sqrDistance = (obj.transform.position - refPos).sqrMagnitude;

            if (sqrDistance > sqrMaxDistance)
            {
                obj.SetActive(false);
            }
        }
    }
}
