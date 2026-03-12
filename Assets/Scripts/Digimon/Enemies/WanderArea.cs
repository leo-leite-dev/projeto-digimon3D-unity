using UnityEngine;
using UnityEngine.AI;

public class WanderArea : MonoBehaviour
{
    public float width = 10f;
    public float depth = 10f;

    public Vector3 GetRandomPosition()
    {
        for (int i = 0; i < 10; i++)
        {
            float randomX = Random.Range(-width * 0.5f, width * 0.5f);
            float randomZ = Random.Range(-depth * 0.5f, depth * 0.5f);

            Vector3 candidate = transform.position + new Vector3(randomX, 0f, randomZ);

            NavMeshHit hit;

            if (NavMesh.SamplePosition(candidate, out hit, 5f, NavMesh.AllAreas))
                return hit.position;
        }

        return transform.position;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, 0.1f, depth));
    }
}
