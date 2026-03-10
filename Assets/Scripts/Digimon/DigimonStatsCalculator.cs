public static class DigimonStatsCalculator
{
    public static void CalculateStats(DigimonAttributes attributes, DigimonStats stats)
    {
        stats.Hp = attributes.Vitality * 10;

        stats.PhysicalAttack = attributes.Strength * 2;
        stats.MagicAttack = attributes.Intelligence * 2;

        stats.PhysicalDefense = attributes.Vitality;
        stats.MagicDefense = attributes.Spirit * 2;

        stats.Speed = attributes.Agility * 2;

        stats.CritChance = attributes.Agility * 0.01f;
        stats.CritDamage = 1.5f;

        stats.Evasion = attributes.Agility * 0.02f;
    }
}
