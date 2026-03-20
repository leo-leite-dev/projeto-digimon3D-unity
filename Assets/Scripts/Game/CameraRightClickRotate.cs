using Unity.Cinemachine;
using UnityEngine;

public class CameraRightClickRotate : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private CinemachineInputAxisController inputController;

    [Header("Settings")]
    [SerializeField]
    private bool lockCursorOnRotate = true;

    void Awake()
    {
        if (inputController == null)
        {
            Debug.LogError("CameraRightClickRotate: inputController não definido!");
        }
    }

    void Update()
    {
        bool isRotating = Input.GetMouseButton(1);

        // Ativa/desativa rotação
        inputController.enabled = isRotating;

        // Cursor estilo MMO
        if (lockCursorOnRotate)
        {
            if (isRotating)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
