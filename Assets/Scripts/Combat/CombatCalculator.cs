using UnityEngine;

public static class CombatCalculator
{
    const float CRIT_MULTIPLIER = 1.5f;

    public static bool DidHit(Digimon attacker, Digimon defender)
    {
        float hitChance = 1f - defender.stats.Evasion;

        int levelDiff = defender.Level - attacker.Level;

        if (levelDiff > 0)
        {
            float levelPenalty = levelDiff * 0.02f;
            hitChance -= levelPenalty;
        }

        float typeMod = CalculatorMatrix.GetTypeModifier(attacker.Type, defender.Type);
        float elementMod = CalculatorMatrix.GetElementModifier(attacker.Element, defender.Element);

        if (typeMod < 1f)
            hitChance -= 0.40f;

        if (elementMod < 1f)
            hitChance -= 0.30f;

        hitChance = Mathf.Clamp(hitChance, 0.01f, 0.95f);

        float roll = Random.value;

        bool hit = roll <= hitChance;

        return hit;
    }

    public static float GetEvasionModifier(int attackerLevel, int defenderLevel)
    {
        int diff = defenderLevel - attackerLevel;

        if (diff >= 20)
            return 0.9f;
        if (diff >= 10)
            return 0.5f;
        if (diff >= 5)
            return 0.25f;

        return 0f;
    }

    public static int CalculateDamage(Digimon attacker, Digimon defender, AttackType attackType)
    {
        float attack;
        float defense;

        if (attackType == AttackType.Magic)
        {
            attack = attacker.stats.MagicAttack;
            defense = defender.stats.MagicDefense;
        }
        else
        {
            attack = attacker.stats.PhysicalAttack;
            defense = defender.stats.PhysicalDefense;
        }

        if (!DidHit(attacker, defender))
        {
            Debug.Log($"{attacker.Name} errou o ataque!");
            return 0;
        }

        float baseDamage = attack;

        float typeMod = CalculatorMatrix.GetTypeModifier(attacker.Type, defender.Type);
        float elementMod = CalculatorMatrix.GetElementModifier(attacker.Element, defender.Element);

        float damageWithoutDefense = baseDamage * typeMod * elementMod;

        float damageAfterDefense = damageWithoutDefense * (100f / (100f + defense));

        bool crit = false;

        float damage = damageAfterDefense;

        if (Random.value < attacker.stats.CritChance)
        {
            damage *= attacker.stats.CritDamage;
            crit = true;
        }

        float randomModifier = Random.Range(0.9f, 1.1f);
        damage *= randomModifier;

        int finalDamage = Mathf.Max(1, Mathf.RoundToInt(damage));

        // string typeRelation =
        //     typeMod > 1f ? "VANTAGEM"
        //     : typeMod < 1f ? "DESVANTAGEM"
        //     : "NEUTRO";

        // string elementRelation =
        //     elementMod > 1f ? "VANTAGEM"
        //     : elementMod < 1f ? "DESVANTAGEM"
        //     : "NEUTRO";

        // Debug.Log(
        //     "========== COMBAT LOG ==========\n"
        //         + $"Atacante: {attacker.Name}  | Tipo: {attacker.Type} | Elemento: {attacker.Element}\n"
        //         + $"Defensor: {defender.Name} | Tipo: {defender.Type} | Elemento: {defender.Element}\n\n"
        //         + $"Tipo de ataque: {attackType}\n\n"
        //         + $"Ataque utilizado: {attack}\n"
        //         + $"Defesa utilizada: {defense}\n\n"
        //         + $"RELACAO DE TIPO: {typeRelation} (x{typeMod})\n"
        //         + $"RELACAO ELEMENTAL: {elementRelation} (x{elementMod})\n\n"
        //         + $"Dano base (sem modificadores): {baseDamage}\n"
        //         + $"Dano após vantagem/desvantagem: {damageWithoutDefense}\n"
        //         + $"Dano após defesa: {damageAfterDefense}\n\n"
        //         + $"Critico ocorreu?: {crit}\n"
        //         + $"Random modifier: x{randomModifier}\n\n"
        //         + $"DANO FINAL: {finalDamage}\n"
        //         + "==============================="
        // );

        return finalDamage;
    }
}
