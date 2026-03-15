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

    private readonly Queue<Vector3> positionHistory = new Queue<Vector3>();

    public bool IsMoving => currentVelocity.sqrMagnitude > 0.01f;
    public Vector3[] PositionHistory => positionHistory.ToArray();

    private RaycastHit groundHit;

    protected override void Awake()
    {
        base.Awake();

        controller = GetComponent<CharacterController>();

        input = new PlayerControls();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    protected override void Validate()
    {
        if (cameraTransform == null)
            Debug.LogError("[PlayerMovement] Camera Transform not assigned.", this);

        if (animator == null)
            Debug.LogWarning("[PlayerMovement] Animator not assigned.", this);
    }

    void OnEnable()
    {
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }

    void Update()
    {
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

        if (cameraTransform == null)
        {
            moveDirection = Vector3.zero;
            return;
        }

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

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

        Vector3 targetVelocity = slopeDirection * speed;

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            currentVelocity = Vector3.MoveTowards(
                currentVelocity,
                targetVelocity,
                acceleration * Time.deltaTime
            );
        }
        else
        {
            currentVelocity = Vector3.MoveTowards(
                currentVelocity,
                Vector3.zero,
                deceleration * Time.deltaTime
            );
        }

        Vector3 velocity = currentVelocity;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);
    }

    void ApplyGravity()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -5f;

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
