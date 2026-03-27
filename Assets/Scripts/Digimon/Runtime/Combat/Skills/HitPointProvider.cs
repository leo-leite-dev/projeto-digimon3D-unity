using UnityEngine;

public class HitPointProvider : MonoBehaviour
{
    [SerializeField]
    private Transform hitPoint;

    public Transform GetHitPoint()
    {
        return hitPoint != null ? hitPoint : transform;
    }
}
