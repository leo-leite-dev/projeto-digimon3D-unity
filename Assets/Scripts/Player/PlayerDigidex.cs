using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    [SerializeField]
    private PlayerCombatController combatController;

    [SerializeField]
    private GameObject digimonFollowerPrefab;

    private GameObject currentDigimonObject;
    private DigimonFollow currentDigimonFollow;

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
            {
                Debug.LogError(
                    $"{nameof(PlayerDigidex)}: DigimonFollow prefab não encontrado em Resources/Prefabs/EntityContainer/DigimonFollow.",
                    this
                );
            }
        }
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
            Debug.LogError($"{nameof(PlayerDigidex)}: Digimon follower prefab não definido.", this);
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
        if (data == null)
            return;

        if (!digidex.Contains(data))
            return;

        equippedDigimon = data;
        SpawnEquippedDigimon();
    }

    private void SpawnEquippedDigimon()
    {
        if (!ValidateSetup())
            return;

        if (equippedDigimon == null)
            return;

        if (equippedDigimon.modelPrefab == null)
        {
            Debug.LogError($"{nameof(PlayerDigidex)}: DigimonData sem modelPrefab.", this);
            return;
        }

        DespawnCurrentDigimon();

        Vector3 spawnPosition = NavMeshUtility.GetValidPosition(spawnPoint.position);

        currentDigimonObject = Instantiate(
            digimonFollowerPrefab,
            spawnPosition,
            spawnPoint.rotation
        );
        currentDigimonObject.name = equippedDigimon.digimonName;

        NavMeshAgent agent = currentDigimonObject.GetComponent<NavMeshAgent>();
        NavMeshUtility.WarpAgentToValidPosition(agent, spawnPosition);

        currentDigimonFollow = currentDigimonObject.GetComponent<DigimonFollow>();

        if (currentDigimonFollow == null)
        {
            Debug.LogError(
                $"{nameof(PlayerDigidex)}: O prefab instanciado não possui DigimonFollow.",
                currentDigimonObject
            );
            return;
        }

        currentDigimonFollow.Setup(equippedDigimon);
        currentDigimonFollow.Initialize(transform, followPoint);
        currentDigimonFollow.SpawnModel(equippedDigimon.modelPrefab);

        DigimonReferences references = currentDigimonObject.GetComponent<DigimonReferences>();
        if (references != null)
            references.RefreshVisualReferences();

        RegisterCombatDigimon(currentDigimonFollow);
    }

    private void DespawnCurrentDigimon()
    {
        if (currentDigimonObject != null)
            Destroy(currentDigimonObject);

        currentDigimonObject = null;
        currentDigimonFollow = null;
    }

    private void RegisterCombatDigimon(DigimonFollow digimonFollow)
    {
        if (combatController == null)
            return;

        combatController.SetDigimon(digimonFollow);
    }
}
