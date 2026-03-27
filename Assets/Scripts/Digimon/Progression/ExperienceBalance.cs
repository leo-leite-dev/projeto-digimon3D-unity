using UnityEngine;

[CreateAssetMenu(fileName = "ExperienceBalance", menuName = "Digimon/Balance/Experience")]
public class ExperienceBalance : ScriptableObject
{
    [Header("XP Penalty (Enemy weaker)")]
    public float[] xpPenaltyTable = { 1f, 0.8f, 0.6f, 0.4f, 0.3f, 0.2f, 0.1f, 0.01f };

    [Header("XP Bonus (Enemy stronger)")]
    public float[] xpBonusTable = { 1f, 1.1f, 1.2f, 1.4f, 1.6f, 1.8f, 2f };
}
