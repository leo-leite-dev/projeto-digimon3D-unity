using System.Collections.Generic;
using UnityEngine;

public class PlayerDigidex : MonoBehaviour
{
    [Header("Digimons armazenados")]
    public List<DigimonData> digidex = new();

    [Header("Digimon equipado")]
    public DigimonData equippedDigimon;

    [Header("Spawn")]
    public Transform spawnPoint;

    private GameObject currentDigimon;

    [SerializeField]
    private GameObject followerPrefab;

    void Start()
    {
        if (equippedDigimon != null)
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

    void SpawnDigimon()
    {
        if (equippedDigimon == null || spawnPoint == null)
            return;

        if (currentDigimon != null)
            Destroy(currentDigimon);

        currentDigimon = Instantiate(followerPrefab, spawnPoint.position, spawnPoint.rotation);

        Digimon digimon = currentDigimon.GetComponent<Digimon>();

        if (digimon != null)
            digimon.Initialize(equippedDigimon);

        PlayerMovement player = GetComponent<PlayerMovement>();
        TargetSystem targetSystem = GetComponent<TargetSystem>();
        PlayerCombatController combat = GetComponent<PlayerCombatController>();

        DigimonFollow follow = currentDigimon.GetComponent<DigimonFollow>();

        if (follow != null)
        {
            follow.player = player;
            follow.SpawnModel(equippedDigimon.modelPrefab);
        }

        DigimonAttack attack = currentDigimon.GetComponentInChildren<DigimonAttack>();

        if (attack != null)
        {
            attack.targetSystem = targetSystem;
            attack.digimon = digimon;

            FirePoint firePoint = currentDigimon.GetComponentInChildren<FirePoint>();

            if (firePoint != null)
                attack.firePoint = firePoint.transform;
        }

        if (combat != null && attack != null)
            combat.SetDigimon(attack);
    }
}
