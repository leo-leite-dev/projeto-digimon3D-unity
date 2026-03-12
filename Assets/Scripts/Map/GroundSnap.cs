using UnityEngine;

public class GroundSnap : MonoBehaviour
{
    public float rayDistance = 5f;
    public float heightOffset = 0.1f;
    public LayerMask groundLayer;

    void Update()
    {
        Ray ray = new Ray(transform.position + Vector3.up, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, groundLayer))
        {
            Vector3 pos = transform.position;
            pos.y = hit.point.y + heightOffset;
            transform.position = pos;
        }
    }
}
