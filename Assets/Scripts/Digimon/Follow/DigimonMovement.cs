using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class DigimonMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float rotationSpeed = 10f;

    private NavMeshAgent agent;
    private int movementLockCount;

    public bool IsMoving => agent.velocity.sqrMagnitude > 0.01f;
    public Vector3 Velocity => agent.velocity;
    public bool IsMovementLocked => movementLockCount > 0;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }

    void Update()
    {
        HandleRotation();
    }

    public void MoveTo(Vector3 position)
    {
        if (!agent.enabled)
            return;

        if (IsMovementLocked)
            return;

        agent.isStopped = false;
        agent.SetDestination(position);
    }

    public void FollowTarget(Transform target)
    {
        if (target == null)
            return;

        if (IsMovementLocked)
            return;

        MoveTo(target.position);
    }

    public void StopMovement()
    {
        if (!agent.enabled)
            return;

        agent.isStopped = true;
        agent.ResetPath();
    }

    public void AddMovementLock()
    {
        movementLockCount++;
        StopMovement();
    }

    public void RemoveMovementLock()
    {
        movementLockCount = Mathf.Max(0, movementLockCount - 1);

        if (!agent.enabled)
            return;

        if (!IsMovementLocked)
            agent.isStopped = false;
    }

    public void ClearAllMovementLocks()
    {
        movementLockCount = 0;

        if (!agent.enabled)
            return;

        agent.isStopped = false;
    }

    public bool HasReachedDestination()
    {
        if (!agent.hasPath)
            return true;

        return agent.remainingDistance <= agent.stoppingDistance;
    }

    void HandleRotation()
    {
        Vector3 velocity = agent.velocity;
        velocity.y = 0;

        if (velocity.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(velocity);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}
