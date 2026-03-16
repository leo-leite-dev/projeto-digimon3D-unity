using UnityEngine;

public class TargetIndicatorPresenter : MonoBehaviour
{
    [SerializeField]
    private TargetSystem targetSystem;

    void Awake()
    {
        if (targetSystem == null)
            TryGetComponent(out targetSystem);
    }

    void OnEnable()
    {
        if (targetSystem != null)
            targetSystem.OnTargetChanged += HandleTargetChanged;
    }

    void OnDisable()
    {
        if (targetSystem != null)
            targetSystem.OnTargetChanged -= HandleTargetChanged;
    }

    void HandleTargetChanged(GameObject oldTarget, GameObject newTarget)
    {
        SetIndicatorState(oldTarget, false);
        SetIndicatorState(newTarget, true);
    }

    void SetIndicatorState(GameObject target, bool active)
    {
        if (target == null)
            return;

        TargetIndicator indicator = target.GetComponentInChildren<TargetIndicator>(true);

        if (indicator != null)
            indicator.SetActive(active);
    }
}
