using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    public GameObject indicator;

    public void SetActive(bool active)
    {
        if (indicator != null)
            indicator.SetActive(active);
    }
}
