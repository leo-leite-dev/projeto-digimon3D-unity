using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
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
    private float acceleration = 10f;

    [SerializeField]
    private float rotationSpeed = 10f;

    [SerializeField]
    private float gravity = -9.81f;

    [Header("Debug / Systems")]
    [SerializeField]
    private int maxPositionHistory = 200;

    private CharacterController controller;

    private Vector3 moveDirection;
    private Vector3 currentVelocity;
    private float verticalVelocity;

    private readonly Queue<Vector3> positionHistory = new Queue<Vector3>();

    public Vector3 MoveDirection => moveDirection;

    public bool IsMoving => currentVelocity.sqrMagnitude > 0.01f;

    public Vector3[] PositionHistory => positionHistory.ToArray();

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
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
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

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
        Vector3 targetVelocity = moveDirection * speed;

        float lerpFactor = Mathf.Clamp01(acceleration * Time.deltaTime);
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, lerpFactor);

        ApplyGravity();

        Vector3 velocity = new Vector3(currentVelocity.x, verticalVelocity, currentVelocity.z);

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
