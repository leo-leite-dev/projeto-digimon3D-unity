using UnityEngine;

[CreateAssetMenu(fileName = "Digimon", menuName = "Digimon/Data")]
public class DigimonData : ScriptableObject
{
    public string digimonName;

    public DigimonType type;
    public DigimonStage stage;
    public DigimonElement element;

    [Header("Level")]
    public int startLevel = 1;

    [Header("Base Attributes")]
    public DigimonAttributes attributes;

    [Header("Prefab")]
    public GameObject prefab;

    [Header("Calculated Stats")]
    public DigimonStats calculatedStats = new DigimonStats();

    public int baseExpReward = 50;

    void RecalculateStats()
    {
        DigimonAttributes tempAttributes = new DigimonAttributes
        {
            Strength = attributes.Strength,
            Intelligence = attributes.Intelligence,
            Agility = attributes.Agility,
            Vitality = attributes.Vitality,
            Spirit = attributes.Spirit,
        };

        for (int i = 1; i < startLevel; i++)
        {
            tempAttributes.AddStrength(1);
            tempAttributes.AddIntelligence(1);
            tempAttributes.AddAgility(1);
            tempAttributes.AddVitality(1);
            tempAttributes.AddSpirit(1);
        }

        DigimonStatsCalculator.CalculateStats(tempAttributes, calculatedStats);
    }

    void OnValidate()
    {
        RecalculateStats();
    }
}
