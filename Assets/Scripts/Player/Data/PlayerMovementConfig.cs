using UnityEngine;

[CreateAssetMenu(menuName = "Player/Movement Config")]
public class PlayerMovementConfig : ScriptableObject
{
    [Header("Movement")]
    public float speed = 4f;
    public float acceleration = 15f;
    public float deceleration = 25f;
    public float rotationSpeed = 10f;
    public float gravity = -9.81f;
}
