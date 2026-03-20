using UnityEngine;

public static class DigimonFactory
{
    public static GameObject Create(GameObject prefab, DigimonData data, Vector3 position)
    {
        GameObject go = Object.Instantiate(prefab, position, Quaternion.identity);

        var digimon = go.GetComponent<Digimon>();

        if (digimon == null)
        {
            Debug.LogError("❌ Sem Digimon component", go);
            Object.Destroy(go);
            return null;
        }

        digimon.Setup(data);

        return go;
    }
}
