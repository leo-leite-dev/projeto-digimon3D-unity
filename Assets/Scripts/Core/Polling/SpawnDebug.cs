using UnityEngine;

public class SpawnDebug : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log(
            $"[SpawnDebug.Awake] {name} id={gameObject.GetInstanceID()} scene={gameObject.scene.name}",
            gameObject
        );
        Debug.Log(System.Environment.StackTrace);
    }

    private void OnEnable()
    {
        Debug.Log($"[SpawnDebug.OnEnable] {name} id={gameObject.GetInstanceID()}", gameObject);
    }
}
