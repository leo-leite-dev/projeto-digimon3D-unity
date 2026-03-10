using System.Collections.Generic;
using UnityEngine;

public class PlayerDigidex : MonoBehaviour
{
    [Header("Digimons capturados")]
    public List<DigimonData> digidex = new List<DigimonData>();

    [Header("Digimon equipado")]
    public DigimonData equippedDigimon;

    [Header("Spawn")]
    public Transform spawnPoint;

    private GameObject currentModel;
    private Digimon digimonSlot;

    void Start()
    {
        digimonSlot = spawnPoint.GetComponentInParent<Digimon>();

        DetectCurrentDigimon();

        if (currentModel == null && equippedDigimon != null)
            SpawnDigimon();
    }

    public void CaptureDigimon(DigimonData data)
    {
        if (!digidex.Contains(data))
            digidex.Add(data);
    }

    public void EquipDigimon(DigimonData data)
    {
        if (!digidex.Contains(data))
            return;

        equippedDigimon = data;

        SpawnDigimon();
    }

    void DetectCurrentDigimon()
    {
        if (digimonSlot != null)
        {
            equippedDigimon = digimonSlot.data;

            if (equippedDigimon != null && !digidex.Contains(equippedDigimon))
                digidex.Add(equippedDigimon);
        }
    }

    void SpawnDigimon()
    {
        if (equippedDigimon == null || spawnPoint == null)
            return;

        if (currentModel != null)
            Destroy(currentModel);

        currentModel = Instantiate(equippedDigimon.prefab, spawnPoint);

        if (digimonSlot != null)
            digimonSlot.data = equippedDigimon;
    }
}
