using UnityEngine;

public class GameSettings : MonoBehaviour
{
    void Start()
    {
        QualitySettings.vSyncCount = 0;

        int hz = (int)Screen.currentResolution.refreshRateRatio.value;

        // Usa metade do refresh rate (mais estável)
        Application.targetFrameRate = hz / 2;

        Debug.Log("Refresh Rate: " + hz);
        Debug.Log("Target FPS: " + (hz / 2));
    }
}