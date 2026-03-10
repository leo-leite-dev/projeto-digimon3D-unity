using Unity.Cinemachine;
using UnityEngine;

public class CameraRightClickRotate : MonoBehaviour
{
    public CinemachineInputAxisController inputController;

    void Update()
    {
        if (Input.GetMouseButton(1))
            inputController.enabled = true;
        else
            inputController.enabled = false;
    }
}
