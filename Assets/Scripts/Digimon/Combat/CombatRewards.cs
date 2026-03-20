using UnityEngine;

public static class CombatRewards
{
    public static void GiveKillXP(Digimon attacker, Digimon enemy)
    {
        if (attacker == null || enemy == null)
            return;

        int xp = ExperienceCalculator.CalculateExpGain(attacker, enemy);

        attacker.AddExperience(xp);

        Debug.Log(
            "========== XP REWARD ==========\n"
                + $"{attacker.Name} derrotou {enemy.Name}\n"
                + $"XP ganho: {xp}\n"
                + $"Level atual: {attacker.Level}\n"
                + $"XP atual: {attacker.level.Experience}/{attacker.level.ExpToNextLevel}"
        );
    }
}
