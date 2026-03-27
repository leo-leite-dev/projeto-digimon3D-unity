using UnityEngine;

[RequireComponent(typeof(DigimonMovement))]
public class EnemyWander : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float waitTime = 2f;

    private DigimonMovement movement;
    private IWanderPositionSampler positionSampler;

    private float waitTimer;
    private bool isInitialized;

    private void Awake()
    {
        movement = GetComponent<DigimonMovement>();

        if (movement == null)
        {
            Debug.LogError("❌ EnemyWander: DigimonMovement não encontrado", this);
            enabled = false;
            return;
        }

        positionSampler = GetComponent<IWanderPositionSampler>();

        if (positionSampler == null)
        {
            Debug.LogError("❌ EnemyWander: IWanderPositionSampler não encontrado", this);
            enabled = false;
            return;
        }

        isInitialized = true;
    }

    private void Start()
    {
        if (!isInitialized)
            return;

        ChooseNewDestination();
    }

    private void Update()
    {
        if (!isInitialized)
            return;

        if (movement.PathPending)
            return;

        if (IsIdle())
            HandleIdle();
    }

    private bool IsIdle()
    {
        return movement.HasReachedDestination() || movement.IsIdle();
    }

    private void HandleIdle()
    {
        waitTimer -= Time.deltaTime;

        if (waitTimer <= 0f)
            ChooseNewDestination();
    }

    private void ChooseNewDestination()
    {
        Vector3 destination = positionSampler.Sample();

        movement.SetDestination(destination);

        waitTimer = waitTime;
    }
}
