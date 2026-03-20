using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private Transform spawnPoint;

    [Header("Camera")]
    [SerializeField]
    private Transform mainCamera;

    [SerializeField]
    private CinemachineCamera freeLookCamera;

    private PlayerControls input;
    private Transform playerTransform;

    private IEnumerator Start()
    {
        InitializeInput();

        SpawnPlayer();

        // 👉 Espera 1 frame pro Cinemachine inicializar
        yield return null;

        BindCamera();
    }

    private void InitializeInput()
    {
        input = new PlayerControls();
        input.Enable();
    }

    private void SpawnPlayer()
    {
        if (playerPrefab == null || spawnPoint == null)
        {
            Debug.LogError("PlayerSpawner: Prefab ou SpawnPoint não definidos.");
            return;
        }

        var playerObj = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);

        playerTransform = playerObj.transform;

        SetupPlayer(playerObj);
    }

    private void SetupPlayer(GameObject playerObj)
    {
        var movement = playerObj.GetComponent<PlayerMovement>();

        if (movement == null)
        {
            Debug.LogError("PlayerSpawner: PlayerMovement não encontrado.");
            return;
        }

        movement.Setup(input, Camera.main.transform);
    }

    private void BindCamera()
    {
        if (freeLookCamera == null)
        {
            Debug.LogError("PlayerSpawner: Camera não atribuída.");
            return;
        }

        if (playerTransform == null)
        {
            Debug.LogError("PlayerSpawner: Player não existe ainda.");
            return;
        }

        Debug.Log("BindCamera FINAL com: " + playerTransform.name);

        // 👉 Forma correta na tua versão do Cinemachine
        freeLookCamera.Target.TrackingTarget = playerTransform;
        freeLookCamera.Target.LookAtTarget = playerTransform;
    }
}
