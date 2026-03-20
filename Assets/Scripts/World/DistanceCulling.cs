using UnityEngine;

public class DistanceCulling : MonoBehaviour
{
    public Transform player;
    public float maxDistance = 30f;

    private GameObject[] objects;

    void Start()
    {
        objects = GameObject.FindGameObjectsWithTag("Chunkable");

        ApplyCulling();
    }

    void ApplyCulling()
    {
        foreach (var obj in objects)
        {
            float dist = Vector3.Distance(player.position, obj.transform.position);

            if (dist <= maxDistance)
                obj.SetActive(true);
            else
                obj.SetActive(false);
        }
    }
}
