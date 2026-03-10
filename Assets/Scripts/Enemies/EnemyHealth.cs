using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 10;

    private int currentHealth;

    public bool IsDead => currentHealth <= 0;

    public Digimon lastAttacker;
    public static event Action<DigimonEnemy> OnEnemyKilled;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, Digimon attacker)
    {
        lastAttacker = attacker;

        currentHealth -= damage;

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (lastAttacker != null)
        {
            Digimon enemyDigimon = GetComponent<Digimon>();

            if (enemyDigimon != null)
            {
                int xp = enemyDigimon.data.baseExpReward;
                lastAttacker.AddExperience(xp);

                Debug.Log($"{lastAttacker.Name} ganhou {xp} XP por derrotar {enemyDigimon.Name}");
            }
        }

        EnemyWander wander = GetComponent<EnemyWander>();

        if (wander != null && wander.spawner != null)
            wander.spawner.NotifyEnemyDeath(gameObject);

        Destroy(gameObject);
    }
}
