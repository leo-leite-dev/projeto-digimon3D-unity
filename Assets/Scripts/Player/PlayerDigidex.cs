using System.Collections.Generic;
using UnityEngine;

public class PlayerDigidex : ValidatedMonoBehaviour
{
    [Header("Digimons armazenados")]
    [SerializeField]
    private List<DigimonData> digidex = new();

    [Header("Digimon equipado")]
    public DigimonData equippedDigimon;

    [Header("Spawn")]
    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private Transform followPoint;

    [SerializeField]
    private PlayerCombatController combatController;

    [SerializeField]
    private GameObject digimonFollowerPrefab;

    private GameObject currentDigimon;

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
            digimonFollowerPrefab = Resources.Load<GameObject>(
                "Prefabs/EntityContainer/DigimonFollow"
            );

            if (digimonFollowerPrefab == null)
                Debug.LogError("DigimonFollowerPrefab não encontrado em Resources/Prefabs/");
        }
    }

    void Start()
    {
        if (!ValidateSetup())
            return;

        if (equippedDigimon != null)
            SpawnDigimon();
    }

    void AutoSetup()
    {
        if (spawnPoint == null)
            spawnPoint = transform.Find("SpawnPoint");

        if (followPoint == null)
            followPoint = transform.Find("FollowPoint");
    }

    bool ValidateSetup()
    {
        bool valid = true;

        if (spawnPoint == null)
        {
            Debug.LogError($"{nameof(PlayerDigidex)}: SpawnPoint não encontrado no Player.", this);
            valid = false;
        }

        if (followPoint == null)
        {
            Debug.LogError($"{nameof(PlayerDigidex)}: FollowPoint não encontrado no Player.", this);
            valid = false;
        }

        if (digimonFollowerPrefab == null)
        {
            Debug.LogError($"{nameof(PlayerDigidex)}: FollowerPrefab não definido.", this);
            valid = false;
        }

        return valid;
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
        if (!ValidateSetup())
            return;

        if (equippedDigimon == null)
            return;

        if (equippedDigimon.modelPrefab == null)
        {
            Debug.LogError("DigimonData não possui modelPrefab.", this);
            return;
        }

        if (currentDigimon != null)
            Destroy(currentDigimon);

        Vector3 spawnPos = NavMeshUtility.GetValidPosition(spawnPoint.position);

        currentDigimon = Instantiate(digimonFollowerPrefab, spawnPos, spawnPoint.rotation);
        currentDigimon.name = equippedDigimon.digimonName;

        NavMeshUtility.WarpAgentToValidPosition(
            currentDigimon.GetComponent<UnityEngine.AI.NavMeshAgent>(),
            spawnPos
        );

        DigimonFollow follow = currentDigimon.GetComponent<DigimonFollow>();

        if (follow != null)
        {
            follow.data = equippedDigimon;
            follow.Initialize(transform, followPoint);
            follow.SpawnModel(equippedDigimon.modelPrefab);
        }

        RegisterCombat();
    }

    void RegisterCombat()
    {
        if (combatController == null)
            return;

        DigimonAttack attack = currentDigimon.GetComponentInChildren<DigimonAttack>();

        if (attack == null)
            return;

        combatController.SetDigimon(attack);
    }
}
