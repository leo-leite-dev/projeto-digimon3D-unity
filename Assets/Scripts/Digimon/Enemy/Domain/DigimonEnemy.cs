using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyWander))]
public class DigimonEnemy : Digimon
{
    [Header("Target")]
    [SerializeField]
    private Transform targetPoint;

    public Transform TargetPoint => targetPoint;

    protected override void Awake()
    {
        base.Awake();
        ResolveTargetPoint();
    }

    private void ResolveTargetPoint()
    {
        if (targetPoint != null)
            return;

        Transform found = transform.Find("Target");

        if (found != null)
            targetPoint = found;
    }
}
