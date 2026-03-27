public static class CombatContextData
{
    public static DigimonData SelectedEnemy;

    public static TamerStatsConfig TamerStats;
    public static DigimonData PlayerDigimon;

    public static void Clear()
    {
        SelectedEnemy = null;
        TamerStats = null;
        PlayerDigimon = null;
    }
}
