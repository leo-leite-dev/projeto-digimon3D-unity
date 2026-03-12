using UnityEngine;

public class Digimon : MonoBehaviour
{
    [Header("Data")]
    public DigimonData data;

    public string Name => data.digimonName;
    public DigimonType Type => data.type;
    public DigimonStage Stage => data.stage;
    public DigimonElement Element => data.element;

    public DigimonAttributes attributes { get; private set; }
    public DigimonStats stats { get; private set; }

    public DigimonLevel level { get; private set; }

    public int Level => level.Level;

    protected virtual void Awake() { }

    void Start()
    {
        if (stats == null)
            Debug.LogError($"{name} não foi inicializado! Chame Initialize().");
    }

    public virtual void Initialize(DigimonData digimonData)
    {
        data = digimonData;

        if (data == null)
        {
            Debug.LogError("DigimonData não definido no Digimon!");
            return;
        }

        stats = new DigimonStats();

        level = new DigimonLevel { Level = data.startLevel };

        RecalculateStats();
    }

    public void AddExperience(int amount)
    {
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
        attributes = CalculateAttributesForLevel();

        DigimonStatsCalculator.CalculateStats(attributes, stats);
    }

    [ContextMenu("Reload From Data")]
    public void ReloadFromData()
    {
        level.Level = data.startLevel;

        RecalculateStats();
    }
}
