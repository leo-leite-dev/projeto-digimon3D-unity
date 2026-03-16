using System;
using System.Collections.Generic;
using UnityEngine;

public class TargetSystem : ValidatedMonoBehaviour
{
    [Header("Targeting")]
    [SerializeField]
    private float targetRange = 15f;

    [SerializeField]
    private Transform player;

    private readonly List<GameObject> enemiesInRange = new();
    private int currentIndex = -1;

    public GameObject CurrentTarget { get; private set; }

    public event Action<GameObject, GameObject> OnTargetChanged;

    protected override void Validate()
    {
        if (player == null)
        {
            player = transform;
            Debug.LogWarning("[TargetSystem] Player Transform not assigned. Using self.", this);
        }
    }

    private void Update()
    {
        ValidateCurrentTarget();
    }

    public GameObject GetCurrentTarget()
    {
        if (!IsTargetValid(CurrentTarget))
        {
            ClearTarget();
            return null;
        }

        return CurrentTarget;
    }

    public bool IsTargetValid(GameObject target)
    {
        if (target == null)
            return false;

        float sqrDistance = (target.transform.position - player.position).sqrMagnitude;
        return sqrDistance <= targetRange * targetRange;
    }

    public void CycleTarget()
    {
        UpdateEnemyList();

        if (enemiesInRange.Count == 0)
        {
            ClearTarget();
            return;
        }

        currentIndex++;

        if (currentIndex >= enemiesInRange.Count)
            currentIndex = 0;

        SetTarget(enemiesInRange[currentIndex]);
    }

    public bool TrySetTarget(GameObject target)
    {
        if (!IsTargetValid(target))
            return false;

        SetTarget(target);
        return true;
    }

    private void SetTarget(GameObject target)
    {
        if (CurrentTarget == target)
            return;

        GameObject previousTarget = CurrentTarget;
        CurrentTarget = target;

        currentIndex = enemiesInRange.IndexOf(target);

        OnTargetChanged?.Invoke(previousTarget, CurrentTarget);
    }

    public void ClearTarget()
    {
        if (CurrentTarget == null && currentIndex == -1)
            return;

        GameObject previousTarget = CurrentTarget;

        CurrentTarget = null;
        currentIndex = -1;

        OnTargetChanged?.Invoke(previousTarget, null);
    }

    private void ValidateCurrentTarget()
    {
        if (CurrentTarget == null)
            return;

        if (!IsTargetValid(CurrentTarget))
            ClearTarget();
    }

    private void UpdateEnemyList()
    {
        enemiesInRange.Clear();

        Collider[] hits = Physics.OverlapSphere(player.position, targetRange);
        HashSet<GameObject> uniqueEnemies = new();

        foreach (Collider hit in hits)
        {
            if (!hit.CompareTag("Enemy"))
                continue;

            GameObject enemyRoot = hit.transform.root.gameObject;
            uniqueEnemies.Add(enemyRoot);
        }

        enemiesInRange.AddRange(uniqueEnemies);

        enemiesInRange.Sort(
            (a, b) =>
            {
                float distA = (player.position - a.transform.position).sqrMagnitude;
                float distB = (player.position - b.transform.position).sqrMagnitude;
                return distA.CompareTo(distB);
            }
        );
    }

    private void OnDrawGizmosSelected()
    {
        Transform center = player != null ? player : transform;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(center.position, targetRange);
    }
}
