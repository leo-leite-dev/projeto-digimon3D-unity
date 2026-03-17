using UnityEngine;

public static class DigimonFactory
{
    public static GameObject Create(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null)
            return null;

        return Object.Instantiate(prefab, position, rotation);
    }
}
