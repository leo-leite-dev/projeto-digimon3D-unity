using UnityEngine;

public class ScreenLimiter
{
    private readonly float horizontalLimit;

    public ScreenLimiter(float horizontalLimit)
    {
        this.horizontalLimit = horizontalLimit;
    }

    public Vector3 ClampPosition(Vector3 position, Vector3 cameraCenter)
    {
        float minX = cameraCenter.x - horizontalLimit;
        float maxX = cameraCenter.x + horizontalLimit;

        float clampedX = Mathf.Clamp(position.x, minX, maxX);

        return new Vector3(clampedX, position.y, position.z);
    }
}
