using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : ValidatedMonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Transform cameraTransform;

    [SerializeField]
    private Animator animator;

    [Header("Movement")]
    [SerializeField]
    private float speed = 4f;

    [SerializeField]
    private float acceleration = 15f;

    [SerializeField]
    private float deceleration = 25f;

    [SerializeField]
    private float rotationSpeed = 10f;

    [SerializeField]
    private float gravity = -9.81f;

    [Header("Debug / Systems")]
    [SerializeField]
    private int maxPositionHistory = 200;

    private CharacterController controller;
    private PlayerControls input;

    private Vector3 moveDirection;
    private Vector3 currentVelocity;
    private float verticalVelocity;

    private readonly Queue<Vector3> positionHistory = new();

    public bool IsMoving => currentVelocity.sqrMagnitude > 0.01f;
    public Vector3[] PositionHistory => positionHistory.ToArray();

    private RaycastHit groundHit;

    public void Setup(PlayerControls input, Transform cameraTransform)
    {
        this.input = input;
        this.cameraTransform = cameraTransform;

        if (this.input == null)
            Debug.LogError("[PlayerMovement] Input inválido no Setup.", this);

        if (this.cameraTransform == null)
            Debug.LogError("[PlayerMovement] Camera Transform inválido no Setup.", this);
    }

    protected override void Awake()
    {
        base.Awake();

        controller = GetComponent<CharacterController>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    protected override void Validate()
    {
        if (animator == null)
            Debug.LogError("[PlayerMovement] Animator não encontrado.", this);
    }

    void Update()
    {
        if (input == null || cameraTransform == null)
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

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        moveDirection = cameraForward * vertical + cameraRight * horizontal;

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

        Vector3 targetVelocity = slopeDirection * speed;

        currentVelocity = Vector3.MoveTowards(
            currentVelocity,
            moveDirection.sqrMagnitude > 0.01f ? targetVelocity : Vector3.zero,
            (moveDirection.sqrMagnitude > 0.01f ? acceleration : deceleration) * Time.deltaTime
        );

        Vector3 velocity = currentVelocity;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);
    }

    void ApplyGravity()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.deltaTime;
    }

    void HandleRotation()
    {
        if (moveDirection.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    void UpdateAnimation()
    {
        if (animator == null)
            return;

        float speedPercent = currentVelocity.magnitude / speed;

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
