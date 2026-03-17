using UnityEngine;

public class MovementCapability : MonoBehaviour, IMovementCapability
{
    [SerializeField]
    private DigimonMovement movement;

    public void MoveTo(Transform target)
    {
        movement.SetDestination(target.position);
    }

    public void Stop()
    {
        movement.StopMovement();
    }
}
