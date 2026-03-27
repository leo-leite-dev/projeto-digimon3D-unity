using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private PlayerMovementConfig config;

    [Header("Debug / Systems")]
    [SerializeField]
    private int maxPositionHistory = 200;

    private CharacterController controller;
    private Animator animator;

    private PlayerControls input;

    private Vector3 moveDirection;
    private Vector3 currentVelocity;
    private float verticalVelocity;

    private readonly Queue<Vector3> positionHistory = new();

    public bool IsMoving => currentVelocity.sqrMagnitude > 0.01f;
    public Vector3[] PositionHistory => positionHistory.ToArray();

    private RaycastHit groundHit;

    public void Setup(PlayerControls input)
    {
        if (input == null)
        {
            Debug.LogError("❌ INPUT NULL - Player não vai se mover");
        }
        this.input = input;

        if (this.input == null)
            Debug.LogError("[PlayerMovement] Input inválido no Setup.", this);
    }

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        if (controller == null)
            Debug.LogError("[PlayerMovement] CharacterController não encontrado.", this);

        if (animator == null)
            Debug.LogError("[PlayerMovement] Animator não encontrado.", this);

        if (config == null)
            Debug.LogError("[PlayerMovement] Config não atribuída.", this);
    }

    void Update()
    {
        if (input == null || config == null)
            return;

        ReadInput();
        HandleMovement();
        HandleRotation();
        UpdateAnimation();
        SavePositionHistory();
    }

    void ReadInput()
    {
        Vector2 inputVector = input.Movement.Move.ReadValue<Vector2>();

        float horizontal = inputVector.x;
        float vertical = inputVector.y;

        Transform cam = Camera.main.transform;

        Vector3 forward = cam.forward;
        Vector3 right = cam.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        moveDirection = forward * vertical + right * horizontal;

        if (moveDirection.sqrMagnitude > 1f)
            moveDirection.Normalize();
    }

    void HandleMovement()
    {
        ApplyGravity();

        Vector3 groundNormal = Vector3.up;

        if (Physics.Raycast(transform.position, Vector3.down, out groundHit, 2f))
            groundNormal = groundHit.normal;

        Vector3 slopeDirection = Vector3.ProjectOnPlane(moveDirection, groundNormal).normalized;

        Vector3 targetVelocity = slopeDirection * config.speed;

        currentVelocity = Vector3.MoveTowards(
            currentVelocity,
            moveDirection.sqrMagnitude > 0.01f ? targetVelocity : Vector3.zero,
            (moveDirection.sqrMagnitude > 0.01f ? config.acceleration : config.deceleration)
                * Time.deltaTime
        );

        Vector3 velocity = currentVelocity;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);
    }

    void ApplyGravity()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        verticalVelocity += config.gravity * Time.deltaTime;
    }

    void HandleRotation()
    {
        if (moveDirection.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            config.rotationSpeed * Time.deltaTime
        );
    }

    void UpdateAnimation()
    {
        if (animator == null)
            return;

        float speedPercent = currentVelocity.magnitude / config.speed;

        animator.SetFloat("Speed", speedPercent, 0.1f, Time.deltaTime);
    }

    void SavePositionHistory()
    {
        if (currentVelocity.sqrMagnitude <= 0.01f)
            return;

        positionHistory.Enqueue(transform.position);

        if (positionHistory.Count > maxPositionHistory)
            positionHistory.Dequeue();
    }
}
