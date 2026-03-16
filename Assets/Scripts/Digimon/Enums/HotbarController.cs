using UnityEngine;

public class HotbarController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private PlayerCombatController combatController;

    [SerializeField]
    private InventoryController inventoryController;

    [Header("Slots")]
    [SerializeField]
    private HotbarSlot[] slots = new HotbarSlot[8];

    private void Awake()
    {
        if (combatController == null)
            TryGetComponent(out combatController);

        if (inventoryController == null)
            TryGetComponent(out inventoryController);

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
            return;

        HotbarSlot slot = slots[index];

        switch (slot.type)
        {
            case SlotType.Skill:
                UseDigimonSkill(index);
                break;

            case SlotType.Item:
                UseItem(slot);
                break;
        }
    }

    private void UseDigimonSkill(int index)
    {
        if (combatController == null)
            return;

        DigimonFollow currentDigimon = combatController.CurrentDigimon;

        if (currentDigimon == null || currentDigimon.data == null)
            return;

        var skills = currentDigimon.data.skills;

        if (skills == null || skills.Count == 0)
            return;

        if (index >= skills.Count)
            return;

        DigimonSkill skill = skills[index];

        if (skill == null)
            return;

        combatController.UseSkill(skill);
    }

    private void UseItem(HotbarSlot slot)
    {
        if (inventoryController == null)
            return;

        if (slot.item == null)
            return;

        inventoryController.UseItem(slot.item);
    }

    public int SlotCount => slots.Length;
}
