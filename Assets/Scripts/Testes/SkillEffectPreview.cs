using UnityEngine;

public class SkillEffectPreview : MonoBehaviour
{
    [Header("Preview")]
    [SerializeField]
    private float speed = 8f;

    [SerializeField]
    private bool moveForward = false;

    void Update()
    {
        if (moveForward)
            transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void StartMoveForward()
    {
        moveForward = true;
    }

    public void StopMoveForward()
    {
        moveForward = false;
    }
}
