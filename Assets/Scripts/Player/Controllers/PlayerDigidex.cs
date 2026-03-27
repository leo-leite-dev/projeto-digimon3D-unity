using UnityEngine;

public class PlayerDigidex : MonoBehaviour
{
    private GameObject currentDigimon;

    public void SetCurrent(GameObject digimonGO)
    {
        if (digimonGO == null)
        {
            Debug.LogError("❌ Digidex: digimonGO null");
            return;
        }

        if (currentDigimon != null)
            Destroy(currentDigimon);

        currentDigimon = digimonGO;

        Debug.Log($"📘 Digidex recebeu Digimon: {digimonGO.name}");
    }

    public Digimon GetCurrentDigimon()
    {
        if (currentDigimon == null)
            return null;

        return currentDigimon.GetComponent<Digimon>();
    }
}
