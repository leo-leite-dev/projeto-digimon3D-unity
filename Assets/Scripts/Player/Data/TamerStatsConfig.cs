using UnityEngine;

[CreateAssetMenu(menuName = "Player/Tamer Stats")]
public class TamerStatsConfig : ScriptableObject
{
    public string tamerName;

    public int level;

    public int maxHp;

    [Header("Multipliers (%)")]
    public float attackMultiplier = 1f;
    public float defenseMultiplier = 1f;
    public float critMultiplier = 1f;

    [Header("Prefab")]
    public GameObject prefab;
}
