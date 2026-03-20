using UnityEngine;
using Unity.Cinemachine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cam;
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float minDistance = 3f;
    [SerializeField] private float maxDistance = 10f;

    private CinemachineOrbitalFollow orbital;

    void Awake()
    {
        orbital = cam.GetComponent<CinemachineOrbitalFollow>();
    }

    void Update()
    {
        float scroll = Input.mouseScrollDelta.y;

        if (Mathf.Abs(scroll) < 0.01f)
            return;

        Vector3 offset = orbital.TargetOffset;

        offset.z -= scroll * zoomSpeed; // 👈 zoom

        offset.z = Mathf.Clamp(offset.z, -maxDistance, -minDistance);

        orbital.TargetOffset = offset;
    }
}