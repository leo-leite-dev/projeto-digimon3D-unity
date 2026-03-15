using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Game/Item")]
public class Item : ScriptableObject
{
    public string itemName;

    public Sprite icon;

    public ItemType type;
}

public enum ItemType
{
    Heal,
    Food,
    Buff,
}
