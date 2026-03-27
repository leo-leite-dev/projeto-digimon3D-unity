using UnityEngine;

public class PlayerOverworldComposer : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private PlayerReferences references;

    [Header("Gameplay")]
    [SerializeField]
    private PlayerDigidex digidex;

    [SerializeField]
    private PlayerInputController inputController;

    [SerializeField]
    private InventoryController inventory;

    [Header("Digimon")]
    [SerializeField]
    private GameObject digimonBasePrefab;

    [SerializeField]
    private DigimonData initialDigimon;

    private PlayerControls input;

    private void Awake()
    {
        input = new PlayerControls();

        Compose(references);
    }

    private void Start()
    {
        SpawnInitialDigimon();
    }

    private void OnEnable() => input.Enable();

    private void OnDisable() => input.Disable();

    public void Compose(PlayerReferences refs)
    {
        if (!ValidateAll(refs))
            return;

        SetupInput();
        SetupMovement(refs);

        Debug.Log("[PlayerOverworldComposer] ✅ COMPOSED");
    }

    private void SetupInput()
    {
        inputController.Initialize(input);
    }

    private void SetupMovement(PlayerReferences refs)
    {
        refs.Movement.Setup(input);
    }

    private void SpawnInitialDigimon()
    {
        if (initialDigimon == null)
        {
            Debug.LogWarning("⚠️ Nenhum Digimon inicial definido", this);
            return;
        }

        if (digimonBasePrefab == null)
        {
            Debug.LogError("❌ digimonBasePrefab não definido", this);
            return;
        }

        var spawnPoint = references.SpawnPoint;
        var followPoint = references.FollowPoint;

        if (spawnPoint == null || followPoint == null)
        {
            Debug.LogError("❌ SpawnPoint ou FollowPoint inválidos", this);
            return;
        }

        var go = Instantiate(digimonBasePrefab, spawnPoint.position, spawnPoint.rotation);

        var core = go.GetComponent<DigimonCoreReferences>();
        var followRefs = go.GetComponent<DigimonFollowReferences>();

        if (core == null || !core.IsValid())
        {
            Debug.LogError("❌ Core inválido", go);
            Destroy(go);
            return;
        }

        if (followRefs == null || !followRefs.IsValid())
        {
            Debug.LogError("❌ FollowRefs inválido", go);
            Destroy(go);
            return;
        }

        var digimon = core.Digimon;
        digimon.Setup(initialDigimon);

        var composer = go.GetComponent<DigimonFollowOverworldComposer>();

        if (composer == null)
        {
            Debug.LogError("❌ Composer não encontrado", go);
            Destroy(go);
            return;
        }

        composer.Compose(core, followRefs, spawnPoint.root, followPoint);

        digidex.SetCurrent(go);

        Debug.Log($"🐾 Digimon spawnado: {initialDigimon.digimonName}");
    }

    private bool ValidateAll(PlayerReferences refs)
    {
        bool valid = true;

        if (refs == null || !refs.IsValid())
        {
            Debug.LogError("❌ PlayerReferences inválido", this);
            valid = false;
        }

        if (inputController == null)
        {
            Debug.LogError("❌ PlayerInputController não atribuído", this);
            valid = false;
        }

        if (digidex == null)
        {
            Debug.LogError("❌ PlayerDigidex não atribuído", this);
            valid = false;
        }

        if (inventory == null)
        {
            Debug.LogError("❌ InventoryController não atribuído", this);
            valid = false;
        }

        if (digimonBasePrefab == null)
        {
            Debug.LogError("❌ digimonBasePrefab NÃO setado", this);
            valid = false;
        }

        return valid;
    }
}
