using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyWander))]
public class DigimonEnemy : Digimon
{
    [Header("Model")]
    public Transform modelRoot;

    private GameObject currentModel;

    public override void Initialize(DigimonData data)
    {
        base.Initialize(data);

        SpawnModel();
    }

    void SpawnModel()
    {
        if (currentModel != null)
            Destroy(currentModel);

        if (data == null || data.modelPrefab == null || modelRoot == null)
            return;

        currentModel = Instantiate(data.modelPrefab, modelRoot);
        currentModel.transform.localPosition = Vector3.zero;
        currentModel.transform.localRotation = Quaternion.identity;

        Animator animator = currentModel.GetComponentInChildren<Animator>();

        EnemyWander wander = GetComponent<EnemyWander>();

        if (wander != null)
            wander.animator = animator;
    }
}
