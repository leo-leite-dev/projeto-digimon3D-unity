using UnityEngine;

public class DigimonMovementLimiter
{
    private readonly ScreenLimiter screenLimiter;
    private readonly Transform cameraTransform;

    public DigimonMovementLimiter(ScreenLimiter screenLimiter, Transform cameraTransform)
    {
        this.screenLimiter = screenLimiter;
        this.cameraTransform = cameraTransform;
    }

    public Vector3 ClampPosition(Vector3 desiredPosition)
    {
        Vector3 cameraCenter = cameraTransform.position;

        return screenLimiter.ClampPosition(desiredPosition, cameraCenter);
    }
}
