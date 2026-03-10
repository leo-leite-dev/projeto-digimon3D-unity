using UnityEngine;

public static class ExperienceCalculator
{
    public static int GetExpToNextLevel(int level)
    {
        return Mathf.RoundToInt(50 * Mathf.Pow(level, 1.4f));
    }

    public static int CalculateExpGain(Digimon attacker, Digimon enemy)
    {
        int baseExp = enemy.data.baseExpReward;

        int diff = enemy.level.Level - attacker.level.Level;

        float modifier = 1f;

        if (diff >= 5)
            modifier = 1.5f;
        else if (diff >= 2)
            modifier = 1.2f;
        else if (diff <= -5)
            modifier = 0.2f;
        else if (diff <= -2)
            modifier = 0.5f;

        return Mathf.RoundToInt(baseExp * modifier);
    }
}
