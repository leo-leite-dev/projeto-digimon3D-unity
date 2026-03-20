using UnityEngine;
using UnityEngine.AI;

public class DigimonSpawnService
{
    private readonly GameObject digimonFollowerPrefab;

    public DigimonSpawnService(GameObject digimonFollowerPrefab)
    {
        this.digimonFollowerPrefab = digimonFollowerPrefab;
    }

    public DigimonFollow SpawnFollow(
        DigimonData data,
        Vector3 position,
        Quaternion rotation,
        Transform owner,
        Transform followPoint
    )
    {
        var digimonGO = Object.Instantiate(digimonFollowerPrefab, position, rotation);
        digimonGO.name = data.digimonName;

        var follow = digimonGO.GetComponent<DigimonFollow>();

        if (follow == null)
        {
            Debug.LogError("Prefab sem DigimonFollow.", digimonGO);
            Object.Destroy(digimonGO);
            return null;
        }

        follow.Setup(data);

        DigimonFollowComposer.Compose(digimonGO);

        var agent = digimonGO.GetComponent<NavMeshAgent>();
        NavMeshUtility.WarpAgentToValidPosition(agent, position);

        follow.Initialize(owner, followPoint);

        return follow;
    }
}
