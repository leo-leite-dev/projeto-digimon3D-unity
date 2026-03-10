using UnityEngine;

public class WanderArea : MonoBehaviour
{
    public float width = 10f;
    public float depth = 10f;

    public Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(-width / 2, width / 2);
        float randomZ = Random.Range(-depth / 2, depth / 2);

        Vector3 randomPosition = transform.position + new Vector3(randomX, 0, randomZ);

        return randomPosition;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, 0.1f, depth));
    }
}
