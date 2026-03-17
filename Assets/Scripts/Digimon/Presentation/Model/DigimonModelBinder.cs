using UnityEngine;

public class DigimonModelBinder : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Transform firePoint;

    public Animator Animator => animator;
    public Transform FirePoint => firePoint;

#if UNITY_EDITOR
    void OnValidate()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (firePoint == null)
        {
            var fp = GetComponentInChildren<FirePoint>();
            if (fp != null)
                firePoint = fp.transform;
        }
    }
#endif
}
