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

    protected override void Validate()
    {
        if (player == null)
        {
            player = transform;
            Debug.LogWarning("[TargetSystem] Player Transform not assigned. Using self.", this);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            CycleTarget();

        if (Input.GetMouseButtonDown(0))
            ClickTarget();

        ValidateTargetDistance();
    }

    void CycleTarget()
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

    void UpdateEnemyList()
    {
        enemiesInRange.Clear();

        Collider[] hits = Physics.OverlapSphere(transform.position, targetRange);

        foreach (Collider hit in hits)
        {
            if (!hit.CompareTag("Enemy"))
                continue;

            Vector3 direction = (hit.transform.position - transform.position).normalized;

            float dot = Vector3.Dot(transform.forward, direction);

            if (dot < 0)
                continue;

            GameObject enemyRoot = hit.transform.root.gameObject;

            if (!enemiesInRange.Contains(enemyRoot))
                enemiesInRange.Add(enemyRoot);
        }

        enemiesInRange.Sort(
            (a, b) =>
            {
                float distA = (player.position - a.transform.position).sqrMagnitude;
                float distB = (player.position - b.transform.position).sqrMagnitude;

                return distA.CompareTo(distB);
            }
        );
    }

    void ClickTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (!hit.collider.CompareTag("Enemy"))
                return;

            GameObject enemy = hit.collider.transform.root.gameObject;

            float sqrDistance = (enemy.transform.position - transform.position).sqrMagnitude;

            if (sqrDistance <= targetRange * targetRange)
                SetTarget(enemy);
        }
    }

    void SetTarget(GameObject target)
    {
        if (CurrentTarget != null)
        {
            TargetIndicator oldIndicator = CurrentTarget.GetComponentInParent<TargetIndicator>();

            if (oldIndicator != null)
                oldIndicator.SetActive(false);
        }

        CurrentTarget = target;

        if (CurrentTarget != null)
        {
            TargetIndicator newIndicator = CurrentTarget.GetComponentInParent<TargetIndicator>();

            if (newIndicator != null)
                newIndicator.SetActive(true);
        }
    }

    public void ClearTarget()
    {
        if (CurrentTarget != null)
        {
            TargetIndicator indicator = CurrentTarget.GetComponentInParent<TargetIndicator>();

            if (indicator != null)
                indicator.SetActive(false);
        }

        CurrentTarget = null;
    }

    void ValidateTargetDistance()
    {
        if (CurrentTarget == null)
            return;

        float sqrDistance = (CurrentTarget.transform.position - transform.position).sqrMagnitude;

        if (sqrDistance > targetRange * targetRange)
            ClearTarget();
    }
}
