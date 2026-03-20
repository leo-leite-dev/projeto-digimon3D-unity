using UnityEngine;

public class PlayerComposer : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement movement;

    [SerializeField]
    private Transform cameraTransform;

    private PlayerControls input;

    void Awake()
    {
        input = new PlayerControls();
    }

    void Start()
    {
        Debug.Log($"[Composer] Injetando em: {movement.name}", movement);
        movement.Setup(input, cameraTransform);
    }

    void OnEnable()
    {
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }
}
