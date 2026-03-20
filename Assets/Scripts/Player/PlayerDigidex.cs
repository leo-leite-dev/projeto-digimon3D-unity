using System.Collections.Generic;
using UnityEngine;

public class PlayerDigidex : ValidatedMonoBehaviour
{
    [Header("Digimons armazenados")]
    [SerializeField]
    private List<DigimonData> digidex = new();

    [Header("Digimon equipado")]
    [SerializeField]
    private DigimonData equippedDigimon;

    [Header("Spawn")]
    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private Transform followPoint;

    [Header("Dependencies")]
    [SerializeField]
    private PlayerCombatController combatController;

    [SerializeField]
    private GameObject digimonFollowerPrefab;

    private DigimonSpawnService spawnService;

    private GameObject currentDigimonObject;

    protected override void Validate()
    {
        AutoSetup();

        if (combatController == null)
            combatController = GetComponent<PlayerCombatController>();
    }

    protected override void Awake()
    {
        base.Awake();

        if (digimonFollowerPrefab == null)
        {
            Debug.LogError($"{nameof(PlayerDigidex)}: Prefab não configurado.", this);
            return;
        }

        spawnService = new DigimonSpawnService(digimonFollowerPrefab);
    }

    private void Start()
    {
        if (!ValidateSetup())
            return;

        if (equippedDigimon != null)
            SpawnEquippedDigimon();
    }

    private void AutoSetup()
    {
        if (spawnPoint == null)
            spawnPoint = transform.Find("SpawnPoint");

        if (followPoint == null)
            followPoint = transform.Find("FollowPoint");
    }

    private bool ValidateSetup()
    {
        bool valid = true;

        if (spawnPoint == null)
        {
            Debug.LogError($"{nameof(PlayerDigidex)}: SpawnPoint não encontrado.", this);
            valid = false;
        }

        if (followPoint == null)
        {
            Debug.LogError($"{nameof(PlayerDigidex)}: FollowPoint não encontrado.", this);
            valid = false;
        }

        if (spawnService == null)
        {
            Debug.LogError($"{nameof(PlayerDigidex)}: SpawnService não criado.", this);
            valid = false;
        }

        if (combatController == null)
        {
            Debug.LogError($"{nameof(PlayerDigidex)}: CombatController não definido.", this);
            valid = false;
        }

        return valid;
    }

    public void CaptureDigimon(DigimonData data)
    {
        if (data == null)
            return;

        if (!digidex.Contains(data))
            digidex.Add(data);
    }

    public void EquipDigimon(DigimonData data)
    {
        if (data == null || !digidex.Contains(data))
            return;

        equippedDigimon = data;
        SpawnEquippedDigimon();
    }

    private void SpawnEquippedDigimon()
    {
        if (!ValidateSetup() || equippedDigimon == null)
            return;

        DespawnCurrentDigimon();

        var follow = spawnService.SpawnFollow(
            equippedDigimon,
            spawnPoint.position,
            spawnPoint.rotation,
            transform,
            followPoint
        );

        if (follow == null)
            return;

        currentDigimonObject = follow.gameObject;

        RegisterCombatDigimon(follow);
    }

    private void DespawnCurrentDigimon()
    {
        if (currentDigimonObject != null)
            Destroy(currentDigimonObject);

        currentDigimonObject = null;
    }

    private void RegisterCombatDigimon(DigimonFollow digimonFollow)
    {
        combatController.SetDigimon(digimonFollow);
    }
}
