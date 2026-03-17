using UnityEngine;

public class Digimon : ValidatedMonoBehaviour
{
    [Header("Data")]
    [SerializeField]
    private DigimonData data;

    public DigimonData Data => data;

    public string Name => data != null ? data.digimonName : "Unknown";

    public DigimonType Type => data != null ? data.type : default;
    public DigimonStage Stage => data != null ? data.stage : default;
    public DigimonElement Element => data != null ? data.element : default;

    public DigimonAttributes attributes { get; private set; }
    public DigimonStats stats { get; private set; }
    public DigimonLevel level { get; private set; }

    public int Level => level != null ? level.Level : 1;

    protected override void Awake()
    {
        base.Awake();
    }

    public virtual void Setup(DigimonData digimonData)
    {
        data = digimonData;

        if (data == null)
            return;

        stats ??= new DigimonStats();
        level ??= new DigimonLevel();

        level.Level = data.startLevel;

        RecalculateStats();
    }

    public void AddExperience(int amount)
    {
        if (level == null)
            return;

        level.Experience += amount;

        while (level.Experience >= level.ExpToNextLevel)
        {
            level.Experience -= level.ExpToNextLevel;
            level.Level++;
            ApplyLevelGrowth();
        }
    }

    public void ApplyLevelGrowth()
    {
        if (data == null || level == null || stats == null)
            return;

        RecalculateStats();

        Debug.Log(
            $"{Name} LEVEL UP\n"
                + $"Attributes → STR:{attributes.Strength} INT:{attributes.Intelligence} AGI:{attributes.Agility} VIT:{attributes.Vitality} SPI:{attributes.Spirit}\n"
                + $"Stats → HP:{stats.Hp} PATK:{stats.PhysicalAttack} MATK:{stats.MagicAttack} DEF:{stats.PhysicalDefense} MDEF:{stats.MagicDefense} SPD:{stats.Speed}"
        );
    }

    DigimonAttributes CalculateAttributesForLevel()
    {
        DigimonAttributes result = new DigimonAttributes
        {
            Strength = data.attributes.Strength,
            Intelligence = data.attributes.Intelligence,
            Agility = data.attributes.Agility,
            Vitality = data.attributes.Vitality,
            Spirit = data.attributes.Spirit,
        };

        for (int i = 1; i < level.Level; i++)
        {
            result.AddStrength(1);
            result.AddIntelligence(1);
            result.AddAgility(1);
            result.AddVitality(1);
            result.AddSpirit(1);
        }

        return result;
    }

    protected void RecalculateStats()
    {
        if (data == null || stats == null || level == null)
            return;

        attributes = CalculateAttributesForLevel();
        DigimonStatsCalculator.CalculateStats(attributes, stats);
    }

    [ContextMenu("Reload From Data")]
    public void ReloadFromData()
    {
        if (data == null)
        {
            Debug.LogError($"ReloadFromData falhou: data nulo em {gameObject.name}", this);
            return;
        }

        if (level == null)
            level = new DigimonLevel();

        if (stats == null)
            stats = new DigimonStats();

        level.Level = data.startLevel;
        RecalculateStats();
    }
}
