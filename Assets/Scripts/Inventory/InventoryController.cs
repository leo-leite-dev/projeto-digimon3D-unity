using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField]
    private List<Item> items = new();

    public IReadOnlyList<Item> Items => items;

    public void AddItem(Item item)
    {
        if (item == null)
            return;

        items.Add(item);

#if UNITY_EDITOR
        Debug.Log($"Item adicionado: {item.itemName}");
#endif
    }

    public void RemoveItem(Item item)
    {
        if (item == null)
            return;

        items.Remove(item);
    }

    public void UseItem(Item item)
    {
        if (item == null)
            return;

        switch (item.type)
        {
            case ItemType.Heal:
                UseHealItem(item);
                break;

            case ItemType.Food:
                UseFoodItem(item);
                break;

            case ItemType.Buff:
                UseBuffItem(item);
                break;
        }

        RemoveItem(item);
    }

    void UseHealItem(Item item)
    {
        Debug.Log($"Usou item de cura: {item.itemName}");
    }

    void UseFoodItem(Item item)
    {
        Debug.Log($"Digimon alimentado com: {item.itemName}");
    }

    void UseBuffItem(Item item)
    {
        Debug.Log($"Buff aplicado: {item.itemName}");
    }
}
