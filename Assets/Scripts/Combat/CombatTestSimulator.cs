using UnityEngine;

public class CombatTestSimulator : MonoBehaviour
{
    public Digimon attacker;
    public Digimon defender;
    public DigimonSkill skill;

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
        if (attacker == null || defender == null || skill == null)
        {
            Debug.LogWarning("Attacker, Defender ou Skill não definidos!");
            return;
        }

        int simulations = 100;

        int minDamage = int.MaxValue;
        int maxDamage = int.MinValue;
        int totalDamage = 0;
        int missCount = 0;

        for (int i = 0; i < simulations; i++)
        {
            int damage = CombatCalculator.CalculateDamage(attacker, defender, skill);

            if (damage == 0)
            {
                missCount++;
                continue;
            }

            minDamage = Mathf.Min(minDamage, damage);
            maxDamage = Mathf.Max(maxDamage, damage);

            totalDamage += damage;
        }

        float average = (float)totalDamage / (simulations - missCount);

        Debug.Log(
            "========== RESULTADO DA SIMULAÇÃO ==========\n"
                + $"Skill testada: {skill.skillName}\n\n"
                + $"Combates simulados: {simulations}\n"
                + $"MISS: {missCount}\n"
                + $"Dano mínimo: {minDamage}\n"
                + $"Dano máximo: {maxDamage}\n"
                + $"Dano médio: {average}\n"
                + "============================================="
        );
    }
}
