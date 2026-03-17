using UnityEngine;

public static class ExperienceCalculator
{
    public static ExperienceBalance balance;

    public static int GetExpToNextLevel(int level)
    {
        return Mathf.RoundToInt(50 * Mathf.Pow(level, 1.4f));
    }

    public static int CalculateExpGain(Digimon attacker, Digimon enemy)
    {
        int baseExp = enemy.Data.baseExpReward;

        int diff = enemy.Level - attacker.Level;

        float modifier;

        if (diff >= 0)
        {
            int index = diff;

            if (index >= balance.xpBonusTable.Length)
                index = balance.xpBonusTable.Length - 1;

            modifier = balance.xpBonusTable[index];
        }
        else
        {
            int index = Mathf.Abs(diff);

            if (index >= balance.xpPenaltyTable.Length)
                index = balance.xpPenaltyTable.Length - 1;

            modifier = balance.xpPenaltyTable[index];
        }

        return Mathf.RoundToInt(baseExp * modifier);
    }
}
