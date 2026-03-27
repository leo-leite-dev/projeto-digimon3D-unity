using UnityEngine;

public class HotbarController : MonoBehaviour
{
    [Header("Slots")]
    [SerializeField]
    private HotbarSlot[] slots = new HotbarSlot[8];

    private PlayerBattleController playerController;
    private InventoryController inventoryController;
    private BattleContext context;

    public void Initialize(
        PlayerBattleController playerController,
        InventoryController inventoryController,
        BattleContext context
    )
    {
        this.playerController = playerController;
        this.inventoryController = inventoryController;
        this.context = context;

        ValidateSlots();
    }

    private void ValidateSlots()
    {
        if (slots == null || slots.Length == 0)
            slots = new HotbarSlot[8];

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
                slots[i] = new HotbarSlot();
        }
    }

    public void UseSlot(int index)
    {
        if (index < 0 || index >= slots.Length)
        {
            Debug.LogWarning("[Hotbar] Slot inválido");
            return;
        }

        if (context == null || context.IsFinished)
        {
            Debug.LogWarning("[Hotbar] Battle não está ativa");
            return;
        }

        var slot = slots[index];

        switch (slot.type)
        {
            case SlotType.PlayerSkill:
                UsePlayerSkill(slot);
                break;

            case SlotType.Item:
                UseItem(slot);
                break;

            default:
                Debug.LogWarning("[Hotbar] Tipo inválido para Player");
                break;
        }
    }

    private void UsePlayerSkill(HotbarSlot slot)
    {
        if (playerController == null)
        {
            Debug.LogError("[Hotbar] PlayerBattleController não definido");
            return;
        }

        if (slot.skill == null)
        {
            Debug.LogWarning("[Hotbar] Skill nula");
            return;
        }

        // playerController.UseSkill(slot.skill);
    }

    private void UseItem(HotbarSlot slot)
    {
        if (inventoryController == null)
        {
            Debug.LogError("[Hotbar] InventoryController não definido");
            return;
        }

        if (slot.item == null)
        {
            Debug.LogWarning("[Hotbar] Item nulo");
            return;
        }

        inventoryController.UseItem(slot.item);
    }

    public int SlotCount => slots.Length;
}
