using UnityEngine;
using UnityEngine.AI;

public class DigimonCoreReferences : MonoBehaviour
{
    [Header("=== Core ===")]
    [SerializeField]
    private Digimon digimon;

    [SerializeField]
    private DigimonMovement movement;

    [SerializeField]
    private NavMeshAgent navMeshAgent;

    [Header("=== Visual Root ===")]
    [SerializeField]
    private Transform modelRoot;

    public Digimon Digimon => digimon;
    public DigimonMovement Movement => movement;
    public NavMeshAgent NavMeshAgent => navMeshAgent;
    public Transform ModelRoot => modelRoot;

    public bool IsValid()
    {
        bool valid = true;

        void Check(Object obj, string name)
        {
            if (obj == null)
            {
                Debug.LogError($"❌ CoreReferences: {name} missing", this);
                valid = false;
            }
        }

        Check(digimon, nameof(digimon));
        Check(movement, nameof(movement));
        Check(navMeshAgent, nameof(navMeshAgent));
        Check(modelRoot, nameof(modelRoot));

        return valid;
    }
}
