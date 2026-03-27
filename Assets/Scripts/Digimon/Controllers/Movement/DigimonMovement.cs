using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class DigimonMovement : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField]
    private float rotationSpeed = 10f;

    [Header("Thresholds")]
    [SerializeField]
    private float movementEpsilon = 0.01f;

    private NavMeshAgent agent;
    private int movementLockCount;

    public bool IsMovementLocked => movementLockCount > 0;

    public bool IsAgentReady => agent != null && agent.enabled && agent.isOnNavMesh;

    public bool IsStopped => IsAgentReady && agent.isStopped;
    public bool HasPath => IsAgentReady && agent.hasPath;
    public bool PathPending => IsAgentReady && agent.pathPending;
    public Vector3 Velocity => IsAgentReady ? agent.velocity : Vector3.zero;
    public float RemainingDistance => IsAgentReady ? agent.remainingDistance : Mathf.Infinity;

    public bool IsMoving
    {
        get
        {
            if (!IsAgentReady)
                return false;

            if (agent.isStopped)
                return false;

            return agent.velocity.sqrMagnitude > movementEpsilon;
        }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }

    private void Update()
    {
        HandleRotation();
    }

    public bool SetDestination(Vector3 destination)
    {
        if (!CanMove())
            return false;

        agent.isStopped = false;
        return agent.SetDestination(destination);
    }

    public void StopMovement()
    {
        if (!IsAgentReady)
        {
            Debug.LogWarning("❌ StopMovement → Agent não pronto");
            return;
        }

        agent.isStopped = true;
        agent.ResetPath();
    }

    public void PauseMovement()
    {
        if (!IsAgentReady)
            return;

        agent.isStopped = true;
    }

    public void ResumeMovement()
    {
        if (!IsAgentReady)
            return;

        if (IsMovementLocked)
            return;

        if (!agent.hasPath)
            return;

        agent.isStopped = false;
    }

    public bool WarpTo(Vector3 position)
    {
        if (!IsAgentReady)
            return false;

        bool warped = agent.Warp(position);

        if (warped)
        {
            agent.isStopped = false;
            agent.ResetPath();
        }

        return warped;
    }

    public void AddMovementLock()
    {
        movementLockCount++;

        if (!IsAgentReady)
            return;

        agent.isStopped = true;
        agent.ResetPath();
    }

    public void RemoveMovementLock()
    {
        movementLockCount = Mathf.Max(0, movementLockCount - 1);

        if (!IsAgentReady)
            return;

        if (IsMovementLocked)
            return;

        agent.isStopped = false;
    }

    public void ClearAllMovementLocks()
    {
        movementLockCount = 0;

        if (!IsAgentReady)
            return;

        agent.isStopped = false;
    }

    public bool HasReachedDestination()
    {
        if (!IsAgentReady)
            return false;

        if (agent.pathPending)
            return false;

        if (!agent.hasPath)
            return false;

        if (agent.pathStatus != NavMeshPathStatus.PathComplete)
            return false;

        if (agent.remainingDistance > agent.stoppingDistance)
            return false;

        return agent.velocity.sqrMagnitude <= movementEpsilon;
    }

    public bool IsIdle()
    {
        if (!IsAgentReady)
            return true;

        if (agent.pathPending)
            return false;

        if (agent.hasPath)
            return false;

        return agent.velocity.sqrMagnitude <= movementEpsilon;
    }

    public bool IsCloseTo(Vector3 position, float tolerance = 0.1f)
    {
        Vector3 current = transform.position;
        Vector3 target = position;

        current.y = 0f;
        target.y = 0f;

        return (current - target).sqrMagnitude <= tolerance * tolerance;
    }

    public void RotateToTarget(Transform target)
    {
        if (target == null)
            return;

        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = targetRotation;
    }

    private bool CanMove()
    {
        if (!IsAgentReady)
            return false;

        if (IsMovementLocked)
            return false;

        return true;
    }

    private void HandleRotation()
    {
        if (!IsAgentReady)
            return;

        Vector3 velocity = agent.velocity;
        velocity.y = 0f;

        if (velocity.sqrMagnitude <= movementEpsilon)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(velocity);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}
