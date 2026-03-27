using UnityEngine;
using UnityEngine.AI;

public static class SpawnPositionResolver
{
    public static bool TryGetValidPosition(
        WanderArea area,
        out Vector3 position,
        float maxDistance = 10f
    )
    {
        position = Vector3.zero;

        if (area == null)
            return false;

        Vector3 random = area.GetRandomPosition();

        if (NavMesh.SamplePosition(random, out NavMeshHit hit, maxDistance, NavMesh.AllAreas))
        {
            position = hit.position;
            return true;
        }

        return false;
    }
}
