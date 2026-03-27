using UnityEngine;

public class PlayerStatsController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private TamerStatsConfig config;

    public string Name => config.tamerName;
    public int Level => config.level;
    public int MaxHp => config.maxHp;

    public float AttackMultiplier => config.attackMultiplier;
    public float DefenseMultiplier => config.defenseMultiplier;
    public float CritMultiplier => config.critMultiplier;

    private void Awake()
    {
        if (config == null)
            Debug.LogError("[PlayerStatsController] Config não atribuída", this);
    }

    public TamerStatsConfig GetConfig()
    {
        return config;
    }
}
