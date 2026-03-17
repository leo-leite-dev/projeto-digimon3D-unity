using UnityEngine;

public class DigimonRuntimeBootstrap : MonoBehaviour
{
    [SerializeField]
    private DigimonReferences references;

    [SerializeField]
    private DigimonAttackSceneBinder attackSceneBinder;

    private void Awake()
    {
        if (references == null)
            references = GetComponent<DigimonReferences>();

        if (attackSceneBinder == null)
            attackSceneBinder = GetComponent<DigimonAttackSceneBinder>();

        if (references == null)
        {
            Debug.LogError("[DigimonRuntimeBootstrap] DigimonReferences não encontrado.", this);
            return;
        }

        DigimonCoreReferencesResolver.Refresh(references);
    }
}
