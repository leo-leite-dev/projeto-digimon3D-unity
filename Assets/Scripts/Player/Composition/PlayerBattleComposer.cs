using UnityEngine;

public class PlayerBattleComposer : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private PlayerBattleController battleController;

    [SerializeField]
    private PlayerBattleInputController inputController;

    [SerializeField]
    private HotbarController hotbar;

    [SerializeField]
    private InventoryController inventoryController;

    private PlayerControls input;
    private BattleContext context;

    public void Initialize(BattleContext context)
    {
        this.context = context;

        Compose();
    }

    private void Awake()
    {
        input = new PlayerControls();
    }

    private void OnEnable() => input.Enable();

    private void OnDisable() => input.Disable();

    private void Compose()
    {
        if (!Validate())
            return;

        SetupBattle();
        SetupHotbar();
        SetupInput();

        Debug.Log("[PlayerBattleComposer] ⚔️ COMPOSED");
    }

    private void SetupBattle()
    {
        battleController.Initialize();
        battleController.SetContext(context);
    }

    private void SetupHotbar()
    {
        hotbar.Initialize(battleController, inventoryController, context);
    }

    private void SetupInput()
    {
        inputController.Initialize(input);
    }

    private bool Validate()
    {
        bool valid = true;

        if (battleController == null)
        {
            Debug.LogError("❌ PlayerBattleController não atribuído", this);
            valid = false;
        }

        if (inputController == null)
        {
            Debug.LogError("❌ PlayerBattleInputController não atribuído", this);
            valid = false;
        }

        if (hotbar == null)
        {
            Debug.LogError("❌ HotbarController não atribuído", this);
            valid = false;
        }

        if (inventoryController == null)
        {
            Debug.LogError("❌ InventoryController não atribuído", this);
            valid = false;
        }

        if (context == null)
        {
            Debug.LogError("❌ BattleContext não definido", this);
            valid = false;
        }

        return valid;
    }
}
