using System.Collections.Generic;
using UnityEngine;

public class TargetSystem : MonoBehaviour
{
    public float targetRange = 15f;
    public Transform playerCamera;

    private List<GameObject> enemiesInRange = new List<GameObject>();
    private int currentIndex = -1;

    public GameObject currentTarget;

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
                float distA = Vector3.Distance(playerCamera.position, a.transform.position);
                float distB = Vector3.Distance(playerCamera.position, b.transform.position);

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
        if (currentTarget != null)
        {
            TargetIndicator oldIndicator = currentTarget.GetComponentInParent<TargetIndicator>();

            if (oldIndicator != null)
                oldIndicator.SetActive(false);
        }

        currentTarget = target;

        if (currentTarget != null)
        {
            TargetIndicator newIndicator = currentTarget.GetComponentInParent<TargetIndicator>();

            if (newIndicator != null)
                newIndicator.SetActive(true);
        }
    }

    void ClearTarget()
    {
        if (currentTarget != null)
        {
            TargetIndicator indicator = currentTarget.GetComponentInParent<TargetIndicator>();

            if (indicator != null)
                indicator.SetActive(false);
        }

        currentTarget = null;
    }

    void ValidateTargetDistance()
    {
        if (currentTarget == null)
            return;

        float sqrDistance = (currentTarget.transform.position - transform.position).sqrMagnitude;

        if (sqrDistance > targetRange * targetRange)
            ClearTarget();
    }
}
