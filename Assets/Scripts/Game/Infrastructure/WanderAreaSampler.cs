using UnityEngine;

public class WanderAreaSampler : MonoBehaviour, IWanderPositionSampler
{
    [Header("Area")]
    [SerializeField]
    private Transform center;

    [SerializeField]
    private float radius = 10f;

    public Vector3 Sample()
    {
        if (center == null)
        {
            Debug.LogError("❌ WanderAreaSampler: Center não definido");
            return transform.position;
        }

        Vector2 random = Random.insideUnitCircle * radius;

        Vector3 position = new Vector3(
            center.position.x + random.x,
            center.position.y,
            center.position.z + random.y
        );

        return position;
    }
}
