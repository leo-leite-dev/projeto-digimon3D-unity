using UnityEngine;

public class CombatTestSimulator : MonoBehaviour
{
    public Digimon attacker;
    public Digimon defender;

    public AttackType attackType = AttackType.Physical;

    public EnemySpawner enemySpawner;

    void Awake()
    {
        if (enemySpawner != null)
            enemySpawner.OnEnemySpawned += HandleEnemySpawned;
    }

    void HandleEnemySpawned(DigimonEnemy enemy)
    {
        defender = enemy;
    }

    [ContextMenu("Simular 100 Combates")]
    public void RunSimulation()
    {
        if (attacker == null || defender == null)
        {
            Debug.LogWarning("Attacker ou Defender não definidos!");
            return;
        }

        int simulations = 100;

        int minDamage = int.MaxValue;
        int maxDamage = int.MinValue;
        int totalDamage = 0;
        int missCount = 0;

        for (int i = 0; i < simulations; i++)
        {
            int damage = CombatCalculator.CalculateDamage(attacker, defender, attackType);

            if (damage == 0)
            {
                missCount++;
                continue;
            }

            if (damage < minDamage)
                minDamage = damage;

            if (damage > maxDamage)
                maxDamage = damage;

            totalDamage += damage;
        }

        float average = (float)totalDamage / (simulations - missCount);

        float typeMod = CalculatorMatrix.GetTypeModifier(attacker.Type, defender.Type);
        float elementMod = CalculatorMatrix.GetElementModifier(attacker.Element, defender.Element);

        string typeRelation =
            typeMod > 1f ? "VANTAGEM"
            : typeMod < 1f ? "DESVANTAGEM"
            : "NEUTRO";

        string elementRelation =
            elementMod > 1f ? "VANTAGEM"
            : elementMod < 1f ? "DESVANTAGEM"
            : "NEUTRO";

        int xpGained = defender.data.baseExpReward;

        attacker.AddExperience(xpGained);

        Debug.Log(
            "========== RESULTADO DA SIMULAÇÃO ==========\n"
                + $"Atacante: {attacker.Name} | Level: {attacker.Level} | Tipo: {attacker.Type} | Elemento: {attacker.Element}\n"
                + $"Defensor: {defender.Name} | Level: {defender.Level} | Tipo: {defender.Type} | Elemento: {defender.Element}\n\n"
                + $"Tipo de Ataque: {attackType}\n\n"
                + $"ATK: {attacker.stats.PhysicalAttack} | DEF: {defender.stats.PhysicalDefense}\n\n"
                + $"RELACAO DE TIPO: {typeRelation} (x{typeMod})\n"
                + $"RELACAO ELEMENTAL: {elementRelation} (x{elementMod})\n\n"
                + $"Combates simulados: {simulations}\n"
                + $"MISS: {missCount}\n"
                + $"Dano mínimo: {minDamage}\n"
                + $"Dano máximo: {maxDamage}\n"
                + $"Dano médio: {average}\n"
                + "=============================================\n\n"
                + $"XP GAIN\n"
                + $"{attacker.Name} ganhou {xpGained} XP\n"
                + $"Level atual: {attacker.Level}\n"
                + $"XP atual: {attacker.level.Experience}/{attacker.level.ExpToNextLevel}"
        );
    }
}
