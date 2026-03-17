using UnityEngine;

public interface IMovementCapability : IDigimonCapability
{
    void MoveTo(Transform target);
    void Stop();
}
