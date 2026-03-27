using System.Collections.Generic;
using UnityEngine;

public class Digimon : ValidatedMonoBehaviour
{
    [Header("Data")]
    [SerializeField]
    private DigimonData data;

    public DigimonData Data => data;

    public string Name => data?.digimonName ?? "Unknown";

    public DigimonType Type => data != null ? data.type : default;
    public DigimonStage Stage => data != null ? data.stage : default;
    public DigimonElement Element => data != null ? data.element : default;

    public DigimonAttributes attributes { get; private set; }
    public DigimonStats stats { get; private set; }
    public DigimonLevel level { get; private set; }

    public int Level => level?.Level ?? 1;

    public bool IsInitialized => data != null && level != null && stats != null;

    public GameObject ModelPrefab => data != null ? data.modelPrefab : null;

    public int BaseExpReward => data != null ? data.baseExpReward : 0;

    private static readonly IReadOnlyList<DigimonSkill> emptySkills =
        System.Array.Empty<DigimonSkill>();

    public IReadOnlyList<DigimonSkill> Skills => data?.skills ?? emptySkills;

    public bool HasSkills => Skills.Count > 0;

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

        InitializeCore();
    }

    private void InitializeCore()
    {
        if (data == null)
            return;

        stats ??= new DigimonStats();
        level ??= new DigimonLevel();

        level.Level = data.startLevel;

        RecalculateStats();
    }

    public void AddExperience(int amount)
    {
        if (!IsInitialized)
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
        if (!IsInitialized)
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
        if (!IsInitialized)
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
