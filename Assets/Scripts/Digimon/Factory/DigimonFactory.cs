using UnityEngine;

public static class DigimonFactory
{
    public static GameObject Create(GameObject basePrefab, Vector3 position, Quaternion rotation)
    {
        if (basePrefab == null)
        {
            Debug.LogError("❌ DigimonFactory: BasePrefab nulo");
            return null;
        }

        var go = Object.Instantiate(basePrefab, position, rotation);

        if (go == null)
        {
            Debug.LogError("❌ DigimonFactory: Falha ao instanciar");
            return null;
        }

        var core = go.GetComponent<DigimonCoreReferences>();

        if (core == null)
        {
            Debug.LogError("❌ DigimonFactory: CoreReferences não encontrado", go);
            Object.Destroy(go);
            return null;
        }

        return go;
    }
}
