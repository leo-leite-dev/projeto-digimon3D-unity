using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public int maxHealth = 10;

    private int currentHealth;

    public bool IsDead => currentHealth <= 0;

    private Digimon lastAttacker;

    public static event Action<DigimonEnemy> OnEnemyKilled;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, Digimon attacker)
    {
        if (IsDead)
            return;

        lastAttacker = attacker;

        currentHealth -= damage;

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        GiveExperience();

        NotifySpawner();

        Destroy(gameObject);
    }

    void GiveExperience()
    {
        if (lastAttacker == null)
            return;

        Digimon enemyDigimon = GetComponent<Digimon>();

        if (enemyDigimon == null)
            return;

        int xp = ExperienceCalculator.CalculateExpGain(lastAttacker, enemyDigimon);

        lastAttacker.AddExperience(xp);

        Debug.Log(
            $"========== ENEMY DEFEATED ==========\n"
                + $"{lastAttacker.Name} derrotou {enemyDigimon.Name}\n"
                + $"Level Attacker: {lastAttacker.Level}\n"
                + $"Level Enemy: {enemyDigimon.Level}\n"
                + $"XP ganho: {xp}"
        );
    }

    void NotifySpawner()
    {
        EnemyWander wander = GetComponent<EnemyWander>();

        if (wander != null && wander.spawner != null)
            wander.spawner.NotifyEnemyDeath(gameObject);
    }
}
