using UnityEngine;

public class CombatCameraController : MonoBehaviour
{
    [Header("Targets (Spawn Points)")]
    private Transform playerSpawn;
    private Transform enemySpawn;

    [Header("Settings")]
    [SerializeField]
    private float height = 3f;

    [SerializeField]
    private float distance = 10f;

    private Vector3 currentCenter;
    private bool isInitialized;

    public void Initialize(Transform playerSpawn, Transform enemySpawn)
    {
        this.playerSpawn = playerSpawn;
        this.enemySpawn = enemySpawn;

        if (playerSpawn == null || enemySpawn == null)
        {
            Debug.LogError("❌ CombatCameraController: Spawn points inválidos");
            return;
        }

        CalculateCenter();
        SetupCamera();

        isInitialized = true;

        Debug.Log("📷 CombatCamera inicializada");
    }

    private void CalculateCenter()
    {
        currentCenter = (playerSpawn.position + enemySpawn.position) * 0.5f;
    }

    private void SetupCamera()
    {
        Vector3 targetPosition = new Vector3(currentCenter.x, height, currentCenter.z - distance);

        transform.position = targetPosition;

        transform.LookAt(new Vector3(currentCenter.x, 0f, currentCenter.z));
    }
}
