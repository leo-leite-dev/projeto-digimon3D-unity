using UnityEngine;
using UnityEngine.AI;

public static class NavMeshUtility
{
    /// <summary>
    /// Retorna uma posição válida na NavMesh próxima da posição fornecida.
    /// Se não encontrar dentro do raio, retorna a posição original.
    /// </summary>
    public static Vector3 GetValidPosition(
        Vector3 position,
        float maxDistance = 5f,
        int areaMask = NavMesh.AllAreas
    )
    {
        if (NavMesh.SamplePosition(position, out NavMeshHit hit, maxDistance, areaMask))
            return hit.position;

        return position;
    }

    /// <summary>
    /// Tenta encontrar uma posição válida na NavMesh.
    /// Retorna true se encontrou e preenche result.
    /// </summary>
    public static bool TryGetValidPosition(
        Vector3 position,
        out Vector3 result,
        float maxDistance = 5f,
        int areaMask = NavMesh.AllAreas
    )
    {
        if (NavMesh.SamplePosition(position, out NavMeshHit hit, maxDistance, areaMask))
        {
            result = hit.position;
            return true;
        }

        result = position;
        return false;
    }

    /// <summary>
    /// Ajusta imediatamente um NavMeshAgent para uma posição válida usando Warp.
    /// Útil após spawn.
    /// </summary>
    public static void WarpAgentToValidPosition(
        NavMeshAgent agent,
        Vector3 desiredPosition,
        float maxDistance = 5f,
        int areaMask = NavMesh.AllAreas
    )
    {
        if (agent == null)
            return;

        Vector3 pos = GetValidPosition(desiredPosition, maxDistance, areaMask);
        agent.Warp(pos);
    }
}
