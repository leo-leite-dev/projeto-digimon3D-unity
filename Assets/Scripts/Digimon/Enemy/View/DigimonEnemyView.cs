using UnityEngine;

public class DigimonEnemyView : MonoBehaviour
{
    [SerializeField]
    private Transform modelRoot;

    private GameObject currentModel;
    private Animator animator;

    public void SpawnModel(DigimonData data)
    {
        if (data == null || data.modelPrefab == null || modelRoot == null)
            return;

        if (currentModel != null)
            Destroy(currentModel);

        currentModel = Instantiate(data.modelPrefab, modelRoot);
        currentModel.transform.localPosition = Vector3.zero;
        currentModel.transform.localRotation = Quaternion.identity;

        var binder = currentModel.GetComponent<DigimonModelBinder>();

        if (binder == null)
        {
            Debug.LogError("❌ ModelPrefab sem DigimonModelBinder", currentModel);
            return;
        }
        animator = binder.Animator;
    }

    public Animator GetAnimator()
    {
        if (animator == null)
        {
            Debug.LogError("❌ Animator não inicializado. SpawnModel falhou.", this);
            return null;
        }

        return animator;
    }
}
