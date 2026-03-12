using UnityEngine;

public static class CombatCalculator
{
    const float CRIT_MULTIPLIER = 1.5f;

    public static bool DidHit(Digimon attacker, Digimon defender)
    {
        float hitChance = 1f - defender.stats.Evasion;

        int levelDiff = defender.Level - attacker.Level;

        if (levelDiff > 0)
            hitChance -= levelDiff * 0.02f;

        float typeMod = CalculatorMatrix.GetTypeModifier(attacker.Type, defender.Type);
        float elementMod = CalculatorMatrix.GetElementModifier(attacker.Element, defender.Element);

        if (typeMod < 1f)
            hitChance -= 0.40f;

        if (elementMod < 1f)
            hitChance -= 0.30f;

        hitChance = Mathf.Clamp(hitChance, 0.01f, 0.95f);

        return Random.value <= hitChance;
    }

    public static int CalculateDamage(Digimon attacker, Digimon defender, DigimonSkill skill)
    {
        float attack;
        float defense;

        if (skill.attackType == AttackType.Magic)
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

        float skillPower = skill.damage;
        float baseDamage = attack + skillPower;

        float typeMod = CalculatorMatrix.GetTypeModifier(attacker.Type, defender.Type);
        float elementMod = CalculatorMatrix.GetElementModifier(attacker.Element, defender.Element);

        float damageAfterMultipliers = baseDamage * typeMod * elementMod;

        float damageAfterDefense = damageAfterMultipliers * (100f / (100f + defense));

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

        float skillRatio = skillPower / baseDamage;
        float skillContribution = finalDamage * skillRatio;
        float attackContribution = finalDamage - skillContribution;

        string typeRelation =
            typeMod > 1f ? "VANTAGEM"
            : typeMod < 1f ? "DESVANTAGEM"
            : "NEUTRO";

        string elementRelation =
            elementMod > 1f ? "VANTAGEM"
            : elementMod < 1f ? "DESVANTAGEM"
            : "NEUTRO";

        Debug.Log(
            "========== COMBAT LOG ==========\n"
                + $"Atacante: {attacker.Name} | Tipo: {attacker.Type} | Elemento: {attacker.Element}\n"
                + $"Defensor: {defender.Name} | Tipo: {defender.Type} | Elemento: {defender.Element}\n\n"
                + $"Skill utilizada: {skill.skillName}\n"
                + $"Tipo de ataque: {skill.attackType}\n\n"
                + $"Ataque base do Digimon: {attack}\n"
                + $"Poder da skill: {skillPower}\n"
                + $"Ataque total (ATK + Skill): {baseDamage}\n"
                + $"Defesa utilizada: {defense}\n\n"
                + $"RELACAO DE TIPO: {typeRelation} (x{typeMod})\n"
                + $"RELACAO ELEMENTAL: {elementRelation} (x{elementMod})\n\n"
                + $"Dano base: {baseDamage}\n"
                + $"Após multiplicadores: {damageAfterMultipliers}\n"
                + $"Após defesa: {damageAfterDefense}\n\n"
                + $"Crítico ocorreu?: {crit}\n"
                + $"Random modifier: x{randomModifier}\n\n"
                + $"DANO FINAL: {finalDamage}\n\n"
                + $"Contribuição do ATK no dano final: {attackContribution:F2}\n"
                + $"Contribuição da Skill no dano final: {skillContribution:F2}\n"
                + "==============================="
        );

        return finalDamage;
    }
}
