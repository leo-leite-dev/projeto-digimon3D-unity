using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyWander))]
public class DigimonEnemy : Digimon
{
    private Transform targetPoint;

    public Transform TargetPoint => targetPoint;

    public void InjectTarget(Transform targetPoint)
    {
        if (targetPoint == null)
        {
            Debug.LogError("❌ DigimonEnemy → TargetPoint null", this);
            return;
        }

        if (this.targetPoint != null)
        {
            Debug.LogWarning("⚠️ Target já foi definido", this);
            return;
        }

        this.targetPoint = targetPoint;
    }
}
