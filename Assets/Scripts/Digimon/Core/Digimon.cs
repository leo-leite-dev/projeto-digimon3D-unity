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

    public bool IsInitialized => data != null;

    protected override void Awake()
    {
        base.Awake();
    }

    public virtual void Setup(DigimonData digimonData)
    {
        if (digimonData == null)
        {
            Debug.LogError($"❌ Setup falhou: DigimonData nulo em {gameObject.name}", this);
            return;
        }

        data = digimonData;

        InitializeStats();
    }

    private void InitializeStats()
    {
        stats ??= new DigimonStats();
        level ??= new DigimonLevel();

        level.Level = data.startLevel;

        RecalculateStats();
    }

    public void AddExperience(int amount)
    {
        if (!IsInitialized || level == null)
            return;

        level.Experience += amount;

        while (level.Experience >= level.ExpToNextLevel)
        {
            level.Experience -= level.ExpToNextLevel;
            level.Level++;

            ApplyLevelGrowth();
        }
    }

    private void ApplyLevelGrowth()
    {
        if (!IsInitialized || stats == null || level == null)
            return;

        RecalculateStats();

        Debug.Log(
            $"{Name} LEVEL UP\n"
                + $"Attributes → STR:{attributes.Strength} INT:{attributes.Intelligence} AGI:{attributes.Agility} VIT:{attributes.Vitality} SPI:{attributes.Spirit}\n"
                + $"Stats → HP:{stats.Hp} PATK:{stats.PhysicalAttack} MATK:{stats.MagicAttack} DEF:{stats.PhysicalDefense} MDEF:{stats.MagicDefense} SPD:{stats.Speed}"
        );
    }

    private DigimonAttributes CalculateAttributesForLevel()
    {
        var result = new DigimonAttributes
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
        if (!IsInitialized || stats == null || level == null)
            return;

        attributes = CalculateAttributesForLevel();

        DigimonStatsCalculator.CalculateStats(attributes, stats);
    }

    [ContextMenu("Reload From Data")]
    public void ReloadFromData()
    {
        if (data == null)
        {
            Debug.LogError($"❌ ReloadFromData falhou: data nulo em {gameObject.name}", this);
            return;
        }

        level ??= new DigimonLevel();
        stats ??= new DigimonStats();

        level.Level = data.startLevel;

        RecalculateStats();
    }
}
