using UnityEngine;

public class DigimonReferenceProvider : ValidatedMonoBehaviour
{
    [SerializeField]
    private DigimonReferences references;

    public DigimonReferences References => references;

    protected override void Validate()
    {
        RefreshReferences();
    }

    protected override void Awake()
    {
        base.Awake();
        RefreshReferences();
    }

    [ContextMenu("Refresh References")]
    public void RefreshReferences()
    {
        if (references == null)
            references = GetComponent<DigimonReferences>();

        if (references == null)
            references = GetComponentInParent<DigimonReferences>();
    }
}
